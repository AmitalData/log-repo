update DocumentsFilings
set SecurityId = LTRIM(Id + str(RAND() * POWER(CAST(10 as BIGINT), 10)))
where SecurityId = null;