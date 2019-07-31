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
using Logitude.BL.Security;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;


namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentTypeTemplateQuery
    {
        DocumentTypeTemplateRepository repository;

        public DocumentTypeTemplateQuery()
        {
            repository = new DocumentTypeTemplateRepository(); 
        }

        public DocumentTypeTemplateQuery(int tenant)
        {
            repository = new DocumentTypeTemplateRepository(tenant);
        }

        public DocumentTypeTemplateQuery(DocumentTypeTemplateRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<DocumentTypeTemplatePM> GetDocumentTypeTemplatesByDocumentTypeId(string documentTypeId, int tenant)
        {
            IQueryable<DocumentTypeTemplatePM> documentTypeTemplates = (from a in repository.context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                                                                        where a.DocumentTypeId == documentTypeId && a.Tenant == tenant
                                                                        select new DocumentTypeTemplatePM()
                                                                        {
                                                                            Description = a.Description,
                                                                            DocumentTypeId = a.DocumentTypeId,
                                                                            Id = a.Id,
                                                                            LastUpdateDate = a.LastUpdateDate,
                                                                            LastUpdatedByUserId = a.LastUpdatedByUserId,
                                                                            TemplateBody = a.TemplateBody,
                                                                            TemplateType = a.TemplateType,
                                                                            TemplateBodyHtml = a.TemplateBodyHtml,
                                                                            Tenant = a.Tenant,
                                                                            LastUpdateByUserName = a.LastUpdatedByUser.Contact.EnglishName,
                                                                            IsDefault = a.DocumentType != null ? a.DocumentType.DocumentTypeDefaultReportTemplateId == a.Id || a.DocumentType.DocumentTypeDefaultHTMLTemplateId == a.Id ? true : false : false,
                                                                            InActive = a.InActive,
                                                                            EditorTool = a.EditorTool,
                                                                            HorizontalShift = a.HorizontalShift,
                                                                            VerticalShift = a.VerticalShift,
                                                                            Subject = a.Subject,
                                                                            TemplateBodyjson = a.TemplateBodyjson,

                                                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                                            CountryCode = a.CountryCode,

                                                                            InternalRemarks = a.InternalRemarks,
                                                                            Language = a.Language,
                                                                            OriginalTemplateId = a.OriginalTemplateId,
                                                                            OriginalTemplateName = a.OriginalTemplate.Description,
                                                                            From = a.From,
                                                                            ReplyTo = a.ReplyTo,
                                                                            TemplateFooterHtml =a.TemplateFooterHtml,
                                                                            TemplateHeaderHtml = a.TemplateHeaderHtml,
                                                                            TemplateFooterHeight = a.TemplateFooterHeight,
                                                                            TemplateHeaderHeight = a.TemplateHeaderHeight,
                                                                            TemplateTechnologyCode = a.TemplateTechnologyCode,
                                                                            CC = a.CC,
                                                                        });

            return documentTypeTemplates;
        }

        public List<DocumentTypeTemplateList> GetDocumentTypeTemplateListsByDocumentTypeId(string documentTypeId, int tenant)
        {
            List<DocumentTypeTemplateList> documentTypeTemplates = (from a in repository.context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                                                                    where a.DocumentTypeId == documentTypeId && a.Tenant == tenant
                                                                    select new DocumentTypeTemplateList()
                                                                    {
                                                                        Description = a.Description,
                                                                        DocumentTypeId = a.DocumentTypeId,
                                                                        Id = a.Id,
                                                                        LastUpdateDate = a.LastUpdateDate,
                                                                        LastUpdatedByUserId = a.LastUpdatedByUserId,
                                                                        Name = a.Description,
                                                                        TemplateType = a.TemplateType,
                                                                        Tenant = a.Tenant,
                                                                        LastUpdateByUserName = a.LastUpdatedByUser.Contact.EnglishName,
                                                                        IsDefault = a.DocumentType != null ? a.DocumentType.DocumentTypeDefaultReportTemplateId == a.Id || a.DocumentType.DocumentTypeDefaultHTMLTemplateId == a.Id ? true : false : false,
                                                                        InActive = a.InActive,
                                                                        EditorTool = a.EditorTool,
                                                                        HorizontalShift = a.HorizontalShift,
                                                                        VerticalShift = a.VerticalShift,
                                                                        Subject = a.Subject,
                                                                        IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                                        IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                                        CountryCode = a.CountryCode,
                                                                        //IsHaveJsonString = a.TemplateBodyjson!=null ? true:false,
                                                                        InternalRemarks = a.InternalRemarks,
                                                                        Language = a.Language,
                                                                        OriginalTemplateId = a.OriginalTemplateId,
                                                                        OriginalTemplateName = a.OriginalTemplate.Description,
                                                                        From = a.From,
                                                                        ReplyTo = a.ReplyTo,
                                                                        TemplateFooterHeight = a.TemplateFooterHeight,
                                                                        TemplateHeaderHeight = a.TemplateHeaderHeight,
                                                                        TemplateTechnologyCode = a.TemplateTechnologyCode,
                                                                        CC = a.CC,

                                                                    }).ToList();
            return documentTypeTemplates;
        }

        public IQueryable<DocumentTypeTemplateList> GetIQueryableEntityList(IQueryable<DocumentTypeTemplate> iQueryable)
        {
            IQueryable<DocumentTypeTemplateList> result = from entity in iQueryable
                                                          select new DocumentTypeTemplateList()
                                                          {
                                                              Id = entity.Id,
                                                              Tenant = entity.Tenant,
                                                              Description = entity.Description,
                                                              DocumentTypeId = entity.DocumentTypeId,
                                                              LastUpdatedByUserId = entity.LastUpdatedByUserId,
                                                              TemplateType = entity.TemplateType,
                                                              Subject = entity.Subject,
                                                              IsCopiedAtSignup = entity.IsCopiedAtSignup,
                                                              IsEnabledForCustomers = entity.IsEnabledForCustomers,
                                                              CountryCode = entity.CountryCode,
                                                              From = entity.From,
                                                              ReplyTo = entity.ReplyTo,
                                                              InternalRemarks = entity.InternalRemarks,
                                                              Language = entity.Language,
                                                              OriginalTemplateId = entity.OriginalTemplateId,
                                                              OriginalTemplateName = entity.OriginalTemplate.Description,
                                                              TemplateFooterHeight = entity.TemplateFooterHeight,
                                                              TemplateHeaderHeight = entity.TemplateHeaderHeight,
                                                              TemplateTechnologyCode = entity.TemplateTechnologyCode,
                                                              CC = entity.CC,

                                                          };
            return result;
        }

        public IQueryable<DocumentTypeTemplatePM> GetDocumentTypeTemplatePMsByTenant(int tenant)
        {
            return from a in repository.context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                   where a.Tenant == tenant
                   select new DocumentTypeTemplatePM()
                   {
                       Description = a.Description,
                       DocumentTypeId = a.DocumentTypeId,
                       Id = a.Id,
                       LastUpdateDate = a.LastUpdateDate,
                       LastUpdatedByUserId = a.LastUpdatedByUserId,
                       IsDefault = a.DocumentType != null ? a.DocumentType.DocumentTypeDefaultReportTemplateId == a.Id || a.DocumentType.DocumentTypeDefaultHTMLTemplateId == a.Id ? true : false : false,
                       TemplateBody = a.TemplateBody,
                       TemplateBodyHtml = a.TemplateBodyHtml,
                       TemplateType = a.TemplateType,
                       Tenant = a.Tenant,
                       LastUpdateByUserName = a.LastUpdatedByUser.Contact.EnglishName,
                       InActive = a.InActive,
                       EditorTool = a.EditorTool,
                       HorizontalShift = a.HorizontalShift,
                       VerticalShift = a.VerticalShift,
                       Subject = a.Subject,
                       IsCopiedAtSignup = a.IsCopiedAtSignup,
                       IsEnabledForCustomers = a.IsEnabledForCustomers,
                       CountryCode = a.CountryCode,
                       TemplateBodyjson = a.TemplateBodyjson,
                       InternalRemarks = a.InternalRemarks,
                       Language = a.Language,
                       OriginalTemplateId = a.OriginalTemplateId,
                       OriginalTemplateName = a.OriginalTemplate.Description,
                       From = a.From,
                       ReplyTo = a.ReplyTo,
                       TemplateFooterHtml = a.TemplateFooterHtml,
                       TemplateHeaderHtml = a.TemplateHeaderHtml,
                       TemplateFooterHeight = a.TemplateFooterHeight,
                       TemplateHeaderHeight = a.TemplateHeaderHeight,
                       TemplateTechnologyCode = a.TemplateTechnologyCode,
                       DocumentTypeCode = a.DocumentType!=null ? a.DocumentType.Code :"",
                       DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : "",
                       CC = a.CC,
                   };
        }

        public DocumentTypeTemplatePM GetSingleDocumentTypeTemplatePM(string id)
        {
            return (from a in repository.context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                    where a.Id == id
                    select new DocumentTypeTemplatePM()
                    {
                        Description = a.Description,
                        DocumentTypeId = a.DocumentTypeId,
                        Id = a.Id,
                        LastUpdateDate = a.LastUpdateDate,
                        LastUpdatedByUserId = a.LastUpdatedByUserId,
                        IsDefault = a.DocumentType != null ? a.DocumentType.DocumentTypeDefaultReportTemplateId == a.Id || a.DocumentType.DocumentTypeDefaultHTMLTemplateId == a.Id ? true : false : false,
                        TemplateBody = a.TemplateBody,
                        TemplateBodyHtml = a.TemplateBodyHtml,
                        TemplateBodyjson = a.TemplateBodyjson,
                        TemplateType = a.TemplateType,
                        Tenant = a.Tenant,
                        LastUpdateByUserName = a.LastUpdatedByUser.Contact.EnglishName,
                        InActive = a.InActive,
                        EditorTool = a.EditorTool,
                        HorizontalShift = a.HorizontalShift,
                        VerticalShift = a.VerticalShift,
                        Subject = a.Subject,
                        IsCopiedAtSignup = a.IsCopiedAtSignup,
                        IsEnabledForCustomers = a.IsEnabledForCustomers,
                        CountryCode = a.CountryCode,
                        From = a.From,
                        ReplyTo = a.ReplyTo,
                        InternalRemarks = a.InternalRemarks,
                        Language = a.Language,
                        OriginalTemplateId = a.OriginalTemplateId,
                        OriginalTemplateName = a.OriginalTemplate.Description,
                        TemplateFooterHtml = a.TemplateFooterHtml,
                        TemplateHeaderHtml = a.TemplateHeaderHtml,
                        TemplateFooterHeight = a.TemplateFooterHeight,
                        TemplateHeaderHeight = a.TemplateHeaderHeight,
                        TemplateTechnologyCode = a.TemplateTechnologyCode,
                        CC = a.CC,
                    }).FirstOrDefault();
        }



        public List<DocumentTypeTemplatePM> GetDocumentTypeTemplatesByDocumentTypeIdForAutomation(string documentTypeId, int tenant)
        {


            List<DocumentTypeTemplatePM> documentTypeTemplates = (from a in repository.context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                                                                  where a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.InActive == false && a.TemplateType != "S"
                                                                  select new DocumentTypeTemplatePM()
                                                                  {
                                                                      Description = a.Description,
                                                                      DocumentTypeId = a.DocumentTypeId,
                                                                      Id = a.Id,
                                                                      LastUpdateDate = a.LastUpdateDate,
                                                                      LastUpdatedByUserId = a.LastUpdatedByUserId,
                                                                      IsDefault = a.DocumentType != null ? a.DocumentType.DocumentTypeDefaultReportTemplateId == a.Id || a.DocumentType.DocumentTypeDefaultHTMLTemplateId == a.Id ? true : false : false,
                                                                      TemplateBody = a.TemplateBody,
                                                                      TemplateBodyHtml = a.TemplateBodyHtml,
                                                                      TemplateType = a.TemplateType,
                                                                      Tenant = a.Tenant,
                                                                      LastUpdateByUserName = a.LastUpdatedByUser.Contact.EnglishName,
                                                                      ContactEmail = a.LastUpdatedByUser.Contact.Email,
                                                                      InActive = a.InActive,
                                                                      EditorTool = a.EditorTool,
                                                                      HorizontalShift = a.HorizontalShift,
                                                                      VerticalShift = a.VerticalShift,
                                                                      Subject = a.Subject,
                                                                      ObjectTableId = a.DocumentType.ObjectTableId,
                                                                      IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                                      IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                                      CountryCode = a.CountryCode,
                                                                      TemplateBodyjson = a.TemplateBodyjson,
                                                                      InternalRemarks = a.InternalRemarks,
                                                                      Language = a.Language,
                                                                      OriginalTemplateId = a.OriginalTemplateId,
                                                                      OriginalTemplateName = a.OriginalTemplate.Description,
                                                                      From = a.From,
                                                                      ReplyTo = a.ReplyTo,
                                                                      TemplateFooterHtml = a.TemplateFooterHtml,
                                                                      TemplateHeaderHtml = a.TemplateHeaderHtml,
                                                                      TemplateFooterHeight = a.TemplateFooterHeight,
                                                                      TemplateHeaderHeight = a.TemplateHeaderHeight,
                                                                      TemplateTechnologyCode = a.TemplateTechnologyCode,
                                                                      CC = a.CC,
                                                                  }).ToList();
            return documentTypeTemplates;

        }

    



        public DocumentTypeTemplatePM GetSinglePM(string id,int tenant)
        {

           string [] info = id.Split('@');
           if (info.Count() > 1)
           {
               id = info[0];
               tenant = Int32.Parse(info[1]);

           }
            
            return (from a in repository.context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                    where a.Id == id && a.Tenant == tenant
                    select new DocumentTypeTemplatePM()
                    {
                        Description = a.Description,
                        DocumentTypeId = a.DocumentTypeId,
                        Id = a.Id,
                        LastUpdateDate = a.LastUpdateDate,
                        LastUpdatedByUserId = a.LastUpdatedByUserId,
                        IsDefault = a.DocumentType != null ? a.DocumentType.DocumentTypeDefaultReportTemplateId == a.Id || a.DocumentType.DocumentTypeDefaultHTMLTemplateId == a.Id ? true : false : false,
                        TemplateBody = a.TemplateBody,
                        TemplateBodyHtml = a.TemplateBodyHtml,
                        TemplateType = a.TemplateType,
                        Tenant = a.Tenant,
                        LastUpdateByUserName = a.LastUpdatedByUser.Contact.EnglishName,
                        InActive = a.InActive,
                        EditorTool = a.EditorTool,
                        HorizontalShift = a.HorizontalShift,
                        VerticalShift = a.VerticalShift,
                        Subject = a.Subject,
                        IsCopiedAtSignup = a.IsCopiedAtSignup,
                        IsEnabledForCustomers = a.IsEnabledForCustomers,
                        CountryCode = a.CountryCode,
                        TemplateBodyjson = a.TemplateBodyjson,
                        InternalRemarks = a.InternalRemarks,
                        Language = a.Language,
                        OriginalTemplateId = a.OriginalTemplateId,
                        OriginalTemplateName = a.OriginalTemplate.Description,
                        From = a.From,
                        ReplyTo = a.ReplyTo,
                        ObjectTableId = a.DocumentType !=null ? a.DocumentType.ObjectTableId:"",
                        TemplateFooterHtml = a.TemplateFooterHtml,
                        TemplateHeaderHtml = a.TemplateHeaderHtml,
                        TemplateFooterHeight = a.TemplateFooterHeight,
                        TemplateHeaderHeight = a.TemplateHeaderHeight,
                        TemplateTechnologyCode = a.TemplateTechnologyCode,
                        CC = a.CC,

                    }).FirstOrDefault();
        }



        public List<DocumentTypeTemplateList> GetDocumentTypeTemplatesByObjectTableId(string objectTableId, int tenant, string transportModeId, string shipmentLevelCode, bool withFilter)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
            IQueryable<DocumentTypeList> DocumentTypeListsInMyTenant;
            IQueryable<DocumentTypeTemplateList> documentTypeTemplateForDocumentType;
            List<DocumentTypeList> DocumentTypeListsFromTenat0;
            List<DocumentTypeTemplateList> DocumentTypeTemplateLists = new List<DocumentTypeTemplateList>();
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(tenant);

            DocumentTypeListsInMyTenant = GetDocumentTypeInMyTenant(tenant, documentTypeRepository);

            if (string.IsNullOrEmpty(objectTableId))
            {
                DocumentTypeListsFromTenat0 = GetDocumentTypeByTenent(0, documentTypeRepository, tenantPM , withFilter).ToList();
            }
            else
            {
                DocumentTypeListsFromTenat0 = GetDocumentTypeByTenentAndObjectTableId(0, objectTableId, documentTypeRepository, tenantPM, withFilter).ToList();
            }


            #region TransportModeId && ShipmentLevel



            if (!string.IsNullOrEmpty(transportModeId))
            {
                switch (transportModeId)
                {
                    case "A":

                        DocumentTypeListsFromTenat0 = DocumentTypeListsFromTenat0.Where(d => (d.IsAir == true)).ToList();

                        break;


                    case "O":
                        DocumentTypeListsFromTenat0 = DocumentTypeListsFromTenat0.Where(d => (d.IsOcean == true)).ToList();

                        break;

                    case "I":
                        DocumentTypeListsFromTenat0 = DocumentTypeListsFromTenat0.Where(d => (d.IsInland == true)).ToList();

                        break;
                }


            }



            if (!string.IsNullOrEmpty(shipmentLevelCode))
            {
                switch (shipmentLevelCode)
                {
                    case "D":
                        DocumentTypeListsFromTenat0 = DocumentTypeListsFromTenat0.Where(d => (d.IsDirect == true)).ToList();

                        break;


                    case "M":
                        DocumentTypeListsFromTenat0 = DocumentTypeListsFromTenat0.Where(d => (d.IsMaster == true)).ToList();

                        break;

                    case "C":
                        DocumentTypeListsFromTenat0 = DocumentTypeListsFromTenat0.Where(d => (d.IsMaster == true)).ToList();

                        break;

                    case "H":
                        DocumentTypeListsFromTenat0 = DocumentTypeListsFromTenat0.Where(d => (d.IsHouse == true)).ToList();

                        break;
                }
            }


            #endregion


            if (DocumentTypeListsFromTenat0 != null && DocumentTypeListsFromTenat0.Count() > 0)
            {

                foreach (DocumentTypeList item in DocumentTypeListsInMyTenant)
                {
                    DocumentTypeList documentType = DocumentTypeListsFromTenat0.Where(d => d.Code == item.Code).FirstOrDefault();
                    if (documentType != null) DocumentTypeListsFromTenat0.Remove(documentType);
                }

                foreach (DocumentTypeList item in DocumentTypeListsFromTenat0)
                {
                    documentTypeTemplateForDocumentType = (from a in repository.context.DocumentTypeTemplates.Include("LastUpdatedByUser.Contact").Include("DocumentType")
                                                           where   !a.InActive && a.DocumentTypeId == item.Id 
                                                           select new DocumentTypeTemplateList()
                                                           {
                                                               Description = a.Description,
                                                               DocumentTypeId = a.DocumentTypeId,
                                                               Id = a.Id,
                                                               LastUpdateDate = a.LastUpdateDate,
                                                               LastUpdatedByUserId = a.LastUpdatedByUserId,
                                                               IsDefault = a.DocumentType != null ? a.DocumentType.DocumentTypeDefaultReportTemplateId == a.Id || a.DocumentType.DocumentTypeDefaultHTMLTemplateId == a.Id ? true : false : false,
                                                               TemplateType = a.TemplateType,
                                                               Tenant = a.Tenant,
                                                               LastUpdateByUserName = a.LastUpdatedByUser.Contact.EnglishName,
                                                               InActive = a.InActive,
                                                               EditorTool = a.EditorTool,
                                                               HorizontalShift = a.HorizontalShift,
                                                               VerticalShift = a.VerticalShift,
                                                               Subject = a.Subject,
                                                               IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                               IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                               CountryCode = a.CountryCode,
                                                               InternalRemarks = a.InternalRemarks,
                                                               Language = a.Language,
                                                               OriginalTemplateId = a.OriginalTemplateId,
                                                               OriginalTemplateName = a.OriginalTemplate.Description,
                                                               ObjectTableId = a.DocumentType.ObjectTableId,
                                                               DocumentTypeCode = item.Code,
                                                               DocumentTypeName = item.Name,
                                                               From = a.From,
                                                               ReplyTo = a.ReplyTo,
                                                               TemplateFooterHeight = a.TemplateFooterHeight,
                                                               TemplateHeaderHeight = a.TemplateHeaderHeight,
                                                               TemplateTechnologyCode = a.TemplateTechnologyCode,
                                                               CC = a.CC,

                                                           });

                    if (withFilter)
                    {

                        documentTypeTemplateForDocumentType = documentTypeTemplateForDocumentType.Where(a => a.IsEnabledForCustomers == true && (tenantPM.CountryCode == a.CountryCode || string.IsNullOrEmpty(a.CountryCode)));
                    }

                    if (documentTypeTemplateForDocumentType != null && documentTypeTemplateForDocumentType.Count() > 0)
                    {

                        foreach (DocumentTypeTemplateList itemtemplate in documentTypeTemplateForDocumentType)
                        {
                            if (DocumentTypeTemplateLists.Count > 0)
                            {

                                string code = DocumentTypeTemplateLists.Where(d => d.DocumentTypeName == itemtemplate.DocumentTypeName).Max(d => d.DocumentTypeCode);
                                if (!string.IsNullOrEmpty(code)) itemtemplate.IsHideDocumentName = true;

                                else itemtemplate.IsHideDocumentName = false;

                            }

                            DocumentTypeTemplateLists.Add(itemtemplate);

                        }
                    }

                }


            }
            
            FillCountryName(DocumentTypeTemplateLists, tenant);

            return DocumentTypeTemplateLists;

        }


        public List<DocumentTypeTemplateList> GetDocumentTypeTemplateFromLibraryByTenant(int tenant, string documentTypeId, int mytenant, bool withFilter)
        {
            List<DocumentTypeTemplateList> results = new List<DocumentTypeTemplateList>();

            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
            DocumentTypePM documentTypePM = documentTypeQuery.GetDocumentType(documentTypeId);

            if (documentTypePM != null)
            {

                IQueryable<DocumentTypeTemplateList> documentTypeTemplateLists = (from a in repository.context.DocumentTypeTemplates.Include("DocumentType").Include("LastUpdatedByUser.Contact")
                                                                                  where a.Tenant == tenant && !a.InActive && (a.DocumentType.Code == documentTypePM.Code)
                                                                                  select new DocumentTypeTemplateList()
                                                                                  {
                                                                                      Description = a.Description,
                                                                                      DocumentTypeId = a.DocumentTypeId,
                                                                                      Id = a.Id,
                                                                                      LastUpdateDate = a.LastUpdateDate,
                                                                                      LastUpdatedByUserId = a.LastUpdatedByUserId,
                                                                                      IsDefault = a.DocumentType != null ? a.DocumentType.DocumentTypeDefaultReportTemplateId == a.Id || a.DocumentType.DocumentTypeDefaultHTMLTemplateId == a.Id ? true : false : false,
                                                                                      TemplateType = a.TemplateType,
                                                                                      Tenant = a.Tenant,
                                                                                      LastUpdateByUserName = a.LastUpdatedByUser != null ? a.LastUpdatedByUser.Contact != null ? a.LastUpdatedByUser.Contact.EnglishName : "" : "",
                                                                                      ContactEmail = a.LastUpdatedByUser != null ? a.LastUpdatedByUser.Contact != null ? a.LastUpdatedByUser.Contact.Email : "" : "",
                                                                                      InActive = a.InActive,
                                                                                      EditorTool = a.EditorTool,
                                                                                      HorizontalShift = a.HorizontalShift,
                                                                                      VerticalShift = a.VerticalShift,
                                                                                      Subject = a.Subject,
                                                                                      IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                                                      IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                                                      CountryCode = a.CountryCode,
                                                                                      From = a.From,
                                                                                      ReplyTo = a.ReplyTo,
                                                                                      InternalRemarks = a.InternalRemarks,
                                                                                      Language = a.Language,
                                                                                      OriginalTemplateId = a.OriginalTemplateId,
                                                                                      OriginalTemplateName = a.OriginalTemplate.Description,
                                                                                      TemplateFooterHeight = a.TemplateFooterHeight,
                                                                                      TemplateHeaderHeight = a.TemplateHeaderHeight,
                                                                                      TemplateTechnologyCode = a.TemplateTechnologyCode,
                                                                                      CC = a.CC,

                                                                                  });
                if (withFilter)
                {
                    TenantQuery tenantQuery = new TenantQuery(mytenant);
                    TenantPM tenantPM = tenantQuery.GetSinglePM(mytenant);
                    documentTypeTemplateLists = documentTypeTemplateLists.Where(d => d.IsEnabledForCustomers == true && (tenantPM.CountryCode == d.CountryCode || string.IsNullOrEmpty(d.CountryCode)));
                }

                results = documentTypeTemplateLists.ToList();
                FillLastUpdateByUserName(results, 0);
                FillCountryName(results, tenant);


            }


            return results;



        }
        

 

        private void FillCountryName(List<DocumentTypeTemplateList> documentTypeTemplateLists, int tenant)
        {

            if (documentTypeTemplateLists.Count > 0)
            {
                List<string> counryCodes = documentTypeTemplateLists.GroupBy(d => d.CountryCode).Select(d => d.First().CountryCode).ToList();
                if (counryCodes.Count > 0)
                {
                    CountryQuery countryQuery = new CountryQuery(tenant);
                    List<CountryList> countryLists = countryQuery.GetCountryListsByCounryCodeLists(counryCodes, tenant).ToList();

                    foreach (DocumentTypeTemplateList item in documentTypeTemplateLists)
                    {
                        if (item.CountryCode != null)
                        {
                            CountryList countryList = countryLists.Where(d => d.Code == item.CountryCode).FirstOrDefault();
                            if (countryList != null)
                            {
                                item.CountryName = countryList.EnglishName;

                            }
                        }
                    }

                }

            }
        }



        private void FillLastUpdateByUserName(List<DocumentTypeTemplateList>  documentTypeTemplateLists, int tenant)
        {
            if (documentTypeTemplateLists.Count > 0)
            {
                List<string> contactEmailLists = documentTypeTemplateLists.GroupBy(d => d.ContactEmail).Select(d => d.First().ContactEmail).ToList();
                if (contactEmailLists.Count > 0)
                {
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    List<ContactList> contactLists = contactQuery.GetContactListsByEmailLists(contactEmailLists, tenant).ToList();

                    foreach (DocumentTypeTemplateList item in documentTypeTemplateLists)
                    {
                        ContactList contactList = contactLists.Where(d => d.Email == item.ContactEmail).FirstOrDefault();
                        if (contactList != null) item.LastUpdateByUserName = "Customer Care";
                    }

                }

            }
        }

        #region GetDocumentTypeListsByFilter 

        private IQueryable<DocumentTypeList> GetDocumentTypeInMyTenant(int tenant, DocumentTypeRepository documentTypeRepository)
        {
            return from a in documentTypeRepository.context.DocumentTypes
                   where a.Tenant == tenant 
                   select new DocumentTypeList()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       Name = a.Name,
                       Code = a.Code,
                       Notes = a.Notes,
                       IsAir = a.IsAir,
                       IsOcean = a.IsOcean,
                       IsInland = a.IsInland,
                       IsDocIn = a.IsDocIn,
                       IsDocOut = a.IsDocOut,
                       ObjectTableId = a.ObjectTableId,
                       Subject = a.Subject,
                       DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                       DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                       TemplateFormatCode = a.TemplateFormatCode,
                       DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                       InActive = a.InActive,
                       IsMaster = a.IsMaster,
                       IsDirect = a.IsDirect,
                       IsHouse = a.IsHouse,
                       SearchFields = a.SearchFields,
                       CustomControl = a.CustomControl,
                       IsCustomerView = a.IsCustomerView,
                       IsAgentView = a.IsAgentView,
                       IsReadOnly = a.IsReadOnly,
                       LimitedPrintCopyId = a.LimitedPrintCopyId,
                       IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                       IsCopiedAtSignup = a.IsCopiedAtSignup,
                       IsEnabledForCustomers = a.IsEnabledForCustomers,
                       CountryCode = a.CountryCode,
                 

                   };
        }


        private static IQueryable<DocumentTypeList> GetDocumentTypeByTenentAndObjectTableId(int tenant, string objectTableId, DocumentTypeRepository documentTypeRepository, TenantPM tenantPM , bool withFilter)
        {
            IQueryable<DocumentTypeList> result = from a in documentTypeRepository.context.DocumentTypes
                   where a.Tenant == tenant && !a.InActive 
                   select new DocumentTypeList()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       Name = a.Name,
                       Code = a.Code,
                       Notes = a.Notes,
                       IsAir = a.IsAir,
                       IsOcean = a.IsOcean,
                       IsInland = a.IsInland,
                       IsDocIn = a.IsDocIn,
                       IsDocOut = a.IsDocOut,
                       ObjectTableId = a.ObjectTableId,
                       Subject = a.Subject,
                       DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                       DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                       TemplateFormatCode = a.TemplateFormatCode,
                       DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                       InActive = a.InActive,
                       IsMaster = a.IsMaster,
                       IsDirect = a.IsDirect,
                       IsHouse = a.IsHouse,
                       SearchFields = a.SearchFields,
                       CustomControl = a.CustomControl,

                       IsCustomerView = a.IsCustomerView,
                       IsAgentView = a.IsAgentView,
                       IsReadOnly = a.IsReadOnly,
                       LimitedPrintCopyId = a.LimitedPrintCopyId,
                       IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                       IsCopiedAtSignup = a.IsCopiedAtSignup,
                       IsEnabledForCustomers = a.IsEnabledForCustomers,
                       CountryCode = a.CountryCode,
               


                   };

            if (withFilter)
            {
                result = result.Where(a => a.ObjectTableId == objectTableId && a.IsEnabledForCustomers && (tenantPM.CountryCode == a.CountryCode || string.IsNullOrEmpty(a.CountryCode)));
            }

            return result;
        }

        private static IQueryable<DocumentTypeList> GetDocumentTypeByTenent(int tenant, DocumentTypeRepository documentTypeRepository, TenantPM tenantPM ,bool withFilter)
        {

            IQueryable<DocumentTypeList> result =  from a in documentTypeRepository.context.DocumentTypes
                   where a.Tenant == tenant && !a.InActive 
                   select new DocumentTypeList()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       Name = a.Name,
                       Code = a.Code,
                       Notes = a.Notes,
                       IsAir = a.IsAir,
                       IsOcean = a.IsOcean,
                       IsInland = a.IsInland,
                       IsDocIn = a.IsDocIn,
                       IsDocOut = a.IsDocOut,
                       ObjectTableId = a.ObjectTableId,
                       Subject = a.Subject,
                       DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                       DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                       TemplateFormatCode = a.TemplateFormatCode,
                       DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                       InActive = a.InActive,
                       IsMaster = a.IsMaster,
                       IsDirect = a.IsDirect,
                       IsHouse = a.IsHouse,
                       SearchFields = a.SearchFields,
                       CustomControl = a.CustomControl,

                       IsCustomerView = a.IsCustomerView,
                       IsAgentView = a.IsAgentView,
                       IsReadOnly = a.IsReadOnly,
                       LimitedPrintCopyId = a.LimitedPrintCopyId,
                       IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                       IsCopiedAtSignup = a.IsCopiedAtSignup,
                       IsEnabledForCustomers = a.IsEnabledForCustomers,
                       CountryCode = a.CountryCode,
                   };

            if (withFilter)
            {
                result = result.Where(a => a.IsEnabledForCustomers && (tenantPM.CountryCode == a.CountryCode || string.IsNullOrEmpty(a.CountryCode)));
            }

            return result;
        }

        #endregion

        public byte[] GetTemplateBodyHtmlByDocumentTypeTemplateId(string id, int tenant)
        {
            return (from a in repository.context.DocumentTypeTemplates
                    where a.Tenant == tenant && a.Id == id
                    select a.TemplateBodyHtml).FirstOrDefault();
            

        }


        public byte[] GetTemplateHeaderHtmlByDocumentTypeTemplateId(string id, int tenant)
        {
            return (from a in repository.context.DocumentTypeTemplates
                    where a.Tenant == tenant && a.Id == id
                    select a.TemplateHeaderHtml).FirstOrDefault();

        }
        public byte[] GetTemplateFooterHtmlByDocumentTypeTemplateId(string id, int tenant)
        {
            return (from a in repository.context.DocumentTypeTemplates
                    where a.Tenant == tenant && a.Id == id
                    select a.TemplateFooterHtml).FirstOrDefault();

        }


        public byte[] GetTemplateBodyjsonByDocumentTypeTemplateId(string id, int tenant)
        {
            return (from a in repository.context.DocumentTypeTemplates
                    where a.Tenant == tenant && a.Id == id
                    select a.TemplateBodyjson).FirstOrDefault();
            

        }

        public byte[] GetTemplateBodyByDocumentTypeTemplateId(string id, int tenant)
        {
            return (from a in repository.context.DocumentTypeTemplates
                    where a.Tenant == tenant && a.Id == id
                    select a.TemplateBody).FirstOrDefault();


        }


        public List<DocumentTypeTemplatePM> GetDocumentTypeTemplatesByDocumentTypeIds(List<string> documentTypeIds, int tenant)
        {
            List<DocumentTypeTemplatePM> documentTypeTemplates = (from a in repository.context.DocumentTypeTemplates
                                                                  where documentTypeIds.Contains(a.DocumentTypeId) && a.Tenant == tenant
                                                                  select new DocumentTypeTemplatePM()
                                                                  {
                                                                      Description = a.Description,
                                                                      DocumentTypeId = a.DocumentTypeId,
                                                                      Id = a.Id,
                                                                     OriginalTemplateId = a.OriginalTemplateId
                                                                  }).ToList();
            return documentTypeTemplates;

        }

    }
}
