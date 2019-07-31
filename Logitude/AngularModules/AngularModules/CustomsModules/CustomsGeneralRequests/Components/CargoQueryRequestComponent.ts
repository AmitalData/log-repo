import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { ConsignmentPM } from '../../../Customs/EntityPMs/ConsignmentPM';

import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CargoQueryRequestParams } from '../../../Customs/DataContract/RequestParams/CargoQueryRequestParams';
import { CargoQueryResponseData, CargoResult, CargoAdditionalData, CargoItemResult} from '../../../Customs/DataContract/ResponseData/CargoQueryResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs, SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    selector: 'CargoQueryRequestComponent',
    moduleId: module.id,
    templateUrl: './CargoQueryRequestComponent.html',
})


export class CargoQueryRequestComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, OnInit, IRequestsSheetMassagingComponent {
    public DataContext: CargoQueryRequestComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    _MyResponseObjectToShow: any = null;
    _UserMessagehidden: boolean = true;

    _LastFetchDeclarationList: DeclarationList;
    _LastFetchConsignmentPMList: ConsignmentPM[];
    

    //DeclarationId: string ;
    _IsReady: boolean = false;
    private _IsFromDeclaration: boolean = false;

    DeliveryOrderResultList: ObservableCollection;
    CargosVersionResultList: ObservableCollection;
    CargoItemResultList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.DeliveryOrderResultList = new ObservableCollection([]);
        this.CargosVersionResultList = new ObservableCollection([]);
        this.CargoItemResultList = new ObservableCollection([]);
        this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this._IsReady = true;
            });
        });
        //this.ValidationErrorsList 
        //this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
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

    SetMenuArg(MenuArg) {
        if (MenuArg != null && MenuArg.Mode == "SendCargoQueryRequestFromDeclaration") {
            this._IsFromDeclaration = true;
            this.SendCargoQueryRequestFromDeclaration(MenuArg);
        }
        else {
            this.RequestParams = MenuArg;
            this.OnMassageDisplayMethod();
        }
    }

    SendCargoQueryRequestFromDeclaration(MenuArg) {
        this.OnMassageDisplayMethod();

        this.CargoTypeCode = MenuArg.CargoTypeCode;
        this.ManifestNumber = MenuArg.ManifestNumber;
        this.SecondCargoID = MenuArg.SecondCargoID;
        this.DeclarationId = MenuArg.DeclarationId;

        this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, false);

        //this.CustomSendOptionsButtonIsDisable = true;

        let customSendOptionsArgs: CustomSendOptionsArgs = new CustomSendOptionsArgs();
        customSendOptionsArgs.ForcePersonalSign = false;
        customSendOptionsArgs.RequestVIA = SendRequestVIA.WebServiceInteractive;
        this.OnCustomSendOptionsButtonClick(customSendOptionsArgs);

    }

    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        } else {

            this.CustomFileNo = "";
        }

        this.DeclarationId = "";
        this.ResponseData = null;
        this._LastFetchConsignmentPMList = null;
        this.ValidationErrorsList = [];
    }
    
    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            this.NoConnectedConsignmentEnableField();
        }
        else {
            this.CurrentSession.StartBusyIndicator("")
            this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
                .subscribe((myDeclarationResponse: ServiceResponse) => {
                
this.CurrentSession.StopBusyIndicator();
                    
                    this._LastFetchDeclarationList = myDeclarationResponse.Result;
                    if (AppTool.IsNullOrEmpty(this._LastFetchDeclarationList)) {
                        this.NoConnectedConsignmentEnableField();
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

    NoConnectedConsignmentEnableField() {
        this.DeclarationId = "";
        this.DeclarationNumber = "";
        this.CargoTypeCode = "";
        this.ManifestNumber = "";
        this.SecondCargoID = "";
        this.ThirdCargoID = "";
        //this.ResponseStatusXML = "";

        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, true);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, true);
    }

 
    FetchConsignment(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        this._LastFetchConsignmentPMList = myResponse.Result
        if (this._LastFetchConsignmentPMList != null
            //&& this._LastFetchConsignmentPMList.length > 0
        ) {
            var pm =this._LastFetchConsignmentPMList[0]
            this.DeclarationId = pm.DeclarationId;
            this.CargoTypeCode = pm.CargoTypeCode;
            this.ManifestNumber =pm.ManifestNumber;
            this.SecondCargoID = pm.SecondCargoID;
            this.ThirdCargoID = pm.ThirdCargoID;
            
            this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, false);

            this.DeclarationNumber = this._LastFetchDeclarationList.DeclarationNumber;

            //this.CurrentSession.StartBusyIndicator("")
            //this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.CustomFileNo)
            //    .subscribe((myResponse: ServiceResponse) => {
            //        this.CurrentSession.StopBusyIndicator();

            //        this.FetchConsignment(myResponse, false);

            //    });


        } else {

            this.NoConnectedConsignmentEnableField();

        }
    }
    
    CargoTypeCodeValueChanged(paramValueChanged) {
        console.log(paramValueChanged);
        this.GetCustomFileNo();

    }
    ManifestNumberTextChanged(param) {
        this.GetCustomFileNo();
    }
    SecondCargoIDTextChanged(param) {
        if (AppTool.IsNullOrEmpty(this.SecondCargoID)) {
            return;
        }
        if (this.SecondCargoID.length == 9) // moran 10.5.15 -  Bug 9749
        {
            if (this.SecondCargoID.substring(0, 1) != "I") {
                this.SecondCargoID = "I" + this.SecondCargoID;
            }
        }
        this.GetCustomFileNo();
    }
    GetCustomFileNo() {
        if (!AppTool.IsNullOrEmpty(this.ManifestNumber) && !
            AppTool.IsNullOrEmpty(this.SecondCargoID)) {
            if (!AppTool.IsNullOrEmpty(this.CargoTypeCode)) {
                //LoadOperation op2 = this.context.Load(this.context.GetSingleDeclarationPMByCargoIdentifiersQuery(this.CargoTypeCode, this.ManifestNumber, this.SecondCargoID, TenantContext.Current.Id), LoadBehavior.RefreshCurrent, true);
                //op2.Completed += CargoTypeCodeLostFocus_Completed;

                this._DeclarationExtendedListService.GetSingleDeclarationPMByCargoIdentifiers
                    (this.CargoTypeCode, this.ManifestNumber, this.SecondCargoID, SessionLocator.Tenant)
                    .subscribe((myResponse: ServiceResponse) => {
                        let myDeclarationPMList = myResponse.Result;
                        if (myDeclarationPMList) {
                            if (myDeclarationPMList[0]) {
                                this.CustomFileNo = myDeclarationPMList[0].CustomFileNo;
                                this.DeclarationNumber = myDeclarationPMList[0].DeclarationNumber;
                                this.DeclarationId = myDeclarationPMList[0].Id;
                            }
                        }
                    });
            }
        }
    }

    get CustomFileNo() { return this.RequestParams ? this.RequestParams.CustomsFile : null; }
    set CustomFileNo(value: string) {
        if (this.RequestParams.CustomsFile != value) {
            //this.UIProperties.SetValidity("CustomFileNo", "Customs.Declaration", true, TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile"));

            this.RequestParams.CustomsFile = value;
        }
    }

    get DeclarationNumber() { return this.RequestParams ? this.RequestParams.DeclarationNumber : null; }
    set DeclarationNumber(value: string) {
        if (this.RequestParams.DeclarationNumber != value) {
            this.RequestParams.DeclarationNumber = value;
            if (value) {
                //     this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            }
        } else {
            //this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, true);
        }
    }
    get DeclarationId() { return this.RequestParams ? this.RequestParams.DeclarationId : null }
    set DeclarationId(value: string) {
        if (this.RequestParams.DeclarationId != value) {
            this.RequestParams.DeclarationId = value;
        }
    }




    get CargoTypeCode() { return this.RequestParams ? this.RequestParams.CargoTypeCode : null; }
    set CargoTypeCode(value: string) {
        {
            if (this.RequestParams.CargoTypeCode != value) {
                this.RequestParams.CargoTypeCode = value;
                //FirePropertyChanged("CargoTypeCode");
            }
        }
    }



    get ManifestNumber() { return this.RequestParams ? this.RequestParams.ManifestNumber : null; }
    set ManifestNumber(value: string) {
        if (this.RequestParams.ManifestNumber != value) {
            this.RequestParams.ManifestNumber = value;
            //FirePropertyChanged("ManifestNumber");
        }
    }


    get SecondCargoID() { return this.RequestParams ? this.RequestParams.SecondCargoID : null; }
    set SecondCargoID(value: string) {

        this.RequestParams.SecondCargoID = value;
        //FirePropertyChanged("SecondCargoID");

    }



    get ThirdCargoID() { return this.RequestParams ? this.RequestParams.ThirdCargoID : null; }
    set ThirdCargoID(value: string) {

        if (this.RequestParams.ThirdCargoID != value) {
            this.RequestParams.ThirdCargoID = value;
            //FirePropertyChanged("ThirdCargoID");
        }
    }


    get IsShowUserMessage() {
        return this.ResponseData ? this.ResponseData.IsShowUserMessage : false;
    }
    get UserMessage() {
        return this.ResponseData ? this.ResponseData.UserMessage : null;
    }
    set UserMessage(value: string) {}
    


    RefreshScreen() {
        this._MyResponseObjectToShow = null;
        this._UserMessagehidden = true;
        if (this.ResponseData == null) {
            return;
        }
        this._UserMessagehidden = !this.ResponseData.IsShowUserMessage;


        if (!AppTool.IsNullOrEmpty(this.ResponseData.ResponseStatusXML)) {
            this.UIProperties.SetRequired("DeclarationNumber", this.ObjectTableName, false);
            try {
                this._MyResponseObjectToShow = JSON.parse(this.ResponseData.ResponseStatusXML)
            } catch (err) {
                console.log(err);
            }
        }
    }

    OnMassageDisplayMethod() {

        if (this.RequestParams == null) {
            this.RequestParams = new CargoQueryRequestParams();
        }
        if (this.ResponseData == null) {
            this.ResponseData = new CargoQueryResponseData();
        } else {
            if (this.ResponseData.DeliveryOrderResultList) {
                
                this.DeliveryOrderResultList.InsertCollection(this.ResponseData.DeliveryOrderResultList);
            }
            if (this.ResponseData.CargosVersionResultList) {

                this.CargosVersionResultList.InsertCollection(this.ResponseData.CargosVersionResultList);
            }
            if (this.ResponseData.CargoItemResultList){
                this.CargoItemResultList.InsertCollection(this.ResponseData.CargoItemResultList);
            }
        }     
        
        this.RefreshScreen();

    }
    
    public FillError() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);       

        if (AppTool.IsNullOrEmpty(this.CargoTypeCode) || AppTool.IsNullOrEmpty(this.ManifestNumber))//eitan h 4/3/15 task 11484
        {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CargoDataMissing"));
        }

        this.ValidationErrorsList = errors;
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillError();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        if (!AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
            declarationDisplayOnlyChecks.CheckIfRequestInProgress("2715", this.CustomFileNo, SessionLocator.Tenant)
                .subscribe((response: ServiceResponse) => {
                    if (!response.HasError) {
                        var requestSheets = response.Result;
                        var haveRS2715: boolean = false;
                        if ((requestSheets == null || requestSheets.length == 0)
                            || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                            haveRS2715 = false;
                        } else {
                            haveRS2715 = true;
                        }
                        if (haveRS2715) {
                            var messageWindow = new MessageWindow();
                            messageWindow.Width = 400;
                            messageWindow.Height = 150;
                            messageWindow.Title = "שיחזור מספר הצהרה";
                            messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הצהרה בתהליך ");
                            return;
                        }

                        declarationDisplayOnlyChecks.CheckIfRequestInProgress("2755", this.CustomFileNo, SessionLocator.Tenant)
                            .subscribe((response: ServiceResponse) => {
                                if (!response.HasError) {
                                    var requestSheets = response.Result;
                                    var haveRS2755: boolean = false;
                                    if ((requestSheets == null || requestSheets.length == 0)
                                        || (requestSheets != null && requestSheets.length == 1 && requestSheets[0].InterfaceTypeCode == null)) {
                                        haveRS2755 = false;
                                    } else {
                                        haveRS2755 = true;
                                    }
                                    if (haveRS2755) {
                                        var messageWindow = new MessageWindow();
                                        messageWindow.Width = 400;
                                        messageWindow.Height = 150;
                                        messageWindow.Title = "שיחזור מספר הצהרה";
                                        messageWindow.Show("לא ניתן לשחזר מספר הצהרה ,קיימת בקשה מסוג הגשת תשלום ");
                                        return;
                                    }
                                    this.SendCargoQueryRequest(customSendOptionsArgs);
                                }
                            });
                    }
                });

        }
        else {
            this.SendCargoQueryRequest(customSendOptionsArgs);
        }
    }

    SendCargoQueryRequest(customSendOptionsArgs: CustomSendOptionsArgs) {

        var currRequestParams = new CargoQueryRequestParams();///Force new GUID On Each Send !!
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;

        currRequestParams.AppicationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationId = this.RequestParams.DeclarationId;
        currRequestParams.DeclarationNumber = this.RequestParams.DeclarationNumber;
        currRequestParams.CustomsFile = this.RequestParams.CustomsFile;
        
        currRequestParams.CargoTypeCode = this.RequestParams.CargoTypeCode;
        currRequestParams.ManifestNumber = this.RequestParams.ManifestNumber;
        currRequestParams.SecondCargoID = this.RequestParams.SecondCargoID;
        currRequestParams.ThirdCargoID = this.RequestParams.ThirdCargoID;
        currRequestParams.RequestName = "Manifest Status Query";
        currRequestParams.ResponseName = "Manifest Status Query";
                                                                
        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא למצהר"
            , true)
            .then((res) => {
                this.ResponseData = res;
                if (this._IsFromDeclaration && this.ResponseData.HasException == false) {
                    if (this.CurrentSession.CurrentEditComponent != null) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._IIGGeneralMessagesService.PostCargoQueryRequestParams(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });
    }

    get CargoQueryResponseData() {
        if (this.ResponseData) {
            var my = this.ResponseData as CargoQueryResponseData;
            if (my) {

            } else {
                my = new CargoQueryResponseData();
            }
            if (AppTool.IsNullOrEmpty(my.CargoResultList)) {
                my.CargoResultList = new CargoResult();
            }
            if (AppTool.IsNullOrEmpty(my.CargoResultList.CargoAdditionalDataList)) {
                my.CargoResultList.CargoAdditionalDataList = new CargoAdditionalData();
            }
            return my;
        }
        return null;
    }
    get ManifestTypeName() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.ManifestTypeName : null; }
    get ManifestStatusName() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.ManifestStatusName : null; }
    get ResponseManifestNumber() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.Manifestnumber : null; }
    get CargoIdentifierKey1() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoIdentifierKey1 : null; }
    get ParentCargoID() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.ParentCargoID : null; }
    get UnloadingLocationName() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoAdditionalDataList.UnloadingLocationName : null; }
    get TotalNumberOfPackeges() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.TotalNumberOfPackeges : null; }
    get TotalRecordNumberOfPackeges() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.TotalNumberOfPackeges : null; }
    get GovernmentProcedureTypeName() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.GovernmentProcedureTypeName : null; }
    get TreatmentWayName() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.TreatmentWayName : null; }
    get GoodsReceiptPlaceSiteName() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoAdditionalDataList.GoodsReceiptPlaceSiteName : null; }
    get TotalWeight() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.TotalWeight : null; }
    get TotalRecordWeight() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoAdditionalDataList.TotalRecordWeight : null; }
    get MasterBolNumber() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.MasterBolNumber : null; }
    get BillOfLadingNumber() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.BillOfLadingNumber : null; }
    get TransitDestinationLocationName() { return this.CargoQueryResponseData ? this.CargoQueryResponseData.CargoResultList.CargoAdditionalDataList.TransitDestinationLocationName : null; }
      
    
    OnRowLoaded(Row: any) {
        var isExpandaple = false;
        if (Row) {
            var item: CargoItemResult = Row.rowData;
            if (!AppTool.IsNullOrEmpty(item.SealDetailsList) && item.SealDetailsList.length > 0) {
                item.SealDetailsObservableCollection = new ObservableCollection([]);
                item.SealDetailsObservableCollection.InsertCollection(item.SealDetailsList);
                isExpandaple = true;
            }
            if (!AppTool.IsNullOrEmpty(item.CargoMovmentList) && item.CargoMovmentList.length > 0) {
                item.CargoMovmentObservableCollection = new ObservableCollection([]);
                item.CargoMovmentObservableCollection.InsertCollection(item.CargoMovmentList);
                isExpandaple = true;
            }

            if (isExpandaple) {
                item.CargoMovmentObservableCollection = item.CargoMovmentObservableCollection || new ObservableCollection([]);
                item.SealDetailsObservableCollection = item.SealDetailsObservableCollection || new ObservableCollection([]);
            }
            Row.SetExpandaple(isExpandaple);
        }

    }
    
    CancelButtonClicked() {
        if (this._IsFromDeclaration) {
            if (this.ValidationErrorsList.length) {
                this.CurrentSession.CloseCurrentWindowEmit("");
            }
            else {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.CloseCurrentWindowEmit("ReloadEntity");
            }
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    }

}
