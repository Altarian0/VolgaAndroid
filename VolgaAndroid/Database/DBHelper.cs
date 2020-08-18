using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using VolgaAndroid.Helper;
using VolgaAndroid.Models;

namespace VolgaAndroid.Database
{
    public class DBHelper : SQLiteOpenHelper
    {
        private Context Context;
        public DBHelper(Context context) : base(context, "VolgaAndroid.db", null, 1)
        { 
            this.Context = context;
        }

        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL("CREATE TABLE Favorite (Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                           "Name TEXT, " +
                                                           "ReposId INT, " +
                                                           "Description TEXT, " +
                                                           "OwnerLogin TEXT, " +
                                                           "AvatarUrl TEXT, " +
                                                           "AvatarImage TEXT)");

        }

        public void AddRepos(GitRepository repository)
        {
            SQLiteDatabase db = this.WritableDatabase;

            ContentValues content = new ContentValues();
            content.Put("Name", repository.name);
            content.Put("ReposId", repository.id);
            content.Put("Description", repository.description);
            content.Put("OwnerLogin", repository.owner.login);
            content.Put("AvatarImage", repository.owner.avatar_image);
            content.Put("AvatarUrl", repository.owner.avatar_url);

            Bitmap bitmap;
            using (WebClient client = new WebClient())
            {
                var array = client.DownloadData(repository.owner.avatar_url);
                bitmap = BitmapFactory.DecodeByteArray(array, 0, array.Length);
            }

            using (FileStream stream = new FileStream(repository.owner.avatar_image, FileMode.Create))
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            }

            db.InsertOrThrow("Favorite", null, content);

            Toast.MakeText(Context, "Successfully added!", ToastLength.Short).Show();
        }

        public void RemoveRepos(int id)
        {
            SQLiteDatabase db = this.WritableDatabase;

            db.Delete("Favorite", "ReposId = "+id, null);

            Toast.MakeText(Context, "Successfully removed!", ToastLength.Short).Show();
        }

        public List<GitRepository> GetFavorite()
        {
            SQLiteDatabase db = this.WritableDatabase;
            List<GitRepository> repositories = new List<GitRepository>();
            ICursor cursor = db.RawQuery("SELECT * FROM Favorite", null);

            while (cursor.MoveToNext())
            {
                Owner owner = new Owner
                {
                    login = cursor.GetString(cursor.GetColumnIndex("OwnerLogin")),
                    avatar_url = cursor.GetString(cursor.GetColumnIndex("AvatarUrl")),
                    avatar_image = cursor.GetString(cursor.GetColumnIndex("AvatarImage"))
                };

                GitRepository repository = new GitRepository
                {
                    name = cursor.GetString(cursor.GetColumnIndex("Name")),
                    id = int.Parse(cursor.GetString(cursor.GetColumnIndex("ReposId"))),
                    description = cursor.GetString(cursor.GetColumnIndex("Description")),
                    owner = owner
                };

                repositories.Add(repository);
            }

            return repositories;
        }
        private byte[] GetImageFromUrl(string url)
        {
            using (WebClient wc = new WebClient())
            {
                return wc.DownloadData(url);
            }
        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            throw new NotImplementedException();
        }
    }
}