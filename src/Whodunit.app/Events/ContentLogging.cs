using System.Collections.Generic;
using System.Linq;
using umbraco.BusinessLogic;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Whodunit.app.Events
{

    public class ContentLogging : ApplicationEventHandler
    {

        #region Overrides

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
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

            MediaService.Created += MediaService_Created;
            MediaService.Deleted += MediaService_Deleted;
            MediaService.EmptiedRecycleBin += MediaService_EmptiedRecycleBin;
            MediaService.Moved += MediaService_Moved;
            MediaService.Saved += MediaService_Saved;
            MediaService.Trashed += MediaService_Trashed;
        }

        #endregion

        #region MediaService Event Handlers

        private void MediaService_Trashed(IMediaService sender, Umbraco.Core.Events.MoveEventArgs<IMedia> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " moved the following media item into the trash: " + GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity)));
        }

        private void MediaService_Saved(IMediaService sender, Umbraco.Core.Events.SaveEventArgs<IMedia> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " saved the following media item: " + GetContentSignatures(e.SavedEntities));
        }

        private void MediaService_Moved(IMediaService sender, Umbraco.Core.Events.MoveEventArgs<IMedia> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " moved the following media item: " + GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity)));
        }

        private void MediaService_EmptiedRecycleBin(IMediaService sender, Umbraco.Core.Events.RecycleBinEventArgs e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " emptied the recycle bin, permanently deleting media items with the following ids: " + e.Ids.Select(i => i.ToString()).Aggregate((curr, next) => curr + ListSeparator + next));
        }

        private void MediaService_Deleted(IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<IMedia> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " deleted the following media item: " + GetContentSignatures(e.DeletedEntities));
        }

        private void MediaService_Created(IMediaService sender, Umbraco.Core.Events.NewEventArgs<IMedia> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " created new media item: " + GetContentSignature(e.Entity));
        }

        #endregion

        #region ContentService Event Handlers

        private void ContentService_Copied(IContentService sender, Umbraco.Core.Events.CopyEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " copied content from " + GetContentSignature(e.Original) + " to " + GetContentSignature(e.Copy));
        }

        private void ContentService_Created(IContentService sender, Umbraco.Core.Events.NewEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " created " + GetContentSignature(e.Entity));
        }

        private void ContentService_Deleted(IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " deleted the following content: " + GetContentSignatures(e.DeletedEntities));
        }

        private void ContentService_EmptiedRecycleBin(IContentService sender, Umbraco.Core.Events.RecycleBinEventArgs e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " emptied the recycle bin, permanently deleting nodes with the following ids: " + e.Ids.Select(i => i.ToString()).Aggregate((curr, next) => curr + ListSeparator + next));
        }

        private void ContentService_Moved(IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " moved the following content: " + GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity)));
        }

        private void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " published the following content: " + GetContentSignatures(e.PublishedEntities));
        }

        private void ContentService_RolledBack(IContentService sender, Umbraco.Core.Events.RollbackEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " rolled back " + GetContentSignature(e.Entity));
        }

        private void ContentService_Saved(IContentService sender, Umbraco.Core.Events.SaveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " saved the following content: " + GetContentSignatures(e.SavedEntities));
        }

        private void ContentService_Trashed(IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " moved the following content into the trash: " + GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity)));
        }

        private void ContentService_UnPublished(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            HistoryHelper.AddHistoryItem(GetUserSignature(CurrentUser) + " unpublished the following content: " + GetContentSignatures(e.PublishedEntities));
        }

        #endregion

        #region Constants

        private const string ListSeparator = " | ";

        #endregion

        #region Private Getters
        private User CurrentUser => umbraco.helper.GetCurrentUmbracoUser();

        private string GetUserSignature(User source) => source.Name + " (" + source.Email + ")";

        private string GetContentSignature(IContent item) => item.Id + " (" + item.Path + " " + item.Name + ")";

        private string GetContentSignature(IMedia item) => item.Id + " (" + item.Path + " " + item.Name + ")";

        private string GetContentSignatures(IEnumerable<IContent> items) => items.Select(GetContentSignature).Aggregate((curr, next) => curr + ListSeparator + next);

        private string GetContentSignatures(IEnumerable<IMedia> items) => items.Select(GetContentSignature).Aggregate((curr, next) => curr + ListSeparator + next);
        #endregion

    }

}