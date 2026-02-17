using System;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Server.Infrastructure;
using System.Configuration;

namespace Simplog.Global.Data.GlobalModel.Helpers
{
    public class GlobalDbHelper
    {

        public static GlobalDB GetGlobalDB(int tenant)
        {
            GlobalDB currentDb = null;
            string name = "TenantDB" + tenant;
            //itzik if (HttpContext.Current != null)
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(name) == null)
                {
                    
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalcontext = GlobalContext.GetContext();

                        GlobalTenant globaltenant = (from a in globalcontext.GlobalTenants
                                                     where a.Id == tenant
                                                     select a).FirstOrDefault();

//#if ORACLE_DB
                        string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                        if (dbms == "oracle")
                        {
                            if (globaltenant == null)
                            {
                                currentDb = (from a in globalcontext.GlobalDBs

                                             select a).FirstOrDefault();
                            }
                            else
                            {
                                currentDb = (from a in globalcontext.GlobalDBs
                                             where a.Id == globaltenant.GlobalDBId
                                             select a).FirstOrDefault();
                            }
                            //if (DateTime.Now < new DateTime(2015, 12, 5))
                            //{
                            //    currentDb.DBConnection = 
                            //        //"User Id=logitude_main;  Password=oracle;Direct=True;Data Source=10.10.10.67;port=1521;sid=amital";
                            //        "User Id=AMITESTM3;  Password=AMITESTM3;Direct=True;Data Source=10.10.10.67;port=1521;sid=amital";
                            //}
                        }


//#else
                        else
                        {
                            currentDb = (from a in globalcontext.GlobalDBs
                                         where a.Id == globaltenant.GlobalDBId
                                         select a).FirstOrDefault();
                        }
//#endif
                        


                        CacheManager.CacheWrapper.Insert(name, GetGlobalDbWithoutProxy(currentDb), null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        scope.Complete();
                    }
                }
                else
                {
                    currentDb = (GlobalDB)CacheManager.CacheWrapper.Get(name);
                }
            }


            if (currentDb == null) //Itzik Why i get null ??? else
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalcontext = GlobalContext.GetContext();

