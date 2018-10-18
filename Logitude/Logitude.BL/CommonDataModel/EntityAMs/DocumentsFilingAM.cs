using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.DataContracts;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityAMs
{
    public class DocumentsFilingAM
    {
         
        public int ImporterTenant { get; set; } 
        public CodeProperties DocumentType { get; set; }
        public string EntityNumber { get; set; }
        public string ObjectTableName { get; set; }
        public string Description { get; set; }
        public int? FileSize { get; set; }
        public string ForwarderDocumentId { get; set; }
        public bool DontAddToQueue { get; set; }
        public byte[] FileData { get; set; }
        public bool IsSharedWithCustomer { get; set; }
        public List<DocumentsFilingMetaDataValueAM> DocumentsFilingMetaDataValues { get; set; }
        //public virtual List<DocumentsFilingMetaDataValueAM> DocumentsFilingMetaDataValues
        //{
        //    get
        //    {
        //        if (documentsFilingMetaDataValues == null)
        //        {
        //            documentsFilingMetaDataValues = new List<DocumentsFilingMetaDataValueAM>();
        //        }
        //        return documentsFilingMetaDataValues;
        //    }
        //    set { documentsFilingMetaDataValues = value; }
        //}

        public string Code { get; set; }
        public string CustomerDocumentId { get; set; }
        public string Extension { get; set; }
        public bool IsDigitallySigned { get; set; }
        public string SignersList { get; set; }
        public string FileName { get; set; }
        public bool IsDeleted { get; set; }
        public FileInformation FileInfo { get; set; }
        public string DocumentId { get; set; }
        public bool IsRequested { get; set; }
        public int Tenant { get; set; }
        public string Notes { get; set; }
        public string ExternalCode { get; set; }

    }
}
