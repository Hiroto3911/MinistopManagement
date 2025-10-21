using Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security
{
    public class UserSession : IUserSession
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string IdStore { get; set; }
        public bool IsLoggedIn { get; private set; }

        public void Login(string userId, string userName, string role, string storeId)
        {
            UserId = userId;
            Name = userName;
            Role = role;
            IdStore = storeId;
            IsLoggedIn = true;
        }

        public void LogOut()
        {
            UserId = string.Empty;
            Name = string.Empty;
            Role = string.Empty;
            IdStore = string.Empty;
            IsLoggedIn = false;
        }
    }
}
