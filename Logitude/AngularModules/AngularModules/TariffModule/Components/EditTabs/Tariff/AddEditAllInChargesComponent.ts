import { Component } from '@angular/core';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TariffPM } from '../../../EntityPMs/TariffPM';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { TariffVersionAllInChargePM } from '../../../EntityPMs/TariffVersionAllInChargePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DateTool } from '../../../../Infrastructure/Tools';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAllInChargesComponent.html',
})

export class AddEditAllInChargesComponent {
    public TariffPM: TariffPM = null;
    public EntityPM: TariffVersionPM;
    public ItemsSource: AllInChargeItemClass[] = [];
    public ObjectTableName: string = "TariffVersionAllInCharge";
    public IsEditingEnabled: boolean = true;
    public ValidationErrorsList: string[];
    public IsVisibile: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public ChargeTypesQueryFilters: ApiQueryFilters;
    constructor(private entityResourceService: EntityResourceService) {

    }

    private isVersionDirty: boolean = false;
    private isTariffDirty: boolean = false;
    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(response => {
            if (args) {
                this.IsEditingEnabled = args['IsEditingEnabled'];
                this.EntityPM = args['VersionPM'];
                this.TariffPM = args['TariffPM'];

                this.isVersionDirty = this.EntityPM.IsDirty;
                this.isTariffDirty = this.TariffPM.IsDirty;

                this.BuildQueryFilters();

                this.EntityPM.TariffAllInCharges.forEach((item: TariffVersionAllInChargePM) => {
                    this.ItemsSource.push(new AllInChargeItemClass(item, this));
                });

                this.Clone();
            }

            this.IsVisibile = true;
        });
    }

    BuildQueryFilters() {
        var entityType: string = "IsAir";
        if (this.TariffPM.TypeCode == "OSC" || this.TariffPM.TypeCode == "OLC" || this.TariffPM.TypeCode == "OFC") {
            entityType = "IsOcean";
        }

        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter(entityType, true, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("ChargesGroupCode", "FRT", null, null, "NotEqual", false, false, false, "string");
    }

    AddButtonClicked() {
        var item: TariffVersionAllInChargePM = new TariffVersionAllInChargePM(this.EntityPM);
        item.Version = this.EntityPM.Version;
        item.TariffId = this.TariffPM.Id;
        item.AddDate = DateTool.GetCurrentDateAsUtc();
        item.AddedByUserId = SessionInfo.LoggedUserId;
        item.Tenant = SessionInfo.LoggedUserTenant;
        this.ItemsSource.push(new AllInChargeItemClass(item, this));
    }
    DeleteItem(item: AllInChargeItemClass) {
        if (item) {
            var index = this.ItemsSource.indexOf(item);
            if (index > -1) {
                this.ItemsSource.splice(index, 1);
            }
        }
    }

    CancelButtonClicked() {
        this.ItemsSource.forEach((item: AllInChargeItemClass) => {
            if (item.ChargesTypeId != item.OldValue) {
                item.ChargesTypeId = item.OldValue;
            }
        });

        this.EntityPM.IsDirty = this.isVersionDirty;
        this.TariffPM.IsDirty = this.isTariffDirty;

        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        this.ItemsSource.forEach(item => {
            Validator.TryValidateObject(item.EntityPM, this.ObjectTableName, errors);
        });

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var allItemsPM: TariffVersionAllInChargePM[] = [];

            this.ItemsSource.forEach((item: AllInChargeItemClass) => {
                var index = this.EntityPM.TariffAllInCharges.indexOf(item.EntityPM);

                if (index > -1) {
                    var itemPM = this.EntityPM.TariffAllInCharges[index];
                    if (itemPM) {
                        
                    }
                }

                else {
                    this.EntityPM.TariffAllInCharges.push(item.EntityPM);
                }

                allItemsPM.push(item.EntityPM);
            });

            for (var i = this.EntityPM.TariffAllInCharges.length - 1; i >= 0; i--) {

                var index = allItemsPM.indexOf(this.EntityPM.TariffAllInCharges[i]);

                if (index == -1) {
                    var item = this.EntityPM.TariffAllInCharges[i];
                    this.EntityPM.RemoveTariffVersionAllInCharge(item);
                }
            }
            
            this.CurrentSession.CloseCurrentWindowEmit("Ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {

    }
    private RejectChanges() {
        //this.DataContext.ResetPackageItems();
        //this.myCloner.RejectChanges();
    }    
}

class AllInChargeItemClass extends BaseComponent {
    public Id: string = null;
    public EntityPM: TariffVersionAllInChargePM;
    public ObjectTableName: string = "TariffVersionAllInCharge";
    public OldValue: string = null;
    constructor(item: TariffVersionAllInChargePM, public parent: AddEditAllInChargesComponent) {
        super();
        this.Id = item.Id;
        this.EntityPM = item;
        this.OldValue = item.ChargesTypeId;

        this.SetUIProperties();
    }

    public get ChargesTypeId() { return this.EntityPM.ChargesTypeId; }
    public set ChargesTypeId(value: string) {
        if (this.EntityPM.ChargesTypeId != value) {
            this.EntityPM.ChargesTypeId = value;
        }
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, this.parent.IsEditingEnabled);
    }
}
