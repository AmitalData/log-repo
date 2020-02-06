declare var System: any;
declare var window: any;
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { CargoSealPM } from '../../../../../Customs/EntityPMs/CargoSealPM';
import { CargoSealIdentifierPM } from '../../../../../Customs/EntityPMs/CargoSealIdentifierPM';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';;
import { TapagList } from '../../../../../Customs/EntityLists/TapagList';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';


@Component({
    moduleId: module.id,
    templateUrl: './DeclarationCargoSealTabComponent.html',
})

export class DeclarationCargoSealTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: DeclarationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;
    public CurrentEditComponentId: string;
    public CargoSealObslist: ObservableCollection;

    private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;

    IsLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.CargoSealObslist = new ObservableCollection([]);

            this.EntityResourceService.getEntityResourceByTableName("Customs.CargoSealIdentifier").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.CargoSeal").subscribe((response: any) => {
                    this.EntityPM = this.entityArgs.EntityPM;
                    this.ObjectTableName = this.entityArgs.ObjectTableName;
                    this.LoadCargoSealsList();
                    this.Listen();
                    this.IsLoaded = true;
            });
        });
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
    }

    ngOnDestroy() {
        console.log("DeclarationCargoSealTabComponent:ngOnDestroy");
        this.entityArgs = null;
    }
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadCargoSealsList();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCSE") {
                            this.LoadCargoSealsList();
                        }
                    }
                })
            );
        }
    }

    private LoadCargoSealsList() {
        this.CargoSealObslist = new ObservableCollection([]);

        this._DeclarationWebService.GetDeclarationCargoSealLists(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.GetDeclarationCargoSealListsOp_Completed(myResponse, false);
            });
    }

    private GetDeclarationCargoSealListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            myResponse.Result.forEach((item: CargoSealIdentifierPM) => {
                this.CargoSealObslist.Insert(new CargoSealItemComponent(item));
            });
        }
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    EditButtonClicked(item: CargoSealItemComponent) {

        var windowArgs: any = {};
        //windowArgs.EntityPM = response.Result;
        windowArgs.declarationPM = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 700;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deficit/PaymentOrderDeficitComponent');
        this.CurrentSession.StopBusyIndicator();

    }

}

export class CargoSealItemComponent extends BaseComponent {
    public ObjectTableName = "Customs.CargoSealIdentifier";
    public DataContext: CargoSealItemComponent = this;

    constructor(public entityPM: CargoSealIdentifierPM) {
        super();
        if (entityPM != null && entityPM.CargoSeals != null) {
            var cargoSealPM: CargoSealPM = entityPM.CargoSeals[0];
            this.SealNumber = cargoSealPM.SealNumber;
            this.SealCompletenessStateCode = cargoSealPM.SealCompletenessStateCode;
            this.SealCompletenessStateName = cargoSealPM.SealCompletenessStateName;
        }
    }

    public get CargoRowNumber() { return this.entityPM.CargoRowNumber; }
    public set CargoRowNumber(newValue: string) { this.entityPM.CargoRowNumber = newValue; }

    public get ContainerNumber() { return this.entityPM.ContainerNumber; }
    public set ContainerNumber(newValue: string) { this.entityPM.ContainerNumber = newValue; }

    private _SealNumber: string;
    public get SealNumber() { return this._SealNumber; }
    public set SealNumber(newValue: string) { this._SealNumber = newValue; }

    private _SealCompletenessStateCode: string;
    public get SealCompletenessStateCode() { return this._SealCompletenessStateCode; }
    public set SealCompletenessStateCode(newValue: string) { this._SealCompletenessStateCode = newValue; }

    private _SealCompletenessStateName: string;
    public get SealCompletenessStateName() { return this._SealCompletenessStateName; }
    public set SealCompletenessStateName(newValue: string) { this._SealCompletenessStateName = newValue; }

    //public get SealTypeCode() { return this.entityPM.SealTypeCode; }
    //public set SealTypeCode(newValue: string) { this.entityPM.SealTypeCode = newValue; }

    //public get SealTypeName() { return this.entityPM.SealTypeName; }
    //public set SealTypeName(newValue: string) { this.entityPM.SealTypeName = newValue; }

    //public get UpdateReasonCode() { return this.entityPM.UpdateReasonCode; }
    //public set UpdateReasonCode(newValue: string) { this.entityPM.UpdateReasonCode = newValue; }

    //public get UpdateReasonName() { return this.entityPM.UpdateReasonName; }
    //public set UpdateReasonName(newValue: string) { this.entityPM.UpdateReasonName = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }
}
