using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
 
 
using Logitude.BL.InfrastructureModel.EntityLists;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class DefaultAndConfigurationKeyQuery
    {
        DefaultAndConfigurationKeyRepository repository;

        public DefaultAndConfigurationKeyQuery()
        {
            repository = new DefaultAndConfigurationKeyRepository(); 
        }

        public DefaultAndConfigurationKeyQuery(int tenant)
        {
            repository = new DefaultAndConfigurationKeyRepository(tenant);
        }

        public DefaultAndConfigurationKeyQuery(DefaultAndConfigurationKeyRepository DefaultAndConfigurationKeyRepository)
        {
            repository = DefaultAndConfigurationKeyRepository;
        }

        public DefaultAndConfigurationKeyPM GetSinglePM( int tenant1, string settype, string setkey, int tenant)
        {        
            DefaultAndConfigurationKeyPM result =
          (from a in repository.context.DefaultAndConfigurationKey
           where a.SetKey == setkey
           select new DefaultAndConfigurationKeyPM()
           { 
               Tenant = a.Tenant,
               CreateDateTime = a.CreateDateTime,
               SetType = a.SetType,
               SetKey = a.SetKey,
               ShortDescription = a.ShortDescription,
               FullDesctiption = a.FullDesctiption

           }).FirstOrDefault();


            return result;

        }

        //public DefaultAndConfigurationKeyPM GetSingleDefaultAndConfigurationKeyPM(string id)
        //{
        //    DefaultAndConfigurationKeyPM result =
        //    (from a in repository.context.DefaultAndConfigurationKey
        //     where a.Id == id
        //     select new DefaultAndConfigurationKeyPM()
        //     {
                
        //         Tenant = a.Tenant,
        //         CreateDateTime = a.CreateDateTime,
        //         SetType = a.SetType,
        //         SetKey = a.SetKey,
        //         ShortDescription = a.ShortDescription,
        //         FullDesctiption = a.FullDesctiption

        //     }).FirstOrDefault();

             
        //    return result;

        //}

      
    

        public List<DefaultAndConfigurationKeyPM> GetDefaultAndConfigurationKeyPMByField1(string SetKey)
        {
            List<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKey
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyPM()
             {
                
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             }).ToList();


            return result;

        }

        public List<DefaultAndConfigurationKeyPM> GetDefaultAndConfigurationKeyPMByField2(string SetType)
        {
            List<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKey
             where a.SetType == SetType
             select new DefaultAndConfigurationKeyPM()
             {
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             }).ToList();


            return result;

        }

        public IQueryable<DefaultAndConfigurationKeyList> GetIQueryableEntityList(IQueryable<DefaultAndConfigurationKey> iQueryable)
        {
            IQueryable<DefaultAndConfigurationKeyList> result = from a in iQueryable
                                                             select new DefaultAndConfigurationKeyList()
                                                             {
                                                                 
                                                                 Tenant = a.Tenant,
                                                                 CreateDateTime = a.CreateDateTime,
                                                                 SetType = a.SetType,
                                                                 SetKey = a.SetKey,
                                                                 ShortDescription = a.ShortDescription,
                                                                 FullDesctiption = a.FullDesctiption
                                                             };
            return result;
        }
        
        public IQueryable<DefaultAndConfigurationKeyPM> GetIQueryableDefaultAndConfigurationKeyPMByField1(string SetKey)
        {
            IQueryable<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKey
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyPM()
             {
                
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             });


            return result;

        }

        public IQueryable<DefaultAndConfigurationKeyPM> GetIQueryableDefaultAndConfigurationKeyPMByField1Field2(string SetKey, string SetType)
        {
            IQueryable<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKey
             where a.SetKey == SetKey && a.SetType == SetType
             select new DefaultAndConfigurationKeyPM()
             {
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             });


            return result;

        }

        public IQueryable<DefaultAndConfigurationKeyList> GetIQueryableDefaultAndConfigurationKeyPMByField1List(string SetKey)
        {
            IQueryable<DefaultAndConfigurationKeyList> result =
            (from a in repository.context.DefaultAndConfigurationKey
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyList()
             { 
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             });


            return result;

        }

        public IQueryable<DefaultAndConfigurationKeyList> GetIQueryableDefaultAndConfigurationKeyPMByField2List(string SetType)
        {
            IQueryable<DefaultAndConfigurationKeyList> result =
            (from a in repository.context.DefaultAndConfigurationKey
             where a.SetType == SetType
             select new DefaultAndConfigurationKeyList()
             {
                
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             });


            return result;

        }
         
         
    }
}