namespace Whodunit.app.Controllers
{
    using System;
    using System.Data.SqlTypes;
    using System.Text;
    using System.Web.Mvc;
    using Umbraco.Web.Mvc;

    [PluginController("Whodunit")]
    public class WhodunitController : Umbraco.Web.Mvc.SurfaceController
    {

        /// <summary>
        /// Returns log as CSV
        /// </summary>
        /// <param name="start">Date range start</param>
        /// <param name="stop">Date range stop</param>
        /// <returns></returns>
        public ActionResult GetHistory(DateTime? start, DateTime? stop)
        {
            DateTime actualStart = start.HasValue ? start.Value : SqlDateTime.MinValue.Value;
            DateTime actualStop = stop.HasValue ? stop.Value : SqlDateTime.MaxValue.Value;

            var items = HistoryHelper.GetHistoryItems(actualStart, actualStop);

            StringBuilder result = new StringBuilder();

            result.AppendLine($"Timestamp,Message");
            items.ForEach(x => result.AppendLine($"{x.Timestamp},\"{x.Message}\""));

            return File(
                Encoding.UTF8.GetBytes(result.ToString()),
                "text/csv",
                "AuditTrail-" + actualStart.ToShortDateString() + "_" + actualStop.ToShortDateString() + ".csv"
            );
        }

    }

}