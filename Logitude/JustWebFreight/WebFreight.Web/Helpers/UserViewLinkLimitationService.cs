using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class UserViewLinkLimitationService
    {
        private UserViewLinkDetails userViewLinkDetails = null;
     
        public void Run(string linkName, string userkey)
        {
            if (!string.IsNullOrEmpty(userkey))
            {
                userViewLinkDetails = new UserViewLinkDetails() { Userkey = userkey, LinkName = linkName, LastViewDate = DateTime.Now, NumberOfClick = 1 };
                
                string entityCacheName = linkName + userkey;
                if (CacheManager.CacheWrapper.Get(entityCacheName) != null)
                {
                    userViewLinkDetails = (UserViewLinkDetails)CacheManager.CacheWrapper.Get(entityCacheName);
                    Validation();
                }

                CacheManager.CacheWrapper.Insert(entityCacheName, userViewLinkDetails, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }
        }

        private  void Validation()
        {
            TimeSpan timeSpan = (DateTime.Now - userViewLinkDetails.LastViewDate);
            if (timeSpan.Minutes < 5)
            {
                if (userViewLinkDetails.NumberOfClick >= 4)
                {
                    throw new Exception("Download is limited to 4 times in one hour, please try again later");
                }
                else userViewLinkDetails.NumberOfClick += 1;
            }
            else
            {
                userViewLinkDetails.LastViewDate = DateTime.Now;
                userViewLinkDetails.NumberOfClick = 1;
            }
        }

    }



    public class UserViewLinkDetails
    {
        public string Userkey { get; set; }
        public string LinkName { get; set; }
        public DateTime LastViewDate { get; set; }
        public int NumberOfClick { get; set; }

    }
}