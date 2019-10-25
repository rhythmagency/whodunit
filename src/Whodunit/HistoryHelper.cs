namespace Whodunit
{

    // Namespaces.
    using System;
    using System.Collections.Generic;
    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;
    using Whodunit.Models;


    /// <summary>
    /// Helps with history items in the database.
    /// </summary>
    public class HistoryHelper
    {

        #region Variables

        private static Database _sqlHelper;

        #endregion


        #region Constructors

        /// <summary>
        /// Static constructor.
        /// </summary>
        static HistoryHelper()
        {
            _sqlHelper = ApplicationContext.Current.DatabaseContext.Database;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Gets all history items in the specified date range from the database.
        /// </summary>
        /// <param name="startDate">
        /// The start of the date range, inclusive.
        /// </param>
        /// <param name="endDate">
        /// The end of the date range, exclusive.
        /// </param>
        /// <returns>
        /// The history items.
        /// </returns>
        public static List<HistoryItem> GetHistoryItems(DateTime startDate, DateTime endDate)
        {
            var query = $"SELECT * FROM {HistoryItem.TableName} WHERE Timestamp >= @0 AND Timestamp < @1";
            return _sqlHelper.Fetch<HistoryItem>(query, startDate, endDate);
        }


        /// <summary>
        /// Adds a history item to the databse.
        /// </summary>
        /// <param name="message">
        /// The message to add to the history item.
        /// </param>
        public static void AddHistoryItem(string message)
        {

            // Variables.
            var now = DateTime.Now;
            var parts = new List<string>();


            // Split message into parts to avoid SQL errors.
            for (var i = 0; i < message.Length; i += 3000)
            {
                var length = Math.Min(3000, message.Length - i);
                var part = message.Substring(i, length);
                if (parts.Count > 0)
                {
                    part = "(Truncated message continued) " + part;
                }
                parts.Add(part);
            }


            // Log each message part.
            for (var i = 0; i < parts.Count; i++)
            {
                HistoryItem newItem = new HistoryItem()
                {
                    Message = parts[i],
                    Timestamp = now
                };


                // Attempt to insert history message.
                try
                {
                    _sqlHelper.Insert(
                        HistoryItem.TableName,
                        HistoryItem.PrimaryKeyName,
                        true,
                        newItem
                    );
                }
                catch (Exception ex)
                {
                    LogHelper.Error<HistoryHelper>("Error inserting history message.", ex);
                }

            }

        }

        #endregion

    }

}