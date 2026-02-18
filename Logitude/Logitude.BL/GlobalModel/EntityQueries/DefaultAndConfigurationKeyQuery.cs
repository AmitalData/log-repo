using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.InfrastructureModel.EntityLists;

namespace Logitude.BL.GlobalModel.EntityQueries
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

        public DefaultAndConfigurationKeyPM GetSinglePM(string setkey, int tenant)
        {
            DefaultAndConfigurationKeyPM result =
            (from a in repository.context.DefaultAndConfigurationKeys
            where a.SetKey == setkey && a.Tenant == tenant
            select new DefaultAndConfigurationKeyPM()
            { 
               Tenant = a.Tenant,
               CreateDate = a.CreateDate,
               SetType1 = a.SetType1,
               SetKey = a.SetKey,
               ShortDescription = a.ShortDescription,
               FullDesctiption = a.FullDesctiption,
               SetType2 = a.SetType2,
            }).FirstOrDefault();

            return result;
        }

        public List<DefaultAndConfigurationKeyPM> GetDefaultAndConfigurationKeyPMByField1(string SetKey)
        {
            List<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyPM()
             {
                 Tenant = a.Tenant,
                 CreateDate = a.CreateDate,
                 SetType1 = a.SetType1,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption,
                 SetType2 = a.SetType2

             }).ToList();

            return result;
        }

        public IQueryable<DefaultAndConfigurationKeyList> GetIQueryableEntityList(IQueryable<DefaultAndConfigurationKey> iQueryable)
        {
            IQueryable<DefaultAndConfigurationKeyList> result = from a in iQueryable
                select new DefaultAndConfigurationKeyList()
                {
                                                                 
                    Tenant = a.Tenant,
                    CreateDate = a.CreateDate,
                    SetType1 = a.SetType1,
                    SetKey = a.SetKey,
                    ShortDescription = a.ShortDescription,
                    FullDesctiption = a.FullDesctiption,
                    SetType2 = a.SetType2
                };

            return result;
        }

        public IQueryable<DefaultAndConfigurationKeyPM> GetIQueryableDefaultAndConfigurationKeyPMByField1(string SetKey)
        {
            IQueryable<DefaultAndConfigurationKeyPM> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyPM()
             {
                 Tenant = a.Tenant,
                 CreateDate = a.CreateDate,
                 SetType1 = a.SetType1,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption,
                 SetType2 = a.SetType2
             });

            return result;
        }

        public IQueryable<DefaultAndConfigurationKeyList> GetIQueryableDefaultAndConfigurationKeyPMByField1List(string SetKey)
        {
            IQueryable<DefaultAndConfigurationKeyList> result =
            (from a in repository.context.DefaultAndConfigurationKeys
             where a.SetKey == SetKey
             select new DefaultAndConfigurationKeyList()
             {
                 Tenant = a.Tenant,
                 CreateDate = a.CreateDate,
                 SetType1 = a.SetType1,
                 SetKey = a.SetKey,
                 ShortDescription = a.ShortDescription,
                 FullDesctiption = a.FullDesctiption,
                 SetType2 = a.SetType2
             });

            return result;
        }
    }
}