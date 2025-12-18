using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentsFilingMetaDataValueQuery
    {
        DocumentsFilingMetaDataValueRepository repository;



        public DocumentsFilingMetaDataValueQuery(ICommonDataContext context)
        {
            repository = new DocumentsFilingMetaDataValueRepository(context);
        }
        public DocumentsFilingMetaDataValueQuery(int tenant)
        {
            repository = new DocumentsFilingMetaDataValueRepository(tenant);
        }

        public void Create(DocumentsFilingPM parentPM, DocumentsFilingMetaDataValuePM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("DocumentsFilingMetaDataValue", parentPM.Tenant).ToString();
            itemPM.DocumentsFilingId = parentPM.Id;
            itemPM.Tenant = parentPM.Tenant;

            DocumentsFilingMetaDataValue itemPoco = new DocumentsFilingMetaDataValue()
            {
                Id = itemPM.Id,
            };

            DocumentsFilingMetaDataValueMapping.MapEntity(itemPM, itemPoco, true);
            repository.Add(itemPoco);
        }



        public static void UpSert_Del(DocumentsFilingPM documentsFilingPM, string MetaDataTypeCode, string MetaDataTypeValue)
        {
            string documentsMetaDataTypeId = null;
            var commonDataContext = CommonDataContext.GetContext(documentsFilingPM.Tenant);
            DocumentsMetaDataTypeRepository TypesRepo = new DocumentsMetaDataTypeRepository(commonDataContext);
            var pocoMDType = TypesRepo.GetSingleDocumentsMetaDataTypeByCode(MetaDataTypeCode, documentsFilingPM.Tenant);
            if (pocoMDType == null)
            {
                LogMessagingUtil.Instance.AppendLine("GetDocumentsMetaDataTypeVERId is null !!!!! UpSertVERValue - failed ");
                return;
            }
            documentsMetaDataTypeId = pocoMDType.Id;

            var repository = new DocumentsFilingMetaDataValueRepository(commonDataContext);
            var documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(commonDataContext);
            var mydocumentsFilingMetaDataVERValueList = documentsFilingMetaDataValueQuery
                .GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(documentsFilingPM.Id, documentsFilingPM.Tenant)
                //.FirstOrDefault(r => r.DocumentsMetaDataTypeId == documentsMetaDataTypeId);
                .Where(r => r.DocumentsMetaDataTypeId == documentsMetaDataTypeId)
                .ToList();
            DocumentsFilingMetaDataValuePM mydocumentsFilingMetaDataVERValuePM = null;
            if (mydocumentsFilingMetaDataVERValueList.Count() > 1)
            {
                var listId=mydocumentsFilingMetaDataVERValueList.Select(r => r.Id);
                var pocosDelete=repository.All().Where(r => listId.Contains(r.Id)).ToList();
                pocosDelete.ForEach(itemPoco => { repository.Remove(itemPoco); });

                LogMessagingUtil.Instance.AppendLine("UpSert_Del Del there is more then 1 (hd#353339)");

                documentsFilingPM.DocumentsFilingMetaDataValues.Clear();
            }
            else if (mydocumentsFilingMetaDataVERValueList.Count() == 1)
            {
                mydocumentsFilingMetaDataVERValuePM = mydocumentsFilingMetaDataVERValueList.FirstOrDefault();
            }
            

            if (mydocumentsFilingMetaDataVERValuePM != null)
            {
                if (mydocumentsFilingMetaDataVERValuePM.MetaDataValue == MetaDataTypeValue)
                {
                    LogMessagingUtil.Instance.AppendLine("mydocumentsFilingMetaDataVERValue.MetaDataValue == DeclarationNumber ");
                    return;
                }
                var itemPoco = new DocumentsFilingMetaDataValue();

                mydocumentsFilingMetaDataVERValuePM.MetaDataValue = MetaDataTypeValue;
                DocumentsFilingMetaDataValueMapping.MapEntity(mydocumentsFilingMetaDataVERValuePM, itemPoco,true /*false - if false do not map keys !!*/ );
                //var repository = new DocumentsFilingMetaDataValueRepository(commonDataContext);
                repository.Update(itemPoco);
                var pmInMem = documentsFilingPM.DocumentsFilingMetaDataValues.FirstOrDefault(r => r.DocumentsMetaDataTypeId == documentsMetaDataTypeId);
                if (pmInMem != null)
                {
                    pmInMem.MetaDataValue = MetaDataTypeValue;
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("documentsFilingPM.DocumentsFilingMetaDataValues.Add(mydocumentsFilingMetaDataVERValuePM);");//nir ask to delete 
                    
                }
                
            }
            else
            {
                var temp = new DocumentsFilingMetaDataValuePM()
                {
                    DocumentsMetaDataTypeId = documentsMetaDataTypeId,
                    MetaDataValue = MetaDataTypeValue,
                    DocumentsMetaDataTypeCode = MetaDataTypeCode,

                };
                documentsFilingMetaDataValueQuery.Create(documentsFilingPM, temp);
                documentsFilingPM.DocumentsFilingMetaDataValues.Add(temp);


            }
            commonDataContext.SaveChanges();
        }

        public static void UpSert_OLD_MOVE2DELSERT(DocumentsFilingPM documentsFilingPM, string MetaDataTypeCode, string MetaDataTypeValue)
        {
            string documentsMetaDataTypeId = null;
            var commonDataContext = CommonDataContext.GetContext(documentsFilingPM.Tenant);
            DocumentsMetaDataTypeRepository TypesRepo = new DocumentsMetaDataTypeRepository(commonDataContext);
            var pocoMDType = TypesRepo.GetSingleDocumentsMetaDataTypeByCode(MetaDataTypeCode, documentsFilingPM.Tenant);
            if (pocoMDType == null)
            {
                LogMessagingUtil.Instance.AppendLine("GetDocumentsMetaDataTypeVERId is null !!!!! UpSertVERValue - failed ");
                return;
            }
            documentsMetaDataTypeId = pocoMDType.Id;


            var documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(commonDataContext);
            var mydocumentsFilingMetaDataVERValuePM = documentsFilingMetaDataValueQuery
                .GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(documentsFilingPM.Id, documentsFilingPM.Tenant)
                .FirstOrDefault(r => r.DocumentsMetaDataTypeId == documentsMetaDataTypeId);
            if (mydocumentsFilingMetaDataVERValuePM != null)
            {
                if (mydocumentsFilingMetaDataVERValuePM.MetaDataValue == MetaDataTypeValue)
                {
                    LogMessagingUtil.Instance.AppendLine("mydocumentsFilingMetaDataVERValue.MetaDataValue == DeclarationNumber ");
                    return;
                }
                var itemPoco = new DocumentsFilingMetaDataValue();

                mydocumentsFilingMetaDataVERValuePM.MetaDataValue = MetaDataTypeValue;
                DocumentsFilingMetaDataValueMapping.MapEntity(mydocumentsFilingMetaDataVERValuePM, itemPoco, true /*false - if false do not map keys !!*/ );
                var repository = new DocumentsFilingMetaDataValueRepository(commonDataContext);
                repository.Update(itemPoco);
                var pmInMem = documentsFilingPM.DocumentsFilingMetaDataValues.FirstOrDefault(r => r.DocumentsMetaDataTypeId == documentsMetaDataTypeId);
                if (pmInMem != null)
                {
                    pmInMem.MetaDataValue = MetaDataTypeValue;
                }
                else
                {
                    documentsFilingPM.DocumentsFilingMetaDataValues.Add(mydocumentsFilingMetaDataVERValuePM);
                }

            }
            else
            {
                var temp = new DocumentsFilingMetaDataValuePM()
                {
                    DocumentsMetaDataTypeId = documentsMetaDataTypeId,
                    MetaDataValue = MetaDataTypeValue,
                    DocumentsMetaDataTypeCode = MetaDataTypeCode,

                };
                documentsFilingMetaDataValueQuery.Create(documentsFilingPM, temp);
                documentsFilingPM.DocumentsFilingMetaDataValues.Add(temp);


            }
            commonDataContext.SaveChanges();
        }


        public DocumentsFilingMetaDataValueQuery(DocumentsFilingMetaDataValueRepository documentsFilingMetaDataValueRepository)
        {
            repository = documentsFilingMetaDataValueRepository;
        }

        public DocumentsFilingMetaDataValuePM GetSinglePM(string id, int tenant)
        {
            DocumentsFilingMetaDataValue poco = repository.GetSingleDocumentsFilingMetaDataValue(id, tenant);

            return new DocumentsFilingMetaDataValuePM()
                                             {

                                                 Id = poco.Id,
                                                 DocumentsFilingId = poco.DocumentsFilingId,
                                                 DocumentsMetaDataTypeId = poco.DocumentsMetaDataTypeId,
                                                 MetaDataValue = poco.MetaDataValue,
                                                 Tenant = poco.Tenant,
                                             };
        }

        public IQueryable<DocumentsFilingMetaDataValuePM> GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(string documentsFilingId, int tenant)
        {
            IQueryable<DocumentsFilingMetaDataValuePM> documents = from a in repository.context.DocumentsFilingMetaDataValues.Include("DocumentsMetaDataTypes")
                                                                   where a.Tenant == tenant && a.DocumentsFilingId == documentsFilingId
                                                                   select new DocumentsFilingMetaDataValuePM()
                                             {

                                                 Id = a.Id,
                                                 DocumentsFilingId = a.DocumentsFilingId,
                                                 DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                                 MetaDataValue = a.MetaDataValue,
                                                 Tenant = a.Tenant,
                                                 DocumentsMetaDataTypeCode = a.DocumentsMetaDataType != null ? a.DocumentsMetaDataType.Code : ""
                                             };
            return documents;
        }

        public IQueryable<DocumentsFilingMetaDataValuePM> GetDocumentsFilingMetaDataValuePMsByTenant1(int tenant)
        {
            IQueryable<DocumentsFilingMetaDataValuePM> documents = from a in repository.context.DocumentsFilingMetaDataValues
                                                                   where a.Tenant == tenant
                                                                   select new DocumentsFilingMetaDataValuePM()
                                                                   {
                                                                       Id = a.Id,
                                                                       DocumentsFilingId = a.DocumentsFilingId,
                                                                       DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                                                       MetaDataValue = a.MetaDataValue,
                                                                       Tenant = a.Tenant,
                                                                   };
            return documents;
        }

        public IQueryable<DocumentsFilingMetaDataValuePM> GetDocumentsFilingMetaDataValuePMsByTenantAndDocumentIds(int tenant,string[] documentIds)
        {
            IQueryable<DocumentsFilingMetaDataValuePM> documents = from a in repository.context.DocumentsFilingMetaDataValues
                                                                   where a.Tenant == tenant && documentIds.Contains(a.Id)
                                                                   select new DocumentsFilingMetaDataValuePM()
                                                                   {
                                                                       Id = a.Id,
                                                                       DocumentsFilingId = a.DocumentsFilingId,
                                                                       DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                                                       MetaDataValue = a.MetaDataValue,
                                                                       Tenant = a.Tenant,
                                                                   };
            return documents;
        }

        public DocumentsFilingMetaDataValuePM GetDocumentsFilingMetaDataValuePMsByDocumentIdTypeTenant(string documentsFilingId, string Type, int tenant)
        {
            DocumentsFilingMetaDataValuePM documents = (from a in repository.context.DocumentsFilingMetaDataValues.Include("DocumentsMetaDataTypes")
                                                                   where a.Tenant == tenant && a.DocumentsFilingId == documentsFilingId && a.DocumentsMetaDataTypeId == Type
                                                                   select new DocumentsFilingMetaDataValuePM()
                                                                   {

                                                                       Id = a.Id,
                                                                       DocumentsFilingId = a.DocumentsFilingId,
                                                                       DocumentsMetaDataTypeId = a.DocumentsMetaDataTypeId,
                                                                       MetaDataValue = a.MetaDataValue,
                                                                       Tenant = a.Tenant,
                                                                       DocumentsMetaDataTypeCode = a.DocumentsMetaDataType != null ? a.DocumentsMetaDataType.Code : ""
                                                                   }).FirstOrDefault();
            return documents;
        }

		public List<string> GetDocumentsFilingMetaDataValuesPMsByTenantMetaDataValueDocumentsMetaDataTypeId( int tenant, string  CARFI, string courierhawb, string INTGR_R, string integratorCode)
		{
			var documents = (from a in repository.context.DocumentsFilingMetaDataValues.Include("DocumentsMetaDataTypes")
														where a.Tenant == tenant && ((a.DocumentsMetaDataType.Code == CARFI && a.MetaDataValue == courierhawb)|| (a.DocumentsMetaDataType.Code == INTGR_R && a.MetaDataValue == integratorCode))
							                            orderby a.DocumentsFilingId
							                            select a).GroupBy(p => p.DocumentsFilingId).Where(x => x.Count() == 2).Select(y => y.Key).ToList();
			return documents;
		}
		public List<DocumentsFilingMetaDataValue> GetDocumentsFilingMetaDataValuesByCARFI(int tenant, string CARFI)
		{
	        var list = repository.context.DocumentsFilingMetaDataValues
	               .Where(v =>
	               	v.DocumentsMetaDataType.Tenant == tenant &&
	               	v.DocumentsMetaDataType.Code == CARFI &&
	               	v.DocumentsFiling.EntityId == null)
	               .ToList();
            return list;
		}
		public DocumentsFilingMetaDataValue GetDocumentsFilingMetaDataValueByINTGR_R(int tenant, string documentsFilingId, string INTGR_R)
		{
			return repository.context.DocumentsFilingMetaDataValues
				   .Where(v =>
					   v.DocumentsMetaDataType.Tenant == tenant && v.DocumentsFilingId == documentsFilingId &&
					   v.DocumentsMetaDataType.Code == INTGR_R)
				   .FirstOrDefault();
		}
	}
}
