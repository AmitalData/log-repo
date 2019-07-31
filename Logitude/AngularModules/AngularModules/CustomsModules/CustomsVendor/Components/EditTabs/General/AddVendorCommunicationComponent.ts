declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList } from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../../../Infrastructure/Utilities/LocationDirective';
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

import {CustomsVendorPM} from '../../../../../Customs/EntityPMs/CustomsVendorPM';
import {VendorCommunicationPM} from '../../../../../Customs/EntityPMs/VendorCommunicationPM';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import {VendorAddCommunicationDeviceRequestParams} from '../../../../../Customs/DataContract/RequestParams/VendorAddCommunicationDeviceRequestParams';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {VendorMessagesService} from '../../../../../Customs/Services/WebServices/VendorMessagesService';
import {CustomsVendorPMService} from '../../../../../Customs/Services/StandardPMs/CustomsVendorPMService';

@Component({
    moduleId: module.id,
    templateUrl: './AddVendorCommunicationComponent.html',
})

export class AddVendorCommunicationComponent extends BaseComponent {
    public VendorPM: CustomsVendorPM;
    public EntityPM: VendorCommunicationPM;
    public ObjectTableName: string = "Customs.VendorCommunication";
    public DataContext: any = this;
    ValidationErrorsList: any[];
    vendorMessagesService: VendorMessagesService = new VendorMessagesService();
    customsVendorPMService: CustomsVendorPMService = new CustomsVendorPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.VendorPM = args.VendorPM;
            this.EntityPM = new VendorCommunicationPM(this.VendorPM);
            this.EntityPM.VendorId = this.VendorPM.Id;
            this.EntityPM.LineNumber = 1; // it will be override by EntityUpdateService.OnCreating() in server.
        }
    }

    SendButtonClicked() {

        // Validate Requierd Fields
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        } else {
            this.SendRequest();
        }

    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendRequest() {
        var vendorNumber = null;
        if (!AppTool.IsNullOrEmpty(this.VendorPM.VendorNumber)) {
            var result;
            //int.TryParse(this.VendorPM.VendorNumber, out result);
            vendorNumber = Number(this.VendorPM.VendorNumber);
        }

        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.CustomsVendor")[0];

        var addParams = new VendorAddCommunicationDeviceRequestParams()
        addParams.LoggingEntityId = this.VendorPM.Id;
        addParams.LoggingObjectTableId = ObjectTable.Id;
        addParams.LoggingEntityReference = this.VendorPM.VendorNumber;
        addParams.LoggingUserId = SessionLocator.LoggedUserId;
        addParams.LoggingEnabled = true;
        addParams.RequestName = "Add Vendor Communication Request";
        addParams.ResponseName = "Add Vendor Communication Response";
        addParams.Tenant = SessionLocator.Tenant;
        addParams.VendorNumber = vendorNumber;
        addParams.IsFakeResponse = false;
        //addParams.TestCase = SelectedTest;

        //if (SendOption == null) {
        //    addParams.RequestVIA = SendRequestVIA.Default;
        //}
        //else if (SendOption == "WI") {
        //    addParams.RequestVIA = SendRequestVIA.WebServiceInteractive;

        //}
        //else if (SendOption == "WB") {
        //    addParams.RequestVIA = SendRequestVIA.WebServiceBatch;

        //}
        //else if (SendOption == "D") {
        //    addParams.RequestVIA = SendRequestVIA.DCABatch;

        //}

        var communicationResult = new VendorCommunicationResult();
        communicationResult.CommunicationAddress = this.CommunicationAddress;
        communicationResult.CommunicationType = this.CommunicationTypeCode;

        addParams.CommunicationDevices = [];
        addParams.CommunicationDevices.push(communicationResult);

        CustomMessageProgressComponent.ShowProgressBar(addParams.PBId, "שליחת מסר הוספה/עדכון/מחיקת ספק", true).then((res) => {
            console.log("[Send] Response/ShowProgressBar : ", res);
        }).catch((err) => {
            this.ValidationErrorsList.push(err);
        });


        this.vendorMessagesService.PostAddNewVendorCommunicationRequest(addParams).subscribe((myServiceResponse: ServiceResponse) => {
            console.log("[Send] Response/PostAddNewVendorCommunicationRequest : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;

            if (!AppTool.IsNullOrEmpty(response)) {
                if (response.IsCustomWarning) {

                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Show(response.UserMessage);
                    confirmWindow.WindowClosed.subscribe((event: any) => {

                        if (confirmWindow.Yes) {

                        }

                    });

                }
                else {
                    var message = response.UserMessage;
                    if (AppTool.IsNullOrEmpty((response.UserMessage))) {
                        //if (!response.HasException && response.Succeeded) {
                        //    message = operationDescription + response.ApplicationID + " בוצע בהצלחה";
                        //}
                        //else {
                        //    message = operationDescription + response.ApplicationID + " נכשלה";
                        //}
                    }
                }
            }
            else {
                message = "Service returned a null response!";
            }
            
            this.OnSendCompleted(response);

            //this.CurrentSession.CloseCurrentWindowEmit("Ok");

        });
    }

    OnSendCompleted(response: any) {

        if (!response.HasException && response.Succeeded) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
        else {
            //CustomsMessageViewModel viewmodel = new CustomsMessageViewModel();
            //CustomsMessageControl actionControl = new CustomsMessageControl() { DataContext = viewmodel };
            //actionControl.OkButton.Click += OkButton_Click;
            //window = new SimplogWindow();
            //window.Add(actionControl);
            //window.Height = 600;
            //window.Width = 800;
            //window.CancelButton.Visibility = Visibility.Collapsed;
            //window.Show();
            //viewmodel.Title = "Send Vendor";
            //viewmodel.Message = responseData.UserMessage;
        }
    }

    //#region Properties
    get CommunicationTypeCode() { return this.EntityPM.CommunicationTypeCode; }
    set CommunicationTypeCode(value: string) {
        if (this.EntityPM.CommunicationTypeCode != value) {
            this.EntityPM.CommunicationTypeCode = value;

        }
    }

    get CommunicationAddress() { return this.EntityPM.CommunicationAddress; }
    set CommunicationAddress(value: string) {
        if (this.EntityPM.CommunicationAddress != value) {
            this.EntityPM.CommunicationAddress = value;

        }
    }
    //#endregion
}
