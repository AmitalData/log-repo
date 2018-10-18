--spool output.txt 
set serveroutput on;

--drop table z1607apicredintials ;
create table  z1607apicredintials as SELECT * FROM apicredintials;

--drop table rg_temp_1076803045_106;
CREATE TABLE rg_temp_1076803045_106 (
  "ID" VARCHAR2(15 CHAR) NOT NULL,
  tenant NUMBER(10) NOT NULL,
  usedfor NCLOB,
  hashedprimaryaccesskey NCLOB,
  hashedseconderyaccesskey NCLOB,
  createdate TIMESTAMP(7) NOT NULL,
  updatedate TIMESTAMP(7) NOT NULL,
  allowedips NCLOB,
  createdby NCLOB,
  updatedby NCLOB,
  maskedprimaryaccesskey NCLOB,
  maskedseconderyaccesskey NCLOB,
  CONSTRAINT RG_TEMP_1076803045_116 PRIMARY KEY ("ID") USING INDEX (CREATE UNIQUE INDEX rg_temp_1076803045_117 ON rg_temp_1076803045_106("ID")    )
);


DECLARE
    ex_custom       EXCEPTION;
BEGIN

 
 

 DBMS_OUTPUT.PUT_LINE('create table *2 ' );
 
INSERT INTO rg_temp_1076803045_106("ID",tenant,usedfor,hashedprimaryaccesskey,hashedseconderyaccesskey,createdate,updatedate,allowedips,createdby,updatedby,maskedprimaryaccesskey,maskedseconderyaccesskey) SELECT SUBSTR("ID", 0, LEAST(LENGTH("ID"), 15)),tenant,usedfor,hashedprimaryaccesskey,hashedseconderyaccesskey,createdate,updatedate,allowedips,createdby,updatedby,maskedprimaryaccesskey,maskedseconderyaccesskey FROM apicredintials;
DBMS_OUTPUT.PUT_LINE('INSERT INTO  ' );
--  RAISE ex_custom;
COMMIT ;
DBMS_OUTPUT.PUT_LINE('INSERT INTO  COMMIT ' );

 EXECUTE IMMEDIATE 'DROP TABLE apicredintials' ;
 DBMS_OUTPUT.PUT_LINE('drop ' );

EXECUTE IMMEDIATE 'ALTER TABLE rg_temp_1076803045_106 RENAME TO apicredintials' ;

--ALTER TABLE apicredintials RENAME CONSTRAINT RG_TEMP_1076803045_116 TO pk_apicredintials;
EXECUTE IMMEDIATE 'ALTER TABLE apicredintials RENAME CONSTRAINT RG_TEMP_1076803045_116 TO pk_apicredintials' ;

--ALTER INDEX rg_temp_1076803045_117 RENAME TO PK_APICREDINTIALS;
EXECUTE IMMEDIATE 'ALTER INDEX rg_temp_1076803045_117 RENAME TO PK_APICREDINTIALS' ;

DBMS_OUTPUT.PUT_LINE('done' );

EXCEPTION
    WHEN ex_custom THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM);

end;