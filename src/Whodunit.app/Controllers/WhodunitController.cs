using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Whodunit.app.Controllers {

    public class WhodunitController : Umbraco.Web.Mvc.SurfaceController {

        public ActionResult GetHistory(DateTime? start, DateTime? stop) {
            DateTime actualStart = start.HasValue ? start.Value : DateTime.MinValue;
            DateTime actualStop = stop.HasValue ? stop.Value : DateTime.MaxValue;

            var items = HistoryHelper.GetHistoryItems(actualStart, actualStop);

            StringBuilder result = new StringBuilder();

            // TODO: add logic here to actually write out history items

            return File(
                Encoding.UTF8.GetBytes(result.ToString()),
                "text/csv",
                "AuditTrail-" + actualStart.ToShortDateString() + "-" + actualStop.ToShortDateString() + ".csv"
            );
        } // end method

    } // end class

} // end namespace
