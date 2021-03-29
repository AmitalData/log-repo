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
             WebFreightContext   webFreightContext = (WebFreightContext)WebFreightContext.GetContext(0);
                ScreenFieldsRepository screenfieldsRep = new ScreenFieldsRepository(0);
                zeroscreens = (from a in repository.context.Screens.Include("ObjectTable")
                               where a.Tenant == 0
                               select new ScreenPM()
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
                                   UserTenant = tenant,
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
                                      NumberOfColumns = a.NumberOfColumns,
                                      NumberOfRows = a.NumberOfRows,
                                      ObjectTableId = a.ObjectTableId,
                                      Name = a.Name,
                                      ObjectTableName = a.ObjectTable.Name,
                                      Tenant = a.Tenant,
                                      UserTenant = tenant,

                                  }).ToList();




                foreach (ScreenPM screen in currentscreens)
                {
                    ScreenModification mod = (from a in repository.context.ScreenModifications
                                              where a.ScreenId == screen.Id && a.Tenant == tenant
                                              select a).FirstOrDefault();
                    if (mod != null)
                    {
                        screen.NumberOfRows = mod.NumberOfRows;
                        screen.NumberOfColumns = mod.NumberOfColumns;

                    }


                }
            }
            screens = zeroscreens.Concat(currentscreens).ToList();

            return screens;
        }


    }
}