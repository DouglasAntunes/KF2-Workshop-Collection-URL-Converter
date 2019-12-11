using KF2WorkshopUrlConverter.Core.KF2ServerUtils;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace KF2WorkshopUrlConverter.Test
{
    [TestFixture]
    [Description("Tests related to KF2WorkshopUrlConverter.Core.KF2Utils.WorkshopCollectionListBuilder")]
    class WorkshopCollectionListBuilderTest
    {
        private WorkshopCollectionListBuilder collectionListBuilder;

        [SetUp]
        public void Setup()
        {
            collectionListBuilder = new WorkshopCollectionListBuilder();
        }

        [Test]
        [Description("Trys to generate a list with default values, expecting a empty string as result.")]
        public void GenerateEmptyListWithNoCollection()
        {
            var result = collectionListBuilder.Build();
            Assert.IsEmpty(result);
        }

        [Test]
        [Description("Trys to generate a list providing a collection of 1 item and not providing header, footer or an item format, expecting the default result.")]
        public void GenerateListWithoutHeaderFooterOrFormat()
        {
            var collection = new Collection("a", "b", new List<Item>()
            {
                new Item("AnId", "RandomName")
            });
            var result = collectionListBuilder.WithCollection(collection).Build();
            var resultExpected = $"RandomName https://steamcommunity.com/sharedfiles/filedetails/?id=AnId" + Environment.NewLine;

            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.Equals(resultExpected));
        }

        [Test]
        [Description("Trys to generate a list providing a collection of 1 item, an item format and not providing header or footer, expecting a custom result.")]
        public void GenerateListWithoutHeaderOrFooter()
        {
            var collection = new Collection("a", "b", new List<Item>()
            {
                new Item("AnId", "RandomName")
            });
            var result = collectionListBuilder.WithCollection(collection).WithFormat("{2} {1} {0}").Build();
            var resultExpected = "https://steamcommunity.com/sharedfiles/filedetails/?id=AnId RandomName AnId" + Environment.NewLine;

            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.Equals(resultExpected));
        }
    }
}
