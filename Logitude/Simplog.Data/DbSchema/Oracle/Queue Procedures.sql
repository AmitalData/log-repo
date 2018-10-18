GRANT CREATE SEQUENCE TO aminet_main ;
/

CREATE SEQUENCE QUEUEMESSAGES_SEQ1 START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 99999 NOCACHE  CYCLE  NOORDER
/
 
--------------------- Queue_DelayMessage ---------------

create or replace PROCEDURE Queue_DelayMessage(
    v_MessageId    IN NUMBER,
    v_DelaySeconds IN NUMBER )
AS
  v_temp NUMBER(1, 0) := 0;
BEGIN
  BEGIN
    SELECT 1
    INTO v_temp
    FROM DUAL
    WHERE EXISTS
      ( SELECT Id FROM QueueMessages WHERE ID = v_MessageId
      );
  EXCEPTION
  WHEN OTHERS THEN
    NULL;
  END;
  IF v_temp = 1 THEN
    BEGIN
      UPDATE QueueMessages
      SET NextRunDateTime =  (SYSDATE + v_DelaySeconds/86400)--utils.dateadd('SECOND', v_DelaySeconds, SYSDATE)
      WHERE Id            = v_MessageId;
    END;
  END IF;
END;

--------------------Queue_Enqueue------------------------------

create or replace PROCEDURE Queue_Enqueue(
    v_QueueDefinitionCode IN VARCHAR2,
    v_MessageBody         IN VARCHAR2,
    v_DelaySeconds        IN NUMBER,
    v_CustomerId          IN VARCHAR2,
    v_BatchNumber         IN VARCHAR2 ,
    v_QueueMessageId         OUT QUEUEMESSAGES.ID%TYPE 
    )
AS
  v_currentdate     DATE;  
  v_nextRunDateTime DATE;
  v_CId             VARCHAR2(15);
  v_BNo             VARCHAR2(15);
BEGIN
  v_currentdate     := SYSDATE ;
  v_nextRunDateTime := (SYSDATE + v_DelaySeconds/86400) ;
  INSERT
  INTO QueueMessages
    (
      CreateDateTime,
      QueueDefinitionCode,
      Status,
      MessageBody,
      NextRunDateTime,
      RetryNumber
    )
    VALUES
    (
      v_currentdate,
      v_QueueDefinitionCode,
      0,
      v_MessageBody,
      v_nextRunDateTime,
      0
    );
  IF v_CustomerId = ' ' THEN
    v_CId        := NULL ;
  ELSE
    v_CId := v_CustomerId ;
  END IF;
  IF v_BatchNumber = ' ' THEN
    v_BNo         := NULL ;
  ELSE
    v_BNo := v_BatchNumber ;
  END IF;
  
  v_QueueMessageId :=QUEUEMESSAGES_SEQ1.CURRVAL;
  
  INSERT
  INTO QueueMessageMoreDetails
    (
      Id,
      CreateDateTime,
      QueueDefinitionCode,
      Status,
      MessageBody,
      NextRunDateTime,
      RetryNumber,
      Field1,
      Field2
    )
    VALUES
    (
      v_QueueMessageId ---QUEUEMESSAGES_SEQ1.CURRVAL
      ,
      v_currentdate,
      v_QueueDefinitionCode,
      0,
      v_MessageBody,
      v_nextRunDateTime,
      0,
      v_CId,
      v_BNo
    );
END;


----------------------Queue_Peek---------------------------

create or replace PROCEDURE Queue_Peek(
    v_MessageId OUT NUMBER,
    v_MessageBody OUT VARCHAR2,
    v_RetryNumber OUT NUMBER,
    v_MessageCreatedServerTime OUT QUEUEMESSAGES.CREATEDATETIME%TYPE ,
    v_QueueDefinitionCode IN VARCHAR2 ,
    v_NextRunDelayInSec IN NUMBER)
AS
  --DECLARE @NextId INTEGER
  v_frequency NUMBER(10,0);
  v_row QueueMessages%rowtype;
BEGIN
  v_frequency := 60 ;
  -- Find next available item available where the status is enabled
  
 DECLARE
  CURSOR c_1 IS  
    SELECT /*+FIRST_ROWS_1*/  * --qt.Id
    FROM QueueMessages qt
    WHERE qt.NextRunDateTime <= SYSDATE
     --ROWNUM<2  AND  --do not use rownum due  http://stackoverflow.com/questions/6117254/force-oracle-to-return-top-n-rows-with-skip-locked/6649586#6649586
    AND qt.Status             = 0
    AND QueueDefinitionCode = v_QueueDefinitionCode
    ORDER BY qt.NextRunDateTime ASC
    FOR UPDATE SKIP LOCKED;
