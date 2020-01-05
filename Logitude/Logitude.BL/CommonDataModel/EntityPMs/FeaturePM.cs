using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class FeaturePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string ObjectTableId { get; set; }
        public string NameTextCodeId { get; set; }
        public string FeatureTypeCode { get; set; }
        public bool Packagable { get; set; }
        public bool IsBusinessUnitEnabled { get; set; }
        public bool IsOld { get; set; }
        public bool IsCoreFeature { get; set; }
        public string ToggleCode { get; set; }
        public string NameTextCodeCode { get; set; }

        // Dummy
        public string ObjectTableName { get; set; }
        public string RoleId { get; set; }
        public string ParentRoleId { get; set; }        
        public int RoleTenant { get; set; }
        public bool IsCustomRole { get; set; }
        public bool IsCustomRoleFeature { get; set; }
        public string PackageCode { get; set; }
        public string AccessLevelCode { get; set; }
        public string TranslatedName { get; set; }

        public bool Exists { get; set; }
        public bool IsAdded { get; set; }
        public bool IsRemoved { get; set; }                        
    }
}