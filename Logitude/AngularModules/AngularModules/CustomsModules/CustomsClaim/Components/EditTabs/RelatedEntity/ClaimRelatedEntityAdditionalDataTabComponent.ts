import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClientAddressPM } from '../../../../../Customs/EntityPMs/ClientAddressPM';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { ClaimsRelatedEntsExpDeclarPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntsExpDeclarPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { ClientsAddressCommTypePM } from '../../../../../Customs/EntityPMs/ClientsAddressCommTypePM';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ClientMessagesService } from '../../../../../Customs/Services/WebServices/ClientMessagesService';
import { ClientPMService } from '../../../../../Customs/Services/StandardPMs/ClientPMService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';


@Component({
    moduleId: module.id,
    templateUrl: './ClaimRelatedEntityAdditionalDataTabComponent.html',
})

export class ClaimRelatedEntityAdditionalDataTabComponent extends BaseComponent {
    public DataContext: ClaimRelatedEntityAdditionalDataTabComponent = this;
    public EntityPM: ClaimsRelatedEntityPM = new ClaimsRelatedEntityPM(null);
    public ClaimPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.ClaimsRelatedEntity";
    FooterMethods: any;

    public ClaimsRelatedEntsExpDeclarsList: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = false;

    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.ClaimsRelatedEntsExpDeclarsList = new ObservableCollection([]);
        SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
    }

    private Listen() {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = SessionLocator.SelectedSession.CurrentEditComponent.ComponentId;
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                        this.BuildExportDeclarationlist();
                    }
                })
            );
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == SessionLocator.SelectedSession.CurrentEditComponent.ComponentId) {
                        //if (tabCode == "CLMG") {
                        //    this.RefreshEntity();
                        //    this.BuildExportDeclarationlist();
                        //}
                    }
                })
            );
        }
    }

    InitTab(entityPM: ClaimsRelatedEntityPM, claimPM: ClaimPM, isEnable: boolean) {

        this.EntityPM = entityPM;
        this.ClaimPM = claimPM;
        this.isControlEnabled = isEnable;

        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntsExpDeclar").subscribe(response => {
                    this.BuildExportDeclarationlist();
                    this.Listen();
                });
            });
        });
    }

    RefreshEntity() {
        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
    }

    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    public get SeconderyClaimEntityCode() { return this.EntityPM.SeconderyClaimEntityCode; }
    public set SeconderyClaimEntityCode(newValue: string) { this.EntityPM.SeconderyClaimEntityCode = newValue; }

    public get SeconderyClaimEntityID() { return this.EntityPM.SeconderyClaimEntityID; }
    public set SeconderyClaimEntityID(newValue: string) { this.EntityPM.SeconderyClaimEntityID = newValue; }

    public get CourtCode() { return this.EntityPM.CourtCode; }
    public set CourtCode(newValue: string) { this.EntityPM.CourtCode = newValue;}

    public get ProceedingNumber() { return this.EntityPM.ProceedingNumber; }
    public set ProceedingNumber(newValue: string) { this.EntityPM.ProceedingNumber = newValue; }

    public get AbandonmentDestructionReference() { return this.EntityPM.AbandonmentDestructionReferenc; }
    public set AbandonmentDestructionReference(newValue: string) { this.EntityPM.AbandonmentDestructionReferenc = newValue; }

    public get WarehouseTypeCode() { return this.EntityPM.WarehouseTypeCode; }
    public set WarehouseTypeCode(newValue: string) { this.EntityPM.WarehouseTypeCode = newValue; }

    BuildExportDeclarationlist() {
        this.ClaimsRelatedEntsExpDeclarsList = new ObservableCollection([]);

        if (this.EntityPM.ClaimsRelatedEntsExpDeclars != null && this.EntityPM.ClaimsRelatedEntsExpDeclars.length > 0) {
            for (let item of this.EntityPM.ClaimsRelatedEntsExpDeclars) {
                this.ClaimsRelatedEntsExpDeclarsList.Insert(new ClaimRelatedEntityExpDeclarationComponent(item, this.EntityPM));
            }
        }
    }

    AddEntityExpDeclarationCommand() {
        if (!this.IsControlEnabled) return;

        var newClaimsRelatedEntsExpDeclarPM = new ClaimsRelatedEntsExpDeclarPM(this.EntityPM);
        newClaimsRelatedEntsExpDeclarPM.ClaimId = this.EntityPM.ClaimId;
        newClaimsRelatedEntsExpDeclarPM.CounterKey = this.EntityPM.EntityCounterKey;
        newClaimsRelatedEntsExpDeclarPM.Tenant = this.EntityPM.Tenant;

        this.ClaimsRelatedEntsExpDeclarsList.Insert(new ClaimRelatedEntityExpDeclarationComponent(newClaimsRelatedEntsExpDeclarPM, this.EntityPM));
        this.EntityPM.AddClaimsRelatedEntsExpDeclar(newClaimsRelatedEntsExpDeclarPM);
    }

    DeleteExportDeclarationCommand(item) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.ClaimsRelatedEntsExpDeclarsList.Remove(item);
            this.EntityPM.RemoveClaimsRelatedEntsExpDeclar(item.entityPM);
        }
    }

    Dispose() {
        //this.ClaimsRelatedEntitiesAmountsList.Collection.forEach((item) => {
        //    item.Dispose();
        //});
    }
    //#endregion
}

export class ClaimRelatedEntityExpDeclarationComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimsRelatedEntitiesAmount";
    public DataContext: ClaimRelatedEntityExpDeclarationComponent = this;

    constructor(public entityPM: ClaimsRelatedEntsExpDeclarPM, public claimsRelatedEntity: ClaimsRelatedEntityPM) {
        super();
    }

    public get ExportDeclarationNumber() { return this.entityPM.ExportDeclarationNumber; }
    public set ExportDeclarationNumber(newValue: string) { this.entityPM.ExportDeclarationNumber = newValue; }
}
