using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.ML;
using SpamSmsLicencjat.Klasy_pomocnicze;

namespace LogisticRegression
{
    public class LogisticRegression
    {
        private static string dataPath = Path.Combine(Environment.CurrentDirectory, "dane.tsv");

        public Microsoft.ML.Data.CalibratedBinaryClassificationMetrics metrics;
        public PredictionEngine<SpamInputLogisticRegression, SpamPredictionLogisticRegression> predictionEngine;
        public IReadOnlyList<TrainCatalogBase.CrossValidationResult<Microsoft.ML.Data.CalibratedBinaryClassificationMetrics>> cvResults;


        public LogisticRegression()
        {

        //machine learning kontekst
        var context = new MLContext();

            //dane z pliku mapowane na SpamInput
            var data = context.Data.LoadFromTextFile<SpamInputLogisticRegression>(
                path: dataPath,
                hasHeader: true,
                separatorChar: '\t');

            //użycie 80%  danych na trening i 20% do testowania
            var partitions = context.Data.TrainTestSplit(
                data,
                testFraction: 0.2);

            //ustawiamy pipeline
            //krok 1: zamieniamy spam i ham wartosci na true i false
            var pipeline = context.Transforms.CustomMapping<FromLabel, ToLabel>(
                    (input, output) => { output.Label = input.RawLabel == "spam" ? true : false; },
                    contractName: "MyLambda")

                //krok 2: poprawienie tekstu który został przetwarzany - normalizacja , usuniecie stopwordsow , lowercase , usuniecie znakow , TF-IDF , tokenizacja kazdego slowa (zeby bardziej bylo zrozumiale dla komputera)
                .Append(context.Transforms.Text.FeaturizeText(outputColumnName: "Features",
                    new Microsoft.ML.Transforms.Text.TextFeaturizingEstimator.Options
                    {
                        WordFeatureExtractor = new Microsoft.ML.Transforms.Text.WordBagEstimator.Options
                        { NgramLength = 2, UseAllLengths = true },
                        CharFeatureExtractor = new Microsoft.ML.Transforms.Text.WordBagEstimator.Options
                        { NgramLength = 3, UseAllLengths = false },
                    }, nameof(SpamInputLogisticRegression.Message)))

                //krok 3: klasyfikacja za pomocą skalibrowanej regresji logistycznej
                .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression(
                    ));


            //k-fold krosowa walidacja która ma na celu użycie 5 krotne tych samych danych ale w innym rozkladzie

            cvResults = context.BinaryClassification.CrossValidate(
                data: partitions.TrainSet,
                estimator: pipeline,
                numberOfFolds: 5);

            //trenujemy model na zbiorze treningowym
            var model = pipeline.Fit(partitions.TrainSet);

            // ocena modelu na zbiorze testowym
            var predictions = model.Transform(partitions.TestSet);
            metrics = context.BinaryClassification.Evaluate(
                data: predictions,
                labelColumnName: "Label",
                scoreColumnName: "Score");

            predictionEngine =
                context.Model.CreatePredictionEngine<SpamInputLogisticRegression, SpamPredictionLogisticRegression>(
                    model);

        }


    }
}
