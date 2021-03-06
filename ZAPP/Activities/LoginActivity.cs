﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Collections;
using ZAPP.Records;
using Java.Net;
using ZAPP.Services;
using System.Threading.Tasks;

namespace ZAPP.Activities
{
    [Activity(Label = "LoginActivity",
                NoHistory = true)]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);

            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
            EditText username = FindViewById<EditText>(Resource.Id.usernameField);
            EditText password = FindViewById<EditText>(Resource.Id.passwordField);

            loginButton.Click += delegate
            {
                login(username.Text, password.Text);
            };
        }

        async protected void login(string username, string password)
        {
            ProgressBar progress = FindViewById<ProgressBar>(Resource.Id.simpleProgressBar);
            progress.Visibility = ViewStates.Visible;
            Button btn = FindViewById<Button>(Resource.Id.loginButton);
            btn.Visibility = ViewStates.Gone;
            TextView loginError = FindViewById<TextView>(Resource.Id.loginError);
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                loginError.Text = Resources.GetString(Resource.String.LoginEmptyError);
                loginError.Visibility = ViewStates.Visible;
                progress.Visibility = ViewStates.Invisible;
                btn.Visibility = ViewStates.Visible;
                return;
            }
            // try login, if login wrong 
    
            Task<string> loginCheckTask = loginCheckAsync(username, password);
            string userToken = await loginCheckTask;
            progress.Visibility = ViewStates.Invisible;
            btn.Visibility = ViewStates.Visible;
            if (userToken == "error") 
            {
                loginError.Text = Resources.GetString(Resource.String.LoginWrongError);
                loginError.Visibility = ViewStates.Visible;
                return;
            }
            loginError.Visibility = ViewStates.Gone;
            _database db = new _database(this);
            db.saveUserToken(userToken);
            Singleton.userToken = userToken;
            StartActivity(typeof(SplashActivity));
            
        }

        async protected Task<string> loginCheckAsync(string username, string password)
        {
            // interact with api
            ApiService apiService = new ApiService(this);
            Task<string> userTokenTask = apiService.RequestUserToken(username, password);
            string userToken = await userTokenTask; 
            return userToken;
        }

    }
}