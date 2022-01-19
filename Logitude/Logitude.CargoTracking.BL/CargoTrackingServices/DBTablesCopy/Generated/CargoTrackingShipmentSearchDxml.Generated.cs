using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.CargoTracking.BL.CargoTrackingServices.DBTablesCopy.Generated
{
   public partial class CargoTrackingShipmentSearchDxml  
   {    
      
        public string  GetCargoTrackingShipmentSearchDxml (){
		    string dxmlFile = "<Table Name='CargoTrackingShipmentSearches' Schema='dbo' DBType='CargoTracking' Module='CargoTracking'>"+
"  <Column Name='Tenant' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='SearchFields' Type='nvarchar' Size='1000'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='ShipmentDate' Type='datetime'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='Id' Type='int' Identity='true'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='ShipmentId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='IsPublic' Type='bit'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='ReferenceType' Type='varchar' Size='200'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Index Columns='Tenant,SearchFields,IsPublic' />"+
"  <Index Columns='Tenant,ShipmentId,IsPublic' />"+
"  <Index Columns='ShipmentId' />"+
"  <Index Columns='ShipmentDate' />"+
"  <Index Columns='Tenant' />"+
"  <Index Columns='SearchFields' />"+
"</Table>";
		
		   return dxmlFile;
		}
		 
   }

}
	 