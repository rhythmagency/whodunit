namespace Whodunit.Models
{

    // Namespaces.
    using System;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;


    /// <summary>
    /// A history entry to track an event (e.g., the publish event for a content node).
    /// </summary>
    [TableName(TableName)]
    [PrimaryKey(PrimaryKeyName, autoIncrement = true)]
    public class HistoryItem
    {

        #region Constants

        public const string TableName = "WhodunitHistoryItem";
        public const string PrimaryKeyName = "AutoId";

        #endregion


        #region Properties

        /// <summary>
        /// The primary key.
        /// </summary>
        [PrimaryKeyColumn(AutoIncrement = true, Clustered = true)]
        public long AutoId { get; set; }


        /// <summary>
        /// The timestamp of the event.
        /// </summary>
        public DateTime Timestamp { get; set; }


        /// <summary>
        /// The messaage containing details for the event.
        /// </summary>
        [Length(4000)]
        public string Message { get; set; }

        #endregion

    }

}