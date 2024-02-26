declare var window;
import { Component } from '@angular/core';
import { DocumentTypeMetaDataExtendedService } from '../../../Common/Services/ExtendedPMs/DocumentTypeMetaDataExtendedService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { CustomsClosedTableList } from '../../../Customs/EntityLists/CustomsClosedTableList';
import { CustomDocumentTypeMetaDataPM } from '../../../Customs/EntityPMs/CustomDocumentTypeMetaDataPM';
import { CustomsDocumentMetaDataValuePM } from '../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM';
import { CustomsDocumentPM } from '../../../Customs/EntityPMs/CustomsDocumentPM';
import { CustomsDocumentPointerPM } from '../../../Customs/EntityPMs/CustomsDocumentPointerPM';
import { CustomsDocumentsTicketPM } from '../../../Customs/EntityPMs/CustomsDocumentsTicketPM';
import { CustomDocumentTypeListService } from '../../../Customs/Services/StandardLists/CustomDocumentTypeListService';
import { CustomsClosedTableListService } from '../../../Customs/Services/StandardLists/CustomsClosedTableListService';
import { CustomsSettingListService } from '../../../Customs/Services/StandardLists/CustomsSettingListService';
import { CustomsDocumentPMService } from '../../../Customs/Services/StandardPMs/CustomsDocumentPMService';
import { CustomsRequestSheetExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomsRequestSheetExtendedPMService';
import { CustomsDocumentsTicketPMService } from '../../../Customs/Services/StandardPMs/CustomsDocumentsTicketPMService';
import { CustDocTypeMetaDataWebService } from '../../../Customs/Services/WebServices/CustDocTypeMetaDataWebService';
import { CustomDocumentViewerService } from '../../../Customs/Services/WebServices/CustomDocumentViewerService';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { ConnectedToItem } from './ConnectedToItem';
import { ICustomsDocumentsController } from './ICustomsDocumentsController';
import { CustomDocumentNewVersionService } from '../services/CustomDocumentNewVersion.service';
import { CustomsDocumentsTicketsExtendedService } from 'Customs/Services/ExtendedPMs/CustomsDocumentsTicketsExtendedService';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { CustomsSettingExtendedListService } from 'Customs/Services/ExtendedLists/CustomsSettingExtendedListService';

@Component({

    templateUrl: './AddEditCustomsDocumentComponent.html',
})

export class AddEditCustomsDocumentComponent extends BaseComponent {
    //***********************Properties**************************************//
    public ValidationErrorsList: any[];
    DataContext = this;
    private documentTypeCode: string;
    get DocumentTypeCode() {
        if (this.CustomsDocument) {
            this.documentTypeCode = this.CustomsDocument.DocumentTypeCode;
        }
        else if (this.CustomsDocumentsTicket) {
            this.documentTypeCode = this.CustomsDocumentsTicket.DocumentTypeCode;
        }

        return this.documentTypeCode;

    }
    set DocumentTypeCode(value: string) {
        if (value != this.documentTypeCode) {
            this.documentTypeCode = value;
            //if (value != null) {
            if (this.CustomsDocument) {
                //not needed
                //if (this.newVersionAdded) {
                //    this.documentTypeCode = value;
                //    this.CurrentSession.StopBusyIndicator();
                //    var confirmWindow = new ConfirmWindow();
                //    confirmWindow.Width = 400;
                //    confirmWindow.Height = 200;
                //    confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                //    confirmWindow.ShowNoButton = false;
                //    confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.No");
                //    confirmWindow.Show(TextCodeTranslator.Translate("Customs.CustomsDocuments.DocMetadataWarning"));
                //    confirmWindow.WindowClosed.subscribe((event: any) => {
                //        if (confirmWindow.Yes) {
                //            this.CustomsDocument.DocumentTypeCode = this.documentTypeCode;
                //            //customDocumentMetaDataControlViewModel.LoadMetaData(previousValueList);
                //            confirmWindow.Close();
                //        }
                //    });
                //}
                // else {
                this.CustomsDocument.DocumentTypeCode = value;
                this.ClearAllMetaDataValues();
                this.InitializeMetaData(this.previousValueList);
                // }
            }
            if (this.CustomsDocumentsTicket) {
                this.CustomsDocumentsTicket.DocumentTypeCode = value;
                //ChangePointersDocumentTypeCode(value);
                //this.IsTicketChanged = true;
            }
            //}
        }
    }
    IsDisplayOnly: boolean;
    IsEntityDisplayOnly: boolean;
    DisplayOnlyMessage: string;
    public CustomsDocument: CustomsDocumentPM;
    public CustomsDocumentsTicket: CustomsDocumentsTicketPM;
    newVersionAdded: boolean;
    IsNewState: boolean;
    IsTicketChanged = false;
    ParentEntityCode: string;
    ParentEntityId: string;
    Child1EntityId: string;
    Child1EntityCode: string;
    Child2EntityId: string;
    Child2EntityCode: string;
    Child3EntityId: string;
    Child3EntityCode: string;
    IsCustomsDocumentInRequest: boolean;
    ViewDisableMessageVisibility: boolean;
    IsDocumentTypeEnabled: boolean;
    IsMetaDataEditEnabled: boolean;
    IsSendDocumentEnabled: boolean;
    IsPointerChangeEnabled: boolean;
    IsEditEnabled: boolean;
    ConnectedToItems: ConnectedToItem[];
    SelectedIndex: number;
    public iCustomsDocumentsController: ICustomsDocumentsController;
    ChildrenComboBoxVisibility: boolean = true;
    UpperHeight: any = { 'height': '130px' };
    DisplayConnectedEntityNumber: string;
    RelatedEntityLable: string;
    SupplierInvoiceItemsVisibility: boolean = true;
    SupplierInvoiceItemNumber: string;
    SupplierInvoiceNumber: string;
    CRENumber: string;
    LayoutDirection: string = 'rtl';
    SecondChildVisibility: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    showPdfDocument: boolean = false;

    get Remarks() {
        if (this.CustomsDocumentsTicket) {
            return this.CustomsDocumentsTicket.Remarks;
        }
        else {
            return null;
        }
    }
    set Remarks(value: string) {
        this.CustomsDocumentsTicket.Remarks = value
    }

    get VerificationRemarks() {
        if (this.CustomsDocumentsTicket) {
            return this.CustomsDocumentsTicket.VerificationRemarks;
        }
        else {
            return null;
        }
    }
    set VerificationRemarks(value: string) {
        this.CustomsDocumentsTicket.VerificationRemarks = value
    }

    get UserRemarks() {
        if (this.CustomsDocumentsTicket) {
            return this.CustomsDocumentsTicket.UserRemarks;
        }
        else {
            return null;
        }
    }
    set UserRemarks(value: string) {
        this.CustomsDocumentsTicket.UserRemarks = value
    }

    UserRemarksTextBoxVisiblity: boolean;
    VerificationRemarksTextBoxVisiblity: boolean;
    RemarksTextBoxVisiblity: boolean;
    IsUserTabSelected: boolean = false;

    customDocumentTypeMetaDataList: CustomDocumentTypeMetaDataPM[];
    previousValueList: CustomsDocumentMetaDataValuePM[];
    customDocumentMetaDataValueList: CustomsDocumentMetaDataValuePM[];
    customsClosedTableList: CustomsClosedTableList[];
    MetaDataViewModels: MetaDataViewModel[];
    private _CustomDocumentViewerService: CustomDocumentViewerService = new CustomDocumentViewerService();
    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    private customsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
    IsActionButtonsEnabled: boolean;
    ClosingData: any;
    WindowArgs: any;
    private readonly customDocumentNewVersionService: CustomDocumentNewVersionService = new CustomDocumentNewVersionService();

    //***********************************************************************//
    constructor() {
        super();
        if (FeatureLocator.HasFeaturePermession("Customs.CustomsDocument", "ViewDocumentAsPdf")) {
            this.customsSettingExtendedListService.GetSettingByTenant().subscribe((response: ServiceResponse) => {
                this.showPdfDocument = response?.Result?.CompanyType == "B";
            });
        }
        else {
            this.showPdfDocument = false;
        }
    }

    public isRequireDocumentTicket: string = null;

    SetWindowArgs(windowArgs) {
        this.WindowArgs = windowArgs;

        this.CustomsDocumentsTicket = windowArgs.CustomsDocumentsTicket;

        if (this.CustomsDocumentsTicket) {
            this.CustomsDocumentsTicket.CloneMe();
        }
        this.IsDisplayOnly = windowArgs.IsDisplayOnly;
        this.IsEntityDisplayOnly = windowArgs.IsEntityDisplayOnly;
        this.IsNewState = windowArgs.IsNewState;
        console.log(this.CustomsDocument);

        this.CustomsDocument = windowArgs.CustomsDocument;
        if (this.CustomsDocument) {
            this.CustomsDocument.CloneMe();
        }
        
        this.isRequireDocumentTicket = windowArgs.RequestedDocumentId;
        
        this.ParentEntityCode = windowArgs.ParentEntityCode;
        this.ParentEntityId = windowArgs.ParentEntityId;
        this.Child1EntityCode = windowArgs.Child1EntityCode;
        this.Child1EntityId = windowArgs.Child1EntityId;
        this.Child2EntityCode = windowArgs.Child2EntityCode;
        this.Child2EntityId = windowArgs.Child2EntityId;
        this.Child3EntityCode = windowArgs.Child3EntityCode;
        this.Child3EntityId = windowArgs.Child3EntityId;
        this.iCustomsDocumentsController = windowArgs.iCustomsDocumentsController;
        this.IsCustomsDocumentInRequest = windowArgs.IsCustomsDocumentInRequest;
        this.EntityPM = windowArgs.EntityPM;
        this.ClosingData = windowArgs.ClosingData;
        this.CheckEditEnabled(this.IsCustomsDocumentInRequest, !this.IsDisplayOnly);
        this.SelectedIndex = 0;
        this.FillConnectedToItems();
        if (this.CustomsDocumentsTicket != null && this.CustomsDocumentsTicket.CustomsDocumentPointers != null && this.CustomsDocumentsTicket.CustomsDocumentPointers.length > 0) {
            this.FillConnectedDocumentPointer();
        }

        if (this.CustomsDocumentsTicket != null) {//&& this.objectTableName != "Customs.CustomsCollateral") { this is to be added later when work on collateral
            this.ChildrenComboBoxVisibility = this.iCustomsDocumentsController.GetChildrenComboboxVisibility();
            this.SecondChildVisibility = this.iCustomsDocumentsController.GetSecondChildVisibility();
            if (this.ChildrenComboBoxVisibility) {
                this.UpperHeight = { 'height': '130px' };
            }
            else {
                this.UpperHeight = { 'height': '30px' };
            }
        }
        else {
            this.ChildrenComboBoxVisibility = false;
            this.UpperHeight = { 'height': '30px' };
        }
        this.RelatedEntityLable = this.iCustomsDocumentsController.GetRelatedEntityLabel();
        if (this.ParentEntityCode == "Claim" && AppTool.IsNullOrEmpty(this.DisplayConnectedEntityNumber)) {
            this.SetDefaultConnectedEntityNumber();
        }
        this.RefereshConnectedInvoices();

        if (this.CustomsDocumentsTicket != null) {
            this.RemarksTextBoxVisiblity = true;
            this.VerificationRemarksTextBoxVisiblity = true;
            this.UserRemarksTextBoxVisiblity = true;
            if (!AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.VerificationRemarks)) {
                this.VerificationRemarksTextBoxVisiblity = true;
            }
            else {
                this.VerificationRemarksTextBoxVisiblity = false;
            }

            //if (this.CustomsDocumentsTicket.DocumentsFilingId == null) {
            //    this.RemarksTextBoxVisiblity = false;
            //    this.IsUserTabSelected = true;
            //}
            //else {
            //    this.RemarksTextBoxVisiblity = true;
            //    this.IsUserTabSelected = false;
            //}
        }
        else {
            this.RemarksTextBoxVisiblity = false;
            this.VerificationRemarksTextBoxVisiblity = false
            this.UserRemarksTextBoxVisiblity = false;
        }

        this.BuildTabs();
        if (this.CustomsDocument) {
            this.InitializeMetaData(null);
        }
        this.LoadCustomsSettings();
    }
    LoadCustomsSettings() {
        this.customsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString()).subscribe((response: ServiceResponse) => {
            var list = response.Result;
            if (!AppTool.IsNullOrEmpty(list)) {
                var customsSetting = list;
                this.IsConnectedToUniFreight = customsSetting.IsConnectedToUniFreight;
            }
            this.LoadDocumentPage();
        });
    }
    //#region Document Loading
    base64Document: string;
    IsConnectedToUniFreight: boolean = false;

    LoadDocumentPage() {
        if (this.CustomsDocument) {
            if (this.showPdfDocument) {
                this.LoadPdfDocument();
            }
            else {
                this.LoadTiffDocument();
            }
        }
    }

    LoadTiffDocument() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._CustomDocumentViewerService.GetDocumentPage(this.CustomsDocument.DocumentId, 0, this.IsConnectedToUniFreight).subscribe((myResponse: ServiceResponse) => {
            var result = myResponse.Result;
            if (result) {

                console.log("[Response] GetDocumentPage", result);

                if (!AppTool.IsNullOrEmpty(result.Page)) {
                    this.base64Document = "data:image/png;base64," + result.Page;
                } else {
                    this.base64Document = null;
                }

            } else {
                this.base64Document = null;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    LoadPdfDocument() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._CustomDocumentViewerService.GetDocumentPageAsPdf(this.CustomsDocument.DocumentId).subscribe((myResponse: ServiceResponse) => {
            var result = myResponse.Result;
            if (result) {

                if (!AppTool.IsNullOrEmpty(result.contentField)) {
                    this.base64Document = "data:application/pdf;base64," + result.contentField;
                } else {
                    this.base64Document = null;
                }

            } else {
                this.base64Document = null;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }
    //#endregion

    CheckEditEnabled_Old(isCustomsDocumentInRequest: boolean, isEditEnabled: boolean) {
        this.ViewDisableMessageVisibility = false;
        if (this.CustomsDocument != null) {
            if (!AppTool.IsNullOrEmpty(this.CustomsDocument.CustomsDocId)) {
                if (!isCustomsDocumentInRequest && this.CustomsDocument.DocumentStatusCode != "7" && this.CustomsDocument.DocumentStatusCode != "1") {
                    this.IsDocumentTypeEnabled = false;
                    this.IsEditEnabled = true;
                    this.IsMetaDataEditEnabled = true;
                    this.IsSendDocumentEnabled = true;
                }
                else { //removed redundunt code
                    //if (!isEditEnabled) {
                    //    this.IsEditEnabled = falseca;
                    //    this.IsMetaDataEditEnabled = false;
                    //    this.IsSendDocumentEnabled = false;
                    //    this.IsDocumentTypeEnabled = false;
                    //    this.ViewDisableMessageVisibility = true;
                    //    this.DisplayOnlyMessage = " המסך לתצוגה בלבד - סטטוס המסמך " + this.CustomsDocument.DocumentStatusName;
                    //}
                    //else {
                    this.IsEditEnabled = false;
                    this.IsMetaDataEditEnabled = false;
                    this.IsDocumentTypeEnabled = false;
                    this.IsSendDocumentEnabled = false;
                    this.ViewDisableMessageVisibility = true;
                    this.DisplayOnlyMessage = " המסך לתצוגה בלבד - סטטוס המסמך " + this.CustomsDocument.DocumentStatusName;
                    //}
                }
            }
            else {
                if (!isEditEnabled) {
                    this.IsEditEnabled = false;
                    this.IsMetaDataEditEnabled = false;
                    this.IsSendDocumentEnabled = false;
                    this.IsDocumentTypeEnabled = false;
                    this.ViewDisableMessageVisibility = true;
                    this.DisplayOnlyMessage = " המסך לתצוגה בלבד - סטטוס המסמך " + this.CustomsDocument.DocumentStatusName;
                }
                else {
                    if (!isCustomsDocumentInRequest && this.CustomsDocument.DocumentStatusCode != "7" && this.CustomsDocument.DocumentStatusCode != "1") {
                        this.IsDocumentTypeEnabled = false;
                        this.IsEditEnabled = true;
                        this.IsMetaDataEditEnabled = true;
                        this.IsSendDocumentEnabled = true;
                        this.ViewDisableMessageVisibility = false;
                    }
                    else {
                        this.IsEditEnabled = false;
                        this.IsMetaDataEditEnabled = false;
                        this.IsSendDocumentEnabled = false;
                        this.IsDocumentTypeEnabled = false;
                        this.ViewDisableMessageVisibility = true;
                        this.DisplayOnlyMessage = " המסך לתצוגה בלבד - סטטוס המסמך " + this.CustomsDocument.DocumentStatusName;
                    }
                }
            }
        }
        else {
            if (this.CustomsDocumentsTicket != null) {
                if (!AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId)) {
                    this.IsMetaDataEditEnabled = true;
                    this.IsSendDocumentEnabled = true;
                    this.IsEditEnabled = false;
                    this.IsDocumentTypeEnabled = false;


                }
                else {
                    this.IsMetaDataEditEnabled = true;
                    this.IsSendDocumentEnabled = true;
                    this.IsEditEnabled = true;
                    this.IsDocumentTypeEnabled = true;
                }
            }

        }
        //-----mohamma bug 34085***********

        if (this.CustomsDocumentsTicket == null) {
            this.IsDocumentTypeEnabled = true;

        }

        // Mohammad- task 31851 ****************
        if (this.CustomsDocumentsTicket && this.CustomsDocument) {
            if (!AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId) && this.CustomsDocument.DocumentStatusCode == '3') {
                this.IsSendDocumentEnabled = true;
            }
        }
        if (this.CustomsDocument) {
            if (this.CustomsDocument.DocumentStatusCode == '4' || this.CustomsDocument.DocumentStatusCode == '8') {
                this.IsMetaDataEditEnabled = false;
                this.IsSendDocumentEnabled = false;
                this.IsEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
            }
        }
        //******************

        //*********************** task 32398********************//

        if (this.CustomsDocument) {
            var one = "1";
            var seven = "7";
            var statusCodes = ['1', '7'];
            if (statusCodes.indexOf(this.CustomsDocument.DocumentStatusCode) > -1 && !AppTool.IsNullOrEmpty(this.CustomsDocument.CustomsDocId)
                && this.CustomsDocumentsTicket && AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId)
                && !this.IsEntityDisplayOnly && this.CustomsDocument.CurrentEntityId == this.ParentEntityId) {
                this.IsActionButtonsEnabled = true;
            }
            else {
                if (this.CustomsDocument.DocumentStatusCode == '2') {
                    this.IsActionButtonsEnabled = true;
                } else {
                    this.IsActionButtonsEnabled = false;
                }
            }
        }

        //*********************************************************//

        this.UIProperties.SetEnabled("DocumentTypeCode", "Customs.CustomsDocument", this.IsDocumentTypeEnabled);
    }

    CheckEditEnabled(isCustomsDocumentInRequest: boolean, isEditEnabled: boolean) {
        // default values
        this.IsEditEnabled = true;
        this.IsMetaDataEditEnabled = true;
        this.IsDocumentTypeEnabled = true;
        this.IsSendDocumentEnabled = true;
        this.IsPointerChangeEnabled = true;
        this.ViewDisableMessageVisibility = false;
        this.DisplayOnlyMessage = null;
        this.IsActionButtonsEnabled = false;

        var isSendWithCustomsDocId: boolean = false;
        if (this.ParentEntityCode == "CustomsCollateral") {
            isSendWithCustomsDocId = true;
        }

        /// moved by mohammad to here because of maintenance board: 48882 CALL#325385 yaron saw the code too.
        if (this.CustomsDocument) {
            if (this.CustomsDocument.CustomsDocId && !isSendWithCustomsDocId) {
                this.IsMetaDataEditEnabled = false;
                this.IsSendDocumentEnabled = false;
            }
        }
        /// document varification
        if (this.CustomsDocumentsTicket) {
            if (this.CustomsDocumentsTicket.VerificationStatusTypeCode == '4' || this.CustomsDocumentsTicket.VerificationStatusTypeCode == '5') {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
                this.ViewDisableMessageVisibility = true;
                this.DisplayOnlyMessage = " לתצוגה בלבד - מסמך כבר אומת על ידי המכס ";//Document Was already Verified By Customs
            }

            if (this.CustomsDocumentsTicket.VerificationStatusTypeCode == '8') {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
                this.ViewDisableMessageVisibility = true;
                this.DisplayOnlyMessage = " לתצוגה בלבד - המסמך בתהליך אימות במכס ";//Document is in verfication prgress
            }

            if (this.CustomsDocumentsTicket.VerificationStatusTypeCode == '6') {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
                this.ViewDisableMessageVisibility = true;
                this.DisplayOnlyMessage = " לתצוגה בלבד - המסמך נדחה על ידי המכס ";//Document was deneid
            }

            if (this.IsEntityDisplayOnly) { // declaration display only
                this.IsPointerChangeEnabled = false;
                this.IsEditEnabled = false;
                //this.IsSendDocumentEnabled = false;
                //this.IsDocumentTypeEnabled = false;
            }
        }
        //Display only Logic - (Task 35024)
        if (this.CustomsDocumentsTicket && this.CustomsDocument) {
            var entitySpecialCondition = this.iCustomsDocumentsController.GetAddEditDocumentsEntitySpecialCondition();
            if ((this.IsEntityDisplayOnly || !entitySpecialCondition) && AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId)) {//(Regular doc in a paid declaration)
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
            }
            if (AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId) && !AppTool.IsNullOrEmpty(this.CustomsDocument.CustomsDocId) && !isSendWithCustomsDocId) {//(Regular doc that was already sent to customs)
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
            }

            if (!AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId) && !AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.VerificationStatusTypeCode)) {//(Requested doc that was verified/denied/in verification process)
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
            }

            if (!AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId) && AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.VerificationStatusTypeCode)) {//Requested Doc that wasn't send to customs for verification
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = true;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = true;
            }
            if (this.CustomsDocument.DocumentStatusCode == '7') {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                this.IsMetaDataEditEnabled = false;
                this.IsDocumentTypeEnabled = false;
                this.IsSendDocumentEnabled = false;
            }
            if (!entitySpecialCondition || this.CustomsDocument.DocumentStatusCode == '7' || this.IsEntityDisplayOnly) {
                this.IsEditEnabled = false;
                this.IsPointerChangeEnabled = false;
                //this.IsSendDocumentEnabled = false;
                //this.IsDocumentTypeEnabled = false;
            }
            else {
                this.IsEditEnabled = true;
                this.IsPointerChangeEnabled = true;
            }
        }
        //*********************** task 32398 new version button********************//

        if (this.CustomsDocument) {
            var statusCodes = ['1', '7'];
            if (statusCodes.indexOf(this.CustomsDocument.DocumentStatusCode) > -1 && !AppTool.IsNullOrEmpty(this.CustomsDocument.CustomsDocId)
            && this.CustomsDocumentsTicket && AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket.RequestedCustomsDocId)
            && !this.IsEntityDisplayOnly) {
                new CustomsDocumentsTicketsExtendedService().GetDocConnectTicket(this.CustomsDocument.DocumentsFilingId, this.ParentEntityId, SessionLocator.Tenant).subscribe((response: ServiceResponse) => {
                    const anotherDeclarationConnect: string[] = response.Result.decConnect;

                    if (anotherDeclarationConnect.length === 0)
                        this.IsActionButtonsEnabled = true;
                })            
            }
            else {
                if (this.CustomsDocument.DocumentStatusCode == '2') {
                    this.IsActionButtonsEnabled = true;
                }
                else {
                    if (this.CustomsDocument.DocumentStatusCode == '7') {
                        let objecttable: any = window.ObjectTables.filter(d => d.Name == "Customs.CustomsDocument")[0];
                        var ser = new CustomsRequestSheetExtendedPMService();
                        ser.GetGeneralRequestInProgress("2715", objecttable.Id, this.CustomsDocument.DocumentsFilingId, SessionLocator.Tenant)
                            .subscribe((rsp: any) => {
                                var myCustomsRequestsSheet = rsp.Result;
                                this.CurrentSession.StopBusyIndicator();
                                if (myCustomsRequestsSheet == null || myCustomsRequestsSheet.length == 0) {
                                    this.IsActionButtonsEnabled = true;
                                }
                                else {
                                    this.IsActionButtonsEnabled = false;
                                }
                            });
                    }
                    else {
                        this.IsActionButtonsEnabled = false;
                    }
                }
            }
        }

        //*********************************************************//
        if(this.isRequireDocumentTicket){
            this.IsDocumentTypeEnabled = false;
        }
        this.UIProperties.SetEnabled("DocumentTypeCode", "Customs.CustomsDocument", this.IsDocumentTypeEnabled);
    }

    updateCustomsDocument: boolean = false;
    OkButtonClicked() {
        this.OkMethod(false);
    }

    SendButtonClicked() {
       
       
            this.OkMethod(true);
        
    }

    OkMethod(isSendToQueue: boolean) {
        var errors = [];

        if (AppTool.IsNullOrEmpty(this.DocumentTypeCode) && this.EntityPM.Direction == 'E') {
            this.CurrentSession.StopBusyIndicator();
            var messageWindow = new MessageWindow();
            messageWindow.Width = 400;
            messageWindow.Height = 200;
            messageWindow.RTL=true;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.MustCustomDocumentType"));
            messageWindow.WindowClosed.subscribe((event: any) => {

                messageWindow.Close();

            });
            return;
        }
        if (this.CustomsDocument) {
            Validator.TryValidateObject(this.CustomsDocument, "Customs.CustomsDocument", errors);
            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
                return;
            }
        }
        if (this.CustomsDocumentsTicket) {
            Validator.TryValidateObject(this.CustomsDocumentsTicket, "Customs.CustomsDocumentsTicket", errors);
            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
                return;
            }
        }

        if (this.connectTo == null)
            this.connectTo = this.SelectedIndex;

        var err = this.iCustomsDocumentsController.ValidationBeforeSave(this.CustomsDocumentsTicket, this.connectTo.toString());
        if (!AppTool.IsNullOrEmpty(err)) {
            this.ValidationErrorsList = [];

            this.ValidationErrorsList.push(err);
            return;
        }

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));
        if (this.ViewDisableMessageVisibility) {
            this.CancelButtonClicked();
            return;
        }

        var requiredFieldsErrors: string[] = [];

        if (this.CustomsDocument != null) {
            requiredFieldsErrors = this.GetRequiredFieldsErrors();
            this.MetaDataViewModels.forEach((viewModel) => {
                var value = this.CustomsDocument.CustomsDocumentMetaDataValues.filter(d => d.MetaDataTypeCode == viewModel.MetaDataValue.MetaDataTypeCode)[0];
                if (!value) {
                    this.CustomsDocument.AddCustomsDocumentMetaDataValue(viewModel.MetaDataValue);
                }
            });

            this.CustomsDocument.IsSendToQueue = isSendToQueue;

            let updateByController = true;
            if (!updateByController) {

                if (AppTool.IsNullOrEmpty(this.CustomsDocument.DeclarationId)) {
                    this.CustomsDocument.DeclarationId = this.ParentEntityId;
                }

                if (AppTool.IsNullOrEmpty(this.CustomsDocument.DeclarationId)) {
                    this.CurrentSession.StopBusyIndicator();
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 400;
                    messageWindow.Height = 200;
                    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                    messageWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DocumentAndCustomDocumentType"));
                    messageWindow.WindowClosed.subscribe((event: any) => {

                        messageWindow.Close();

                    });
                }

            } else {
                this.iCustomsDocumentsController.UpdateCustomsDocumentb4Send(this.CustomsDocument)
            }
            //throw new Exception("Do not send  CustomsDocumentPM  without  parentEntityId !!!");

        }

        if (this.CustomsDocumentsTicket) {
            if (this.CustomsDocument != null) {
                this.updateCustomsDocument = true;
                this.CustomsDocument.CurrentCustomsDocumentsTicketId = this.CustomsDocumentsTicket.Id;
            }
            if (this.IsTicketChanged || this.IsNewState) {
                // Validator.TryValidateObject(CustomsDocumentsTicket, new ValidationContext(CustomsDocumentsTicket), errors);
                if (this.CustomsDocumentsTicket.CustomsDocumentPointers.length == 0) {
                    var newPointer: CustomsDocumentPointerPM = new CustomsDocumentPointerPM(this.CustomsDocumentsTicket)

                    newPointer.Tenant = SessionLocator.Tenant;
                    newPointer.ParentEntityId = this.ParentEntityId;
                    newPointer.ParentEntityCode = this.ParentEntityCode;
                    newPointer.Child1EntityCode = null;
                    newPointer.Child2EntityCode = null;
                    newPointer.Child3EntityCode = null;
                    newPointer.Child1EntityId = null;
                    newPointer.Child2EntityId = null;
                    newPointer.Child3EntityId = null;
                    newPointer.DocumentTypeCode = this.CustomsDocumentsTicket.DocumentTypeCode;

                    this.CustomsDocumentsTicket.AddCustomsDocumentPointer(newPointer);
                }
                else {
                    this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach((newPointer) => {
                        newPointer.DocumentTypeCode = this.CustomsDocumentsTicket.DocumentTypeCode;
                    });

                }
                this.InsertNewTicket();
                return;
            }
        }
        // for required meta data
        //string requiredFieldsWarning = "";
        //foreach(ValidationResult result in requiredFieldsErrors)
        //{
        //    if (string.IsNullOrEmpty(requiredFieldsWarning)) {
        //        requiredFieldsWarning = result.ErrorMessage;
        //    }
        //    else {
        //        requiredFieldsWarning = requiredFieldsWarning + Environment.NewLine + result.ErrorMessage;
        //    }

        //}
        var requiredFieldsWarning = "";
        requiredFieldsErrors.forEach((error) => {
            if (AppTool.IsNullOrEmpty(requiredFieldsWarning)) {
                requiredFieldsWarning = error;
            }
            else {
                requiredFieldsWarning = requiredFieldsWarning + '\n' + error;
            }
        });

        if (requiredFieldsErrors.length == 0) {
            if (this.CustomsDocument != null) {
                this.CustomsDocument.IsMetaDataReady = true;
            }
            this.PerformSubmitChanges();
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.CustomsDocument.IsMetaDataReady = false;

            //here we pop a confirm window to ask weather to continue or not if there is required fields errors.
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Height = 200;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.ShowNoButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
            confirmWindow.Show(requiredFieldsWarning);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));
                    this.PerformSubmitChanges();
                    confirmWindow.Close();
                }
                if (confirmWindow.No) {
                    confirmWindow.Close();
                }

            });
        }

    }

    CancelButtonClicked() {
        if (this.CustomsDocument) {
            this.CustomsDocument.RejectChanges();
        }
        if (this.CustomsDocumentsTicket) {
            this.CustomsDocumentsTicket.RejectChanges();
        }
        if (this.newVersionAdded) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");

        }
        else {
            this.CurrentSession.CloseCurrentWindowEmit("cancel");
        }
    }

    InsertNewTicket() {
        var customsDocumentsTicketPMService: CustomsDocumentsTicketPMService = new CustomsDocumentsTicketPMService();
        customsDocumentsTicketPMService.insert(this.CustomsDocumentsTicket).subscribe((resp: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!resp.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
        });
    }

    FillConnectedToItems() {
        this.ConnectedToItems = this.iCustomsDocumentsController.FillConnectedToItems();
        //if (entityCode.toLowerCase() == "declaration") {
        //    var connectedItem1 = new ConnectedToItem();
        //    connectedItem1.Id = 0;
        //    connectedItem1.Name = TextCodeTranslator.Translate("Customs.Declaration");

        //    var connectedItem2 = new ConnectedToItem();
        //    connectedItem2.Id = 1;
        //    connectedItem2.Name = TextCodeTranslator.Translate("Customs.SupplierInvoice");

        //    var connectedItem3 = new ConnectedToItem();
        //    connectedItem3.Id = 2;
        //    connectedItem3.Name = TextCodeTranslator.Translate("Customs.SupplierInvoiceItem");

        //    this.ConnectedToItems.push(connectedItem1);
        //    this.ConnectedToItems.push(connectedItem2);
        //    this.ConnectedToItems.push(connectedItem3);
        // }
        // else if (entityCode.toLowerCase() == "claim") {
        //    this.ConnectedToItems.push(new ConnectedToItem() { Id = 0, Name = TextCodeTranslator.Translate("Customs.Claim") });
        //    this.ConnectedToItems.push(new ConnectedToItem() { Id = 1, Name = TextCodeTranslator.Translate("Customs.ClaimsRelatedEntity") });
        // }
        // else if (entityCode.toLowerCase() == "customscollateral") {
        //    this.ConnectedToItems.push(new ConnectedToItem() { Id = 0, Name = TextCodeTranslator.Translate("Customs.CustomsCollateral") });
        // }

    }

    FillConnectedDocumentPointer() {
        let connectedDocumentPointer: any = this.iCustomsDocumentsController.FillConnectedDocumentPointer(this.CustomsDocumentsTicket.CustomsDocumentPointers[0]);
        if (connectedDocumentPointer != null) {
            this.SelectedIndex = connectedDocumentPointer.SelectedIndex;
            if (this.EntityPM.Direction == 'E') {
                if (this.DocumentTypeCode == "IL_140") {
                    this.SelectedIndex = 1;
                    if (this.EntityPM.SupplierInvoices[0] != null) {
                        this.CustomsDocumentsTicket.ConnectedInvoicesSequences = this.EntityPM.SupplierInvoices[0].SequenceNumeric;
                    }
                }
            }
            this.DisplayConnectedEntityNumber = connectedDocumentPointer.DisplayConnectedEntityNumber;
            if (this.ParentEntityCode == "Declaration" && connectedDocumentPointer.SupplierInvoiceItemNumber != null) {
                this.SupplierInvoiceItemNumber = connectedDocumentPointer.SupplierInvoiceItemNumber;
            }
        }
    }

    SetDefaultConnectedEntityNumber() {
        this.iCustomsDocumentsController.SetDefaultConnectedEntityNumber(this.CustomsDocumentsTicket, this.EntityPM);
    }


    connectTo: number;

    ConnectedItemSelectionChanged(index: number) {
        if (index != 0) {
            var selectInvoicesOnly = true;
            if (index == 2) {
                selectInvoicesOnly = false;
            }

            this.connectTo = index;
            this.SelectedIndex = index;
            this.ShowSelectionComponent(selectInvoicesOnly);

        }


    }

    //if(index != 0) {
    //    bool selectInvoicesOnly = true;
    //    if (index == 2) {
    //        selectInvoicesOnly = false;
    //    }

    //    SimplogWindow window = new SimplogWindow();
    //    window.Height = 700;
    //    window.Width = 1000;
    //    window.ShowCloseButtonOnly = true;
    //    if (this.objectTableName == "Customs.Declaration") {
    //        CustomsDocumentPointerChildSelectionViewModel viewModel = new CustomsDocumentPointerChildSelectionViewModel(selectInvoicesOnly, this.context, window, CustomsDocumentsTicket, null, parentEntityId, entityChild1Id, entityChild2Id, entityChild3Id);
    //        viewModel.ChildrenChoosingCompleted += viewModel_ChildrenChoosingCompleted;
    //        CustomsDocumentPointerChildSelectionControl control = new CustomsDocumentPointerChildSelectionControl() { DataContext = viewModel };
    //        window.Add(control);
    //        window.Show();
    //    }
    //    else if (this.objectTableName == "Customs.Claim") {
    //        ClaimRelatedEntitiesPointersSelectionViewModel CREViewModel = new ClaimRelatedEntitiesPointersSelectionViewModel(new ClaimDomainContext(), window, CustomsDocumentsTicket, parentEntityId, entityChild1Id, entityChild2Id, entityChild3Id);
    //        CREViewModel.ChildrenChoosingCompleted += CREViewModel_ChildrenChoosingCompleted;
    //        ClaimRelatedEntitiesPointersSelectionControl control = new ClaimRelatedEntitiesPointersSelectionControl() { DataContext = CREViewModel };
    //        window.Height = 500;
    //        window.Width = 800;
    //        window.Add(control);
    //        window.Show();
    //    }


    //}

    ShowSelectionComponent(selectInvoicesOnly: boolean = false) {
        if (!AppTool.IsNullOrEmpty(this.CustomsDocumentsTicket)) {
            this.iCustomsDocumentsController.SelectionCompleted.subscribe(s => {
                this.SelectionCompleted(s);
            });
            this.iCustomsDocumentsController.ShowSelectionComponent(this.CustomsDocumentsTicket, this.EntityPM, selectInvoicesOnly, this.IsEntityDisplayOnly, this.SelectedIndex);

        }

    }
 
    u

    RefereshConnectedInvoices() {
        if (this.CustomsDocumentsTicket != null) {
            this.SupplierInvoiceItemNumber = this.CustomsDocumentsTicket.ConnectedInvoiceItemsSequences;
            this.SupplierInvoiceNumber = this.CustomsDocumentsTicket.ConnectedInvoicesSequences;
            this.CRENumber = this.CustomsDocumentsTicket.ConnectedCREsSequences;
            this.DisplayConnectedEntityNumber = this.CustomsDocumentsTicket.ConnectedInvoicesSequences != null ? this.CustomsDocumentsTicket.ConnectedInvoicesSequences : this.CustomsDocumentsTicket.ConnectedCREsSequences;
        }
    }

    //#region Tabs Code
    TabsSource: any[] = [];
    SelectedTab: string = "";

    BuildTabs() {
        this.TabsSource = [];
        if (!this.IsUserTabSelected) {
            this.SelectedTab = "CustomsDocumentRemarks";
        }
        else {
            this.SelectedTab = "UserRemarks";
        }
        if (this.RemarksTextBoxVisiblity) {
            this.TabsSource.push({ Name: "CustomsDocumentRemarks", isSelected: true, Header: TextCodeTranslator.Translate("Customs.Declaration.O.CustomsDocumentRemarks") });
        }
        else if (this.SelectedTab != "UserRemarks") {
            this.SelectedTab = "VerificationRemarks"
        }
        if (this.VerificationRemarksTextBoxVisiblity) {
            this.TabsSource.push({ Name: "VerificationRemarks", isSelected: false, Header: TextCodeTranslator.Translate("Customs.CustomsDocumentsTicket.F.VerificationRemarks") });
        }

        if (this.UserRemarksTextBoxVisiblity) {
            this.TabsSource.push({ Name: "UserRemarks", isSelected: this.IsUserTabSelected, Header: TextCodeTranslator.Translate("Customs.CustomsDocumentsTicket.F.UserRemarks") });
        }
    }
    SelectionChanged(tab: any) {

        this.TabsSource.forEach(item => { // reset selection
            item.isSelected = false;
        });

        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab); return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    }
    //#endregion

    private _SendCustomsDocumentAfter = false;//IT SEEMS THAT ITS OK IN THE DIST =- SOO REVERT MY CODE 
    PerformSubmitChanges() {
        if (!this._SendCustomsDocumentAfter && this.CustomsDocument) {
            this.SendCustomsDocumentMethod();
        }
        else {
            this.SubmitTicketChanges();
        }
    }

    private SendCustomsDocumentMethod() {
        if (this.CustomsDocument.IsDirty) {
            var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
            customsDocumentPMService.update(this.CustomsDocument).subscribe((docRes: ServiceResponse) => {
                if (!docRes.HasError) {
                    if (!this._SendCustomsDocumentAfter) {
                        this.SubmitTicketChanges();
                    } else {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }


                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                    if (docRes.ErrorsArray && docRes.ErrorsArray.length > 0) {
                        this.ValidationErrorsList = docRes.ErrorsArray;
                    }
                    //let jDoit = false;
                    //if (jDoit && !AppTool.IsNullOrEmpty(docRes.ErrorsArray[0])) {//in customsDocumentPMService.update there is message : לא נמצא כרטיס חתימה חברתי (מסר 2715)
                    //    this.CurrentSession.StopBusyIndicator();
                    //    var messageWindow = new MessageWindow();
                    //    messageWindow.Width = 400;
                    //    messageWindow.Height = 200;
                    //    messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                    //    messageWindow.Show(docRes.ErrorsArray[0]);
                    //    messageWindow.WindowClosed.subscribe((event: any) => {
                    //        messageWindow.Close();
                    //    });
                    //}
                }
            });
        }
    }

    SubmitTicketChanges() {

        if (this.CustomsDocumentsTicket) {
            if (this.CustomsDocumentsTicket.IsDirty) {
                var customsDocumentsTicketPMService: CustomsDocumentsTicketPMService = new CustomsDocumentsTicketPMService();
                customsDocumentsTicketPMService.update(this.CustomsDocumentsTicket).subscribe((ticketRes: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (!ticketRes.HasError) {
                        if (!this._SendCustomsDocumentAfter) {
                            this.CurrentSession.CloseCurrentWindowEmit("ok");
                        } else {
                            this.SendCustomsDocumentMethod();
                        }
                    }
                    else {
                        var messageWindow = new MessageWindow();
                        messageWindow.Width = 400;
                        messageWindow.Height = 200;
                        messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                        messageWindow.Show(ticketRes.ErrorsArray[0]);
                        messageWindow.WindowClosed.subscribe((event: any) => {

                            messageWindow.Close();

                        });
                    }
                });
            }
            else {
                if (!this._SendCustomsDocumentAfter) {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                } else {
                    this.SendCustomsDocumentMethod();
                }
            }
        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    InitializeMetaData(previousList: CustomsDocumentMetaDataValuePM[]) {
        this.previousValueList = previousList;
        var custDocTypeMetaDataWebService: CustDocTypeMetaDataWebService = new CustDocTypeMetaDataWebService();
        var customDocumentTypeListService: CustomDocumentTypeListService = new CustomDocumentTypeListService();
        var _DocumentTypeMetaDataExtendedService: DocumentTypeMetaDataExtendedService = new DocumentTypeMetaDataExtendedService();
        custDocTypeMetaDataWebService.GetCustomDocumentTypeMetaDataByType(this.CustomsDocument.DocumentTypeCode).subscribe((res: ServiceResponse) => {
            this.customDocumentTypeMetaDataList = res.Result;
            this.customDocumentMetaDataValueList = this.CustomsDocument.CustomsDocumentMetaDataValues;
            var customsClosedTableListService: CustomsClosedTableListService = new CustomsClosedTableListService();
            customsClosedTableListService.getAll().subscribe((resp: ServiceResponse) => {
                this.customsClosedTableList = resp.Result;
                customDocumentTypeListService.getSingle(this.CustomsDocument.DocumentTypeCode).subscribe((docTypeRes: ServiceResponse) => {
                    if (this.previousValueList != null) {
                        this.customDocumentTypeMetaDataList.forEach((metaData) => {
                            if (!AppTool.IsNullOrEmpty(this.customDocumentMetaDataValueList) && this.customDocumentMetaDataValueList.length > 0) {
                                var value: CustomsDocumentMetaDataValuePM = this.customDocumentMetaDataValueList.filter(d => d != null && d.MetaDataTypeCode == metaData.MetaDataTypeCode)[0];
                                if (value == null) {


                                    _DocumentTypeMetaDataExtendedService.GetDocumentsFilingMetaDataValueByFilingIdAndCode(this.CustomsDocument.DocumentsFilingId, metaData.MetaDataTypeCode)
                                        .subscribe(myDocFilingResult => {

                                            value = new CustomsDocumentMetaDataValuePM(this.CustomsDocument);
                                            value.MetaDataTypeCode = metaData.MetaDataTypeCode;
                                            value.Tenant = SessionLocator.Tenant;
                                            value.CustomsDocumentId = this.CustomsDocument.DocumentsFilingId;
                                            value.ChangeSetOp = "Insert";
                                            if (docTypeRes.Result) {
                                                if (docTypeRes.Result.AutoSetOriginalDocumentTrue) {
                                                    if (value.MetaDataTypeCode == "87") {
                                                        value.MetaDataValue = "True";
                                                    }
                                                }
                                            }
                                            if (myDocFilingResult.Result && myDocFilingResult.Result.length > 0) {
                                                if (AppTool.IsNullOrEmpty(value.MetaDataValue) && !AppTool.IsNullOrEmpty(myDocFilingResult.Result.MetaDataValue)) {
                                                    value.MetaDataValue = myDocFilingResult.Result.MetaDataValue;
                                                }
                                            }
                                            else {

                                            }
                                        });


                                    if (value != null)
                                        this.customDocumentMetaDataValueList.push(value);
                                }
                            }
                        });
                        this.SetCommonMetaDataValues(this.previousValueList, docTypeRes.Result);
                    }
                    else {
                        this.GenerateControl(docTypeRes.Result);
                    }
                });
            });
        });
    }

    SetCommonMetaDataValues(previousValues: CustomsDocumentMetaDataValuePM[], docType: any) {
        previousValues.forEach((valuePM) => {
            var newValue = this.customDocumentMetaDataValueList.filter(d => d != null && d.MetaDataTypeCode == valuePM.MetaDataTypeCode)[0];
            if (newValue != null) {
                newValue.MetaDataValue = valuePM.MetaDataValue;
            }
        });

        this.GenerateControl(docType);
    }

    GenerateControl(docType: any) {
        this.MetaDataViewModels = [];
        this.customDocumentTypeMetaDataList = this.customDocumentTypeMetaDataList.sort((a, b) => {
            return (a.Mandatory === b.Mandatory) ? 0 : (a.Mandatory < b.Mandatory) ? 1 : -1
        });
        this.customDocumentTypeMetaDataList.forEach((type) => {
            var value: CustomsDocumentMetaDataValuePM = this.customDocumentMetaDataValueList.filter(d => d != null && d.MetaDataTypeCode == type.MetaDataTypeCode)[0];

            var metaDataViewModel = new MetaDataViewModel(type, value, this.CustomsDocument, this.customsClosedTableList, docType);
            this.MetaDataViewModels.push(metaDataViewModel);

        });

        if (this.EntityPM.Direction == "E" && (this.EntityPM.transportModeId == "O" || this.EntityPM.transportModeId == "A") && (docType.Code == "707" || docType.Code == "419")) {
            var FinalCargoTypeCode = this.MetaDataViewModels.find(x => x.MetaDataType.MetaDataTypeCode == "99")
            var FinalManifestNumber = this.MetaDataViewModels.find(x => x.MetaDataType.MetaDataTypeCode == "100")
            if (AppTool.IsNullOrEmpty(FinalCargoTypeCode.MetaDataValue.MetaDataValue)) FinalCargoTypeCode.MetaDataValue.MetaDataValue = this.ClosingData.FinalCargoTypeCode;
            if (AppTool.IsNullOrEmpty(FinalManifestNumber.MetaDataValue.MetaDataValue)) FinalManifestNumber.MetaDataValue.MetaDataValue = this.ClosingData.FinalManifestNumber;

        }

    }

    GetRequiredFieldsErrors() {
        var requiredFieldsErrors = [];
        this.MetaDataViewModels.forEach((viewModel) => {
            var metaDataType = this.customDocumentTypeMetaDataList.filter(d => d.MetaDataTypeCode == viewModel.MetaDataType.MetaDataTypeCode)[0];
            if (metaDataType.Mandatory) {
                if (AppTool.IsNullOrEmpty(viewModel.MetaDataValue.MetaDataValue)) {
                    requiredFieldsErrors.push(metaDataType.MetaDataTypeName + " " + TextCodeTranslator.Translate("Customs.General.O.IsRequired"));
                }
            }
        });
        return requiredFieldsErrors;
    }

    SelectionCompleted(args) {

        this.RefereshConnectedInvoices();
    }

    ClearAllMetaDataValues() {
        this.previousValueList = [];
        if (this.customDocumentMetaDataValueList! = null)
            this.customDocumentMetaDataValueList.forEach((value) => {
                this.previousValueList.push(value);
            });
        this.previousValueList.forEach((value) => {
            var exists = this.CustomsDocument.CustomsDocumentMetaDataValues.filter(d => d.MetaDataTypeCode == value.MetaDataTypeCode)[0];
            if (exists) {
                this.CustomsDocument.RemoveCustomsDocumentMetaDataValue(exists);

            }
        });

    }

    NewVersion() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Height = 200;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        confirmWindow.ShowNoButton = true;
        confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.CustomsDocuments.NewVersionWarning"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CustomsDocument.DocumentVersion = this.CustomsDocument.DocumentVersion + 1;
                this.CustomsDocument.DocumentStatusCode = null;
                this.CustomsDocument.CustomRecievedDate = null;
                this.CustomsDocument.CustomsDocId = null;
                this.CustomsDocument.ForceRemoveCustomsDocId = true;
                confirmWindow.Close();
                this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Customs.General.O.Saving"));
                var customsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
                customsDocumentPMService.update(this.CustomsDocument).subscribe((docRes: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    if (!docRes.HasError) {
                        // this.CheckEditEnabled(this.IsCustomsDocumentInRequest, !this.IsDisplayOnly);

                        this.WindowArgs.CustomsDocument = docRes.Result;
                        this.SetWindowArgs(this.WindowArgs);
                        this.newVersionAdded = true;
                    }
                    else {
                        this.ValidationErrorsList = docRes.ErrorsArray;
                    }
                });


            }
            else {
                confirmWindow.Close();
            }
        });
    }

    OnTextAreaKeyDown(event) {

        event.preventDefault();

    }

}

