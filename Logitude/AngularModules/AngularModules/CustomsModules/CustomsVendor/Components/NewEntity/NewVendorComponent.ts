import {Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { CustomSendOptionsArgs, SendRequestVIA} from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';

import {CustomsVendorPM} from '../../../../Customs/EntityPMs/CustomsVendorPM';
import {VendorCommunicationPM} from '../../../../Customs/EntityPMs/VendorCommunicationPM';

// Send Request
import {VendorSearchByCustomsAgentRequestParams} from '../../../../Customs/DataContract/RequestParams/VendorSearchByCustomsAgentRequestParams';
import { VendorCommunicationResult } from '../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { VendorMessagesService } from '../../../../Customs/Services/WebServices/VendorMessagesService';

import { CustomsVendorPMService } from '../../../../Customs/Services/StandardPMs/CustomsVendorPMService';
import { CustomsVendorListService } from '../../../../Customs/Services/StandardLists/CustomsVendorListService';

import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

import { ImporterDeclarationRequestParams } from '../../../../Customs/DataContract/RequestParams/ImporterDeclarationRequestParams';
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';


@Component({
    
    templateUrl: './NewVendorComponent.html',
})

export class NewVendorComponent extends BaseComponent {
    public DataContext: any = this;
    public EntityPM: CustomsVendorPM;
    public ObjectTableName: string = "Customs.CustomsVendor";
    VendorsList: ObservableCollection;
    ValidationErrorsList: any[] = [];
    IsWarningBoxVisible: boolean = false;
    WarningMsg: string = "";

    vendorMessagesService: VendorMessagesService = new VendorMessagesService();
    customsVendorPMService: CustomsVendorPMService = new CustomsVendorPMService();
    customsVendorListService: CustomsVendorListService = new CustomsVendorListService();
    iIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    IsFromDeclarationMode: boolean = false;
    IsImporterDespositionValide: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new CustomsVendorPM();
        this.EntityPM.VendorTypeCode = "1";

        this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, false);

        this.VendorsList = new ObservableCollection([]);

        this.entityResourceService.getEntityResourceByTableName("Customs.CustomsClosedTable").subscribe((response: any) => { });
        this.entityResourceService.getEntityResourceByTableName("Customs.ClientsAddressCommType").subscribe((response: any) => { });

        if (this.CurrentSession.CurrentEditComponent != null && this.CurrentSession.CurrentEditComponent.ObjectTableName == "Customs.Declaration") {
            this.IsFromDeclarationMode = true;           
        }
        

    }
    IsSearchMode: boolean = false; // yaron want to allowed to send response Even there is only VendorNum
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            //this.EntityPM = args.EntityPM;
            if (args.IsSearchMode) {
                this.IsSearchMode = true;
            }
        }
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

    public get VendorNumber() { return this.EntityPM.VendorNumber; }
    public set VendorNumber(newValue: string) {
        this.EntityPM.VendorNumber = newValue;
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

    RequestVIA: SendRequestVIA;


    SendButtonClicked(event) {
        this.RequestVIA = event.RequestVIA;
        this.ValidationErrorsList = [];
        this.VendorsList.Clear();
        this.IsWarningBoxVisible = false;


        if ( (!AppTool.IsNullOrEmpty(this.VendorName) && !AppTool.IsNullOrEmpty(this.CountryCode) )
            || this.VendorName == "TST"  || !AppTool.IsNullOrEmpty(this.VendorNumber)) {

            if (this.VendorName && this.VendorName.length < 2) {
                this.CurrentSession.StopBusyIndicator();
                var msg = TextCodeTranslator.Translate("Customs.CustomsVendor.O.mustEnterAtLeast2Chars"); // Bug 28579: Customs Query - search Vendor
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(msg);
                return;
            }

            if (//this.IsSearchMode &&
                !AppTool.IsNullOrEmpty(this.VendorNumber)) {
                    // yaron want to allowed to send response Even there is only VendorNum
            } else {

                var errors = [];
                Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
                if (errors.length > 0) {

                    this.ValidationErrorsList = errors;
                    return;
                }
            }
            // Replaced with if clause ternarry this.VendorNumber == "0" ? null : this.VendorNumber
            //var vendortypeCode;
            //var vendortypeCodeNullable;
            ////int.TryParse(VendorTypeCode, out vendortypeCode);
            //if (vendortypeCode == 0) {
            //    vendortypeCodeNullable = null;
            //}
            //else {
            //    vendortypeCodeNullable = vendortypeCode;
            //}

            //var vendorNumber;
            //var vendorNumberNullable;
            ////int.TryParse(VendorNumber, out vendorNumber);
            //if (vendorNumber == 0) {
            //    vendorNumberNullable = null;
            //}
            //else {
            //    vendorNumberNullable = vendorNumber;
            //}


            //var dunsNumber;
            //var dunsNumberNullable;
            ////int.TryParse(DunsNumber, out dunsNumber);
            //if (dunsNumber == 0) {
            //    dunsNumberNullable = null;
            //}
            //else {
            //    dunsNumberNullable = dunsNumber;
            //}

            var searchParams = new VendorSearchByCustomsAgentRequestParams()
            searchParams.IsFakeResponse = true;
            searchParams.LoggingEnabled = false;
            searchParams.RequestName = "Search For Vendor Request";
            searchParams.ResponseName = "Search For Vendor Response";
            searchParams.Tenant = SessionLocator.Tenant;
            searchParams.VendorName = this.VendorName == undefined ? null : this.VendorName;
            searchParams.MainAddressLine = this.MainAddressLine;
            searchParams.VendorNumber = this.VendorNumber == "0" ? null : +this.VendorNumber;
            searchParams.VendorTypeCode = this.VendorTypeCode == "0" ? null : +this.VendorTypeCode;
            searchParams.DunsNumber = this.DunsNumber == "0" ? null : +this.DunsNumber;
            searchParams.CountryCode = this.CountryCode;
            searchParams.CityName = this.CityName;
            searchParams.PostalCode = this.PostalCode;
            searchParams.SubCountryCode = this.SubCountryCode;
            searchParams.LicensedDealerNumber = this.VATNumber;
            searchParams.IsPalestinian = this.EntityPM.IsPalestinian;
            searchParams.RequestVIA = this.RequestVIA;
            //searchParams.TestCase = SelectedTest;


            //
            CustomMessageProgressComponent.ShowProgressBar(this.CurrentSession,searchParams.PBId, "חיפוש ספק", true)
                .then((myServiceResponse) => {

                    console.log("[Send] Response/ShowProgressBar : ", myServiceResponse);
                var response = myServiceResponse.Result;

                if (!AppTool.IsNullOrEmpty(response)) {
                    var VendorResults: any[] = response.VendorResults;
                    if (response.IsCustomWarning) {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                }
                }).catch((err) => {
                    console.error("[ERROR] CustomMessageProgressComponent error: ", err);
                });


            this.vendorMessagesService.PostSearchVendorRequest(searchParams).subscribe((myServiceResponse: ServiceResponse) => {
                console.log("[Send] Response/PostSearchVendorRequest : ", myServiceResponse.Result);
                var response = myServiceResponse.Result;

                if (!AppTool.IsNullOrEmpty(response)) {
                var VendorResults: any[] = response.VendorResults;
                    if (response.IsCustomWarning) {
                        
                        var confirmWindow = new ConfirmWindow();
                        confirmWindow.Show(response.UserMessage);
                        confirmWindow.WindowClosed.subscribe((event: any) => {

                            if (confirmWindow.Yes) {

                            }

                        });

                    }
                    else {
                        //var message = response.UserMessage;
                        //if (AppTool.IsNullOrEmpty((response.UserMessage))) {
                        //    if (!response.HasException && response.Succeeded) {
                        //        message = operationDescription + response.ApplicationID + " בוצע בהצלחה";
                        //    }
                        //    else {
                        //        message = operationDescription + response.ApplicationID + " נכשלה";
                        //    }
                        //}
                    }

                    // fill grid
                    this.VendorsList.Clear();
                    this.VendorsList.InsertCollection(VendorResults);

                    // warning msg 
                    var resultNumber = AppTool.IsNullOrEmpty(response.NumberOfResult) ? 0 : response.NumberOfResult;
                    var vendorsResultLength = AppTool.IsNullOrEmpty(VendorResults) ? 0 : VendorResults.length;
                    this.IsWarningBoxVisible = vendorsResultLength < resultNumber ? true : false;
                    if (this.IsWarningBoxVisible) {
                        var pre = TextCodeTranslator.Translate("Customs.CustomsVendor.O.DifferenceVendorCountMessagePre");
                        var post = TextCodeTranslator.Translate("Customs.CustomsVendor.O.DifferenceVendorCountMessagePost");
                        var msg = pre + response.NumberOfResult + post;
                        this.WarningMsg = msg;
                    }

                    if (this.VendorsList != null && this.VendorsList.Length > 0 && this.IsFromDeclarationMode) {
                        this.GetImporterDeposition();
                    }

                }
                else {
                    var message = "Service returned a null response!";
                }
                
                //this.OnSendCompleted();
            });

        }
        else {
            this.CurrentSession.StopBusyIndicator();
            var msg = TextCodeTranslator.Translate("Customs.CustomsVendor.O.SearchRequieredFieldsError");
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(msg);
        }
    }
    CreateSupplierButtonClicked() {


        this.entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe((response:any) => {
            this.entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe((response:any) => {
                var vendor = new CustomsVendorPM();
                vendor.Tenant = SessionLocator.Tenant;
                vendor.VendorTypeCode = "1";

                var args:any = {};
                args.IsNewEntity = true;
                args.EntityPM = vendor;

                var logWindow = new LogitudeWindow();
                logWindow.Width = 960;
                logWindow.Height = 570;
                logWindow.Title = TextCodeTranslator.Translate("Customs.Vendor.O.New");
                logWindow.WindowArgs = args;
                logWindow.ShowCloseButton = true;
              logWindow.Show('./CustomsModules/CustomsVendor/Components/EditTabs/VendorEditComponent');

                logWindow.WindowClosed.subscribe(($event: any) => {

                });
            });
        });


    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    AddButtonClicked(item: any) {
        if (item.StatusCode == '3') return;

        var errors = [];
        this.ValidationErrorsList = errors;

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        var newVendor = new CustomsVendorPM();
        newVendor.Tenant = SessionLocator.Tenant;
        newVendor.VendorName = item.VendorName;
        newVendor.CityName = item.CityName;
        newVendor.CountryCode = item.CountryCode;
        newVendor.DunsNumber = item.DunsNumber;
        newVendor.MainAddressLine = item.MainAddressLine;
        newVendor.PostalCode = item.PostalCode;
        newVendor.StatusCode = item.StatusCode;
        newVendor.SubCountryCode = item.SubCountryCode;
        newVendor.VendorNumber = item.VendorNumber;
        newVendor.VendorTypeCode = item.VendorTypeCode;
        newVendor.VATNumber = item.VATNumber;


        Validator.TryValidateObject(newVendor, this.ObjectTableName, errors);

        if (errors.length == 0) {
            for (let vendorCommunicationResult of item.VendorCommunications)
            {
                var newVendorCommunication = new VendorCommunicationPM(newVendor);
                newVendorCommunication.Tenant = SessionLocator.Tenant;
                newVendorCommunication.CommunicationAddress = vendorCommunicationResult.CommunicationAddress;
                newVendorCommunication.CommunicationTypeCode = vendorCommunicationResult.CommunicationType;

                newVendor.VendorCommunications.push(newVendorCommunication);
            }

            // Call service to add vendor
            this.customsVendorPMService.insert(newVendor).subscribe((myResult:any) => {

                var res: ServiceResponse = myResult;
                if (!res.HasError) {
                    var entity = res.Result;

                    console.log("..Vendor added successfully ", entity);

                    // after success
                    var index = this.VendorsList.GetIndex(item);
                    item.Exists = true;

                    var temp;
                    temp = this.VendorsList.Collection;
                    this.VendorsList.Clear();
                    this.VendorsList.InsertCollection(temp);

                }
                else {
                    this.ValidationErrorsList = res.ErrorsArray;
                }
                this.CurrentSession.StopBusyIndicator();
            });

        }
        else {
            this.CurrentSession.StopBusyIndicator();
            this.ValidationErrorsList = errors;
        }
    }
    UpdateButtonClicked(item: any) {


        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.CustomsVendor.O.UpdateVendor"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

                // Get vendor and update it
                this.vendorMessagesService.GetVendorByNumber(item.VendorNumber).subscribe((res:any) => {

                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        var entity = pmResponse.Result;
                        this.UpdateVendor(entity, item);
                        console.log("entity:" , entity);

                    }
                    this.CurrentSession.StopBusyIndicator();

                });
            }
            else {

            }
        });

    }

    UpdateVendor(vendorPM, VendorResult) {
        var errors = [];
        this.ValidationErrorsList = errors;

        if (vendorPM != null) {
            vendorPM.CityName = VendorResult.CityName;
            vendorPM.CountryCode = VendorResult.CountryCode;
            vendorPM.InActive = VendorResult.InActive;
            vendorPM.MainAddressLine = VendorResult.MainAddressLine;
            vendorPM.PostalCode = VendorResult.PostalCode;
            vendorPM.StatusCode = VendorResult.StatusCode;
            vendorPM.SubCountryCode = VendorResult.SubCountryCode;
            vendorPM.Tenant = VendorResult.Tenant;
            vendorPM.VATNumber = VendorResult.VATNumber;
            vendorPM.VendorNumber = VendorResult.VendorNumber;
            vendorPM.VendorTypeCode = VendorResult.VendorTypeCode;
            vendorPM.DunsNumber = VendorResult.DunsNumber;
            vendorPM.IsPalestinian = VendorResult.IsPalestinian;
            vendorPM.VendorName = VendorResult.VendorName;
            vendorPM.InActive = VendorResult.StatusCode == "1" ? false : true;
        }

        Validator.TryValidateObject(vendorPM, this.ObjectTableName, errors);

        if (errors.length == 0) {
            // update service
            this.customsVendorPMService.update(vendorPM).subscribe((myResult:any) => {

                var res: ServiceResponse = myResult;
                if (!res.HasError) {
                    var entity = res.Result;

                    var temp;
                    temp = this.VendorsList.Collection;
                    this.VendorsList.Clear();
                    this.VendorsList.InsertCollection(temp);

                    console.log(".. Vendor update successfully ", entity);
                    
                }
                else {
                    this.ValidationErrorsList = res.ErrorsArray;
                }
                this.CurrentSession.StopBusyIndicator();
            });
        }
        else {
            this.ValidationErrorsList = errors;
            this.CurrentSession.StopBusyIndicator();

        }

    }

    OnRowLoaded(myRow: any) {
        if (myRow) {
            var isExpandaple = false;

            var item: any = myRow.rowData;
            if (item) {
                item.Row = myRow;

                //if (item.InsideItemsSource.length > 0) {
                //    isExpandaple = true;
                //}
            }
            myRow.SetExpandaple(true);
        }
    }
    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }
    
    GetImporterDeposition() {
        this.CurrentSession.StartBusyIndicator("");
        let customSendOptionsArgs: CustomSendOptionsArgs = new CustomSendOptionsArgs();
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var day = new Date().getDay();

        var currRequestParams = new ImporterDeclarationRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.ImporterNumber = this.CurrentSession.CurrentEditComponent.EntityPM.ImporterCode;
        currRequestParams.IsByExpireDate = true;
        currRequestParams.DeclarationExpire = new Date(Year + 1, month, day);

        this.iIGGeneralMessagesService.PostImporterDeclarationRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                if (!AppTool.IsNullOrEmpty(myServiceResponse.Result)) {
                    this.JoinVendorDeposition(myServiceResponse.Result.PeriodDeclarationList);
                }
                this.CurrentSession.StopBusyIndicator();
            });
    }

    JoinVendorDeposition(periodDeclarationList: any[]) {

        if (periodDeclarationList != null && periodDeclarationList.length > 0) {
            this.VendorsList.Collection.forEach((item) => {
                var temp = periodDeclarationList.filter(a => a.VendorID == item.VendorNumber)[0];
                if (temp != null) {
                    item.ImporterDesposition = temp.ExpirationDate;
                    if (temp.ExpirationDate > new Date().getDate) {
                        this.IsImporterDespositionValide = false;
                    }
                    else {
                        this.IsImporterDespositionValide = true;
                    }
                }
            });

        }
    }

}
