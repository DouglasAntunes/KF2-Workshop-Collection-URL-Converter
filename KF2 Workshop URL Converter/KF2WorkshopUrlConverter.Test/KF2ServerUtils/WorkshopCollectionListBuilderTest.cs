using KF2WorkshopUrlConverter.Core.KF2ServerUtils;
using KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace KF2WorkshopUrlConverter.Test.KF2ServerUtils
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

        [Test, 
         Description("Trys to generate a list providing a header and collection of 1 item and not providing footer or an item format, expecting the default result with a header.")]
        public void GenerateListWithHeader()
        {
            string header = TestContext.CurrentContext.Random.GetString();
            string collectionId = TestContext.CurrentContext.Random.GetString();
            string collectionName = TestContext.CurrentContext.Random.GetString();
            string itemId = TestContext.CurrentContext.Random.GetString();
            string itemName = TestContext.CurrentContext.Random.GetString();
            var collection = new Collection(collectionId, collectionName, new List<Item>()
            {
                new Item(itemId, itemName)
            });
            var result = collectionListBuilder.WithHeader(header).WithCollection(collection).Build();
            var resultExpected = header + Environment.NewLine + $"{itemName} https://steamcommunity.com/sharedfiles/filedetails/?id={itemId}" + Environment.NewLine;
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, resultExpected);
        }

        [Test,
         Description("Trys to generate a list providing a footer and collection of 1 item and not providing header or an item format, expecting the default result with a footer.")]
        public void GenerateListWithFooter()
        {
            string footer = TestContext.CurrentContext.Random.GetString();
            string collectionId = TestContext.CurrentContext.Random.GetString();
            string collectionName = TestContext.CurrentContext.Random.GetString();
            string itemId = TestContext.CurrentContext.Random.GetString();
            string itemName = TestContext.CurrentContext.Random.GetString();
            var collection = new Collection(collectionId, collectionName, new List<Item>()
            {
                new Item(itemId, itemName)
            });
            var result = collectionListBuilder.WithFooter(footer).WithCollection(collection).Build();
            var resultExpected = $"{itemName} https://steamcommunity.com/sharedfiles/filedetails/?id={itemId}" + Environment.NewLine + footer + Environment.NewLine;
            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, resultExpected);
        }

        [Test, Description("Trys to generate a list with default values, expecting a empty string as result.")]
        public void GenerateEmptyListWithNoCollection()
        {
            var result = collectionListBuilder.Build();
            Assert.IsEmpty(result);
        }

        [Test, 
         Description("Trys to generate a list providing a collection of 1 item and not providing header, footer or an item format, expecting the default result.")]
        public void GenerateListWithoutHeaderFooterOrFormat()
        {
            string collectionId = TestContext.CurrentContext.Random.GetString();
            string collectionName = TestContext.CurrentContext.Random.GetString();
            string itemId = TestContext.CurrentContext.Random.GetString();
            string itemName = TestContext.CurrentContext.Random.GetString();
            var collection = new Collection(collectionId, collectionName, new List<Item>()
            {
                new Item(itemId, itemName)
            });
            var result = collectionListBuilder.WithCollection(collection).Build();
            var resultExpected = $"{itemName} https://steamcommunity.com/sharedfiles/filedetails/?id={itemId}" + Environment.NewLine;

            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, resultExpected);
        }

        [Test, 
         Description("Trys to generate a list providing a collection of 1 item, an item format and not providing header or footer, expecting a custom result.")]
        public void GenerateListWithoutHeaderOrFooter()
        {
            string collectionId = TestContext.CurrentContext.Random.GetString();
            string collectionName = TestContext.CurrentContext.Random.GetString();
            string itemId = TestContext.CurrentContext.Random.GetString();
            string itemName = TestContext.CurrentContext.Random.GetString();
            var collection = new Collection(collectionId, collectionName, new List<Item>()
            {
                new Item(itemId, itemName)
            });
            var result = collectionListBuilder.WithCollection(collection).WithFormat("{2} {1} {0}").Build();
            var resultExpected = $"https://steamcommunity.com/sharedfiles/filedetails/?id={itemId} {itemName} {itemId}" + Environment.NewLine;

            Assert.IsNotEmpty(result);
            Assert.AreEqual(result, resultExpected);
        }
    }
}
