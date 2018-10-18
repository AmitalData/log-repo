-- later
-- After update
-- excute on logitude2-4_Main

update Cards set SharedLogisticsInvitationStatusCode = 1
where PartnerTypeId = 'CS' OR PartnerTypeId = 'AG'