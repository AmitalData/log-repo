-----------------------------------------------------------
-- customs diffrent then main- mssql
USE [main]
GO

/****** Object:  StoredProcedure [dbo].[QUEUE_PEEK]    Script Date: 19/09/2022 14:05:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[QUEUE_PEEK]  
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_MESSAGEID float(53)  OUTPUT,
   @V_MESSAGEBODY varchar(max)  OUTPUT,
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_RETRYNUMBER float(53)  OUTPUT,
   @V_MESSAGECREATEDSERVERTIME datetime2(7)  OUTPUT,
   @V_QUEUEDEFINITIONCODE varchar(max),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_NEXTRUNDELAYINSEC float(53),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_WATINGSTATUS float(53)
AS 
   BEGIN

      SET @V_MESSAGEID = NULL

      SET @V_MESSAGEBODY = NULL

      SET @V_RETRYNUMBER = NULL

      SET @V_MESSAGECREATEDSERVERTIME = NULL

      /*DECLARE @NextId INTEGER*/
      DECLARE
         @V_FREQUENCY numeric(10, 0)

      DECLARE
         @V_ROW$ID numeric(18, 0), 
         @V_ROW$QUEUEDEFINITIONCODE varchar(265), 
         @V_ROW$CREATEDATETIME datetime2(7), 
         @V_ROW$STATUS numeric(10, 0), 
         @V_ROW$MESSAGEBODY varchar(1000), 
         @V_ROW$NEXTRUNDATETIME datetime2(7), 
         @V_ROW$PROCESSINGDATETIME datetime2(7), 
         @V_ROW$COMPLETEDATETIME datetime2(7), 
         @V_ROW$RETRYNUMBER numeric(10, 0), 
         @V_ROW$TENANT numeric(10, 0), 
         @V_ROW$HASHCODE nvarchar(max), 
         @V_ROW$TENANTPRIORITY numeric(10, 0), 
         @V_ROW$INTERFACETYPECODE varchar(32), 
         @V_ROW$QUEUECODERABBIT varchar(256), 
         @V_ROW$USERABBITMQ numeric(1, 0), 
         @V_ROW$HAVERABBITMQ numeric(1, 0), 
         @V_ROW$RABBITMQCREATEDATE datetime2(7), 
         @V_ROW$RABBITMQRETRYNUMBER numeric(10, 0), 
         @V_ROW$RABBITMQERRMESS varchar(256), 
         @V_ROW$ENTITYCODE varchar(40), 
         @V_ROW$ENTITYID varchar(40)

      /*-20220127 IM Rabbit*/
      SET @V_FREQUENCY = 60

      /* Find next available item available where the status is enabled*/
      BEGIN

         DECLARE
             C_1 CURSOR LOCAL FOR 
               /*
               *   SSMA warning messages:
               *   O2SS0210: Conversion of the specified hint is not supported: +FIRST_ROWS_1.
               */

               SELECT 
                  QT.ID, 
                  QT.QUEUEDEFINITIONCODE, 
                  QT.CREATEDATETIME, 
                  QT.STATUS, 
                  QT.MESSAGEBODY, 
                  QT.NEXTRUNDATETIME, 
                  QT.PROCESSINGDATETIME, 
                  QT.COMPLETEDATETIME, 
                  QT.RETRYNUMBER, 
                  QT.TENANT, 
                  QT.HASHCODE, 
                  QT.TENANTPRIORITY, 
                  QT.INTERFACETYPECODE, 
                  QT.QUEUECODERABBIT, 
                  QT.USERABBITMQ, 
                  QT.HAVERABBITMQ, 
                  QT.RABBITMQCREATEDATE, 
                  QT.RABBITMQRETRYNUMBER, 
                  QT.RABBITMQERRMESS, 
                  QT.ENTITYCODE, 
                  QT.ENTITYID
               FROM dbo.QUEUEMESSAGES  AS QT 
                  WITH ( UPDLOCK,  READPAST )
               WHERE 
                  QT.NEXTRUNDATETIME <= sysdatetime() AND 
                  /*ROWNUM<2  AND  --do not use rownum due  http://stackoverflow.com/questions/6117254/force-oracle-to-return-top-n-rows-with-skip-locked/6649586#6649586*/QT.STATUS = @V_WATINGSTATUS AND 
                  QT.QUEUEDEFINITIONCODE = @V_QUEUEDEFINITIONCODE AND 
                  QT.USERABBITMQ = 0
               ORDER BY QT.TENANTPRIORITY ASC, QT.NEXTRUNDATETIME ASC

         OPEN C_1

         FETCH C_1
             INTO 
               @V_ROW$ID, 
               @V_ROW$QUEUEDEFINITIONCODE, 
               @V_ROW$CREATEDATETIME, 
               @V_ROW$STATUS, 
               @V_ROW$MESSAGEBODY, 
               @V_ROW$NEXTRUNDATETIME, 
               @V_ROW$PROCESSINGDATETIME, 
               @V_ROW$COMPLETEDATETIME, 
               @V_ROW$RETRYNUMBER, 
               @V_ROW$TENANT, 
               @V_ROW$HASHCODE, 
               @V_ROW$TENANTPRIORITY, 
               @V_ROW$INTERFACETYPECODE, 
               @V_ROW$QUEUECODERABBIT, 
               @V_ROW$USERABBITMQ, 
               @V_ROW$HAVERABBITMQ, 
               @V_ROW$RABBITMQCREATEDATE, 
               @V_ROW$RABBITMQRETRYNUMBER, 
               @V_ROW$RABBITMQERRMESS, 
               @V_ROW$ENTITYCODE, 
               @V_ROW$ENTITYID/* v_MessageId;*/

         /*
         *   SSMA warning messages:
         *   O2SS0113: The value of @@FETCH_STATUS might be changed by previous FETCH operations on other cursors, if the cursors are used simultaneously.
         */
		 
         IF @@FETCH_STATUS = 0
            BEGIN
               
               /*
               *   If f
               *   If found, flag it to prevent being picked up again
               */
               IF (@V_ROW$ID IS NOT NULL)
                  BEGIN

                     SET @V_MESSAGEID = @V_ROW$ID

                     /*SELECT RetryNumber       INTO v_RetryNumber        FROM QueueMessages        WHERE Id = v_MessageId;*/
                     SET @V_RETRYNUMBER = @V_ROW$RETRYNUMBER

                     /*SELECT MessageBody      INTO v_MessageBody       FROM QueueMessages       WHERE Id = v_MessageId;*/
                     SET @V_MESSAGEBODY = @V_ROW$MESSAGEBODY

                     SET @V_MESSAGECREATEDSERVERTIME = @V_ROW$CREATEDATETIME

                     /*
                     *   SSMA warning messages:
                     *   O2SS0425: Dateadd operation may cause bad performance.
                     */

                     UPDATE dbo.QUEUEMESSAGES
                        SET 
                           PROCESSINGDATETIME = sysdatetime(), 
                           NEXTRUNDATETIME = DATEADD(ss,@V_NEXTRUNDELAYINSEC,GETDATE()) , --ssma_oracle.dateadd(@V_NEXTRUNDELAYINSEC / 86400, sysdatetime())/*-(SYSDATE + v_frequency/86400),----utils.dateadd('SECOND', v_frequency, SYSDATE),*/, 
                           RETRYNUMBER = (QUEUEMESSAGES.RETRYNUMBER + 1)
                     WHERE QUEUEMESSAGES.ID = @V_MESSAGEID/* return queue data*/

                     /*
                     *   SSMA warning messages:
                     *   O2SS0425: Dateadd operation may cause bad performance.
                     */

                     UPDATE dbo.QUEUEMESSAGEMOREDETAILS
                        SET 
                           PROCESSINGDATETIME = sysdatetime(), 
                           NEXTRUNDATETIME = DATEADD(ss,@V_NEXTRUNDELAYINSEC,GETDATE()) , --ssma_oracle.dateadd(@V_NEXTRUNDELAYINSEC / 86400, sysdatetime())/*-(SYSDATE + v_frequency/86400),----utils.dateadd('SECOND', v_frequency, SYSDATE),*/, 
                           RETRYNUMBER = (QUEUEMESSAGEMOREDETAILS.RETRYNUMBER + 1)
                     WHERE QUEUEMESSAGEMOREDETAILS.ID = @V_MESSAGEID/* return queue data*/

                  END
            END

         CLOSE C_1

         DEALLOCATE C_1

      END

   END
