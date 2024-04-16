
using Logitude.Customs.Def.EntityPMs;
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
    public interface ISendBondedCustomDocumentService
    {
        void JustDoIt(object documentsFilingPM);

    }
    public interface IDICustomsSettingQueryService
    {
        bool IsCourierTenant(int tenant);
    }

    public interface IDIUnifreightTaskService
    {
        void OpenUnifreighTaskGen(DeclarationPM dirtyDeclarationPM, string entname, string primary, string taskType, string status, bool raiseStatus, string xmlStatus, bool toLock);
    }
}
               