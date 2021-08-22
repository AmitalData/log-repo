declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef, Output, EventEmitter}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {SendRequestVIA} from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';

import {CustomsVendorPM} from '../../../../../Customs/EntityPMs/CustomsVendorPM';
import {VendorCommunicationPM} from '../../../../../Customs/EntityPMs/VendorCommunicationPM';

import {CustomsHouseTypeExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import {VendorInsertUpdateDeleteMessageRequestParams, OperationTypes} from '../../../../../Customs/DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import { CustomMessageProgressComponent, ShowProgressBarParams } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {VendorMessagesService} from '../../../../../Customs/Services/WebServices/VendorMessagesService';
import {CustomsVendorPMService} from '../../../../../Customs/Services/StandardPMs/CustomsVendorPMService';


@Component({
    
    templateUrl: './VendorGeneralTabComponent.html',
})

export class VendorGeneralTabComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public EntityPM: CustomsVendorPM;
    public ObjectTableName: string = "Customs.CustomsVendor";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValdationErrorList: any[];
    public SubCountryCodeEnabled: boolean = false;
    IsDelete: boolean = false;
    CommunicationsList: ObservableCollection;
    RequestParams: VendorInsertUpdateDeleteMessageRequestParams;
    ResponseData: INF_MSG_GenericResponseData;
    vendorMessagesService: VendorMessagesService = new VendorMessagesService();
    customsVendorPMService: CustomsVendorPMService = new CustomsVendorPMService();

    RequestVIA: SendRequestVIA;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.CommunicationsList = new ObservableCollection([]);

    }

    SetTabArgs(args: any, valdationErrorList: any[]) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;

        console.log("EntityPM", this.EntityPM);

        this.FillGridData();
        this.SetFieldsEditability();
    }

    FillGridData() {
        // Communications List
        this.CommunicationsList = new ObservableCollection([]);
        for (let item of this.EntityPM.VendorCommunications) {
            this.CommunicationsList.Insert(new CommunicationItemModel(item));
        }
    }

    SetFieldsEditability() {
        this.UIProperties.SetEnabled("VendorTypeCode", this.ObjectTableName, this.IsNewEntity);
        this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityPM.CountryCode));
    }

    //#region Properties

    public get VendorTypeCode() { return this.EntityPM.VendorTypeCode; }
    public set VendorTypeCode(newValue: string) {
        this.EntityPM.VendorTypeCode = newValue;
    }

    public get CountryCode() { return this.EntityPM.CountryCode; }
    public set CountryCode(newValue: string) {
        this.EntityPM.CountryCode = newValue;
        this.SubCountryCode = null;
        this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !AppTool.IsNullOrEmpty(newValue));

    }

    public get CityName() { return this.EntityPM.CityName; }
    public set CityName(newValue: string) {
        this.EntityPM.CityName = newValue;
    }

    public get PostalCode() { return this.EntityPM.PostalCode; }
    public set PostalCode(newValue: string) {
        this.EntityPM.PostalCode = newValue;
    }

    public get VATNumber() { return this.EntityPM.VATNumber; }
    public set VATNumber(newValue: string) {
        this.EntityPM.VATNumber = newValue;
    }

    public get VendorName() { return this.EntityPM.VendorName; }
    public set VendorName(newValue: string) {
        this.EntityPM.VendorName = newValue;
    }

    public get SubCountryCode() { return this.EntityPM.SubCountryCode; }
    public set SubCountryCode(newValue: string) {
        this.EntityPM.SubCountryCode = newValue;
    }

    public get MainAddressLine() { return this.EntityPM.MainAddressLine; }
    public set MainAddressLine(newValue: string) {
        this.EntityPM.MainAddressLine = newValue;
    }

    public get DunsNumber() { return this.EntityPM.DunsNumber; }
    public set DunsNumber(newValue: string) {
        this.EntityPM.DunsNumber = newValue;
    }

    public get TransactionTypeID() { return this.EntityPM.TransactionTypeID; }
    public set TransactionTypeID(newValue: string) {
        this.EntityPM.TransactionTypeID = newValue;
    }

    //#endregion

    line = 0;
    AddButonClicked() {
        if (this.IsNewEntity) {
            var newCommunicationPM = new VendorCommunicationPM(this.EntityPM);
            newCommunicationPM.Tenant = SessionLocator.Tenant;
            newCommunicationPM.VendorId = this.EntityPM.Id;
            newCommunicationPM.LineNumber = this.line++; // it will be override by EntityUpdateService.OnCreating() in server.

            if (!this.EntityPM.VendorCommunications.includes(newCommunicationPM)) {
                this.EntityPM.AddVendorCommunication(newCommunicationPM);
                this.CommunicationsList.Insert(new CommunicationItemModel(newCommunicationPM));
            }
        } else {
            //open send window

            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 400;
            logWindow.Title = TextCodeTranslator.Translate("Customs.CustomsVendor.O.AddVendorCommunication"); //"Add Vendor Communication";
            logWindow.WindowArgs = { VendorPM: this.EntityPM };
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "ok") {

                    this.CurrentSession.StartBusyIndicatorLoading();
                    this.customsVendorPMService.get(this.EntityPM.Id).subscribe((res: ServiceResponse) => {
                        var vendor: CustomsVendorPM = res.Result;
                        this.CurrentSession.StopBusyIndicator();
                        if (!AppTool.IsNullOrEmpty(vendor)) {
                            this.EntityPM = vendor;
                            this.FillGridData();
                            this.SetFieldsEditability();

                        } else {
                            console.log("[ERROR/VendorCommunication] empty response");
                        }

                    });

                }
            });
          logWindow.Show('./CustomsModules/CustomsVendor/Components/EditTabs/General/AddVendorCommunicationComponent');
        }
    }
    RemoveRow(item: CommunicationItemModel) {
        if (!AppTool.IsNullOrEmpty(item)) {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.Vendor.O.DeleteCommunication"));

            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.CommunicationsList.Remove(item);
                    this.EntityPM.RemoveVendorCommunication(item.CommunicationPM);
                }

            });

        }
    }

    //#region Send + Delete
    SendButtonClicked(event) {
        this.RequestVIA = event.RequestVIA;

        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs

        // validate Vendor
        Validator.TryValidateObject(this.EntityPM, "Customs.CustomsVendor", errors);

        if (this.EntityPM.VendorCommunications.length == 0) {
            errors.push(TextCodeTranslator.Translate("Customs.Vendor.O.RequierdCommunication"));
        } else {
            // validate vendor communication items
            this.EntityPM.VendorCommunications.forEach((item) => {
                Validator.TryValidateObject(item, "Customs.VendorCommunication", errors);
            });
        }

        if (errors.length > 0) {
            this.ValdationErrorList = errors;
            this.FillValidationErrorList.emit(errors);
        } else {
            // send request
            this.SendRequest(false);
        }

    }

    DeleteButtonClicked(isAfterWarning: boolean, event) {
        if (event != null)
            this.RequestVIA = event.RequestVIA;

        this.IsDelete = true;

        var operationDescription = "Delete vendor ";

        var operation = OperationTypes.Delete;

        this.CurrentSession.StartBusyIndicator("Customs.General.O.Sending");

        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.CustomsVendor")[0];

        var deleteParams = new VendorInsertUpdateDeleteMessageRequestParams();

        deleteParams.OperationType = operation;
        deleteParams.LoggingObjectTableId = ObjectTable.Id;
        deleteParams.LoggingUserId = SessionLocator.LoggedUserId;
        deleteParams.IsFakeResponse = true;
        deleteParams.LoggingEnabled = true;
        deleteParams.RequestName = "delete Vendor Request";
        deleteParams.ResponseName = "delete Vendor Response";
        deleteParams.Tenant = SessionLocator.Tenant;
        deleteParams.LoggingEntityId = this.EntityPM.Id;
        deleteParams.LoggingEntityReference = this.EntityPM.VendorNumber;
        deleteParams.VendorName = this.EntityPM.VendorName;
        deleteParams.CityName = this.EntityPM.CityName;
        deleteParams.CountryCode = this.EntityPM.CountryCode;
        deleteParams.DunsNumber = this.EntityPM.DunsNumber;
        deleteParams.MainAddressLine = this.EntityPM.MainAddressLine;
        deleteParams.PostalCode = this.EntityPM.PostalCode;
        deleteParams.StatusCode = this.EntityPM.StatusCode;
        deleteParams.SubCountryCode = this.EntityPM.SubCountryCode;
        deleteParams.VendorNumber = this.EntityPM.VendorNumber;
        deleteParams.VendorTypeCode = this.EntityPM.VendorTypeCode;
        //deleteParams.TestCase = SelectedTest;
        deleteParams.IsAfterWarning = isAfterWarning;

        deleteParams.ResponseName = "מחיקת ספק" + " - " + this.EntityPM.VendorName;

        deleteParams.RequestVIA = this.RequestVIA;


        if (this.IsNewEntity) { // Abdullah: No delete button in Add New Vendor !!!
            deleteParams.CommunicationDevices = [];
            this.EntityPM.VendorCommunications.forEach((item: VendorCommunicationPM) => {

                var vendorCommunicationResult = new VendorCommunicationResult();
                vendorCommunicationResult.CommunicationAddress = item.CommunicationAddress;
                vendorCommunicationResult.CommunicationType = item.CommunicationTypeCode;
                vendorCommunicationResult.CommunicationTypeName = item.CommunicationTypeName;

                deleteParams.CommunicationDevices.push(vendorCommunicationResult);
            });
        }

        CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession,deleteParams.PBId, "שליחת מסר הוספה/עדכון/מחיקת ספק", false).then((res) => {

            this.ResponseData = res;
            console.log("[Delete] Response/ShowProgressBar : ", this.ResponseData);

        }).catch((err) => {
            this.FillValidationErrorList.emit(err);
        });



        this.vendorMessagesService.PostAddNewVendorRequest(deleteParams).subscribe((myServiceResponse: ServiceResponse) => {
            console.log("[Delete] Response/PostAddNewVendorRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;

            if (!AppTool.IsNullOrEmpty(response)) {
                if (response.IsCustomWarning) {

                    //_CustomMassagingProgressService.CloseWin();
                    //_CustomMassagingProgressService.Dispose();
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Show(response.UserMessage);
                    confirmWindow.WindowClosed.subscribe((event: any) => {

                        if (confirmWindow.Yes) {
                            if (!this.IsDelete) {
                                this.SendRequest(true);
                            }
                            else {
                                this.DeleteButtonClicked(true, null);
                            }
                        }
                    });
                }
                else {
                    var message = response.UserMessage;
                    if (AppTool.IsNullOrEmpty((response.UserMessage))) {
                        if (!response.HasException && response.Succeeded) {
                            message = operationDescription + response.ApplicationID + " בוצע בהצלחה";
                        }
                        else {
                            message = operationDescription + response.ApplicationID + " נכשלה";
                        }
                    }
                }
            }
            else {
                message = "Service returned a null response!";
            }

            this.OnSendCompleted();


        });
    }

    SendRequest(isAfterWarning: boolean) {
        var operationDescription;
        var operation;

        if (this.IsNewEntity) {
            operation = OperationTypes.Add;
            operationDescription = "הקמת ספק ";
        }
        else {
            operation = OperationTypes.Update;
            operationDescription = "עדכון ספק ";
        }

        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.CustomsVendor")[0];

        var addParams = new VendorInsertUpdateDeleteMessageRequestParams();
        addParams.OperationType = operation;
        addParams.Tenant = SessionLocator.Tenant;
        addParams.LoggingObjectTableId = ObjectTable.Id;
        addParams.LoggingUserId = SessionLocator.LoggedUserId;
        addParams.IsFakeResponse = true;
        addParams.LoggingEnabled = true;
        addParams.RequestName = "Add Vendor Request";
        addParams.ResponseName = "Add Vendor Response";

        addParams.LoggingEntityId = this.EntityPM.Id;
        addParams.LoggingEntityReference = this.EntityPM.VendorNumber;
        addParams.VendorName = this.EntityPM.VendorName;
        addParams.CityName = this.EntityPM.CityName;
        addParams.CountryCode = this.EntityPM.CountryCode;
        addParams.DunsNumber = this.EntityPM.DunsNumber;
        addParams.MainAddressLine = this.EntityPM.MainAddressLine;
        addParams.PostalCode = this.EntityPM.PostalCode;
        addParams.StatusCode = this.EntityPM.StatusCode;
        addParams.SubCountryCode = this.EntityPM.SubCountryCode;
        addParams.VendorNumber = this.EntityPM.VendorNumber;
        addParams.VendorTypeCode = this.EntityPM.VendorTypeCode;
        addParams.VATNumber = this.EntityPM.VATNumber;
        addParams.TransactionTypeID = this.EntityPM.TransactionTypeID;
        addParams.IsPalestinian = this.EntityPM.IsPalestinian;
        addParams.ExternalId = this.EntityPM.ExternalId;
        addParams.ConcurrencyGUID = this.EntityPM.ConcurrencyGUID;
        addParams.RequestVIA = this.RequestVIA;
        //addParams.TestCase = SelectedTest; // this is should be in RequestParamsBase but it does not
        addParams.IsAfterWarning = isAfterWarning;


        addParams.ResponseName = operationDescription + " - " + this.EntityPM.VendorName;

        addParams.CommunicationDevices = [];
        this.EntityPM.VendorCommunications.forEach((item: VendorCommunicationPM) => {

            var vendorCommunicationResult = new VendorCommunicationResult();
            vendorCommunicationResult.CommunicationAddress = item.CommunicationAddress;
            vendorCommunicationResult.CommunicationType = item.CommunicationTypeCode;
            vendorCommunicationResult.CommunicationTypeName = item.CommunicationTypeName;

            addParams.CommunicationDevices.push(vendorCommunicationResult);
        });

        //if (editSendOption == null) {
        //    addParams.RequestVIA = SendRequestVIA.Default;
        //}
        //else if (editSendOption == "WI") {
        //    addParams.RequestVIA = SendRequestVIA.WebServiceInteractive;

        //}
        //else if (editSendOption == "WB") {
        //    addParams.RequestVIA = SendRequestVIA.WebServiceBatch;

        //}
        //else if (editSendOption == "D") {
        //    addParams.RequestVIA = SendRequestVIA.DCABatch;
        //}


        let myShowProgressBarParams = new ShowProgressBarParams();
        myShowProgressBarParams.OnSuccessAnalyzeCloseWinMethod =
            (res1) => {
                if (!AppTool.IsNullOrEmpty(res1)) {

                    if (res1.IsCustomWarning) {
                        return true;
                    }
                }
                return false;
            };
        CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession,addParams.PBId, "שליחת מסר הוספה/עדכון/מחיקת ספק", false

            ,
            //(res1) => {
            //    if (!AppTool.IsNullOrEmpty(res1)) {

            //        if (res1.IsCustomWarning) {
            //            return true;
            //        }
            //    }
            //    return false;
            //}
            myShowProgressBarParams
            ).then((res) => {

            this.ResponseData = res;
            console.log("[Send] Response/ShowProgressBar : ", this.ResponseData);


            //--OnMassageDisplayMethod
            if (this.RequestParams == null) {
                this.RequestParams = new VendorInsertUpdateDeleteMessageRequestParams();
            }
            if (this.ResponseData == null) {
                this.ResponseData = new INF_MSG_GenericResponseData();
            }
            //--

        }).catch((err) => {
            this.FillValidationErrorList.emit(err);
        });


        this.vendorMessagesService.PostAddNewVendorRequest(addParams).subscribe((myServiceResponse: ServiceResponse) => {
            console.log("[Send] Response/PostAddNewVendorRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;
            this._ApplicationID = response.ApplicationID;
            


            if (!AppTool.IsNullOrEmpty(response)) {
                if (response.IsCustomWarning) {

                    //_CustomMassagingProgressService.CloseWin();
                    //_CustomMassagingProgressService.Dispose();
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Show(response.UserMessage);
                    confirmWindow.WindowClosed.subscribe((event: any) => {

                        if (confirmWindow.Yes) {
                            if (!this.IsDelete) {
                                this.SendRequest(true);
                            }
                            else {
                                this.DeleteButtonClicked(true, null);
                            }
                        }

                    });

                }
                else {
                    var message = response.UserMessage;
                    if (AppTool.IsNullOrEmpty((response.UserMessage))) {
                        if (!response.HasException && response.Succeeded) {
                            message = operationDescription + response.ApplicationID + " בוצע בהצלחה";
                        }
                        else {
                            message = operationDescription + response.ApplicationID + " נכשלה";
                        }
                    }
                }
            }
            else {
                message = "Service returned a null response!";
            }

            //var mess = message;
            //if (!string.IsNullOrWhiteSpace(mess)) {
            //    _CustomMassagingProgressService.ShowResponseMessage(mess);
            //    _CustomMassagingProgressService.WindowClosed += (canIContinueEventArgs) => {
            //        _CustomMassagingProgressService.Dispose();
            this.OnSendCompleted();
            //    };
            //}


        });

    }
    _ApplicationID: string = "";
    OnSendCompleted() {
        if (this.IsDelete) {
            this.ApplyDeleteVendor();
        } else {
            if (this.IsNewEntity && !AppTool.IsNullOrEmpty(this._ApplicationID)) {
                if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                    this.EntityPM.Id = this._ApplicationID;//ResponseData.ApplicationID;
                    console.log("Vendor created " + this.EntityPM.Id);
                }
            }
                
        }
    }

    ApplyDeleteVendor() {
        this.IsDelete = false;

        //RefreshDataEvent refreshDataEvent = eventAggregator.GetEvent<RefreshDataEvent>();
        //refreshDataEvent.Publish(new RefreshDataEventArgs() { });
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vendor", DateTime.UtcNow, true);
        //this.Dispose();
    }
    //#endregion
}

// Communication Tab
export class CommunicationItemModel extends BaseComponent {
    public CommunicationPM: VendorCommunicationPM = null;
    public ObjectTableName = "Customs.VendorCommunication";
    public DataContext = this;

    constructor(private communicationPM: VendorCommunicationPM) {
        super();
        this.CommunicationPM = communicationPM;
    }

    //#region Properties

    get CommunicationAddress() { return this.CommunicationPM.CommunicationAddress; }
    set CommunicationAddress(value: string) {
        if (this.CommunicationPM.CommunicationAddress != value) {
            this.CommunicationPM.CommunicationAddress = value;

        }
    }

    get CommunicationTypeCode() { return this.CommunicationPM.CommunicationTypeCode; }
    set CommunicationTypeCode(value: string) {
        if (this.CommunicationPM.CommunicationTypeCode != value) {
            this.CommunicationPM.CommunicationTypeCode = value;

        }
    }

    get CommunicationTypeName() { return this.CommunicationPM.CommunicationTypeName; }
    set CommunicationTypeName(value: string) {
        if (this.CommunicationPM.CommunicationTypeName != value) {
            this.CommunicationPM.CommunicationTypeName = value;

        }
    }


    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}
