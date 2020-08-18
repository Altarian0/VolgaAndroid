using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using VolgaAndroid.Activites;
using VolgaAndroid.Activities;
using VolgaAndroid.Database;
using VolgaAndroid.Models;

namespace VolgaAndroid.Helper
{
    public class RepositoryListViewHolder : RecyclerView.ViewHolder
    {
        public TextView NameText;
        public TextView DescriptionText;
        public TextView AuthorText;
        public ImageView ImageLogo;
        public ImageButton FavoriteButton;
        public RepositoryListViewHolder(View itemView) : base(itemView)
        {
            NameText = itemView.FindViewById<TextView>(Resource.Id.NameText);
            DescriptionText = itemView.FindViewById<TextView>(Resource.Id.DescriptionText);
            AuthorText = itemView.FindViewById<TextView>(Resource.Id.AuthorText);
            ImageLogo = itemView.FindViewById<ImageView>(Resource.Id.ImageLogo);
            FavoriteButton = itemView.FindViewById<ImageButton>(Resource.Id.FavoriteButton);
        }
    }

    public class RepositoryListAdapter : RecyclerView.Adapter
    {
        private List<GitRepository> GitRepositories;
        private Context Context;
        private DBHelper db;
        private List<GitRepository> FavoriteList;

        public RepositoryListAdapter(Context context, List<GitRepository> gitRepositories)
        {
            this.GitRepositories = gitRepositories;
            this.Context = context;
            db = new DBHelper(Context);
            FavoriteList = db.GetFavorite();
        }

        public override int ItemCount
        {
            get
            {
                return GitRepositories.Count();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var repos = GitRepositories[position];
            RepositoryListViewHolder view = holder as RepositoryListViewHolder;

            view.ImageLogo.SetImageBitmap(GetBitmapFromUrl(repos.owner.avatar_url));
            view.NameText.Text = repos.name;
            view.DescriptionText.Text = repos.description;
            view.AuthorText.Text = repos.owner.login;

            FavoriteList = db.GetFavorite();
            bool isFav = FavoriteList.Where(n => n.id == repos.id).ToList().Count() > 0;

            if (isFav)
                view.FavoriteButton.SetImageResource(Resource.Drawable.star_on);
            else
                view.FavoriteButton.SetImageResource(Resource.Drawable.star_off);

            view.FavoriteButton.Click += (s, e) =>
            {
                FavoriteList = db.GetFavorite();

                bool isFav = FavoriteList.Where(n => n.id == repos.id).ToList().Count() > 0;
                if (isFav)
                {
                    view.FavoriteButton.SetImageResource(Resource.Drawable.star_off);
                    db.RemoveRepos(repos.id);

                    try
                    {
                        ((FavoriteReposActivity)Context).FillReposList();
                    }
                    catch
                    {

                    }
                }
                else
                {
                    view.FavoriteButton.SetImageResource(Resource.Drawable.star_on);
                    db.AddRepos(repos);
                }
            };
        }


        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new RepositoryListViewHolder(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.repository_temp, parent, false));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private Bitmap GetBitmapFromUrl(string url)
        {
            Bitmap bitmap;

            using (WebClient wc = new WebClient())
            {
                var imageArray = wc.DownloadData(url);
                bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            }
            return bitmap;
        }
        private Bitmap GetBitmapFromByteArray(byte[] array)
        {
            Bitmap bitmap;

            bitmap = BitmapFactory.DecodeByteArray(array, 0, array.Length);

            return bitmap;
        }
    }
}