GO

---------

-- customs diffrent then main- mssql
USE [main]
GO

/****** Object:  StoredProcedure [dbo].[QUEUE_ENQUEUE]    Script Date: 15/09/2022 13:19:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Queue_Enqueue]  
--- CustomsBranch- Translate from Oracle!!!
   @V_QUEUEDEFINITIONCODE /*-create or replace PROCEDURE  TSTQueue_Enqueue(*/varchar(max),
   @V_MESSAGEBODY varchar(max),

   @V_TENANT float(53),
   @V_DELAYSECONDS float(53),
   @V_CUSTOMERID varchar(max),
   @V_BATCHNUMBER varchar(max),
   @V_HASHCODE varchar(max),

   @V_WATINGSTATUS float(53),

   @P_TENANTPRIORITY float(53),
   @P_INTERFACETYPECODE varchar(max),

   @P_USERABBITMQ float(53),
   @P_QUEUECODERABBIT varchar(max),
   @P_ENTITYCODE varchar(max) = NULL,
   @P_ENTITYID varchar(max) = NULL,
   @V_QUEUEMESSAGEID numeric(18, 0)  OUTPUT
AS 
   BEGIN

      SET @V_QUEUEMESSAGEID = NULL

      
      /*
      *   -20211108 IM TenantPriority
      *   -20220127 IM Rabbit
      */
      DECLARE
		@V_CURRENT numeric(10, 0), 
         @V_CURRENTDATE datetime2(0), 
         @V_NEXTRUNDATETIME datetime2(0), 
         @V_CID varchar(15), 
         @V_BNO varchar(15), 
         /*
         *   SSMA warning messages:
         *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
         */

         @V_TENANTPRIORITY float(53)

      SET @V_TENANTPRIORITY = @P_TENANTPRIORITY
	   
	   
	   SELECT @V_CURRENT = NEXT VALUE FOR [dbo].[QUEUEMESSAGES_SEQ]
      IF (@V_TENANTPRIORITY < 1)
         SET @V_TENANTPRIORITY = 89

      SET @V_CURRENTDATE = GETDATE()

      /*
      *   SSMA warning messages:
      *   O2SS0425: Dateadd operation may cause bad performance.
      */
	  
      SET @V_NEXTRUNDATETIME = DATEADD(ss,@V_DELAYSECONDS,GETDATE()) --ssma_oracle.dateadd(@V_DELAYSECONDS / 86400, sysdatetime())

      INSERT dbo.QUEUEMESSAGES(
		ID,
         CREATEDATETIME, 
         QUEUEDEFINITIONCODE, 
         STATUS, 
         MESSAGEBODY, 
         TENANT, 
         NEXTRUNDATETIME, 
         RETRYNUMBER, 
         HASHCODE, 
         TENANTPRIORITY, 
         INTERFACETYPECODE, 
         USERABBITMQ, 
         QUEUECODERABBIT, 
         ENTITYCODE, 
         ENTITYID)
         VALUES (
			@V_CURRENT,
            @V_CURRENTDATE, 
            @V_QUEUEDEFINITIONCODE, 
            @V_WATINGSTATUS, 
            @V_MESSAGEBODY, 
            @V_TENANT, 
            @V_NEXTRUNDATETIME, 
            0, 
            @V_HASHCODE, 
            @V_TENANTPRIORITY, 
            @P_INTERFACETYPECODE, 
            @P_USERABBITMQ, 
            @P_QUEUECODERABBIT, 
            @P_ENTITYCODE, 
            @P_ENTITYID)

      IF @V_CUSTOMERID = ' '
         SET @V_CID = NULL
      ELSE 
         SET @V_CID = @V_CUSTOMERID

      IF @V_BATCHNUMBER = ' '
         SET @V_BNO = NULL
      ELSE 
         SET @V_BNO = @V_BATCHNUMBER

      /* 
      *   SSMA error messages:
      *   O2SS0490: Conversion of identifier QUEUEMESSAGES_SEQ1.CURRVAL for CURRVAL is not supported. SQL Server sequence generator does not support retrieving current sequence value. CURRVAL method can be converted by changing the project setting for sequence conversion to use “Using SSMA sequence generator”.

      SET @V_QUEUEMESSAGEID = (NULL)
      */

	  SET @V_QUEUEMESSAGEID=@V_CURRENT

      INSERT dbo.QUEUEMESSAGEMOREDETAILS(
         ID, 
         CREATEDATETIME, 
         QUEUEDEFINITIONCODE, 
         STATUS, 
         MESSAGEBODY, 
         TENANT, 
         NEXTRUNDATETIME, 
         RETRYNUMBER, 
         FIELD1, 
         FIELD2, 
         TENANTPRIORITY)
         VALUES (
            @V_QUEUEMESSAGEID, 
            @V_CURRENTDATE, 
            @V_QUEUEDEFINITIONCODE, 
            @V_WATINGSTATUS, 
            @V_MESSAGEBODY, 
            @V_TENANT, 
            @V_NEXTRUNDATETIME, 
            0, 
            @V_CID, 
            @V_BNO, 
            @V_TENANTPRIORITY)

   END
