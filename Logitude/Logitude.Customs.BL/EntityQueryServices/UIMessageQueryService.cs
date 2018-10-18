using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class UIMessageQueryService : EntityQueryService<UIMessage, UIMessageKeys, UIMessagePM, object, UIMessageKeys>
        , ICanGetAllClosedTable<UIMessagePM>,
        ICanGetAllWithAdditionalClosedTable<UIMessagePM>
    {

        public UIMessagePM GetUIMessageWithAdditional(string code, int tenant)
        {
            UIMessagePM uIMessagePM = null;
            if (!string.IsNullOrWhiteSpace(code))
            {
                if (code.Equals("Buisness", StringComparison.OrdinalIgnoreCase))
                {
                    return null; 
                }
                UIMessage UIMessage = repository.GetSingle(new UIMessageKeys() { Code = code });
                UIMessageAdditionalRepository additionalRepository = new UIMessageAdditionalRepository(context);
                UIMessageAdditional additional = additionalRepository.GetSingleAdditionalByCode(code, tenant);
                if (UIMessage == null)
                {
                    //throw new Exception("UIMessage code does not exist in DB !!! code =" + code);
                    return null;
                }
                uIMessagePM = new UIMessagePM()
                {
                    Code = UIMessage.Code,
                    EnglishName = UIMessage.EnglishName,
                    LocalName = UIMessage.LocalName,
                    Tenant = tenant
                };
                if (additional != null)
                {
                    uIMessagePM.Sort = additional.Sort;
                }
            }
            return uIMessagePM;
        }


        public List<UIMessagePM> GetAllWithAdditional(int tenant)
        {
            var additionalRepository = new UIMessageAdditionalRepository(context);
            var q =
                (from h in repository.GetAll()
                 join ha in additionalRepository.GetAll(tenant)
                 on h.Code equals ha.Code into haJ
                 from ha in haJ.DefaultIfEmpty()
                 select new UIMessagePM()
                 {
                     Code = h.Code,
                     EnglishName = h.EnglishName,
                     LocalName = h.LocalName,

                     ///Calc
                     Tenant = tenant,
                     Sort = ha == null ? null : ha.Sort,
                 }

                );
            var l = q.ToList();
            return l;
        }

        public List<UIMessagePM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }
    }
}
