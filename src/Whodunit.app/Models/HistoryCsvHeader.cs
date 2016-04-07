namespace Whodunit.app.Models
{

    /// <summary>
    /// Used to write a header to the history CSV file.
    /// </summary>
    public class HistoryCsvHeader
    {
        public string Timestamp { get; set; }
        public string Message { get; set; }
    }

}