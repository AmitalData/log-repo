using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteDocumentVersionQuery
    {
         QuoteDocumentVersionRepository repository;
             
        public QuoteDocumentVersionQuery()
        {
            repository = new QuoteDocumentVersionRepository(); 
        }

        public QuoteDocumentVersionQuery(int tenant)
        {
            repository = new QuoteDocumentVersionRepository(tenant);
        }

        public QuoteDocumentVersionQuery(QuoteDocumentVersionRepository quoteDocumentVersionRepository)
        {
            repository = quoteDocumentVersionRepository;
        }
        public QuoteDocumentVersionPM GetSinglePM(string quoteid, int tenant, int versionNumber)
        {
            QuoteDocumentVersionPM entity;
            entity = (from a in repository.quotesContext.QuoteDocumentVersions.Include("User").Include("Contact").Include("Doc")
                      where a.Tenant == tenant && a.QuoteId == quoteid && a.VersionNumber == versionNumber
                      select new QuoteDocumentVersionPM()
                      {
                          QuoteId = a.QuoteId,
                          VersionNumber = a.VersionNumber,
                          Tenant = a.Tenant,
                          CreateDate = a.CreateDate,
                          UpdateDate = a.UpdateDate,
                          CreatedByUserId = a.CreatedByUserId,
                          UpdatedByUserId = a.UpdatedByUserId,
                          VersionType = a.VersionType,
                          DocumentId = a.DocumentId,
                          SendDate = a.SendDate,
                          IsSent = a.IsSent,
                          QuoteTemplateId = a.QuoteTemplateId,
                                
                          CreatedByUserName = a.CreatedByUser.Contact.EnglishName,
                          UpdateByUserName = a.UpdatedByUser.Contact.EnglishName,
                          VersionTypeName = a.VersionType == "G" ? "Generated" : "Uploaded",
                          FileName = a.Doc != null ? a.Doc.FileName : null,
                          FileSize = a.Doc != null ? a.Doc.FileSize : null,
                        
                      }).FirstOrDefault();

            return entity;

        }

        public IQueryable<QuoteDocumentVersionPM> GetQuoteDocumentVersionPMsByTenant(int tenant)
        {
            IQueryable<QuoteDocumentVersionPM> quoteDocumentVersion = from a in repository.quotesContext.QuoteDocumentVersions.Include("User").Include("Contact").Include("Doc")
                                                                      where a.Tenant == tenant
                                                                     select new QuoteDocumentVersionPM()
                                                                      {
                                                                          QuoteId = a.QuoteId,
                                                                          VersionNumber = a.VersionNumber,
                                                                          Tenant = a.Tenant,
                                                                          CreateDate = a.CreateDate,
                                                                          UpdateDate = a.UpdateDate,
                                                                          CreatedByUserId = a.CreatedByUserId,
                                                                          UpdatedByUserId = a.UpdatedByUserId,
                                                                          VersionType = a.VersionType,
                                                                          DocumentId = a.DocumentId,
                                                                          SendDate = a.SendDate,
                                                                          IsSent = a.IsSent,
                                                                          QuoteTemplateId = a.QuoteTemplateId,
                                                                          CreatedByUserName = a.CreatedByUser.Contact.EnglishName,
                                                                          UpdateByUserName = a.UpdatedByUser.Contact.EnglishName,
                                                                          VersionTypeName = a.VersionType == "G" ? "Generated" : "Uploaded",
                                                                          FileName = a.Doc != null ? a.Doc.FileName : null,
                                                                          FileSize = a.Doc != null ? a.Doc.FileSize : null,
                                                                      };
            return quoteDocumentVersion;
        }

        public IQueryable<QuoteDocumentVersionPM> GetQuoteDocumentVersionPMsByQuoteId(string quoteId,int tenant)
        {
            IQueryable<QuoteDocumentVersionPM> quoteDocumentVersion = from a in repository.quotesContext.QuoteDocumentVersions.Include("User").Include("Contact").Include("Doc")
                                                                      where a.Tenant == tenant && a.QuoteId == quoteId
                                                                      select new QuoteDocumentVersionPM()
                                                                      {
                                                                          QuoteId = a.QuoteId,
                                                                          VersionNumber = a.VersionNumber,
                                                                          Tenant = a.Tenant,
                                                                          CreateDate = a.CreateDate,
                                                                          UpdateDate = a.UpdateDate,
                                                                          CreatedByUserId = a.CreatedByUserId,
                                                                          UpdatedByUserId = a.UpdatedByUserId,
                                                                          VersionType = a.VersionType,
                                                                          DocumentId = a.DocumentId,
                                                                          SendDate = a.SendDate,
                                                                          IsSent = a.IsSent,
                                                                          QuoteTemplateId = a.QuoteTemplateId,
                                                                          CreatedByUserName = a.CreatedByUser.Contact.EnglishName,
                                                                          UpdateByUserName = a.UpdatedByUser.Contact.EnglishName,
                                                                          VersionTypeName = a.VersionType == "G" ? "Generated" : "Uploaded",
                                                                          FileName = a.Doc != null ? a.Doc.FileName : null,
                                                                          FileSize = a.Doc != null ? a.Doc.FileSize : null,
                                                                      };
            return quoteDocumentVersion;
        }

        public IQueryable<QuoteDocumentVersionPM> GetQuoteDocumentVersionPMsByQuoteIdAndTemplateId(string quoteId,string templateId ,int tenant)
        {
            IQueryable<QuoteDocumentVersionPM> quoteDocumentVersion = from a in repository.quotesContext.QuoteDocumentVersions.Include("User").Include("Contact").Include("Doc")
                                                                      where a.Tenant == tenant && a.QuoteId == quoteId && a.QuoteTemplateId == templateId
                                                                      select new QuoteDocumentVersionPM()
                                                                      {
                                                                          QuoteId = a.QuoteId,
                                                                          VersionNumber = a.VersionNumber,
                                                                          Tenant = a.Tenant,
                                                                          CreateDate = a.CreateDate,
                                                                          UpdateDate = a.UpdateDate,
                                                                          CreatedByUserId = a.CreatedByUserId,
                                                                          UpdatedByUserId = a.UpdatedByUserId,
                                                                          VersionType = a.VersionType,
                                                                          DocumentId = a.DocumentId,
                                                                          SendDate = a.SendDate,
                                                                          IsSent = a.IsSent,
                                                                          QuoteTemplateId = a.QuoteTemplateId,
                                                                          CreatedByUserName = a.CreatedByUser.Contact.EnglishName,
                                                                          UpdateByUserName = a.UpdatedByUser.Contact.EnglishName,
                                                                          VersionTypeName = a.VersionType == "G" ? "Generated" : "Uploaded",
                                                                          FileName = a.Doc != null ? a.Doc.FileName : null,
                                                                          FileSize = a.Doc != null ? a.Doc.FileSize : null,
                                                                      };
            return quoteDocumentVersion;
        }

        public QuoteDocumentVersionPM GetQuoteDocumentVersionPMByQuoteId(string quoteid, int tenant)
        {
            QuoteDocumentVersionPM entity;
            entity = (from a in repository.quotesContext.QuoteDocumentVersions.Include("Doc")
                      where a.Tenant == tenant && a.QuoteId == quoteid 
                      select new QuoteDocumentVersionPM()
                      {
                          QuoteId = a.QuoteId,
                          VersionNumber = a.VersionNumber,
                          Tenant = a.Tenant,
                          CreateDate = a.CreateDate,
                          UpdateDate = a.UpdateDate,
                          CreatedByUserId = a.CreatedByUserId,
                          UpdatedByUserId = a.UpdatedByUserId,
                          VersionType = a.VersionType,
                          DocumentId = a.DocumentId,
                          SendDate = a.SendDate,
                          IsSent = a.IsSent,
                          QuoteTemplateId = a.QuoteTemplateId,
                          FileName = a.Doc != null ? a.Doc.FileName : null,
                          FileSize = a.Doc != null ? a.Doc.FileSize : null,
                          Extension = a.Doc != null ? a.Doc.Extension : null,
                      }).OrderByDescending(d=>d.VersionNumber).FirstOrDefault();

            return entity;

        }


        public QuoteDocumentVersion GetFirstQuoteDocumentVersionForTenant(int tenant)
        {
            return (from a in repository.quotesContext.QuoteDocumentVersions
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public IQueryable<QuoteDocumentVersionList> GetIQueryableEntityList(IQueryable<QuoteDocumentVersion> iQueryable)
        {
            IQueryable<QuoteDocumentVersionList> result = from quoteDocumentVersion in iQueryable
                                                          select new QuoteDocumentVersionList()
                                                          {
                                                              QuoteId = quoteDocumentVersion.QuoteId,
                                                              VersionNumber = quoteDocumentVersion.VersionNumber,
                                                              Tenant = quoteDocumentVersion.Tenant,
                                                              CreateDate = quoteDocumentVersion.CreateDate,
                                                              UpdateDate = quoteDocumentVersion.UpdateDate,
                                                              CreatedByUserId = quoteDocumentVersion.CreatedByUserId,
                                                              UpdatedByUserId = quoteDocumentVersion.UpdatedByUserId,
                                                              VersionType = quoteDocumentVersion.VersionType,
                                                              DocumentId = quoteDocumentVersion.DocumentId,
                                                              SendDate = quoteDocumentVersion.SendDate,
                                                              IsSent = quoteDocumentVersion.IsSent,
                                                              QuoteTemplateId = quoteDocumentVersion.QuoteTemplateId,
                                                            
                                                          };
            return result;
        }
    }
}