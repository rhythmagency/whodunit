namespace Whodunit.app.Controllers
{

    // Namespaces.
    using Models;
    using System;
    using System.Data.SqlTypes;
    using System.IO;
    using System.Web;
    using Umbraco.Core;
    using Umbraco.Web.Mvc;
    using Umbraco.Web.WebApi;


    /// <summary>
    /// API controller for history information.
    /// </summary>
    [PluginController("Whodunit")]
    public class WhodunitApiController : UmbracoAuthorizedApiController
    {

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
        public string GetHistory(GetHistoryModel model)
        {

            // Variables.
            var start = model.StartDate.HasValue
                ? model.StartDate.Value
                : SqlDateTime.MinValue.Value;
            var stop = model.EndDate.HasValue
                ? model.EndDate.Value.AddDays(1)
                : SqlDateTime.MaxValue.Value;
            var request = HttpContext.Current.Request;
            var trimPath = request.MapPath("~");
            var basePath = request.MapPath("~/GeneratedReports");
            var filename = Guid.NewGuid().ToString("N") + ".csv";
            var filePath = Path.Combine(basePath, filename);
            var config = new CsvHelper.Configuration.CsvConfiguration()
            {
                HasHeaderRecord = true,
                QuoteAllFields = true
            };


            // Get history items.
            var items = HistoryHelper.GetHistoryItems(start, stop);


            // Ensure folder exists.
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }


            // Store to CSV.
            using (var textWriter = File.CreateText(filePath))
            {
                using (var writer = new CsvHelper.CsvWriter(textWriter, config))
                {
                    writer.WriteHeader<HistoryCsvHeader>();
                    foreach (var item in items)
                    {
                        writer.WriteField(item.Timestamp.ToString());
                        writer.WriteField(item.Message);
                        writer.NextRecord();
                    }
                }
            }


            // Return a path that the website can link to.
            var relativePath = filePath.InvariantStartsWith(trimPath)
                ? filePath.Substring(trimPath.Length)
                : filePath;
            relativePath = "/" + relativePath.Replace(@"\", @"/");
            return relativePath;

        }

        #endregion

    }

}