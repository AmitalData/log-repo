using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization; 

namespace Logitude.DashboardModule.Data.EntityLists
{
   [DataContract]
   public partial class AnalyticsFactsFieldsMetaDataList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string AnalyticsFactsMetaDataId  { get; set; }
       [DataMember]
       public string DataTypeCode  { get; set; }
       [DataMember]
       public bool CanMeasure  { get; set; }
       [DataMember]
       public bool CanGroup  { get; set; }
       [DataMember]
       public string FieldCode  { get; set; }
       [DataMember]
       public string DisplayName  { get; set; }
       [DataMember]
       public string DisplayNamePlural  { get; set; }
       [DataMember]
       public string JoinedTableName  { get; set; }
       [DataMember]
       public string JoinedTableKey  { get; set; }
       [DataMember]
       public string JoinedTableDisplayField  { get; set; }
       [DataMember]
       public string SearchFields  { get; set; }
       [DataMember]
       public string JoinedTableDBName  { get; set; }
       [DataMember]
       public bool HasUnit  { get; set; }
       [DataMember]
       public string Unit  { get; set; }
       [DataMember]
       public string CommonFilterCode  { get; set; }
       [DataMember]
       public bool CanSecondaryGroup  { get; set; }
       [DataMember]
       public bool AllowTenantZeroFilter  { get; set; }
   }

}
	 