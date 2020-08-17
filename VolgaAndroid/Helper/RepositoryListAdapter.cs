using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using VolgaAndroid.Models;

namespace VolgaAndroid.Helper
{
    public class RepositoryListViewHolder : RecyclerView.ViewHolder
    {
        public TextView NameText;
        public TextView DescriptionText;
        public ImageView ImageLogo;
        public RepositoryListViewHolder(View itemView) : base(itemView)
        {
            NameText = itemView.FindViewById<TextView>(Resource.Id.NameText);
            DescriptionText = itemView.FindViewById<TextView>(Resource.Id.NameText);
            ImageLogo = itemView.FindViewById<ImageView>(Resource.Id.ImageLogo);
        }
    }

    public class RepositoryListAdapter : RecyclerView.Adapter
    {
        private List<GitRepository> GitRepositories;

        public RepositoryListAdapter(Context context, List<GitRepository> gitRepositories)
        {
            this.GitRepositories = gitRepositories;
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
            view.NameText.Text = repos.owner.login;
            view.DescriptionText.Text = repos.owner.login;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new RepositoryListViewHolder(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.repository_temp, parent, false));
        }

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
    }
}