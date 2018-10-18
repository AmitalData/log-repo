using System.Collections.Generic;
using System.ServiceModel.DomainServices.Server;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System.Linq;
using Simplog.Data.Helpers;
namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class DocumentOutService: DomainService
    {
        bool isNewEntity;
        private int tenant;
        public DocumentOut Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private DocumentOutPM entityPM;
        private ICommonDataContext objectContext;
        private DocumentOutRepository entityRepository;
        public DocumentOutService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new DocumentOutRepository(objectContext);
        }

        public void Create(DocumentOutPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("DocumentOut", tenant).ToString();
            this.Poco = new DocumentOut();
            this.Poco.Id = this.entityPM.Id;

            DocumentOutValidating.Validate(theEntityPm);
            DocumentOutTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentOutMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(DocumentOutPM theEntityPm , List<DocumentOutCopyPM> documentOutCopyList)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleDocumentOut(theEntityPm.Id, theEntityPm.Tenant);


            if (theEntityPm.IsChangeIssuedDate)
            {
                theEntityPm.IssuedDate = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
                theEntityPm.IsChangeIssuedDate = false;
            }


            DocumentOutCopyRepository documentOutCopyRepository = new DocumentOutCopyRepository(objectContext);
            bool isdocumenOutCopyPocoDirty = false;
            if (!Poco.NeedsRebuild && theEntityPm.NeedsRebuild)
            {
                DocumentOutCopyPM copy =null;
                if (documentOutCopyList != null)
                {
                    copy = documentOutCopyList.Where(d => !string.IsNullOrEmpty(d.LastPrintedByUserId)).FirstOrDefault();
                    if (copy != null)
                    {
                        copy.LastPrintedByUserId = null;
                        copy.LastPrintDate = null;
                        copy.changeOp = ChangeSetOperation.Update;
                       
                    }
                }
                if (copy == null)
                {
                    DocumentOutCopy copyPoco = documentOutCopyRepository.GetThePrintedOnceDocumentOutCopyForDocumentOut(theEntityPm.Id, theEntityPm.Tenant);

                    if (copyPoco != null)
                    {

                        copyPoco.LastPrintedByUserId = null;
                        copyPoco.LastPrintDate = null;
                        documentOutCopyRepository.Update(copyPoco);
                        isdocumenOutCopyPocoDirty = true;
                    }
                   
                }
            }
            DocumentOutValidating.Validate(theEntityPm);
            DocumentOutTracing.Trace(theEntityPm, Poco, isNewEntity);
            DocumentOutMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            #region DocumentOutCopies
            foreach (DocumentOutCopyPM r in documentOutCopyList)
            {
              
                switch (r.changeOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            r.Id = IdCounter.GetNumber("Document", theEntityPm.Tenant).ToString();
                            DocumentOutCopy newDocumentOutCopy = new DocumentOutCopy();
                            newDocumentOutCopy.Id = r.Id;
                            DocumentOutCopyMapping.MapEntity(r, newDocumentOutCopy, isNewEntity);
                            documentOutCopyRepository.Add(newDocumentOutCopy);
                            break;
                        }
                    case ChangeSetOperation.Update:
                        {
                            DocumentOutCopy documentOutCopy = documentOutCopyRepository.GetSingleDocumentOutCopy(r.Id);
                            DocumentOutCopyMapping.MapEntity(r, documentOutCopy, isNewEntity);
                            documentOutCopyRepository.Update(documentOutCopy);
                            break;
                        }
                    case ChangeSetOperation.Delete:
                        {
                            DocumentOutCopy documentOutCopy = documentOutCopyRepository.GetSingleDocumentOutCopy(r.Id);
                            documentOutCopyRepository.Remove(documentOutCopy);
                            break;
                        }
                    case ChangeSetOperation.None:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            # endregion


            if (isdocumenOutCopyPocoDirty)
            {
                documentOutCopyRepository.SubmitChanges();
            }
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
