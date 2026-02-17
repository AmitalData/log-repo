import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClaimImporterDeclarsPage3BPM } from '../../../../../Customs/EntityPMs/ClaimImporterDeclarsPage3BPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './ClaimImporterDeclBCTabComponent.html',
})

export class ClaimImporterDeclBCTabComponent extends BaseComponent {
    public DataContext: ClaimImporterDeclBCTabComponent = this;
    public EntityPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.Claim";

    public SaleDeclarlist: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;

    IsLoaded: boolean = false;

    private _ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.SaleDeclarlist = new ObservableCollection([]);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimImporterDeclarsPage3B").subscribe(response => {
                    if (this.entityArgs.EntityPM != null) {
                        this.EntityPM = this.entityArgs.EntityPM;
                        this.BuildSaleDeclarList();
                    }
                    this.Listen();
                    this.IsLoaded = true;
            });
        });

    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.BuildSaleDeclarList();
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            });

            this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "CLMA") {
                        this.BuildSaleDeclarList();
                    }
                }
            });
        }
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
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

    public get ValidationErrorsList() { return this._ValidationErrorsList; }
    public set ValidationErrorsList(newValue: string[]) { this._ValidationErrorsList = newValue; }

    public get RawMaterialsDescription() { return this.EntityPM.RawMaterialsDescription; }
    public set RawMaterialsDescription(newValue: string) { this.EntityPM.RawMaterialsDescription = newValue; }

    BuildSaleDeclarList() {
        this.SaleDeclarlist = new ObservableCollection([]);

        if (this.EntityPM.ClaimImporterDeclarsPage3B != null && this.EntityPM.ClaimImporterDeclarsPage3B.length > 0) {
            for (let item of this.EntityPM.ClaimImporterDeclarsPage3B) {
                this.SaleDeclarlist.Insert(new SaleDeclarLineComponent(item));
            }
        }
    }

    AddSaleDeclarCommand() {
        if (!this.IsControlEnabled) return;

        if (this.SaleDeclarlist != null && this.SaleDeclarlist.Length > 0) {
            var nullVM = this.SaleDeclarlist.Collection.filter(vm => vm.DescriptionOfGoods == null);
            if (nullVM.length > 0) {
                this.ValidationErrorsList = [];
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.Claim.O.UseEmptyRow"));
                return;
            }
        }

        var newClaimImporterDeclarsPage3BPM = new ClaimImporterDeclarsPage3BPM(this.EntityPM);
        newClaimImporterDeclarsPage3BPM.Tenant = this.EntityPM.Tenant;
        newClaimImporterDeclarsPage3BPM.ClaimId = this.EntityPM.Id;
        newClaimImporterDeclarsPage3BPM.LineNo = (ArrayTool.Max(this.EntityPM.ClaimImporterDeclarsPage3, "LineNo") + 1);

        this.SaleDeclarlist.Insert(new SaleDeclarLineComponent(newClaimImporterDeclarsPage3BPM));
        this.EntityPM.AddClaimImporterDeclarsPage3B(newClaimImporterDeclarsPage3BPM);
    }

    DeleteSaleDeclarCommand(item: SaleDeclarLineComponent) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.SaleDeclarlist.Remove(item);
            this.EntityPM.RemoveClaimImporterDeclarsPage3B(item.entityPM);
        }
    }

}

export class SaleDeclarLineComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimImporterDeclarsPage3B";
    public DataContext: SaleDeclarLineComponent = this;

    constructor(public entityPM: ClaimImporterDeclarsPage3BPM) {
        super();
    }

    public get DescriptionOfGoods() { return this.entityPM.DescriptionOfGoods; }
    public set DescriptionOfGoods(newValue: string) { this.entityPM.DescriptionOfGoods = newValue; }

    public get SaleAmountBefore() { return this.entityPM.SaleAmountBefore; }
    public set SaleAmountBefore(newValue: number) { this.entityPM.SaleAmountBefore = newValue; }

    public get SaleAmountAfter() { return this.entityPM.SaleAmountAfter; }
    public set SaleAmountAfter(newValue: number) { this.entityPM.SaleAmountAfter = newValue; }

    public get SaleAmountClaim() { return this.entityPM.SaleAmountClaim; }
    public set SaleAmountClaim(newValue: number) { this.entityPM.SaleAmountClaim = newValue; }

    public get InventoryAmount() { return this.entityPM.InventoryAmount; }
    public set InventoryAmount(newValue: number) { this.entityPM.InventoryAmount = newValue; }

    public get SoldGoodsAmount() { return this.entityPM.SoldGoodsAmount; }
    public set SoldGoodsAmount(newValue: number) { this.entityPM.SoldGoodsAmount = newValue; }

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }

}
