using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Models
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem
    {
        public SampleDataItem(string uniqueId, string title, string subtitle, string imagePath, string description, string content)
        {
            UniqueId = uniqueId;
            Title = title;
            Subtitle = subtitle;
            Description = description;
            ImagePath = imagePath;
            Content = content;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public string Content { get; private set; }

        public override string ToString()
        {
            return Title;
        }
    }    
}
