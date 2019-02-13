
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class AccountingIntegrityCheckDataMapping: IMapping<AccountingIntegrityCheckPM, AccountingIntegrityCheck>
   {

        public void CustomPMToPOCO(AccountingIntegrityCheckPM entityPM, AccountingIntegrityCheck entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ParametersXML);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            entityPOCO.Id = entityPM.Id;

            // create params obj
            var _paramsObj = new AccountingIntegrityInParam()
            {
                Tenant = entityPM.Tenant,
                FromMonthInclusive = entityPM.FromMonthInclusive,
                ToMonthInclusive = entityPM.ToMonthInclusive
            };

            // serialize
            string _xmlString = LogitudeXmlSerializer.SerializeObjectToXmlElementString<AccountingIntegrityInParam>(_paramsObj);

            //get status name
            if (entityPM.StatusCode != null)
            {
                IntegrityCheckStatusQueryService query = new IntegrityCheckStatusQueryService(entityPM.Tenant);

                var checkStatus = query.GetSingle(entityPM.StatusCode, false, false);
                if (checkStatus != null)
                {
                    entityPM.StatusName = checkStatus.Name;
                }
            }

            // set
            if (!string.IsNullOrWhiteSpace(_xmlString))
            {
                entityPOCO.ParametersXML = _xmlString;
            }

            BuildSearchFields(entityPM, entityPOCO);


        }

        public void CustomPOCOToPM(AccountingIntegrityCheckPM entityPM, AccountingIntegrityCheck entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.StatusName);
            CustomMappedPMProperties.Add(PMPropertyNames.FromMonthInclusive);
            CustomMappedPMProperties.Add(PMPropertyNames.ToMonthInclusive);
            CustomMappedPMProperties.Add(PMPropertyNames.SearchFields);

            if (entityPOCO.StatusCode != null)
            {
                IntegrityCheckStatusQueryService query = new IntegrityCheckStatusQueryService(entityPOCO.Tenant);

                var checkStatus = query.GetSingle(entityPOCO.StatusCode, false, false);
                if (checkStatus != null)
                {
                    entityPM.StatusName = checkStatus.Name;
                }
            }

            // parameters
            if (!string.IsNullOrWhiteSpace(entityPOCO.ParametersXML))
            {
                // deserialize
                AccountingIntegrityInParam _params = LogitudeXmlSerializer.DeserializeObject<AccountingIntegrityInParam>(entityPOCO.ParametersXML);

                // set
                if (_params != null)
                {
                    entityPM.FromMonthInclusive = _params.FromMonthInclusive;
                    entityPM.ToMonthInclusive = _params.ToMonthInclusive;
                }
            }

            BuildSearchFields(entityPM, entityPOCO);
        }

        private void BuildSearchFields(AccountingIntegrityCheckPM entityPM, AccountingIntegrityCheck poco)
        {
            string result = "";


            if (!string.IsNullOrEmpty(entityPM.StatusName))
            {
                result = entityPM.StatusName + ',' + entityPM.StatusCode;
            }

            entityPM.SearchFields = result;
            poco.SearchFields = result;

        }

    }


}
   