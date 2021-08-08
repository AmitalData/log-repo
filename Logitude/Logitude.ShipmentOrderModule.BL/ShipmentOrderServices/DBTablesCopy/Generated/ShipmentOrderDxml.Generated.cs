using System;
using System.Collections.Generic;
using System.Text;

namespace Logitude.ShipmentOrderModule.BL.ShipmentOrderServices.DBTablesCopy.Generated
{
   public partial class ShipmentOrderDxml  
   {    
      
        public string  GetShipmentOrderDxml (){
		    string dxmlFile = "<Table Name='ShipmentOrders' Schema='dbo' DBType='Main' Module='ShipmentOrder'>"+
"  <Column Name='Id' Type='varchar' Size='15'>"+
"    <Constraints PrimaryKey='true' Nullable='false' />"+
"  </Column>"+
"  <Column Name='Tenant' Type='int'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='CreateDate' Type='datetime'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='CreatedByUserId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='UpdateDate' Type='datetime'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='UpdatedByUserId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='SearchFields' Type='nvarchar' Size='-1'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='OrderNumber' Type='varchar' Size='20'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='TransportModeId' Type='char' Size='1'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='ConsigneeId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='ShipperId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='AgentId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='IncotermId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='AccountManagerId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='PONumber' Type='varchar' Size='50'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='DescriptionOfGoods' Type='nvarchar' Size='2000'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='ShipmentTypeId' Type='varchar' Size='4'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='Master' Type='varchar' Size='20'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='House' Type='varchar' Size='20'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='VesselId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='CustomsAgentId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='SpecialServicesTypeId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='CustomerReferences' OldNames='CustomerRefrences' Type='varchar' Size='300'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='IsReadyForPickup' Type='bit'>"+
"    <Constraints Nullable='false' />"+
"  </Column>"+
"  <Column Name='PickupEstimatedDateTime' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='PickupActualDateTime' Type='datetime'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='ForwarderId' Type='varchar' Size='15'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Column Name='BookingConfirmationDate' Type='date'>"+
"    <Constraints Nullable='true' />"+
"  </Column>"+
"  <Relation ForeignKeyColumn='CreatedByUserId' ReferencedTable='Users' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='UpdatedByUserId' ReferencedTable='Users' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='TransportModeId' ReferencedTable='TransportModes' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='ConsigneeId' ReferencedTable='Cards' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='ShipperId' ReferencedTable='Cards' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='AgentId' ReferencedTable='Cards' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='IncotermId' ReferencedTable='Incoterms' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='AccountManagerId' ReferencedTable='Users' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='ShipmentTypeId' ReferencedTable='ShipmentTypes' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='VesselId' ReferencedTable='Vessels' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='CustomsAgentId' ReferencedTable='Cards' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='SpecialServicesTypeId' ReferencedTable='SpecialServicesTypes' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Relation ForeignKeyColumn='ForwarderId' ReferencedTable='Cards' ReferencedColumn='Id' ReferencedTableSchema='dbo' />"+
"  <Index Columns='Tenant,OrderNumber' />"+
"</Table>";
		
		   return dxmlFile;
		}
		 
   }

}
	 