export class MetaDataViewModel extends BaseComponent {

    ValuesTableName: string;
    controlType: string;

    private dateMetaDataValue: any;
    public get DateMetaDataValue() {
        return this.dateMetaDataValue;
    }
    public set DateMetaDataValue(newValue: any) {

        this.SetDateValue(newValue);
    }

    constructor(public MetaDataType: CustomDocumentTypeMetaDataPM,
        public MetaDataValue: CustomsDocumentMetaDataValuePM, public CustomsDocument: CustomsDocumentPM, public closedTables: CustomsClosedTableList[], docType: any) {
        super();
        if (!this.MetaDataValue) {
            this.MetaDataValue = new CustomsDocumentMetaDataValuePM(CustomsDocument);

            this.MetaDataValue.CustomsDocumentId = this.CustomsDocument.DocumentsFilingId;
            this.MetaDataValue.MetaDataTypeCode = this.MetaDataType.MetaDataTypeCode;
            this.MetaDataValue.Tenant = SessionLocator.Tenant;
            if (docType && docType.AutoSetOriginalDocumentTrue && this.MetaDataValue.MetaDataTypeCode == "87") {
                this.MetaDataValue.MetaDataValue = "True";
            }
        }
        if (MetaDataType.ValuesTable) {
            var currentClosedTable: CustomsClosedTableList = this.closedTables.filter(d => d.Id == MetaDataType.ValuesTable)[0];
            if (currentClosedTable) {
                var lookUpTable = window.ObjectTables.filter(d => d.Id === currentClosedTable.ObjectTableId)[0];
                this.ValuesTableName = lookUpTable.Name;
            }
            else {
                this.MetaDataType.ValuesTable = null;
            }
        }


        switch (MetaDataType.Format.toLocaleLowerCase()) {
            case "string": {
                if (this.MetaDataType.ValuesTable) {
                    this.controlType = 'loglov';
                }
                else {
                    this.controlType = 'logtextbox';
                }
                break;
            }
            case "int":
                {
                    if (this.MetaDataType.ValuesTable) {
                        this.controlType = 'loglov';
                    }
                    else {
                        this.controlType = 'logtextbox';
                    }
                    break;
                }
            case "date":
                {
                    this.GetDateValue();
                    this.controlType = 'datepicker';
                    break;
                }

            case "boolean":
                {
                    this.controlType = 'boolean';
                    break;
                }
        }
    }

