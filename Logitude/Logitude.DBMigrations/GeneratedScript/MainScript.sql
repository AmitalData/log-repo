-- General Script From 202007201100_ReferantSearchfield.sxml File
DECLARE
StartTime TIMESTAMP;
EndTime TIMESTAMP;
BEGIN
SAVEPOINT ScriptSavePoint;
StartTime := SYSTIMESTAMP;
BEGIN
update declarationreferantdatas set
declarationreferantdatas.SEARCHFIELDS=
(select   to_char(d.DeclarationNumber)||',' ||to_char(d.CustomFileNo) ||',' || to_char(d.CustomerId) ||',' ||
to_char(c.ManifestNumber )  ||',' || c.SecondCargoID  ||',' || c.ThirdCargoID as SEARCHFIELDS
from  declarationreferantdatas r
inner join
declarations d on  r.DECLARATIONID= d.id
inner join  CONSIGNMENTS c
on d.id= c.DECLARATIONID  and c.CONSIGNMENTNUMBER=1
where  d.id= declarationreferantdatas.DECLARATIONID);
END;
EndTime:= SYSTIMESTAMP;
BEGIN
DECLARE ScriptBody NCLOB;
BEGIN
ScriptBody := 'update declarationreferantdatas set
declarationreferantdatas.SEARCHFIELDS=
(select   to_char(d.DeclarationNumber)||'','' ||to_char(d.CustomFileNo) ||'','' || to_char(d.CustomerId) ||'','' ||
to_char(c.ManifestNumber )  ||'','' || c.SecondCargoID  ||'','' || c.ThirdCargoID as SEARCHFIELDS
from  declarationreferantdatas r
inner join
declarations d on  r.DECLARATIONID= d.id
inner join  CONSIGNMENTS c
on d.id= c.DECLARATIONID  and c.CONSIGNMENTNUMBER=1
where  d.id= declarationreferantdatas.DECLARATIONID);';
INSERT INTO "DBSCRIPTSHISTORY"("SXMLFILENAME", "EXECUTIONDATE", "SCRIPTBODY", "ELAPSEDTIMEINMS", "HASHVALUE", "VERSION")VALUES('202007201100_ReferantSearchfield.sxml', SYSDATE, ScriptBody, EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), '24d87dde50c2bf9294d31e637e485ad6', 1);
END;
END;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK TO ScriptSavePoint;
COMMIT;
END;

