using Accord;
using Accord.Controls;
using Accord.MachineLearning;
using Accord.MachineLearning.Bayes;
using Accord.Math.Optimization.Losses;
using Accord.Statistics.Analysis;
using Accord.Statistics.Distributions.Univariate;
using Deedle;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SpamSmsLicencjat
{
    public partial class NaiveBayesForm : Form
    {
        private static string stopWordsDataPath = Path.Combine(Environment.CurrentDirectory, "stopwords.txt");
        private static string dataPath = Path.Combine(Environment.CurrentDirectory, "dane.csv");

        private Frame<int, string> rawDf;
        private Frame<int, string> wiadomoscWordVecDf;

        private ISet<string> stopWords;

        private bool _loading = true;

        public NaiveBayesForm()
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
            // wczytanie danych z pliku csv
            rawDf = Frame.ReadCsv(
                dataPath,
                hasHeaders: true,
                inferTypes: false,
                schema: "int,string"

            );
            //
            // wczytanie stopwordsow z pliku tekstowego
            stopWords = new HashSet<string>(
                File.ReadLines(stopWordsDataPath)
            );
            
            // sprawdzenie slow 
            wiadomoscWordVecDf = MakeWordVec(rawDf.GetColumn<string>("wiadomosc"));
            wiadomoscWordVecDf.SaveCsv(Path.Combine(Environment.CurrentDirectory, "tekst_wordvec_tylko_litery.csv"));
            
            wiadomoscWordVecDf.AddColumn("czy_ham", rawDf.GetColumn<int>("czy_ham"));
            
            var spamTermFrequencies = wiadomoscWordVecDf.Where(
                x => x.Value.GetAs<int>("czy_ham") == 0
            ).Sum().Sort().Reversed;
            
            var spamTermFrequenciesAfterStopWords = spamTermFrequencies.Where(
                x => !stopWords.Contains(x.Key)
            );
            
            File.WriteAllLines(
                Path.Combine(Environment.CurrentDirectory, "spam-czestotliwosc-po-przefiltrowaniu-stopwordsow.csv"),
                spamTermFrequenciesAfterStopWords.Keys.Zip(
                    spamTermFrequenciesAfterStopWords.Values, (a, b) => string.Format($"{a},{b}")
                )
            );

            // Load Term Frequency Data
            var spamTermFrequencyDF = Frame.ReadCsv(
                Path.Combine(Environment.CurrentDirectory, "spam-czestotliwosc-po-przefiltrowaniu-stopwordsow.csv"),
                hasHeaders: false,
                inferTypes: false,
                schema: "string,int"
            );

            spamTermFrequencyDF.RenameColumns(new string[] { "word", "num_occurences" });
            var indexedSpamTermFrequencyDF = spamTermFrequencyDF.IndexRows<string>("word");

            // słowo musi wystepowac minimum 10 razy
            int minNumOccurences = 10;
            string[] wordFeatures = indexedSpamTermFrequencyDF.Where(
                x => x.Value.GetAs<int>("num_occurences") >= minNumOccurences
            ).RowKeys.ToArray();

            // jesli ham to labelka 1
            var targetVariables = 1 - rawDf.GetColumn<int>("czy_ham");

            // tworzenie tablicy dla algorytmu, by utwalo mu sie przetworzyc
            double[][] input = wiadomoscWordVecDf.Columns[wordFeatures].Rows.Select(
                x => Array.ConvertAll<object, double>(x.Value.ValuesAll.ToArray(), o => Convert.ToDouble(o))
            ).ValuesAll.ToArray();
            int[] output = targetVariables.Values.ToArray();

            // liczba zakladek
            int numFolds = 5;

            var cvNaiveBayesClassifierTeacher = CrossValidation.Create(
                // liczba zakladek
                k: numFolds,
                // Naive Bayes Classifier BernoulliDistribution
                learner: (p) => new NaiveBayesLearning<BernoulliDistribution>(),
                //Zero-One function jako cost function
                loss: (actual, expected, p) => new ZeroOneLoss(expected).Loss(actual),
                
                fit: (teacher, x, y, w) => teacher.Learn(x, y, w),
                x: input,
                y: output
            );

            // Krosowa walidacja
            var result = cvNaiveBayesClassifierTeacher.Learn(input, output);
            
            // wielkosc przykladow
            int numberOfSamples = result.NumberOfSamples;
            int numberOfInputs = result.NumberOfInputs;
            int numberOfOutputs = result.NumberOfOutputs;

            // trenowanie i walidacja błędów
            double trainingError = result.Training.Mean;
            double validationError = result.Validation.Mean;


            // Confusion Matrix
            Console.WriteLine("\n---- Confusion Matrix ----");
            GeneralConfusionMatrix gcm = result.ToConfusionMatrix(input, output);
           
            Console.WriteLine("");
            Console.Write("\t\t 0\t\t 1\n");
            for (int i = 0; i < gcm.Matrix.GetLength(0); i++)
            {
                Console.Write("Predykcja {0} :\t", i);
                for (int j = 0; j < gcm.Matrix.GetLength(1); j++)
                {
                    Console.Write(gcm.Matrix[i, j] + "\t\t\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n---- Wielkość ----");
            Console.WriteLine($"# ilość: {numberOfSamples}");
            Console.WriteLine("błąd trenowania: {0}", trainingError);
            Console.WriteLine("błąd walidacji: {0}\n", validationError);


            float truePositive = (float)gcm.Matrix[1, 1];
            float trueNegative = (float)gcm.Matrix[0, 0];
            float falsePositive = (float)gcm.Matrix[1, 0];
            float falseNegative = (float)gcm.Matrix[0, 1];

            // Accuracy
            Console.WriteLine(
                "Dokładność: {0}",
                (truePositive + trueNegative) / numberOfSamples
            );
            
            Console.WriteLine("Precyzja: {0}", (truePositive / (truePositive + falsePositive)));

            Console.WriteLine("Odrzut: {0}", (truePositive / (truePositive + falseNegative)));

            _loading = false;
            ChangeVisibility();
        }

        private void ChangeVisibility()
        {
            if (_loading)
            {
                labelLoading.Visible = true;

                isSpamLabel.Visible = false;
                buttonCheckSpam.Visible = false;
                textBoxCheckSpam.Visible = false;
                buttonShowDiagrams.Visible = false;
                buttonLoad.Visible = false;
            }
            else
            {
                labelLoading.Visible = false;

                isSpamLabel.Visible = true;
                buttonCheckSpam.Visible = true;
                textBoxCheckSpam.Visible = true;
                buttonShowDiagrams.Visible = true;

                buttonLoad.Visible = false;
            }
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

        private static Frame<int, string> MakeWordVec(string[] rows)
        {
            var wordsByRows = rows.Select((x, i) =>
            {
                var sb = new SeriesBuilder<string, int>();

                ISet<string> words = new HashSet<string>(
                    Regex.Matches(
                        // same litery
                        x, "[a-zA-Z]+('(s|d|t|ve|m))?"
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

        private void buttonShowDiagrams_Click(object sender, EventArgs e)
        {
            _loading = true;
            ChangeVisibility();
            MessageBox.Show("Trwa ładowanie", "Ładowanie");
            
            var hamSmsCount = rawDf.GetColumn<int>("czy_ham").NumSum();
            var spamSmsCount = wiadomoscWordVecDf.RowCount - hamSmsCount;

          
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
                new string[] { "Ham", "Spam" },
                new double[] {
                    hamSmsCount,
                    spamSmsCount
                }
            );
            barChart.SetTitle("Ham vs. Spam ogólnie");

            var hamBarChart = DataBarBox.Show(
                topHamTerms.ToArray(),
                new double[][] {
                    topHamTermsProportions.ToArray(),
                    spamTermProportions.GetItems(topHamTerms).Values.ToArray()
                }
            );
            hamBarChart.AutoScaleMode = AutoScaleMode.Inherit;
            hamBarChart.SetTitle("Top 10 najczęściej użytych słów w Ham Smsach przed filtrowaniem stopwordsow(niebieski: HAM, czerwony: SPAM)");
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
            spamBarChart.SetTitle("Top 10 najczęściej użytych słów w Spam Smsach przed filtrowaniem stopwordsow(niebieskie: HAM, czerwony: SPAM)");
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

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void buttonCheckSpam_Click(object sender, EventArgs e)
        {

        }
    }
}
