using Microsoft.ML.Data;

namespace SpamSmsLicencjat.Klasy_pomocnicze
{
    public class SpamPrediction
    {
        [ColumnName("PredictedLabel")] public bool IsSpam { get; set; }
        public float Score { get; set; }
        public float Probability { get; set; }
    }
}
