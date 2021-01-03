using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SecurityTests.Models.Login
{
    public class UsersData
    {
        public UsersData()
        {
            ListOfUserData = new List<UserData>();
        }
        public List<UserData> ListOfUserData { set; get; }
    }
}
