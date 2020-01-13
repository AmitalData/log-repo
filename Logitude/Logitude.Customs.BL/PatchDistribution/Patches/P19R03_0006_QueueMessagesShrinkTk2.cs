using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    class P19R03_0006_QueueMessagesShrinkTk2 : PatchDistributionBase
    {
        public P19R03_0006_QueueMessagesShrinkTk2()
             : base("-רישום ComepleteDateTime ועדכון סטטוס תור ", new DateTime(2019, 12, 2))
        {

        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript(@"create or replace PROCEDURE Queue_DelayMessage(
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
      , RetryNumber          = (RetryNumber + 1)
      WHERE Id            = v_MessageId;
       
       UPDATE QueueMessageMoreDetails
      SET NextRunDateTime =  (SYSDATE + v_DelaySeconds/86400)--utils.dateadd('SECOND', v_DelaySeconds, SYSDATE)
      , RetryNumber          = (RetryNumber + 1)
      WHERE Id            = v_MessageId;
    END;
  END IF;
END");

            this.AddUpSqlScript(@"create or replace PROCEDURE Queue_Peek(
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
    
    
      UPDATE queuemessagemoredetails
      SET ProcessingDateTime = SYSDATE,
        NextRunDateTime      = (SYSDATE + v_NextRunDelayInSec/86400), ---(SYSDATE + v_frequency/86400),----utils.dateadd('SECOND', v_frequency, SYSDATE),
        RetryNumber          = (RetryNumber + 1)
      WHERE Id               = v_MessageId;-- return queue data
    
    
    END;
   END IF; 
  END IF;
  CLOSE c_1;
END;
  
END");
            this.AddUpSqlScript(
                @"CREATE OR REPLACE PROCEDURE queue_setstatus (
    v_messageid   IN   NUMBER,
    v_statud      IN   NUMBER
) AS
    v_temp NUMBER(1, 0) := 0;
BEGIN
    BEGIN
        SELECT
            1
        INTO v_temp
        FROM
            dual
        WHERE
            EXISTS (
                SELECT
                    id
                FROM
                    queuemessages
                WHERE
                    id = v_messageid
            );

    EXCEPTION
        WHEN OTHERS THEN
            NULL;
    END;

    IF v_temp = 1 THEN
        IF ( v_statud = 1 ) THEN
            BEGIN
                DELETE FROM queuemessages
                WHERE
                    id = v_messageid;

            END;
        ELSE
            BEGIN
                UPDATE queuemessages
                SET
                    status = v_statud,
                    completedatetime = SYSDATE
                WHERE
                    id = v_messageid;

            END;
        END IF;

    END IF;

    UPDATE queuemessagemoredetails
                SET
        status = v_statud,
        completedatetime = SYSDATE
    WHERE
        id = v_messageid;

END");

            
            
        }
    }
}
