

delete from ScreenFields where Id in
(
select ScreenFields.Id from ScreenFields join ObjectFields on ScreenFields.ObjectFieldId = ObjectFields.Id
where
ScreenFields.ScreenId = (select Id from Screens where Code = 'EventType.GeneralTabScreen')
and ObjectFields.FieldName in ('ManualActivatedFollowUp', 'IsManualEntry')
)
