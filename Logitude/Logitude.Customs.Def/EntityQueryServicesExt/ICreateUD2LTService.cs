
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.EntityQueryServicesExt
{
    public interface ICreateUD2LTService
    {
        void JustDoIt(object documentsFilingPM);
        //void JustDoIt(string DocumentsFilingId,int tenant);
        //void MustInit(DocumentsFiling pocoDocumentsFiling);
    }
}
