using KF2WorkshopUrlConverter.Core.SteamWorkshop.Services;
using NUnit.Framework;

namespace KF2WorkshopUrlConverter.Test.Services
{
    [TestFixture]
    public class SteamWorkshopServiceTest
    {
        private readonly SteamWorkshopService workshopService = new SteamWorkshopService();

        [Test]
        public void FailsToFetchCollectionFromInvalidUrlAndThrowAnException()
        {
            var testUrl = "https://www.google.com.br";
            Assert.That(() => workshopService.FetchCollectionFromURL(testUrl), Throws.Exception);
        }
    }
}
