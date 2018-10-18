create or replace PROCEDURE usp_GetNextTableIdValue
  --drop procedure [dbo].[usp_GetNextTableIdValue]
  (
    v_pLastNumber OUT VARCHAR2,
    v_pTableName IN VARCHAR2 )
AS
  v_COUNTER          NUMBER(10,0);
  v_Current          NUMBER(10,0);
  v_DataBaseNumber   NUMBER(10,0);
  v_DBStringNumber   VARCHAR2(50);
  v_CurrentStrNumber VARCHAR2(50);
  v_temp             NUMBER(1, 0) := 0;
BEGIN
  v_COUNTER := 1 ;
  SELECT DataBaseNumber
  INTO v_DataBaseNumber
  FROM DataBaseProperties
  WHERE ROWNUM     <= 1;
   
  v_DBStringNumber := CAST (v_DataBaseNumber as varchar2);
  BEGIN
    SELECT 1
    INTO v_temp
    FROM DUAL
    WHERE NOT EXISTS
      ( SELECT TableName FROM DBIdCounters WHERE TableName = v_pTableName
      );
  EXCEPTION
  WHEN OTHERS THEN
    NULL;
  END;
  IF v_temp = 1 THEN
    BEGIN
      v_COUNTER := 1 ;
      INSERT
      INTO DBIdCounters
        (
          TableName,
          LastIdNumber
        )
        VALUES
        (
          v_pTableName,
          v_COUNTER
        );
      v_Current := 1 ;
    END;
  ELSE
    BEGIN
      SELECT LastIdNumber
      INTO v_Current
      FROM DBIdCounters
      WHERE TableName = v_pTableName 
      FOR UPDATE ; -- 20160211  ora-0001 ----no need     skip locked good for gggq select 1 row from alot ;;
      v_Current      := v_Current + 1 ;
      UPDATE DBIdCounters
      SET LastIdNumber = v_Current
      WHERE TableName  = v_pTableName;
    END;
  END IF;
  Begin
  v_CurrentStrNumber := CAST (v_Current as varchar2);
  v_pLastNumber      := v_DBStringNumber || '-' || v_CurrentStrNumber ;
  end;
END;