GO


-------------------------------------------
-- customs diffrent then main- mssql
USE [main]
GO

/****** Object:  StoredProcedure [dbo].[QUEUE_SETSTATUS]    Script Date: 18/09/2022 09:57:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[QUEUE_SETSTATUS]  
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_MESSAGEID float(53),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_STATUD float(53)
AS 
   BEGIN

      /*--INSERT INTO ZQUEUE_SETSTATUS (ID, STATUS, COMPLETEDATETIME) VALUES (v_messageid, v_statud,sysdate  );*/
      DELETE dbo.QUEUEMESSAGES
      WHERE QUEUEMESSAGES.ID = @V_MESSAGEID AND @V_STATUD = 1

      UPDATE dbo.QUEUEMESSAGES
         SET 
            COMPLETEDATETIME = sysdatetime(), 
            STATUS = @V_STATUD
      WHERE QUEUEMESSAGES.ID = @V_MESSAGEID AND @V_STATUD != 1

      UPDATE dbo.QUEUEMESSAGEMOREDETAILS
         SET 
            STATUS = @V_STATUD, 
            COMPLETEDATETIME = sysdatetime()
      WHERE QUEUEMESSAGEMOREDETAILS.ID = @V_MESSAGEID

   END
GO


---------------------------------
-- customs diffrent then main- mssql
USE [main]
GO
/****** Object:  StoredProcedure [dbo].[QUEUE_DELAYMESSAGE]    Script Date: 18/09/2022 09:59:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[QUEUE_DELAYMESSAGE]  
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_MESSAGEID float(53),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_DELAYSECONDS float(53)
AS 
   BEGIN

      DECLARE
         @V_TEMP numeric(1, 0) = 0

      /*20220213 set haverabbitmq =0*/
      BEGIN

         BEGIN TRY
            SELECT @V_TEMP = 1
            WHERE EXISTS 
               (
                  SELECT QUEUEMESSAGES.ID
                  FROM dbo.QUEUEMESSAGES
                  WHERE QUEUEMESSAGES.ID = @V_MESSAGEID
               )
         END TRY

         BEGIN CATCH
            BEGIN
               DECLARE
                  @db_null_statement int
            END
         END CATCH

      END

      IF @V_TEMP = 1
         BEGIN

            /*
            *   SSMA warning messages:
            *   O2SS0425: Dateadd operation may cause bad performance.
            */

            UPDATE dbo.QUEUEMESSAGES
               SET 
                  NEXTRUNDATETIME = DATEADD(ss,@V_DELAYSECONDS,GETDATE()) ,--ssma_oracle.dateadd(@V_DELAYSECONDS / 86400, sysdatetime())/*utils.dateadd('SECOND', v_DelaySeconds, SYSDATE)*/, 
                  RETRYNUMBER = (QUEUEMESSAGES.RETRYNUMBER + 1), 
                  HAVERABBITMQ = 0
            WHERE QUEUEMESSAGES.ID = @V_MESSAGEID

            /*
            *   SSMA warning messages:
            *   O2SS0425: Dateadd operation may cause bad performance.
            */

            UPDATE dbo.QUEUEMESSAGEMOREDETAILS
               SET 
                  NEXTRUNDATETIME = DATEADD(ss,@V_DELAYSECONDS,GETDATE()) ,-- ssma_oracle.dateadd(@V_DELAYSECONDS / 86400, sysdatetime())/*utils.dateadd('SECOND', v_DelaySeconds, SYSDATE)*/, 
                  RETRYNUMBER = (QUEUEMESSAGEMOREDETAILS.RETRYNUMBER + 1)
            WHERE QUEUEMESSAGEMOREDETAILS.ID = @V_MESSAGEID

         END

   END
   ----------------------------------------------
   ---revive from main never change !!
