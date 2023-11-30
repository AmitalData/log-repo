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
   public partial class WidgetList
   {
   
       [Key]
       [DataMember]
       public string Id  { get; set; }
       [DataMember]
       public int Tenant  { get; set; }
       [DataMember]
       public string Title  { get; set; }
       [DataMember]
       public string Filters  { get; set; }
       [DataMember]
       public bool TimeOverTime  { get; set; }
       [DataMember]
       public int? ComparisonPeriod  { get; set; }
       [DataMember]
       public string Increase  { get; set; }
       [DataMember]
       public string ComparisonOperator  { get; set; }
       [DataMember]
       public string ComparisonDateGroup  { get; set; }
       [DataMember]
       public DateTime? FromDate  { get; set; }
       [DataMember]
       public DateTime? ToDate  { get; set; }
       [DataMember]
       public string Alignment  { get; set; }
       [DataMember]
       public bool? ThousandSeparator  { get; set; }
       [DataMember]
       public bool? UseNumberAbbreviation  { get; set; }
       [DataMember]
       public int? DecimalPlaces  { get; set; }
       [DataMember]
       public string UseAbbreviationAfter  { get; set; }
       [DataMember]
       public string LabelsPosition  { get; set; }
   }

}
	 