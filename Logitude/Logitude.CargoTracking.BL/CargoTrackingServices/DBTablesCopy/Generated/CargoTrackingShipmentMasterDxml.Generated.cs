using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingShipmentMasterDxml  
   {    
      
        public string  GetCargoTrackingShipmentMasterDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingShipmentMasters' Schema='dbo' DBType='CargoTracking'>"+
"  <Column Name='Id' Type='varchar' Size='15'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='Master' Type='varchar' Size='20'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='MainCarriageATD' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='MainCarriageETD' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='MainCarriageATA' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='MainCarriageETA' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='Tenant' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"</Table>";
		
		   return DxmlFile;
		}
		 
   }

}
	 