USE [main]
GO

/****** Object:  StoredProcedure [dbo].[USP_GETNEXTTABLENUMBERVALUE]    Script Date: 19/09/2022 08:11:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

השתמש ב של רמאללה CREATE OR ALTER PROCEDURE [dbo].[USP_GETNEXTTABLENUMBERVALUE]
(
@pLastValue INT OUTPUT,
@pTenant    INT,
@pCounterId    nvarchar(40),
@pPrefix    nvarchar(15),
@pStartNumber    INT
)
AS
DECLARE @COUNTER AS INT
SET @COUNTER = @pStartNumber
Declare @Current As Int
IF @pPrefix IS NOT NULL
BEGIN
IF NOT EXISTS (SELECT CounterId
FROM CounterStats with (UPDLOCK ROWLOCK) WHERE CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant)
BEGIN;
SET @COUNTER = @pStartNumber
INSERT INTO CounterStats
(
Tenant,
CounterId,
Prefix,
LastValue
)
VALUES
(
@pTenant,
@pCounterId,
@pPrefix,
@pStartNumber
)
Set @Current = @pStartNumber
End
Else
BEGIN;
Set @Current	= (SELECT  LastValue
FROM CounterStats with (UPDLOCK ROWLOCK) WHERE  CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant)
Set @Current = @Current + 1
Update CounterStats
set LastValue = @Current
Where   CounterId = @pCounterId and Prefix = @pPrefix and Tenant =  @pTenant
End;
END
ELSE
BEGIN
IF NOT EXISTS (SELECT CounterId
FROM CounterStats with (UPDLOCK ROWLOCK) WHERE CounterId = @pCounterId and Tenant =  @pTenant)
BEGIN;
SET @COUNTER = @pStartNumber
INSERT INTO CounterStats
(
Tenant,
CounterId,
Prefix,
LastValue
)
VALUES
(
@pTenant,
@pCounterId,
@pPrefix,
@pStartNumber
)
Set @Current = @pStartNumber
End
Else
BEGIN;
Set @Current	= (SELECT  LastValue
FROM CounterStats with (UPDLOCK ROWLOCK) WHERE  CounterId = @pCounterId  and Tenant =  @pTenant)
Set @Current = @Current + 1
Update CounterStats
set LastValue = @Current
Where   CounterId = @pCounterId  and Tenant =  @pTenant
End;
END
Set @pLastValue = @Current
GO


-----------------------------------------
-- customs diffrent then main- mssql
USE [main]
GO
/****** Object:  StoredProcedure [dbo].[USP_GETNEXTTABLEIDVALUE]    Script Date: 19/09/2022 08:13:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[USP_GETNEXTTABLEIDVALUE]  
   @V_PLASTNUMBER /*drop procedure [dbo].[usp_GetNextTableIdValue]*/varchar(max)  OUTPUT,
   @V_PTABLENAME varchar(max)
