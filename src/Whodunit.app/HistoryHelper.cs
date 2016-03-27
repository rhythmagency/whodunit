using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Whodunit.app.Models;

namespace Whodunit.app {
    using System.Data;
    using System.Data.SqlClient;

    public class HistoryHelper {

        private static Database _sqlHelper;

        static HistoryHelper() {
            _sqlHelper = ApplicationContext.Current.DatabaseContext.Database;
        }

        public static HistoryItem GetHistoryItem(long autoId)
        {
            return _sqlHelper.SingleOrDefault<HistoryItem>(autoId);
        }

        public static List<HistoryItem> GetHistoryItems(DateTime startDate, DateTime endDate)
        {
            return _sqlHelper.Fetch<HistoryItem>($"SELECT * FROM {HistoryItem.TableName} WHERE Timestamp>=@0 AND Timestamp<=@1", 
                new SqlParameter() { DbType = DbType.DateTime, Value = startDate},
                new SqlParameter() { DbType = DbType.DateTime, Value = endDate});
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
