using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Database;
using Android.Provider;

namespace ContactApp
{
    [Activity(Label = "ContactApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, LoaderManager.ILoaderCallbacks
    {
        private ListView _list;
        private ContactCursorAdapter _adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _list = FindViewById<ListView>(Resource.Id.ViewRecipients_List);
            _adapter = new ContactCursorAdapter(this, new List<Contact>());
            _list.Adapter = _adapter;

            LoaderManager.InitLoader(ContactsQuery.QUERY_ID, null, this);
        }

        public Loader OnCreateLoader(int id, Bundle args)
        {
            if (id == ContactDisplayQuery.QUERY_ID)
            {
                try
                {
                    return new CursorLoader(this, ContactDisplayQuery.CONTENT_URI, ContactDisplayQuery.PROJECTION, ContactDisplayQuery.SELECTION, null, ContactsContract.Data.InterfaceConsts.DisplayName);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Error: " + exc.StackTrace);
                }
            }
            else if (id == ContactsQuery.QUERY_ID)
            {
                if (id == ContactsQuery.QUERY_ID)
                {
                    Android.Net.Uri uri;

                    uri = ContactsQuery.CONTENT_URI;

                    return new CursorLoader(this, uri, ContactsQuery.PROJECTION, ContactsQuery.SELECTION, null, ContactsQuery.SORT_ORDER);
                }
            }
            return null;
        }

        public void OnLoadFinished(Loader loader, Java.Lang.Object data)
        {
            var cursor = (ICursor)data;
            List<Contact> contacts = new List<Contact>();

            cursor.MoveToFirst();
            if (loader.Id == ContactDisplayQuery.QUERY_ID)
            {
                while (!cursor.IsAfterLast)
                {
                    string photoUri = cursor.GetString(ContactDisplayQuery.PHOTO_THUMBNAIL_DATA);
                    string displayName = cursor.GetString(ContactDisplayQuery.DISPLAY_NAME);
                    string id = cursor.GetString(ContactDisplayQuery.ID);
                    string mimeType = cursor.GetString(ContactDisplayQuery.Mimetype);
                    string firstName = cursor.GetString(ContactDisplayQuery.GivenName);
                    string lastName = cursor.GetString(ContactDisplayQuery.FamilyName);
                    Android.Net.Uri path = null;

                    if (mimeType == ContactsContract.CommonDataKinds.Phone.ContentItemType)
                    {
                        var person = ContentUris.WithAppendedId(ContactDisplayQuery.CONTENT_URI, cursor.GetLong(ContactDisplayQuery.ID));
                        path = Android.Net.Uri.WithAppendedPath(person, ContactsContract.Contacts.Photo.ContentDirectory);
                    }

                    var contact = new Contact
                    {
                        Id = id,
                        Thumbnail = !string.IsNullOrEmpty(photoUri) ? path : null,
                        DisplayName = displayName
                    };

                    if (!contacts.Any(c => c.DisplayName == contact.DisplayName))
                        contacts.Add(contact);
                    cursor.MoveToNext();
                }
            }
            else if (loader.Id == ContactsQuery.QUERY_ID)
            {
                while (!cursor.IsAfterLast)
                {
                    string photoUri = cursor.GetString(ContactsQuery.PHOTO_THUMBNAIL_DATA);
                    string displayName = cursor.GetString(ContactsQuery.DISPLAY_NAME);
                    string id = cursor.GetString(ContactsQuery.ID);

                    var newCursor = ContentResolver.Query(ContactDisplayQuery.CONTENT_URI, ContactDisplayQuery.PROJECTION, ContactDisplayQuery.SELECTION, new string[] { id }, ContactDisplayQuery.SORT_ORDER);
                    if (newCursor != null && newCursor.MoveToFirst())
                    {
                        var first = newCursor.GetString(ContactDisplayQuery.GivenName);
                        var last = newCursor.GetString(ContactDisplayQuery.FamilyName);

                        var person = ContentUris.WithAppendedId(ContactsQuery.CONTENT_URI, cursor.GetLong(ContactsQuery.ID));
                        var path = Android.Net.Uri.WithAppendedPath(person, ContactsContract.Contacts.Photo.ContentDirectory);

                        var contact = new Contact
                        {
                            Id = id,
                            Thumbnail = !string.IsNullOrEmpty(photoUri) ? path : null,
                            DisplayName = displayName
                        };

                        if (!contacts.Contains(contact))
                        {
                            contacts.Add(contact);
                        }
                    }

                    cursor.MoveToNext();
                }
            }
            _adapter.SwapCursor(contacts);
        }

        public void OnLoaderReset(Loader loader)
        {
            _adapter.SwapCursor(null);
        }

        public class Contact
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public Android.Net.Uri Thumbnail { get; set; }
        }
    }
}

