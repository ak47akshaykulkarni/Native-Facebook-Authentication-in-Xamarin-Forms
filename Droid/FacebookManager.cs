using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;

using GigUpdates;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Org.Json;
using Xamarin.Forms;
using GigUpdates.Droid;
using GigUpdates.Helpers;
using Newtonsoft.Json;

[assembly: Dependency(typeof(FacebookManager))]
namespace GigUpdates.Droid
{
    public class FacebookManager : Java.Lang.Object, IFacebookManager, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback, GraphRequest.IGraphJSONArrayCallback
    {
        public Action<FacebookUser, string> _onLoginComplete;
        public Action<List<FacebookUser>, string> _onGetFriends;
        public ICallbackManager _callbackManager;
        private const string GraphPathsFriends = "FRIENDS";
        private const string GraphPathsLogin = "LOGIN";
        private string CurrentPath;
        public FacebookManager()
        {
            _callbackManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(_callbackManager, this);
        }

        public void Login(Action<FacebookUser, string> onLoginComplete)
        {
            CurrentPath = GraphPathsLogin;
            _onLoginComplete = onLoginComplete;
            LoginManager.Instance.SetLoginBehavior(LoginBehavior.NativeWithFallback);
            LoginManager.Instance.LogInWithReadPermissions(Xamarin.Forms.Forms.Context as Activity, new List<string> { "public_profile", "email", "user_friends" });
        }

        public void Logout()
        {
            LoginManager.Instance.LogOut();
        }

        public void GetFriends(Action<List<FacebookUser>, string> onGetFriends)
        {
            CurrentPath = GraphPathsFriends;
            _onGetFriends = onGetFriends;
            var request = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);
            request.GraphPath = "/me/friends";
            var bundle = new Android.OS.Bundle();
            bundle.PutString("fields", "id, first_name,last_name,picture.height(200).width(200){is_silhouette,url},gender");
            request.Parameters = bundle;
            request.ExecuteAsync();
        }

        #region IFacebookCallback
        public void OnSuccess(Java.Lang.Object result)
        {
            var n = result as LoginResult;
            if (n != null)
            {
                var request = GraphRequest.NewMeRequest(n.AccessToken, this);
                var bundle = new Android.OS.Bundle();
                bundle.PutString("fields", "id, first_name, last_name ,email,picture.height(200).width(200){is_silhouette,url},gender");
                request.Parameters = bundle;
                request.ExecuteAsync();
            }
        }
        public void OnCancel()
        {
            _onLoginComplete?.Invoke(null, "Canceled!");
        }

        public void OnError(FacebookException error)
        {
            _onLoginComplete?.Invoke(null, error.Message);
        }
        public void OnCompleted(JSONObject p0, GraphResponse p1)
        {
            if (p0 == null) return;
            if (CurrentPath == GraphPathsLogin)
            {
                var facebookIUser = JsonConvert.DeserializeObject<FacebookUser>(p0.ToString());
                facebookIUser.Token = AccessToken.CurrentAccessToken.Token;
                _onLoginComplete?.Invoke(facebookIUser, string.Empty);
            }
            else if(CurrentPath == GraphPathsFriends)
            {
                var facebookFriends = JsonConvert.DeserializeObject<FacebookFriends>(p0.ToString());
                _onGetFriends?.Invoke(facebookFriends.data, facebookFriends.summary.total_count.ToString());
            }
        }
        #endregion

        public void OnCompleted(JSONArray objects, GraphResponse response)
        {
            int ii = 123;
            ii++;
            //do something
            //throw new NotImplementedException();
        }
    }
}
