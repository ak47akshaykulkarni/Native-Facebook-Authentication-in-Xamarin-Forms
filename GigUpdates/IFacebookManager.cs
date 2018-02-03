using System;
using System.Collections.Generic;

namespace GigUpdates
{
    public interface IFacebookManager
    {
        void Login(Action<FacebookUser, string> onLoginComplete);

        void Logout();

        void GetFriends(Action<List<FacebookUser>, string> onGetFriends);
    }
}
