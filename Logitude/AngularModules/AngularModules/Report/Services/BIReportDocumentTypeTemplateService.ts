import { DocumentTypeTemplatePMExtendedService } from "../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService";
import { DocumentTypeListService } from "../../Common/Services/StandardLists/DocumentTypeListService";
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
    constructor(docuemntTypeTemplateId:string ,  bIReportPreviewComponent:BIReportPreviewComponent) {
        this.bIReportPreviewComponent = bIReportPreviewComponent;
        this.bIReportPreviewComponent.DocumentTypeTemplateLists = [];
        this.docuemntTypeTemplateId = docuemntTypeTemplateId;


    }

    Load() {

        let apiQueryFilters = this.GetDocumentTypeApiQueryFilters();
        new DocumentTypeListService().getAllFromCache(apiQueryFilters).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.HasError || !serviceResponse.Result) return;
            let documentType = serviceResponse.Result.filter(d => d.Code == this.documentTypeCode)[0];
            if (!documentType) {
                alert("Please add document type");
                return;
            }
            this.LoadDocumentTypeHTMLTemplate(documentType.Id);
        });
    }


    private GetDocumentTypeApiQueryFilters() {
        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionLocator.Tenant;
        return apiQueryFilters;
    }

 
    LoadDocumentTypeHTMLTemplate(documentTypeId: string) {
        let documentTypeTemplatePMExtendedService = new DocumentTypeTemplatePMExtendedService();
        documentTypeTemplatePMExtendedService.getDocumentTypeTemplatesByDocumentTypeIdAndEditorToolCode(documentTypeId, "R", SessionLocator.Tenant).subscribe((serviceResponse: ServiceResponse) => {
            if (serviceResponse.HasError || !serviceResponse.Result) return;
            var documentTypes = serviceResponse.Result;
            //myResult = myResult.filter(d => d.AutomationId == this.CurrentEntityPM.Id || !d.AutomationId);
            this.FillDocumentTypeList(documentTypes);
        });
    }

    FillDocumentTypeList(documentTypes: any) {
        documentTypes.forEach((item) => {
            this.bIReportPreviewComponent.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
        });
        this.SetDocumentTypeTemplateSelected();
    }

    SetDocumentTypeTemplateSelected() {
        if (!AppTool.IsNullOrEmpty(this.docuemntTypeTemplateId)) {
            this.bIReportPreviewComponent.DocumentTypeTemplateSelected = this.bIReportPreviewComponent.DocumentTypeTemplateLists.filter(d => d.Id == this.docuemntTypeTemplateId)[0];
        }
        if (!this.bIReportPreviewComponent.DocumentTypeTemplateSelected) {
            this.bIReportPreviewComponent.DocumentTypeTemplateSelected = this.bIReportPreviewComponent.DocumentTypeTemplateLists[0];
        }
    }
}
