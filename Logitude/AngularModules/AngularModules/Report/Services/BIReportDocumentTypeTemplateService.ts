import { DocumentTypeTemplatePMExtendedService } from "../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService";
import { DocumentTypeListService } from "../../Common/Services/StandardLists/DocumentTypeListService";
import { MessageWindow } from "../../Controls/Windows/MessageWindow";
import { ApiQueryFilters } from "../../Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "../../Infrastructure/DataContracts/ServiceResponse";
import { AppTool } from "../../Infrastructure/Tools";
import { SessionLocator } from "../../Infrastructure/Utilities/SessionLocator";
import { BIReportPreviewComponent } from "../../InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent";
import { DocumentTypeTemplateViewModel } from "../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel";

export class BIReportDocumentTypeTemplateService {


    private bIReportPreviewComponent: BIReportPreviewComponent;
    private documentTypeCode = "BIRSC";
    private docuemntTypeTemplateId: string;
    private documentType: any;

    constructor(docuemntTypeTemplateId:string ,  bIReportPreviewComponent:BIReportPreviewComponent) {
        this.bIReportPreviewComponent = bIReportPreviewComponent;
        this.bIReportPreviewComponent.DocumentTypeTemplateLists = [];
        this.docuemntTypeTemplateId = docuemntTypeTemplateId;


    }

    Load() {

        new DocumentTypeListService().getAllFromCache(this.GetDocumentTypeApiQueryFilters()).subscribe((serviceResponse: ServiceResponse) => {

            if (serviceResponse.HasError || !serviceResponse.Result) {
                this.ShowMessage((serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) ?  serviceResponse.ErrorsArray[0]: "", "Logitude Message");
                return;
            }

            this.documentType = serviceResponse.Result.filter(d => d.Code == this.documentTypeCode)[0];
            if (!this.documentType) {
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
        new DocumentTypeTemplatePMExtendedService().getDocumentTypeTemplatesByDocumentTypeIdAndEditorToolCode(this.documentType.Id, "R", SessionLocator.Tenant).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.HasError || !serviceResponse.Result) {
                this.ShowMessage((serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) ? serviceResponse.ErrorsArray[0] : "", "Logitude Message");
                return;
            }
            this.FillDocumentTypeList(serviceResponse.Result);
        });
    }

    FillDocumentTypeList(documentTypeTemplates: any) {

        documentTypeTemplates.forEach((item) => {
            this.bIReportPreviewComponent.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
        });


        this.SetDocumentTypeTemplateSelected();
    }

    SetDocumentTypeTemplateSelected() {
        if (!AppTool.IsNullOrEmpty(this.docuemntTypeTemplateId)) {
            this.bIReportPreviewComponent.DocumentTypeTemplateSelected = this.bIReportPreviewComponent.DocumentTypeTemplateLists.filter(d => d.Id == this.docuemntTypeTemplateId)[0];
        }

        if (!this.bIReportPreviewComponent.DocumentTypeTemplateSelected) {
            this.bIReportPreviewComponent.DocumentTypeTemplateSelected = this.bIReportPreviewComponent.DocumentTypeTemplateLists.filter(d => d.Id == this.documentType.DocumentTypeDefaultHTMLTemplateId)[0];
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





}