AS 
   BEGIN

      SET @V_PLASTNUMBER = NULL

      DECLARE
         @V_COUNTER numeric(10, 0), 
         @V_CURRENT numeric(10, 0), 
         @V_DATABASENUMBER numeric(10, 0), 
         @V_DBSTRINGNUMBER varchar(50), 
         @V_CURRENTSTRNUMBER varchar(50), 
         @V_TEMP numeric(1, 0) = 0

      SET @V_COUNTER = 1

      SELECT TOP (1) @V_DATABASENUMBER = DATABASEPROPERTIES.DATABASENUMBER
      FROM dbo.DATABASEPROPERTIES

      SET @V_DBSTRINGNUMBER = CAST(@V_DATABASENUMBER AS varchar(max))

      /*-!<new> 20220426- faster create seq*/
      IF @V_PTABLENAME = 'Document'
         BEGIN

            SELECT @V_CURRENT = NEXT VALUE FOR dbo.DOCUMENTDBIDCOUNTERS_SEQ

            SET @V_CURRENTSTRNUMBER = CAST(@V_CURRENT AS varchar(max))

            SET @V_PLASTNUMBER = ISNULL(@V_DBSTRINGNUMBER, '') + '-' + ISNULL(@V_CURRENTSTRNUMBER, '')

            RETURN 

         END

      IF @V_PTABLENAME = 'CommunicationLog'
         BEGIN

            SELECT @V_CURRENT = NEXT VALUE FOR dbo.COMMLOG_DBIDCOUNTERS_SEQ

            SET @V_CURRENTSTRNUMBER = CAST(@V_CURRENT AS varchar(max))

            SET @V_PLASTNUMBER = ISNULL(@V_DBSTRINGNUMBER, '') + '-' + ISNULL(@V_CURRENTSTRNUMBER, '')

            RETURN 

         END

      /*-!</new>*/
      BEGIN

         BEGIN TRY
            SELECT @V_TEMP = 1
            WHERE NOT EXISTS 
               (
                  SELECT DBIDCOUNTERS.TABLENAME
                  FROM dbo.DBIDCOUNTERS
                  WHERE DBIDCOUNTERS.TABLENAME = @V_PTABLENAME
               )
         END TRY

         BEGIN CATCH
            BEGIN
               DECLARE
                  @db_null_statement int
            END
         END CATCH

      END

      IF @V_TEMP = 1
         BEGIN

            SET @V_COUNTER = 1

            INSERT dbo.DBIDCOUNTERS(TABLENAME, LASTIDNUMBER)
               VALUES (@V_PTABLENAME, @V_COUNTER)

            SET @V_CURRENT = 1

         END
      ELSE 
         BEGIN

            SELECT @V_CURRENT = DBIDCOUNTERS.LASTIDNUMBER
            FROM dbo.DBIDCOUNTERS 
               WITH ( UPDLOCK )
            WHERE DBIDCOUNTERS.TABLENAME = @V_PTABLENAME

            /* 20160211  ora-0001 ----no need     skip locked good for gggq select 1 row from alot ;;*/
            SET @V_CURRENT = @V_CURRENT + 1

            UPDATE dbo.DBIDCOUNTERS
               SET 
                  LASTIDNUMBER = @V_CURRENT
            WHERE DBIDCOUNTERS.TABLENAME = @V_PTABLENAME

         END

      BEGIN

         SET @V_CURRENTSTRNUMBER = CAST(@V_CURRENT AS varchar(max))

         SET @V_PLASTNUMBER = ISNULL(@V_DBSTRINGNUMBER, '') + '-' + ISNULL(@V_CURRENTSTRNUMBER, '')

      END

   END



