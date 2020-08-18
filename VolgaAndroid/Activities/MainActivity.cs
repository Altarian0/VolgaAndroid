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
using Android.Content;
using VolgaAndroid.Activities;

namespace VolgaAndroid.Activites
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private RecyclerView RepositoryList;
        private Button FavoriteButton;
        private RepositoryListAdapter RepositoryListAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            InitializeComponent();
            FillReposList();
            FavoriteButton.Click += FavoriteButton_Click;
        }
        private void FavoriteButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(FavoriteReposActivity));
            StartActivity(intent);
        }

        /// <summary>
        /// Initialize component at layout
        /// </summary>
        private void InitializeComponent()
        {
            RepositoryList = FindViewById<RecyclerView>(Resource.Id.RepositoryList);
            FavoriteButton = FindViewById<Button>(Resource.Id.FavoriteButton);
        }

        /// <summary>
        /// 
        /// </summary>
        private async void FillReposList()
        {
            try
            {
                GitHubApiMethod github = new GitHubApiMethod();
                List<GitRepository> repositories = await github.GetRepositories();
                LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);

                RepositoryList.SetLayoutManager(linearLayoutManager);
                RepositoryListAdapter = new RepositoryListAdapter(this, repositories);
                RepositoryList.SetAdapter(RepositoryListAdapter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                Toast.MakeText(this, "Network connection failure.", ToastLength.Long).Show();
                return;
            }
        }
    }
}