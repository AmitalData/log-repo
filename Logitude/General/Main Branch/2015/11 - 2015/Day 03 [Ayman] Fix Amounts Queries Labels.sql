

delete from QueryColumns where ObjectFieldId in (select Id from ObjectFields where FieldName = 'PayablesInLocalCurrency')
go

delete from ObjectFields where FieldName = 'PayablesInLocalCurrency'
go

update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%ProfitInLocalCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%ProfitInProfitCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%OpenReceivablesInLocalCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%OpenReceivablesInProfitCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%AccountedReceivablesInLocalCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%AccountedReceivablesInProfitCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%OpenPayablesInLocalCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%OpenPayablesInProfitCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%AccountedPayablesInLocalCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%AccountedPayablesInProfitCurrency%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like 'Shipment.CH%Profit%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like 'Shipment.CH%Local%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like 'Master.CH%Profit%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like 'Master.CH%Local%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%Invoice%SubTotal%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%Invoice%AmountDue%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%Invoice%AmountIn%'



