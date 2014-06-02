using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Models
{
    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup
    {
        public SampleDataGroup(string uniqueId, string title, string subtitle, string imagePath, string description, IEnumerable<SampleDataItem> items)
        {
            UniqueId = uniqueId;
            Title = title;
            Subtitle = subtitle;
            Description = description;
            ImagePath = imagePath;
            Items = items;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public IEnumerable<SampleDataItem> Items { get; private set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
