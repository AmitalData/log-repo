
namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingTransportModeDxml  
   {    
        public string  GetCargoTrackingTransportModeDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingTransportModes' Schema='dbo' DBType='CargoTracking'>"+
"  <Column Name='Id' Type='char' Size='1'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='SearchFields' Type='nvarchar' Size='1000'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='Name' Type='varchar' Size='10'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"</Table>";
		
		   return DxmlFile;
		}
   }

}
	 