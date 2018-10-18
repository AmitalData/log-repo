update Questionnaires set SearchFields = Name
update QuoteTemplates set SearchFields = Name


-- Global DB
update LogitudeLeads set SearchFields =  isnull(CompanyName,'')  + ',' + isnull(Email,'') +','+  isnull(ContactName,'') + ',' + isnull(Comments,'')  + ',' +isnull(PhoneNumber,'')  + ',' +isnull(StatusCode,'')  + ',' + isnull(RequestType,'') + ',' + isnull(Country,'')


 