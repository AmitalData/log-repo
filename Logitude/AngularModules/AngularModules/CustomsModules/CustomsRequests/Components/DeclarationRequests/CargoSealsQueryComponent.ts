import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConsignmentPM } from '../../../../Customs/EntityPMs/ConsignmentPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { DeclarationMessagesService } from '../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { CargoSealsRequestParams } from '../../../../Customs/DataContract/RequestParams/CargoSealsRequestParams';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';


@Component({
    moduleId: module.id,
    selector: 'CargoSealsQueryComponent',
    templateUrl: './CargoSealsQueryComponent.html',
})


export class CargoSealsQueryComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent, OnInit {
    public DataContext: CargoSealsQueryComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();
    _LastFetchDeclarationList: DeclarationList;
    _LastFetchConsignmentPMList: ConsignmentPM[];

    //public MorningMessageObservableList: ObservableCollection;

    text: any;
    IsRePackingApproval: any;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        //this.MorningMessageObservableList = new ObservableCollection([]);
    }

    ngOnInit() {
        super.ngOnInit();
    }

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new CargoSealsRequestParams();
        }

        //if (this.ResponseData && this.ResponseData.MorningMessageList) {
        //    var myArr = this.ResponseData.MorningMessageList;
        //    var fast = true;
        //    if (!fast) {
        //        this.ResponseData.MorningMessageList.forEach((itemMess) => {
        //            this.MorningMessageObservableList.Insert(itemMess);
        //        });
        //    } else {

        //        this.MorningMessageObservableList.InsertCollection(myArr);
        //    }
        //}
    }

    get UpdateDate() { return this.RequestParams.UpdateDate; }
    set UpdateDate(value: Date) {
        if (this.RequestParams.UpdateDate != value) {
            this.RequestParams.UpdateDate = value;
        }
    }

    get CustomFileNo() { return this.RequestParams.CustomFileNo; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomFileNo != value) {
            this.RequestParams.CustomFileNo = value;
        }
    }

    get DeclarationId() { return this.RequestParams ? this.RequestParams.DeclarationId : null }
    set DeclarationId(value: string) {
        if (this.RequestParams.DeclarationId != value) {
            this.RequestParams.DeclarationId = value;
        }
    }

    get DeclarationNumber() { return this.RequestParams.DeclarationNumber; }
    set DeclarationNumber(value: string) {
        if (this.RequestParams.DeclarationNumber != value) {
            this.RequestParams.DeclarationNumber = value;
        }
    }

    get CargoIdentifierTypeCode() { return this.RequestParams.CargoIdentifierTypeCode; }
    set CargoIdentifierTypeCode(value: string) {
        if (this.RequestParams.CargoIdentifierTypeCode != value) {
            this.RequestParams.CargoIdentifierTypeCode = value;
        }
    }

    get CargoIdentifierKey1() { return this.RequestParams.CargoIdentifierKey1; }
    set CargoIdentifierKey1(value: string) {
        if (this.RequestParams.CargoIdentifierKey1 != value) {
            this.RequestParams.CargoIdentifierKey1 = value;
        }
    }

    get CargoIdentifierKey2() { return this.RequestParams.CargoIdentifierKey2; }
    set CargoIdentifierKey2(value: string) {
        if (this.RequestParams.CargoIdentifierKey2 != value) {
            this.RequestParams.CargoIdentifierKey2 = value;
        }
    }

    get CargoIdentifierKey3() { return this.RequestParams.CargoIdentifierKey3; }
    set CargoIdentifierKey3(value: string) {
        if (this.RequestParams.CargoIdentifierKey3 != value) {
            this.RequestParams.CargoIdentifierKey3 = value;
        }
    }

    get CargoRowNumber() { return this.RequestParams.CargoRowNumber; }
    set CargoRowNumber(value: string) {
        if (this.RequestParams.CargoRowNumber != value) {
            this.RequestParams.CargoRowNumber = value;
        }
    }

    get ContainerNumber() { return this.RequestParams.ContainerNumber; }
    set ContainerNumber(value: string) {
        if (this.RequestParams.ContainerNumber != value) {
            this.RequestParams.ContainerNumber = value;
        }
    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        errors.forEach((err) => { this.ValidationErrorsList.push(err); });

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        //if (this.MorningMessageObservableList.Length > 0) {
        //    this.MorningMessageObservableList.Clear();
        //}

        var currRequestParams = new CargoSealsRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.UpdateDate = this.UpdateDate;
        currRequestParams.CargoRowNumber = this.CargoRowNumber;
        currRequestParams.ContainerNumber = this.ContainerNumber;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת מסר לעדכון סגרים", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._DeclarationMessagesService.PostSendCargoSealsRequest(currRequestParams)
            .subscribe(() => { }
            )
            ;
    }

    CustomFileNoTextChanged(searchtext) {

        if (!AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.CurrentSession.StartBusyIndicator("")
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe((myDeclarationResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();
                    this._LastFetchDeclarationList = myDeclarationResponse.Result;
                    if (AppTool.IsNullOrEmpty(this._LastFetchDeclarationList)) {
                        this.SetFieldsEnabled(true);
                    } else {
                        this.CurrentSession.StartBusyIndicator("")
                        this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
                            .subscribe((myResponse: ServiceResponse) => {
                                this.CurrentSession.StopBusyIndicator();
                                this.FetchConsignment(myResponse, false);

                            });
                    }

                });

        }
    }

    SetFieldsEnabled(isDisplay: boolean) {

        if (isDisplay) {
            this.DeclarationId = "";
            this.DeclarationNumber = "";
            this.CargoIdentifierTypeCode = "";
            this.CargoIdentifierKey1 = "";
            this.CargoIdentifierKey2 = "";
            this.CargoIdentifierKey3 = "";
        }

        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, isDisplay);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, isDisplay);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, isDisplay);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, isDisplay);
    }

    FetchConsignment(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this._LastFetchConsignmentPMList = myResponse.Result
        if (this._LastFetchConsignmentPMList != null) {
            var pm = this._LastFetchConsignmentPMList[0]
            this.DeclarationId = pm.DeclarationId;
            this.DeclarationNumber = this._LastFetchDeclarationList.DeclarationNumber;
            this.CargoIdentifierTypeCode = pm.CargoTypeCode;
            this.CargoIdentifierKey1 = pm.ManifestNumber;
            this.CargoIdentifierKey2 = pm.SecondCargoID;
            this.CargoIdentifierKey3 = pm.ThirdCargoID;
            this.SetFieldsEnabled(false);

        } else {
            this.SetFieldsEnabled(true);

        }
    }

}
