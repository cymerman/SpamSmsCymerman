using Microsoft.ML.Data;

namespace SpamSmsLicencjat.Klasy_pomocnicze
{
    public class SpamPredictionAveragedPerceptron
    {
        [ColumnName("PredictedLabel")] public string isSpam { get; set; }
    }
}
