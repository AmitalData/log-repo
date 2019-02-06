//OpenAccess please define!!! 
//logitude please undefine!!!  
#define reserveword


using System;
using System.Collections.Generic;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class GDMFILINGDataMapping : IMapping<GDMFILINGPM, GDMFILING>
    {
        public void PMToPOCO(GDMFILINGPM entityPM, GDMFILING entityPOCO)
        {
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.FOLDERCODE = entityPM.FOLDERCODE;
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.LASTVERSION = entityPM.LASTVERSION;
            entityPOCO.EXTENSION = entityPM.EXTENSION;
            entityPOCO.ORIGNALFILE = entityPM.ORIGNALFILE;
            entityPOCO.DOCID = entityPM.DOCID;
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            entityPOCO.DESC = entityPM.DESC;
#endif
            entityPOCO.SEARCHDESC = entityPM.SEARCHDESC;
            entityPOCO.REMARKS = entityPM.REMARKS;
            entityPOCO.SEARCHREMARKS = entityPM.SEARCHREMARKS;
            entityPOCO.OPENDATE = entityPM.OPENDATE;
            entityPOCO.BRANCHID = entityPM.BRANCHID;
            entityPOCO.DEPARTID = entityPM.DEPARTID;
            entityPOCO.ISCLOSE = entityPM.ISCLOSE;
            entityPOCO.FOLUPDATE = entityPM.FOLUPDATE;
            entityPOCO.PRIORITY = entityPM.PRIORITY;
            entityPOCO.STATUSID = entityPM.STATUSID;
            entityPOCO.LSTSTATUSID = entityPM.LSTSTATUSID;
            entityPOCO.LSTSTATUSDATE = entityPM.LSTSTATUSDATE;
            entityPOCO.UPDATEBY = entityPM.UPDATEBY;
            entityPOCO.AUTOYN = entityPM.AUTOYN;
            entityPOCO.DELETEAUTO = entityPM.DELETEAUTO;
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            entityPOCO.SOURCE = entityPM.SOURCE;
#endif
            entityPOCO.ORIGIN = entityPM.ORIGIN;
            entityPOCO.OWNERCODE = entityPM.OWNERCODE;
            entityPOCO.USRCODE = entityPM.USRCODE;
            entityPOCO.LASTOPENDATE = entityPM.LASTOPENDATE;
            entityPOCO.LASTEDITDATE = entityPM.LASTEDITDATE;
            entityPOCO.VENDORID = entityPM.VENDORID;
            entityPOCO.OLDCARD = entityPM.OLDCARD;
            entityPOCO.FUCLOSED = entityPM.FUCLOSED;
            entityPOCO.CONID = entityPM.CONID;
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            entityPOCO.FROM = entityPM.FROM;
            entityPOCO.TO = entityPM.TO;
#endif
            entityPOCO.FILINGDATE = entityPM.FILINGDATE;
            entityPOCO.NOTVALID = entityPM.NOTVALID;
            entityPOCO.COMPANYID = entityPM.COMPANYID;
            entityPOCO.CARDIDLIST = entityPM.CARDIDLIST;
            entityPOCO.CUSTOMDOCID = entityPM.CUSTOMDOCID;
            entityPOCO.SPLITTEDSTATUS = entityPM.SPLITTEDSTATUS;
            entityPOCO.PARENTCOMID = entityPM.PARENTCOMID;
            entityPOCO.MD5HASH = entityPM.MD5HASH;
            entityPOCO.SPLITRESULT = entityPM.SPLITRESULT;
            entityPOCO.SERVERVER = entityPM.SERVERVER;
            entityPOCO.DELETED = entityPM.DELETED;
            entityPOCO.HASSIGN = entityPM.HASSIGN;
            entityPOCO.SIGNMETADATA = entityPM.SIGNMETADATA;
            entityPOCO.OCR = entityPM.OCR;
            entityPOCO.CONVERT2TIFF = entityPM.CONVERT2TIFF;
            entityPOCO.ISSHAREDWITHIMPORTER = entityPM.ISSHAREDWITHIMPORTER;
            entityPOCO.ISORIGINAL = entityPM.ISORIGINAL;
            entityPOCO.PAGECOUNT = entityPM.PAGECOUNT;
            entityPOCO.ISDECLARATIONRELATED = entityPM.ISDECLARATIONRELATED;
        }

        public void POCOToPM(GDMFILINGPM entityPM, GDMFILING entityPOCO)
        {
            entityPM.COMID = entityPOCO.COMID;
            entityPM.FOLDERCODE = entityPOCO.FOLDERCODE;
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.LASTVERSION = entityPOCO.LASTVERSION;
            entityPM.EXTENSION = entityPOCO.EXTENSION;
            entityPM.ORIGNALFILE = entityPOCO.ORIGNALFILE;
            entityPM.DOCID = entityPOCO.DOCID;
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            entityPM.DESC = entityPOCO.DESC;
#endif
            entityPM.SEARCHDESC = entityPOCO.SEARCHDESC;
            entityPM.REMARKS = entityPOCO.REMARKS;
            entityPM.SEARCHREMARKS = entityPOCO.SEARCHREMARKS;
            entityPM.OPENDATE = entityPOCO.OPENDATE;
            entityPM.BRANCHID = entityPOCO.BRANCHID;
            entityPM.DEPARTID = entityPOCO.DEPARTID;
            entityPM.ISCLOSE = entityPOCO.ISCLOSE;
            entityPM.FOLUPDATE = entityPOCO.FOLUPDATE;
            entityPM.PRIORITY = entityPOCO.PRIORITY;
            entityPM.STATUSID = entityPOCO.STATUSID;
            entityPM.LSTSTATUSID = entityPOCO.LSTSTATUSID;
            entityPM.LSTSTATUSDATE = entityPOCO.LSTSTATUSDATE;
            entityPM.UPDATEBY = entityPOCO.UPDATEBY;
            entityPM.AUTOYN = entityPOCO.AUTOYN;
            entityPM.DELETEAUTO = entityPOCO.DELETEAUTO;
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            entityPM.SOURCE = entityPOCO.SOURCE;
#endif
            entityPM.ORIGIN = entityPOCO.ORIGIN;
            entityPM.OWNERCODE = entityPOCO.OWNERCODE;
            entityPM.USRCODE = entityPOCO.USRCODE;
            entityPM.LASTOPENDATE = entityPOCO.LASTOPENDATE;
            entityPM.LASTEDITDATE = entityPOCO.LASTEDITDATE;
            entityPM.VENDORID = entityPOCO.VENDORID;
            entityPM.OLDCARD = entityPOCO.OLDCARD;
            entityPM.FUCLOSED = entityPOCO.FUCLOSED;
            entityPM.CONID = entityPOCO.CONID;
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            entityPM.FROM = entityPOCO.FROM;
            entityPM.TO = entityPOCO.TO;
#endif
            entityPM.FILINGDATE = entityPOCO.FILINGDATE;
            entityPM.NOTVALID = entityPOCO.NOTVALID;
            entityPM.COMPANYID = entityPOCO.COMPANYID;
            entityPM.CARDIDLIST = entityPOCO.CARDIDLIST;
            entityPM.CUSTOMDOCID = entityPOCO.CUSTOMDOCID;
            entityPM.SPLITTEDSTATUS = entityPOCO.SPLITTEDSTATUS;
            entityPM.PARENTCOMID = entityPOCO.PARENTCOMID;
            entityPM.MD5HASH = entityPOCO.MD5HASH;
            entityPM.SPLITRESULT = entityPOCO.SPLITRESULT;
            entityPM.SERVERVER = entityPOCO.SERVERVER;
            entityPM.DELETED = entityPOCO.DELETED;
            entityPM.HASSIGN = entityPOCO.HASSIGN;
            entityPM.SIGNMETADATA = entityPOCO.SIGNMETADATA;
            entityPM.OCR = entityPOCO.OCR;
            entityPM.CONVERT2TIFF = entityPOCO.CONVERT2TIFF;
            entityPM.ISSHAREDWITHIMPORTER = entityPOCO.ISSHAREDWITHIMPORTER;
            entityPM.ISORIGINAL = entityPOCO.ISORIGINAL;
            entityPM.PAGECOUNT = entityPOCO.PAGECOUNT;
            entityPM.ISDECLARATIONRELATED = entityPOCO.ISDECLARATIONRELATED;
        }

        public void CustomPMToPOCO(GDMFILINGPM entityPM, GDMFILING entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GDMFILINGPM entityPM, GDMFILING entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GDMFILINGPM entityPM, GDMFILINGPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
