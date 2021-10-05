declare var System: any;
declare var window: any;
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Component, OnInit}  from '@angular/core';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {DocumentTypeTemplatePMService} from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
declare var  StringToBase64, Base64ToString: any;
import {ReportsTemplatePM} from '../../../../Common/EntityPMs/ReportsTemplatePM';
import {ReportsTemplatePMExtendedService} from '../../../../Common/Services/ExtendedPMs/ReportsTemplatePMExtendedService';
 
@Component({
    
    selector: 'SaveAsTemplate',
    templateUrl: './SaveAsTemplateComponent.html',
    providers: [DocumentTypeTemplatePMService, ServiceArgs]
})

export class SaveAsTemplateComponent implements OnInit {

    SelectedTemplate: any;
    public ValidationErrorsList: string[];
    Description: string;

    private documentTypeTemplatePMService: DocumentTypeTemplatePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();
    
          }
    }

    ngOnInit(


    ) {



    }

    PageType: string;
    DataContext: any;
    SetDataContext(dataContext: any) {

        this.DataContext = dataContext;
        if (this.DataContext) {
            this.PageType = this.DataContext.PageType;
            this.SelectedTemplate = this.DataContext.template;
            
        }
    }



    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }
 


    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        if (!this.Description) {
            this.ValidationErrorsList.push("Description field is required");
        }
        else {

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            var newTemplatePM: any = null;

            if (this.PageType == "ReportTemplate") {
                newTemplatePM = new ReportsTemplatePM();
                newTemplatePM.Description = this.Description;
                newTemplatePM.TemplateData = StringToBase64(this.DataContext.froalaEditorSetting.froalaEditorComponent.getHtml());
                newTemplatePM.CreatedByUserId = SessionLocator.LoggedUserId;
                newTemplatePM.UpdatedByUserId = SessionLocator.LoggedUserId;

                newTemplatePM.TemplateType = "M";
                newTemplatePM.ReportId = this.SelectedTemplate.ReportId;

                

                var reportsTemplatePMExtendedService: ReportsTemplatePMExtendedService = new ReportsTemplatePMExtendedService();
                reportsTemplatePMExtendedService.CreateReportTemplate(newTemplatePM).subscribe((res: any) => {

                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    var pmResponse: ServiceResponse = res;


                    if (!pmResponse.HasError) {
                        var result = pmResponse.Result;
                        if (result) {
                            this.CurrentSession.CurrentWindow.Close(result.Id);
                        }
                    }
                    else {
                        pmResponse.ErrorsArray.forEach((item) => {
                            this.ValidationErrorsList.push(item);
                        });
                    }
                });
            }
            else {
                newTemplatePM = new DocumentTypeTemplatePM();
                newTemplatePM.Description = this.Description;
                newTemplatePM.DocumentTypeId = this.SelectedTemplate.DocumentTypeId;
                newTemplatePM.LastUpdateDate = this.SelectedTemplate.LastUpdateDate;
                newTemplatePM.LastUpdatedByUserId = SessionInfo.LoggedUserPM.Id;
                newTemplatePM.TemplateType = this.SelectedTemplate.TemplateType;
                newTemplatePM.Tenant = SessionInfo.LoggedUserTenant;
                newTemplatePM.EditorTool = this.SelectedTemplate.EditorTool;
                newTemplatePM.HorizontalShift = this.SelectedTemplate.HorizontalShift;
                newTemplatePM.VerticalShift = this.SelectedTemplate.VerticalShift;
                newTemplatePM.Subject = this.DataContext.Subject;
                newTemplatePM.IsCopiedAtSignup = true;
                newTemplatePM.IsEnabledForCustomers = true;
                newTemplatePM.CountryCode = this.SelectedTemplate.CountryCode;
                newTemplatePM.InternalRemarks = this.SelectedTemplate.InternalRemarks;
                newTemplatePM.Language = this.SelectedTemplate.Language;
                newTemplatePM.OriginalTemplateId = this.SelectedTemplate.Id;
                newTemplatePM.TemplateTechnologyCode = "AG";
                newTemplatePM.TemplateHeaderHeight = this.SelectedTemplate.TemplateHeaderHeight;
                newTemplatePM.TemplateHeaderHtml = this.SelectedTemplate.TemplateHeaderHtml;
                newTemplatePM.TemplateFooterHeight = this.SelectedTemplate.TemplateFooterHeight;
                newTemplatePM.TemplateFooterHtml = this.SelectedTemplate.TemplateFooterHtml;
                newTemplatePM.AutomationId = !this.DataContext.AutomationId ? null : this.DataContext.AutomationId;
                this.MapDefultAttachmentsDocumentFields(newTemplatePM);
                 
                if (this.DataContext) {
                    newTemplatePM.TemplateBodyHtml = StringToBase64(this.DataContext.froalaEditorSetting.froalaEditorComponent.getHtml());
                }

                this.documentTypeTemplatePMService.insert(newTemplatePM).subscribe((myResult:any) => {




                    if (myResult) {

                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (myResult.HasError) {

                            myResult.ErrorsArray.forEach((item) => {
                                this.ValidationErrorsList.push(item);
                            });
                        }
                        else {
                            if (this.DataContext && myResult) {

                                if (this.DataContext.DocumentTypeTemplatePMLists) {
                                    this.DataContext.DocumentTypeTemplatePMLists.push(myResult.Result);
                                }

                                this.CurrentSession.CurrentWindow.Close(myResult.Result.Id);


                            }

                        }

                    }


                });

            }
    
        }
      
    }





    private MapDefultAttachmentsDocumentFields(newTemplatePM: any) {
        newTemplatePM.DefultAttachmentsXML = this.SelectedTemplate.DefultAttachmentsXML;
        newTemplatePM.DocumentDefultAttachments = this.SelectedTemplate.DocumentDefultAttachments;
        newTemplatePM.AttachedExternalDocumentsIds = this.SelectedTemplate.AttachedExternalDocumentsIds;
        newTemplatePM.IsDefultAttachmentsXMLChanged = true;
    }
}
