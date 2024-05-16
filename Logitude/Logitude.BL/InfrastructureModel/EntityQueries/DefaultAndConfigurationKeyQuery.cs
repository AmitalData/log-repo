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

        public DefaultAndConfigurationKeyQuery(DefaultAndConfigurationKeyRepository DefaultAndConfigurationKeysRepository)
        {
            repository = DefaultAndConfigurationKeysRepository;
        }

        public DefaultAndConfigurationKeyPM GetSinglePM(string id)
        {        
            DefaultAndConfigurationKeyPM result =
          (from a in repository.context.DefaultAndConfigurationKeys
           where a.Id == id
           select new DefaultAndConfigurationKeyPM()
           {
               Id = a.Id,
               Tenant = a.Tenant,
               CreateDateTime = a.CreateDateTime,
               SetType = a.SetType,
               SetKey = a.SetKey,
               ShortDescription = a.ShortDescription,
               FullDesctiption = a.FullDesctiption

           }).FirstOrDefault();


            return result;

        }

        public DefaultAndConfigurationKeyPM GetSingleDefaultAndConfigurationKeysPM(string id)
        {
            DefaultAndConfigurationKeyPM result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.Id == id
             select new DefaultAndConfigurationKeyPM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             }).FirstOrDefault();

             
            return result;

        }

        public DefaultAndConfigurationKeyList GetSingleDefaultAndConfigurationKeyList(string id)
        {
            DefaultAndConfigurationKeyList result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.Id == id
             select new DefaultAndConfigurationKeyList()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             }).FirstOrDefault();


            return result;

        }

        public List<DefaultAndConfigurationKeyPM> GetDefaultAndConfigurationKeysPMByField1(string SetKey)
        {
            List<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyPM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             }).ToList();


            return result;

        }

        public List<DefaultAndConfigurationKeyPM> GetDefaultAndConfigurationKeysPMByField2(string SetType)
        {
            List<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetType == SetType
             select new DefaultAndConfigurationKeyPM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             }).ToList();


            return result;

        }

        public IQueryable<DefaultAndConfigurationKeyList> GetIQueryableEntityList(IQueryable<DefaultAndConfigurationKeys> iQueryable)
        {
            IQueryable<DefaultAndConfigurationKeyList> result = from a in iQueryable
                                                             select new DefaultAndConfigurationKeyList()
                                                             {
                                                                 Id = a.Id,
                                                                 Tenant = a.Tenant,
                                                                 CreateDateTime = a.CreateDateTime,
                                                                 SetType = a.SetType,
                                                                 SetKey = a.SetKey,
                                                                 ShortDescription = a.ShortDescription,
                                                                 FullDesctiption = a.FullDesctiption
                                                             };
            return result;
        }
        
        public IQueryable<DefaultAndConfigurationKeyPM> GetIQueryableDefaultAndConfigurationKeysPMByField1(string SetKey)
        {
            IQueryable<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyPM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             });


            return result;

        }

        public IQueryable<DefaultAndConfigurationKeyPM> GetIQueryableDefaultAndConfigurationKeysPMByField1Field2(string SetKey, string SetType)
        {
            IQueryable<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetKey == SetKey && a.SetType == SetType
             select new DefaultAndConfigurationKeyPM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             });


            return result;

        }

        public IQueryable<DefaultAndConfigurationKeyList> GetIQueryableDefaultAndConfigurationKeysPMByField1List(string SetKey)
        {
            IQueryable<DefaultAndConfigurationKeyList> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyList()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDateTime = a.CreateDateTime,
                 SetType = a.SetType,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption

             });


            return result;

        }

        public IQueryable<DefaultAndConfigurationKeyList> GetIQueryableDefaultAndConfigurationKeysPMByField2List(string SetType)
        {
            IQueryable<DefaultAndConfigurationKeyList> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetType == SetType
             select new DefaultAndConfigurationKeyList()
             {
                 Id = a.Id,
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