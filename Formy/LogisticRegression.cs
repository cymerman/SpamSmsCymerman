using Deedle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.ML;
using Accord.Controls;
using SpamSmsLicencjat.Klasy_pomocnicze;

namespace SpamSmsLicencjat
{
    public partial class LogisticRegression : Form
    {
        private static string dataPath = Path.Combine(Environment.CurrentDirectory, "dane.tsv");
        private static string stopWordsDataPath = Path.Combine(Environment.CurrentDirectory, "stopwords.txt");
        private static string dataPathCsv = Path.Combine(Environment.CurrentDirectory, "dane.csv");


        PredictionEngine<SpamInput, SpamPrediction> predictionEngine;
        private bool _loading = true;


        public LogisticRegression()
        {
            InitializeComponent();
            ChangeVisibility();
            labelLoading.Visible = false;
            buttonLoad.Visible = true;
        }

        private void Start()
        {
            _loading = true;
            ChangeVisibility();
            MessageBox.Show("Trwa ładowanie", "Ładowanie");

            StartModel();
        }

        private void StartModel()
        {
            global::LogisticRegression.LogisticRegression log = new global::LogisticRegression.LogisticRegression();

            var cvResults = log.cvResults;
            var metrics = log.metrics;
            predictionEngine = log.predictionEngine;


            StringBuilder sb = new StringBuilder();
            //raport wyniku - sprawdzamy czy nie ma zbyt wielu szczegołów (ale jest ok , bo wcześniej wyknaliśmy NLP w kroku 2
            foreach (var r in cvResults)
            {
                sb.Append(
                    $"  Średni AUC - czyli jak model jest dokładny: {cvResults.Average(x => x.Metrics.AreaUnderRocCurve)} \n");
            }

            // raport wynik


            sb.Append($"  Dokładność:          {metrics.Accuracy:P2} \n");
            sb.Append(
                $"  Dokładność modelu (AUC) (1 = model zgaduje cały czas, 0 = myli się cały czas , 0.5 = losowe wyniki:               {metrics.AreaUnderRocCurve:P2} \n");
            sb.Append(
                $"  AUCPRC - metryka dla bardzo niezbalansowanych zbiorów danych która lepiej sobie radzi gdy jest więcej negatywnych wyników:             {metrics.AreaUnderPrecisionRecallCurve:P2} \n");
            sb.Append(
                $"  F1Score - balans pomiędzy dokładnością,a odrzuceniu. Bardzo przydatne przy niezbalansowanych zbiorach danych:           {metrics.F1Score:P2} \n");
            sb.Append($"  Jak dużo błędów (0 = brak , 1 = same błędy:           {metrics.LogLoss:0.##} \n");
            sb.Append($"  Ile pozytywnych:  {metrics.LogLossReduction:0.##} \n");
            sb.Append($"  Pozytywne precyzja: {metrics.PositivePrecision:0.##} \n");
            sb.Append($"  Odrzut pozytywnych:    {metrics.PositiveRecall:0.##} \n");
            sb.Append($"  Negatywne precyzja: {metrics.NegativePrecision:0.##} \n");
            sb.Append($"  Odrzut negatywnych:    {metrics.NegativeRecall:0.##} \n");

            textBoxLog.Text = sb.ToString();

            Console.WriteLine(sb.ToString());

            _loading = false;
            ChangeVisibility();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void ChangeVisibility()
        {
            if (_loading)
            {
                labelLoading.Visible = true;

                buttonCheckSpam.Visible = false;
                textBoxCheckSpam.Visible = false;
                buttonShowDiagrams.Visible = false;
                buttonLoad.Visible = false;

                labelWriteMessage.Visible = false;
                labelLog.Visible = false;
                textBoxLog.Visible = false;
            }
            else
            {
                labelLoading.Visible = false;

                buttonCheckSpam.Visible = true;
                textBoxCheckSpam.Visible = true;
                buttonShowDiagrams.Visible = true;

                buttonLoad.Visible = false;

                labelWriteMessage.Visible = true;
                labelLog.Visible = true;
                textBoxLog.Visible = true;
            }
        }

        private void buttonCheckSpam_Click(object sender, EventArgs e)
        {
            var inputUser = textBoxCheckSpam.Text;

            var message = new SpamInput();
            message.Message = inputUser;


            // utworzenie predykcji
            var prediction = predictionEngine.Predict(message);

            // pokazanie wyniku
            MessageBox.Show($"wiadomość jest na { prediction.Probability:P2} - { IsSpamToString(prediction.IsSpam) } przetestowana twoja wiadomość:  { inputUser}", "Wynik");
            textBoxCheckSpam.Clear();
          }

        private static string IsSpamToString(bool isSpam)
        {
            if (isSpam)
                return "spamem";
            else
                return "hamem";
        }

        private void buttonShowDiagrams_Click(object sender, EventArgs e)
        {
            _loading = true;
            ChangeVisibility();
            MessageBox.Show("Trwa ładowanie", "Ładowanie");

            // wczytanie danych z pliku csv
            var przetworzoneDaneZPlikuCsv = Frame.ReadCsv(
                dataPathCsv,
                hasHeaders: true,
                inferTypes: false,
                schema: "int,string"
            );

            // sprawdzenie slow 
            var wiadomoscWordVecDf = MakeWordVec(przetworzoneDaneZPlikuCsv.GetColumn<string>("wiadomosc"));
            wiadomoscWordVecDf.SaveCsv(Path.Combine(Environment.CurrentDirectory, "tekst_wordvec_tylko_litery.csv"));
            
            // wczytanie stopwordsow z pliku tekstowego
            ISet<string> stopWords = new HashSet<string>(
                File.ReadLines(stopWordsDataPath)
            );

            var spamSmsCount = przetworzoneDaneZPlikuCsv.GetColumn<int>("czy_ham").NumSum();
            var hamSmsCount = wiadomoscWordVecDf.RowCount - spamSmsCount;

            wiadomoscWordVecDf.AddColumn("czy_ham", przetworzoneDaneZPlikuCsv.GetColumn<int>("czy_ham"));
            var hamTermFrequencies = wiadomoscWordVecDf.Where(
                x => x.Value.GetAs<int>("czy_ham") == 0
            ).Sum().Sort().Reversed.Where(x => x.Key != "czy_ham");

            var spamTermFrequencies = wiadomoscWordVecDf.Where(
                x => x.Value.GetAs<int>("czy_ham") == 1
            ).Sum().Sort().Reversed;

            var hamTermProportions = hamTermFrequencies / hamSmsCount;
            var topHamTerms = hamTermProportions.Keys.Take(10);
            var topHamTermsProportions = hamTermProportions.Values.Take(10);

            File.WriteAllLines(
               Path.Combine(Environment.CurrentDirectory, "ham-czestotliwosc.csv"),
                hamTermFrequencies.Keys.Zip(
                    hamTermFrequencies.Values, (a, b) => string.Format($"{a},{b}")
                )
            );

            var spamTermProportions = spamTermFrequencies / spamSmsCount;
            var topSpamTerms = spamTermProportions.Keys.Take(10);
            var topSpamTermsProportions = spamTermProportions.Values.Take(10);

            File.WriteAllLines(
                Path.Combine(Environment.CurrentDirectory, "spam-czestotliwosc.csv"),
                spamTermFrequencies.Keys.Zip(
                    spamTermFrequencies.Values, (a, b) => string.Format($"{a},{b}")
                )
            );

            var barChart = DataBarBox.Show(
                new string[] {"Spam" , "Ham" },
                new double[] {
                    spamSmsCount,
                    hamSmsCount
                }
            );
            barChart.SetTitle("Ham vs. Spam ogólny rozkład");

            var hamBarChart = DataBarBox.Show(
                topHamTerms.ToArray(),
                new double[][] {
                    topHamTermsProportions.ToArray(),
                    spamTermProportions.GetItems(topHamTerms).Values.ToArray()
                }
            );
            hamBarChart.AutoScaleMode = AutoScaleMode.Inherit;
            hamBarChart.SetTitle("Top używanych 10 słów w Ham Smsach przed filtrowaniem stopwordsow (niebieski: HAM, czerwony: SPAM)");
            System.Threading.Thread.Sleep(3000);
            hamBarChart.Invoke(
                new Action(() =>
                {
                    hamBarChart.Size = new Size(2000, 1500);
                })
            );

            var spamBarChart = DataBarBox.Show(
                topSpamTerms.ToArray(),
                new double[][] {
                    hamTermProportions.GetItems(topSpamTerms).Values.ToArray(),
                    topSpamTermsProportions.ToArray()
                }
            );

            spamBarChart.AutoScaleMode = AutoScaleMode.Inherit;
            spamBarChart.SetTitle("Top 10 używanych słów w Spam Smsach przed filtrowaniem stopwordsów(niebieskie: HAM, czerwony: SPAM)");
            System.Threading.Thread.Sleep(2000);
            spamBarChart.Invoke(
                new Action(() =>
                {
                    spamBarChart.Size = new Size(2000, 1500);
                })
            );


            var hamTermFrequenciesAfterStopWords = hamTermFrequencies.Where(
                x => !stopWords.Contains(x.Key)
            );
            var hamTermProportionsAfterStopWords = hamTermProportions.Where(
                x => !stopWords.Contains(x.Key)
            );
            var topHamTermsAfterStopWords = hamTermProportionsAfterStopWords.Keys.Take(10);
            var topHamTermsProportionsAfterStopWords = hamTermProportionsAfterStopWords.Values.Take(10);
            File.WriteAllLines(
                Path.Combine(Environment.CurrentDirectory, "ham-czestotliwosc-po-przefiltrowaniu-stopwordsow.csv"),
                hamTermFrequenciesAfterStopWords.Keys.Zip(
                    hamTermFrequenciesAfterStopWords.Values, (a, b) => string.Format($"{a},{b}")
                )
            );

            var spamTermFrequenciesAfterStopWords = spamTermFrequencies.Where(
                x => !stopWords.Contains(x.Key)
            );
            var spamTermProportionsAfterStopWords = spamTermProportions.Where(
                x => !stopWords.Contains(x.Key)
            );
            var topSpamTermsAfterStopWords = spamTermProportionsAfterStopWords.Keys.Take(10);
            var topSpamTermsProportionsAfterStopWords = spamTermProportionsAfterStopWords.Values.Take(10);
            File.WriteAllLines(
                 Path.Combine(Environment.CurrentDirectory, "spam-czestotliwosc-po-przefiltrowaniu-stopwordsow.csv"),
                spamTermFrequenciesAfterStopWords.Keys.Zip(
                    spamTermFrequenciesAfterStopWords.Values, (a, b) => string.Format($"{a},{b}")
                )
            );

            hamBarChart = DataBarBox.Show(
                topHamTermsAfterStopWords.ToArray(),
                new double[][] {
                    topHamTermsProportionsAfterStopWords.ToArray(),
                    spamTermProportionsAfterStopWords.GetItems(topHamTermsAfterStopWords).Values.ToArray()
                }
            );
            hamBarChart.SetTitle("Top 10 słów w Ham smsach - po filtrowaniu stopwordsów (niebieski: HAM, czerwony: SPAM)");
            hamBarChart.AutoScaleMode = AutoScaleMode.Inherit;
            System.Threading.Thread.Sleep(3000);

            hamBarChart.Invoke(
                new Action(() =>
                {
                    hamBarChart.Size = new Size(2000, 1500);
                })
            );

            spamBarChart = DataBarBox.Show(
                topSpamTermsAfterStopWords.ToArray(),
                new double[][] {
                    hamTermProportionsAfterStopWords.GetItems(topSpamTermsAfterStopWords).Values.ToArray(),
                    topSpamTermsProportionsAfterStopWords.ToArray()
                }
            );
            spamBarChart.SetTitle("Top 10 słów w Spam smsach - po wyfiltrowaniu stopwordsow (niebieski: HAM, czerwony: SPAM)");
            spamBarChart.AutoScaleMode = AutoScaleMode.Inherit;
            System.Threading.Thread.Sleep(3000);

            spamBarChart.Invoke(
                new Action(() =>
                {
                    spamBarChart.Size = new Size(2000, 1500);
                })
            );

            _loading = false;
            ChangeVisibility();
        }

        private static Frame<int, string> MakeWordVec(Series<int, string> rows)
        {
            var wordsByRows = rows.GetAllValues().Select((x, i) =>
            {
                var sb = new SeriesBuilder<string, int>();

                ISet<string> words = new HashSet<string>(
                    Regex.Matches(
                        // same litery
                        x.Value, "[a-zA-Z]+('(s|d|t|ve|m))?"
                    ).Cast<Match>().Select(
                        // zamiana na male litery
                        y => y.Value.ToLower()
                    ).ToArray()
                );

                // zamiana slowa ktore wystepuja w kazdym wierszu na 1
                foreach (string w in words)
                {
                    sb.Add(w, 1);
                }

                return KeyValue.Create(i, sb.Series);
            });

            // brakujace zamieniam na 0
            var wordVecDF = Frame.FromRows(wordsByRows).FillMissing(0);

            return wordVecDF;
        }
    }
}

