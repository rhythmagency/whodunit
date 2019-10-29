namespace Whodunit.Controllers {

    // Namespaces.
    using Models;
    using System;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web;
    using Umbraco.Core;
    using Umbraco.Web.Mvc;
    using Umbraco.Web.WebApi;


    /// <summary>
    /// API controller for history information.
    /// </summary>
    [PluginController("Whodunit")]
    public class WhodunitApiController : UmbracoAuthorizedApiController {

        #region Methods

        /// <summary>
        /// Returns the path to the CSV file log.
        /// </summary>
        /// <param name="model">
        /// The parameters used to generate the CSV file.
        /// </param>
        /// <returns>
        /// The path to the CSV file of history information.
        /// </returns>
        [System.Web.Http.HttpPost]
        public HttpResponseMessage DownloadCsv(GenerateCsvModel model) {

            // Variables.
            var start = model.StartDate.HasValue
                ? model.StartDate.Value
                : SqlDateTime.MinValue.Value;
            var stop = model.EndDate.HasValue
                ? model.EndDate.Value.AddDays(1)
                : SqlDateTime.MaxValue.Value;

            // Get history items.
            var items = HistoryHelper.GetHistoryItems(start, stop);

            var config = new CsvHelper.Configuration.CsvConfiguration() {
                HasHeaderRecord = true,
                QuoteAllFields = true
            };


            // write CSV data to memory stream
            using (var stream = new MemoryStream()) {
                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true)) {
                    using (var csvWriter = new CsvHelper.CsvWriter(writer, config)) {
                        csvWriter.WriteHeader<HistoryCsvHeader>();
                        foreach (var item in items) {
                            csvWriter.WriteField(item.Timestamp.ToString());
                            csvWriter.WriteField(item.Message);
                            csvWriter.NextRecord();
                        }
                    }
                }

                stream.Seek(0, SeekOrigin.Begin);

                return CreateFileResponse(
                    stream.ToArray(),
                    $"site-activity_{start.ToString("yyyy-MM-dd")}_{stop.ToString("yyyy-MM-dd")}.csv",
                    "text/csv"
                );
            }
        }

        #endregion

        #region helpers

        private static HttpResponseMessage CreateFileResponse(byte[] content, string filename, string contentType) {
            var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(content) };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = filename };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return result;
        }

        #endregion

    }

}