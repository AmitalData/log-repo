using System.Collections.Generic;

namespace Logitude.Test.Base.Models.Login
{
    public class MultiUsers
    {
        public MultiUsers()
        {
            Users = new List<User>();
        }

        public List<User> Users { get; set; }
    }
}