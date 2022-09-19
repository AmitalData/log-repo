-- customs diffrent then main- mssql
USE [main]
GO

/****** Object:  StoredProcedure [dbo].[QUEUE_ENQUEUE]    Script Date: 15/09/2022 13:19:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[QUEUE_ENQUEUE]  
   @V_QUEUEDEFINITIONCODE /*-create or replace PROCEDURE  TSTQueue_Enqueue(*/varchar(max),
   @V_MESSAGEBODY varchar(max),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_TENANT float(53),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_DELAYSECONDS float(53),
   @V_CUSTOMERID varchar(max),
   @V_BATCHNUMBER varchar(max),
   @V_HASHCODE varchar(max),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @V_WATINGSTATUS float(53),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

   @P_TENANTPRIORITY float(53),
   @P_INTERFACETYPECODE varchar(max),
   /*
   *   SSMA warning messages:
   *   O2SS0356: Conversion from NUMBER datatype can cause data loss.
   */

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

CREATE OR ALTER PROCEDURE [dbo].[USP_GETNEXTTABLENUMBERVALUE]
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
