
update DWObjectFields set CannotFilter=1 where name in ('Name','Local Name','English Name')
and DWObjectTableCode in('DIM_Branches','DIM_Incoterms','DIM_ShipmentStatuses','DIM_SpecialServicesTypes','DIM_MoveTypes','DIM_Vessels','DIM_Partners','DIM_Currencies','DIM_Ports')

update DWObjectFields set LOVAdditionalColumns= LOVAdditionalColumns+',[Local Name]' where name='Code' and LOVAdditionalColumns not like ('%Local Name%') and DWObjectTableCode in (select DWObjectTableCode from DWObjectFields where name='Local Name')
