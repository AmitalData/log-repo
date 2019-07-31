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
import { ReleaseGoodsResponseData, GoodsItems } from '../../../../Customs/DataContract/ResponseData/ReleaseGoodsResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
//import { GenericRequestParams } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase'; test06
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';

@Component({
    selector: 'ReleaseGoodsComponent',
    moduleId: module.id,
    templateUrl: './ReleaseGoodsComponent.html',
})

export class ReleaseGoodsComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, IRequestsSheetMassagingComponent {

    public DataContext: ReleaseGoodsComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    //_DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    //_IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    GoodsItemsList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.GoodsItemsList = new ObservableCollection([]);
        this.CurrentSession.entityResourceService.getEntityResourceByTableName("Customs.ReleaseGoods").subscribe(response => {

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
        this.subscribeWrapperComponent()
    }


    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            //this.RequestParams = new RequestParamsBase();
        }

        if (this.ResponseData) {
            if (this.ResponseData.GoodsItemsList) {
                this.GoodsItemsList.InsertCollection(this.ResponseData.GoodsItemsList);
            }
        }
        else {
            this.ResponseData = new ReleaseGoodsResponseData();
        }

    }

    OnRowLoaded(myRow: any) {
        if (myRow) {
            myRow.SetExpandaple(true);
        }
    }

    //#region Properties

    get DeclarationNumber() { return this.ResponseData ? this.ResponseData.DeclarationNumber : null; }
    set DeclarationNumber(value: string) {
        if (this.ResponseData.DeclarationNumber != value) {
            this.ResponseData.DeclarationNumber = value;
        }
    }

    get FileNumber() { return this.ResponseData ? this.ResponseData.FileNumber : null; }
    set FileNumber(value: string) {
        if (this.ResponseData.FileNumber != value) {
            this.ResponseData.FileNumber = value;
        }
    }

    get governmentProcedureType() { return this.ResponseData ? this.ResponseData.governmentProcedureType : null; }
    set governmentProcedureType(value: string) {
        if (this.ResponseData.governmentProcedureType != value) {
            this.ResponseData.governmentProcedureType = value;
        }
    }

    get releaseDate() { return this.ResponseData ? this.ResponseData.releaseDate : null; }
    set releaseDate(value: string) {
        if (this.ResponseData.releaseDate != value) {
            this.ResponseData.releaseDate = value;
        }
    }

    get dealValueNIS() { return this.ResponseData ? this.ResponseData.dealValueNIS : null; }
    set dealValueNIS(value: string) {
        if (this.ResponseData.dealValueNIS != value) {
            this.ResponseData.dealValueNIS = value;
        }
    }

    get CifValueNis() { return this.ResponseData ? this.ResponseData.CifValueNis : null; }
    set CifValueNis(value: string) {
        if (this.ResponseData.CifValueNis != value) {
            this.ResponseData.CifValueNis = value;
        }
    }

    get CurrencyTypeCode() { return this.ResponseData ? this.ResponseData.CurrencyTypeCode : null; }
    set CurrencyTypeCode(value: string) {
        if (this.ResponseData.CurrencyTypeCode != value) {
            this.ResponseData.CurrencyTypeCode = value;
        }
    }

    get ExchangeRate() { return this.ResponseData ? this.ResponseData.ExchangeRate : null; }
    set ExchangeRate(value: string) {
        if (this.ResponseData.ExchangeRate != value) {
            this.ResponseData.ExchangeRate = value;
        }
    }

    get TaxationDate() { return this.ResponseData ? this.ResponseData.TaxationDate : null; }
    set TaxationDate(value: string) {
        if (this.ResponseData.TaxationDate != value) {
            this.ResponseData.TaxationDate = value;
        }
    }

    get importerExpoterExternalID() { return this.ResponseData ? this.ResponseData.importerExpoterExternalID : null; }
    set importerExpoterExternalID(value: string) {
        if (this.ResponseData.importerExpoterExternalID != value) {
            this.ResponseData.importerExpoterExternalID = value;
        }
    }

    get cargoIdentifierType() { return this.ResponseData ? this.ResponseData.cargoIdentifierType : null; }
    set cargoIdentifierType(value: string) {
        if (this.ResponseData.cargoIdentifierType != value) {
            this.ResponseData.cargoIdentifierType = value;
        }
    }

    get cargoIdentifierKey1() { return this.ResponseData ? this.ResponseData.cargoIdentifierKey1 : null; }
    set cargoIdentifierKey1(value: string) {
        if (this.ResponseData.cargoIdentifierKey1 != value) {
            this.ResponseData.cargoIdentifierKey1 = value;
        }
    }

    get cargoIdentifierKey2() { return this.ResponseData ? this.ResponseData.cargoIdentifierKey2 : null; }
    set cargoIdentifierKey2(value: string) {
        if (this.ResponseData.cargoIdentifierKey2 != value) {
            this.ResponseData.cargoIdentifierKey2 = value;
        }
    }

    get loadingPort() { return this.ResponseData ? this.ResponseData.loadingPort : null; }
    set loadingPort(value: string) {
        if (this.ResponseData.loadingPort != value) {
            this.ResponseData.loadingPort = value;
        }
    }

    get unloadingSiteNumber() { return this.ResponseData ? this.ResponseData.unloadingSiteNumber : null; }
    set unloadingSiteNumber(value: string) {
        if (this.ResponseData.unloadingSiteNumber != value) {
            this.ResponseData.unloadingSiteNumber = value;
        }
    }

    get storageSiteNumber() { return this.ResponseData ? this.ResponseData.storageSiteNumber : null; }
    set storageSiteNumber(value: string) {
        if (this.ResponseData.storageSiteNumber != value) {
            this.ResponseData.storageSiteNumber = value;
        }
    }

    get cargoDescription() { return this.ResponseData ? this.ResponseData.cargoDescription : null; }
    set cargoDescription(value: string) {
        if (this.ResponseData.cargoDescription != value) {
            this.ResponseData.cargoDescription = value;
        }
    }

    get packageType() { return this.ResponseData ? this.ResponseData.packageType : null; }
    set packageType(value: string) {
        if (this.ResponseData.packageType != value) {
            this.ResponseData.packageType = value;
        }
    }

    get packageQuantity() { return this.ResponseData ? this.ResponseData.packageQuantity : null; }
    set packageQuantity(value: string) {
        if (this.ResponseData.packageQuantity != value) {
            this.ResponseData.packageQuantity = value;
        }
    }

    get packagesWeight() { return this.ResponseData ? this.ResponseData.packagesWeight : null; }
    set packagesWeight(value: string) {
        if (this.ResponseData.packagesWeight != value) {
            this.ResponseData.packagesWeight = value;
        }
    }

    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) { }

    //#endregion Commands
}
