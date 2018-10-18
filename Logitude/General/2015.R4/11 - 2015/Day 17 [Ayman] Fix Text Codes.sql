
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null
where Code in
(
'ARPayment.M.CantSetZeroAmount',
'ARPayment.M.CantSetMinusAmount',
'ARPayment.M.CantSetFutureDatePayment',
'ARPayment.M.PaymentAmountPaidCantBeMinus',
'ARPayment.M.PaymentAmountPaidCantBeBigger',
'ARPayment.M.AccountingSettingsDontAllowVoid',
'ARPayment.M.AmountPaidNotLess',
'ARPayment.M.CantPayMinusValue',
'APPayment.M.CantSetZeroAmount',
'APPayment.M.CantSetMinusAmount',
'APPayment.M.CantSetFutureDatePayment',
'APPayment.M.PaymentAmountPaidCantBeMinus',
'APPayment.M.PaymentAmountPaidCantBeBigger',
'APPayment.M.AccountingSettingsDontAllowVoid',
'APPayment.M.AmountPaidNotLess',
'APPayment.M.CantPayMinusValue'
)

delete from TextCodes where Code in
(
'APPayment.M.PaymentInvoicesHasErrors',
'ARPayment.M.PaymentInvoicesHasErrors'
)