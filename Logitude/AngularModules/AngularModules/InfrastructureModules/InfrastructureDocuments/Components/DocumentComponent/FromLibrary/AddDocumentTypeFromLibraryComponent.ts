import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../../../Infrastructure/Utilities/Guid';
import {DocumentTypeTemplateListExtendedService} from '../../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import {DocumentTypeListExtendedService} from '../../../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';
import {DocumentTypePMExtendedService} from '../../../../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {DocumentTypeTemplateViewModel} from '../DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {GroupByPipe} from '../../../../../Infrastructure/Pipes/GroupByPipe';
import {DocumentTypeTemplatePM} from '../../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import { AppTool } from 'Infrastructure/Tools';

@Component({
    
    templateUrl: './AddDocumentTypeFromLibraryComponent.html',
    providers: [DocumentTypeTemplateListExtendedService, DocumentTypePMExtendedService, DocumentTypeListExtendedService]
})

export class AddDocumentTypeFromLibraryComponent implements OnInit {

    public ObjectTableId: string = "";
    public EntityId: string = "";
    public TransportModeId: string = "";
    public ShipmentlevelCode: string = "";
    public ChildEntityId: string = "";
    public ChildObjectTableId: string = "";
    public PageRequest: string = "";
    
    IsLoadTextCode: boolean;
    IsShowMessageNoDocument: boolean;
    public DocumentTypeTemplatePMLists: DocumentTypeTemplatePM[];
    public DocumentTypeTemplateLists: DocumentTypeTemplateViewModel[];
    public FullDocumentTypeTemplateLists: DocumentTypeTemplateViewModel[];

