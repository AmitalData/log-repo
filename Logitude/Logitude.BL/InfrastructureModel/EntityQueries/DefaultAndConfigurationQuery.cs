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
    public class DefaultAndConfigurationQuery
    {
        DefaultAndConfigurationRepository repository;

        public DefaultAndConfigurationQuery()
        {
            repository = new DefaultAndConfigurationRepository();
        }

        public DefaultAndConfigurationQuery(int tenant)
        {
            repository = new DefaultAndConfigurationRepository(tenant);
        }

        public DefaultAndConfigurationQuery(DefaultAndConfigurationRepository DefaultAndConfigurationsRepository)
        {
            repository = DefaultAndConfigurationsRepository;
        }

        public DefaultAndConfigurationPM GetSinglePM(string id, int tenant)
        {
            long? LongId = null;
            if (id != null)
            {
                LongId = long.Parse(id);
            }

            DefaultAndConfigurationPM result =
          (from a in repository.context.DefaultAndConfigurations
           where a.Id == id
           select new DefaultAndConfigurationPM()
           {
               Id = a.Id,
               Tenant = a.Tenant,
               CreateDate = a.CreateDate,
               SearchFields = a.SearchFields,
               Is_Active = a.Is_Active,
               StoreInCache = a.StoreInCache,
               SetKey = a.SetKey,
               AdditionalKey = a.AdditionalKey,
               SortOrder = a.SortOrder,
               SetValueType1 = a.SetValueType1,
               Value1 = a.Value1,
               SetValueType2 = a.SetValueType2,
               Value2 = a.Value2,
               AllowInheritance = a.AllowInheritance

           }).FirstOrDefault();


            return result;

        }

        public DefaultAndConfigurationPM GetSingleDefaultAndConfigurationsPM(string id)
        {
            DefaultAndConfigurationPM result =
            (from a in repository.context.DefaultAndConfigurations
             where a.Id == id
             select new DefaultAndConfigurationPM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDate = a.CreateDate,
                 SearchFields = a.SearchFields,
                 Is_Active = a.Is_Active,
                 StoreInCache = a.StoreInCache,
                 SetKey = a.SetKey,
                 AdditionalKey = a.AdditionalKey,
                 SortOrder = a.SortOrder,
                 SetValueType1 = a.SetValueType1,
                 Value1 = a.Value1,
                 SetValueType2 = a.SetValueType2,
                 Value2 = a.Value2,
                 AllowInheritance = a.AllowInheritance

             }).FirstOrDefault();


            return result;

        }

        public DefaultAndConfigurationList GetSingleDefaultAndConfigurationList(string id)
        {
            DefaultAndConfigurationList result =
            (from a in repository.context.DefaultAndConfigurations
             where a.Id == id
             select new DefaultAndConfigurationList()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDate = a.CreateDate,
                 SearchFields = a.SearchFields,
                 Is_Active = a.Is_Active,
                 StoreInCache = a.StoreInCache,
                 SetKey = a.SetKey,
                 AdditionalKey = a.AdditionalKey,
                 SortOrder = a.SortOrder,
                 SetValueType1 = a.SetValueType1,
                 Value1 = a.Value1,
                 SetValueType2 = a.SetValueType2,
                 Value2 = a.Value2,
                 AllowInheritance = a.AllowInheritance

             }).FirstOrDefault();


            return result;

        }

        public List<DefaultAndConfigurationPM> GetDefaultAndConfigurationsPMBySetKey(string SetKey)
        {
            List<DefaultAndConfigurationPM> result =
            (from a in repository.context.DefaultAndConfigurations
             where a.SetKey == SetKey
             select new DefaultAndConfigurationPM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDate = a.CreateDate,
                 SearchFields = a.SearchFields,
                 Is_Active = a.Is_Active,
                 StoreInCache = a.StoreInCache,
                 SetKey = a.SetKey,
                 AdditionalKey = a.AdditionalKey,
                 SortOrder = a.SortOrder,
                 SetValueType1 = a.SetValueType1,
                 Value1 = a.Value1,
                 SetValueType2 = a.SetValueType2,
                 Value2 = a.Value2,
                 AllowInheritance = a.AllowInheritance

             }).ToList();


            return result;

        }

        public IQueryable<DefaultAndConfigurationList> GetIQueryableEntityList(IQueryable<DefaultAndConfiguration> iQueryable)
        {
            IQueryable<DefaultAndConfigurationList> result = from a in iQueryable
                                                             select new DefaultAndConfigurationList()
                                                             {
                                                                 Id = a.Id,
                                                                 Tenant = a.Tenant,
                                                                 CreateDate = a.CreateDate,
                                                                 SearchFields = a.SearchFields,
                                                                 Is_Active = a.Is_Active,
                                                                 StoreInCache = a.StoreInCache,
                                                                 SetKey = a.SetKey,
                                                                 AdditionalKey = a.AdditionalKey,
                                                                 SortOrder = a.SortOrder,
                                                                 SetValueType1 = a.SetValueType1,
                                                                 Value1 = a.Value1,
                                                                 SetValueType2 = a.SetValueType2,
                                                                 Value2 = a.Value2,
                                                                 AllowInheritance = a.AllowInheritance
                                                             };
            return result;
        }

        public IQueryable<DefaultAndConfigurationPM> GetIQueryableDefaultAndConfigurationsPMBySetKey(string SetKey)
        {
            IQueryable<DefaultAndConfigurationPM> result =
            (from a in repository.context.DefaultAndConfigurations
             where a.SetKey == SetKey
             select new DefaultAndConfigurationPM()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDate = a.CreateDate,
                 SearchFields = a.SearchFields,
                 Is_Active = a.Is_Active,
                 StoreInCache = a.StoreInCache,
                 SetKey = a.SetKey,
                 AdditionalKey = a.AdditionalKey,
                 SortOrder = a.SortOrder,
                 SetValueType1 = a.SetValueType1,
                 Value1 = a.Value1,
                 SetValueType2 = a.SetValueType2,
                 Value2 = a.Value2,
                 AllowInheritance = a.AllowInheritance

             });


            return result;

        }


        public IQueryable<DefaultAndConfigurationList> GetIQueryableDefaultAndConfigurationsPMByField1List(string SetKey)
        {
            IQueryable<DefaultAndConfigurationList> result =
            (from a in repository.context.DefaultAndConfigurations
             where a.SetKey == SetKey
             select new DefaultAndConfigurationList()
             {
                 Id = a.Id,
                 Tenant = a.Tenant,
                 CreateDate = a.CreateDate,
                 SearchFields = a.SearchFields,
                 Is_Active = a.Is_Active,
                 StoreInCache = a.StoreInCache,
                 SetKey = a.SetKey,
                 AdditionalKey = a.AdditionalKey,
                 SortOrder = a.SortOrder,
                 SetValueType1 = a.SetValueType1,
                 Value1 = a.Value1,
                 SetValueType2 = a.SetValueType2,
                 Value2 = a.Value2,
                 AllowInheritance = a.AllowInheritance

             });


            return result;

        }

    }
}