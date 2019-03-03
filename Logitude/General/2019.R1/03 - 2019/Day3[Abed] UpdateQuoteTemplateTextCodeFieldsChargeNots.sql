
UPDATE QuoteTemplateTextCodes set   OriginalEnglishName = 'Charge Notes' , OriginalLocalName = 'Charge Notes' where  TextCode='CHARGENOTEPACKAGES' or TextCode = 'CHARGENOTEPACKAGES'
UPDATE QuoteTemplateTextCodes set EnglishName = 'Charge Notes'  where  (TextCode='CHARGENOTEPACKAGES' or TextCode = 'CHARGENOTEPACKAGES') and EnglishName = 'Charge Note'
UPDATE QuoteTemplateTextCodes set LocalName = 'Charge Notes'  where  (TextCode='CHARGENOTEPACKAGES' or TextCode = 'CHARGENOTEPACKAGES') and LocalName = 'Charge Note'