                    GlobalTenant globaltenant = (from a in globalcontext.GlobalTenants
                                                 where a.Id == tenant
                                                 select a).FirstOrDefault();
                    if (globaltenant != null)
                    {
                        currentDb = (from a in globalcontext.GlobalDBs
                                     where a.Id == globaltenant.GlobalDBId
                                     select a).FirstOrDefault();

                        if (currentDb != null && CacheManager.CacheWrapper != null) //Itzik 
                        {
                            CacheManager.CacheWrapper.Insert(name, GetGlobalDbWithoutProxy(currentDb), null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }

                    }
                    else
                    {
                        string dbtenant = tenant.ToString();
                        currentDb = (from a in globalcontext.GlobalDBs
                                     where a.Id == dbtenant
                                     select a).FirstOrDefault();

                        if (currentDb != null && CacheManager.CacheWrapper != null) //Itzik 
                        {
                            CacheManager.CacheWrapper.Insert(name, GetGlobalDbWithoutProxy(currentDb), null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }
            }
            currentDb.DBConnection = DbContextBaseUtil.GetConnectionStringWithAmitalNetRole(currentDb.DBConnection);
            return currentDb;
        }

        private static GlobalDB GetGlobalDbWithoutProxy(GlobalDB currentDb)
        {
            GlobalDB currentDbCacheWithoutProxy = new GlobalDB()
            {
                DBConnection = currentDb.DBConnection,
                Id = currentDb.Id,
                IsActive = currentDb.IsActive,
                IsUpgrading = currentDb.IsUpgrading
            };
            return currentDbCacheWithoutProxy;
        }


        public static GlobalDB GetGlobalDBById(string id)
        {

            string name = "TenantDB" + id;
            GlobalDB db = null;



            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(name) == null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext context = GlobalContext.GetContext();


                        db = (from a in context.GlobalDBs
                              where a.Id == id
                              select a).FirstOrDefault();

                        CacheManager.CacheWrapper.Insert(name, GetGlobalDbWithoutProxy(db), null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                    //}
                }
                else
                {
                    db = (GlobalDB)CacheManager.CacheWrapper.Get(name);
                }
            }

            else
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext context = GlobalContext.GetContext();



                    db = (from a in context.GlobalDBs
                          where a.Id == id
                          select a).FirstOrDefault();
                }
            }



            return db;
        }
         
        public static GlobalDB GetSingleGlobalDB()
        {

            string name = "TenantDB," + "GetSingleGlobalDB";
            
            GlobalDB db = null;
            db  =CacheManager.GetOrInsertNewObject<GlobalDB>(name, () =>
            {
                IGlobalContext context = GlobalContext.GetContext();


                var db1stAndOnly1 = (from a in context.GlobalDBs
                      select a).Single( );
                return db1stAndOnly1;

            }, true); 


            return db;
        }

        public static GlobalDB GetGlobalDBWithNoCache(int tenant)
        {
            GlobalDB currentDb = null;
            string name = "TenantDB" + tenant;
             
               
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                    {
                        IGlobalContext globalcontext = GlobalContext.GetContext();

                        GlobalTenant globaltenant = (from a in globalcontext.GlobalTenants
                                                     where a.Id == tenant
                                                     select a).FirstOrDefault();

                        //#if ORACLE_DB
                        string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                        if (dbms == "oracle")
                        {
                            if (globaltenant == null)
                            {
                                currentDb = (from a in globalcontext.GlobalDBs

                                             select a).FirstOrDefault();
                            }
                            else
                            {
                                currentDb = (from a in globalcontext.GlobalDBs
                                             where a.Id == globaltenant.GlobalDBId
                                             select a).FirstOrDefault();
                            }
                            //if (DateTime.Now < new DateTime(2015, 12, 5))
                            //{
                            //    currentDb.DBConnection = 
                            //        //"User Id=logitude_main;  Password=oracle;Direct=True;Data Source=10.10.10.67;port=1521;sid=amital";
                            //        "User Id=AMITESTM3;  Password=AMITESTM3;Direct=True;Data Source=10.10.10.67;port=1521;sid=amital";
                            //}
                        }


                        //#else
                        else
                        {
                            currentDb = (from a in globalcontext.GlobalDBs
                                         where a.Id == globaltenant.GlobalDBId
                                         select a).FirstOrDefault();
                        }
                        //#endif



                        //CacheManager.CacheWrapper.Insert(name, GetGlobalDbWithoutProxy(currentDb), null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        scope.Complete();
                    }
               


            if (currentDb == null) //Itzik Why i get null ??? else
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    IGlobalContext globalcontext = GlobalContext.GetContext();

                    GlobalTenant globaltenant = (from a in globalcontext.GlobalTenants
                                                 where a.Id == tenant
                                                 select a).FirstOrDefault();
                    if (globaltenant != null)
                    {
                        currentDb = (from a in globalcontext.GlobalDBs
                                     where a.Id == globaltenant.GlobalDBId
                                     select a).FirstOrDefault();

                        if (currentDb != null && CacheManager.CacheWrapper != null) //Itzik 
                        {
                            CacheManager.CacheWrapper.Insert(name, GetGlobalDbWithoutProxy(currentDb), null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }

                    }
                    else
                    {
                        string dbtenant = tenant.ToString();
                        currentDb = (from a in globalcontext.GlobalDBs
                                     where a.Id == dbtenant
                                     select a).FirstOrDefault();

                        if (currentDb != null && CacheManager.CacheWrapper != null) //Itzik 
                        {
                            CacheManager.CacheWrapper.Insert(name, GetGlobalDbWithoutProxy(currentDb), null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                }
            }
            currentDb.DBConnection = DbContextBaseUtil.GetConnectionStringWithAmitalNetRole(currentDb.DBConnection);
            return currentDb;
        }

    }
}
