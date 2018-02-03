using Xamarin.Forms;
using System;
using System.Collections.Generic;
using GigUpdates;
using GigUpdates.Helpers;
using Newtonsoft.Json;
using System.ComponentModel;

namespace GigUpdates
{
    public partial class GigUpdatesPage : ContentPage, INotifyPropertyChanged
    {
        private FacebookUser _facebookUser;

        public FacebookUser FacebookUser
        {
            get
            {
                return _facebookUser??new FacebookUser();
            }
            set { _facebookUser = value;}
        }
        private List<FacebookUser> _facebookUsers;

        public List<FacebookUser> FacebookUsers
        {
            get { return _facebookUsers?? new List<FacebookUser>(); }
            set { _facebookUsers = value; }
        }

        private bool _isLogedIn;

        public bool IsLogedIn
        {
            get { return _isLogedIn; }
            set { _isLogedIn= value; }
        }

        public GigUpdatesPage()
        {
            
            InitializeComponent();
            FbLogin.Clicked += (s, e) =>
            {
                if (FbLogin.Text == "Logout")
                {
                    DependencyService.Get<IFacebookManager>().Logout();
                    FacebookUser = null;
                    FacebookUsers = null;
                    Settings.FacebookMeUser = string.Empty;
                    UpdateUserInterface(FacebookUser);
                    FbLogin.Text = "Login with facebook";
                    ListViewUsers.ItemsSource = FacebookUsers;
                }
                else
                {
                    try{
                        DependencyService.Get<IFacebookManager>().Login(OnLoginComplete);
                    }
                    catch(Exception ex){
                        DisplayAlert("What?", ex.Message, "faak");
                    }
                }
            };
            FbGetFriends.Clicked += (s, e) =>
            {
                try
                {
                    //_facebookManager.Login(OnLoginComplete);
                    DependencyService.Get<IFacebookManager>().GetFriends(OnGetFriends);
                }
                catch (Exception ex)
                {
                    DisplayAlert("What?", ex.Message, "faak");
                }
            };
            try
            {
                FacebookUser = JsonConvert.DeserializeObject<FacebookUser>(Settings.FacebookMeUser);
                UpdateUserInterface(FacebookUser);
            }
            catch (Exception e)
            {
                FbLogin.Text = "Login with facebook";
            }


        }
        private void OnLoginComplete(FacebookUser facebookUser, string message)
        {
            if (facebookUser != null)
            {
                
                FacebookUser = facebookUser;
                UpdateUserInterface(facebookUser);
                Settings.FacebookMeUser = JsonConvert.SerializeObject(FacebookUser);
            }
            else
            {
                DisplayAlert("What?",message,"faak" );
            }
        }

        private void OnGetFriends(List<FacebookUser> facebookUsers, string message)
        {
            if (facebookUsers != null)
            {
                FacebookUsers = facebookUsers;
                TotalFriendsLabel.Text = message;
                ListViewUsers.ItemsSource = FacebookUsers;
            }
            else
            {
                ListViewUsers.ItemsSource = new FacebookFriends().data;
                DisplayAlert("What?", message, "faak");
            }
        }

        private void UpdateUserInterface(FacebookUser fu)
        {
            if (fu==null)
            {
                IsLogedIn = false;
                FbLogin.Text = "Login with facebook";
                FacebookUser =new FacebookUser();
            }
            else
            {
                FbLogin.Text = "Logout";
                IsLogedIn = true;
                UserDataPanel.BindingContext = FacebookUser;
            }
            
        }
    }

}
