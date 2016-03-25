using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Auditing;
using Umbraco.Core.Persistence;
using Whodunit.app.Models;

namespace Whodunit.app {

    public class HistoryHelper {

        private static Database _sqlHelper;

        static HistoryHelper() {
            _sqlHelper = ApplicationContext.Current.DatabaseContext.Database;
        }

        public static HistoryItem GetHistoryItem(long autoId) {
            return _sqlHelper.Fetch<HistoryItem>(
                "select * from " + HistoryItem.TableName + " where AutoId = @0",
                autoId).FirstOrDefault();
        }

        public static List<HistoryItem> GetHistoryItems(DateTime startDate, DateTime endDate) {
            return _sqlHelper.Fetch<HistoryItem>(
                "select * from " + HistoryItem.TableName + " where Timestamp >= @0 and Timestamp <= @1 order by Timestamp",
                startDate, endDate);
        }

        public static void AddHistoryItem(string message) {
            HistoryItem newItem = new HistoryItem() {
                Message = message,
                Timestamp = DateTime.Now
            };
            _sqlHelper.Insert(
                HistoryItem.TableName,
                HistoryItem.PrimaryKeyName,
                true,
                newItem
            );
        }

    } // end class

} // end namespace
