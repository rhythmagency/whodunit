namespace Whodunit.app.Models {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Umbraco.Core.Persistence;
    using Umbraco.Core.Persistence.DatabaseAnnotations;

    [TableName(HistoryItem.TableName)]
    [PrimaryKey(HistoryItem.PrimaryKeyName, autoIncrement = true)]
    public class HistoryItem {

        public const string TableName = "Whodunit.HistoryItem";
        public const string PrimaryKeyName = "AutoId";

        [PrimaryKeyColumn(AutoIncrement = true, Clustered = true)]
        public ulong AutoId { get; set; }

        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

    }

}
