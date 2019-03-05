
UPDATE QuoteTemplateTextCodes set   OriginalEnglishName = 'Charge Notes' , OriginalLocalName = 'Charge Notes' where  TextCode='CHARGENOTEPACKAGES' or TextCode = 'CHARGENOTECONTAINERS'
UPDATE QuoteTemplateTextCodes set EnglishName = 'Charge Notes'  where  (TextCode='CHARGENOTEPACKAGES' or TextCode = 'CHARGENOTECONTAINERS') and EnglishName = 'Charge Note'
UPDATE QuoteTemplateTextCodes set LocalName = 'Charge Notes'  where  (TextCode='CHARGENOTEPACKAGES' or TextCode = 'CHARGENOTECONTAINERS') and LocalName = 'Charge Note'
