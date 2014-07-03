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
using Android.Provider;
using Uri = Android.Net.Uri;

namespace ContactApp
{
    public class ContactsQuery
    {

        // An identifier for the loader
        public static int QUERY_ID = 1;

        // A content URI for the Contacts table
        public static Uri CONTENT_URI = ContactsContract.Contacts.ContentUri;

        // The search/filter query Uri
        public static Uri FILTER_URI = ContactsContract.Contacts.ContentFilterUri;


        public static string SELECTION = ContactsContract.Contacts.InterfaceConsts.DisplayNamePrimary
            + "<>''" + " AND " + ContactsContract.Contacts.InterfaceConsts.InVisibleGroup + "=1";

        public static string SORT_ORDER = ContactsContract.Contacts.InterfaceConsts.SortKeyPrimary;

        public static string[] PROJECTION = {

                // The contact's row id
                ContactsContract.Contacts.InterfaceConsts.Id,

                // A pointer to the contact that is guaranteed to be more permanent than _ID. Given
                // a contact's current _ID value and LOOKUP_KEY, the Contacts Provider can generate
                // a "permanent" contact URI.
                ContactsContract.Contacts.InterfaceConsts.LookupKey,

                // In platform version 3.0 and later, the Contacts table contains
                // DISPLAY_NAME_PRIMARY, which either contains the contact's displayable name or
                // some other useful identifier such as an email address. This column isn't
                // available in earlier versions of Android, so you must use Contacts.DISPLAY_NAME
                // instead.
                ContactsContract.Contacts.InterfaceConsts.DisplayNamePrimary,

                // In Android 3.0 and later, the thumbnail image is pointed to by
                // PHOTO_THUMBNAIL_URI. In earlier versions, there is no direct pointer; instead,
                // you generate the pointer from the contact's ID value and constants defined in
                // android.provider.ContactsContract.Contacts.
                ContactsContract.Contacts.InterfaceConsts.PhotoThumbnailUri,

                // The sort order column for the returned Cursor, used by the AlphabetIndexer
                SORT_ORDER,
        };

        // The query column numbers which map to each value in the projection
        public static int ID = 0;
        public static int LOOKUP_KEY = 1;
        public static int DISPLAY_NAME = 2;
        public static int PHOTO_THUMBNAIL_DATA = 3;
        public static int SORT_KEY = 4;
    }

    public class ContactDisplayQuery
    {
        // An identifier for the loader
        public static int QUERY_ID = 2;

        // A content URI for the Contacts table
        public static Uri CONTENT_URI = ContactsContract.Data.ContentUri;

        // The search/filter query Uri
        public static Uri FILTER_URI = ContactsContract.Contacts.ContentFilterUri;

        public static string SELECTION = ContactsContract.Data.InterfaceConsts.ContactId + " = ? " + " AND " + ContactsContract.Data.InterfaceConsts.InVisibleGroup + " = '1' AND " + ContactsContract.CommonDataKinds.StructuredName.GivenName + " IS NOT NULL AND " + ContactsContract.CommonDataKinds.StructuredName.FamilyName + " IS NOT NULL";

        public static string SORT_ORDER = ContactsContract.Data.InterfaceConsts.SortKeyPrimary;

        public static string[] PROJECTION = {

                // The contact's row id
                ContactsContract.Data.InterfaceConsts.Id,

                // A pointer to the contact that is guaranteed to be more permanent than _ID. Given
                // a contact's current _ID value and LOOKUP_KEY, the Contacts Provider can generate
                // a "permanent" contact URI.
                ContactsContract.Data.InterfaceConsts.LookupKey,
                ContactsContract.Data.InterfaceConsts.DisplayNamePrimary,
                ContactsContract.Data.InterfaceConsts.PhotoThumbnailUri,
                ContactsContract.Data.InterfaceConsts.Mimetype,
                ContactsContract.CommonDataKinds.StructuredName.GivenName,
                ContactsContract.CommonDataKinds.StructuredName.FamilyName,
                ContactsContract.CommonDataKinds.Email.Address,
                ContactsContract.CommonDataKinds.Phone.Number,

                // The sort order column for the returned Cursor, used by the AlphabetIndexer
                SORT_ORDER,
        };

        // The query column numbers which map to each value in the projection
        public static int ID = 0;
        public static int LOOKUP_KEY = 1;
        public static int DISPLAY_NAME = 2;
        public static int PHOTO_THUMBNAIL_DATA = 3;
        public static int Mimetype = 4;
        public static int GivenName = 5;
        public static int FamilyName = 6;
        public static int Address = 7;
        public static int Number = 8;
        public static int SORT_KEY = 9;
    }
}