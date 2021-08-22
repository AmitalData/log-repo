import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { TapagMessagesService } from '../../../../Customs/Services/WebServices/TapagMessagesService'; 
import { GuaranteeCertificateRequestParams } from '../../../../Customs/DataContract/RequestParams/GuaranteeCertificateRequestParams';
import { GuaranteeCertificateResponseData, GeneralDetails } from '../../../../Customs/DataContract/ResponseData/GuaranteeCertificateResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';

import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
@Component({
    selector: 'GuaranteeCertificateComponent',
    
    templateUrl: './GuaranteeCertificateComponent.html',
})

export class GuaranteeCertificateComponent 
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent { 

    public DataContext: GuaranteeCertificateComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _TapagMessagesService: TapagMessagesService = new TapagMessagesService();

    public AllocationObservableList: ObservableCollection;
    public RequestObservableList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.AllocationObservableList = new ObservableCollection([]);
        this.RequestObservableList = new ObservableCollection([]);
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
            this.RequestParams = new GuaranteeCertificateRequestParams();
        }

        if (this.ResponseData) {
            if (this.ResponseData.GeneralDetailsData == null) {
                this.ResponseData.GeneralDetailsData = new GeneralDetails();
            }

            if (this.ResponseData.AllocationList) {
                this.AllocationObservableList.InsertCollection(this.ResponseData.AllocationList);
                this.AllocationVisibility = true;
            }

            if (this.ResponseData.RequestList) {
                this.ResponseData.RequestList.forEach((itemMess) => {
                    this.RequestObservableList.Insert(itemMess);
                    this.RequestVisibility = true;
                });
            }
        }
        else
        {
            this.ResponseData = new GuaranteeCertificateResponseData();
            this.ResponseData.GeneralDetailsData = new GeneralDetails();
        }
    }

    //#region Properties
    get GuaranteeCertificateType() { return this.RequestParams ? this.RequestParams.guaranteeCertificateType : null; }
    set GuaranteeCertificateType(value: number) {
        if (this.RequestParams.guaranteeCertificateType != value) {
            this.RequestParams.guaranteeCertificateType = value;
        }
    }

    get CertificateID() { return this.RequestParams.certificateID; }
    set CertificateID(value: number) {
        if (this.RequestParams.certificateID != value) {
            this.RequestParams.certificateID = value;
        }
    }

    get GuaranteeExternalCertificateNumber() { return this.RequestParams.guaranteeExternalCertificateNumber; }
    set GuaranteeExternalCertificateNumber(value: string) {
        if (this.RequestParams.guaranteeExternalCertificateNumber != value) {
            this.RequestParams.guaranteeExternalCertificateNumber = value;
        }
    }

    get GuarantorID() { return this.RequestParams.guarantorID; }
    set GuarantorID(value: number) {
        if (this.RequestParams.guarantorID != value) {
            this.RequestParams.guarantorID = value;
        }
    }

    private _AllocationVisibility: boolean;
    SetAllocationVisibility(newValue: boolean) {
        this._RequestVisibility = newValue;
    }

    get AllocationVisibility() { return this._AllocationVisibility; }
    set AllocationVisibility(newValue: boolean) {
        if (this._AllocationVisibility != newValue) {
            this._AllocationVisibility = newValue;
        }
    }

    private _RequestVisibility: boolean;
    SetRequestVisibility(newValue: boolean) {
        this._RequestVisibility = newValue;
    }

    get RequestVisibility() { return this._RequestVisibility; }
    set RequestVisibility(newValue: boolean) {
        if (this._RequestVisibility != newValue) {
            this._RequestVisibility = newValue;
        }
    }
    //#endregion Properties

    //#region Response Properties
    get GuaranteeTypeName() { return this.ResponseData.GeneralDetailsData.guaranteeTypeName; }
    set GuaranteeTypeName(value: string) {
        if (this.ResponseData.GeneralDetailsData.guaranteeTypeName != value) {
            this.ResponseData.GeneralDetailsData.guaranteeTypeName = value;
        }
    }

    get GuaranteeExternalCertificateNumebr() { return this.ResponseData.GeneralDetailsData.guaranteeExternalCertificateNumebr; }
    set GuaranteeExternalCertificateNumebr(value: string) {
        if (this.ResponseData.GeneralDetailsData.guaranteeExternalCertificateNumebr != value) {
            this.ResponseData.GeneralDetailsData.guaranteeExternalCertificateNumebr = value;
        }
    }

    get GuaranteeValidityDate() { return this.ResponseData.GeneralDetailsData.guaranteeValidityDate; }
    set GuaranteeValidityDate(value: string) {
        if (this.ResponseData.GeneralDetailsData.guaranteeValidityDate != value) {
            this.ResponseData.GeneralDetailsData.guaranteeValidityDate = value;
        }
    }

    get CertificateAvailableAmount() { return this.ResponseData.GeneralDetailsData.certificateAvailableAmount; }
    set CertificateAvailableAmount(value: string) {
        if (this.ResponseData.GeneralDetailsData.certificateAvailableAmount != value) {
            this.ResponseData.GeneralDetailsData.certificateAvailableAmount = value;
        }
    }

    get GuaranteeCertificateStatusName() { return this.ResponseData.GeneralDetailsData.GuaranteeCertificateStatusName; }
    set GuaranteeCertificateStatusName(value: string) {
        if (this.ResponseData.GeneralDetailsData.GuaranteeCertificateStatusName != value) {
            this.ResponseData.GeneralDetailsData.GuaranteeCertificateStatusName = value;
        }
    }

    get GuaranteedName() { return this.ResponseData.GeneralDetailsData.guaranteedName; }
    set GuaranteedName(value: string) {
        if (this.ResponseData.GeneralDetailsData.guaranteedName != value) {
            this.ResponseData.GeneralDetailsData.guaranteedName = value;
        }
    }

    get GeneralDetailsCertificateID() { return this.ResponseData.GeneralDetailsData.certificateID; }
    set GeneralDetailsCertificateID(value: number) {
        if (this.ResponseData.GeneralDetailsData.certificateID != value) {
            this.ResponseData.GeneralDetailsData.certificateID = value;
        }
    }

    get GuaranteeAmount() { return this.ResponseData.GeneralDetailsData.guaranteeAmount; }
    set GuaranteeAmount(value: string) {
        if (this.ResponseData.GeneralDetailsData.guaranteeAmount != value) {
            this.ResponseData.GeneralDetailsData.guaranteeAmount = value;
        }
    }

    get TotalCertificateAllocation() { return this.ResponseData.GeneralDetailsData.totalCertificateAllocation; }
    set TotalCertificateAllocation(value: string) {
        if (this.ResponseData.GeneralDetailsData.totalCertificateAllocation != value) {
            this.ResponseData.GeneralDetailsData.totalCertificateAllocation = value;
        }
    }

    //#endregion

    //#region Commands

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        errors.forEach((err) => { this.ValidationErrorsList.push(err); });

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.AllocationObservableList.Clear();
        this.RequestObservableList.Clear();

        var currRequestParams = new GuaranteeCertificateRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.guaranteeCertificateType = this.GuaranteeCertificateType;
        currRequestParams.certificateID = this.CertificateID;
        currRequestParams.guaranteeExternalCertificateNumber = this.GuaranteeExternalCertificateNumber;
        currRequestParams.guarantorID = this.GuarantorID;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;


        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא לנתוני כתב ערבות", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });

        this._TapagMessagesService.PostGuaranteeCertificateRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    //#endregion Commands
}
