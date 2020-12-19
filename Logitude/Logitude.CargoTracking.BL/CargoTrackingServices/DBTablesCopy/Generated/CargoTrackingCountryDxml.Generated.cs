
namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingCountryDxml  
   {    
        public string  GetCargoTrackingCountryDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingCountries' OldNames='Countries' Schema='dbo' DBType='CargoTracking' Module='CargoTracking'>"+
"  <Column Name='Id' Type='varchar' Size='15'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='LocalName' Type='nvarchar' Size='120'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='Code' Type='char' Size='2'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='EnglishName' Type='varchar' Size='120'>"+
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
	 