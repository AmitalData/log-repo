using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingWatermarkDxml  
   {    
      
        public string  GetCargoTrackingWatermarkDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingWatermarks' Schema='dbo' DBType='CargoTracking' Module='CargoTracking'>"+
"  <Column Name='TableName' Type='varchar' Size='100'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='LastUpdateDate' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='LastRun' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"</Table>";
		
		   return DxmlFile;
		}
		 
   }

}
	 