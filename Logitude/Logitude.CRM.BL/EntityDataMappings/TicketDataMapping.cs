
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.BL.Helpers;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class TicketDataMapping: IMapping<TicketPM, Ticket>
   {
        public void CustomPMToPOCO(TicketPM entityPM, Ticket entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
            entityPOCO.TicketNumber = entityPM.TicketNumber;

            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field1);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field2);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field3);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field4);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field5);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field6);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field7);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field8);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field9);
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Field10);

            //entityPOCO.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;
            //entityPOCO.Field2 = entityPM.Field2 != null ? entityPM.Field2.Value : null;
            //entityPOCO.Field3 = entityPM.Field3 != null ? entityPM.Field3.Value : null;
            //entityPOCO.Field4 = entityPM.Field4 != null ? entityPM.Field4.Value : null;
            //entityPOCO.Field5 = entityPM.Field5 != null ? entityPM.Field5.Value : null;
            //entityPOCO.Field6 = entityPM.Field6 != null ? entityPM.Field6.Value : null;
            //entityPOCO.Field7 = entityPM.Field7 != null ? entityPM.Field7.Value : null;
            //entityPOCO.Field8 = entityPM.Field8 != null ? entityPM.Field8.Value : null;
            //entityPOCO.Field9 = entityPM.Field9 != null ? entityPM.Field9.Value : null;
            //entityPOCO.Field10 = entityPM.Field10 != null ? entityPM.Field10.Value : null;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
      
        }

        public void CustomPOCOToPM(TicketPM entityPM, Ticket entityPOCO)
        {
            //entityPM.Field1 = new CustomFieldClass("Field1", "Ticket", entityPOCO.Field1);
            //entityPM.Field2 = new CustomFieldClass("Field2", "Ticket", entityPOCO.Field2);
            //entityPM.Field3 = new CustomFieldClass("Field3", "Ticket", entityPOCO.Field3);
            //entityPM.Field4 = new CustomFieldClass("Field4", "Ticket", entityPOCO.Field4);
            //entityPM.Field5 = new CustomFieldClass("Field5", "Ticket", entityPOCO.Field5);
            //entityPM.Field6 = new CustomFieldClass("Field6", "Ticket", entityPOCO.Field6);
            //entityPM.Field7 = new CustomFieldClass("Field7", "Ticket", entityPOCO.Field7);
            //entityPM.Field8 = new CustomFieldClass("Field8", "Ticket", entityPOCO.Field8);
            //entityPM.Field9 = new CustomFieldClass("Field9", "Ticket", entityPOCO.Field9);
            //entityPM.Field10 = new CustomFieldClass("Field10", "Ticket", entityPOCO.Field10);

            this.CustomMappedPMProperties.Add(PMPropertyNames.OwnerName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CompanyName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.SeverityName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.SeverityCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.StageName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.StageCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.MainClassificationName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.SecondaryClassificationName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactPhone);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactEmail);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByContactName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UpdatedByUserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TicketHeader);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TicketFooter);
            this.CustomMappedPMProperties.Add(PMPropertyNames.SourceName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedbyTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.OwnerEmail);

            #region Data Fields

            entityPM.TicketHeader = @"<t:RadDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:t='clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents' xmlns:s='clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents' xmlns:r='clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Documents' xmlns:n='clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Documents' xmlns:th='clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Documents' version='1.2' LayoutMode='Flow' LineSpacing='1.14999997615814' LineSpacingType='Auto' ParagraphDefaultSpacingAfter='10' ParagraphDefaultSpacingBefore='0' SectionDefaultPageMargin='95,95,95,95' SelectedBibliographicStyleName='\APA.XSL' StyleName='defaultDocumentStyle'>
                                  <t:RadDocument.Captions>
                                    <t:CaptionDefinition IsDefault='True' IsLinkedToHeading='False' Label='Figure' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen' />
                                    <t:CaptionDefinition IsDefault='True' IsLinkedToHeading='False' Label='Table' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen' />
                                  </t:RadDocument.Captions>
                                  <t:RadDocument.ProtectionSettings>
                                    <t:DocumentProtectionSettings EnableDocumentProtection='False' Enforce='False' HashingAlgorithm='None' HashingSpinCount='0' ProtectionMode='ReadOnly' />
                                  </t:RadDocument.ProtectionSettings>
                                  <t:RadDocument.Styles>
                                    <s:StyleDefinition DisplayName='Document Default Style' IsCustom='False' IsDefault='False' IsPrimary='True' Name='defaultDocumentStyle' Type='Default'>
                                      <s:StyleDefinition.ParagraphStyle>
                                        <s:ParagraphProperties LineSpacing='1.14999997615814' SpacingAfter='10' />
                                      </s:StyleDefinition.ParagraphStyle>
                                      <s:StyleDefinition.SpanStyle>
                                        <s:SpanProperties FontFamily='Verdana' FontSize='16' FontStyle='Normal' FontWeight='Normal' />
                                      </s:StyleDefinition.SpanStyle>
                                    </s:StyleDefinition>
                                    <s:StyleDefinition DisplayName='Normal' IsCustom='False' IsDefault='True' IsPrimary='True' Name='Normal' Type='Paragraph' UIPriority='0' />
                                    <s:StyleDefinition BasedOnName='TableNormal' DisplayName='Table Grid' IsCustom='False' IsDefault='False' IsPrimary='False' Name='TableGrid' Type='Table' UIPriority='59'>
                                      <s:StyleDefinition.ParagraphStyle>
                                        <s:ParagraphProperties LineSpacing='1' SpacingAfter='0' />
                                      </s:StyleDefinition.ParagraphStyle>
                                      <s:StyleDefinition.TableStyle>
                                        <s:TableProperties Borders='1,Single,#FF000000,none,,'>
                                          <s:TableProperties.TableLook>
                                            <t:TableLook />
                                          </s:TableProperties.TableLook>
                                        </s:TableProperties>
                                      </s:StyleDefinition.TableStyle>
                                    </s:StyleDefinition>
                                    <s:StyleDefinition DisplayName='Table Normal' IsCustom='False' IsDefault='True' IsPrimary='False' Name='TableNormal' Type='Table' UIPriority='59'>
                                      <s:StyleDefinition.TableStyle>
                                        <s:TableProperties CellPadding='5,0,5,0'>
                                          <s:TableProperties.TableLook>
                                            <t:TableLook />
                                          </s:TableProperties.TableLook>
                                        </s:TableProperties>
                                      </s:StyleDefinition.TableStyle>
                                    </s:StyleDefinition>
                                  </t:RadDocument.Styles>
                                  <t:Section>
                                    <t:Paragraph />
                                    <t:Paragraph TextAlignment='Center'>
                                      <t:Span FontFamily='Lucida Sans Unicode' FontSize='9' ForeColor='#FF548ED5' Text='#Please type your reply above this line#' />
                                    </t:Paragraph>
                                    <t:Table Borders='1,None,#FF000000,none,,' GridColumnWidthsSerializationInfo='' HasFixedStructure='False' LayoutMode='AutoFit' PreferredWidth='Auto' StyleName='TableGrid' TableIndent='0'>
                                      <t:Table.TableLook>
                                        <t:TableLook />
                                      </t:Table.TableLook>
                                      <t:TableRow Height='73.4833221435547'>
                                        <t:TableCell Background='#FF98D0DE' Borders='0,Inherit,#FF000000,none,,' ColumnSpan='10' Padding='5,0,5,0' RowSpan='1' TextAlignment='Center' VerticalAlignment='Center'>
                                          <t:Paragraph Background='#FF98D0DE' TextAlignment='Center'>
                                            <t:Span FontFamily='Lucida Sans Unicode' FontSize='11' Text='Ticket # [TicketNumber]([StageName]): [Subject]'/>
                                          </t:Paragraph>
                                        </t:TableCell>
                                      </t:TableRow>
                                    </t:Table>
                                    <t:Paragraph />
                                  </t:Section>
                                </t:RadDocument>";

            string emailFooterMessage = SetEmailFooterMessage();

            entityPM.TicketFooter = @"<t:RadDocument xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:t='clr-namespace:Telerik.Windows.Documents.Model;assembly=Telerik.Windows.Documents' xmlns:s='clr-namespace:Telerik.Windows.Documents.Model.Styles;assembly=Telerik.Windows.Documents' xmlns:r='clr-namespace:Telerik.Windows.Documents.Model.Revisions;assembly=Telerik.Windows.Documents' xmlns:n='clr-namespace:Telerik.Windows.Documents.Model.Notes;assembly=Telerik.Windows.Documents' xmlns:th='clr-namespace:Telerik.Windows.Documents.Model.Themes;assembly=Telerik.Windows.Documents' version='1.2' LayoutMode='Flow' LineSpacing='1.15' LineSpacingType='Auto' ParagraphDefaultSpacingAfter='12' ParagraphDefaultSpacingBefore='0' SelectedBibliographicStyleName='\APA.XSL' StyleName='defaultDocumentStyle'>
                              <t:RadDocument.Captions>
                                <t:CaptionDefinition IsDefault='True' IsLinkedToHeading='False' Label='Figure' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen'/>
                                <t:CaptionDefinition IsDefault='True' IsLinkedToHeading='False' Label='Table' LinkedHeadingLevel='0' NumberingFormat='Arabic' SeparatorType='Hyphen'/>
                              </t:RadDocument.Captions>
                              <t:RadDocument.ProtectionSettings>
                                <t:DocumentProtectionSettings EnableDocumentProtection='False' Enforce='False' HashingAlgorithm='None' HashingSpinCount='0' ProtectionMode='ReadOnly'/>
                              </t:RadDocument.ProtectionSettings>
                              <t:RadDocument.Styles>
                                <s:StyleDefinition DisplayName='Document Default Style' IsCustom='False' IsDefault='False' IsPrimary='True' Name='defaultDocumentStyle' Type='Default'>
                                  <s:StyleDefinition.ParagraphStyle>
                                    <s:ParagraphProperties LineSpacing='1.15' SpacingAfter='12'/>
                                  </s:StyleDefinition.ParagraphStyle>
                                  <s:StyleDefinition.SpanStyle>
                                    <s:SpanProperties FontFamily='Verdana' FontSize='16' FontStyle='Normal' FontWeight='Normal'/>
                                  </s:StyleDefinition.SpanStyle>
                                </s:StyleDefinition>
                                <s:StyleDefinition DisplayName='Normal' IsCustom='False' IsDefault='True' IsPrimary='True' Name='Normal' Type='Paragraph' UIPriority='0'/>
                                <s:StyleDefinition BasedOnName='TableNormal' DisplayName='Table Grid' IsCustom='False' IsDefault='False' IsPrimary='False' Name='TableGrid' Type='Table' UIPriority='59'>
                                  <s:StyleDefinition.ParagraphStyle>
                                    <s:ParagraphProperties LineSpacing='1' SpacingAfter='0'/>
                                  </s:StyleDefinition.ParagraphStyle>
                                  <s:StyleDefinition.TableStyle>
                                    <s:TableProperties Borders='1,Single,#FF000000,none,,'>
                                      <s:TableProperties.TableLook>
                                        <t:TableLook/>
                                      </s:TableProperties.TableLook>
                                    </s:TableProperties>
                                  </s:StyleDefinition.TableStyle>
                                </s:StyleDefinition>
                                <s:StyleDefinition DisplayName='Table Normal' IsCustom='False' IsDefault='True' IsPrimary='False' Name='TableNormal' Type='Table' UIPriority='59'>
                                  <s:StyleDefinition.TableStyle>
                                    <s:TableProperties CellPadding='5,0,5,0'>
                                      <s:TableProperties.TableLook>
                                        <t:TableLook/>
                                      </s:TableProperties.TableLook>
                                    </s:TableProperties>
                                  </s:StyleDefinition.TableStyle>
                                </s:StyleDefinition>
                              </t:RadDocument.Styles>
                              <t:Section>
                                <t:Paragraph TextAlignment='Center'/>
                                <t:Table Borders='1,None,#FF000000,none,,' GridColumnWidthsSerializationInfo='' HasFixedStructure='False' LayoutMode='AutoFit' PreferredWidth='Auto' StyleName='TableGrid' TableIndent='0'>
                                  <t:Table.TableLook>
                                    <t:TableLook/>
                                  </t:Table.TableLook>
                                  <t:TableRow Height='20.4833221435547'>
                                    <t:TableCell Borders='0,Inherit,#FF000000,none,,' ColumnSpan='10' RowSpan='1' TextAlignment='Center' VerticalAlignment='Center'>
                                      <t:Paragraph Background='#FF98D0DE' TextAlignment='Center'>
                                        <t:Span FontFamily='Lucida Sans Unicode' FontSize='11' Text='" + emailFooterMessage + @"'/>
                                      </t:Paragraph>
                                    </t:TableCell>
                                  </t:TableRow>
                                </t:Table>
                                <t:Paragraph/>
                              </t:Section>
                            </t:RadDocument>";

            entityPM.TicketReplyto = this.GetReplyToEmail(entityPOCO.Tenant, entityPOCO.GuidId);
            entityPM.EntityNumber = entityPM.ShipmentNumber != null ? entityPM.ShipmentNumber : entityPM.QuoteNumber;


            // Automation Fields
            ICRMContext context = CRMContext.GetContext(entityPM.Tenant);
            TicketClassificationPM classification = new TicketClassificationPM();
            TicketClassificationQueryService classificationQuery = new TicketClassificationQueryService(context);
            SLAHeaderQueryService sLAHeaderQuery = new SLAHeaderQueryService(context);
            EmployeeGroupQueryService employeeGroupQuery = new EmployeeGroupQueryService(context);
            TicketClassificationRepository repClassification = new TicketClassificationRepository(entityPOCO.Tenant);

            if (!string.IsNullOrEmpty(entityPM.SecondaryClassificationId))
            {
                classification = classificationQuery.GetSingle(entityPM.SecondaryClassificationId, true, false);
            }
            else
            {
                classification = classificationQuery.GetSingle(entityPM.MainClassificationId, true, false);
            }
            entityPM.ClassificationManager = classification.ManagerUserEmail;
            entityPM.ClassificationNotify = classification.EscalationNotify;

            EmployeeGroupPM groupManager = employeeGroupQuery.GetSingle(entityPM.EmployeeGroupId, true, false);
            if (groupManager != null)
            {
                entityPM.GroupManager = groupManager.ManagerUserEmail;
            }

            var employeeGroupPM = employeeGroupQuery.GetSingle(entityPM.EmployeeGroupId, true, false);
            if (employeeGroupPM != null)
            {
                entityPM.GroupNotify = employeeGroupPM.EscalationNotify;
                entityPM.EmployeeGroupName = employeeGroupPM.Name;
            }
            #endregion 

            if (!string.IsNullOrEmpty(entityPOCO.OwnerId))
            {
                UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
                User user = userRepository.GetSingleUser(entityPOCO.OwnerId, entityPOCO.Tenant, false);
                if (user != null)
                {
                    entityPM.OwnerName = user.Contact.EnglishName;
                    entityPM.OwnerEmail = user.Contact.Email;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.UpdatedByUserId))
            {
                UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
                User user = userRepository.GetSingleUser(entityPOCO.UpdatedByUserId, entityPOCO.Tenant, false);
                if (user != null)
                {
                    entityPM.UpdatedByUserName = user.Contact.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.CompanyId))
            {
                CardRepository rep = new CardRepository(entityPOCO.Tenant);
                Card myCard = rep.GetSingleCard(entityPOCO.CompanyId, entityPOCO.Tenant);
                if (myCard != null)
                {
                    entityPM.CompanyName = myCard.EnglishName;
                    entityPM.CompanyTableName = MethodHelper.ConvertPartnerTypeToTableName(myCard.PartnerTypeId);

                    if (myCard.PartnerTypeId == "CS" || myCard.PartnerTypeId == "PO")
                    {
                        CustomerRepository myRepository = new CustomerRepository(entityPOCO.Tenant);
                        Customer myCustomer = myRepository.GetSingleCustomer(myCard.Id, entityPOCO.Tenant, true);
                        if (myCustomer != null)
                        {
                            //entityPM.CompanyRankCode = myCustomer.Rank == null ? null : myCustomer.Rank.Code;

                            RankRepository rankRepository = new RankRepository(entityPOCO.Tenant);
                            Rank rank = rankRepository.GetSingleRank(myCustomer.RankId, entityPOCO.Tenant);
                            if (rank != null)
                            {
                                entityPM.RankCode = rank.Code;
                                entityPM.RankName = rank.Name;
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.StageId))
            {
                TicketStageRepository repStage = new TicketStageRepository(entityPOCO.Tenant);
                TicketStage stage = repStage.GetSingle(entityPOCO.StageId, entityPOCO.Tenant);
                if (stage != null)
                {
                    entityPM.StageName = stage.Name;
                    entityPM.StageCode = stage.Code;
                }
            }

            TicketTypeRepository repType = new TicketTypeRepository(entityPOCO.Tenant);
            TicketType type = repType.GetSingle(entityPOCO.TicketTypeId, entityPOCO.Tenant);
            if (type != null)
            {
                entityPM.TypeName = type.Name;
            }

            if (!string.IsNullOrEmpty(entityPOCO.SeverityId))
            {
                TicketSeverityRepository repSeverity = new TicketSeverityRepository(entityPOCO.Tenant);
                TicketSeverity severity = repSeverity.GetSingle(entityPOCO.SeverityId, entityPOCO.Tenant);
                if (severity != null)
                {
                    entityPM.SeverityName = severity.Name;
                    entityPM.SeverityCode = severity.Code;

                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.MainClassificationId))
            {
                TicketClassification classificationPOCO = repClassification.GetSingle(entityPOCO.MainClassificationId, entityPOCO.Tenant);
                if (classification != null)
                {
                    entityPM.MainClassificationName = classification.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.SecondaryClassificationId))
            {
                TicketClassification classificationPOCO = repClassification.GetSingle(entityPOCO.SecondaryClassificationId, entityPOCO.Tenant);
                if (classification != null)
                {
                    entityPM.SecondaryClassificationName = classification.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.ContactId))
            {
                ContactRepository rep = new ContactRepository(entityPOCO.Tenant);
                Contact contact = rep.GetSingleContact(entityPOCO.ContactId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.ContactName = contact.EnglishName;
                    entityPM.ContactPhone = contact.BusinessPhone;
                    entityPM.ContactEmail = contact.Email;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.CreatedByContactId))
            {
                ContactRepository rep = new ContactRepository(entityPOCO.Tenant);
                Contact contact = rep.GetSingleContact(entityPOCO.CreatedByContactId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.CreatedByContactName = contact.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.LastCompletedActivityTypeCode))
            {
                ActivityTypeRepository typeRepository = new ActivityTypeRepository(entityPOCO.Tenant);
                ActivityType activitytype = typeRepository.GetSingle(entityPOCO.LastCompletedActivityTypeCode);
                if (activitytype != null)
                {
                    entityPM.LastCompletedActivityTypeName = activitytype.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.NextActivityTypeCode))
            {
                ActivityTypeRepository typeRepository = new ActivityTypeRepository(entityPOCO.Tenant);
                ActivityType activitytype = typeRepository.GetSingle(entityPOCO.NextActivityTypeCode);
                if (activitytype != null)
                {
                    entityPM.NextActivityTypeName = activitytype.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.Source))
            {
                TicketSourceRepository sourceRepository = new TicketSourceRepository(entityPOCO.Tenant);
                TicketSource source = sourceRepository.GetSingle(entityPOCO.Source);
                if (source != null)
                {
                    entityPM.SourceName = source.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.CreatedbyType))
            {
                TicketCreatedByTypeRepository typeRepository = new TicketCreatedByTypeRepository(entityPOCO.Tenant);
                TicketCreatedByType createdByType = typeRepository.GetSingle(entityPOCO.CreatedbyType);
                if (createdByType != null)
                {
                    entityPM.CreatedbyTypeName = createdByType.Name;
                }
            }
        }

        private string SetEmailFooterMessage()
        {
            var emailFooterMessage = "";
            var logitudeFooterMessage = "This email is a service from Logitude!";
            var cloudFooterMessage = "This email is a service from Unifreight Cloud Generation!";

            if (LogitudeSettings.DeploymentStage == "Simplog")
                emailFooterMessage = logitudeFooterMessage;
            else
                emailFooterMessage = cloudFooterMessage;

            return emailFooterMessage;
        }

        private void BuildSearchFields(TicketPM entityPM, Ticket entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.TicketNumber))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.TicketNumber);
            }

            if (!string.IsNullOrEmpty(entityPM.Subject))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Subject);
            }

            if (!string.IsNullOrEmpty(entityPM.ContactId))
            {
                Contact myContact = ContactRepository.GetSingleContact(entityPM.ContactId, entityPM.Tenant, true);
                if (myContact != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myContact.Email);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myContact.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.CompanyId))
            {
                CardRepository rep = new CardRepository(entityPOCO.Tenant);
                Card myCard = rep.GetSingleCard(entityPOCO.CompanyId, entityPOCO.Tenant);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ShipmentNumber))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipmentNumber);
            }

            mySearchFields = AddCustomFieldsToSearchFields(entityPM, mySearchFields);
            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }

        private static string AddCustomFieldsToSearchFields(TicketPM entityPM, string mySearchFields)
        {
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Ticket", entityPM.Tenant).Where(o => o.DataTypeCode == "Text" || o.DataTypeCode == "nText" || o.DataTypeCode == "PickList").ToList();
            string searchFields = mySearchFields;

            foreach (ObjectField field in customFields)
            {
                searchFields = AddCustomFieldValueToSearchFields(entityPM, searchFields, field);
            }
            return searchFields;
        }

        private static string AddCustomFieldValueToSearchFields(TicketPM entityPM, string mySearchFields, ObjectField field)
        {
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            string searchFields = mySearchFields;

            object value = customFieldResolver.GetFieldValue(entityPM, field, entityPM.Tenant);
            if (value != null)
            {
                MethodHelper.AddToSearchFields(ref searchFields, value.ToString());
            }

            return searchFields;
        }

        public string GetReplyToEmail(int tenant, string guidId)
        {
            string email = "";
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement myTenant = tenantManagementRepository.GetSingleTenantManagement(tenant);
                if (myTenant != null)
                {
                    string supportEmail = myTenant.SupportEmail;
                    if (!string.IsNullOrEmpty(supportEmail))
                        email = supportEmail.Split('@')[0] + "+" + guidId + "-ex"+"@" + supportEmail.Split('@')[1];
                }

                scope.Complete();
            }

            return email;
        }

   }
}
   