using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingHeaderEntityTypeDxml  
   {    
      
        public string  GetCargoTrackingHeaderEntityTypeDxml (){
		    string dxmlFile = "<Table Name='CargoTrackingHeaderEntityTypes' Schema='dbo' DBType='CargoTracking' Module='CargoTracking'>"+
"  <Column Name='Code' Type='varchar' Size='1'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='EnglishName' Type='varchar' Size='30'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='SearchFields' Type='nvarchar' Size='-1'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='LocalName' Type='nvarchar' Size='30'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='Inactive' Type='bit'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"</Table>";
		
		   return dxmlFile;
		}
		 
   }

}
	 