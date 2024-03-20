using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CertificateOfOriginMandatoryFieldsQueryService : ICanGetAllClosedTable<CertificateOfOriginMandatoryFieldsPM>
    {
        public List<CertificateOfOriginMandatoryFieldsPM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public List<CertificateOfOriginMandatoryFieldsPM> GetMandatoryFieldsByCooTypeCode(string cooTypeCode)
        {
            CertificateOfOriginMandatoryFieldsRepository rep = new CertificateOfOriginMandatoryFieldsRepository(context);
            List<CertificateOfOriginMandatoryFields> mandatoryFieldslist = rep.GetMandatoryFieldsByCooTypeCode(cooTypeCode);
            List<CertificateOfOriginMandatoryFieldsPM> mandatoryFieldslistPM = new List<CertificateOfOriginMandatoryFieldsPM>();

            if (mandatoryFieldslist != null)
            {
                foreach (var item in mandatoryFieldslist)
                {
                    if (item.IsMandatory)
                    {
                        var mandatoryFieldPM = this.GetEntityPM(item, false, new CertificateOfOriginMandatoryFieldsKeys { Code = item.Code.ToString() });
                        mandatoryFieldslistPM.Add(mandatoryFieldPM);
                    }

                }
            }
            return mandatoryFieldslistPM;
        }
    }
}
