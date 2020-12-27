using System.Collections.Generic;

namespace KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities
{
    class Collection
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public List<Item> Items { get; private set; }

        public string Url { get => $"https://steamcommunity.com/sharedfiles/filedetails/?id={Id}"; }

        public int ItemCount { get => Items != null ? Items.Count : 0; }

        public Collection(string id, string name, List<Item> items)
        {
            Id = id;
            Name = name;
            Items = items;
        }
    }
}
