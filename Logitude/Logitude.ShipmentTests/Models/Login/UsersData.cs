using System.Collections.Generic;

namespace Logitude.SecurityTests.Models.Login
{
    public class UsersData
    {
        public UsersData()
        {
            Users = new List<UserData>();
        }

        public List<UserData> Users { get; set; }
    }
}