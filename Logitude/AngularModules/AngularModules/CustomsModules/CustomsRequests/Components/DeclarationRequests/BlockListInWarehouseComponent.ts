import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'

import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { BlockListInWarehouseRequestParams } from '../../../../Customs/DataContract/RequestParams/BlockListInWarehouseRequestParams';
import { WarehouseBlockBalanceResponseData } from '../../../../Customs/DataContract/ResponseData/WarehouseBlockBalanceResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    selector: 'BlockListInWarehouseComponent',
    moduleId: module.id,
    templateUrl: './BlockListInWarehouseComponent.html',
})

export class BlockListInWarehouseComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: BlockListInWarehouseComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    public BlockListInWarehouseResultList: ObservableCollection;
    
    ShowResetBlocksList;
    constructor() {
        super();
        this.ShowResetBlocksList =
            [
                { 'EnumId': 0, 'Name': 'No' },
                { 'EnumId': 1, 'Name': 'Yes' },
                { 'EnumId': 2, 'Name': 'All' }
            ];
        
        this.BlockListInWarehouseResultList = new ObservableCollection([]);
        

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
            this.RequestParams = new BlockListInWarehouseRequestParams();
            
            
        }

        if (this.ResponseData) {
            this.NumberOfBlocksInList = this.ResponseData.NumberOfBlocksInList;
            if (this.ResponseData.BlockListInWarehouseResultList) {
                this.BlockListInWarehouseResultList.InsertCollection(this.ResponseData.BlockListInWarehouseResultList);
            }
        }
        else {
            this.ResponseData = new WarehouseBlockBalanceResponseData();
        }
    }

    OnRowLoaded(myRow: any) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    }

    //#region Properties
    _NumberOfBlocksInList = "";
    get NumberOfBlocksInList() { return this._NumberOfBlocksInList; }
    set NumberOfBlocksInList(value: string) {
        this._NumberOfBlocksInList = value;
    }
    get FromDate() { return this.RequestParams.FromDate; }
    set FromDate(value: Date) {
        if (this.RequestParams.FromDate != value) {
            this.RequestParams.FromDate = value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.FromDate)) {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
            }
            
        }
    }

    get ToDate() { return this.RequestParams.ToDate; }
    set ToDate(value: Date) {
        if (this.RequestParams.ToDate != value) {
            this.RequestParams.ToDate = value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.ToDate)) {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
            }
        }
    }



    get StorageSiteNumber() { return this.RequestParams ? this.RequestParams.StorageSiteNumber : null; }
    set StorageSiteNumber(value: string) {
        if (this.RequestParams.StorageSiteNumber != value) {
            this.RequestParams.StorageSiteNumber = value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.StorageSiteNumber)) {
                this.UIProperties.SetRequired("StorageSiteNumber", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("StorageSiteNumber", this.ObjectTableName, false);
            }
        }
    }
    _ShowResetBlocks;
    ResetBlockListChangeSelected(enumvalue) {
        this._ShowResetBlocks = enumvalue;

    }

    
    //#region General Commands
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;     
        if (this.FromDate == null) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.FromDateMandatory"));
        }
        if (this.ToDate == null) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.ToDateMandatory"));
            
        }
        if (this.StorageSiteNumber == null) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.CustomsBlockListInWarehouse.O.StorageSiteNumberMandatory"));
        }
       

    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new BlockListInWarehouseRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.FromDate = this.FromDate;
        currRequestParams.ToDate = this.ToDate;
        currRequestParams.StorageSiteNumber = this.StorageSiteNumber;
        currRequestParams.ShowResetBlocks = this._ShowResetBlocks;



        CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId,
            "שליחת שאילתא לגושים במחסן", true)
            .then((res) => {
                this.ResponseData = res;
                this.MyLastCustomsRequestSheetId = currRequestParams.PBId;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        

        this._IIGGeneralMessagesService.PostBlockListInWarehouseRequestParams(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    //#endregion Commands
}
