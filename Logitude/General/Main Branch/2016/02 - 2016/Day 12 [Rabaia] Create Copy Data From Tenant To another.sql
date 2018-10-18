SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ahmad Rabaia
-- Create date: 13/01/2016
-- Description:	Copy Package Type From Source Tenant To Destanation Tenant
-- =============================================
Create PROCEDURE [dbo].[usp_CopyDocTypes] 
@SourceTenant int,
@DestanationTenant int
AS
BEGIN
declare @Tenant as int
declare @DocTypeId as varchar(15)
declare @DocumentTypeDefaultHTMLTemplateId as varchar(15)
declare @DocumentTypeDefaultReportTemplateId as varchar(15)
declare @DocTypeCode as varchar(15) 
declare @DocumentTypeTemplatesHtmlId as varchar(15) 
declare @DocumentTypeTemplatesReportId as varchar(15) 
declare @NewDocTypeId as varchar(15) 
declare @DocTypeOTId as varchar(15)
declare @DocTypeCopyId as varchar(15)
declare @NewDocTypeCopyId as varchar(15) 
declare @DocTypeCustomFieldId as varchar(15)
declare @NewDocTypeCustomFieldId as varchar(15) 


BEGIN 
 
        print '1111'
		DECLARE eventsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Code,ObjectTableId,DocumentTypeDefaultHTMLTemplateId,DocumentTypeDefaultReportTemplateId
		FROM [dbo].[DocumentTypes] where Tenant = @SourceTenant
    	OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @DocTypeId,@DocTypeCode,@DocTypeOTId,@DocumentTypeDefaultHTMLTemplateId,@DocumentTypeDefaultReportTemplateId
		WHILE @@FETCH_STATUS = 0
			BEGIN 
			
			IF NOT EXISTS ( SELECT [Code] FROM [dbo].[DocumentTypes] where [Code] = @DocTypeCode and [Tenant] = @DestanationTenant)
			   begin
               print '1111'
			    set @NewDocTypeId = null 
			    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewDocTypeId OUTPUT,
		                @pTableName = N'DocumentType' 
                print '@NewDocTypeId: ' + @NewDocTypeId
		         
           declare @ObjectTableName as varchar(100)
           declare @ObjectTableId as varchar(15)  
           SELECT @ObjectTableName = [Name]
               FROM [dbo].[ObjectTables] where Id = @DocTypeOTId and Tenant = @SourceTenant
           SELECT @ObjectTableId = [Id]
               FROM [dbo].[ObjectTables] where Name = @ObjectTableName and Tenant = @DestanationTenant
                  
           INSERT INTO [dbo].[DocumentTypes]
           ([Id],[Tenant],[Name],[Code],[Notes],[IsAir],[IsOcean],[IsInland],[IsDocIn],[IsDocOut],[ObjectTableId],[Subject],[DocumentTypeDefaultReportTemplateId]
           ,[DocumentTypeDefaultHTMLTemplateId],[TemplateFormatCode],[DocumentTypeDefaultEditorTool],[InActive],[IsMaster],[IsHouse],[IsDirect],[SearchFields]
           ,[CustomControl],[CustomerRoleId],[AgentRoleId],[IsCustomerView],[IsAgentView],[IsReadOnly],[IsDocumentOneTimePrintLimited],[LimitedPrintCopyId]
           ,[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[DocumentsDataProviderCode],[DocumentTypeCategoryCode])
           ( select @NewDocTypeId,@DestanationTenant,[Name],[Code],[Notes],[IsAir],[IsOcean],[IsInland],[IsDocIn],[IsDocOut],@ObjectTableId,[Subject],null
           ,null,[TemplateFormatCode],[DocumentTypeDefaultEditorTool],[InActive],[IsMaster],[IsHouse],[IsDirect],[SearchFields]
           ,[CustomControl],[CustomerRoleId],[AgentRoleId],[IsCustomerView],[IsAgentView],[IsReadOnly],[IsDocumentOneTimePrintLimited],[LimitedPrintCopyId]
           ,[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[DocumentsDataProviderCode],[DocumentTypeCategoryCode]
             from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
             
              ----------------------------------------------------------------------------
		   if @DocumentTypeDefaultHTMLTemplateId is not null
		   begin       
		   print '2'
                set @DocumentTypeTemplatesHtmlId = null 
                EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @DocumentTypeTemplatesHtmlId OUTPUT,
		                @pTableName = N'DocumentTypeTemplate'  
		                
            INSERT INTO [dbo].[DocumentTypeTemplates]
           ([Id],[Tenant],[DocumentTypeId],[TemplateBody],[TemplateType],[Description],[LastUpdateDate],[LastUpdatedByUserId] ,[InActive]
           ,[EditorTool],[VerticalShift],[HorizontalShift],[Subject],[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[InternalRemarks]
           ,[Language],[OriginalTemplateId])
           ( select @DocumentTypeTemplatesHtmlId,@DestanationTenant,@NewDocTypeId,[TemplateBody],[TemplateType],[Description],[LastUpdateDate],[LastUpdatedByUserId],[InActive]
           ,[EditorTool],[VerticalShift],[HorizontalShift],[Subject],[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[InternalRemarks]
           ,[Language],[OriginalTemplateId]
              from [dbo].[DocumentTypeTemplates] where Id = @DocumentTypeDefaultHTMLTemplateId and Tenant = @SourceTenant)
           end
           ------------------------------------------------------------------------------      
           
           ------------------------------------------------------------------------------
           if @DocumentTypeDefaultReportTemplateId is not null
		   begin 
		           
                set @DocumentTypeTemplatesReportId = null 
                EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @DocumentTypeTemplatesReportId OUTPUT,
		                @pTableName = N'DocumentTypeTemplate'  
		    print '@DocumentTypeTemplatesReportId: ' + @DocumentTypeTemplatesReportId       
            INSERT INTO [dbo].[DocumentTypeTemplates]
           ([Id],[Tenant],[DocumentTypeId],[TemplateBody],[TemplateType],[Description],[LastUpdateDate],[LastUpdatedByUserId] ,[InActive]
           ,[EditorTool],[VerticalShift],[HorizontalShift],[Subject],[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[InternalRemarks]
           ,[Language],[OriginalTemplateId])
           ( select @DocumentTypeTemplatesReportId,@DestanationTenant,@NewDocTypeId,[TemplateBody],[TemplateType],[Description],[LastUpdateDate],[LastUpdatedByUserId],[InActive]
           ,[EditorTool],[VerticalShift],[HorizontalShift],[Subject],[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[InternalRemarks]
           ,[Language],[OriginalTemplateId]
              from [dbo].[DocumentTypeTemplates] where Id = @DocumentTypeDefaultReportTemplateId and Tenant = @SourceTenant)
          end
           ------------------------------------------------------------------------------
           
           update [dbo].[DocumentTypes] set [DocumentTypeDefaultHTMLTemplateId] = @DocumentTypeTemplatesHtmlId,[DocumentTypeDefaultReportTemplateId] = @DocumentTypeTemplatesReportId
           
           ------------------------------------------------------------------------------
           
           DECLARE eventsCursor1 CURSOR READ_ONLY LOCAL
		   FOR
		   SELECT Id
		   FROM [dbo].[DocumentTypeCopies] where DocumentTypeId = @DocTypeId and Tenant = @SourceTenant
    	   OPEN eventsCursor1 FETCH NEXT FROM eventsCursor1 INTO @DocTypeCopyId
		   WHILE @@FETCH_STATUS = 0
			 BEGIN 
			 
			    set @NewDocTypeCopyId = null 
			    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewDocTypeCopyId OUTPUT,
		                @pTableName = N'DocumentTypeCopy' 
		        print '@NewDocTypeCopyId : ' + @NewDocTypeCopyId
		        INSERT INTO [dbo].[DocumentTypeCopies]
                ([Id],[Tenant],[Code],[Name],[DocumentTypeId],[IndexOrder],[IsSelectedByDefault],[InActive]) 
                (select @NewDocTypeCopyId,@DestanationTenant,[Code],[Name],[DocumentTypeId],[IndexOrder],[IsSelectedByDefault],[InActive]
                 from [dbo].[DocumentTypeCopies] where Id = @DocTypeCopyId and Tenant = @SourceTenant)    
                 
		        FETCH NEXT FROM eventsCursor1 INTO @DocTypeCopyId
			 END
			CLOSE eventsCursor1;
	        DEALLOCATE eventsCursor1;
		   -----------------------------------------------------------------------------
			 
			DECLARE eventsCursor2 CURSOR READ_ONLY LOCAL
		   FOR
		   SELECT Id
		   FROM [dbo].[DocumentTypeCustomFields1] where DocumentTypeId = @DocTypeId and Tenant = @SourceTenant
    	   OPEN eventsCursor2 FETCH NEXT FROM eventsCursor2 INTO @DocTypeCustomFieldId
		   WHILE @@FETCH_STATUS = 0
			 BEGIN 
			 print '6'
			    set @NewDocTypeCustomFieldId = null 
			    EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @NewDocTypeCustomFieldId OUTPUT,
		                @pTableName = N'DocumentTypeCustomField' 
		                
		       INSERT INTO [dbo].[DocumentTypeCustomFields1]
               ([Id],[Tenant],[DocumentTypeId],[FieldCode],[FieldDataTypeCode],[InActive],[IsRequired],[DefaultValue],[MultiLine],[Name],[IndexOrder]) 
                (select @NewDocTypeCustomFieldId,@DestanationTenant,[DocumentTypeId],[FieldCode],[FieldDataTypeCode],[InActive],[IsRequired],[DefaultValue],[MultiLine],[Name],[IndexOrder]
                 from [dbo].[DocumentTypeCustomFields1] where Id = @DocTypeCustomFieldId and Tenant = @SourceTenant)    
			 FETCH NEXT FROM eventsCursor2 INTO @DocTypeCustomFieldId
			 END
			 CLOSE eventsCursor2;
	         DEALLOCATE eventsCursor2;
               end -- First If 
              
              else
              begin
              print 'Doc ID' + @DocTypeId
              declare @xx as varchar(200) 
              set @xx = (select [Notes] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
              print @xx
              --print (select [Notes] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
              UPDATE [dbo].[DocumentTypes] SET
            [Name] = (select [Name] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[Code] = (select [Code] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[Notes]= (select [Notes] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsAir] = (select [IsAir] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsOcean] = (select [IsOcean] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsInland] = (select [IsInland] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsDocIn] = (select [IsDocIn] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsDocOut] = (select [IsDocOut] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[ObjectTableId] = (select [ObjectTableId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[Subject] = (select [Subject] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           --,[DocumentTypeDefaultReportTemplateId] = (select [DocumentTypeDefaultReportTemplateId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           --,[DocumentTypeDefaultHTMLTemplateId] = (select [DocumentTypeDefaultHTMLTemplateId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[TemplateFormatCode] = (select [TemplateFormatCode] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[DocumentTypeDefaultEditorTool] = (select [DocumentTypeDefaultEditorTool] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[InActive] = (select [InActive] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsMaster] = (select [IsMaster] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsHouse] = (select [IsHouse] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsDirect] = (select [IsDirect] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[SearchFields] = (select [SearchFields] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[CustomControl] = (select [CustomControl] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           --,[CustomerRoleId] = (select [CustomerRoleId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           --,[AgentRoleId] = (select [AgentRoleId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsCustomerView] = (select [IsCustomerView] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsAgentView] = (select [IsAgentView] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsReadOnly] = (select [IsReadOnly] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsDocumentOneTimePrintLimited] = (select [IsDocumentOneTimePrintLimited] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[LimitedPrintCopyId] = (select [LimitedPrintCopyId] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsEnabledForCustomers] = (select [IsEnabledForCustomers] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[IsCopiedAtSignup] = (select [IsCopiedAtSignup] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[CountryCode] = (select [CountryCode] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[DocumentsDataProviderCode] = (select [DocumentsDataProviderCode] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           ,[DocumentTypeCategoryCode] = (select [DocumentTypeCategoryCode] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant)
           where Code = @DocTypeCode and Tenant = @DestanationTenant
           --(select [Name] from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant),[Code],[Notes],[IsAir],[IsOcean],[IsInland],[IsDocIn],[IsDocOut],@ObjectTableId,[Subject],null
           --,null,[TemplateFormatCode],[DocumentTypeDefaultEditorTool],[InActive],[IsMaster],[IsHouse],[IsDirect],[SearchFields]
           --,[CustomControl],[CustomerRoleId],[AgentRoleId],[IsCustomerView],[IsAgentView],[IsReadOnly],[IsDocumentOneTimePrintLimited],[LimitedPrintCopyId]
          -- ,[IsEnabledForCustomers],[IsCopiedAtSignup],[CountryCode],[DocumentsDataProviderCode],[DocumentTypeCategoryCode]
           --  from [dbo].[DocumentTypes] where Id = @DocTypeId and Tenant = @SourceTenant
              end -- else 
		   FETCH NEXT FROM eventsCursor INTO @DocTypeId,@DocTypeCode,@DocTypeOTId,@DocumentTypeDefaultHTMLTemplateId,@DocumentTypeDefaultReportTemplateId			
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END
END

-- ======================================================================================

 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ahmad Rabaia
-- Create date: 14/02/2016
-- Description:	Copy DocMetaDataTypes From Source Tenant To Destanation Tenant
-- =============================================
Create PROCEDURE [dbo].[usp_CopyDocumentsMetaDataTypes] 
@SourceTenant int,
@DestanationTenant int
AS
BEGIN
declare @Tenant as int
declare @Id as varchar(15)
declare @Code as varchar(15)
declare @MeasurementId as varchar(15)
declare @SourceCode as varchar(15) 
declare @DestId as varchar(15)  

BEGIN 
		DECLARE eventsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Code
		FROM dbo.DocumentsMetaDataTypes where Tenant = @SourceTenant
    	OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@Code
		WHILE @@FETCH_STATUS = 0
			BEGIN 
			               
               set @Code = null
	           SELECT @Code = Code 
               FROM [dbo].[DocumentsMetaDataTypes] where [Code] = @Code and Tenant = @DestanationTenant
               print @Code
               IF @Code = '' or @Code is null 
               begin
               print 'innnnnn'
                 EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @DestId OUTPUT,
		                @pTableName = N'DocumentsMetaDataType'          
		       print '@DestId : ' + @DestId        
                  INSERT INTO [dbo].[DocumentsMetaDataTypes]([Id],[Tenant],[Code],[EnglishName],[LocalName],[InActive],[CustomsMetaDataCode],[Format])
                  ( select @DestId,@DestanationTenant,[Code],[EnglishName],[LocalName],[InActive],[CustomsMetaDataCode],[Format]
                    from [dbo].[DocumentsMetaDataTypes] where Id = @Id and Tenant = @SourceTenant)
               
               end
               
		   FETCH NEXT FROM eventsCursor INTO @Id,@Code			
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END
END

-- =================================================================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ahmad Rabaia
-- Create date: 27/12/2015
-- Description:	Copy Package Type From Source Tenant To Destanation Tenant
-- =============================================
Create PROCEDURE [dbo].[usp_CopyPackageTypes] 
@SourceTenant int,
@DestanationTenant int
AS
BEGIN
	 declare @Tenant as int
declare @Id as varchar(15)
declare @MeasurementId as varchar(15)
declare @SourceCode as varchar(15) 
declare @DestId as varchar(15)  

BEGIN 
 
		DECLARE eventsCursor CURSOR READ_ONLY
		FOR
		SELECT Id,MeasurementId
		FROM [dbo].[PackageTypes] where Tenant = @SourceTenant
    	OPEN eventsCursor FETCH NEXT FROM eventsCursor INTO @Id,@MeasurementId
		WHILE @@FETCH_STATUS = 0
			BEGIN 
			   set @SourceCode = null
			   SELECT @SourceCode = [Code] 
               FROM [dbo].[Measurements] where Id = @MeasurementId
               
               set @DestId = null
	           SELECT @DestId = Id 
               FROM [dbo].[Measurements] where [Code] = @SourceCode and Tenant = @DestanationTenant
    
               IF @DestId = '' or @DestId is null and @SourceCode is not null
               begin
               
                 EXEC   [dbo].[usp_GetNextTableIdValue]
		                @pLastNumber = @DestId OUTPUT,
		                @pTableName = N'Measurement'          
		                   
                  INSERT INTO [dbo].[Measurements]([Code] ,[Name],[ShortName],[Id],[Tenant],[IsContainerMeasurement],[IsContainer],[InActive],[SearchFields],[LocalName])
                  ( select [Code],[Name],[ShortName],@DestId,@DestanationTenant,[IsContainerMeasurement],[IsContainer],[InActive],[SearchFields],[LocalName]
                    from [dbo].[Measurements] where Id = @MeasurementId and Tenant = @SourceTenant)
            
               end
              
               IF @SourceCode is null
               begin
                set @DestId = null
               end
               
             declare @PackageTypeId as varchar(15)
             declare @DestTypeCode as varchar(100)
             declare @DestTypeId as varchar(15)
               
            SELECT @DestTypeCode = [Code] 
               FROM [dbo].[PackageTypes] where Id = @Id
     
            SELECT @DestTypeId = Id 
               FROM [dbo].[PackageTypes] where [Code] = @DestTypeCode and Tenant = @DestanationTenant
               
            IF @DestTypeId = '' or @DestTypeId is null
                 begin
                  
                     EXEC   [dbo].[usp_GetNextTableIdValue]
		                    @pLastNumber = @PackageTypeId OUTPUT,
		                    @pTableName = N'PackageType'                  
            
                     INSERT INTO [dbo].[PackageTypes]
           ([Id],[Tenant],[Code],[EnglishName],[LocalName],[IsAir],[IsOcean],[IsInland],[AddedManually],[Notes],[IsContainer],[TEU],[ContainerSize],[Volume],[InActive]
           ,[MeasurementId],[SearchFields],[PrintAs])
           (SELECT @PackageTypeId,@DestanationTenant,Code,EnglishName,LocalName,IsAir,IsOcean,IsInland,AddedManually,Notes,IsContainer,TEU,ContainerSize,Volume,InActive
           ,@DestId,SearchFields,PrintAs
           from [PackageTypes] where Id = @Id and Tenant = @SourceTenant)
          
                 end
           
		   FETCH NEXT FROM eventsCursor INTO @Id,@MeasurementId				
			END
		CLOSE eventsCursor
		DEALLOCATE eventsCursor
END
END
