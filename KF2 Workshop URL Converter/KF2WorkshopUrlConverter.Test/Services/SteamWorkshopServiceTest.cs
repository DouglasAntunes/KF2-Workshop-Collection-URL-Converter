using KF2WorkshopUrlConverter.Core.SteamWorkshop.Exceptions;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Services;
using NUnit.Framework;
using System;

namespace KF2WorkshopUrlConverter.Test.Services
{
    [TestFixture]
    public class SteamWorkshopServiceTest
    {
        private readonly SteamWorkshopService workshopService = new SteamWorkshopService();

        [Test]
        public void FailsToFetchCollectionFromInvalidUrlAndThrowAnException()
        {
            var testUrl = "randomTextAndNotAUrl";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.TypeOf<NotASteamWorkshopUrlException>());
        }

        [Test]
        public void FailsToFetchCollectionFromValidUrlButNotASteamWorkshopUrlAndThrowAnException()
        {
            var testUrl = "https://google.com.br";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.TypeOf<NotASteamWorkshopUrlException>());
        }

        [Test]
        public void FailsToFetchCollectionFromValidSteamWorkshopUrlWithoutProtocolAndThrowAnException()
        {
            var testUrl = "steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.TypeOf<UriFormatException>());
        }

        [Test]
        public void FailsToFetchCollectionFromASteamWorkshopItemUrlAndThrowAnException()
        {
            var testUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=650252240";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.TypeOf<NotACollectionException>());
        }

        [Test]
        public void SucceedToFetchTheProjectExampleCollection()
        {
            var testUrl = "https://steamcommunity.com/sharedfiles/filedetails/?id=882417829";
            var collection = workshopService.FetchCollectionFromURL(testUrl);
            Assert.IsTrue(collection.ItemCount == 1);
            Assert.IsTrue(collection.Name.Equals("Map Collection Example"));
            Assert.IsTrue(collection.Items[0].Url.Equals("https://steamcommunity.com/sharedfiles/filedetails/?id=650252240"));
        }


    }
}
