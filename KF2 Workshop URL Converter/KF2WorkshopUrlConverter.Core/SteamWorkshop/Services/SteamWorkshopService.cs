using HtmlAgilityPack;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Exceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KF2WorkshopUrlConverter.Core.SteamWorkshop.Services
{
    class SteamWorkshopService
    {
        public Collection FetchCollectionFromURL(string url)
        {
            if (!url.Contains("steamcommunity.com/sharedfiles/filedetails/?id="))
            {
                throw new NotASteamWorkshopUrlException("Not a Steam Workshop URL");
            }
            HtmlWeb web = new HtmlWeb();
            HtmlDocument pageDoc;
            try
            {
                pageDoc = web.Load(url);
            }
            catch (UriFormatException e)
            {
                throw new UriFormatException("Must contain http:// or https:// on the URL.", e);
            }
            if (!IsASteamWorkshopCollection(pageDoc))
            {
                throw new NotACollectionException("This url is not a Steam Workshop collection");
            }

            string colId = Regex.Replace(url, @"[^0-9]", string.Empty);
            string colName = pageDoc.DocumentNode.SelectNodes("//div[@class='workshopItemTitle']")[0].InnerText;
            List<Item> colItems = new List<Item>();

            HtmlNodeCollection nodes = pageDoc.DocumentNode.SelectNodes("//div[@class='collectionItemDetails']");
            foreach (HtmlNode n in nodes)
            {
                string ItemUrl = n.SelectSingleNode(".//a").Attributes["href"].Value;
                string ItemID = Regex.Replace(ItemUrl, @"[^0-9]", "");
                string ItemName = n.SelectSingleNode(".//div[@class='workshopItemTitle']").InnerText;
                colItems.Add(new Item(ItemID, ItemName));
            }

            return new Collection(colId, colName, colItems);
        }

        public static bool IsASteamWorkshopCollection(string url)
        {
            if (!url.Contains("steamcommunity.com/sharedfiles/filedetails/?id="))
            {
                return false;
            }
            HtmlWeb web = new HtmlWeb();
            HtmlDocument pageDoc;
            try
            {
                pageDoc = web.Load(url);
            }
            catch (UriFormatException e)
            {
                throw new UriFormatException("Must contain http:// or https:// on the URL.", e);
            }
            return IsASteamWorkshopCollection(pageDoc);
        }

        public static bool IsASteamWorkshopCollection(HtmlDocument document)
        {
            return document.DocumentNode.SelectNodes("//div[@class='collectionItemDetails']") != null;
        }

        public Item FetchItemFromUrl(string url)
        {
            throw new NotImplementedException();
        }

        public static bool IsASteamWorkshopItem(string url)
        {
            throw new NotImplementedException();
        }
    }
}
