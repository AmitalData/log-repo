-- General Script From 202007031145_DeclarationCancellationAddOn.sxml File
DECLARE
StartTime TIMESTAMP;
EndTime TIMESTAMP;
BEGIN
SAVEPOINT ScriptSavePoint;
StartTime := SYSTIMESTAMP;
BEGIN
declare
n int := 0;
begin
select count(*) into n
from PACKAGES
where code='DECA';
if n = 0 then
Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('DECA','Declaration Cancellation','DECA,Declaration Cancellation',0,'BS');
Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values ('DECAA','Declaration Cancellation AddOn','DECAA,Declaration Cancellation AddOn',0,'AD');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('G-23','DECA','DECAA');commit;
end if;
end;
END;
EndTime:= SYSTIMESTAMP;
BEGIN
DECLARE ScriptBody NCLOB;
BEGIN
ScriptBody := 'declare
n int := 0;
begin
select count(*) into n
from PACKAGES
where code=''DECA'';
if n = 0 then
Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values (''DECA'',''Declaration Cancellation'',''DECA,Declaration Cancellation'',0,''BS'');
Insert into PACKAGES (CODE,NAME,SEARCHFIELDS,INACTIVE,FEATUREPACKAGETYPECODE) values (''DECAA'',''Declaration Cancellation AddOn'',''DECAA,Declaration Cancellation AddOn'',0,''AD'');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values (''G-23'',''DECA'',''DECAA'');commit;
end if;
end;';
INSERT INTO "DBSCRIPTSHISTORY"("SXMLFILENAME", "EXECUTIONDATE", "SCRIPTBODY", "ELAPSEDTIMEINMS", "HASHVALUE", "VERSION")VALUES('202007031145_DeclarationCancellationAddOn.sxml', SYSDATE, ScriptBody, EXTRACT(DAY FROM(EndTime - StartTime) * 24 * 60 * 60 * 1000), '584da0fff34716b45bb5f101fe923b74', 1);
END;
END;
EXCEPTION
WHEN OTHERS THEN
ROLLBACK TO ScriptSavePoint;
COMMIT;
END;

