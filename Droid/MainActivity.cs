using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Facebook;
using Xamarin.Forms;
using Java.Security;

namespace GigUpdates.Droid
{
    [Activity(Label = "GigUpdates.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            
            global::Xamarin.Forms.Forms.Init(this, bundle);

            #region LetMeHelpYouGetKeyHashes
            PackageInfo info = this.PackageManager.GetPackageInfo("YOUR_PACKAGE_NAME_HERE",PackageInfoFlags.Signatures);
            foreach (Android.Content.PM.Signature signature in info.Signatures)
            {
                MessageDigest md = MessageDigest.GetInstance("SHA");

                md.Update(signature.ToByteArray());
                //For the first run put a debugger on below line. copy keyhash value you get and paste in  
                // facebook app settings on https://developers.facebook.com/apps/YOURAPPID/basic
                // then remove this region "LetMeHelpYouGetKeyHashes"
                string keyhash = Convert.ToBase64String(md.Digest());
                Console.WriteLine("keyhash", keyhash);
            }
            #endregion

            // Okay, want to thank me?
            // do visit http://yekarlo.hol.es
            // You are welcome in advance!
            FacebookSdk.SdkInitialize(this);

            DependencyService.Register<IFacebookManager, FacebookManager>();

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            var manager = DependencyService.Get<IFacebookManager>();
            if (manager != null)
            {
                (manager as FacebookManager)._callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            }
        }
    }
}
