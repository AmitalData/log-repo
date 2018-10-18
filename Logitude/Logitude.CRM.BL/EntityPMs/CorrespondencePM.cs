using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityPMs
{
    public partial class CorrespondencePM
    {
        public bool IsFirst { get; set; }

        //private List<string> attachments;

        //[DataMember]
        //public virtual List<string> Attachments
        //{
        //    get
        //    {
        //        if (attachments == null)
        //        {
        //            attachments = new List<string>();
        //        }
        //        return attachments;
        //    }
        //    set { attachments = value; }
        //}

        private List<DocumentDataPM> correspondenceDocumentDatas;

        [DataMember]
        public virtual List<DocumentDataPM> CorrespondenceDocumentData
        {
            get
            {
                if (correspondenceDocumentDatas == null)
                {
                    correspondenceDocumentDatas = new List<DocumentDataPM>();
                }
                return correspondenceDocumentDatas;
            }
            set { correspondenceDocumentDatas = value; }
        }
    }
}
