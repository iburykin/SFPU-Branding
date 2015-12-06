using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Microsoft.Office.Server.Search.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint.WebPartPages;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace sfpucBranding.Features
{
    public static class Utility
    {
        internal static SPContentType CreateSiteContentType(SPWeb web, string contentTypeName, SPContentTypeId parentItemCTypeId, string group, string perentCT)
        {
            var tt = web.AvailableContentTypes[contentTypeName];
            if (web.AvailableContentTypes[contentTypeName] == null)
            {
                SPContentType itemCType = web.AvailableContentTypes[perentCT];//parentItemCTypeId
                SPContentType contentType =
                    new SPContentType(itemCType, web.ContentTypes, contentTypeName) { Group = @group };
                web.ContentTypes.Add(contentType);
                contentType.Update();
                return contentType;
            }
            return web.ContentTypes[contentTypeName];
        }

        public static bool CratePage(SPWeb web, string pageName, string nameLayout, bool checkinPublish, string title = "")
        {
            try
            {
                web.AllowUnsafeUpdates = true;
                if (PublishingWeb.IsPublishingWeb(web))
                {
                    PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(web);
                    if (publishingWeb.GetPublishingPages().FirstOrDefault(x => x.Name.Equals(pageName)) == null)
                    {
                        PageLayout layout =
                            publishingWeb.GetAvailablePageLayouts()
                                .FirstOrDefault(
                                    p => p.Name.Equals(nameLayout, StringComparison.InvariantCultureIgnoreCase));
                        PublishingPage newPage = publishingWeb.GetPublishingPages().Add(pageName, layout);
                        newPage.ListItem.Update();
                        newPage.Title = title;
                        newPage.Update();
                        if (checkinPublish)
                        {
                            try
                            {
                                SPFile pageFile = newPage.ListItem.File;

                                if (pageFile.CheckOutStatus != SPFile.SPCheckOutStatus.None)
                                {
                                    pageFile.CheckIn("CheckedIn");
                                    pageFile.Publish("publihsed");
                                }

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void CreateSiteColumn(SPWeb web, SiteColumnCustom customColumn)
        {
            if (!web.Fields.ContainsField(customColumn.Name))
            {
                string fieldName;
                if (customColumn.ColumnType == SPFieldType.Choice && customColumn.Choices != null)
                {
                    fieldName = web.Fields.Add(customColumn.Name, customColumn.ColumnType, false, false, customColumn.Choices);
                    SPFieldChoice field = (SPFieldChoice)web.Fields.GetFieldByInternalName(fieldName);
                    field.Group = customColumn.Group;
                    field.DefaultValue = customColumn.Choices[0];
                    field.Update();
                }
                else if (customColumn.ColumnType == SPFieldType.DateTime)
                {

                    fieldName = web.Fields.Add(customColumn.Name, customColumn.ColumnType, false);
                    SPFieldDateTime field = (SPFieldDateTime)web.Fields.GetFieldByInternalName(fieldName);
                    field.Group = customColumn.Group;
                    if (customColumn.DateOnly) field.DisplayFormat = SPDateTimeFieldFormatType.DateOnly;
                    field.Update();
                }
                else
                {
                    fieldName = web.Fields.Add(customColumn.Name, customColumn.ColumnType, false);
                    SPField field = web.Fields.GetFieldByInternalName(fieldName);
                    field.Group = customColumn.Group;
                    field.Update();
                }
            }
        }

        public static SPField GetSiteColumn(SPWeb web, string displayName, SPFieldType fieldType, string groupDescriptor)
        {
            SPSite spSite = new SPSite(web.Url);
            SPWeb spWeb = spSite.RootWeb;
            if (!spWeb.Fields.ContainsField(displayName))
            {
                string fieldName = spWeb.Fields.Add(displayName, fieldType, false);
                SPField field = spWeb.Fields.GetFieldByInternalName(fieldName);
                field.Group = groupDescriptor;
                field.Update();
                return field;
            }
            return spWeb.Fields[displayName];
        }

        public static SPField CreateSiteColumnTaxonomy(SPWeb web, string displayName, string groupDescriptor)
        {
            if (!web.Fields.ContainsField(displayName))
            {
                //Get the Taxonomy session for current SPSite
                TaxonomyField field = web.Fields.CreateNewField("TaxonomyFieldType", displayName) as TaxonomyField;
                field.TargetTemplate = string.Empty;
                field.AllowMultipleValues = true;
                field.CreateValuesInEditForm = true;
                field.Open = true;
                field.AnchorId = Guid.Empty;
                field.Group = "Taxonomy Fields";

                string fieldName = web.Fields.Add(field);
                SPField fieldsp = web.Fields.GetFieldByInternalName(fieldName);
                fieldsp.Group = groupDescriptor;
                fieldsp.Update();
                return fieldsp;
            }
            return web.Fields[displayName];
        }

        public static void AddFieldToContentType(SPWeb web, SPContentTypeId contentTypeId, SPField field)
        {
            SPContentType contentType = web.ContentTypes[contentTypeId];
            if (contentType == null) return;
            if (contentType.Fields.ContainsField(field.Title)) return;
            SPFieldLink fieldLink = new SPFieldLink(field);
            contentType.FieldLinks.Add(fieldLink);
            contentType.Update();
        }

        internal static void CreateList(SPWeb spWeb, ListCustom listEntity)
        {
            if (spWeb.Lists.TryGetList(listEntity.Name) == null)
            {
                spWeb.AllowUnsafeUpdates = true;
                spWeb.Lists.Add(listEntity.Name, string.Empty, listEntity.ListTemplateType);
            }
            foreach (SPField field in listEntity.Fields)
            {
                var newList = spWeb.Lists[listEntity.Name];
                if (!newList.Fields.ContainsField(field.InternalName))
                {
                    newList.Fields.Add(field);

                    SPView view = newList.DefaultView;
                    view.ViewFields.Add(field.InternalName);
                    view.Update();
                }
            }
        }

        internal static bool CheckListForExist(string listName, SPWeb spWeb, SPListTemplateType listTemplateType, bool createList)
        {
            try
            {
                if (spWeb.Lists.TryGetList(listName) == null)
                {
                    if (createList)
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spWeb.Lists.Add(listName, string.Empty, listTemplateType);
                        if (listName == "QuickPolls")
                        {
                            SPList newList = spWeb.Lists["QuickPolls"];
                            newList.Fields.Add("Question", SPFieldType.Text, true);
                            newList.Fields.Add("Answers", SPFieldType.Note, true);
                            newList.Fields.Add("Response", SPFieldType.Text, true);
                            newList.Fields.Add("Expires", SPFieldType.DateTime, true);

                            SPView view = newList.DefaultView;
                            view.ViewFields.Add("Question");
                            view.ViewFields.Add("Answers");
                            view.ViewFields.Add("Response");
                            view.ViewFields.Add("Expires");
                            view.Update();

                            SPListItem newItem = newList.AddItem();
                            newItem["Title"] = "Favorite berry";
                            newItem["Question"] = "What is your favorite berry?";
                            newItem["Answers"] = "Strawberry\r\nBlackberry\r\nBlueberry\r\nHuckleberry\r\nRasberry";
                            newItem["Expires"] = DateTime.Today.AddYears(1);
                            newItem.Update();
                        }
                    }
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void RefreshDisplayTemplate(SPFeatureReceiverProperties properties, string path)
        {
            try
            {
                string featureId = properties.Feature.Definition.Id.ToString();
                SPWeb site = properties.Feature.Parent as SPWeb;

                SPFolder displayTemplateFolder = site.GetFolder(path);
                if (displayTemplateFolder.Exists)
                {
                    SPList parentList = displayTemplateFolder.ParentWeb.Lists[displayTemplateFolder.ParentListId];

                    SPFileCollection files = displayTemplateFolder.Files;
                    var templateFiles = from SPFile f
                        in files
                                        where
                                            String.Equals(f.Properties["FeatureId"] as string, featureId,
                                                StringComparison.InvariantCultureIgnoreCase)
                                        select f;

                    List<Guid> guidFilesToModify = new List<Guid>();
                    foreach (SPFile file in templateFiles)
                    {
                        guidFilesToModify.Add(file.UniqueId);
                    }

                    foreach (Guid fileId in guidFilesToModify)
                    {
                        // instantiate new object to avoid modifying the collection during enumeration
                        SPFile file = parentList.ParentWeb.GetFile(fileId);

                        // get the file contents
                        byte[] fileBytes = file.OpenBinary();

                        // re-add the same file again, forcing the event receiver to fire
                        displayTemplateFolder.Files.Add(file.Name, fileBytes, true);

                        SPFile fileNew = parentList.ParentWeb.GetFile(fileId);
                        CheckInAndPublishFile(fileNew, String.Empty, SPCheckinType.MajorCheckIn);

                    }
                }
                else
                {
                    EssnLog.logInfo("Folder '" + path + "' is not exist on " + site.Url);
                }
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on RefreshDisplayTemplate in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }

        public static void CheckInAndPublishFile(SPFile homePageFile, string message, SPCheckinType checkinType)
        {
            try
            {
                homePageFile.CheckIn(message, checkinType);
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on homePageFile.CheckIn in CheckInAndPublishFile in FeatureActivated.");
                EssnLog.logExc(ee);
            }
            try
            {
                homePageFile.Publish(message);
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on homePageFile.Publish in CheckInAndPublishFile in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }

        public static void CheckInAndPublishFile(SPFile homePageFile, string message)
        {
            try
            {
                homePageFile.CheckIn(message);
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on homePageFile.CheckIn in CheckInAndPublishFile in FeatureActivated.");
                EssnLog.logExc(ee);
            }
            try
            {
                homePageFile.Publish(message);
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on homePageFile.Publish in CheckInAndPublishFile in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }

        public static void CheckOutFile(SPFile homePageFile)
        {
            try
            {
                if (homePageFile.CheckOutStatus == SPFile.SPCheckOutStatus.None)
                    homePageFile.CheckOut();
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on CheckOutFile in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }

        public static void SetWelcomePage(SPWeb spWeb, string nameAndPathWelcomePage)
        {
            try
            {
                SPFolder rootFolder = spWeb.RootFolder;
                rootFolder.WelcomePage = nameAndPathWelcomePage;
                rootFolder.Update();
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on SetWelcomePage in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }

        public static void MasterUrlAndCustomMasterUrl(SPWeb spWeb, string siteMasterUrl, string siteSystemMasterUrl)
        {
            try
            {
                Uri siteMaster = new Uri(spWeb.Url + siteMasterUrl);
                Uri siteSystemMaster = new Uri(spWeb.Url + siteSystemMasterUrl);
                spWeb.CustomMasterUrl = siteMaster.AbsolutePath;
                spWeb.MasterUrl = siteSystemMaster.AbsolutePath;
                spWeb.Update();
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on MasterUrlAndCustomMasterUrl in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }

        public static void SetContentTypeForLayOut(SPWeb spWeb, Dictionary<string, string> dictionary)
        {
            try
            {
                foreach (KeyValuePair<string, string> pageName in dictionary)
                {
                    SPFile pageFile = spWeb.GetFile(pageName.Key);
                    if (pageFile.Exists) // check if page exist
                    {
                        pageFile.Item.Properties["PublishingAssociatedContentType"] = String.Format(";#{0};#{1};#", pageName.Value, spWeb.AvailableContentTypes[pageName.Value].Id.ToString());
                        pageFile.Item.SystemUpdate();
                    }
                    pageFile.Update();
                }

            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on SetContentTypeForLayOut in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }

        public static SPContentType GetContentTypeByName(string name, SPWeb web)
        {
            SPSite spSite = new SPSite(web.Url);
            SPWeb spWeb = spSite.RootWeb;
            try
            {
                SPContentType contentType = spWeb.AvailableContentTypes[name];
                return contentType;
            }
            catch (Exception ee)
            {
                EssnLog.logInfo(String.Format("Error in GetContentTypeByName with Name {0} and SpWeb {1}.", name, spWeb));
                EssnLog.logExc(ee);
                return null;
            }
        }

        public static void AddWebPartsAndConfig(string nameAndPathWelcomePage, List<WebPartDriskolls> webPerts, SPWeb spWeb, string sourceId, string contentSearchPath)
        {
            try
            {
                using (SPLimitedWebPartManager manager = spWeb.GetLimitedWebPartManager(spWeb.Url + "/" + nameAndPathWelcomePage, PersonalizationScope.Shared))
                {
                    foreach (WebPartDriskolls wpDriskolls in webPerts)
                    {
                        ContentBySearchWebPart contSearchWebPart = new ContentBySearchWebPart();
                        contSearchWebPart.Title = wpDriskolls.Name;
                        switch (wpDriskolls.Params["ChromeType"])
                        {
                            case "None":
                                contSearchWebPart.ChromeType = PartChromeType.None;
                                break;
                            case "Title":
                                contSearchWebPart.ChromeType = PartChromeType.TitleOnly;
                                break;
                            default:
                                contSearchWebPart.ChromeType = PartChromeType.None;
                                break;
                        }

                        contSearchWebPart.NumberOfItems = int.Parse(wpDriskolls.Params["NumberOfItems"]);
                        contSearchWebPart.RenderTemplateId = contentSearchPath + wpDriskolls.Params["TemplateControl"] + ".js";
                        contSearchWebPart.ItemTemplateId = contentSearchPath + wpDriskolls.Params["ItemTemplateId"] + ".js";
                        var querySettings = new DataProviderScriptWebPart
                        {
                            PropertiesJson = contSearchWebPart.DataProviderJSON
                        };
                        //setting the search query text
                        querySettings.Properties["QueryTemplate"] = wpDriskolls.Params["Custom Query"];
                        querySettings.Properties["FallbackSortJson"] = wpDriskolls.Params["FallbackSortJson"];
                        querySettings.Properties["SourceID"] = sourceId;
                        contSearchWebPart.DataProviderJSON = querySettings.PropertiesJson;
                        manager.AddWebPart(contSearchWebPart, wpDriskolls.Params["wpZone"], 1);
                    }
                }
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on AddWebPartsAndConfig in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }

        public static string AddCustomWebPartsWithCustomProperty(SPWeb web, string pageUrl, string webPartName, string wpZone, int zoneIndex, PartChromeType chrome, Dictionary<string, object> customPropertiesDictionary)
        {
            try
            {
                using (SPLimitedWebPartManager manager = web.GetLimitedWebPartManager(pageUrl, PersonalizationScope.Shared))
                {
                    using (System.Web.UI.WebControls.WebParts.WebPart webPart = FindWebPart(web, webPartName, manager, "Web Part Gallery"))
                    {
                        if (webPart == null) return null;//check if webpart exist
                        webPart.ChromeType = chrome;

                        PropertyInfo[] pinProperties = webPart.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        foreach (PropertyInfo pinProperty in pinProperties)
                        {
                            foreach (var customProperty in customPropertiesDictionary)
                            {
                                if (pinProperty.Name == customProperty.Key)
                                {
                                    pinProperty.SetValue(webPart, customProperty.Value, null);
                                }
                            }

                        }
                        manager.AddWebPart(webPart, wpZone, zoneIndex);

                        return webPart.ID;
                    }
                }
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on AddCustomWebPartsWithCustomProperty in FeatureActivated.");
                EssnLog.logExc(ee);
                return null;
            }
        }

        public static WebPart FindWebPart(SPWeb web, string webPartName, SPLimitedWebPartManager manager, string webPartListName)
        {
            try
            {
                SPSite spSite = new SPSite(web.Url);
                SPWeb spWeb = spSite.RootWeb;

                SPQuery query = new SPQuery();
                query.Query = String.Format(CultureInfo.CurrentCulture,
                    "<Where><Eq><FieldRef Name='FileLeafRef'/><Value Type='File'>{0}</Value></Eq></Where>",
                    webPartName);

                SPList webPartGallery = spWeb.Lists[webPartListName];
                if (null == spWeb.ParentWeb)
                {
                    webPartGallery = spWeb.GetCatalog(
                       SPListTemplateType.WebPartCatalog);
                }
                else
                {
                    webPartGallery = spWeb.Site.RootWeb.GetCatalog(
                       SPListTemplateType.WebPartCatalog);
                }
                SPListItemCollection webParts = webPartGallery.GetItems(query);

                if (webParts.Count == 0)
                {
                    EssnLog.logInfo(String.Format("Web part with name {0} is not exist on SpWeb {1}", webPartName, spWeb.Url));
                    return null; //check if webpart exist
                }

                XmlReader xmlReader = new XmlTextReader(webParts[0].File.OpenBinaryStream());
                string errorMessage;
                WebPart webPart = manager.ImportWebPart(xmlReader, out errorMessage);
                webPart.ChromeType = PartChromeType.BorderOnly;
                return webPart;
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on FindWebPart in FeatureActivated.");
                EssnLog.logExc(ee);
                return null;
            }
        }

        public static void AddWebPartContentEditor(SPWeb web, string pageUrl, string zoneID, int zoneIndex, string testForContentEditor, string link, PartChromeType cromeType, string title)
        {
            try
            {
                ContentEditorWebPart contentEditor = new ContentEditorWebPart();
                XmlDocument xmlDoc = new XmlDocument();
                XmlElement xmlElement = xmlDoc.CreateElement("HtmlContent");
                xmlElement.InnerText = testForContentEditor;
                contentEditor.Content = xmlElement;
                using (SPLimitedWebPartManager manager =
                  web.GetLimitedWebPartManager(pageUrl, PersonalizationScope.Shared))
                {
                    contentEditor.ChromeType = cromeType;
                    contentEditor.ContentLink = link;
                    contentEditor.Title = title;
                    manager.AddWebPart(contentEditor, zoneID, zoneIndex);
                }
            }
            catch (Exception ee)
            {
                EssnLog.logInfo("Error on AddWebPartContentEditor in FeatureActivated.");
                EssnLog.logExc(ee);
            }
        }
    }
    public class SiteColumnCustom
    {
        public string Name;
        public SPFieldType ColumnType;
        public string Group;
        public StringCollection Choices;
        public bool DateOnly;

        public SiteColumnCustom(string name, SPFieldType columnType, string group, string[] choices = null, bool dateOnly = false)
        {
            Name = name;
            ColumnType = columnType;
            Group = group;
            DateOnly = dateOnly;
            if (choices != null)
            {
                StringCollection choiceCollection = new StringCollection();
                choiceCollection.AddRange(choices);
                Choices = choiceCollection;
            }
        }
    }

    public class ListCustom
    {
        public string Name;
        public SPListTemplateType ListTemplateType;
        public List<SPField> Fields;

        public ListCustom(string name, SPListTemplateType listTemplateType, List<SPField> fields = null)
        {
            Name = name;
            ListTemplateType = listTemplateType;
            Fields = fields;
        }
    }

    public class WebPartDriskolls
    {
        public string Name;
        public Dictionary<string, string> Params;

        public WebPartDriskolls(string name, Dictionary<string, string> parameters)
        {
            Name = name;
            Params = parameters;
        }
    }

    /// <summary>
    /// Defines extensions made to the <see cref="SPWeb"/> class
    /// </summary>
    [CLSCompliant(false)]
    public static class SpWebExtensions
    {
        public static bool IsFeatureActivated(this SPWeb web, Guid featureId)
        {
            return web.Site.Features[featureId] != null;
        }

        public static bool IsFeatureInstalled(Guid featureId)
        {
            SPFeatureDefinition featureDefinition = SPFarm.Local.FeatureDefinitions[featureId];
            if (featureDefinition == null)
            {
                return false;
            }

            if (featureDefinition.Scope != SPFeatureScope.Web)
            {
                //     string.Format("Feature with the ID {0} was installed but is not scoped at the web level.", featureId));
            }

            return true;
        }
    }
}