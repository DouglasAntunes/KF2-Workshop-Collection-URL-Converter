namespace KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities
{
    class Item
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        public string Url { get => $"https://steamcommunity.com/sharedfiles/filedetails/?id={Id}"; }

        public Item(string ID, string Name)
        {
            this.Id = ID;
            this.Name = Name;
        }
    }
}
