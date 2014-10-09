using Android.Content;
using Android.Database;
using Android.Provider;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CursorAdapter = Android.Support.V4.Widget.CursorAdapter;

namespace ContactApp
{
    public class ContactCursorAdapter : BaseAdapter, ISectionIndexer
    {
        private Context _context;
        private AlphabetIndexer _indexer;
        private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private List<ContactApp.MainActivity.Contact> _contacts;

        public ContactCursorAdapter(Context context, List<ContactApp.MainActivity.Contact> contacts)
        {
            _context = context;
            _contacts = contacts;

            _indexer = new AlphabetIndexer(null, ContactsQuery.SORT_KEY, alphabet);
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = LayoutInflater.From(_context).Inflate(Resource.Layout.ContactLayout, parent, false);

            var name = view.FindViewById<TextView>(Resource.Id.ContactName);
            var thumbnail = view.FindViewById<QuickContactBadge>(Resource.Id.ContactBadge);

            var contact = _contacts[position];

            name.Text = contact.DisplayName;

            if (contact.Thumbnail == null)
            {
                thumbnail.SetImageResource(Android.Resource.Drawable.SymContactCard);
            }
            else
            {
                thumbnail.SetImageURI(contact.Thumbnail);
                //_imageCache.LoadImage(photoUri, thumbnail);
            }
            return view;
        }

        public void SwapCursor(List<ContactApp.MainActivity.Contact> displayNames)
        {
            _contacts = displayNames;
            NotifyDataSetChanged();
        }

        public override int Count
        {
            get
            {
                return _contacts != null ? _contacts.Count : 0;
            }
        }

        public int GetPositionForSection(int section)
        {
            return _indexer.GetPositionForSection(section);
        }

        public int GetSectionForPosition(int position)
        {
            return _indexer.GetSectionForPosition(position);
        }

        public Java.Lang.Object[] GetSections()
        {
            return _indexer.GetSections();
        }


    }
}
