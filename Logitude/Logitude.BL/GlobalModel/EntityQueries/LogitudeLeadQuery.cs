using System;
using System.Linq;
using System.Web;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class LogitudeLeadQuery
    {

        LogitudeLeadRepository repository;
             
        public LogitudeLeadQuery()
        {
            repository = new LogitudeLeadRepository(); 
        }



        public LogitudeLeadQuery(LogitudeLeadRepository logitudeleadrepository)
        {
            repository = logitudeleadrepository;
        }


        public LogitudeLeadPM GetSinglePM(string id)
        {
            LogitudeLeadPM entity;
            entity = (from a in repository.context.LogitudeLeads
                      where a.Id == id
                      select new LogitudeLeadPM()
                      {
                          Id = a.Id,
                          TenantNumber = a.TenantNumber,
                          PhoneNumber = a.PhoneNumber,
                          CompanyName = a.CompanyName,
                          ContactName = a.ContactName,
                          Email = a.Email,
                          IsEmailVerified = a.IsEmailVerified,
                          IsSentToCustomer = a.IsSentToCustomer,
                          CreateDate = a.CreateDate,
                          LastUpdateDate = a.LastUpdateDate,
                          StatusCode = a.StatusCode,
                          Comments = a.Comments,
                          NumberOfBranches = a.NumberOfBranches,
                          Country = a.Country,
                          NumberOfUsers = a.NumberOfUsers,
                          RequestType = a.RequestType,
                          CustomerId = a.CustomerId,
                          IsUserOpened = a.IsUserOpened,
                          OpportunityId = a.OpportunityId,
                          SearchFields = a.SearchFields,
                          IsUserEmailSent = a.IsUserEmailSent,
                          CASSCode = a.CASSCode,
                          IATACode = a.IATACode,
                          LeadSource = a.LeadSource,
                          City = a.City,
                          PackageCode = a.PackageCode,
                          VatNumber = a.VatNumber,
                          ZipCode = a.ZipCode,
                          State = a.State,
                          Street = a.Street,
                          ClientId = a.ClientId,
                          LeadOrigin = a.LeadOrigin,
                          Campaign = a.Campaign,
                      }).FirstOrDefault();

            return entity;
        }

        public LogitudeLeadPM GetSingleLogitudeLeadPM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "LogitudeLeadPM" + id + tenant;
                LogitudeLeadPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.LogitudeLeads.Include("EntityStatus").Include("LogitudeLeadCategory")
                                            
                                              select new LogitudeLeadPM()
                                              {
                                                  Id = a.Id,
                                                  TenantNumber = a.TenantNumber,
                                                  PhoneNumber = a.PhoneNumber,
                                                  CompanyName = a.CompanyName,
                                                  ContactName = a.ContactName,
                                                  Email = a.Email,
                                                  IsEmailVerified = a.IsEmailVerified,
                                                  IsSentToCustomer = a.IsSentToCustomer,
                                                  CreateDate = a.CreateDate,
                                                  LastUpdateDate = a.LastUpdateDate,
                                                  StatusCode = a.StatusCode,
                                                  Comments = a.Comments,
                                                  NumberOfBranches = a.NumberOfBranches,
                                                  Country = a.Country,
                                                  NumberOfUsers = a.NumberOfUsers,
                                                  RequestType = a.RequestType,
                                                  CustomerId = a.CustomerId,
                                                  IsUserOpened = a.IsUserOpened,
                                                  OpportunityId = a.OpportunityId,
                                                  SearchFields = a.SearchFields,
                                                  IsUserEmailSent = a.IsUserEmailSent,
                                                  CASSCode = a.CASSCode,
                                                  IATACode = a.IATACode,
                                                  LeadSource = a.LeadSource,
                                                  City = a.City,
                                                  PackageCode = a.PackageCode,
                                                  VatNumber = a.VatNumber,
                                                  ZipCode = a.ZipCode,
                                                  State = a.State,
                                                  Street = a.Street,
                                                  ClientId = a.ClientId,
                                                  LeadOrigin = a.LeadOrigin,
                                                  Campaign = a.Campaign,
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "LogitudeLeadPM" + s.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (LogitudeLeadPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (LogitudeLeadPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.LogitudeLeads.Include("EntityStatus").Include("LogitudeLeadCategory")
                              where a.TenantNumber == tenant && a.Id == id
                              select new LogitudeLeadPM()
                              {
                                  Id = a.Id,
                                  TenantNumber = a.TenantNumber,
                                  PhoneNumber = a.PhoneNumber,
                                  CompanyName = a.CompanyName,
                                  ContactName = a.ContactName,
                                  Email = a.Email,
                                  IsEmailVerified = a.IsEmailVerified,
                                  IsSentToCustomer = a.IsSentToCustomer,
                                  CreateDate = a.CreateDate,
                                  LastUpdateDate = a.LastUpdateDate,
                                  StatusCode = a.StatusCode,
                                  Comments = a.Comments,
                                  NumberOfBranches = a.NumberOfBranches,
                                  Country = a.Country,
                                  NumberOfUsers = a.NumberOfUsers,
                                  RequestType = a.RequestType,
                                  CustomerId = a.CustomerId,
                                  IsUserOpened = a.IsUserOpened,
                                  OpportunityId = a.OpportunityId,
                                  SearchFields = a.SearchFields,
                                  IsUserEmailSent = a.IsUserEmailSent,
                                  CASSCode = a.CASSCode,
                                  IATACode = a.IATACode,
                                  LeadSource = a.LeadSource,
                                  City = a.City,
                                  PackageCode = a.PackageCode,
                                  VatNumber = a.VatNumber,
                                  ZipCode = a.ZipCode,
                                  State = a.State,
                                  Street = a.Street,
                                  ClientId = a.ClientId,
                                  LeadOrigin = a.LeadOrigin,
                                  Campaign = a.Campaign,
                              }).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }
        
        public IQueryable<LogitudeLeadPM> GetLogitudeLeadPMsByTenant(int tenant)
        {
            IQueryable<LogitudeLeadPM> logitudeleadSetting = from a in repository.context.LogitudeLeads
                                                             where a.TenantNumber == tenant
                                                             select new LogitudeLeadPM()
                                                                      {
                                                                          Id = a.Id,
                                                                          TenantNumber = a.TenantNumber,
                                                                          PhoneNumber = a.PhoneNumber,
                                                                          CompanyName = a.CompanyName,
                                                                          ContactName = a.ContactName,
                                                                          Email = a.Email,
                                                                          IsEmailVerified = a.IsEmailVerified,
                                                                          IsSentToCustomer = a.IsSentToCustomer,
                                                                          CreateDate = a.CreateDate,
                                                                          LastUpdateDate = a.LastUpdateDate,
                                                                          StatusCode = a.StatusCode,
                                                                          Comments = a.Comments,
                                                                          NumberOfBranches = a.NumberOfBranches,
                                                                          Country = a.Country,
                                                                          NumberOfUsers = a.NumberOfUsers,
                                                                          RequestType = a.RequestType,
                                                                          CustomerId = a.CustomerId,
                                                                          IsUserOpened = a.IsUserOpened,
                                                                          OpportunityId = a.OpportunityId,
                                                                          SearchFields = a.SearchFields,
                                                                          IsUserEmailSent = a.IsUserEmailSent,
                                                                          CASSCode = a.CASSCode,
                                                                          IATACode = a.IATACode,
                                                                          LeadSource = a.LeadSource,
                                                                          City = a.City,
                                                                          PackageCode = a.PackageCode,
                                                                          VatNumber = a.VatNumber,
                                                                          ZipCode = a.ZipCode,
                                                                          State = a.State,
                                                                          Street = a.Street,
                                                                          ClientId = a.ClientId,
                                                                          LeadOrigin = a.LeadOrigin,
                                                                          Campaign = a.Campaign,
                                                             };

            return logitudeleadSetting;
        }
        
        public IQueryable<LogitudeLeadList> GetIQueryableEntityList(IQueryable<LogitudeLead> iQueryable )
        {
            IQueryable<LogitudeLeadList> result = from a in iQueryable
                                              
                                                  select new LogitudeLeadList()
                                                          {
                                                              Id = a.Id,
                                                              TenantNumber = a.TenantNumber,
                                                              PhoneNumber = a.PhoneNumber,
                                                              CompanyName = a.CompanyName,
                                                              ContactName = a.ContactName,
                                                              Email = a.Email,
                                                              IsEmailVerified = a.IsEmailVerified,
                                                              IsSentToCustomer = a.IsSentToCustomer,
                                                              CreateDate = a.CreateDate,
                                                              LastUpdateDate = a.LastUpdateDate,
                                                              StatusCode = a.StatusCode,
                                                              Comments = a.Comments,
                                                              NumberOfBranches = a.NumberOfBranches,
                                                              Country = a.Country,
                                                              NumberOfUsers = a.NumberOfUsers,
                                                              RequestType = a.RequestType,
                                                              CustomerId = a.CustomerId,
                                                              IsUserOpened = a.IsUserOpened,
                                                              OpportunityId = a.OpportunityId,
                                                              SearchFields = a.SearchFields,
                                                              IsUserEmailSent = a.IsUserEmailSent,
                                                              CASSCode = a.CASSCode,
                                                              IATACode = a.IATACode,
                                                              LeadSource = a.LeadSource,
                                                              City = a.City,
                                                              PackageCode = a.PackageCode,
                                                              ZipCode = a.ZipCode,
                                                              State = a.State,
                                                              Street = a.Street,
                                                              ClientId = a.ClientId,
                                                              LeadOrigin = a.LeadOrigin,
                                                              Campaign = a.Campaign,

                                                  };
            return result;
        }

        public LogitudeLead GetFirstQuoteTemplateSettingForTenant(string id)
        {
            return (from a in repository.context.LogitudeLeads
                    where a.Id == id
                    select a).FirstOrDefault();
        }
    }
}
