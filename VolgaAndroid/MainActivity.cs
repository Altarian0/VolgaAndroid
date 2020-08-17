using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.Widget;
using VolgaAndroid.Models;
using System.Collections.Generic;
using VolgaAndroid.Helper;
using System;

namespace VolgaAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private RecyclerView RepositoryList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            InitializeComponent();
            FillReposList();
        }

        private void InitializeComponent()
        {
            RepositoryList = FindViewById<RecyclerView>(Resource.Id.RepositoryList);
        }
        
        private async void FillReposList()
        {
            GitHubApiMethod github = new GitHubApiMethod();

            List<GitRepository> repositories =await github.GetRepositories();
            LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);

            RepositoryList.SetLayoutManager(linearLayoutManager);
            RepositoryList.SetAdapter(new RepositoryListAdapter(this, repositories));
        }
    }
}