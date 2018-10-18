

update TextCodes
set IsSpellChecked = 0,
SpellCheckDate = null,
SpellCheckedByUserId = null

where 
TextCodeTypeCode = 'T'
and Code in
(
'VatType',
'CustomAgent'
)