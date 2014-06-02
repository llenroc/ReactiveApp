using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;
using TestApp.Models;

namespace TestApp.ViewModels
{
    public class ItemViewModel : ReactiveObject
    {
        private readonly SampleDataItem item;

        public ItemViewModel(SampleDataItem item)
        {
            this.item = item;

            BitmapLoader.Current.LoadFromResource("ms-appx:///" + item.ImagePath, null, null).ToObservable().ToProperty(this, x => x.Image, out this.image, null, RxApp.MainThreadScheduler);
        }

        public string Title { get { return item.Title; } }

        public string Subtitle { get { return item.Subtitle; } }

        public string Description { get { return item.Description; } }

        private ObservableAsPropertyHelper<IBitmap> image;
        public IBitmap Image
        {
            get { return image != null ? image.Value : null; }
        }
    }
}
