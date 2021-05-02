import {Component, Output, EventEmitter, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { ProductItemPM } from '../../../../Common/EntityPMs/ProductItemPM';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';

@Component({
    
    templateUrl: './AddEdiCustomerProductItemComponent.html',
})

export class AddEdiCustomerProductItemComponent extends BaseComponent implements OnInit {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public TenantPM: TenantPM;
    public DataContext: AddEdiCustomerProductItemComponent = this;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new ProductItemPM(null);
        this.EntityPM.Tenant = SessionLocator.Tenant;
    }

    ngOnInit() {
        this.CreateProductItem();
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ItemCode));
    }

    CreateProductItem() {
        this.EntityPM = new ProductItemPM(null);
        this.EntityPM.Tenant = this.TenantPM.Id;
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    get ItemCode() { return null }
    set ItemCode(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
            this.SetUIProperties();
        }
    }
}
