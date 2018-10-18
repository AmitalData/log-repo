--2/10/2016

ALTER TABLE [dbo].[TenantManagements] ADD [RequestedAirlines] [varchar](250)
ALTER TABLE [dbo].[TenantManagements] ADD [RegisteredAirlines] [varchar](250)
ALTER TABLE [dbo].[TenantManagements] ADD [PendingAirlines] [varchar](250)

ALTER TABLE [dbo].[MobileNotificationLogs] ALTER COLUMN [NotificationMessage] [nvarchar](500) NOT NULL
ALTER TABLE [dbo].[MobileNotificationLogs] ALTER COLUMN [NotificationMessageIOS] [nvarchar](500) NULL
ALTER TABLE [dbo].[MobileNotificationLogs] ALTER COLUMN [NotificationMessageAndroid] [nvarchar](500) NULL
ALTER TABLE [dbo].[Settings] ADD [IsUpgradingChamp] [bit] NOT NULL DEFAULT 0


--------------------- 6/12/2016----------------------------------
-------------------------2016R5----------------------------------

ALTER TABLE [dbo].[TenantManagements] ALTER COLUMN [RequestedAirlines] [varchar](1000) NULL
ALTER TABLE [dbo].[TenantManagements] ALTER COLUMN [RegisteredAirlines] [varchar](1000) NULL
ALTER TABLE [dbo].[TenantManagements] ALTER COLUMN [PendingAirlines] [varchar](1000) NULL



ALTER TABLE [dbo].[TenantManagements] ADD [EnableBranding] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[TenantManagements] ADD [CustomerURL] [varchar](250)
ALTER TABLE [dbo].[TenantManagements] ADD [HideSharedlogistics] [bit] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[TenantManagements] ADD [ContactEmail] [varchar](70)

ALTER TABLE [dbo].[Settings] ADD [AndroidAppLink] [varchar](1000)
ALTER TABLE [dbo].[Settings] ADD [IOSAppLink] [varchar](1000)


-------------14/12/2016

ALTER TABLE [dbo].[Settings] ADD [AndroidPodAppMinimumVersion] [float] NOT NULL DEFAULT 0
ALTER TABLE [dbo].[Settings] ADD [IOSPodAppMinimumVersion] [float] NOT NULL DEFAULT 0




--24/1/2017

ALTER TABLE [dbo].[Settings] ADD [MinimumOutlookVersion] [nvarchar](max)

ALTER TABLE [dbo].[Settings] ALTER COLUMN [MinimumOutlookVersion] [varchar](10) NULL

ALTER TABLE [dbo].[Settings] ADD [SameUserLoginEnabled] [bit] NOT NULL DEFAULT 0

ALTER TABLE [dbo].[Settings] ADD [QBOConsumerKey] [varchar](100)
ALTER TABLE [dbo].[Settings] ADD [QBOConsumerSecretKey] [varchar](100)


-----------------------------------------------------------------------------------

--30/1/2017


-----------------------------------------------------------------------------------

--31/1/2017

----------------------------------------------------------------------------------

--1/2/2017

----------------------------------------------------------------------------------

--12/2/2017

-----------------------------------------------------------------------------------

--13/2/2017

ALTER TABLE [dbo].[Settings] ADD [QBOAppToken] [varchar](100)
