
namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingPortDxml  
   {    
        public string  GetCargoTrackingPortDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingPorts' Schema='dbo' DBType='CargoTracking'>"+
"  <Column Name='Id' Type='varchar' Size='15'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='Code' Type='varchar' Size='3'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='EnglishName' Type='varchar' Size='40'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='CountryId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='Tenant' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"</Table>";
		
		   return DxmlFile;
		}
   }

}
	 