using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoReferencesSyncQueueDxml  
   {    
      
        public string  GetCargoReferencesSyncQueueDxml (){
		    string dxmlFile = "<Table Name='CargoReferencesSyncQueues' Schema='dbo' DBType='CargoTracking' Module='CargoTracking'>"+
"  <Column Name='Id' Type='int'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='ShipmentId' OldNames='ForwardingShipmentId' Type='varchar' Size='30'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='ShipmentType' OldNames='ShipmentNeedUpdateType' Type='varchar' Size='1'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='Tenant' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"</Table>";
		
		   return dxmlFile;
		}
		 
   }

}
	 