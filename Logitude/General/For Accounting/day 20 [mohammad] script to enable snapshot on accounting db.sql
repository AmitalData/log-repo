-------------------------------------------------------

ALTER DATABASE [Accounting_Logs]
SET ALLOW_SNAPSHOT_ISOLATION ON

ALTER DATABASE [Accounting_Logs]
SET ALLOW_SNAPSHOT_ISOLATION ON

use master
ALTER DATABASE [Accounting_Logs] SET SINGLE_USER WITH ROLLBACK IMMEDIATE 

--
ALTER DATABASE [Accounting_Logs]
SET READ_COMMITTED_SNAPSHOT ON
--

ALTER DATABASE [Accounting_Logs] SET MULTI_USER
----------------------------------------------------------------------------


-------------------------------------------------------

ALTER DATABASE [Accounting_Global]
SET ALLOW_SNAPSHOT_ISOLATION ON

ALTER DATABASE [Accounting_Global]
SET ALLOW_SNAPSHOT_ISOLATION ON

use master
ALTER DATABASE [Accounting_Global] SET SINGLE_USER WITH ROLLBACK IMMEDIATE 

--
ALTER DATABASE [Accounting_Global]
SET READ_COMMITTED_SNAPSHOT ON
--

ALTER DATABASE [Accounting_Global] SET MULTI_USER
----------------------------------------------------------------------------

-------------------------------------------------------

ALTER DATABASE [Accounting_Main]
SET ALLOW_SNAPSHOT_ISOLATION ON

ALTER DATABASE [Accounting_Main]
SET ALLOW_SNAPSHOT_ISOLATION ON

use master
ALTER DATABASE [Accounting_Main] SET SINGLE_USER WITH ROLLBACK IMMEDIATE 

--
ALTER DATABASE [Accounting_Main]
SET READ_COMMITTED_SNAPSHOT ON
--

ALTER DATABASE [Accounting_Main] SET MULTI_USER
----------------------------------------------------------------------------