using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoDisconnectQueueDxml  
   {    
      
        public string  GetCargoDisconnectQueueDxml (){
		    string dxmlFile = "<Table Name='CargoDisconnectQueues' Schema='dbo' DBType='CargoTracking' Module='CargoTracking'>"+
"  <Column Name='Id' Type='int' Identity='true'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='Tenant' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='ShipmentId' Type='varchar' Size='30'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='ShipmentType' Type='varchar' Size='1'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"</Table>";
		
		   return dxmlFile;
		}
		 
   }

}
	 