
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'DeleteDraft')
delete from Features where Code = 'DeleteDraft'
delete from MenuButtons where EventCode = 'DeleteDraft'
delete from TextCodes where Code = 'ARInvoice.B.DeleteDraft'
delete from TextCodes where Code = 'ARInvoice.Features.DeleteDraft'

