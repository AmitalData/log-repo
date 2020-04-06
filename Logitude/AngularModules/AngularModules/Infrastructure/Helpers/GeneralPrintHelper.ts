
declare var System: any;
declare var window: any;
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
import {SessionLocator} from '../Utilities/SessionLocator';
import {SessionInfo} from '../Utilities/SessionInfo';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {DocumentTypePM} from '../../Common/EntityPMs/DocumentTypePM';
import {DocumentOutPM} from '../../Common/EntityPMs/DocumentOutPM';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {DocumentTypeListService} from '../../Common/Services/StandardLists/DocumentTypeListService';
import {DocumentTypePMExtendedService} from '../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';

import {DocumentTypeList} from '../../Common/EntityLists/DocumentTypeList';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {DocumentOutPMService} from '../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {DocsOutDataViewModel} from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';


export class GeneralPrintHelper {
    public ObjectTableName: string;
    public CurrentObjectTableId: string;
    ChildObjectTableId: string;

    public DocumentTypeCode: string;
    public ChildEntityId: string;

    ChildReference: string;
    public EntityId: string;
    documentTypeList: DocumentTypeList;
    documentOutPM: DocumentOutPM;
    documentTypePM: DocumentTypePM;
    documentTypePMService: DocumentTypePMExtendedService;
    documentOutPMService: DocumentOutPMService;
    public IsLoadPrintControl: boolean = false;
    public IsStartPrint: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(objecttablename: string, documentTypeCode: string, entityId: string, childEntityId: string, childReference:string ,childObjectTableId:string ) {
        this.ObjectTableName = objecttablename;
        if (!AppTool.IsNullOrEmpty(documentTypeCode)) {
            this.DocumentTypeCode = documentTypeCode.toUpperCase();
        }

        this.CurrentObjectTableId = window.ObjectTables.filter(d => d.Name == objecttablename)[0].Id;
        this.EntityId = entityId == "null" || !entityId ? "" : entityId;
        this.ChildEntityId = childEntityId == "null" || !childEntityId ? "" : childEntityId;
        this.ChildObjectTableId = childObjectTableId == "null" || !childObjectTableId ? "" : childObjectTableId;
        this.ChildReference = childReference == "null" || !childReference ? "" : childReference;

        this.documentTypePMService = new DocumentTypePMExtendedService();
        this.documentOutPMService = new DocumentOutPMService();
        var documentTypeListService = new DocumentTypeListService();


        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;

        documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                  this.documentTypeList = myResult.filter(d => d.Code.toUpperCase() == this.DocumentTypeCode)[0];
                if (this.documentTypeList) {
                    if (this.documentTypeList.DocumentTypeDefaultReportTemplateId) {
                        this.IsLoadPrintControl = true;

                    }
                    else this.ShowMessage("Document type of code " + this.DocumentTypeCode + " has no default template");

                }

                else {
                    if (this.ObjectTableName == "APPayment") {
                        this.ShowMessage("There is no document type for A/P Payment please go to maintenance and add it!");
                    }
                    else if (this.ObjectTableName == "ARPayment") {
                        this.ShowMessage("There is no document type for A/R Payment please go to maintenance and add it!");
                    }

                    else this.ShowMessage("Document type of code " + this.DocumentTypeCode + " not exists");

                }
            

            }
        });


    }


    ShowPrintControl() {

        if (this.IsLoadPrintControl && !this.IsStartPrint) {
            this.IsStartPrint = true;
            this.CurrentSession.StartBusyIndicatorLoading();
            this.documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(this.EntityId, SessionInfo.LoggedUserTenant, this.ChildEntityId, this.documentTypeList.Id).subscribe((res:any) => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;

                    this.documentOutPM = myResult;

                    if (!this.documentOutPM) {
                        this.documentOutPMService.getCreateDocumentOut(this.documentTypeList.Id, this.EntityId, this.ChildEntityId, this.ChildReference, this.CurrentObjectTableId, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
                            var pmResponse: ServiceResponse = res;
                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;
                                if (myResult) {
                                    this.documentOutPM = myResult;
                                    this.LoadDocumentTypePm();
                                }

                            }
                            else {
                                this.CurrentSession.StopBusyIndicator();
                                this.IsStartPrint = false;
                            }

                        });
                    }
                    else {


                        this.LoadDocumentTypePm();
                    }

                }
                else {
                    this.IsStartPrint = false;
                    this.CurrentSession.StopBusyIndicator();
                }

            });

        }

    }

    LoadDocumentTypePm() {


        this.documentTypePMService.getSingleDocumentType(this.documentTypeList.Id, this.documentOutPM.Id, SessionInfo.LoggedUserTenant).subscribe((res:any) => {


            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.documentTypePM = myResult;
                    this.LoadPrintControl();
                }

            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.IsStartPrint = false;
            }


        });
                            
    }


    LoadPrintControl() {
        
           this.IsStartPrint = false;

            this.CurrentSession.StopBusyIndicator();
           var documentOutPmLists = new Array<DocumentOutPM>();
            //this.documentOutPM.NeedsRebuild = true;
           documentOutPmLists.push(this.documentOutPM);
           var SelectedInternalDocument = new DocsOutDataViewModel(this.documentTypePM, this.EntityId, this.documentOutPM.ChildEntityId, this.CurrentObjectTableId, this.ChildObjectTableId, this.documentOutPM.ChildEntityReference,
               documentOutPmLists, null, null, null);

           SelectedInternalDocument.IsNotFromDocsOutListOpenPrintControl = true;
            SelectedInternalDocument.IsAWBWizard = false;
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 760;

            var heightwindwo: number = this.documentOutPM.IssuedDate ? 552 : 502; 
            logitudeWindow.Height = heightwindwo;
            logitudeWindow.DataContext = SelectedInternalDocument;
            logitudeWindow.Title = "Print " + this.documentTypePM.Name;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/PrintDocumentComponent');
            logitudeWindow.WindowClosed.subscribe(($event: any) => {
                if (this.CurrentSession.CurrentEditComponent) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            });
       
    }


    public ShowMessage(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }







}
