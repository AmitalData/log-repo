create or replace PROCEDURE usp_PreDeleteMetadata(
    v_pTableName IN VARCHAR2 )
AS
  v_ObjectTableId VARCHAR2(15);
  tbl_exist  PLS_INTEGER;
 featuresCreation varchar2(500);
 spellCheckedCreation varchar2(500);
BEGIN
  
    ----------------------------------------------------------------Run automation Update before delete

select count(*) into tbl_exist from user_tables where table_name = 'TEMPOLDFEATURES';
if tbl_exist = 1 then
    execute immediate 'drop table TEMPOLDFEATURES';
    dbms_output.put_line('drop table TEMPOLDFEATURES');
end if;
dbms_output.put_line('out of drop table TEMPOLDFEATURES' || tbl_exist);
 --select * from TempOldFeatures
featuresCreation:= 'CREATE TABLE TEMPOLDFEATURES
AS
SELECT *
    FROM
      (SELECT Code ,
        FeatureUniqeCode ,
        IsOld ,
        Tenant
      FROM features
      WHERE IsOld = 1
      ) t';
     execute immediate featuresCreation;

select count(*) into tbl_exist from user_tables where table_name = 'TEMPISSPELLCHECKEDTEXTCODES';
if tbl_exist = 1 then
    execute immediate 'drop table TEMPISSPELLCHECKEDTEXTCODES';
end if;

spellCheckedCreation:= 'CREATE TABLE TEMPISSPELLCHECKEDTEXTCODES
   AS
    SELECT * FROM
      ( SELECT * FROM TextCodes WHERE IsSpellChecked = 1
      ) t';--select * from TempIsSpellCheckedTextCodes
   execute immediate spellCheckedCreation;
  
END;