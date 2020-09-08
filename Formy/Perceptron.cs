using Accord.Controls;
using Deedle;
using Microsoft.ML;
using SpamSmsLicencjat.Klasy_pomocnicze;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SpamSmsLicencjat.Formy
{
    public partial class Perceptron : Form
    {

        private static string dataPath = Path.Combine(Environment.CurrentDirectory, "dane.tsv");
        private static string stopWordsDataPath = Path.Combine(Environment.CurrentDirectory, "stopwords.txt");
        private static string dataPathCsv = Path.Combine(Environment.CurrentDirectory, "dane.csv");

        PredictionEngine<SpamInputAveragedPerceptron, SpamPredictionAveragedPerceptron> predictionEngine;
        private bool _loading = true;

        public Perceptron()
        {
            InitializeComponent();
            ChangeVisibility();
            labelLoading.Visible = false;
            buttonLoad.Visible = true;
        }

        private void StartModel()
        {

            //machine learning kontekst
            var context = new MLContext();

            //dane z pliku mapowane na SpamInput
            var data = context.Data.LoadFromTextFile<SpamInputAveragedPerceptron>(
                path: dataPath,
                hasHeader: true,
                separatorChar: '\t');

            //użycie 80%  danych na trening i 20% do testowania
            var partitions = context.Data.TrainTestSplit(
                data,
                testFraction: 0.2);

            // transofrmacja tekstu
            var dataProcessPipeline = context.Transforms.Conversion.MapValueToKey("Label", "Label")
                .Append(context.Transforms.Text.FeaturizeText("FeaturesText",
                    new Microsoft.ML.Transforms.Text.TextFeaturizingEstimator.Options
                    {
                        WordFeatureExtractor = new Microsoft.ML.Transforms.Text.WordBagEstimator.Options
                        { NgramLength = 2, UseAllLengths = true },
                        CharFeatureExtractor = new Microsoft.ML.Transforms.Text.WordBagEstimator.Options
                        { NgramLength = 3, UseAllLengths = false },
                    }, "Message"))
                .Append(context.Transforms.CopyColumns("Features", "FeaturesText"))
                .Append(context.Transforms.NormalizeLpNorm("Features", "Features"))
                .AppendCacheCheckpoint(context);

            // ustaw aalgorytm
            var trainer = context.MulticlassClassification.Trainers.OneVersusAll(
                    context.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "Label",
                        numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "Label")
                .Append(context.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
            var trainingPipeLine = dataProcessPipeline.Append(trainer);

            var crossValidationResults =
                context.MulticlassClassification.CrossValidate(data: data, estimator: trainingPipeLine,
                    numberOfFolds: 5);

            StringBuilder sb = new StringBuilder();

            //raport wyniku - sprawdzamy czy nie ma zbyt wielu szczegołów (ale jest ok , bo wcześniej wyknaliśmy NLP w kroku 2
            foreach (var r in crossValidationResults)
            {
                sb.Append(
                    $"  Średni AUC - czyli jak model jest dokładny (skala mikro): {crossValidationResults.Average(r => r.Metrics.MicroAccuracy)} \n");
                sb.Append(
                    $"  Średni AUC - czyli jak model jest dokładny (skala makro): {crossValidationResults.Average(r => r.Metrics.MacroAccuracy)} \n");
            }

            //trenujemy model na zbiorze treningowym
            Console.WriteLine("Trenowanie modelu ...");
            var model = trainingPipeLine.Fit(partitions.TrainSet);

            // ocena modelu na zbiorze testowym
            Console.WriteLine("Ocena modelu ....");
            var predictions = model.Transform(partitions.TestSet);
            var metrics = context.MulticlassClassification.Evaluate(
                data: predictions,
                labelColumnName: "Label",
                scoreColumnName: "Score");

            // raport wynik
            sb.Append($"  Dokładność makro:          {metrics.MacroAccuracy:P2} \n");
            sb.Append($"  Dokładność mikro:          {metrics.MicroAccuracy:P2} \n");
            sb.Append($"  Jak dużo błędów (0 = brak , 1 = same błędy:           {metrics.LogLoss:0.##} \n");
            sb.Append($"  LogLossReduction:  {metrics.LogLossReduction:0.##} \n");
            sb.Append(metrics.ConfusionMatrix.GetFormattedConfusionTable());


            PredictionEngine<SpamInputAveragedPerceptron, SpamPredictionAveragedPerceptron> predictionEngine =
                context.Model.CreatePredictionEngine<SpamInputAveragedPerceptron, SpamPredictionAveragedPerceptron>(
                    model);

            textBoxLog.Text = sb.ToString();

            _loading = false;
            ChangeVisibility();
        }

        private void Start()
        {
            _loading = true;
            ChangeVisibility();
            MessageBox.Show("Trwa ładowanie", "Ładowanie");

            StartModel();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void buttonCheckSpam_Click(object sender, EventArgs e)
        {
            var inputUser = textBoxCheckSpam.Text;

            var message = new SpamInputAveragedPerceptron();
            message.Message = inputUser;


            // utworzenie predykcji
            var prediction = predictionEngine.Predict(message);

            // pokazanie wyniku
            MessageBox.Show($"Wiadomość '{inputUser}' jest { (prediction.isSpam == "spam" ? "spam" : "not spam" )}", "Wynik");
            textBoxCheckSpam.Clear();
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

            var hamSmsCount = przetworzoneDaneZPlikuCsv.GetColumn<int>("czy_ham").NumSum();
            var spamSmsCount = wiadomoscWordVecDf.RowCount - hamSmsCount;

            wiadomoscWordVecDf.AddColumn("czy_ham", przetworzoneDaneZPlikuCsv.GetColumn<int>("czy_ham"));
            var hamTermFrequencies = wiadomoscWordVecDf.Where(
                x => x.Value.GetAs<int>("czy_ham") == 1
            ).Sum().Sort().Reversed.Where(x => x.Key != "czy_ham");

            var spamTermFrequencies = wiadomoscWordVecDf.Where(
                x => x.Value.GetAs<int>("czy_ham") == 0
            ).Sum().Sort().Reversed;

            var hamTermProportions = hamTermFrequencies / hamSmsCount;
            var topHamTerms = hamTermProportions.Keys.Take(10);
            var topHamTermsProportions = hamTermProportions.Values.Take(10);

            File.WriteAllLines(
               Path.Combine(Environment.CurrentDirectory, "ham-czestotliwosc.csv"),
                hamTermFrequencies.Keys.Zip(
                    hamTermFrequencies.Values, (a, b) => string.Format("{0},{1}", a, b)
                )
            );

            var spamTermProportions = spamTermFrequencies / spamSmsCount;
            var topSpamTerms = spamTermProportions.Keys.Take(10);
            var topSpamTermsProportions = spamTermProportions.Values.Take(10);

            File.WriteAllLines(
                Path.Combine(Environment.CurrentDirectory, "spam-czestotliwosc.csv"),
                spamTermFrequencies.Keys.Zip(
                    spamTermFrequencies.Values, (a, b) => string.Format("{0},{1}", a, b)
                )
            );

            var barChart = DataBarBox.Show(
                new string[] { "Ham", "Spam" },
                new double[] {
                    hamSmsCount,
                    spamSmsCount
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
    }
}
