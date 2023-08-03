using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.Data;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Customs.BL.EntityDataMappings;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsDocumentQueryService : EntityQueryService<CustomsDocument, CustomsDocumentKeys, CustomsDocumentPM, object, CustomsDocumentKeys>
    {

        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, CustomsDocumentPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomsDocumentKeys keys = entityKeys as CustomsDocumentKeys;
            CustomsDocumentMetaDataValueQueryService customsDocumentMetaDataValueQueryService = new CustomsDocumentMetaDataValueQueryService(context);
            entityPM.CustomsDocumentMetaDataValues = customsDocumentMetaDataValueQueryService.GetMulti(keys, true);
        }

        public string GetDocumentInIdByCustomsDocId(string customsDocId, int tenant)
        {
            if (String.IsNullOrWhiteSpace(customsDocId)) return "";
            return repository.GetDocumentInIdByCustomsDocId(customsDocId, tenant);
        }

        //Yuval Chalup 03.11.2014 TASK-4238 ---> // Mirit 16/04/15 12570
        public List<CustomsDocumentPM> GetDeclarationDocumentList(string parentEntityId, string parentEntityCode, int tenant)
        {
            using (var s = (this.context as DbContextBase).CreateLogger())//check genrated sql 
            {
                var myCustomsDocumentPointerRepository = new CustomsDocumentPointerRepository(this.context);
                //var myQcustomsDocumentPointerList = myCustomsDocumentPointerRepository.GetQParentDocumentPointer(parentEntityId, parentEntityCode, tenant);


                var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(this.context);
                var q = (
                    from cdp in myCustomsDocumentPointerRepository.GetQParentDocumentPointer(parentEntityId, parentEntityCode, tenant)
                    join cdt in customsDocumentsTicketRepository.GetAll(tenant) on cdp.CustomsDocumentsTicketId equals cdt.Id
                    select cdt
                            );
                var q2 = (from cdt in q
                          join cd in repository.GetAll(tenant) on cdt.DocumentsFilingId equals cd.DocumentsFilingId
                          select cd
                             )
                             .Distinct();


                var customsDocumentListPocos = q2.ToList();

                var customsDocumentListPMs = customsDocumentListPocos.Select(poko => GetEntityPM(poko)).ToList();
                return customsDocumentListPMs;
            }

        }

        public List<CustomsDocumentPM> GetDeclarationMandatoryTicketList(string parentEntityId, string parentEntityCode, int tenant)
        {
            ///*** 1 min b4 deloy  - where not Send to Mehes !!!!!!
            using (var s = (this.context as DbContextBase).CreateLogger())//check genrated sql 
            {
                var myCustomsDocumentPointerRepository = new CustomsDocumentPointerRepository(this.context);
                var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(this.context);
                var q = (
                    from cdp in myCustomsDocumentPointerRepository.GetQParentDocumentPointer(parentEntityId, parentEntityCode, tenant)
                    join cdt in customsDocumentsTicketRepository.GetAll(tenant)
                    on cdp.CustomsDocumentsTicketId equals cdt.Id
                    select cdt);

                q = q.Where(rec => rec.IsSendMandatory == true);



                var q2 = (from cdt in q
                          join cd in repository.GetAll(tenant) on cdt.DocumentsFilingId equals cd.DocumentsFilingId
                          select cd
                             ).Distinct();

                q2 = q2.Where(r => r.DocumentStatusCode != "1");// 1 min b4 deloy - NOT SEND!!!

                var customsDocumentListPocos = q2.ToList();

                customsDocumentListPocos = customsDocumentListPocos.Where(r => string.IsNullOrWhiteSpace(r.CustomsDocId)).ToList();  // 1 min b4 deloy  - NO Custom REF!!!


                var customsDocumentListPMs = customsDocumentListPocos.Select(poko => GetEntityPM(poko)).ToList();
                return customsDocumentListPMs;
            }

        }

        private List<CustomsDocumentPM> GetDeclarationDocumentListOld(string parentEntityId, string parentEntityCode, int tenant)
        {
            var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(this.context);
            var customsDocumentPointerList = myCustomsDocumentPointerQueryService.GetParentDocumentPointer(parentEntityId, parentEntityCode, tenant);
            if (customsDocumentPointerList == null)
            {
                return null;
            }
            /*var pointerIdList = customsDocumentPointerList.Select(rec => rec.Id).ToList();
            var customsDocumentListPocos = this.repository.GetCustomsDocumentListByPointerList(pointerIdList, tenant);
            var customsDocumentListPMs = customsDocumentListPocos.Select(poko => GetEntityPM(poko)).ToList();
            return customsDocumentListPMs;*/

            var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(this.context);
            var q = (from cdp in customsDocumentPointerList
                     join cdt in customsDocumentsTicketRepository.GetAll(tenant) on cdp.CustomsDocumentsTicketId equals cdt.Id
                     select cdt
                        );
            var q2 = (from cdt in q
                      join cd in repository.GetAll(tenant) on cdt.DocumentsFilingId equals cd.DocumentsFilingId
                      select cd
                         );

            q2 = q2.Distinct();
            var customsDocumentListPocos = q2.ToList();
            var customsDocumentListPMs = customsDocumentListPocos.Select(poko => GetEntityPM(poko)).ToList();
            return customsDocumentListPMs;
        }

        //Yuval Chalup 03.11.2014 TASK-4238 --->

        public CustomsDocumentPM GetSingleCustomsDocumentPMWithDeclarationId(string id, int tenant)
        {
            DocumentsFilingRepository documentFilingRep = new DocumentsFilingRepository(tenant);
            CustomsDocumentPM documentPM = GetSingle(id, true, false);
            if (documentPM != null)
            {
                DocumentsFiling documentFiling = documentFilingRep.GetSingleDocumentsFiling(id, tenant);
                //CustomsDocumentPM documentPM = GetSingle(id, true, false);
                string objecttableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                if (documentFiling.ObjectTableId == objecttableId)
                {
                    documentPM.DeclarationId = documentFiling.EntityId;
                }


            }
            return documentPM;
        }


        public List<CustomsDocumentPM> GetLoadTest(int tenant, int top, List<string> keys = null)
        {
            var lst30 = DateTime.Now.Subtract(TimeSpan.FromDays(131));
            var q = this.repository.GetAll(tenant)
                .Where(rec =>
                    rec.Tenant == tenant &&
                   //rec.CreateDateTime.Value > lst30 &&
                   rec.CustomRecievedDate.Value > lst30  );
            if (keys != null)
            {
                q = q.Where(rec => keys.Contains(rec.DocumentsFilingId));
            }
            var pocos = q
                    .Take(top).ToList();
            var pmList = pocos.Select(poco => this.GetEntityPM(poco, false, null))
               .ToList();
            return pmList;


        }
        public List<CustomsDocumentPM> GetCustomsDocumentList(List<string> documentsFilingIdList, int tenant)
        {
            var customsDocumentListPocos = repository.GetAll(tenant).Where(rec => documentsFilingIdList.Contains(rec.DocumentsFilingId)).ToList();
            var customsDocumentListPMs = customsDocumentListPocos.Select(poko => GetEntityPM(poko)).ToList();
            return customsDocumentListPMs;
        }

        public List<CustomsDocumentPM> GetCustomsDocumentPMListWithoutRequestedDoc(GetTicketsParams parameters, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
            var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(context);
            //var q = (from cdp in customsDocumentPointerQueryService.GetCustomsDocumentPointerList(parameters, tenant)
            //         join cdt in customsDocumentsTicketRepository.GetAll(tenant) on cdp.CustomsDocumentsTicketId equals cdt.Id
            //         select cdt
            //            );
            var cdp = customsDocumentPointerQueryService.GetCustomsDocumentPointerList(parameters, tenant);
            var customsDocumentsTicketIds = cdp.Select(r=>r.CustomsDocumentsTicketId).ToList();
            var q = (from cdt in customsDocumentsTicketRepository.GetAll(tenant).Where(r=> customsDocumentsTicketIds.Contains(r.Id))
                     select cdt
                        );

            var q2 = (from cdt in q where cdt.RequestedCustomsDocId == null
                      join cd in repository.GetAll(tenant) on cdt.DocumentsFilingId equals cd.DocumentsFilingId
                      select cd
                         );

            q2 = q2.Distinct();
            var customsDocumentListPocos = q2.ToList();
            var customsDocumentListPMs = customsDocumentListPocos.Select(poko => GetEntityPM(poko)).ToList();
            return customsDocumentListPMs;
        }

        public List<CustomsDocumentPM> GetCustomsDocumentPMListWithoutRequestedDocAndDeclarationAmendmentDocs(GetTicketsParams parameters, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
            var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(context);
            //var q = (from cdp in customsDocumentPointerQueryService.GetCustomsDocumentPointerList(parameters, tenant)
            //         where  cdp.Child1EntityCode!= "DeclarationAmendment"
            //         join cdt in customsDocumentsTicketRepository.GetAll(tenant) on cdp.CustomsDocumentsTicketId equals cdt.Id
            //         select cdt
            //            );

            var cdp =customsDocumentPointerQueryService.GetCustomsDocumentPointerList(parameters, tenant).Where(r=>r.Child1EntityCode != "DeclarationAmendment");
            var customsDocumentsTicketIds = cdp.Select(r => r.CustomsDocumentsTicketId);
            var q = (from cdt in customsDocumentsTicketRepository.GetAll(tenant).Where(r => customsDocumentsTicketIds.Contains(r.Id))
                     select cdt
                        );
            var q2 = (from cdt in q
                       join cd in repository.GetAll(tenant) on cdt.DocumentsFilingId equals cd.DocumentsFilingId
                      select cd
                         );
 

            q2 = q2.Distinct();
            var customsDocumentListPocos = q2.ToList();
            var customsDocumentListPMs = customsDocumentListPocos.Select(poko => GetEntityPM(poko)).ToList();
            return customsDocumentListPMs;
        }
        public List<CustomsDocumentPM> GetCustomsDocumentPMListWithoutRequestedDocParentOnly(GetTicketsParams parameters, int tenant, bool getComposition = false)
        {
            ICustomContext context = MainContext as CustomContext;
            var customsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
            var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(context);
            var q = (from cdp in customsDocumentPointerQueryService.GetCustomsDocumentPointerListParentOnly(parameters, tenant)
                     join cdt in customsDocumentsTicketRepository.GetAll(tenant) on cdp.CustomsDocumentsTicketId equals cdt.Id
                     select cdt
                        );
            var q2 = (from cdt in q
                      where cdt.RequestedCustomsDocId == null
                      join cd in repository.GetAll(tenant) on cdt.DocumentsFilingId equals cd.DocumentsFilingId
                      select cd
                         );

            q2 = q2.Distinct();
            var customsDocumentListPocos = q2.ToList();
            var customsDocumentListPMs = customsDocumentListPocos.Select(poko =>
            GetEntityPM(poko,
                        getComposition,
                        new CustomsDocumentKeys()
                        {
                            DocumentsFilingId = poko.DocumentsFilingId
                        })
                ).ToList();
            return customsDocumentListPMs;
        }

        public string GetDocumentDeclarationId(string declarationId, int tenant, out string DeclarationVersion)
        {
            string DocumentDeclarationId = "";
            DeclarationVersion = null; ;
            //CacheMessageSender
            DocumentTypeRepository documentTypeRep = new DocumentTypeRepository(tenant);
            DocumentType type = documentTypeRep.GetSingleDocumentTypeByCode("DEC", tenant);
            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTableRep.GetObjectTableByName("Customs.Declaration", tenant, true);
            DocumentsFilingRepository documentRepository = new DocumentsFilingRepository(tenant);
            if (type != null)
            {
                if (type.ObjectTableId == objectTable.Id)
                {
                    string DocumentsFilingId = null;
                    bool checkDeleted = true;
                    if (!checkDeleted)
                    {
                        DocumentDeclarationId =
         documentRepository.GetDocumentIdByDocumentType(type.Id, objectTable.Id, declarationId, tenant, out DocumentsFilingId);

                    }
                    else
                    {
                        var res = GetDocumentIdByDocumentTypeNotDeleted(declarationId, tenant, type, objectTable, documentRepository);
                        if (res != null)
                        {
                            DocumentsFilingId = res.DocumentsFilingId;
                            DocumentDeclarationId = res.DocumentId;
                        }
                    }
                    DocumentsMetaDataTypeRepository TypesRepo = new DocumentsMetaDataTypeRepository(tenant);
                    var pocoMDType = TypesRepo.GetSingleDocumentsMetaDataTypeByCode("VER", tenant);
                    if (pocoMDType != null)
                    {


                        var repository = new DocumentsFilingMetaDataValueRepository(tenant);
                        var mdValues = repository.GetDocumentsFilingMetaDataValuesByTenantDocFilingId(tenant, DocumentsFilingId);
                        var mdValue = mdValues.FirstOrDefault(r => r.DocumentsMetaDataTypeId == pocoMDType.Id);
                        if (mdValue != null)
                        {
                            DeclarationVersion = mdValue.MetaDataValue;
                        }
                    }
                }
            }
            return DocumentDeclarationId;
        }

        private static ResultByDocumentType GetDocumentIdByDocumentTypeNotDeleted(string declarationId, int tenant, DocumentType type, ObjectTable objectTable, DocumentsFilingRepository documentRepository)
        {
            string DocumentsFilingId;
            string documentTypeId = type.Id;
            var dtoList = documentRepository.GetByEntity(objectTable.Id, declarationId, tenant);
            dtoList = dtoList.Where(a => a.DocumentTypeId == documentTypeId).ToList();
            var list = dtoList.Select(r => r.Id).ToList();
            if (!LogitudeSettings.GetLogitudeCustomsSettingsMInject(tenant).IsConnectedToUniFreight)
            {
                var dtoDF = dtoList.FirstOrDefault();
                if (dtoDF == null)
                {
                    return null;
                }
                
                return new ResultByDocumentType()
                {
                    DocumentId = dtoDF.DocumentId,
                    DocumentsFilingId = dtoDF.Id
                };

            }
            var gDMFILINGRepository = new GDMFILINGRepository(tenant);
            var filingNotDeletedList = gDMFILINGRepository.GetNotDeleted(list);
            var filingNotDeleted=filingNotDeletedList.FirstOrDefault();
            if (filingNotDeleted==null)
            {
                return null;
            }
            var rec4DocumentTypeId=dtoList.FirstOrDefault(r => r.Id == filingNotDeleted.COMID);
            if (rec4DocumentTypeId == null) return null;
            DocumentsFilingId = filingNotDeleted.COMID;
            return new ResultByDocumentType()
            {
                DocumentId= rec4DocumentTypeId.DocumentId,
                DocumentsFilingId = DocumentsFilingId
            };
        }

        public CustomsDocumentPM GetSingleByDocFileId(string docFileId, int tenant)
        {
            var query =
                  (from rec in context.CustomsDocuments
                   join o in context.OcrDocuments on rec.DocumentsFilingId equals o.DocId into ocrDocs
                   from o in ocrDocs.DefaultIfEmpty()
                   where rec.DocumentsFilingId == docFileId && rec.Tenant == tenant
                   select new CustomsDocumentPM()
                   {
                       DocumentsFilingId = rec.DocumentsFilingId,
                       Tenant = rec.Tenant,
                       CustomsDocId = rec.CustomsDocId,
                       DocumentStatusCode = rec.DocumentStatusCode,
                       DocumentRemarks = rec.DocumentRemarks,
                       DocumentTypeCode = rec.DocumentTypeCode,
                       IsMetaDataReady = rec.IsMetaDataReady,
                       CustomRecievedDate = rec.CustomRecievedDate,
                       DocumentVersion = rec.DocumentVersion,
                       ExternalAttachmentId = rec.ExternalAttachmentId,
                       IsPartOfDeclaration = rec.IsPartOfDeclaration,
                       OcrStatusCode = o != null ? o.StatusCode : null,
                       OcrScore = o != null ? (decimal)o.Score : -1, 

                   });



            return query.FirstOrDefault();

        }

        public class ResultByDocumentType
        {
            public string DocumentsFilingId { get; set; }
            public string DocumentId { get; set; }
            
        }
        public List<CustomsDocumentPM> GetDeclarationDocumentWithConnectNotValid(string parentEntityId, string parentEntityCode, int tenant)
        {
            using (var s = (this.context as DbContextBase).CreateLogger()) 
            {
                var myCustomsDocumentPointerRepository = new CustomsDocumentPointerRepository(this.context);
 

                var customsDocumentsTicketRepository = new CustomsDocumentsTicketRepository(this.context);
                var q = (
                    from cdp in myCustomsDocumentPointerRepository.GetQParentDocumentPointer(parentEntityId, parentEntityCode, tenant)
                    join cdt in customsDocumentsTicketRepository.GetAll(tenant) on cdp.CustomsDocumentsTicketId equals cdt.Id
                    join cd in repository.GetAll(tenant) on cdt.DocumentsFilingId equals cd.DocumentsFilingId

                    where ((cdp.Child1EntityCode == "SupplierInvoice" && string.IsNullOrEmpty(cdp.Child1EntityId))
                          || (cdp.Child2EntityCode == "SupplierInvoiceItem" && string.IsNullOrEmpty(cdp.Child2EntityId)))
                    select cd
                    )
                             .Distinct();
                 

                           // );

                //var q2 = (from  cdp, cdt  in q 
                         
                //          where   ((cdt.Child1EntityCode== "SupplierInvoice" && string.IsNullOrEmpty( cdt.Child1EntityId))
                //          || (cdt.Child2EntityCode == "SupplierInvoiceItem" && string.IsNullOrEmpty(cdt.Child2EntityId)))
                //          select cd
                //             )
                //             .Distinct();


                var customsDocumentListPocos = q.ToList();

                var customsDocumentListPMs = customsDocumentListPocos.Select(poko => GetEntityPM(poko)).ToList();
                return customsDocumentListPMs;
            }

        }
    }
}
