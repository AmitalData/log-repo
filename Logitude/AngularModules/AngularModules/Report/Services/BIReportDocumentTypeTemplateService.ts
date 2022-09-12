import { DocumentTypeList } from "../../Common/EntityLists/DocumentTypeList";
import { DocumentTypeTemplatePMExtendedService } from "../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService";
import { DocumentTypeListService } from "../../Common/Services/StandardLists/DocumentTypeListService";
import { MessageWindow } from "../../Controls/Windows/MessageWindow";
import { LogitudeWindow } from "../../Controls/Windows/LogitudeWindow";
import { ApiQueryFilters } from "../../Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "../../Infrastructure/DataContracts/ServiceResponse";
import { AppTool } from "../../Infrastructure/Tools";
import { SessionLocator } from "../../Infrastructure/Utilities/SessionLocator";
import { BIReportPreviewComponent } from "../../InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent";
import { DocumentTypeTemplateViewModel } from "../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel";
declare var window: any;

export class BIReportDocumentTypeTemplateService {


    private bIReportPreviewComponent: BIReportPreviewComponent;
    private documentTypeCode = "BIRSC";
    private docuemntTypeTemplateId: string;
    private documentType: any;
    private DocumentTypeSelected: any;
    private EntityId: any;
    private ObjectTableId: any;
    private RequsetPageName: string;

    constructor(docuemntTypeTemplateId: string, bIReportPreviewComponent: BIReportPreviewComponent, requsetPageName: string, objectTableId: string) {

        this.bIReportPreviewComponent = bIReportPreviewComponent;
        this.bIReportPreviewComponent.DocumentTypeTemplateLists = [];
        this.docuemntTypeTemplateId = docuemntTypeTemplateId;
        this.EntityId = bIReportPreviewComponent.EntityId;
        this.ObjectTableId = this.GetObjectTableId();
        this.RequsetPageName = requsetPageName;
    }

    GetObjectTableId() {
        return window.ObjectTables.filter(f => f.Name == "BIReport")[0].Id;

    }

    Load() {

        let apiQueryFilters = this.GetDocumentTypeApiQueryFilters();
        new DocumentTypeListService().getAllFromCache(apiQueryFilters).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.HasError || !serviceResponse.Result) {
                this.ShowMessage((serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) ?  serviceResponse.ErrorsArray[0]: "", "Logitude Message");
                return;
            }
             this.DocumentTypeSelected = serviceResponse.Result.filter(d => d.Code == this.documentTypeCode)[0];
            if (!this.DocumentTypeSelected) {
                this.ShowMessage("Please add document type");
                return;
            }


            this.LoadDocumentTypeHTMLTemplate();
        });
    }


    private GetDocumentTypeApiQueryFilters() {
        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionLocator.Tenant;
        return apiQueryFilters;
    }

 
    LoadDocumentTypeHTMLTemplate() {
        this.bIReportPreviewComponent.DocumentTypeTemplateLists = [];
        let documentTypeTemplatePMExtendedService = new DocumentTypeTemplatePMExtendedService();
        documentTypeTemplatePMExtendedService.getDocumentTypeTemplatesByDocumentTypeIdAndEditorToolCode(this.DocumentTypeSelected.Id, "R", SessionLocator.Tenant).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.HasError || !serviceResponse.Result) {
                this.ShowMessage((serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) ? serviceResponse.ErrorsArray[0] : "", "Logitude Message");
                return;
            }
            var documentTypeTemplate = serviceResponse.Result;
            documentTypeTemplate = documentTypeTemplate.filter(d => (d.EntityId == this.EntityId && d.ObjectTableId == this.ObjectTableId) || (!d.AutomationId && !d.EntityId));
            this.FillDocumentTypeList(documentTypeTemplate);
        });
    }

    FillDocumentTypeList(documentTypeTemplates: any) {

        documentTypeTemplates.forEach((item) => {
            this.bIReportPreviewComponent.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
        });


        this.SetDocumentTypeTemplateSelected();
    }

    private SetDocumentTypeTemplateSelected() {
        if (!AppTool.IsNullOrEmpty(this.docuemntTypeTemplateId)) {
            this.bIReportPreviewComponent.DocumentTypeTemplateSelected = this.bIReportPreviewComponent.DocumentTypeTemplateLists.filter(d => d.Id == this.docuemntTypeTemplateId)[0];
        }

        if (!this.bIReportPreviewComponent.DocumentTypeTemplateSelected) {
            this.bIReportPreviewComponent.DocumentTypeTemplateSelected = this.bIReportPreviewComponent.DocumentTypeTemplateLists[0];
        }

    }


    public ShowMessage(message: string, title: string = "") {
        if (!message) return;
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }
    
    private IsQuotationDocument(documentTypeList: DocumentTypeList) {

        return documentTypeList.Code == "QUOTE" ? true : false;
    }
    private OpenEditQuoteTemplateComponent(documentTemplate: any) {

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteTemplateId = documentTemplate.Id;
        logWindow.Title = documentTemplate.Name;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = window.innerWidth - 150;
        logWindow.Height = window.innerHeight - 150;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/EditQuoteTemplateComponent");

    }
    private GetWindowsArgsEdit(documentTemplate: any) {
        let windowArgs: any = {};
        windowArgs.DataViewModel = this.bIReportPreviewComponent;
        windowArgs.TemplateId = documentTemplate.Id;
        windowArgs.Tenant = documentTemplate.Tenant;
        windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
        windowArgs.ObjectTableId = this.ObjectTableId ? this.ObjectTableId : null;
        windowArgs.EntityId = this.EntityId ? this.EntityId : null;
        windowArgs.ChildObjectTableId = "";
        windowArgs.RequsetPageName = "BIReport";
        windowArgs.DocumentTypeTemplatePMLists = this.bIReportPreviewComponent.DocumentTypeTemplateLists;
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Maintenance";
        windowArgs.DontShowToField = true;
        return windowArgs;
    }
    private GetWindowsArgsAdd() {
        let windowArgs: any = {};
        windowArgs.DataViewModel = this.bIReportPreviewComponent;
        windowArgs.CurrentEntityPM = this.DocumentTypeSelected;
        windowArgs.DocumentTypeTemplateLists = this.bIReportPreviewComponent.DocumentTypeTemplateLists
        windowArgs.EntityId = this.EntityId ? this.EntityId : null;
        windowArgs.ObjectTableId = this.ObjectTableId ? this.ObjectTableId : null;
        windowArgs.PageType = "Maintenance";
        windowArgs.TypeTab = "RichText";
        windowArgs.RequsetPageName = "BIReport";
        return windowArgs;
    }
    EditDocumentTemplate(documentTemplate: any) {
        if (documentTemplate) {
            if (this.IsQuotationDocument(this.DocumentTypeSelected) && !documentTemplate.EditorTool) {
                this.OpenEditQuoteTemplateComponent(documentTemplate);
                return;
            }
            var windowArgs = this.GetWindowsArgsEdit(documentTemplate);
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var logWindow = new LogitudeWindow();
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = "Edit Html Template";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event) {
                    this.LoadDocumentTypeHTMLTemplate();
                }
            });
        }
    }
    AddDocumentTypeTemplate() {
        var windowArgs = this.GetWindowsArgsAdd();

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 550;
        logitudeWindow.Title = "New Html Template";

        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentType/NewReportTemplateComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if ($event && this.bIReportPreviewComponent.DocumentTypeTemplateLists && this.bIReportPreviewComponent.DocumentTypeTemplateLists.length > 0) {
                this.bIReportPreviewComponent.IsEnableEditTemplate = true;
            }
        });
    }





}
