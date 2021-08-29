import {Component, ChangeDetectorRef}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ChartOfAccountPM} from '../../../EntityPMs/ChartOfAccountPM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './ChartOfAccountGeneralTabComponent.html'
})

export class ChartOfAccountGeneralTabComponent extends BaseComponent {
    public oldCurrency: string = null;
    public EntityPM: ChartOfAccountPM = null;
    public ObjectTableName = "ChartOfAccount";
    public DataContext = this;
    public DisableChartOfAccount: boolean = false;
    public IsEditMode: boolean = false;
    public IsCustomerAccount: boolean = false;
    public ChartOfAccountTypeFilterItems: ApiQueryFilters;
    public ParentsFilterItems: ApiQueryFilters;


    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();

        // Set Entity
        this.EntityPM = entityArgs.EntityPM;

        // initialize query filters for Parent Account
        this.ParentsFilterItems = new ApiQueryFilters();
        this.ParentsFilterItems.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "Exclude", false, false, false, "string");

        this.SetUIProperties();

    }

    // Properties
    get Inactive() { return this.EntityPM.Inactive == null ? false : this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.EntityPM.TypeCode != value) {
            this.EntityPM.TypeCode = value;
            if (value != null) {
                this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, true);
            } else {
                this.ParentId = null;
                this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
            }
        }
    }

    get ParentId() { return this.EntityPM.ParentId; }
    set ParentId(value: string) {
        if (this.EntityPM.ParentId != value) {
            this.EntityPM.ParentId = value;
        }
    }

    get ChartOfAccountSecurityLevel() { return this.EntityPM.ChartOfAccountSecurityLevel; }
    set ChartOfAccountSecurityLevel(value: number) {
        if (this.EntityPM.ChartOfAccountSecurityLevel != value) {
            this.EntityPM.ChartOfAccountSecurityLevel = value;
        }
    }

    SetUIProperties() {
        if (!this.EntityPM.TypeCode) {
            this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
        }

    }

    OnLovItemChanged(item: any) {
        //if (item == null) {
        //    this.ChartOfAccountsId = null;
        //    this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
        //    this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, false);
        //    this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, true, "Chart Of Accounts is requierd");
        //} else {
        //    this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, true);
        //    if (!this.EntityPM.ChartOfAccountsId) {
        //        this.UIProperties.SetValidity("ChartOfAccountsId", this.ObjectTableName, false, "");
        //        this.UIProperties.SetRequired("ChartOfAccountsId", this.ObjectTableName, true);
        //    }
        //}
    }

}
