using System;
using GigUpdates.Helpers;

namespace GigUpdates
{
    public class FacebookUser
    {
        //public FacebookUser(string id, string token, string firstName, string lastName, string email, string imageUrl,bool isExpired)
        //{
        //    id = id;
        //    Token = token;
        //    FirstName = firstName;
        //    LastName = lastName;
        //    Email = email;
        //    Picture = imageUrl;
        //    //Expires = expires;
        //    IsExpired = isExpired;
        //}

        public string id { get; set; }

        public string Token { get; set; }

        //public DateTime Expires { get; set; }

        public bool IsExpired { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public Picture picture { get; set; }

        public string gender { get; set; }
    }
    public class Data
    {
        public bool is_silhouette { get; set; }
        public string url { get; set; }
    }

    public class Picture
    {
        public Data data { get; set; }
    }
}
