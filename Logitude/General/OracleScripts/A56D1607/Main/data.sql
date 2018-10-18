
update contacts set email=lower(email) ;


commit 
/


--
-- Run this script on: (these schemas will be modified)
--
-- univ56(amital).AMINETPRE_MAIN (11.2g)
--
-- to synchronize it with:
--
-- univ56(amital).AMINET_MAIN (11.2g)
--
-- You are recommended to back up your database before running this script.
--
create table z1607PACKAGES as  select * from PACKAGES ;


-- Script created by Data Compare for Oracle, version 2.6.5.1090 from Red Gate Software Ltd at 18/07/2016 08:02:04.
--

DECLARE
  null_value CHAR(1) := NULL;
  statement1 CHAR(126);
  statement2 CHAR(120);
BEGIN
  statement1 := 'INSERT INTO  "PACKAGES" (code,"NAME",searchfields,"INACTIVE",featurepackagetypecode) VALUES (:0, :1, :2, :3, :4)';
  EXECUTE IMMEDIATE statement1 USING 'BASCB', 'Basic', n'BASCB,Basic', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'BUCHB', 'BUCH', n'BUCHB,BUCH', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'BUSNB', 'Business', n'BUSNB,Business', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'CUSTB', 'Customs', n'CUSTB,Customs', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'DVMTB', 'Development', n'DVMT,Development', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'EAWBB', 'E-AWB', n'EAWBB,E-AWB', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'ECONB', 'Economy', n'ECONB,Economy', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'HBRDB', 'Hybrid CRM', n'HBRDB,Hybrid CRM', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'IMPOB', 'Importers', n'IMPOB,Importers', 0, 'BS';
  EXECUTE IMMEDIATE statement1 USING 'TNT0B', 'Tenant 0', n'TNT0B,Tenant 0', 0, 'BS';
  statement2 := 'UPDATE  "PACKAGES" SET "NAME"=:0, searchfields=:1, "INACTIVE"=:2, featurepackagetypecode=:3 WHERE code=:w0';
  EXECUTE IMMEDIATE statement2 USING 'Basic', n'BASC,Basic', 0, 'PK', 'BASC';
  EXECUTE IMMEDIATE statement2 USING 'Business', n'BUSN,Business', 0, 'PK', 'BUSN';
  EXECUTE IMMEDIATE statement2 USING 'Customs', n'CUST,Customs', 0, 'PK', 'CUST';
  EXECUTE IMMEDIATE statement2 USING 'Development', n'DVMT,Development', 0, 'PK', 'DVMT';
  EXECUTE IMMEDIATE statement2 USING 'E-AWB', n'EAWB,E-AWB', 0, 'PK', 'EAWB';
  EXECUTE IMMEDIATE statement2 USING 'Economy', n'ECON,Economy', 0, 'PK', 'ECON';
  EXECUTE IMMEDIATE statement2 USING 'Importers', n'IMPO,Importers', 0, 'PK', 'IMPO';
END;
/
COMMIT;

/

begin 

Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('2','BUSN','BUSNB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('3','DVMT','DVMTB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('4','ECON','ECONB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('1-13','BASC','BASCB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('1-14','CUST','CUSTB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('1-15','EAWB','EAWBB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('1-16','IMPO','IMPOB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('6','IMPO','IMPOB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('8','BASC','BASCB');
Insert into PACKAGECONNECTEDPACKAGES (ID,PACKAGECODE,CONNECTEDPACKAGECODE) values ('9','CUSTB','CUSTB');
END;
/
COMMIT;

/


DECLARE
  v_temp NUMBER(1, 0) := 0;
BEGIN
  -- select PackageCode from PackageFeatures group by PackageCode
  BEGIN
    SELECT 1
    INTO v_temp
    FROM DUAL
    WHERE NOT EXISTS
      ( SELECT * FROM PackageFeatures WHERE LENGTH(PackageCode) = 5
      );
  EXCEPTION
  WHEN OTHERS THEN
    NULL;
  END;
  IF v_temp = 1 THEN
    DECLARE
      v_Id      VARCHAR2(15);
      v_Code    VARCHAR2(5);
      v_NewCode VARCHAR2(5);
    BEGIN
      DECLARE
        CURSOR DataCursor
        IS
          SELECT Id , PackageCode FROM PackageFeatures ;
      BEGIN
        OPEN DataCursor;
        LOOP
        FETCH DataCursor INTO v_Id,v_Code;
          EXIT WHEN  DataCursor%notfound;
        
          BEGIN
            v_NewCode := v_Code || 'B' ;
            UPDATE PackageFeatures SET PackageCode = v_NewCode WHERE Id = v_Id;
            FETCH DataCursor INTO v_Id,v_Code;
          END;
        END LOOP;
        CLOSE DataCursor;
      END;
    END;
  END IF;
END;


/

Commit ;
 /