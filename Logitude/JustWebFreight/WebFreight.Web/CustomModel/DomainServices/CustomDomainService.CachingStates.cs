using Logitude.Customs.BL.Messaging.Customs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
        
    {
        /// <summary>
        /// itzik my 1st RIA tester its not against the DB but Against The Chae Object !!
        /// </summary>
        /// <param name="PB_Id"></param>
        /// <returns></returns>
#if false
        public ClientProgressBarIndicatorM GetSingleClientProgressBarIndicatorM(string PB_Id)
        {
            string entityKeyString = "ClientProgressBarIndicatorM," + PB_Id;
            var CachClientProgressBarIndicatorM = CacheManager.CacheWrapper.Get(entityKeyString) as ClientProgressBarIndicatorM;
            if (CachClientProgressBarIndicatorM == null)
            {
                //CacheManager.CacheWrapper.Remove(entityKeyString);
                //CacheManager.CacheWrapper.Insert(entityKeyString, MyClientProgressBarIndicatorM);
            }

            return CachClientProgressBarIndicatorM;
        }
#endif

    }
}