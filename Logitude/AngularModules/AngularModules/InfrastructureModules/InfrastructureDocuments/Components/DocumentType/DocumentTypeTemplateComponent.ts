declare var System: any;
declare var window: any;
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Component, OnInit}  from '@angular/core';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {DocumentTypeTemplatePMService} from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {DocumentTypeTemplateViewModel} from '../DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import {AppTool, DateTool, FileLoader} from '../../../../Infrastructure/Tools';
import { DocumentTypePMService } from '../../../../Common/Services/StandardPMs/DocumentTypePMService';

@Component({
    
    selector: 'DocumentTypeTemplate',
    templateUrl: './DocumentTypeTemplateComponent.html',
    inputs: ['DocumentType','DocumentTypeTemplates', 'TypeTab'],
    providers: [DocumentTypeTemplatePMService]    
})

export class DocumentTypeTemplateComponent extends BaseComponent implements OnInit {
    DocumentType: DocumentTypePM;

    DocumentTypeTemplateLists: DocumentTypeTemplateViewModel[];
    Title: string;
    TypeTab: string;
    public IsShowButtonDelete: boolean = false;
    TemplateTabCode = "";
    public PageType: string;
    IsShowDeflutCoulm: boolean = false;
    IsShowOriginalTemplateColum: boolean = false; 
    IsEnableEdit: boolean = false;
    Tenant: number = SessionInfo.LoggedUserTenant;
    public documentTypeTemplatePMService: DocumentTypeTemplatePMService;
    CurrentDocumentTypeTemplatePM: DocumentTypeTemplateViewModel;
    DocumentTypeTemplates: DocumentTypeTemplatePM[];
    private CurrentSession = SessionLocator.SelectedSession;
    public documentTypePMService: DocumentTypePMService;
    constructor() {
        super();
         
        this.documentTypePMService = new DocumentTypePMService();;
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();

        }
    }

    ngOnInit(


    ) {

        if (this.DocumentType) {
            if (this.TypeTab == "Document") {

                this.Title =   TextCodeTranslator.Translate("DocumentType.O.Templates")
                this.TemplateTabCode = "P";
            }
            else {
                this.TemplateTabCode = "M";
                this.Title =    TextCodeTranslator.Translate("DocumentType.O.HTMLTemplates")
            }

            this.FillDocumentTypeTemplate();
        }
        this.CheckManageDocumentFeature();

        if (FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPEPROPERTIES")) {
            this.IsShowDeflutCoulm = true;
        }
        else {
            this.IsShowDeflutCoulm = false;
        }


        if (FeatureLocator.HasFeaturePermession("DocumentTypeTemplate", "ORGINALTEMPLATE")) {
            this.IsShowOriginalTemplateColum = true;
        } else this.IsShowOriginalTemplateColum = false;
        

    
    }

    CheckManageDocumentFeature() {
        if (FeatureLocator.HasFeaturePermession("DocumentType", "MANAGEDOCUMENTTEMPLATES")) {
            this.IsEnableEdit = true;
        }
    }

    private StartBusyIndicator() {
        this.CurrentSession.StartBusyIndicator("Saving...");
    }
    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }

    FillDocumentTypeTemplate() {

        this.DocumentTypeTemplateLists = [];

        if (this.DocumentType && this.DocumentTypeTemplates) {
            this.DocumentTypeTemplates.filter(D=> D.TemplateType == this.TemplateTabCode).forEach((item) => {
                if (item.TemplateType == "M") {
                    if (item.Id == this.DocumentType.DocumentTypeDefaultHTMLTemplateId) item.IsDefault = true;
                    else item.IsDefault = false;
                }
                else {

                    if (item.Id == this.DocumentType.DocumentTypeDefaultReportTemplateId) item.IsDefault = true;
                    else item.IsDefault = false;

                   

                }
              
                this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));

            });
        }

    }






    AddTemplateButtonClicked() {



        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Maintenance";
        windowArgs.ObjectTableId = this.DocumentType.ObjectTableId;;
        windowArgs.CurrentEntityPM = this.DocumentType;
        windowArgs.DocumentTypeTemplateLists = this.DocumentTypeTemplateLists;
        windowArgs.TypeTab = this.TypeTab;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 550;
        logitudeWindow.Title = this.TypeTab == "Document" ? "New Print Template" : "New Html Template";
       
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/NewReportTemplateComponent');

    }


    DeleteTemplateButtonClicked() {

    }

    SetAsDefaultButtonClicked() {

        if (this.CurrentDocumentTypeTemplatePM && this.DocumentType){
            if (this.TypeTab == "Document") {

                this.DocumentType.DocumentTypeDefaultReportTemplateId = this.CurrentDocumentTypeTemplatePM.Id;
                this.CurrentDocumentTypeTemplatePM.IsDefault = true;
                this.DocumentTypeTemplateLists.forEach((item) => {
                    if (item.Id != this.CurrentDocumentTypeTemplatePM.Id) {
                        item.IsDefault = false;
                    }
                });
            }

            else {
                this.DocumentType.DocumentTypeDefaultHTMLTemplateId = this.CurrentDocumentTypeTemplatePM.Id;
                this.CurrentDocumentTypeTemplatePM.IsDefault = true;
                this.DocumentTypeTemplateLists.forEach((item) => {
                    if (item.Id != this.CurrentDocumentTypeTemplatePM.Id) {
                        item.IsDefault = false;
                    }
                });
            }
        }
    }



    CheckInActiveclick(item: DocumentTypeTemplateViewModel) {

        if (item.InActive) item.InActive = false;
        else item.InActive = true;   
        this.UpdateDocumentTypeTemplate(item.Entity);



    }

    public SelectedDocumentTypeTemplate: DocumentTypeTemplateViewModel;
    EditDocumentTemplate(item: DocumentTypeTemplateViewModel) {

        this.SelectedDocumentTypeTemplate = item;
        if (item.Entity.EditorTool == "R") {
          

            var windowArgs: any = {};
            windowArgs.DataViewModel = this;
            windowArgs.PageType = "Maintenance";
            windowArgs.TemplateId = item.Id;
            windowArgs.Tenant = item.Entity.Tenant;
            windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplateLists;
            windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
            windowArgs.EntityId = "";
            windowArgs.ChildEntityId = "";
            windowArgs.ChildObjectTableId = "";
            if (this.DocumentType) {
                windowArgs.ObjectTableId = this.DocumentType.ObjectTableId;
            }

            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;

            var logWindow = new LogitudeWindow();
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = "Edit Html Template";


            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");

        }

        else {
            var windowArgs: any = {};
            windowArgs.DataViewModel = this;
            windowArgs.TemplateId = item.Id;
            windowArgs.Tenant = item.Entity.Tenant;
            windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplateLists;
            windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
            windowArgs.EntityId = "";
            windowArgs.ChildEntityId = "";
            windowArgs.ChildObjectTableId = "";
            if (this.DocumentType) {
                windowArgs.ObjectTableId = this.DocumentType.ObjectTableId;
            }

            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;

            var logWindow = new LogitudeWindow();
            
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = "Edit Print Template";
            logWindow.IsShowCloseButton = true;

            logWindow.WindowArgs = windowArgs;
            window.designerClosed = false;
                logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");
              
                var pollTimer = window.setInterval(function () {
                    if (window.sessionStorage.getItem("designerClosed")) { // !== is required for compatibility with Opera
                        window.clearInterval(pollTimer);
                        this.designerPopUpClosed();
                    }
                }, 200);
           
        }


    }

    RunStimulsoftDesigner(templateId: string) {
        var URI = AppTool.GetLogitudeURL() + "/Stimulsoft/Designer.aspx?token=" + SessionInfo.Token + "&tenant=" + SessionInfo.LoggedUserTenant + "&templateId=" + templateId;

        var WindowHeight = window.innerHeight - 200;
        var WindowWidth = window.innerWidth - 100;

        var win = window.open(URI, 'Stimulsoft Designer', 'left=100, top=200,directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=' + WindowWidth + ',height=' + WindowHeight );


        var pollTimer = window.setInterval(function () {
            if (win.closed !== false) { // !== is required for compatibility with Opera
                window.clearInterval(pollTimer);
                //this.designerPopUpClosed();
            }
        }, 200);
    }

    designerPopUpClosed() {
        alert("designer closed!");
    }

    ShowDefaultsDocumentTypeTemplates(item: DocumentTypeTemplateViewModel) {

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 350;
        logitudeWindow.DataContext = item.Entity;
        logitudeWindow.Title = "Defaults";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/AdvanceDocumentTypeTemplateComponent');


    }



    DescriptionKeyUpMethod(selectedItem: DocumentTypeTemplateViewModel) {
        if (selectedItem != null && selectedItem.Entity && selectedItem.Entity.IsDirty) {
            this.UpdateDocumentTypeTemplate(selectedItem.Entity);
        }
    }



    OnSelectedDocumentTypeTemplateLists(selectedItem: DocumentTypeTemplateViewModel) {
        this.CurrentDocumentTypeTemplatePM = selectedItem;
     

    }


    UpdateDocumentTypeTemplate(item: any) { 
        this.DocumentType.IsAir = !this.DocumentType.IsAir;
        this.DocumentType.IsAir = !this.DocumentType.IsAir; 
        this.StartBusyIndicator();
        this.documentTypeTemplatePMService.update(item).subscribe((res:any) => {
         
        });
        this.UpdateDocumentType();  
    } 
    UpdateDocumentType() {
        this.documentTypePMService.update(this.DocumentType).subscribe((res: any) => {
            this.StopBusyIndicator();
        });
    } 
}
