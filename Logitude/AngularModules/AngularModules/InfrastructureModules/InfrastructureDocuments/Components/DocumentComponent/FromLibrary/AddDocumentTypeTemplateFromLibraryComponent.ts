import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';
import {DocumentTypeTemplateListExtendedService} from '../../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import {DocumentTypePMExtendedService} from '../../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {DocumentTypeTemplateViewModel} from '../DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import {DocumentTypeTemplatePMExtendedService} from '../../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {GroupByPipe} from '../../../../../Infrastructure/Pipes/GroupByPipe';
import {DocumentTypeTemplatePMService} from '../../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {DocumentTypeTemplatePM} from '../../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {DocumentTypePM} from '../../../../../Common/EntityPMs/DocumentTypePM';

@Component({
    moduleId: module.id,
    templateUrl: './AddDocumentTypeTemplateFromLibraryComponent.html',
    providers: [DocumentTypeTemplateListExtendedService, DocumentTypePMExtendedService, DocumentTypeTemplatePMService, DocumentTypeTemplatePMExtendedService]
})

export class AddDocumentTypeTemplateFromLibraryComponent implements OnInit {

    public ObjectTableId: string = "";
    public EntityId: string = "";
    public TransportModeId: string = "";
    public ShipmentlevelCode: string = "";
    public ChildEntityId: string = "";
    public ChildObjectTableId: string = "";
    public PageRequest: string = "";

    IsLoadTextCode: boolean;
    IsShowMessageNoTemplate: boolean;
    public SelectedDcumentType: DocumentTypeTemplatePM;
    public CurrentDocumentType: DocumentTypePM;
    public DocumentTypeTemplatePMLists: DocumentTypeTemplatePM[];
    public DocumentTypeTemplateLists: DocumentTypeTemplateViewModel[];

    newTemplatePm: DocumentTypeTemplatePM;
    public documentTypeTemplatePMService: DocumentTypeTemplatePMService;

