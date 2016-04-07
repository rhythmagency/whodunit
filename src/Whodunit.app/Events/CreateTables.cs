namespace Whodunit.app.Events
{

    // Namespaces.
    using System;
    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Persistence;
    using Whodunit.app.Models;


    /// <summary>
    /// Handles the creation of database tables.
    /// </summary>
    public class CreateTables : ApplicationEventHandler
    {

        #region Event Handlers

        /// <summary>
        /// Handles the application started event.
        /// </summary>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            var ctx = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(ctx.Database, applicationContext.ProfilingLogger.Logger,
                ctx.SqlSyntax);
            TryCreateTable<HistoryItem>(db, HistoryItem.TableName);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Attempt to create the specified table.
        /// </summary>
        /// <typeparam name="T">
        /// The type information for the table.
        /// </typeparam>
        /// <param name="db">
        /// The database.
        /// </param>
        /// <param name="tableName">
        /// The name of the table.
        /// </param>
        private void TryCreateTable<T>(DatabaseSchemaHelper db, string tableName) where T : class, new()
        {
            try
            {
                if (!db.TableExist(tableName))
                {
                    db.CreateTable<T>(false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<CreateTables>($"Unable to create table {tableName}.", ex);
            }
        }

        #endregion

    }

}