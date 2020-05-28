
declare var System: any;
declare var window: any;
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
import {DocumentTypeListService} from '../../Common/Services/StandardLists/DocumentTypeListService';
import {AttachmentsList} from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/Filters/AttachmentsList';
import {DocumentTypeList} from '../../Common/EntityLists/DocumentTypeList';
import {SessionInfo} from '../Utilities/SessionInfo';
import {SessionLocator} from '../Utilities/SessionLocator';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';

import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {DocsOutDataViewModel} from '../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/ViewModel/DocsOutDataViewModel';
import {DocumentOutPMService} from '../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {DocumentTypePMExtendedService} from '../../Common/Services/ExtendedPMs/DocumentTypePMExtendedService';
import {EntityPartner} from '../../Infrastructure/DataContracts/EntityPartner';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';

export class GeneralEmailSender {
    public ObjectTableName: string;
    public CurrentObjectTableId: string;
    PartnersObslist: EntityPartner[];

    public ChildObjectTableId: string = "";
    public ChildEntityId: string;
    public CurrentEntityId: string;
    public ChildEntityReference: string;
    public EntityReference: string;
    DocumentFilingId: string;
    Attachments: AttachmentsList[];
    DocumentTypeLists: DocumentTypeList[];
    EntityPM: any;
    ToSpecificeEmail: string = "";
    Subject: string;
    Replyto: string;
    From: string;
    IsCrm: boolean;
    EventTypeCode: string;
    DocumentTypeCode: string;
    EventRefreshName: string;
    LoadingSendingComponent: boolean = false;
    constructor(objecttablename: string, documentTypeCode: string, entityId: string, entityReference: string, childEntityId: string, childEntityReference: string, documentFilingId: string, subject: string, attachments: AttachmentsList[], eventRefreshName: string = null, entityPM: any = null, isCrm: boolean = false, eventTypeCode: string = null, fromMail: string = null) {
        this.LoadingSendingComponent = true;
        this.CurrentObjectTableId = window.ObjectTables.filter(d => d.Name == objecttablename)[0].Id;
        this.ObjectTableName = window.ObjectTables.filter(d => d.Name == objecttablename)[0].Name;
        this.EventRefreshName = eventRefreshName;
        this.CurrentEntityId = entityId;
        this.ChildEntityId = childEntityId;
        this.ChildEntityReference = childEntityReference;
        this.Attachments = attachments;
        this.EntityReference = entityReference;
        this.Subject = subject;
        this.Replyto = "";
        this.From = fromMail != null ? fromMail : "";
        this.DocumentFilingId = documentFilingId;
        this.DocumentTypeCode = documentTypeCode;
        this.EntityPM = entityPM;
        this.IsCrm = isCrm;
        this.EventTypeCode = eventTypeCode;
        this.ToSpecificeEmail = "";

    }


    SendMessage() {

        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 1000) sendWindowWidth = 1000;
        if (sendWindowHeight < 600) sendWindowHeight = 600;

        var logWindow = new LogitudeWindow();

        var windowArgs: any = {};
        windowArgs.WindowHeight = sendWindowHeight;
        windowArgs.Attachments = this.Attachments;
        windowArgs.ChildObjectTableId = this.ChildObjectTableId ? this.ChildObjectTableId : "";
        windowArgs.ChildEntityId = this.ChildEntityId ? this.ChildEntityId : "";
        windowArgs.ChildEntityReference = this.ChildEntityReference ? this.ChildEntityReference : "";
        windowArgs.EntityReference = this.EntityReference ? this.EntityReference : "";
        windowArgs.DocumentFilingId = this.DocumentFilingId ? this.DocumentFilingId : "";
        windowArgs.EntityId = this.CurrentEntityId ? this.CurrentEntityId : "";
        windowArgs.ObjecttableName = this.ObjectTableName ? this.ObjectTableName : "";
        windowArgs.ObjectTableId = this.CurrentObjectTableId ? this.CurrentObjectTableId : "";
        windowArgs.Subject = this.Subject ? this.Subject : "";
        windowArgs.ToEmail = this.Replyto ? this.Replyto : "";
        windowArgs.From = this.From ? this.From : "";
        windowArgs.PartnersObslist = this.PartnersObslist;
        windowArgs.IsUserFromReport = this.PartnersObslist ? true : false;
        windowArgs.EntityPM = this.EntityPM;