    public DocumentTypeTemplateViewModelSelected: DocumentTypeTemplateViewModel;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeTemplateListExtendedService: DocumentTypeTemplateListExtendedService, public _dcumentTypePMExtendedService: DocumentTypePMExtendedService, public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService) {
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();
        }
    }
    DataViewModel: any;
    ngOnInit(

    ) {


    }

    DocumentTemplateEditorTool: string;
    SetWindowArgs(args: any) {


    

            this.DataViewModel = args.DataViewModel;
            this.CurrentDocumentType = args.CurrentDocumentType;
            this.PageRequest = args.PageRequest;
            this.ObjectTableId = args.ObjectTableId;
            this.EntityId = args.EntityId;
            this.ChildEntityId = args.ChildEntityId;
            this.ChildObjectTableId = args.ChildObjectTableId;
            this.DocumentTemplateEditorTool = this.CurrentDocumentType.DocumentTypeDefaultEditorTool;
            if (this.PageRequest != "Send" && args.DocumentTemplateEditorTool) {
                this.DocumentTemplateEditorTool = args.DocumentTemplateEditorTool;
            }

            this.DocumentTypeTemplatePMLists = [];
            this.DocumentTypeTemplateLists = [];
  
            this.Load();

     

    }





    Load() {
        this.DocumentTypeTemplateLists = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        var isfilter = FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPE") ? false : true;
        this._documentTypeTemplateListExtendedService.GetDocumentTypeTemplatesFromLibraryByDocumentTypeId(0, this.CurrentDocumentType.Id, isfilter,SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                myResult.forEach((item) => {
  
                    if (this.CurrentDocumentType.TemplateFormatCode == "P" && this.DocumentTemplateEditorTool == "S" && this.PageRequest != "Send") {
                            if (item.TemplateType == "P" && item.EditorTool == "S") {
                            this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                        }
                    }
                    else {

                        if (this.PageRequest == "Send") {
                            if (item.TemplateType == "M") {
                                this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                            }
                        }
                        else {
                            if (item.TemplateType == "P" && item.EditorTool == "R") {
                                this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                            }

                        }
                    }
                

                });
         
                this.newTemplatePm = new DocumentTypeTemplatePM();
                this.newTemplatePm.Tenant = SessionInfo.LoggedUserTenant;
                this.newTemplatePm.DocumentTypeId = this.CurrentDocumentType.Id;
                this.newTemplatePm.LastUpdatedByUserId = SessionInfo.LoggedUserId;
                this.newTemplatePm.LastUpdateByUserName = SessionInfo.LoggedUserPM.Contact;
                this.newTemplatePm.DocumentTypeId = this.CurrentDocumentType.Id;

                if (!this.CurrentDocumentType.DocumentTypeDefaultHTMLTemplateId) {
                    this.newTemplatePm.IsDefault = true;

                }
                if (this.DocumentTypeTemplateLists.length == 0) {
                    this.IsShowMessageNoTemplate = true;
                } else this.IsShowMessageNoTemplate = false;
            }


            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        });
    }



    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }

    AddFromLibraryButtonClicked(item: DocumentTypeTemplateViewModel) {
    
        this.DocumentTypeTemplateViewModelSelected = item;
        this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = false;

        var countOfCopy: number = 0;
        var   documentTypeTemplateList: DocumentTypeTemplateViewModel = null;


        if (this.DataViewModel && this.DataViewModel.ReportTemplates) {

              documentTypeTemplateList = this.DataViewModel.ReportTemplates.filter(d=> d.OriginalTemplateId == this.DocumentTypeTemplateViewModelSelected.Id)[0];
            if (documentTypeTemplateList) {
                countOfCopy = this.DataViewModel.ReportTemplates.filter(d=> d.OriginalTemplateId == this.DocumentTypeTemplateViewModelSelected.Id).length;
              }

           
            this.newTemplatePm.IsDefault = this.DataViewModel.ReportTemplates.length == 0 ? true : false;
            this.newTemplatePm.Description = countOfCopy > 0 ? this.DocumentTypeTemplateViewModelSelected.Description + " [" + countOfCopy + "]" : this.DocumentTypeTemplateViewModelSelected.Description; ;  

            if (documentTypeTemplateList || countOfCopy > 0) {

                var confirmWindow = new ConfirmWindow();
                confirmWindow.YesButtonText = "Ok";
                confirmWindow.NoButtonText = "Cancel";
                confirmWindow.Width = 400;
                confirmWindow.Height = 200;
                confirmWindow.Show("Please note that this template already exists, Please confirm to add a new template");

                confirmWindow.Title = "DocumentTypeTemplate";
          
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                    
                        this.CopyDocumentTypeTemplate();
          
                    }
                });

            }
            else {
                this.CopyDocumentTypeTemplate();
         
            }

        }

      





    }


    CopyDocumentTypeTemplate() {

        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

        var documenttypetemplatePm: DocumentTypeTemplatePM = this.DocumentTypeTemplatePMLists.filter(d=> d.Id == this.DocumentTypeTemplateViewModelSelected.Id)[0];
        if (!documenttypetemplatePm) {

            this._documentTypeTemplatePMExtendedService.GetSingleDocumentTypeTemplate(this.DocumentTypeTemplateViewModelSelected.Id, 0).subscribe(res => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var pmResponse: ServiceResponse = res;
                var myResult = pmResponse.Result;
                if (myResult) {
                    documenttypetemplatePm = myResult;
                    this.SaveDocumentTypeTemplate(documenttypetemplatePm);
                }
                else {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = true;
                }

            });

        }
        else {

            this.SaveDocumentTypeTemplate(documenttypetemplatePm);
        }

    }



    SaveDocumentTypeTemplate(documenttypetemplatePm: DocumentTypeTemplatePM) {

        this.newTemplatePm.TemplateBodyHtml = documenttypetemplatePm.TemplateBodyHtml;
        this.newTemplatePm.TemplateBody = documenttypetemplatePm.TemplateBody;
        this.newTemplatePm.TemplateType = documenttypetemplatePm.TemplateType;
        this.newTemplatePm.IsEnabledForCustomers = true;
        this.newTemplatePm.Language = documenttypetemplatePm.Language;
        this.newTemplatePm.CountryCode = documenttypetemplatePm.CountryCode;
        this.newTemplatePm.DocumentTypeCode = documenttypetemplatePm.DocumentTypeCode;
        this.newTemplatePm.InternalRemarks = documenttypetemplatePm.InternalRemarks;
        this.newTemplatePm.EditorTool = documenttypetemplatePm.EditorTool;
        this.newTemplatePm.Subject = documenttypetemplatePm.Subject;
        this.newTemplatePm.OriginalTemplateId = documenttypetemplatePm.Id;
        this.newTemplatePm.IsCopiedAtSignup = true;

       
        this.documentTypeTemplatePMService.insert(this.newTemplatePm).subscribe(res=> {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var pmResponse: ServiceResponse = res;

            this.CurrentSession.CloseCurrentWindow();
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.DocumentTypeTemplatePMLists.push(myResult);

                    if (this.DataViewModel) {
                        var template = new DocumentTypeTemplateViewModel(myResult);
                        this.DataViewModel.ReportTemplates.push(template);
                        this.DataViewModel.OnSelectTemplateChange(template);
                    }
                }


            }


        });
    }



    PreviewFromLibraryButtonClicked(item: DocumentTypeTemplateViewModel) {

        this.DocumentTypeTemplateViewModelSelected = item;
        if (item.TemplateType == "P" && item.EditorTool == "S") {
            this.PreviewStimualTemplate(item, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId);
        }
        else {
            this.PreviewHtmlTemplate(item);
        }
    }

    PageType: string;
    PreviewStimualTemplate(item: DocumentTypeTemplateViewModel, currentEntityId: string, currentObjectTableId: string, childEntityId: string, ChildObjectTableId: string) {
        var token = ServiceHelper.GetLDocumentDownloadToken();
        window.open(ServiceHelper.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=" + item.Id + "&documentTypeTemplateName" + item.Description + "&entityId=" + currentEntityId + "&entityObjectTableId=" + currentObjectTableId + "&childEntityId=" + childEntityId + "&childObjectTableId=" + ChildObjectTableId + "&TemplateType=" + item.TemplateType + "&EditorTool=" + item.EditorTool + "&tempId=" + token);
    }

    PreviewHtmlTemplate(item: DocumentTypeTemplateViewModel) {


        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Preview";
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant = 0;



        windowArgs.EntityId = this.EntityId;
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ChildEntityId = this.ChildEntityId;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId;

        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;

        var logWindow = new LogitudeWindow();
        logWindow.Width = widthwindow - 100;
        logWindow.Height = heighthwindow - 100;
        logWindow.Title = item.Description;
        windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplatePMLists;

        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");



    }
}
