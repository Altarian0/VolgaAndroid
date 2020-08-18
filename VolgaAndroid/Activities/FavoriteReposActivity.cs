using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using VolgaAndroid.Database;
using VolgaAndroid.Helper;
using VolgaAndroid.Models;

namespace VolgaAndroid.Activities
{
    [Activity(Label = "FavoriteReposActivity")]
    public class FavoriteReposActivity : Activity
    {
        public RecyclerView RepositoryList;
        private RepositoryListAdapter RepositoryListAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.favorite_layout);
            InitializeComponent();
            FillReposList();
        }

        private void InitializeComponent()
        {
            RepositoryList = FindViewById<RecyclerView>(Resource.Id.RepositoryList);
        }

        public void FillReposList()
        {
            RepositoryList.RemoveAllViews();
            DBHelper db = new DBHelper(this);
            List<GitRepository> repositories = db.GetFavorite();
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);

            RepositoryList.SetLayoutManager(linearLayoutManager);
            RepositoryListAdapter = new RepositoryListAdapter(this, repositories);
            RepositoryList.SetAdapter(RepositoryListAdapter);
        }

        public void ListRefresh()
        {
            RepositoryList.SetAdapter(RepositoryListAdapter);
        }
    }
}