        logWindow.WindowArgs = windowArgs;
        logWindow.Width = sendWindowWidth;
        logWindow.Height = sendWindowHeight;
        logWindow.Title = "Send Message";
        logWindow.NotifyOnClose = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./Infrastructure/Components/LogitudeComponents/GeneralSendComponent");
        this.LoadingSendingComponent = false;

    }

    ShowFullSendControll() {

        if (this.EntityPM) {

            var documentTypeListService = new DocumentTypeListService();
            var documentOutPM = null;
            var documentTypeList: DocumentTypeList;
            var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;

            documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    documentTypeList = myResult.filter(d => d.Code == this.DocumentTypeCode)[0];
                    if (documentTypeList) {
                        var documentOutPMService = new DocumentOutPMService();
                        documentOutPMService.getDocumentOutByDocumentTypeEntityAndChild(this.CurrentEntityId, SessionInfo.LoggedUserTenant, "", documentTypeList.Id).subscribe((res:any) => {
                            var pmResponse: ServiceResponse = res;
                            if (!pmResponse.HasError) {
                                var myResult = pmResponse.Result;

                                documentOutPM = myResult;

                                if (!documentOutPM) {
                                    documentOutPMService.getCreateDocumentOut(documentTypeList.Id, this.CurrentEntityId, "", "", this.CurrentObjectTableId, SessionInfo.LoggedUserTenant).subscribe((res:any) => {
                                        var pmResponse: ServiceResponse = res;
                                        if (!pmResponse.HasError) {
                                            var myResult = pmResponse.Result;
                                            if (myResult) {
                                                documentOutPM = myResult;
                                                this.LoadDocumentTypePm(documentOutPM, documentTypeList);
                                            }

                                        } else this.LoadingSendingComponent = false;

                                    });
                                }
                                else {

                                    this.LoadDocumentTypePm(documentOutPM, documentTypeList);
                                }

                            }
                            else this.LoadingSendingComponent = false;


                        });
                    }
                    else {
                        var messageWindow: MessageWindow = new MessageWindow();
                        messageWindow.Show("Document Type with code (" + this.DocumentTypeCode + ") is not found");
                    }
                }

                else this.LoadingSendingComponent = false;
            });
        }
        else this.LoadingSendingComponent = false;
    }

    documentTypePMService: DocumentTypePMExtendedService;
    LoadDocumentTypePm(documentOutPM: any, documentTypeList: any) {


        var documentTypePM = null;
        this.documentTypePMService = new DocumentTypePMExtendedService();
        this.documentTypePMService.getSingleDocumentType(documentTypeList.Id, documentOutPM.Id, SessionInfo.LoggedUserTenant).subscribe((res:any) => {


            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    documentTypePM = myResult;

                    this.LoadSendDocumentComponent(documentTypePM, documentOutPM, documentTypeList);

                }
            } else this.LoadingSendingComponent = false;

        });
    }


    LoadSendDocumentComponent(documentTypePM: any, documentOutPM: any, documentTypeList: any) {
        var SelectedInternalDocument = new DocsOutDataViewModel(documentTypePM, this.CurrentEntityId, documentOutPM.ChildEntityId, this.CurrentObjectTableId, this.ChildObjectTableId, documentOutPM.ChildEntityReference, null, null, null, this.EntityPM);
        SelectedInternalDocument.CurrentDocument = documentOutPM;
        ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, documentTypeList.Name + " Sending");
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 1000) sendWindowWidth = 1000;
        if (sendWindowHeight < 600) sendWindowHeight = 600;

        SelectedInternalDocument.PageRequestSendComponent == "GeneralComponent";
        SelectedInternalDocument.EntityId = this.CurrentEntityId ? this.CurrentEntityId : "";
        SelectedInternalDocument.IsSend = true;
        SelectedInternalDocument.TemplateType = "M";
        SelectedInternalDocument.ModeSendDocument = "Edit";
        SelectedInternalDocument.IsCrm = this.IsCrm;
        SelectedInternalDocument.EventTypeCode = this.EventTypeCode;
        SelectedInternalDocument.Subject = this.Subject;
        SelectedInternalDocument.EventRefreshName = this.EventRefreshName;
        SelectedInternalDocument.AttachmentsLists = this.Attachments;
        SelectedInternalDocument.ToSpecificeEmail = this.ToSpecificeEmail;


        var logWindow = new LogitudeWindow();
        logWindow.Width = sendWindowWidth;
        logWindow.Height = SelectedInternalDocument.WindowHeight = sendWindowHeight;
        logWindow.Title = "Send Message";
        logWindow.DataContext = SelectedInternalDocument;
        logWindow.NotifyOnClose = true;
        logWindow.IsShowCloseButton = true;
        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/SendDocumentComponent");
        this.LoadingSendingComponent = false;

    }
}