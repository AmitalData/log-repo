using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingShipmentComputedDxml  
   {    
      
        public string  GetCargoTrackingShipmentComputedDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingShipmentComputeds' Schema='dbo' DBType='CargoTracking'>"+
"  <Column Name='Id' Type='varchar' Size='15'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='FirstPickupATD' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='FinalDeliveryATA' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='FinalDeliveryETA' Type='datetime'>"+
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
	 