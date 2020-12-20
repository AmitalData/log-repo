
namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingCardDxml  
   {    
        public string  GetCargoTrackingCardDxml (){
		    string DxmlFile = "<Table Name='CargoTrackingCards' Schema='dbo' DBType='CargoTracking'>"+
"  <Column Name='Id' Type='varchar' Size='15'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='Code' Type='varchar' Size='15'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='EnglishName' Type='varchar' Size='70'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='LocalName' Type='nvarchar' Size='100'>"+
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
	 