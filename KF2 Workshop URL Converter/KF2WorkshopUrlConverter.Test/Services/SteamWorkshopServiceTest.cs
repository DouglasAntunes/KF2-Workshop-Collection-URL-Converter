using KF2WorkshopUrlConverter.Core.SteamWorkshop.Exceptions;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Services;
using NUnit.Framework;
using System;

namespace KF2WorkshopUrlConverter.Test.Services
{
    [TestFixture]
    [Description("Tests related to KF2WorkshopUrlConverter.Core.SteamWorkshop.Services.SteamWorkshopService")]
    public class SteamWorkshopServiceTest
    {
        private readonly SteamWorkshopService workshopService = new SteamWorkshopService();

        [Test]
        [Description("Trys to fetch a collection with an invalid url and throws a NotASteamWorkshopUrlException.")]
        public void FailsToFetchCollectionFromInvalidUrlAndThrowAnException()
        {
            var testUrl = "randomTextAndNotAUrl";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.TypeOf<NotASteamWorkshopUrlException>());
        }

        [Test]
        [Description("Trys to fetch a collection with a valid url, but not a SteamWorkshop Url and throws a NotASteamWorkshopUrlException.")]
        public void FailsToFetchCollectionFromValidUrlButNotASteamWorkshopUrlAndThrowAnException()
        {
            var testUrl = "https://google.com.br";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.TypeOf<NotASteamWorkshopUrlException>());
        }

        [Test]
        [Description("Trys to fetch a collection with a valid SteamWorkshop URL, but without protocol and throws an UriFormatException.")]
        public void FailsToFetchCollectionFromValidSteamWorkshopUrlWithoutProtocolAndThrowAnException()
        {
            var testUrl = "steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.TypeOf<UriFormatException>());
        }

        [Test]
        [Description("Trys to fetch a collection with a Item SteamWorkshop URL and throws a NotACollectionException.")]
        public void FailsToFetchCollectionFromASteamWorkshopItemUrlAndThrowAnException()
        {
            var testUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=650252240";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.TypeOf<NotACollectionException>());
        }

        [Test]
        [Description("Trys to fetch the Example Collection from SteamWorkshop and expects: 1 item, correct name & correct url from an item.")]
        public void SucceedToFetchTheProjectExampleCollection()
        {
            var testUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417820";
            var collection = workshopService.FetchCollectionFromURL(testUrl);
            Assert.IsTrue(collection.ItemCount == 1);
            Assert.IsTrue(collection.Name.Equals("Map Collection Example"));
            Assert.IsTrue(collection.Items[0].Url.Equals("https://steamcommunity.com/sharedfiles/filedetails/?id=650252240"));
        }
    }
}
