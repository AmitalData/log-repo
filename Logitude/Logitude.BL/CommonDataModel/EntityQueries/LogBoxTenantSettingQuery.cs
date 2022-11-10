using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.DataContracts;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class LogBoxTenantSettingQuery
    {
        LogBoxTenantSettingRepository repository;
        public LogBoxTenantSettingQuery(int tenant)
        {
            repository = new LogBoxTenantSettingRepository(tenant);
        }
        //public LogBoxTenantSettingQuery(int tenant)
        //{
        //    repository = new LogBoxTenantSettingRepository(tenant);
        //}


        public LogBoxTenantSettingQuery(LogBoxTenantSettingRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<LogBoxTenantSettingPM> GetLogBoxTenantSettingPMs()
        {
            IQueryable<LogBoxTenantSettingPM> LogBoxTenantSettings = (from a in repository.context.LogBoxTenantSettings
                                                         select new LogBoxTenantSettingPM()
                                            {
                                                Id = a.Id,
                                                IsDocumentsArchive = a.IsDocumentsArchive,
                                                CustomerTenantShareImportFile = a.CustomerTenantShareImportFile,
                                                LogBoxAdminUserId = a.LogBoxAdminUserId,
                                                DocumentShareAsDefault = a.DocumentShareAsDefault,
                                                AutoArchiveOnInvoice = a.AutoArchiveOnInvoice,
                                                AutoArchiveOnPODExport   = a.AutoArchiveOnPODExport ,
                                                StockTypeCode = a.StockTypeCode,
                                                         });

           
            return LogBoxTenantSettings;
        }



        public LogBoxTenantSettingPM GetSinglePM(int id)
        {
            var tt = (from a in repository.context.LogBoxTenantSettings
                      where a.Id == id
                      select a).FirstOrDefault();

            LogBoxTenantSettingPM logBoxTenantSettingPM = new LogBoxTenantSettingPM()
            {
                Id = tt.Id,
                IsDocumentsArchive = tt.IsDocumentsArchive,
                CustomerTenantShareImportFile = tt.CustomerTenantShareImportFile,
                LogBoxAdminUserId = tt.LogBoxAdminUserId,
                DocumentShareAsDefault = tt.DocumentShareAsDefault,
                AutoArchiveOnInvoice = tt.AutoArchiveOnInvoice,
                AutoArchiveOnPODExport = tt.AutoArchiveOnPODExport,
                StockTypeCode = tt.StockTypeCode,
                ShowTaxAmountWarning = tt.ShowTaxAmountWarning
            };

            return logBoxTenantSettingPM;
        }



        public IQueryable<LogBoxTenantSettingList> GetIQueryableEntityList(IQueryable<LogBoxTenantSetting> iQueryable)
        {
            IQueryable<LogBoxTenantSettingList> result = from a in iQueryable
                                                         select new LogBoxTenantSettingList()
                                                         {
                                                             Id = a.Id,
                                                             IsDocumentsArchive = a.IsDocumentsArchive,
                                                             CustomerTenantShareImportFile = a.CustomerTenantShareImportFile,

                                                         };
            return result;
        }

        public static LogBoxTenantSettingPM GetSingleLogBoxTenantSettingPM(int id)
        {
            ICommonDataContext context = CommonDataContext.GetContext(id);
            var LogBoxTenantSetting = (from a in context.LogBoxTenantSettings
                                       where a.Id == id
                                       select new LogBoxTenantSettingPM()
                                       {
                                           Id = a.Id,
                                           IsDocumentsArchive = a.IsDocumentsArchive,
                                           CustomerTenantShareImportFile = a.CustomerTenantShareImportFile,
                                           LogBoxAdminUserId = a.LogBoxAdminUserId,
                                           DocumentShareAsDefault = a.DocumentShareAsDefault,
                                           AutoArchiveOnInvoice = a.AutoArchiveOnInvoice,
                                           AutoArchiveOnPODExport = a.AutoArchiveOnPODExport,
                                           StockTypeCode = a.StockTypeCode,
                                       }).FirstOrDefault();

            return LogBoxTenantSetting;
        }

       
      

        public IQueryable<LogBoxTenantSettingList> GetIQueryableEntityList(IQueryable<LogBoxTenantSettingList> iQueryable)
        {
            IQueryable<LogBoxTenantSettingList> result = from a in iQueryable
                                            select new LogBoxTenantSettingList()
                                            {
                                                Id = a.Id,
                                                IsDocumentsArchive = a.IsDocumentsArchive,
                                                CustomerTenantShareImportFile = a.CustomerTenantShareImportFile,
                                              
                                            };
            return result;
        }

        
        public List<LogBoxTenantSettingList> GetLogBoxTenantSettingLists()
        {
            List<LogBoxTenantSettingList> LogBoxTenantSettingLists = (from a in repository.context.LogBoxTenantSettings
                                                         select new LogBoxTenantSettingList()
                                            {
                                                Id = a.Id,
                                                             IsDocumentsArchive = a.IsDocumentsArchive,
                                                             CustomerTenantShareImportFile = a.CustomerTenantShareImportFile,
                                                         }).ToList();


            return LogBoxTenantSettingLists;

        }

        public IQueryable<LogBoxTenantSettingList> GetAllLogBoxTenantSettingLists()
        {
            IQueryable<LogBoxTenantSettingList> LogBoxTenantSettingLists = (from a in repository.context.LogBoxTenantSettings
                                                                            select new LogBoxTenantSettingList()
                                                  {
                                                      Id = a.Id,
                                                                                IsDocumentsArchive = a.IsDocumentsArchive,
                                                                                CustomerTenantShareImportFile = a.CustomerTenantShareImportFile,

                                                                            });


            return LogBoxTenantSettingLists;

        }
      


    }
}