------------------------
USE [main]
GO

/****** Object:  StoredProcedure [dbo].[USP_GETNEXTTABLECODEVALUE]    Script Date: 18/09/2022 16:36:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-----------------------------------------------------
-- customs diffrent then main- mssql - change 
CREATE PROCEDURE [dbo].[USP_GETNEXTTABLECODEVALUE]  
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @pLastNumber float(53)  OUTPUT,
   @pTableName varchar(max),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @pTenant float(53)
AS 
   BEGIN

      SET @pLastNumber = NULL

      DECLARE
         @V_COUNTER numeric(10), 
         @V_CURRENT numeric(10), 
         @V_TEMP numeric(1) = 0, 
         @V_WAITCOUNTER numeric(10), 
         @V_DONE bit

      SET @V_COUNTER = 1000

      SET @V_WAITCOUNTER = 0

      SET @V_DONE = 0

      DECLARE
         @db_null_statement int

      BEGIN

         BEGIN TRY
            SELECT @V_TEMP = 1
            WHERE NOT EXISTS 
               (
                  SELECT COUNTERLASTNUMBERS.TABLENAME
                  FROM dbo.COUNTERLASTNUMBERS
                  WHERE COUNTERLASTNUMBERS.TABLENAME = @pTableName AND COUNTERLASTNUMBERS.TENANT = @pTenant
               )
         END TRY

         BEGIN CATCH
            BEGIN
               DECLARE
                  @db_null_statement$2 int
            END
         END CATCH

      END

      IF @V_TEMP = 1
         BEGIN
            IF (@pTableName = 'Tenant')
               BEGIN

                  SET @V_COUNTER = 0

                  INSERT dbo.COUNTERLASTNUMBERS(TABLENAME, TENANT, LASTNUMBER)
                     VALUES (@pTableName, @pTenant, @V_COUNTER)

                  SET @V_CURRENT = 0

               END
            ELSE 
               BEGIN

                  SET @V_COUNTER = 1000

                  IF (@pTableName = 'Agent')
                     BEGIN
                        SET @V_COUNTER = 10000
                     END

                  IF (@pTableName = 'CustomAgent')
                     BEGIN
                        SET @V_COUNTER = 20000
                     END

                  IF (@pTableName = 'ShippingAgent')
                     BEGIN
                        SET @V_COUNTER = 30000
                     END

                  IF (@pTableName = 'Customer')
                     BEGIN
                        SET @V_COUNTER = 70000
                     END

                  INSERT dbo.COUNTERLASTNUMBERS(TABLENAME, TENANT, LASTNUMBER)
                     VALUES (@pTableName, @pTenant, @V_COUNTER)

                  SET @V_CURRENT = @V_COUNTER

               END
         END
      ELSE 
         BEGIN

            SET @V_WAITCOUNTER = 0

            WHILE @V_DONE = 0
            
               BEGIN
                  BEGIN

                     BEGIN TRY

                        /*
                        *   SSMA warning messages:
                        *   O2SS0165: The WAIT clause was ignored during conversion.
                        */

                        SELECT @V_CURRENT = COUNTERLASTNUMBERS.LASTNUMBER
                        FROM dbo.COUNTERLASTNUMBERS 
                           WITH ( UPDLOCK )
                        WHERE COUNTERLASTNUMBERS.TABLENAME = @pTableName AND COUNTERLASTNUMBERS.TENANT = @pTenant

                        SET @V_DONE = 1

                     END TRY

                     BEGIN CATCH

                        DECLARE
                           @errornumber int

                        SET @errornumber = ERROR_NUMBER()

                        DECLARE
                           @errormessage nvarchar(4000)

                        SET @errormessage = ERROR_MESSAGE()

                        DECLARE
                           @exceptionidentifier nvarchar(4000)

                        SELECT @exceptionidentifier = ssma_oracle.db_error_get_oracle_exception_id(@errormessage, @errornumber)

                        BEGIN/* exception handlers begin*/

                           /* handles all other errors*/
                           IF (@V_WAITCOUNTER > 5)
                              BEGIN
                                 IF (@exceptionidentifier IS NOT NULL)
                                    BEGIN
                                       IF @errornumber = 59998
                                          RAISERROR(59998, 16, 1, @exceptionidentifier)
                                       ELSE 
                                          RAISERROR(59999, 16, 1, @exceptionidentifier)
                                    END
                                 ELSE 
                                    BEGIN
                                       EXECUTE ssma_oracle.ssma_rethrowerror
                                    END
                              END

                           SET @V_WAITCOUNTER = @V_WAITCOUNTER + 1

                        END

                     END CATCH

                  END
                  /* exception handlers and block end here*/
               END

            SET @V_CURRENT = @V_CURRENT + 1

            UPDATE dbo.COUNTERLASTNUMBERS
               SET 
                  LASTNUMBER = @V_CURRENT
            WHERE COUNTERLASTNUMBERS.TABLENAME = @pTableName AND COUNTERLASTNUMBERS.TENANT = @pTenant

         END

      SET @pLastNumber = @V_CURRENT

   END
GO


-----------------------------------------
