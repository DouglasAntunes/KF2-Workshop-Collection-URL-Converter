﻿using KF2WorkshopUrlConverter.Core.SteamWorkshop.Entities;
using System.Text;

namespace KF2WorkshopUrlConverter.Core.KF2ServerUtils
{
    class WorkshopCollectionListBuilder
    {
        private string _header;
        private string _footer;
        private Collection _collection;
        private string _format;

        public WorkshopCollectionListBuilder()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            _header = string.Empty;
            _footer = string.Empty;
            _collection = null;
            _format = "{1} {2}";
        }

        public WorkshopCollectionListBuilder WithCollection(Collection collection)
        {
            _collection = collection;
            return this;
        }

        public WorkshopCollectionListBuilder WithHeader(string header)
        {
            _header = header;
            return this;
        }

        public WorkshopCollectionListBuilder WithFooter(string footer)
        {
            _footer = footer;
            return this;
        }

        /// <summary>
        ///     Provides a format to each of the Collection Items.
        /// </summary>
        /// <param name="format">
        ///     Format String with:
        ///         {0} Id of the Item
        ///         {1} Name of the Item
        ///         {2} Url of the Item
        /// </param>
        public WorkshopCollectionListBuilder WithFormat(string format)
        {
            _format = format;
            return this;
        }

        public string Build()
        {
            if (_collection == null)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();

            if (!_header.Equals(string.Empty))
            {
                sb.AppendLine(_header);
            }

            foreach (var item in _collection.Items)
            {
                sb.AppendFormat(_format, item.Id, item.Name, item.Url).AppendLine();
            }

            if (!_footer.Equals(string.Empty))
            {
                sb.AppendLine(_footer);
            }
            return sb.ToString();
        }
    }
}
