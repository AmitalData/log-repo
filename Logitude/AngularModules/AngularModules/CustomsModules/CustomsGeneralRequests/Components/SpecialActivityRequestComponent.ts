import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM'; 
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { SpecialActivityRequestParams, OtherActivityDetails, GeneralDetails, CargoIdentifier, GoodsDetails, RepresentativeDetails, RePackingApprovalDetails, CurrentPackingDetails, PackingDetails, DesiredPackingDetails, SampleRequestDetails } from '../../../Customs/DataContract/RequestParams/SpecialActivityRequestParams';
import { INF_MSG_GenericResponseData } from '../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { CustomsSettingListService } from '../../../Customs/Services/StandardLists/CustomsSettingListService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { LuhnAlgorithm } from '../../../Customs/Utilities/LuhnAlgorithm';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'SpecialActivityRequestComponent',
    
    templateUrl: './SpecialActivityRequestComponent.html',
})

export class SpecialActivityRequestComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: SpecialActivityRequestComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public StorageFilterItems: ApiQueryFilters;

    private _IsGoodsDetailsData: boolean = true;
    private _IsOtherActivity: boolean = false;
    private _IsRePackingApproval: boolean = false;
    private _IsSampleRequest: boolean = false;
    private _IsResponseMessageVisibility: boolean = false;
    private _IsLoadResponseData: boolean = true;
    private _TodayDate: Date;

    public RepresentativeList: ObservableCollection;
    public RepackingCurrentList: ObservableCollection;
    public RepackingDesiredList: ObservableCollection;
    public SampleRequestList: ObservableCollection;
    public RequestId: number;
    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    _CustomsSettingListService: CustomsSettingListService = new CustomsSettingListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.RepresentativeList = new ObservableCollection([]);
        this.RepackingCurrentList = new ObservableCollection([]);
        this.RepackingDesiredList = new ObservableCollection([]);
        this.SampleRequestList = new ObservableCollection([]);

        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe((response:any) => {
        });
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
        this.RequestId = new Date(Date.now()).getTime();
        this.subscribeWrapperComponent()
    }

    ViewDocumentsComponent() {
        var windowArgs: any = {};
        this.EntityPM = {};
        this.EntityPM.Id = this.RequestId;
        this.EntityPM.Tenant = SessionLocator.Tenant;
        windowArgs.EntityPM = this.EntityPM;
        // windowArgs.ObjectTableName = "Customs.CustomsCollateral";Cancellation
        windowArgs.ObjectTableName = "Customs.Declaration";// this.ObjectTableName;
        windowArgs.EntityParentPM = "SpecialRequest";
        //    windowArgs.SkipCtor = this.SkipCtor;

        var windowTitle = "Customs.Declaration.TH.Documents";

        var logWindow = new LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
       // logWindow.WindowClosed.subscribe(($event: any) => this.SkipCtor = true);

       
            logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
 

    }


    OnMassageDisplayMethod() {
        this.StorageFilterItems = new ApiQueryFilters();
        this.StorageFilterItems.addAdditionalFilter("Code", "1,2,3,4,8,9,10,11,12", null, null, "Exclude", false, false, false, "string", false, true);
        this._TodayDate = DateTool.GetCurrentDateTimeAsUtc();

        if (this.RequestParams == null) {
            this.RequestParams = new SpecialActivityRequestParams();
            this.RequestParams.GeneralDetailsData = new GeneralDetails();
            this.RequestParams.GeneralDetailsData.CargoIdentifier = new CargoIdentifier();
            this.RequestParams.GoodsDetailsData = new GoodsDetails();
            this.RequestParams.RePackingApprovalDetailsData = new RePackingApprovalDetails();
            this.RequestParams.OtherActivityDetailsData = new OtherActivityDetails();

            this.SpecialActivityType = "6";

            if (this._CustomsSettingListService == null) {
                this._CustomsSettingListService = new CustomsSettingListService();
            }
            this._CustomsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
                .subscribe((customsSettingList: ServiceResponse) => {
                    if (customsSettingList != null) {
                        this.ApplicantAgentNumber = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;
                        this.UIProperties.SetEnabled("ApplicantAgentNumber", null, false);
                    }
                });

            this.UIProperties.SetRequired("ActivityRequestStartDate", null, true);
            this.UIProperties.SetRequired("ActivityRequestEndDate", null, true);
            this.UIProperties.SetRequired("SiteNumber", null, true);
            this.UIProperties.SetRequired("CargoIdentifierType", null, true);
            this.UIProperties.SetRequired("CargoIdentifierKey1", null, true);
        }

        if (this.ResponseData) {
            this.IsResponseMessageVisibility = true;
            if (this._IsLoadResponseData) {
                this.LoadResponseData();
            }
        }
    }

    SetMenuArg(MenuArg) {
        if (MenuArg) {
            this.OnMassageDisplayMethod();
            this.CustomFileNo = MenuArg.CustomFileNo;
            this.DeclarationId = MenuArg.DeclarationId;

            this.CustomFileNoTextChanged("");
        }
    }

    LoadResponseData() {
        if (this.ResponseData) {

            this.IsResponseMessageVisibility = true;

            if (this.RequestParams.GeneralDetailsData != null && this.RequestParams.GeneralDetailsData.CargoIdentifier != null) {
                this.CargoIdentifierType = this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierType.toString();
            }

            switch (this.RequestParams.GeneralDetailsData.SpecialActivityType) {
                case "5": //RePackingApproval
                case 5:
                    this.SpecialActivityType = "5";
                    this.SetIsRePackingApproval(true);
                    if (this.RequestParams.CurrentPackingDetailsDataList != null && this.RequestParams.CurrentPackingDetailsDataList != null) {
                        this.RequestParams.CurrentPackingDetailsDataList.forEach((item) => {
                            this.RepackingCurrentList.Insert(new RepackingCurrentRequestDetailsComponent(item));
                        });
                    }

                    if (this.RequestParams.DesiredPackingDetailsDataList != null && this.RequestParams.DesiredPackingDetailsDataList != null) {
                        this.RequestParams.DesiredPackingDetailsDataList.forEach((item) => {
                            this.RepackingDesiredList.Insert(new RepackingDesiredRequestDetailsComponent(item));
                        });
                    }
                    break;
                case "6": //GoodsDetails 
                case 6:
                    this.SpecialActivityType = "6";
                    this.SetIsGoodsDetailsData(true);
                    if (this.RequestParams.GoodsDetailsData != null) {
                        if (this.RequestParams.GoodsDetailsData.SpecialActionsCode != null) {
                            this.SpecialActionsCode = this.RequestParams.GoodsDetailsData.SpecialActionsCode.toString();
                        }
                        if (this.RequestParams.GoodsDetailsData.RepresentativeList != null) {
                            this.RequestParams.GoodsDetailsData.RepresentativeList.forEach((item) => {
                                this.RepresentativeList.Insert(new RepresentativeComponent(item));
                            });
                        }
                    }
                    break;
                case "7": //SampleRequest 
                case 7:
                    this.SpecialActivityType = "7";
                    this.SetIsSampleRequest(true);
                    if (this.RequestParams.SampleRequestDetailsDataList != null && this.RequestParams.SampleRequestDetailsDataList != null) {
                        this.RequestParams.SampleRequestDetailsDataList.forEach((item) => {
                            this.SampleRequestList.Insert(new SampleRequestDetailsComponent(item));
                        });
                    }
                    break;
                case "13"://Other
                case 13:
                    this.SpecialActivityType = "13";
                    this.SetIsOtherActivity(true);
                    break;
                default:
                    break;
            }
        }
    }

    //#region Properties

    private _SpecialActivityType: string;
    get SpecialActivityType() { return this._SpecialActivityType; }
    set SpecialActivityType(value: string) {
        if (this._SpecialActivityType != value) {
            this._SpecialActivityType = value;
        }

        switch (value) {
            case "5": //RePackingApproval
                this.SetIsRePackingApproval(true);
                break;
            case "6": //GoodsDetails 
                this.SetIsGoodsDetailsData(true);
                break;
            case "7": //SampleRequest 
                this.SetIsSampleRequest(true);
                break;
            case "13"://Other
                this.SetIsOtherActivity(true);
                break;
            default:
                break;
        }
        this.SetWarning(value);
    }

    get ApplicantAgentNumber() { return this.RequestParams.GeneralDetailsData.ApplicantAgentNumber; }
    set ApplicantAgentNumber(value: number) {
        if (this.RequestParams.GeneralDetailsData.ApplicantAgentNumber != value) {
            this.RequestParams.GeneralDetailsData.ApplicantAgentNumber = value;
        }
    }

    get SpecialActivityRequestNumber() { return this.RequestParams.GeneralDetailsData.SpecialActivityRequestNumber; }
    set SpecialActivityRequestNumber(value: string) {
        if (this.RequestParams.GeneralDetailsData.SpecialActivityRequestNumber != value) {
            this.RequestParams.GeneralDetailsData.SpecialActivityRequestNumber = value;
        }
    }

    get ActivityRequestStartDate() { return this.RequestParams.GeneralDetailsData.ActivityRequestStartDate; }
    set ActivityRequestStartDate(value: Date) {
        if (this.RequestParams.GeneralDetailsData.ActivityRequestStartDate != value) {
            this.RequestParams.GeneralDetailsData.ActivityRequestStartDate = value;
            this.ActivityRequestStartDateLostFocusMethod(value);
        }
        if (value && !AppTool.IsNullOrEmpty(this.ActivityRequestStartTime)) {
            this.UIProperties.SetRequired("ActivityRequestStartDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("ActivityRequestStartDate", null, true);

        }
    }

    get ActivityRequestStartTime() { return this.RequestParams.GeneralDetailsData.ActivityRequestStartTime ? this.RequestParams.GeneralDetailsData.ActivityRequestStartTime : null; }
    set ActivityRequestStartTime(value: Date) {
        if (this.RequestParams.GeneralDetailsData.ActivityRequestStartTime != value) {
            this.RequestParams.GeneralDetailsData.ActivityRequestStartTime = value;
        }
        if (value && !AppTool.IsNullOrEmpty(this.ActivityRequestStartDate)) {
            this.UIProperties.SetRequired("ActivityRequestStartDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("ActivityRequestStartDate", null, true);

        }
    }

    get ActivityRequestEndDate() { return this.RequestParams.GeneralDetailsData.ActivityRequestEndDate; }
    set ActivityRequestEndDate(value: Date) {
        if (this.RequestParams.GeneralDetailsData.ActivityRequestEndDate != value) {
            this.RequestParams.GeneralDetailsData.ActivityRequestEndDate = value;
            this.ActivityRequestEndDateLostFocusMethod(value);
        }
        if (value && !AppTool.IsNullOrEmpty(this.ActivityRequestEndTime)) {
            this.UIProperties.SetRequired("ActivityRequestEndDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("ActivityRequestEndDate", null, true);
        }
    }

    get ActivityRequestEndTime() { return this.RequestParams.GeneralDetailsData.ActivityRequestEndTime; }
    set ActivityRequestEndTime(value: Date) {
        if (this.RequestParams.GeneralDetailsData.ActivityRequestEndTime != value) {
            this.RequestParams.GeneralDetailsData.ActivityRequestEndTime = value;
        }
        if (value && !AppTool.IsNullOrEmpty(this.ActivityRequestEndDate)) {
            this.UIProperties.SetRequired("ActivityRequestEndDate", null, false);
        }
        else {
            this.UIProperties.SetRequired("ActivityRequestEndDate", null, true);

        }
    }

    get CustomFileNo() { return this.RequestParams.GeneralDetailsData.CustomFileNo; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.GeneralDetailsData.CustomFileNo != value) {
            this.RequestParams.GeneralDetailsData.CustomFileNo = value;
        }
    }

    get DeclarationId() { return this.RequestParams.GeneralDetailsData.DeclarationId; }
    set DeclarationId(value: string) {
        if (this.RequestParams.GeneralDetailsData.DeclarationId != value) {
            this.RequestParams.GeneralDetailsData.DeclarationId = value;
        }
    }

    get SiteNumber() { return this.RequestParams.GeneralDetailsData.SiteNumber; }
    set SiteNumber(value: string) {
        if (this.RequestParams.GeneralDetailsData.SiteNumber != value) {
            this.RequestParams.GeneralDetailsData.SiteNumber = value;
        }

        if (value) {
            this.UIProperties.SetRequired("SiteNumber", null, false);
        }
        else {
            this.UIProperties.SetRequired("SiteNumber", null, true);
        }
    }

    get WarehouseBlockNumber() { return this.RequestParams.GeneralDetailsData.WarehouseBlockNumber; }
    set WarehouseBlockNumber(value: number) {
        if (this.RequestParams.GeneralDetailsData.WarehouseBlockNumber != value) {
            this.RequestParams.GeneralDetailsData.WarehouseBlockNumber = value;
        }
    }

    private _CargoIdentifierType: string;
    get CargoIdentifierType() { return this._CargoIdentifierType; }
    set CargoIdentifierType(value: string) {
        if (this._CargoIdentifierType != value) {
            this._CargoIdentifierType = value;
        }

        if (value) {
            this.UIProperties.SetRequired("CargoIdentifierType", null, false);
        }
        else {
            this.UIProperties.SetRequired("CargoIdentifierType", null, true);
        }
    }

    get CargoIdentifierKey1() { return this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1; }
    set CargoIdentifierKey1(value: string) {
        if (this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1 != value) {
            this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1 = value;
        }

        if (value) {
            this.UIProperties.SetRequired("CargoIdentifierKey1", null, false);
        }
        else {
            this.UIProperties.SetRequired("CargoIdentifierKey1", null, true);
        }
    }

    get CargoIdentifierKey2() { return this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2; }
    set CargoIdentifierKey2(value: string) {
        if (this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2 != value) {
            this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2 = value;
        }

        if (this.IsRePackingApproval || this.IsSampleRequest) {
            if (value) {
                this.UIProperties.SetWarning("CargoIdentifierKey2", null, false);
            }
            else {
                this.UIProperties.SetWarning("CargoIdentifierKey2", null, true);
            }
        }
        else {
            this.UIProperties.SetWarning("CargoIdentifierKey2", null, false);
        }
    }

    get CargoIdentifierKey3() { return this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3; }
    set CargoIdentifierKey3(value: string) {
        if (this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3 != value) {
            this.RequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3 = value;
        }
    }

    get CargoRowNumber() { return this.RequestParams.GeneralDetailsData.CargoRowNumber; }
    set CargoRowNumber(value: string) {
        if (this.RequestParams.GeneralDetailsData.CargoRowNumber != value) {
            this.RequestParams.GeneralDetailsData.CargoRowNumber = value;
        }
        if (value || this.IsGoodsDetailsData == false) {
            this.UIProperties.SetWarning("CargoRowNumber", null, false);
        }
        else {
            this.UIProperties.SetWarning("CargoRowNumber", null, true);
        }
    }

    get AuthorityCode() { return this.RequestParams.GeneralDetailsData.AuthorityCode; }
    set AuthorityCode(value: string) {
        if (this.RequestParams.GeneralDetailsData.AuthorityCode != value) {
            this.RequestParams.GeneralDetailsData.AuthorityCode = value;
        }
        if (value || this.IsSampleRequest == false) {
            this.UIProperties.SetWarning("AuthorityCode", null, false);
        }
        else {
            this.UIProperties.SetWarning("AuthorityCode", null, true);
        }
    }

    get ImporterNumber() { return this.RequestParams.GeneralDetailsData.ImporterNumber; }
    set ImporterNumber(value: string) {
        if (this.RequestParams.GeneralDetailsData.ImporterNumber != value) {
            this.RequestParams.GeneralDetailsData.ImporterNumber = value;
        }
    }

    ImporterNumberTextChanged(code: string) {
        this.ImporterNumber = code;
        if (this.IsRePackingApproval || this.IsSampleRequest) {
            if (!AppTool.IsNullOrEmpty(this.ImporterNumber)) {
                this.UIProperties.SetWarning("ImporterNumber", "Customs.Client", false);
            }
            else {
                this.UIProperties.SetWarning("ImporterNumber", "Customs.Client", true);
            }
        }
    }

    get CheckSite() { return this.RequestParams.GeneralDetailsData.CheckSite; }
    set CheckSite(value: string) {
        if (this.RequestParams.GeneralDetailsData.CheckSite != value) {
            this.RequestParams.GeneralDetailsData.CheckSite = value;
        }
    }

    get SpecialActivityTypeEssence() { return this.RequestParams.GeneralDetailsData.SpecialActivityTypeEssence; }
    set SpecialActivityTypeEssence(value: string) {
        if (this.RequestParams.GeneralDetailsData.SpecialActivityTypeEssence != value) {
            this.RequestParams.GeneralDetailsData.SpecialActivityTypeEssence = value;
        }
    }

    get ResponseMessage() { return this.ResponseData ? this.ResponseData.UserMessage : null; }
    set ResponseMessage(value: string) {
        if (this.ResponseData.UserMessage != value) {
            this.ResponseData.UserMessage = value;
        }
    }

    get IsResponseMessageVisibility() { return this._IsResponseMessageVisibility; }
    set IsResponseMessageVisibility(newValue: boolean) {
        if (this._IsResponseMessageVisibility != newValue) {
            this._IsResponseMessageVisibility = newValue;
        }
    }

    //#endregion Properties


    //#region GoodsDetailsData
    SetIsGoodsDetailsData(newValue: boolean) {
        this.IsGoodsDetailsData = newValue;
        //this.UIProperties.SetRequired("DeclarationNumber", null, true);
    }

    get IsGoodsDetailsData() { return this._IsGoodsDetailsData; }
    set IsGoodsDetailsData(newValue: boolean) {
        if (this._IsGoodsDetailsData != newValue) {
            this._IsGoodsDetailsData = newValue;
            if (newValue == true) {
                this.SetIsOtherActivity(false);
                this.SetIsRePackingApproval(false);
                this.SetIsSampleRequest(false);
            }
        }
    }

    get IdemanderType() { return this.RequestParams.GoodsDetailsData != null ? this.RequestParams.GoodsDetailsData.IdemanderType : null; }
    set IdemanderType(value: number) {
        if (this.RequestParams.GoodsDetailsData.IdemanderType != value) {
            this.RequestParams.GoodsDetailsData.IdemanderType = value;
        }
        if (value || this.IsGoodsDetailsData == false) {
            this.UIProperties.SetWarning("IdemanderType", null, false);
        }
        else {
            this.UIProperties.SetWarning("IdemanderType", null, true);
        }
    }

    private _SpecialActionsCode: string;
    get SpecialActionsCode() { return this._SpecialActionsCode; }
    set SpecialActionsCode(value: string) {
        if (this._SpecialActionsCode != value) {
            this._SpecialActionsCode = value;
        }
        if (value || this.IsGoodsDetailsData == false) {
            this.UIProperties.SetWarning("SpecialActionsCode", null, false);
        }
        else {
            this.UIProperties.SetWarning("SpecialActionsCode", null, true);
        }
    }

    get OtherDescription() { return this.RequestParams.GoodsDetailsData != null ? this.RequestParams.GoodsDetailsData.OtherDescription : null; }
    set OtherDescription(value: string) {
        if (this.RequestParams.GoodsDetailsData.OtherDescription != value) {
            this.RequestParams.GoodsDetailsData.OtherDescription = value;
        }
        if (value || this.IsGoodsDetailsData == false) {
            this.UIProperties.SetWarning("OtherDescription", null, false);
        }
        else {
            this.UIProperties.SetWarning("OtherDescription", null, true);
        }
    }

    get GoodsDescription() { return this.RequestParams.GoodsDetailsData != null ? this.RequestParams.GoodsDetailsData.GoodsDescription : null; }
    set GoodsDescription(value: string) {
        if (this.RequestParams.GoodsDetailsData.GoodsDescription != value) {
            this.RequestParams.GoodsDetailsData.GoodsDescription = value;
        }
        if (value || this.IsGoodsDetailsData == false) {
            this.UIProperties.SetWarning("GoodsDescription", null, false);
        }
        else {
            this.UIProperties.SetWarning("GoodsDescription", null, true);
        }
    }

    AddRepresentativeItemCommand() {
        this.RepresentativeList.Insert(new RepresentativeComponent(new RepresentativeDetails()));
        this.SetRepresentativeRowNumber();
    }

    DeleteRepresentativeDetailsCommand(item: RepresentativeComponent) {
        this.RepresentativeList.Remove(item);
        this.SetRepresentativeRowNumber();
    }

    private SetRepresentativeRowNumber() {
        let representativeRowNumber: number = 0;
        for (let item of this.RepresentativeList.Collection) {
            representativeRowNumber = representativeRowNumber + 1;
            item.RepresentativeNumber = representativeRowNumber;
        }
    }
    //#endregion GoodsDetailsData


    //#region OtherActivity Properties
    SetIsOtherActivity(newValue: boolean) {
        this.IsOtherActivity = newValue;
    }

    get IsOtherActivity() { return this._IsOtherActivity; }
    set IsOtherActivity(newValue: boolean) {
        if (this._IsOtherActivity != newValue) {
            this._IsOtherActivity = newValue;
            if (newValue == true) {
                this.SetIsGoodsDetailsData(false);
                this.SetIsRePackingApproval(false);
                this.SetIsSampleRequest(false);
            }
        }
    }

    get OtherActivityComment() { return this.RequestParams.OtherActivityDetailsData.OtherActivityComment; }
    set OtherActivityComment(value: string) {
        if (this.RequestParams.OtherActivityDetailsData.OtherActivityComment != value) {
            this.RequestParams.OtherActivityDetailsData.OtherActivityComment = value;
        }
    }
    //#endregion OtherActivity Properties


    //#region RePackingApproval
    SetIsRePackingApproval(newValue: boolean) {
        this.IsRePackingApproval = newValue;
    }

    get IsRePackingApproval() { return this._IsRePackingApproval; }
    set IsRePackingApproval(newValue: boolean) {
        if (this._IsRePackingApproval != newValue) {
            this._IsRePackingApproval = newValue;
            if (newValue == true) {
                this.SetIsGoodsDetailsData(false);
                this.SetIsOtherActivity(false);
                this.SetIsSampleRequest(false);
            }
        }
    }

    get RepackingSiteNumber() { return this.RequestParams.RePackingApprovalDetailsData.SiteNumber; }
    set RepackingSiteNumber(value: string) {
        if (this.RequestParams.RePackingApprovalDetailsData.SiteNumber != value) {
            this.RequestParams.RePackingApprovalDetailsData.SiteNumber = value;
        }
        if (value || this.IsRePackingApproval == false) {
            this.UIProperties.SetWarning("RepackingSiteNumber", null, false);
        }
        else {
            this.UIProperties.SetWarning("RepackingSiteNumber", null, true);
        }
    }

    get ApprovalDate() { return this.RequestParams.RePackingApprovalDetailsData.ApprovalDate; }
    set ApprovalDate(value: Date) {
        if (this.RequestParams.RePackingApprovalDetailsData.ApprovalDate != value) {
            this.RequestParams.RePackingApprovalDetailsData.ApprovalDate = value;
        }
        if (value || this.IsRePackingApproval == false) {
            this.UIProperties.SetWarning("ApprovalDate", null, false);
        }
        else {
            this.UIProperties.SetWarning("ApprovalDate", null, true);
        }
    }

    get ApprovalName() { return this.RequestParams.RePackingApprovalDetailsData.ApprovalName; }
    set ApprovalName(value: string) {
        if (this.RequestParams.RePackingApprovalDetailsData.ApprovalName != value) {
            this.RequestParams.RePackingApprovalDetailsData.ApprovalName = value;
        }
        if (value || this.IsRePackingApproval == false) {
            this.UIProperties.SetWarning("ApprovalName", null, false);
        }
        else {
            this.UIProperties.SetWarning("ApprovalName", null, true);
        }
    }

    AddRepackingCurrentItemCommand() {
        this.RepackingCurrentList.Insert(new RepackingCurrentRequestDetailsComponent(new CurrentPackingDetails()));
        this.SetRepackingCurrentRowNumber();
    }

    DeleteRepackingCurrentItemCommand(item: RepackingCurrentRequestDetailsComponent) {
        this.RepackingCurrentList.Remove(item);
        this.SetRepackingCurrentRowNumber();
    }

    private SetRepackingCurrentRowNumber() {
        let rowNumber: number = 0;
        for (let item of this.RepackingCurrentList.Collection) {
            rowNumber = rowNumber + 1;
            item.CurrentRePackingOldLineNumber = rowNumber;
        }
    }

    AddRepackingDesiredItemCommand() {
        this.RepackingDesiredList.Insert(new RepackingDesiredRequestDetailsComponent(new DesiredPackingDetails()));
        this.SetRepackingDesiredRowNumber();
    }

    DeleteRepackingDesiredRequestDetailsCommand(item: RepackingDesiredRequestDetailsComponent) {
        this.RepackingDesiredList.Remove(item);
        this.SetRepackingDesiredRowNumber();
    }

    private SetRepackingDesiredRowNumber() {
        let rowNumber: number = 0;
        for (let item of this.RepackingDesiredList.Collection) {
            rowNumber = rowNumber + 1;
            item.DesiredRePackingNewLineNumber = rowNumber;
        }
    }
    //#endregion RePackingApproval


    //#region SampleRequest
    SetIsSampleRequest(newValue: boolean) {
        this.IsSampleRequest = newValue;
    }

    get IsSampleRequest() { return this._IsSampleRequest; }
    set IsSampleRequest(newValue: boolean) {
        if (this._IsSampleRequest != newValue) {
            this._IsSampleRequest = newValue;
            if (newValue == true) {
                this.SetIsOtherActivity(false);
                this.SetIsRePackingApproval(false);
                this.SetIsGoodsDetailsData(false);
            }
        }
    }

    AddSampleRequestItemCommand() {
        this.SampleRequestList.Insert(new SampleRequestDetailsComponent(new SampleRequestDetails()));
        this.SetSampleRequestRowNumber();
    }

    DeleteSampleRequestDetailsCommand(item: SampleRequestDetailsComponent) {
        this.SampleRequestList.Remove(item);
        this.SetSampleRequestRowNumber();
    }

    private SetSampleRequestRowNumber() {
        let rowNumber: number = 0;
        for (let item of this.SampleRequestList.Collection) {
            rowNumber = rowNumber + 1;
            item.SampleRowNumber = rowNumber;
        }
    }
    //#endregion GoodsDetailsData


    //#region Properties Commands
    SetWarning(specialActivityType: string) {
        this.UIProperties.SetWarning("CargoIdentifierKey2", null, false);
        this.UIProperties.SetWarning("ImporterNumber", null, false);
        this.UIProperties.SetWarning("CargoRowNumber", null, false);
        this.UIProperties.SetWarning("AuthorityCode", null, false);

        switch (specialActivityType) {
            case "5": //RePackingApproval
                this.UIProperties.SetWarning("CargoIdentifierKey2", null, true);
                this.UIProperties.SetWarning("ImporterNumber", null, true);
                this.UIProperties.SetWarning("ApprovalDate", null, true);
                this.UIProperties.SetWarning("RepackingSiteNumber", null, true);
                this.UIProperties.SetWarning("ApprovalName", null, true);
                break;
            case "6": //GoodsDetails 
                this.UIProperties.SetWarning("CargoRowNumber", null, true);
                this.UIProperties.SetWarning("IdemanderType", null, true);
                this.UIProperties.SetWarning("SpecialActionsCode", null, true);
                this.UIProperties.SetWarning("OtherDescription", null, true);
                this.UIProperties.SetWarning("GoodsDescription", null, true);
                break;
            case "7": //SampleRequest 
                this.UIProperties.SetWarning("ImporterNumber", null, true);
                this.UIProperties.SetWarning("CargoIdentifierKey2", null, true);
                this.UIProperties.SetWarning("AuthorityCode", null, true);
                break;
            default:
                break;
        }
    }

    ActivityRequestStartDateLostFocusMethod(startDateItem: Date) {

        this.ValidationErrorsList = [];
        this.ActivityRequestStartTime = null;

        if (AppTool.IsNullOrEmpty(startDateItem)) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(startDateItem) && startDateItem > this.ActivityRequestEndDate) {
            var msg = "תאריך תחילת הפעולה לא יכול להיות אחרי תאריך סיום הפעולה";
            this.ValidationErrorsList.push(msg);
        }

        if (startDateItem.getDate() == this._TodayDate.getDate()) {
            var date: Date = new Date();
            date.setMinutes(0);
            this.ActivityRequestStartTime = DateTool.AddHour(date, 3);
        }
    }

    ActivityRequestEndDateLostFocusMethod(endDateItem: Date) {
        this.ValidationErrorsList = [];
        this.ActivityRequestEndTime = null;

        if (AppTool.IsNullOrEmpty(endDateItem)) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(this.ActivityRequestStartDate) && this.ActivityRequestStartDate > endDateItem) {
            var msg = "תאריך תחילת הפעולה לא יכול להיות אחרי תאריך סיום הפעולה";
            this.ValidationErrorsList.push(msg);
        }

        if (endDateItem.getDate() == this._TodayDate.getDate()) {
            var date: Date = new Date();
            date.setMinutes(0);
            this.ActivityRequestEndTime = DateTool.AddHour(date, 3);
        }
    }

    //#endregion Properties Commands


    //#region Declaration Commands
    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.DeclarationId = "";
            this.CargoIdentifierType = "";
            this.CargoIdentifierKey1 = "";
            this.CargoIdentifierKey2 = "";
            this.CargoIdentifierKey3 = "";
            this.SiteNumber = "";
            this.ImporterNumber = "";
        } else {

            this.CustomFileNo = "";
        }
    }

    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.DeclarationId = "";
            this.CargoIdentifierType = "";
            this.CargoIdentifierKey1 = "";
            this.CargoIdentifierKey2 = "";
            this.CargoIdentifierKey3 = "";
            this.ImporterNumber = "";
            this.SiteNumber = "";

            //this.UIProperties.SetEnabled("SiteNumber", "Customs.SiteLookup", true);
            //this.UIProperties.SetEnabled("CargoIdentifierType", "Customs.CargoIdentifireType", true);
            //this.UIProperties.SetEnabled("CargoIdentifierKey1", null, true);
            //this.UIProperties.SetEnabled("CargoIdentifierKey2", null, true);
            //this.UIProperties.SetEnabled("CargoIdentifierKey3", null, true);
            //this.UIProperties.SetEnabled("ImporterNumber", "Customs.Client", true);
            return;
        }

        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetConsignmentListPMByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclarationConsignment(myResponse, true);
            });
    }

    FetchDeclarationConsignment(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            lastFetchDeclarationList = lastFetchDeclarationList[0];
            this.DeclarationId = lastFetchDeclarationList.DeclarationId;
            this.CargoIdentifierType = lastFetchDeclarationList.CargoTypeCode;
            this.CargoIdentifierKey1 = lastFetchDeclarationList.ManifestNumber;
            this.CargoIdentifierKey2 = lastFetchDeclarationList.SecondCargoID;
            this.CargoIdentifierKey3 = lastFetchDeclarationList.ThirdCargoID;
            this.SiteNumber = lastFetchDeclarationList.StorageSiteCode;

            //this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            //this.UIProperties.SetEnabled("SiteNumber", "Customs.SiteLookup", false);
            //this.UIProperties.SetEnabled("CargoIdentifierType", "Customs.CargoIdentifireType", false);
            //this.UIProperties.SetEnabled("CargoIdentifierKey1", null, false);
            //this.UIProperties.SetEnabled("CargoIdentifierKey2", null, false);
            //this.UIProperties.SetEnabled("CargoIdentifierKey3", null, false);
            //this.UIProperties.SetEnabled("ImporterNumber", this.ObjectTableName, false);

            this.CurrentSession.StartBusyIndicator("");
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();
                    this.FetchDeclaration(myResponse, true);
                });

        }
        else {
            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            } else {
                //this.SetValidityDeclarationNumber();
            }

        }
    }

    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.ImporterNumber = lastFetchDeclarationList.ImporterCode;
        }
    }

    SetValidityCustomFileNo() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    }
    //#endregion Declaration Commands


    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (AppTool.IsNullOrEmpty(this.ActivityRequestStartDate) || AppTool.IsNullOrEmpty(this.ActivityRequestEndDate)
            || AppTool.IsNullOrEmpty(this.ActivityRequestStartTime) || AppTool.IsNullOrEmpty(this.ActivityRequestEndTime)) {
            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.ActivityRequestStartEndDateMandatory");
            if (AppTool.IsNullOrEmpty(this.ActivityRequestStartTime) || AppTool.IsNullOrEmpty(this.ActivityRequestEndTime)) {
                msg = msg + " (כולל שעות)";
            }
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.SiteNumber)) {
            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SiteNumberMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.CargoIdentifierType)) {
            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierTypMandatory");
            this.ValidationErrorsList.push(msg);
        }

        if (AppTool.IsNullOrEmpty(this.CargoIdentifierKey1)) {
            var msg = TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CargoIdentifierKey1Mandatory");
            this.ValidationErrorsList.push(msg);
        }

        switch (this.SpecialActivityType) {
            case "5": //RePackingApproval
                if (this.RepackingCurrentList.Length == 0) {
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.RepackingCurrentItemsItemsMandatory"));
                }
                if (this.RepackingDesiredList.Length == 0) {
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.RepackingDesiredItemsItemsMandatory"));
                }
                break;
            case "6": //GoodsDetails 
                if (this.RepresentativeList.Length == 0) {
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.RepresentativeItemsItemsMandatory"));
                }
                break;
            case "7": //SampleRequest 
                if (this.SampleRequestList.Length == 0) {
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SampleItemsMandatory"));
                }
                else {
                    this.SampleRequestList.Collection.forEach((item) => {
                        if (AppTool.IsNullOrEmpty(item.SampleReturnDate)) {
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SampleReturnDateMandatory"));
                        }
                        if (AppTool.IsNullOrEmpty(item.SampleValue)) {
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SampleValueMandatory"));
                        }
                        if (AppTool.IsNullOrEmpty(item.CurrencyTypeCode)) {
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.CurrencyTypeCodeMandatory"));
                        }
                        if (AppTool.IsNullOrEmpty(item.SampleDescription)) {
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.SpecialActivityRequest.F.SampleDescriptionMandatory"));
                        }
                    });
                }

                break;
            default:
                break;
        }
    }

    CheckBeforeSend() {
        var isMissingWarningField: boolean = false;
        var userMessage: string = "לא הוזנו כל השדות המומלצים (מסומנים בצהוב), האם ברצונך לשלוח בכל זאת?";

        switch (this.SpecialActivityType) {
            case "5": //RePackingApproval
                if (AppTool.IsNullOrEmpty(this.CargoIdentifierKey2) || AppTool.IsNullOrEmpty(this.RepackingSiteNumber) || AppTool.IsNullOrEmpty(this.ApprovalDate)
                    || AppTool.IsNullOrEmpty(this.ApprovalName) || AppTool.IsNullOrEmpty(this.ImporterNumber)) {
                    isMissingWarningField = true;
                }
                break;
            case "6": //GoodsDetails
                if (AppTool.IsNullOrEmpty(this.CargoRowNumber) || AppTool.IsNullOrEmpty(this.IdemanderType) || AppTool.IsNullOrEmpty(this.OtherDescription)
                    || AppTool.IsNullOrEmpty(this.SpecialActionsCode) || AppTool.IsNullOrEmpty(this.GoodsDescription)) {
                    isMissingWarningField = true;
                }

                if (this.RepackingCurrentList.Length > 0) {
                    this.RepackingCurrentList.Collection.forEach((item) => {
                        if (AppTool.IsNullOrEmpty(item.RepresentativeName) || AppTool.IsNullOrEmpty(item.RepresentativeID)) {
                            isMissingWarningField = true;
                        }
                    });
                }
                break;
            case "7": //SampleRequest 
                if (AppTool.IsNullOrEmpty(this.CargoIdentifierKey2) || AppTool.IsNullOrEmpty(this.AuthorityCode) || AppTool.IsNullOrEmpty(this.ImporterNumber)) {
                    isMissingWarningField = true;
                }
                break;
            default:
                break;
        }

        if (isMissingWarningField) {
            var confirmWindow: ConfirmWindow = new ConfirmWindow();
            confirmWindow.Width = 250;
            confirmWindow.Height = 150;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
            confirmWindow.ShowCancelButton = false;
            confirmWindow.Show(userMessage);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.SendSpecialActivityRequestMethod()
                }
            });
        }
        else {
            this.SendSpecialActivityRequestMethod()
        }
    }

    private _CustomSendOptionsArgs: CustomSendOptionsArgs;
    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this._CustomSendOptionsArgs = customSendOptionsArgs;
        this.CheckBeforeSend();
    }

    SendSpecialActivityRequestMethod() {

        var currRequestParams = new SpecialActivityRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = this._CustomSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = this._CustomSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.AppicationId = this.RequestId.toString();
        //GeneralDetails
        currRequestParams.GeneralDetailsData = new GeneralDetails();
        currRequestParams.GeneralDetailsData.SpecialActivityRequestNumber = this.SpecialActivityRequestNumber;
        currRequestParams.GeneralDetailsData.ActivityRequestStartDate = this.ActivityRequestStartDate;
        if (this.ActivityRequestStartTime != null) {
            var startDate = this.ActivityRequestStartDate;
            startDate.setHours(this.ActivityRequestStartTime.getHours());
            startDate.setMinutes(this.ActivityRequestStartTime.getMinutes());
            currRequestParams.GeneralDetailsData.ActivityRequestStartDate = startDate;
            currRequestParams.GeneralDetailsData.ActivityRequestStartTime = this.ActivityRequestStartTime;
        }
        currRequestParams.GeneralDetailsData.ActivityRequestEndDate = this.ActivityRequestEndDate;
        if (this.ActivityRequestEndTime != null) {
            var endDate = this.ActivityRequestEndDate;
            endDate.setHours(this.ActivityRequestEndTime.getHours());
            endDate.setMinutes(this.ActivityRequestEndTime.getMinutes());
            currRequestParams.GeneralDetailsData.ActivityRequestEndDate = endDate;
            currRequestParams.GeneralDetailsData.ActivityRequestEndTime = this.ActivityRequestEndTime;
        }
        currRequestParams.GeneralDetailsData.CustomFileNo = this.CustomFileNo;
        currRequestParams.GeneralDetailsData.DeclarationId = this.DeclarationId;
        currRequestParams.GeneralDetailsData.SiteNumber = this.SiteNumber;
        currRequestParams.GeneralDetailsData.WarehouseBlockNumber = this.WarehouseBlockNumber;
        currRequestParams.GeneralDetailsData.CargoRowNumber = this.CargoRowNumber;
        currRequestParams.GeneralDetailsData.CargoRowNumberSpecified = this.CargoRowNumber == null ? false : true;
        currRequestParams.GeneralDetailsData.AuthorityCode = this.AuthorityCode;
        currRequestParams.GeneralDetailsData.AuthorityCodeSpecified = this.AuthorityCode == null ? false : true;
        currRequestParams.GeneralDetailsData.SpecialActivityType = Number(this.SpecialActivityType);
        currRequestParams.GeneralDetailsData.ImporterNumber = this.ImporterNumber;
        currRequestParams.GeneralDetailsData.ImporterNumberSpecified = this.ImporterNumber == null ? false : true;
        currRequestParams.GeneralDetailsData.ApplicantAgentNumber = this.ApplicantAgentNumber;
        currRequestParams.GeneralDetailsData.CheckSite = this.CheckSite;
        currRequestParams.GeneralDetailsData.SpecialActivityTypeEssence = this.SpecialActivityTypeEssence;

        //CargoIdentifier
        currRequestParams.GeneralDetailsData.CargoIdentifier = new CargoIdentifier();
        var cargoIdentifierType = <number><any>this.CargoIdentifierType;
        currRequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierType = cargoIdentifierType;
        currRequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey1 = this.CargoIdentifierKey1;
        currRequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey2 = this.CargoIdentifierKey2;
        currRequestParams.GeneralDetailsData.CargoIdentifier.CargoIdentifierKey3 = this.CargoIdentifierKey3;

        switch (this.SpecialActivityType) {
            case "5": //RePackingApproval
                {
                    currRequestParams.RePackingApprovalDetailsData = new RePackingApprovalDetails();
                    currRequestParams.RePackingApprovalDetailsData.ApprovalName = this.ApprovalName;
                    currRequestParams.RePackingApprovalDetailsData.ApprovalDateSpecified = this.ApprovalDate == null ? false : true;
                    currRequestParams.RePackingApprovalDetailsData.ApprovalDate = this.ApprovalDate;
                    currRequestParams.RePackingApprovalDetailsData.SiteNumber = this.SiteNumber;

                    if (this.RepackingCurrentList != null && this.RepackingCurrentList.Length > 0) {
                        currRequestParams.CurrentPackingDetailsDataList = [];

                        this.RepackingCurrentList.Collection.forEach((repackingCurrentItem) => {
                            var currentPackingDetails: CurrentPackingDetails = new CurrentPackingDetails();
                            currentPackingDetails.RePackingOldLineNumber = repackingCurrentItem.CurrentRePackingOldLineNumber;
                            currentPackingDetails.PresentPackingStateContent = repackingCurrentItem.CurrentPresentPackingStateContent;
                            currentPackingDetails.RePackingOldLineNumberSpecified = repackingCurrentItem.CurrentRePackingOldLineNumber == null ? false : true;

                            var packingDetails: PackingDetails = new PackingDetails();
                            packingDetails.PackageId = repackingCurrentItem.RepackingCurrentPackageId;
                            packingDetails.PackageType = repackingCurrentItem.RepackingCurrentPackageType;
                            packingDetails.PackageTypeName = repackingCurrentItem.RepackingCurrentPackageName;
                            packingDetails.Quantity = repackingCurrentItem.RepackingCurrentQuantity;
                            packingDetails.Weight = repackingCurrentItem.RepackingCurrentWeight;
                            packingDetails.WeightSpecified = repackingCurrentItem.RepackingCurrentWeight == null ? false : true;
                            currentPackingDetails.PackingDetails = packingDetails;

                            currRequestParams.CurrentPackingDetailsDataList.push(currentPackingDetails);
                        });
                    }

                    if (this.RepackingDesiredList != null && this.RepackingDesiredList.Length > 0) {
                        currRequestParams.DesiredPackingDetailsDataList = [];

                        this.RepackingDesiredList.Collection.forEach((repackingDesiredItem) => {
                            var desiredPackingDetails: DesiredPackingDetails = new DesiredPackingDetails();
                            desiredPackingDetails.RePackingOldLineNumber = repackingDesiredItem.DesiredRePackingOldLineNumber;
                            desiredPackingDetails.RePackingNewLineNumber = repackingDesiredItem.DesiredRePackingNewLineNumber;
                            desiredPackingDetails.RePackingOldLineNumberSpecified = repackingDesiredItem.DesiredRePackingOldLineNumber == null ? false : true;
                            desiredPackingDetails.RePackingNewLineNumberSpecified = repackingDesiredItem.DesiredRePackingNewLineNumber == null ? false : true;

                            var packingDetails: PackingDetails = new PackingDetails();
                            packingDetails.PackageId = repackingDesiredItem.RepackingDesiredPackageId;
                            packingDetails.PackageType = repackingDesiredItem.RepackingDesiredPackageType;
                            packingDetails.PackageTypeName = repackingDesiredItem.RepackingDesiredPackageName;
                            packingDetails.Quantity = repackingDesiredItem.RepackingDesiredQuantity;
                            packingDetails.Weight = repackingDesiredItem.RepackingDesiredWeight;
                            packingDetails.WeightSpecified = repackingDesiredItem.RepackingDesiredWeight == null ? false : true;
                            desiredPackingDetails.PackingDetails = packingDetails;

                            currRequestParams.DesiredPackingDetailsDataList.push(desiredPackingDetails);
                        });
                    }
                    currRequestParams.RequestName = "Special Activity Request - Repacking";
                    currRequestParams.ResponseName = "Special Activity Request - Repacking";

                    break;
                }
            case "6": //GoodsDetails 
                {
                    currRequestParams.GoodsDetailsData = new GoodsDetails();
                    currRequestParams.GoodsDetailsData.IdemanderType = this.IdemanderType;
                    currRequestParams.GoodsDetailsData.IdemanderTypeSpecified = this.IdemanderType == null ? false : true;
                    var specialActionsCode = <number><any>this.SpecialActionsCode;
                    currRequestParams.GoodsDetailsData.SpecialActionsCode = specialActionsCode;
                    currRequestParams.GoodsDetailsData.SpecialActionsCodeSpecified = specialActionsCode > 0 ? true : false;
                    currRequestParams.GoodsDetailsData.GoodsDescription = this.GoodsDescription;
                    currRequestParams.GoodsDetailsData.OtherDescription = this.OtherDescription;

                    if (this.RepresentativeList != null && this.RepresentativeList.Length > 0) {
                        currRequestParams.GoodsDetailsData.RepresentativeList = [];

                        this.RepresentativeList.Collection.forEach((representativeItem) => {
                            var representativeDetails: RepresentativeDetails = new RepresentativeDetails();
                            representativeDetails.RepresentativeNumber = representativeItem.RepresentativeNumber;
                            representativeDetails.RepresentativeName = representativeItem.RepresentativeName;
                            representativeDetails.RepresentativeID = representativeItem.RepresentativeID;
                            representativeDetails.RepresentativeNumberSpecified = representativeItem.RepresentativeNumber == null ? false : true;
                            representativeDetails.RepresentativeIDSpecified = representativeItem.RepresentativeID == null ? false : true;

                            currRequestParams.GoodsDetailsData.RepresentativeList.push(representativeDetails);
                        });
                    }
                    currRequestParams.RequestName = "Special Activity Request - Goods Details";
                    currRequestParams.ResponseName = "Special Activity Request - Goods Details";
                    break;
                }
            case "7": //SampleRequest 
                {
                    if (this.SampleRequestList != null && this.SampleRequestList.Length > 0) {
                        currRequestParams.SampleRequestDetailsDataList = [];

                        this.SampleRequestList.Collection.forEach((sampleItem) => {
                            var sampleRequestDetails: SampleRequestDetails = new SampleRequestDetails();
                            sampleRequestDetails.SampleRowNumber = sampleItem.SampleRowNumber;
                            sampleRequestDetails.SampleReturnDate = sampleItem.SampleReturnDate;
                            sampleRequestDetails.CustomsItem = sampleItem.CustomsItem;
                            sampleRequestDetails.CustomsItemQuantity = sampleItem.CustomsItemQuantity;
                            sampleRequestDetails.SampleValue = sampleItem.SampleValue;
                            sampleRequestDetails.CurrencyTypeCode = sampleItem.CurrencyTypeCode;
                            sampleRequestDetails.CurrencyTypeName = sampleItem.CurrencyTypeName;
                            sampleRequestDetails.SampleDescription = sampleItem.SampleDescription;
                            sampleRequestDetails.SampleRowNumberSpecified = sampleItem.SampleRowNumber == null ? false : true;
                            sampleRequestDetails.CustomsItemQuantitySpecified = sampleItem.CustomsItemQuantity == null ? false : true;

                            var packingDetails: PackingDetails = new PackingDetails();
                            packingDetails.PackageId = sampleItem.SamplePackageId;
                            packingDetails.PackageType = sampleItem.SamplePackageType;
                            packingDetails.PackageTypeName = sampleItem.SamplePackageName;
                            packingDetails.Quantity = sampleItem.SampleQuantity;
                            packingDetails.Weight = sampleItem.SampleWeight;
                            packingDetails.WeightSpecified = sampleItem.SampleWeight == null ? false : true;
                            sampleRequestDetails.SamplePackingDetails = packingDetails;

                            currRequestParams.SampleRequestDetailsDataList.push(sampleRequestDetails);
                        });
                    }
                    currRequestParams.RequestName = "Special Activity Request - Sample request";
                    currRequestParams.ResponseName = "Special Activity Request - Sample request";
                    break;
                }
            case "13": //Other
                currRequestParams.OtherActivityDetailsData = new OtherActivityDetails();
                currRequestParams.OtherActivityDetailsData.OtherActivityComment = this.OtherActivityComment;
                break;
        }


        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת בקשה לפעולות מיוחדות", true)
            .then((res) => {
                this.ResponseData = res;
                this._IsLoadResponseData = false;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._IIGGeneralMessagesService.PostSpecialActivityRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }
    //#endregion Commands
}

export class RepresentativeComponent extends BaseComponent {
    public DataContext: RepresentativeComponent = this;
    private representativeNumber: number;
    private representativeName: string;
    private representativeID: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(representativeDetails: RepresentativeDetails) {
        super();
        this.RepresentativeNumber = representativeDetails.RepresentativeNumber;
        this.RepresentativeName = representativeDetails.RepresentativeName;
        this.RepresentativeID = representativeDetails.RepresentativeID;
    }

    public get RepresentativeNumber() { return this.representativeNumber; }
    public set RepresentativeNumber(newValue: number) { this.representativeNumber = newValue; }

    public get RepresentativeName() { return this.representativeName; }
    public set RepresentativeName(newValue: string) { this.representativeName = newValue; }

    public get RepresentativeID() { return this.representativeID; }
    public set RepresentativeID(newValue: string) { this.representativeID = newValue; }
}

export class RepackingCurrentRequestDetailsComponent extends BaseComponent {
    public DataContext: RepackingCurrentRequestDetailsComponent = this;
    private currentRePackingOldLineNumber: number;
    private repackingCurrentPackageType: string;
    private repackingCurrentPackageName: string;
    private currentPresentPackingStateContent: string;
    private repackingCurrentQuantity: string;
    private repackingCurrentPackageId: string;
    private repackingCurrentWeight: number;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(currentPackingDetails: CurrentPackingDetails) {
        super();

        this.CurrentRePackingOldLineNumber = currentPackingDetails.RePackingOldLineNumber;
        this.CurrentPresentPackingStateContent = currentPackingDetails.PresentPackingStateContent;
        if (currentPackingDetails.PackingDetails != null) {
            this.RepackingCurrentPackageId = currentPackingDetails.PackingDetails.PackageId;
            this.RepackingCurrentPackageType = currentPackingDetails.PackingDetails.PackageType;
            this.RepackingCurrentPackageName = currentPackingDetails.PackingDetails.PackageTypeName;
            this.RepackingCurrentQuantity = currentPackingDetails.PackingDetails.Quantity;
            this.RepackingCurrentWeight = currentPackingDetails.PackingDetails.Weight;
        }
    }

    public get CurrentRePackingOldLineNumber() { return this.currentRePackingOldLineNumber; }
    public set CurrentRePackingOldLineNumber(newValue: number) { this.currentRePackingOldLineNumber = newValue; }

    public get RepackingCurrentPackageName() { return this.repackingCurrentPackageName; }
    public set RepackingCurrentPackageName(newValue: string) { this.repackingCurrentPackageName = newValue; }

    public get RepackingCurrentPackageType() { return this.repackingCurrentPackageType; }
    public set RepackingCurrentPackageType(newValue: string) { this.repackingCurrentPackageType = newValue; }

    public get RepackingCurrentPackageId() { return this.repackingCurrentPackageId; }
    public set RepackingCurrentPackageId(newValue: string) { this.repackingCurrentPackageId = newValue; }

    public get RepackingCurrentQuantity() { return this.repackingCurrentQuantity; }
    public set RepackingCurrentQuantity(newValue: string) { this.repackingCurrentQuantity = newValue; }

    public get RepackingCurrentWeight() { return this.repackingCurrentWeight; }
    public set RepackingCurrentWeight(newValue: number) { this.repackingCurrentWeight = newValue; }

    public get CurrentPresentPackingStateContent() { return this.currentPresentPackingStateContent; }
    public set CurrentPresentPackingStateContent(newValue: string) { this.currentPresentPackingStateContent = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }
}

export class RepackingDesiredRequestDetailsComponent extends BaseComponent {
    public DataContext: RepackingDesiredRequestDetailsComponent = this;
    private desiredRePackingNewLineNumber: number;
    private repackingDesiredPackageType: string;
    private repackingDesiredPackageName: string;
    private repackingDesiredPackageId: string;
    private repackingDesiredQuantity: string;
    private repackingDesiredWeight: number;
    private desiredRePackingOldLineNumber: number;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(desiredPackingDetails: DesiredPackingDetails) {
        super();

        this.DesiredRePackingNewLineNumber = desiredPackingDetails.RePackingNewLineNumber;
        this.DesiredRePackingOldLineNumber = desiredPackingDetails.RePackingOldLineNumber;
        if (desiredPackingDetails.PackingDetails != null) {
            this.RepackingDesiredPackageId = desiredPackingDetails.PackingDetails.PackageId;
            this.RepackingDesiredPackageType = desiredPackingDetails.PackingDetails.PackageType;
            this.RepackingDesiredPackageName = desiredPackingDetails.PackingDetails.PackageTypeName;
            this.RepackingDesiredQuantity = desiredPackingDetails.PackingDetails.Quantity;
            this.RepackingDesiredWeight = desiredPackingDetails.PackingDetails.Weight;
        }
    }

    public get DesiredRePackingNewLineNumber() { return this.desiredRePackingNewLineNumber; }
    public set DesiredRePackingNewLineNumber(newValue: number) { this.desiredRePackingNewLineNumber = newValue; }

    public get RepackingDesiredPackageType() { return this.repackingDesiredPackageType; }
    public set RepackingDesiredPackageType(newValue: string) { this.repackingDesiredPackageType = newValue; }

    public get RepackingDesiredPackageName() { return this.repackingDesiredPackageName; }
    public set RepackingDesiredPackageName(newValue: string) { this.repackingDesiredPackageName = newValue; }

    public get RepackingDesiredPackageId() { return this.repackingDesiredPackageId; }
    public set RepackingDesiredPackageId(newValue: string) { this.repackingDesiredPackageId = newValue; }

    public get RepackingDesiredQuantity() { return this.repackingDesiredQuantity; }
    public set RepackingDesiredQuantity(newValue: string) { this.repackingDesiredQuantity = newValue; }

    public get RepackingDesiredWeight() { return this.repackingDesiredWeight; }
    public set RepackingDesiredWeight(newValue: number) { this.repackingDesiredWeight = newValue; }

    public get DesiredRePackingOldLineNumber() { return this.desiredRePackingOldLineNumber; }
    public set DesiredRePackingOldLineNumber(newValue: number) { this.desiredRePackingOldLineNumber = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }
}

export class SampleRequestDetailsComponent extends BaseComponent {
    public DataContext: SampleRequestDetailsComponent = this;
    private sampleRowNumber: number;
    private sampleReturnDate: Date;
    private customsItemQuantity: number;
    private currencyTypeCode: string;
    private currencyTypeName: string;
    private customsItem: string;
    private sampleValue: number;
    private sampleDescription: string;
    private samplePackageName: string;
    private samplePackageType: string;
    private samplePackageId: string;
    private sampleWeight: number;
    private sampleQuantity: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(sampleRequestDetails: SampleRequestDetails) {
        super();
        this.SampleRowNumber = sampleRequestDetails.SampleRowNumber;
        this.SampleReturnDate = sampleRequestDetails.SampleReturnDate;
        this.CustomsItemQuantity = sampleRequestDetails.CustomsItemQuantity;
        this.CurrencyTypeCode = sampleRequestDetails.CurrencyTypeCode;
        this.CurrencyTypeName = sampleRequestDetails.CurrencyTypeName;
        this.SampleValue = sampleRequestDetails.SampleValue;
        this.SampleDescription = sampleRequestDetails.SampleDescription;
        this.CustomsItem = sampleRequestDetails.CustomsItem;
        if (sampleRequestDetails.SamplePackingDetails != null) {
            this.SamplePackageId = sampleRequestDetails.SamplePackingDetails.PackageId;
            this.SamplePackageType = sampleRequestDetails.SamplePackingDetails.PackageType;
            this.SamplePackageName = sampleRequestDetails.SamplePackingDetails.PackageTypeName;
            this.SampleQuantity = sampleRequestDetails.SamplePackingDetails.Quantity;
            this.SampleWeight = sampleRequestDetails.SamplePackingDetails.Weight;
        }
    }

    public get SampleRowNumber() { return this.sampleRowNumber; }
    public set SampleRowNumber(newValue: number) { this.sampleRowNumber = newValue; }

    public get SampleReturnDate() { return this.sampleReturnDate; }
    public set SampleReturnDate(newValue: Date) { this.sampleReturnDate = newValue; }

    public get CustomsItem() { return this.customsItem; }
    public set CustomsItem(newValue: string) { this.customsItem = newValue; }

    OnCustomsItemLostFocus(logCellTemplate: any, customsItemTextBox: any) {
        //var newValue = this.CustomsItem;
        var newValue = customsItemTextBox.textValue
        var valid = true;
        this.UIProperties.SetValidity("CustomsItem", "Customs.SupplierInvoiceItem", true, "");

        if (AppTool.IsNullOrEmpty(newValue)) {
            return;
        }

        if (newValue.toString().length > 11) {
            valid = false;
            this.UIProperties.SetValidity("CustomsItem", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));
        }
        else if (newValue.toString().length < 8) {
            valid = false;
            this.UIProperties.SetValidity("CustomsItem", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));
        }
        else if (newValue.toString().length == 8) {
            newValue = newValue + "00";
            var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + checkDigit;
            valid = true;;
        }
        else if (newValue.toString().length == 9) {
            var digit: string = newValue.toString().substring(8);
            newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
            var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));

            if (digit != checkDigit.toString()) {
                valid = false;
                this.UIProperties.SetValidity("CustomsItem", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
            }
            else {
                valid = true;
            }
        }
        else if (newValue.toString().length == 10) {
            var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + "" + checkDigit;
            valid = true;
        }
        else if (newValue.toString().length == 11) {
            var digit: string = newValue.toString().substring(10);
            var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
            if (digit != checkDigit.toString()) {
                valid = false;
                this.UIProperties.SetValidity("CustomsItem", null, false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
            }
            else {
                valid = true;
            }
        }
        else {
            valid = true;
            this.UIProperties.SetValidity("CustomsItem", null, true, "");
        }
        this.CustomsItem = newValue;
        if (valid) {
            SessionLocator.SustainFocusOnCell = false;
        }
        else {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: customsItemTextBox.InputId });
        }

    }

    OnSamplePackageIdLostFocus(logCellTemplate: any, samplePackageIdTextBox: any) {
        var newValue = samplePackageIdTextBox.textValue
        var valid = true;
        this.UIProperties.SetValidity("SamplePackageId", "Customs.Declaration", true, "");

        if (AppTool.IsNullOrEmpty(newValue)) {
            return;
        }

        if (newValue.toString().length > 32) {
            valid = false;
            this.UIProperties.SetValidity("SamplePackageId", "Customs.Declaration", false, "השדה זהוי אריזה חייב להיות קטן מ 32 תווים");
        }
        else {
            valid = true;
            this.UIProperties.SetValidity("SamplePackageId", "Customs.Declaration", true, "");
        }
        this.SamplePackageId = newValue;
        if (valid) {
            SessionLocator.SustainFocusOnCell = false;
        }
        else {
            SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: samplePackageIdTextBox.InputId });
        }

    }

    public get CustomsItemQuantity() { return this.customsItemQuantity; }
    public set CustomsItemQuantity(newValue: number) { this.customsItemQuantity = newValue; }

    public get CurrencyTypeCode() { return this.currencyTypeCode; }
    public set CurrencyTypeCode(newValue: string) { this.currencyTypeCode = newValue; }

    public get CurrencyTypeName() { return this.currencyTypeName; }
    public set CurrencyTypeName(newValue: string) { this.currencyTypeName = newValue; }

    public get SampleValue() { return this.sampleValue; }
    public set SampleValue(newValue: number) { this.sampleValue = newValue; }

    public get SampleDescription() { return this.sampleDescription; }
    public set SampleDescription(newValue: string) { this.sampleDescription = newValue; }

    public get SamplePackageId() { return this.samplePackageId; }
    public set SamplePackageId(newValue: string) { this.samplePackageId = newValue; }

    public get SamplePackageType() { return this.samplePackageType; }
    public set SamplePackageType(newValue: string) { this.samplePackageType = newValue; }

    public get SamplePackageName() { return this.samplePackageName; }
    public set SamplePackageName(newValue: string) { this.samplePackageName = newValue; }

    public get SampleWeight() { return this.sampleWeight; }
    public set SampleWeight(newValue: number) { this.sampleWeight = newValue; }

    public get SampleQuantity() { return this.sampleQuantity; }
    public set SampleQuantity(newValue: string) { this.sampleQuantity = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }
}
