using AmitalCloud.Infrastructure.APITools.Helpers;
using AmitalCloud.Infrastructure.Application.EntityQueryServices;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityKeys;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using AmitalCloud.Infrastructure.Model.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using POCO = AmitalCloud.Infrastructure.Model.EntityClasses ;
namespace AmitalCloud.Infrastructure.APITools.ApiV1
{
    public class DocumentTypeQueryService
    {
        IAmitalCloudContext context;
        public DocumentTypeQueryService(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
        }
        public DocumentType GetDocumentTypeById(string Id, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = GetSinglePM(Id, Tenant);
                return DocumentTypeDataMapping(temp, Tenant, ComputingPartnerName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DocumentType GetDocumentTypeByCode(string Code, int Tenant)
        {
            try
            {


                var temp = GetSinglePMByCode(Code, Tenant);
                return DocumentTypeDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DocumentType DocumentTypeDataMapping(DocumentTypePM MyEntityPM, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var temp = new DocumentType();
                temp.Id = MyEntityPM.Id;
                temp.Code = MyEntityPM.Code;
                temp.Name = MyEntityPM.Name;
                ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
                temp.PartnerCode = helper.GetComputingPartnerCodeTranslation(MyEntityPM.Code, ComputingPartnerName, "DocumentType");
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DocumentTypePM DocumentTypeDataMappingAndValidatin(DocumentType MyEntity, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            try
            {
                var temp = new DocumentTypePM();
                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = GetSinglePM(MyEntity.Id, Tenant);
                }

                if (!string.IsNullOrEmpty(MyEntity.Code))
                {
                    temp = GetSinglePMByCode(MyEntity.Code, Tenant);
                }
                if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
                {
                    ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
                    var MyCode = helper.GetLocalCodeTranslation(MyEntity.PartnerCode, ComputingPartnerName, "DocumentType");
                    if (string.IsNullOrEmpty(MyCode))
                    {
                        throw new ApplicationException("DocumentType with Partner Code " + MyEntity.PartnerCode + " doesn't match any record");
                    }
                    temp = GetSinglePMByCode(MyCode, Tenant);


                }


                if (temp == null)
                {
                    throw new ApplicationException("DocumentType with Code " + MyEntity.Code + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.Code;
                }
                temp.Name = MyEntity.Name;
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.PartnerCode;
                }
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public DocumentTypePM GetDocumentTypeCodeById(string Id, int Tenant)
        {
            try
            {
                return GetSinglePM(Id, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        DocumentTypePM GetSinglePMByCode(string code, int tenant) => GetSinglePMByPredicate(a => a.Code == code, tenant);
        DocumentTypePM GetSinglePM(string id, int tenant) => GetSinglePMByPredicate(a => a.Id == id, tenant);

        DocumentTypePM GetSinglePMByPredicate(Expression<Func<POCO.DocumentType, bool>> predicate, int tenant)
        {
            return new Repository<POCO.DocumentType>(context).GetMulti(predicate, a =>
            new DocumentTypePM(a)
            {
                //DocumentTypeCustomFields = new DocumentTypeCustomFieldQueryService(context)
                //                            .GetMultiByParent<DocumentTypeKeys<string>>(new DocumentTypeKeys<string>() { Id = a.Id }, true, true),
                DocumentTypeCopies = new DocumentTypeCopyQueryService(context)
                                            .GetMultiByParent<DocumentTypeKeys<string>>(new DocumentTypeKeys<string>() { Id = a.Id }, true, true)
            }
            , "DocumentTypeCustomFields"
            ).FirstOrDefault();
        }
    }
}
