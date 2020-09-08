using Microsoft.ML.Data;

namespace SpamSmsLicencjat.Klasy_pomocnicze
{
    public class SpamInputAveragedPerceptron
    {
        [LoadColumn(0)] public string Label { get; set; }
        [LoadColumn(1)] public string Message { get; set; }
    }
}
