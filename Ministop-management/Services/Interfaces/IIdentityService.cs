using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IIdentityService
    {
        bool Authentication(string userID, string password);
        void LogOut();
    }
}
