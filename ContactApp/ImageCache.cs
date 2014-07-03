using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;

namespace ContactApp
{
    public class ImageCache : IImageUpdated
    {
        private class Lookup
        {
            public ImageView ImageView { get; set; }

            public string UrlPath { get; set; }

            public ArrayAdapter Adapter { get; set;}
        }

        private Dictionary<string, Lookup> _lookup = new Dictionary<string, Lookup>();

        public Drawable LoadImage(string url, ImageView imageView)
        {
            _lookup[url] = new Lookup { ImageView = imageView };
            return ImageLoader.DefaultRequestImage(new Uri(url), this);
        }

        public Drawable LoadImage(string url, ImageView imageView, ArrayAdapter adapter)
        {
            _lookup[url] = new Lookup { ImageView = imageView, UrlPath = url, Adapter = adapter };
            return ImageLoader.DefaultRequestImage(new Uri(url), this);
        }

        public void UpdatedImage(Uri uri)
        {
            Lookup lookup;
            if (_lookup.TryGetValue(uri.ToString(), out lookup))
            {
                lookup.ImageView.SetImageDrawable(ImageLoader.DefaultRequestImage(uri, this));
                if (lookup.Adapter != null)
                {
                    lookup.Adapter.NotifyDataSetChanged();
                }
                else
                {
                    lookup.ImageView.Invalidate();
                }
            }
            _lookup.Remove(uri.ToString());
        }
    }
}