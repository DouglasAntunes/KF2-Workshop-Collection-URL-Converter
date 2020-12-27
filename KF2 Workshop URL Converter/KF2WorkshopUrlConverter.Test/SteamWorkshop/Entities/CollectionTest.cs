using KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities;
using NUnit.Framework;
using System.Collections.Generic;

namespace KF2WorkshopUrlConverter.Test.SteamWorkshop.Entities
{
    [TestFixture]
    class CollectionTest
    {
        [Test, Description("Tests the attributes after creating class with no items")]
        public void TestAttributesWithNoItems()
        {
            string id = TestContext.CurrentContext.Random.GetString();
            string name = TestContext.CurrentContext.Random.GetString();

            Collection collection = new Collection(id, name, null);
            Assert.AreEqual(collection.Id, id);
            Assert.AreEqual(collection.Name, name);

            Assert.AreEqual(collection.Url, $"https://steamcommunity.com/sharedfiles/filedetails/?id={id}");

            Assert.IsNull(collection.Items);
            Assert.AreEqual(collection.ItemCount, 0);
        }

        [Test, Description("Tests the attributes after creating class with items")]
        public void TestAttributesWithItems()
        {
            string id = TestContext.CurrentContext.Random.GetString();
            string name = TestContext.CurrentContext.Random.GetString();
            string itemId = TestContext.CurrentContext.Random.GetString();
            string itemName = TestContext.CurrentContext.Random.GetString();
            List<Item> items = new List<Item>()
            {
                new Item(itemId, itemName)
            };

            Collection collection = new Collection(id, name, items);
            Assert.AreEqual(collection.Id, id);
            Assert.AreEqual(collection.Name, name);

            Assert.AreEqual(collection.Url, $"https://steamcommunity.com/sharedfiles/filedetails/?id={id}");

            Assert.IsNotNull(collection.Items);
            Assert.IsNotEmpty(collection.Items);
            Assert.IsTrue(collection.ItemCount == 1);
            Assert.AreEqual(collection.Items[0].Id, itemId);
            Assert.AreEqual(collection.Items[0].Name, itemName);
        }
    }
}
