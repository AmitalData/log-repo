using System;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.Helpers
{
    public static class TenantServerConfigration
    {


        public static DateTime GetCurrentDateTime(int tenant)
        {
        
            if (LogitudeSettings.IsCostomsDeploy)
            {
                return DateTime.Now;
            }
            double offsetHours = 0;
            //DateTime dateTime = DateTime.Now;
            //var format = "yyyy-MM-dd HH:mm:ss:fff";
            //var stringDate = DateTime.Now.ToString(format);
            var dateTime = DateTime.UtcNow;//DateTime.ParseExact(stringDate, format, new CultureInfo("en-US"));
            string datetimeoffset = "datetimeoffset" + tenant;

                if (CacheManager.CacheWrapper.Get(datetimeoffset) == null)
                {

                    offsetHours = GetCurrentDateWithTimeZoneOffset(tenant);
                    dateTime = dateTime.AddHours(offsetHours);

                    CacheManager.CacheWrapper.Insert(datetimeoffset, offsetHours, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);

                }
                else
                {
                    offsetHours = (double)CacheManager.CacheWrapper.Get(datetimeoffset);
                    dateTime = dateTime.AddHours(offsetHours);
                }

            
   


            //double offsetHours = Entity.TimeZoneOffset;

            //if (Entity.DayLightOffset != 0)
            //{

            //    if (Entity.DayLightStartDate.Value.Date <= DateTime.Now.Date && Entity.DayLightEndDate.Value.Date >= DateTime.Now.Date)
            //    {
            //        offsetHours = offsetHours + Entity.DayLightOffset; 
            //    } 
            //}



            //if (dateTime.Date > DateTime.Now.Date)
            //{

            //}

            return dateTime;

        }

        private static double GetCurrentDateWithTimeZoneOffset(int tenant)
        {

            string entityName = "Tenant" + tenant;

            Tenant entity;
         
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    TenantRepository tenantRepository = new TenantRepository(tenant);
                    entity = tenantRepository.GetSingleTenant(tenant);
                    if (entity != null)
                    {
                        CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }
                else
                {
                    entity = (Tenant)CacheManager.CacheWrapper.Get(entityName);
                }
            
       

            double offsetHours = 0;
            if (entity != null)
            {
                if (entity.TimeZoneOffset != null)
                {
                    offsetHours = entity.TimeZoneOffset.Value;
                }

                if (entity.DayLightOffset != 0)
                {
                    if (entity.DayLightStartDate.Value.Date <= DateTime.Now.Date && entity.DayLightEndDate.Value.Date >= DateTime.Now.Date)
                    {
                        offsetHours = offsetHours + entity.DayLightOffset;
                    }
                }

            }

            return offsetHours;
        }


        public static string GetDbConnection(int tenant)
        {
          
            GlobalDB currentDb;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                //GlobalDBRep = new GlobalDBRepository();
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);

            }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }
    }

    public class TenantServerConfigrationWrapper
    {
        public DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }
    }
}
