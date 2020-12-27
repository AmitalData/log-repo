
namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingIncrementalStatDxml  
   {    
        public string  GetCargoTrackingIncrementalStatDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingIncrementalStats' Schema='dbo' DBType='CargoTracking' Module='CargoTracking'>"+
"  <Column Name='StartDate' Type='datetime'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='EndDate' Type='datetime'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='Shipments' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='Cards' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='Ports' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='Countries' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='TransportModes' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='ShipmentComputedFields' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='ShipmentMasterDatas' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='Id' Type='int' Identity='true'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='ErrorLog' Type='nvarchar' Size='4000'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"</Table>";
		
		   return DxmlFile;
		}
   }

}
	 