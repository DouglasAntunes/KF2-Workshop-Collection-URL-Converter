using KF2WorkshopUrlConverter.Core.KF2ServerUtils;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace KF2WorkshopUrlConverter.Test
{
    [TestFixture]
    class CollectionListBuilderTest
    {
        private WorkshopCollectionListBuilder collectionListBuilder;

        [SetUp]
        public void Setup()
        {
            collectionListBuilder = new WorkshopCollectionListBuilder();
        }

        [Test]
        public void GenerateEmptyListWithNoCollection()
        {
            var result = collectionListBuilder.Build();
            Assert.IsEmpty(result);
        }

        [Test]
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
