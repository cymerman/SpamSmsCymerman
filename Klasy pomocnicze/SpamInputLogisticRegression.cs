using Microsoft.ML.Data;

namespace SpamSmsLicencjat.Klasy_pomocnicze
{
    public class SpamInputLogisticRegression
    {
        [LoadColumn((0))] public string RawLabel { get; set; }
        [LoadColumn(1)] public string Message { get; set; }
    }
}
