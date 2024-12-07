using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityLists;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ScreensQuery
    {
             ScreensRepository repository;
        public ScreensQuery()
        {
            repository = new ScreensRepository(); 
        }

        public ScreensQuery(int tenant)
        {
            repository = new ScreensRepository(tenant);
        }

        public ScreensQuery(ScreensRepository screensRepository)
        {
            repository = screensRepository;
        }

        public List<ScreenPM> GetScreenPMsByTenant(int tenant)
        {
            List<ScreenPM> screens;
            List<ScreenPM> zeroscreens;
            List<ScreenPM> currentscreens;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
             WebFreightContext   webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);
                ScreenFieldsRepository screenfieldsRep = new ScreenFieldsRepository(tenant);
                zeroscreens = (from a in repository.context.Screens.Include("ObjectTable")
                               where a.Tenant == 0
                               select new ScreenPM()
                               {
                                   Code = a.Code,
                                   Id = a.Id,
                                   IsReadOnly = a.IsReadOnly,
                                   Inactive = a.Inactive,
                                   NumberOfColumns = a.NumberOfColumns,
                                   NumberOfRows = a.NumberOfRows,
                                   ObjectTableId = a.ObjectTableId,
                                   Name = a.Name,
                                   ObjectTableName = a.ObjectTable.Name,
                                   Tenant = a.Tenant,
                                   UserTenant = tenant,
                                   Type = a.Type,
                                   SortedByFieldCode = a.SortedByFieldCode,
                                   SortedType = a.SortedType,
                                   SearchFields = a.SearchFields,
                                   RelatedScreenCode = a.RelatedScreenCode,
                                   IsHeaderScreen = a.IsHeaderScreen
                               }).ToList();


                Dictionary<string, ScreenModification> screensDictionary = new Dictionary<string, ScreenModification>();
                screensDictionary = repository.context.ScreenModifications.Where(te => te.Tenant == tenant).ToDictionary(dic => dic.ScreenId, dic => dic);

                foreach (ScreenPM screen in zeroscreens)
                {
                    //ScreenModification mod = (from a in context.ScreenModifications
                    //                          where a.ScreenId == screen.Id && a.Tenant == tenant
                    //                          select a).FirstOrDefault();

                    if (screensDictionary.Keys.Contains(screen.Id))
                    {
                        ScreenModification mod = screensDictionary[screen.Id];

                        if (mod != null)
                        {
                            screen.NumberOfRows = mod.NumberOfRows;
                            screen.NumberOfColumns = mod.NumberOfColumns;

                        }
                    }


                }
            }

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);
                ScreenFieldsRepository screenfieldsRep = new ScreenFieldsRepository(tenant);
                currentscreens = (from a in repository.context.Screens.Include("ObjectTable")
                                  where a.Tenant == tenant
                                  select new ScreenPM()
                                  {
                                      Code = a.Code,
                                      Id = a.Id,
                                      IsReadOnly = a.IsReadOnly,
                                      Inactive = a.Inactive,
                                      NumberOfColumns = a.NumberOfColumns,
                                      NumberOfRows = a.NumberOfRows,
                                      ObjectTableId = a.ObjectTableId,
                                      Name = a.Name,
                                      ObjectTableName = a.ObjectTable.Name,
                                      Tenant = a.Tenant,
                                      UserTenant = tenant,
                                      Type = a.Type,
                                      SortedByFieldCode = a.SortedByFieldCode,
                                      SortedType = a.SortedType,
                                      SearchFields = a.SearchFields,
                                      RelatedScreenCode = a.RelatedScreenCode,
                                      IsHeaderScreen = a.IsHeaderScreen
                                  }).ToList();

            }
            screens = zeroscreens.Concat(currentscreens).ToList();

            return screens;
        }

        public ScreenPM GetSinglePM( string id,int tenant)
        {
            WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);
            ScreenFieldsRepository screenfieldsRep = new ScreenFieldsRepository(tenant);
            return (from a in repository.context.Screens.Include("ObjectTable")
                    where a.Tenant == tenant && a.Id == id
                    select new ScreenPM()
                    {
                        Code = a.Code,
                        Id = a.Id,
                        IsReadOnly = a.IsReadOnly,
                        Inactive = a.Inactive,
                        NumberOfColumns = a.NumberOfColumns,
                        NumberOfRows = a.NumberOfRows,
                        ObjectTableId = a.ObjectTableId,
                        Name = a.Name,
                        ObjectTableName = a.ObjectTable.Name,
                        Tenant = a.Tenant,
                        UserTenant = tenant,
                        Type = a.Type,
                        SortedByFieldCode = a.SortedByFieldCode,
                        SortedType = a.SortedType,
                        SearchFields = a.SearchFields,
                        RelatedScreenCode = a.RelatedScreenCode,
                        IsHeaderScreen = a.IsHeaderScreen
                    }).FirstOrDefault();

        }

        public List<ScreenPM> GetByEntity(string entityId, int tenant)
        {
            WebFreightContext webFreightContext = (WebFreightContext)WebFreightContext.GetContext(tenant);
            ScreenFieldsRepository screenfieldsRep = new ScreenFieldsRepository(tenant);
            return (from a in repository.context.Screens.Include("ObjectTable")
                    where a.Tenant == tenant && a.ObjectTableId == entityId
                    select new ScreenPM()
                    {
                        Code = a.Code,
                        Id = a.Id,
                        IsReadOnly = a.IsReadOnly,
                        Inactive = a.Inactive,
                        NumberOfColumns = a.NumberOfColumns,
                        NumberOfRows = a.NumberOfRows,
                        ObjectTableId = a.ObjectTableId,
                        Name = a.Name,
                        ObjectTableName = a.ObjectTable.Name,
                        Tenant = a.Tenant,
                        UserTenant = tenant,
                        Type = a.Type,
                        SortedByFieldCode = a.SortedByFieldCode,
                        SortedType = a.SortedType,
                        SearchFields = a.SearchFields,
                        RelatedScreenCode = a.RelatedScreenCode,
                        IsHeaderScreen = a.IsHeaderScreen
                    }).ToList();
        }

        public IQueryable<ScreenList> GetIQueryableEntityList(IQueryable<Screen> iQueryable)
        {
            IQueryable<ScreenList> result = from a in iQueryable
                                                 select new ScreenList()
                                                 {
                                                     Code = a.Code,
                                                     Id = a.Id,
                                                     IsReadOnly = a.IsReadOnly,
                                                     NumberOfColumns = a.NumberOfColumns,
                                                     NumberOfRows = a.NumberOfRows,
                                                     ObjectTableId = a.ObjectTableId,
                                                     Name = a.Name,
                                                     ObjectTableName = a.ObjectTable.Name,
                                                     Tenant = a.Tenant,
                                                     Type = a.Type,
                                                     SearchFields = a.SearchFields,
                                                     RelatedScreenCode = a.RelatedScreenCode,
                                                     IsHeaderScreen = a.IsHeaderScreen
                                                 };
            return result;
        }
    }
}