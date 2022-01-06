using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoReferencesSyncQueueDxml  
   {    
      
        public string  GetCargoReferencesSyncQueueDxml (){
		    string dxmlFile = "<Table Name='CargoReferencesSyncQueues' Schema='dbo' DBType='CargoTracking'>"+
"  <Column Name='Id' Type='varchar' Size='15'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='ForwardingShipmentId' OldNames='ShipmentId' Type='varchar' Size='30'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='ShipmentNeedUpdateType' OldNames='ShipmentType' Type='varchar' Size='1'>"+
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
	 