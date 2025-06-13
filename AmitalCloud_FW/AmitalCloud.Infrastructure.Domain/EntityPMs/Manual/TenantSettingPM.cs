using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class TenantSettingPM : BaseEntityPM
    {
        public TenantSettingPM() : base() { }
        public TenantSettingPM(TenantSetting entityPOCO) : base()
        {
            Id = entityPOCO.Id;
            Tenant = entityPOCO.Tenant;
            SettingCode = entityPOCO.SettingCode;
            SettingValue = entityPOCO.SettingValue;
            ObjectTableId = entityPOCO.ObjectTableId;
            Size = entityPOCO.Size;
            Prefix = entityPOCO.Prefix;
            DontIncludeDirects = entityPOCO.DontIncludeDirects;
            ObjectTable = new ObjectTablePM(entityPOCO.ObjectTable);
            IsDocumentFilingByEmailEnabled = entityPOCO.IsDocumentFilingByEmailEnabled;
        }
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string SettingCode { get; set; }

        public string SettingValue { get; set; }

        public string ObjectTableId { get; set; }

        public int Size { get; set; }
        public string Prefix { get; set; }

        public bool DontIncludeDirects { get; set; }
        public ObjectTablePM ObjectTable { get; set; }
        public bool IsDocumentFilingByEmailEnabled { get; set; }
    }
}
