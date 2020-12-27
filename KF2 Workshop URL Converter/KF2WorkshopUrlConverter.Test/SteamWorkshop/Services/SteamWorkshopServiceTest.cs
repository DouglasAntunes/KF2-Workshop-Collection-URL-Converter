using KF2WorkshopUrlConverter.Core.SteamWorkshop.Exceptions;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Services;
using NUnit.Framework;
using System;

namespace KF2WorkshopUrlConverter.Test.SteamWorkshop.Services
{
    [TestFixture, Description("Tests related to KF2WorkshopUrlConverter.Core.SteamWorkshop.Services.SteamWorkshopService")]
    public class SteamWorkshopServiceTest
    {
        private readonly SteamWorkshopService workshopService = new SteamWorkshopService();

        [Test, Description("Trys to check if is a workshop item from url. Must throw NotImplemented")]
        public void ErrorIsWorkshopItemNotImplemented()
        {
            Assert.Throws<NotImplementedException>(() => SteamWorkshopService.IsASteamWorkshopItem(""));
        }

        [Test, Description("Trys to check if is a Steam Workshop collection with a url that doesn't have protocol.")]
        public void ExceptionOnIsASteamWorkshopCollectionWithURLWithoutProtocol()
        {
            var testUrl = "steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            Assert.Throws<UriFormatException>(() => SteamWorkshopService.IsASteamWorkshopCollection(testUrl));
        }

        [Test, Description("Trys to check if is a Steam Workshop collection with a valid url, but isn't steamcommunity.com")]
        public void SuccessIsASteamWorkshopCollectionWithRandomURL()
        {
            var testUrl = "https://google.com";
            Assert.IsFalse(SteamWorkshopService.IsASteamWorkshopCollection(testUrl));
        }

        [Test, Description("Trys to check if is a Steam Workshop collection with a valid url that isn't a collection url")]
        public void SuccessIsASteamWorkshopCollectionWithItemURL()
        {
            var testUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=650252240";
            Assert.IsFalse(SteamWorkshopService.IsASteamWorkshopCollection(testUrl));
        }

        [Test, Description("Trys to check if is a Steam Workshop collection with a valid url that is a collection url")]
        public void SuccessIsASteamWorkshopCollectionWithCollectionURL()
        {
            var testUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            Assert.IsTrue(SteamWorkshopService.IsASteamWorkshopCollection(testUrl));
        }

        [Test, Description("Trys to fetch a collection with an invalid url and throws a NotASteamWorkshopUrlException.")]
        public void FailsToFetchCollectionFromInvalidUrlAndThrowAnException()
        {
            var testUrl = TestContext.CurrentContext.Random.GetString();
            Assert.Throws<NotASteamWorkshopUrlException>(() => workshopService.FetchCollectionFromURL(testUrl));
        }

        [Test, Description("Trys to fetch a collection with a valid url, but not a SteamWorkshop Url and throws a NotASteamWorkshopUrlException.")]
        public void FailsToFetchCollectionFromValidUrlButNotASteamWorkshopUrlAndThrowAnException()
        {
            var testUrl = "https://google.com.br";
            Assert.Throws<NotASteamWorkshopUrlException>(() => workshopService.FetchCollectionFromURL(testUrl));
        }

        [Test, Description("Trys to fetch a collection with a valid SteamWorkshop URL, but without protocol and throws an UriFormatException.")]
        public void FailsToFetchCollectionFromValidSteamWorkshopUrlWithoutProtocolAndThrowAnException()
        {
            var testUrl = "steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            Assert.Throws<UriFormatException>(() => workshopService.FetchCollectionFromURL(testUrl));
        }

        [Test, Description("Trys to fetch a collection with a Item SteamWorkshop URL and throws a NotACollectionException.")]
        public void FailsToFetchCollectionFromASteamWorkshopItemUrlAndThrowAnException()
        {
            var testUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=650252240";
            Assert.Throws<NotACollectionException>(() => workshopService.FetchCollectionFromURL(testUrl));
        }

        [Test, Description("Trys to fetch the Example Collection from SteamWorkshop and expects: 1 item, correct name & correct url from an item.")]
        public void SucceedToFetchTheProjectExampleCollection()
        {
            var testUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            var collection = workshopService.FetchCollectionFromURL(testUrl);
            Assert.IsTrue(collection.ItemCount == 1);
            Assert.IsTrue(collection.Name.Equals("Map Collection Example"));
            Assert.IsTrue(collection.Items[0].Url.Equals("https://steamcommunity.com/sharedfiles/filedetails/?id=650252240"));
        }

        [Test, Description("Trys to fetch a workshop item from url. Must throw NotImplemented")]
        public void ErrorFetchItemFromURLNotImplemented()
        {
            Assert.Throws<NotImplementedException>(() => workshopService.FetchItemFromUrl(""));
        }
    }
}