    public DocumentTypeTemplateViewModelSelected: DocumentTypeTemplateViewModel;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeTemplateListExtendedService: DocumentTypeTemplateListExtendedService, public _dcumentTypePMExtendedService: DocumentTypePMExtendedService, public _documentTypeListExtendedService: DocumentTypeListExtendedService) {

    }
    DataViewModel: any;
    ngOnInit(

    ) {


    }


    SetWindowArgs(args: any) {


        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe((response:any) => {

            this.DataViewModel = args.DataViewModel;
            this.ObjectTableId = args.ObjectTableId;
            this.EntityId = args.EntityId;
            this.TransportModeId = args.TransportModeId;
            this.ShipmentlevelCode = args.ShipmentlevelCode;
            this.ChildEntityId = args.ChildEntityId;
            this.ChildObjectTableId = args.ChildObjectTableId;
            this.PageRequest = args.PageRequest;

            this.IsLoadTextCode = true;

            this.DocumentTypeTemplatePMLists = [];
            this.DocumentTypeTemplateLists = [];
            this.FullDocumentTypeTemplateLists = [];
            this.Load();

        });

    }


    onSearchTextChangeEvent(search) {
        if (search) {
            if (search != "Search") {
                this.DocumentTypeTemplateLists = this.FullDocumentTypeTemplateLists.filter(d => d.Description.toUpperCase().indexOf(search.toUpperCase()) > -1 || d.DocumentTypeName.toUpperCase().indexOf(search.toUpperCase()) > -1);
            }
        }
        else {
            this.DocumentTypeTemplateLists = this.FullDocumentTypeTemplateLists;
        }

        if (this.DocumentTypeTemplateLists&& this.DocumentTypeTemplateLists.length == 0) {
            this.IsShowMessageNoDocument = true;
        } else this.IsShowMessageNoDocument = false;

    }


    Load() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        //var isfilter = FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPE") ?true:false;
        var objectTableId = AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ObjectTableId : this.ChildObjectTableId;
        this._documentTypeTemplateListExtendedService.GetDocumentTypeTemplatesFromLibrary(objectTableId, SessionInfo.LoggedUserTenant, true, this.TransportModeId, this.ShipmentlevelCode).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                this.DocumentTypeTemplateLists = [];
                myResult.forEach((item) => {
                    this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                    this.FullDocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));
                    
                });
                //if (myResult) {
                //    var temp = new GroupByPipe().transform(myResult, "DocumentTypeId");
                //}

                if (this.DocumentTypeTemplateLists.length == 0) {
                    this.IsShowMessageNoDocument = true;
                } else this.IsShowMessageNoDocument = false;
            }
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            });
    }


    OnSelectedDocumentTypeTemplateLists(item:any) {
        this.DocumentTypeTemplateViewModelSelected = item;

    }


    CloseButtonClicked() {
    
        this.CurrentSession.CloseCurrentWindow();
    }

     AddFromLibraryButtonClicked(item: DocumentTypeTemplateViewModel) {
        this.DocumentTypeTemplateViewModelSelected = item;
        this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = false;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("saving");
        this.CopyDocumentTypeAndTemplate();

    }


    CopyDocumentTypeAndTemplate() {

        this._documentTypeTemplateListExtendedService.CopyDocumentTypeAndDocumentTypTemplate(this.DocumentTypeTemplateViewModelSelected.Id, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var documenttypecode = pmResponse.Result;
                this._documentTypeListExtendedService.getDocumentTypeListByCode(documenttypecode, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;

                        if (this.PageRequest == "DocOut" && this.DataViewModel) {
                            this.DataViewModel.DocumentTypes.push(myResult);
                            this.DataViewModel.FillDocumentTypes();
                            var selectedInternalDocument = this.DataViewModel.StaticDocumentsList.filter(s => s.DocumentTypeCode == documenttypecode)[0];
                            if (selectedInternalDocument) {
                                if (this.DocumentTypeTemplateViewModelSelected.TemplateType == "P") {
                                    this.DataViewModel.PrintButtonClick(selectedInternalDocument);
                                }
                                else
                                    if (this.DocumentTypeTemplateViewModelSelected.TemplateType == "M") {
                                        this.DataViewModel.SendButtonClick(selectedInternalDocument);
                                    }
                            }

                        }
                  
                       // this.DocumentTypeTemplatePMLists.push(myResult);
                        this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = true;
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CloseButtonClicked()

                    }
                    else {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = true;
                        this.CloseButtonClicked();
                    }
                });

            }
            else {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.DocumentTypeTemplateViewModelSelected.IsEnabledAddDocumentTemplate = true;
            }
        });
    //    
     }


    PreviewFromLibraryButtonClicked(item: DocumentTypeTemplateViewModel) {

        this.DocumentTypeTemplateViewModelSelected = item;
        if (item.TemplateType == "P" && item.EditorTool == "S") {
            this.PreviewStimualTemplate(item, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId);
        }
        else {

            this.PreviewHtmlTemplate(item, this.EntityId, this.ObjectTableId, this.ChildEntityId, this.ChildObjectTableId);
        }
    } 

    PageType: string;
    PreviewStimualTemplate(item: DocumentTypeTemplateViewModel, currentEntityId: string, currentObjectTableId: string, childEntityId: string, ChildObjectTableId: string) {
        var token = ServiceHelper.GetLDocumentDownloadToken();
        window.open(ServiceHelper.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=" + item.Id + "&documentTypeTemplateName" + item.Description + "&entityId=" + currentEntityId + "&entityObjectTableId=" + currentObjectTableId + "&childEntityId=" + childEntityId + "&childObjectTableId=" + ChildObjectTableId + "&TemplateType=" + item.TemplateType + "&EditorTool=" + item.EditorTool + "&tempId=" + token);
    }

    PreviewHtmlTemplate(item: DocumentTypeTemplateViewModel, currentEntityId: string, currentObjectTableId: string, childEntityId: string, ChildObjectTableId: string) {

           
        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.PageType = "Preview";
        windowArgs.TemplateId = item.Id;
        windowArgs.Tenant =0;
        windowArgs.EntityId = currentEntityId;
        windowArgs.ObjectTableId = currentObjectTableId;
        windowArgs.ChildEntityId = childEntityId;
        windowArgs.ChildObjectTableId = ChildObjectTableId;


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
