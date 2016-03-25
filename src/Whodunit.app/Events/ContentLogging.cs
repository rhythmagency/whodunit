using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco.BusinessLogic;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Whodunit.app.Models;

namespace Whodunit.app.Events {

    public class ContentLogging : ApplicationEventHandler {

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) {
            ContentService.Copied += ContentService_Copied;
            ContentService.Created += ContentService_Created;
            ContentService.Deleted += ContentService_Deleted;
            ContentService.EmptiedRecycleBin += ContentService_EmptiedRecycleBin;
            ContentService.Moved += ContentService_Moved;
            ContentService.Published += ContentService_Published;
            ContentService.RolledBack += ContentService_RolledBack;
            ContentService.Saved += ContentService_Saved;
            ContentService.Trashed += ContentService_Trashed;
            ContentService.UnPublished += ContentService_UnPublished;
        }

        private void ContentService_Copied(IContentService sender, Umbraco.Core.Events.CopyEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " copied content from " + GetContentSignature(e.Original) + " to " + GetContentSignature(e.Copy));
        }

        private void ContentService_Created(IContentService sender, Umbraco.Core.Events.NewEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " created " + GetContentSignature(e.Entity));
        }

        private void ContentService_Deleted(IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " deleted the following content: " + GetContentSignatures(e.DeletedEntities));
        }

        private void ContentService_EmptiedRecycleBin(IContentService sender, Umbraco.Core.Events.RecycleBinEventArgs e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " emptied the recycle bin, permanently deleting nodes with the following ids: " + e.Ids.Select(i => i.ToString()).Aggregate((curr, next) => curr + ListSeparator + next));
        }

        private void ContentService_Moved(IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " moved the following content: " + GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity)));
        }

        private void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " published the following content: " + GetContentSignatures(e.PublishedEntities));
        }

        private void ContentService_RolledBack(IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " rolled back " + GetContentSignature(e.Entity));
        }

        private void ContentService_Saved(IContentService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " saved the following content: " + GetContentSignatures(e.SavedEntities));
        }

        private void ContentService_Trashed(IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " moved the following content into the trash: " + GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity)));
        }

        private void ContentService_UnPublished(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e) {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " unpublished the following content: " + GetContentSignatures(e.PublishedEntities));
        }

        private const string ListSeparator = " | ";

        private User CurrentUser
        {
            get { return umbraco.helper.GetCurrentUmbracoUser(); }
        }

        private string GetUserSignature(User source) {
            return source.Name + " (" + source.Email + ")";
        }

        private string GetContentSignature(IContent item) {
            return item.Id.ToString() + " (" + item.Path + item.Name + ")";
        }

        private string GetContentSignatures(IEnumerable<IContent> items) {
            return items.Select(s => GetContentSignature(s)).Aggregate((curr, next) => curr + ListSeparator + next);
        }

    } // end class

} // end namespace
