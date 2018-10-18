using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GDMFILINGPM : EntityPM
    {
        public string COMID { get; set; }

        public string FOLDERCODE { get; set; }
 
        public string FILENO { get; set; }

        public int? LASTVERSION { get; set; }

        public string EXTENSION { get; set; }

        public string ORIGNALFILE { get; set; }

        public string DOCID { get; set; }

        public string DESC { get; set; }
 
        public string SEARCHDESC { get; set; }
 
        public string REMARKS { get; set; }

        public string SEARCHREMARKS { get; set; }

        public DateTime? OPENDATE { get; set; }

        public string BRANCHID { get; set; }

        public string DEPARTID { get; set; }

        public string ISCLOSE { get; set; }

        public DateTime? FOLUPDATE { get; set; }

        public int? PRIORITY { get; set; }
 
        public string STATUSID { get; set; }

        public string LSTSTATUSID { get; set; }

        public DateTime? LSTSTATUSDATE { get; set; }

        public string UPDATEBY { get; set; }

        public string AUTOYN { get; set; }

        public int? DELETEAUTO { get; set; }

        public string SOURCE { get; set; }

        public string ORIGIN { get; set; }

        public string OWNERCODE { get; set; }
 
        public string USRCODE { get; set; }

        public DateTime? LASTOPENDATE { get; set; }
 
        public DateTime? LASTEDITDATE { get; set; }
  
        public string VENDORID { get; set; }
  
        public string OLDCARD { get; set; }
 
        public short? FUCLOSED { get; set; }
     
        public string CONID { get; set; }
       
        public string FROM { get; set; }
       
        public string TO { get; set; }
   
        public DateTime? FILINGDATE { get; set; }
       
        public string NOTVALID { get; set; }
       
        public string COMPANYID { get; set; }
        
        public string CARDIDLIST { get; set; }
       
        public string CUSTOMDOCID { get; set; }
       
        public string SPLITTEDSTATUS { get; set; }
        
        public string PARENTCOMID { get; set; }
        
        public string MD5HASH { get; set; }
        
        public string SPLITRESULT { get; set; }
        
        public string SERVERVER { get; set; }
        
        public string DELETED { get; set; }
       
        public string HASSIGN { get; set; }
       
        public string SIGNMETADATA { get; set; }
       
        public string OCR { get; set; }
        
        public string CONVERT2TIFF { get; set; }
        
        public string ISSHAREDWITHIMPORTER { get; set; }
        
        public string ISORIGINAL { get; set; }

        public int? PAGECOUNT { get; set; }

        public List<GDMFILEVERPM> GDMFILEVERs { get; set; }

        public string ISDECLARATIONRELATED { get; set; }

    }
}
