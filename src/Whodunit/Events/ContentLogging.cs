namespace Whodunit.Events
{

    // Namespaces.
    using System.Collections.Generic;
    using System.Linq;
    using umbraco.BusinessLogic;
    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Models;
    using Umbraco.Core.Publishing;
    using Umbraco.Core.Services;
    using Umbraco.Web;
    using static Whodunit.ExtensionMethods.CollectionExtensionMethods;

    public class ContentLogging : ApplicationEventHandler
    {

        #region Constants

        private const string ListSeparator = " | ";

        #endregion

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

        private void MediaService_Trashed(IMediaService sender, MoveEventArgs<IMedia> e)
        {
            var signature = GetUserSignature(CurrentUser);
            var mediaSignature = GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity));
            HistoryHelper.AddHistoryItem($"{signature} moved the following media item into the trash: {mediaSignature}");
        }

        private void MediaService_Saved(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            var signature = GetUserSignature(CurrentUser);
            var mediaSignature = GetContentSignatures(e.SavedEntities);
            HistoryHelper.AddHistoryItem($"{signature} saved the following media item: {mediaSignature}");
        }

        private void MediaService_Moved(IMediaService sender, MoveEventArgs<IMedia> e)
        {
            var signature = GetUserSignature(CurrentUser);
            var mediaSignature = GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity));
            HistoryHelper.AddHistoryItem($"{signature} moved the following media item: {mediaSignature}");
        }

        private void MediaService_EmptiedRecycleBin(IMediaService sender, RecycleBinEventArgs e)
        {
            var signature = GetUserSignature(CurrentUser);
            var ids = e.Ids.Select(i => i.ToString()).CombineStrings(ListSeparator);
            HistoryHelper.AddHistoryItem($"{signature} emptied the recycle bin, permanently deleting media items with the following ids: {ids}");
        }

        private void MediaService_Deleted(IMediaService sender, DeleteEventArgs<IMedia> e)
        {
            var signature = GetUserSignature(CurrentUser);
            var mediaSignature = GetContentSignatures(e.DeletedEntities);
            HistoryHelper.AddHistoryItem($"{signature} deleted the following media item: {mediaSignature}");
        }

        private void MediaService_Created(IMediaService sender, NewEventArgs<IMedia> e)
        {
            var signature = GetUserSignature(CurrentUser);
            var mediaSignature = GetContentSignature(e.Entity);
            HistoryHelper.AddHistoryItem($"{signature} created new media item: {mediaSignature}");
        }

        #endregion

        #region ContentService Event Handlers

        private void ContentService_Copied(IContentService sender, CopyEventArgs<IContent> e)
        {
            var signature = GetUserSignature(CurrentUser);
            var fromSignature = GetContentSignature(e.Original);
            var toSignature = GetContentSignature(e.Copy);
            HistoryHelper.AddHistoryItem($"{signature} copied content from {fromSignature} to {toSignature}");
        }

        private void ContentService_Created(IContentService sender, NewEventArgs<IContent> e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = GetContentSignature(e.Entity);
            HistoryHelper.AddHistoryItem($"{user} created {contentSignature}");
        }

        private void ContentService_Deleted(IContentService sender, DeleteEventArgs<IContent> e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = GetContentSignatures(e.DeletedEntities);
            HistoryHelper.AddHistoryItem($"{user} deleted the following content: {contentSignature}");
        }

        private void ContentService_EmptiedRecycleBin(IContentService sender, RecycleBinEventArgs e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = e.Ids.Select(i => i.ToString()).CombineStrings(ListSeparator);
            HistoryHelper.AddHistoryItem($"{user} emptied the recycle bin, permanently deleting nodes with the following ids: {contentSignature}");
        }

        private void ContentService_Moved(IContentService sender, MoveEventArgs<IContent> e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity));
            HistoryHelper.AddHistoryItem($"{user} moved the following content: {contentSignature}");
        }

        private void ContentService_Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = GetContentSignatures(e.PublishedEntities);
            HistoryHelper.AddHistoryItem($"{user} published the following content: {contentSignature}");
        }

        private void ContentService_RolledBack(IContentService sender, RollbackEventArgs<IContent> e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = GetContentSignature(e.Entity);
            HistoryHelper.AddHistoryItem($"{user} rolled back the following content: {contentSignature}");
        }

        private void ContentService_Saved(IContentService sender, SaveEventArgs<IContent> e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = GetContentSignatures(e.SavedEntities);
            HistoryHelper.AddHistoryItem($"{user} saved the following content: {contentSignature}");
        }

        private void ContentService_Trashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = GetContentSignatures(e.MoveInfoCollection.Select(i => i.Entity));
            HistoryHelper.AddHistoryItem($"{user} moved the following content into the trash: {contentSignature}");
        }

        private void ContentService_UnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            var user = GetUserSignature(CurrentUser);
            var contentSignature = GetContentSignatures(e.PublishedEntities);
            HistoryHelper.AddHistoryItem($"{user} unpublished the following content: {contentSignature}");
        }

        #endregion

        #region Private Getters

        private User CurrentUser => umbraco.helper.GetCurrentUmbracoUser();

        private string GetUserSignature(User source)
        {
            return source == null
                ? "unknown user"
                : source.Name + " (" + source.Email + ")";
        }

        private string GetContentSignature(IContent item)
        {
            var id = item.Id;
            var helper = new UmbracoHelper(UmbracoContext.Current);
            var node = helper.TypedContent(id);
            var url = node?.Url ?? "Unknown URL";
            return $"{item.Id} ({item.Path} {item.Name} at {url})";
        }

        private string GetContentSignature(IMedia item)
        {
            var id = item.Id;
            var helper = new UmbracoHelper(UmbracoContext.Current);
            var node = helper.TypedMedia(id);
            var url = node?.Url ?? "Unknown URL";
            return $"{id} ({item.Path} {item.Name} at {url})";
        }

        private string GetContentSignatures(IEnumerable<IContent> items) =>
            items.Select(GetContentSignature).CombineStrings(ListSeparator);

        private string GetContentSignatures(IEnumerable<IMedia> items) =>
            items.Select(GetContentSignature).CombineStrings(ListSeparator);

        #endregion

    }

}