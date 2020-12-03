using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public partial class DocumentTypeQueryService
    {

        ICommonDataContext context;
        //DocumentTypeService service; 

        DocumentTypeQuery query;

        public DocumentTypeQueryService(int tenant)
        {
            context = CommonDataContext.GetContext(tenant);
            //service = new DocumentTypeService(context, tenant); 
            query = new DocumentTypeQuery(tenant);
        }


        public DocumentType GetDocumentTypeById(string Id, int Tenant, string ComputingPartnerName = "")
        {
            try
            {


                var temp = query.GetSinglePM(Id, Tenant);
                return DocumentTypeDataMapping(temp, Tenant,ComputingPartnerName);
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


                var temp = query.GetSinglePMByCode(Code, Tenant);
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
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }

                if (!string.IsNullOrEmpty(MyEntity.Code))
                {
                    temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
                }
                if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
                {
                    ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
                    var MyCode = helper.GetLogitudeCodeTranslation(MyEntity.PartnerCode, ComputingPartnerName, "DocumentType");
                    if (string.IsNullOrEmpty(MyCode))
                    {
                        throw new ApplicationException("DocumentType with Partner Code " + MyEntity.PartnerCode + " doesn't match any record");
                    }
                    temp = query.GetSinglePMByCode(MyCode, Tenant);


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

    }
}