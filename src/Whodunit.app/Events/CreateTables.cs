using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Whodunit.app.Models;

namespace Whodunit.app.Events {
    public class CreateTables : ApplicationEventHandler {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            var ctx = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(ctx.Database, applicationContext.ProfilingLogger.Logger, ctx.SqlSyntax);

            TryCreateTable<HistoryItem>(db, HistoryItem.TableName);
        }

        public void TryCreateTable<T>(DatabaseSchemaHelper db, string tableName) where T : class, new() {
            try {
                if (!db.TableExist(tableName))
                    db.CreateTable<T>(false);
            }
            catch (Exception ex) {
                LogHelper.Error<CreateTables>($"Unable to create table {tableName}.", ex);
            }
        }

    }
}