BEGIN
  OPEN c_1;
  FETCH c_1 into v_row; -- v_MessageId;
  IF c_1%FOUND THEN
     --If f
  --If found, flag it to prevent being picked up again
  IF ( v_row.Id IS NOT NULL ) THEN
    BEGIN
    v_MessageId := v_row.id;
      --SELECT RetryNumber       INTO v_RetryNumber        FROM QueueMessages        WHERE Id = v_MessageId;
      v_RetryNumber := v_row.RetryNumber;
      
      --SELECT MessageBody      INTO v_MessageBody       FROM QueueMessages       WHERE Id = v_MessageId;
      v_MessageBody := v_row.MessageBody;
      v_MessageCreatedServerTime := v_row.CREATEDATETIME;
      
   
      UPDATE QueueMessages
      SET ProcessingDateTime = SYSDATE,
        NextRunDateTime      = (SYSDATE + v_NextRunDelayInSec/86400), ---(SYSDATE + v_frequency/86400),----utils.dateadd('SECOND', v_frequency, SYSDATE),
        RetryNumber          = (RetryNumber + 1)
      WHERE Id               = v_MessageId;-- return queue data
    
    END;
   END IF; 
  END IF;
  CLOSE c_1;
END;
  
END;

------------------Queue_ReturnMessage----------------------------

create or replace PROCEDURE Queue_ReturnMessage(
    v_MessageId IN NUMBER )
AS
  v_temp NUMBER(1, 0) := 0;
BEGIN
  BEGIN
    SELECT 1
    INTO v_temp
    FROM DUAL
    WHERE EXISTS
      ( SELECT Id FROM QueueMessages WHERE ID = v_MessageId
      );
  EXCEPTION
  WHEN OTHERS THEN
    NULL;
  END;
  IF v_temp = 1 THEN
    BEGIN
      UPDATE QueueMessages SET NextRunDateTime = SYSDATE WHERE Id = v_MessageId;
    END;
  END IF;
END;

------------------------Queue_SetStatus--------------------

create or replace PROCEDURE Queue_SetStatus(
    v_MessageId IN NUMBER,
    v_Statud    IN NUMBER )
AS
  v_temp NUMBER(1, 0) := 0;
BEGIN
  BEGIN
    SELECT 1
    INTO v_temp
    FROM DUAL
    WHERE EXISTS
      ( SELECT Id FROM QueueMessages WHERE Id = v_MessageId
      );
  EXCEPTION
  WHEN OTHERS THEN
    NULL;
  END;
  IF v_temp = 1 THEN
    BEGIN
      UPDATE QueueMessages
      SET Status         = v_Statud,
        CompleteDateTime = SYSDATE
      WHERE Id           = v_MessageId;
    END;
  END IF;
END;


----------------------QueueMessages_SEQ---------------------

  CREATE OR REPLACE TRIGGER "LOGITUDE_MAIN"."QUEUEMESSAGES_INS_TRG" 
  BEFORE INSERT ON QueueMessages
  FOR EACH ROW
BEGIN
  SELECT QueueMessages_SEQ.NEXTVAL INTO :NEW.Id FROM DUAL;
END;

ALTER TRIGGER "LOGITUDE_MAIN"."QUEUEMESSAGES_INS_TRG" ENABLE;
--------------------------------------------------------
--  DDL for Trigger QUEUEMESSAGES_TRG
--------------------------------------------------------

  CREATE OR REPLACE TRIGGER "LOGITUDE_MAIN"."QUEUEMESSAGES_TRG" 
BEFORE INSERT ON QUEUEMESSAGES 
FOR EACH ROW 
BEGIN
  <<COLUMN_SEQUENCES>>
  BEGIN
    IF INSERTING AND :NEW.ID IS NULL THEN
      SELECT QUEUEMESSAGES_SEQ1.NEXTVAL INTO :NEW.ID FROM SYS.DUAL;
    END IF;
  END COLUMN_SEQUENCES;
END;
/
ALTER TRIGGER "LOGITUDE_MAIN"."QUEUEMESSAGES_TRG" ENABLE;