    GetDateValue() {
        if (!AppTool.IsNullOrEmpty(this.MetaDataValue.MetaDataValue)) {
            var date: Date;
            var dateArray: string[] = this.MetaDataValue.MetaDataValue.split('.');

            var day = Number(dateArray[0]);
            var month = Number(dateArray[1]);
            var year = Number(dateArray[2]);
            var nowDate: Date = new Date();
            var currentYear = nowDate.getFullYear();
            var currentYearMillinium = currentYear.toString().substring(0, 1);
            currentYearMillinium = currentYearMillinium + "000";
            var currentMillinium = Number(currentYearMillinium);
            if (year == 0) {
                year = currentYear;
            }
            if (year < 1000) {
                year = year + currentMillinium;
            }

            this.dateMetaDataValue = year + "." + this.ApplyPadding(month + "") + "." + this.ApplyPadding(day + "");
        }
    }

    SetDateValue(newValue: any) {
        if (newValue instanceof Date) {
            var valueDate: Date = newValue;
            var day = valueDate.getUTCDate();
            var month = valueDate.getUTCMonth() + 1;
            var year = valueDate.getUTCFullYear();

            var shortYear = (year + "").substr(2, 2);

            this.MetaDataValue.MetaDataValue = this.ApplyPadding(day + "") + "." + this.ApplyPadding(month + "") + "." + shortYear
        }

    }

    ApplyPadding(str: string) {
        var pad = "00"
        var ans = pad.substring(0, pad.length - str.length) + str
        return ans;
    }



}

