


--Change name of Step to Break 
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code like '%IsFreightBySteps%'
update TextCodes set IsSpellChecked = 0, SpellCheckDate = null, SpellCheckedByUserId = null where Code = 'QuotePriceSteps.F.Step'

update TextCodes set DefaultText = 'Price by break' where Code = 'Quote.F.IsFreightBySteps'
update TextCodes set DefaultText = 'Price by break' where Code = 'Quote.F.IsFreightBySteps.Short'
update TextCodes set DefaultText = 'Price by break' where Code = 'Quote.B.Charges.ByPriceBreak'
update TextCodes set DefaultText = 'Break from' where Code = 'QuotePriceSteps.F.Step'
update TextCodes set DefaultText = 'Cost break price' where Code = 'QuotePriceSteps.F.CostStepPrices.Short'
update TextCodes set DefaultText = 'Sale break price' where Code = 'QuotePriceSteps.F.SaleStepPrices.Short'

update TextCodes set DefaultText = 'Add Break' where Code = 'Quote.B.Charges.AddStep'
update TextCodes set DefaultText = 'Edit Break' where Code = 'Quote.B.Charges.EditStep'
update TextCodes set DefaultText = 'Delete Break' where Code = 'Quote.B.Charges.DeleteStep'

