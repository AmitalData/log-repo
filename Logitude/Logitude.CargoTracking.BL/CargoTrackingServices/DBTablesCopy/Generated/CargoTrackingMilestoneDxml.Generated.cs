
namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingMilestoneDxml  
   {    
        public string  GetCargoTrackingMilestoneDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingMilestones' Schema='dbo' DBType='CargoTracking' Module='CargoTracking'>"+
"  <Column Name='Code' Type='varchar' Size='2'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='EnglishName' Type='varchar' Size='30'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='SearchFields' Type='nvarchar' Size='-1'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='LocalName' Type='nvarchar' Size='30'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='Inactive' Type='bit'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"</Table>";
		
		   return DxmlFile;
		}
   }

}
	 