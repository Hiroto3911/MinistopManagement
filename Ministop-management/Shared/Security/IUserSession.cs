using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Security
{
    public interface IUserSession
    {
        string UserId { get; set; }
        string Name { get; set; }
        string Role { get; set; }
        string IdStore { get; set; }
        bool IsLoggedIn { get; }
        void Login(string userId, string userName, string role, string storeId);
        void LogOut();
    }
}
