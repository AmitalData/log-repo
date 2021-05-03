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
import { CustomerPM } from '../../../../Common/EntityPMs/CustomerPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { CustomerPMService } from '../../../../Common/Services/StandardPMs/CustomerPMService';

@Component({
    
    templateUrl: './AddEditCustomerProductItemComponent.html',
})

export class AddEditCustomerProductItemComponent extends BaseComponent implements OnInit {
    public EntityPM: ProductItemPM;
    public ObjectTableName: string = "ProductItem";
    public TenantPM: TenantPM;
    public DataContext: AddEditCustomerProductItemComponent = this;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    public ParentEntity: CustomerPM = null;
    public IsNew: boolean;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new ProductItemPM(this.ParentEntity);
        this.ParentEntity =  this.entityArgs.EntityPM;
    }

    public IsResourcesReady: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['ProductItemPM'];
        this.ParentEntity = windowArgs['CustomerPM'];
        this.IsNew = windowArgs['IsNew'];
        this.SetUIProperties();
        this.IsResourcesReady = true;
    }

    ngOnInit() {
        this.CreateProductItem();
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("ItemCode", this.ObjectTableName, AppTool.IsNullOrEmpty(this.ItemCode));
    }

    CreateProductItem() {
        this.EntityPM = new ProductItemPM(this.ParentEntity);
        this.EntityPM.Tenant = this.TenantPM.Id;
    }

    SetCustomerId() {
        if (this.ParentEntity != null) {
            this.EntityPM.CustomerId = this.ParentEntity.Id
        }
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

    get ItemCode() { return this.EntityPM.ItemCode }
    set ItemCode(value: string) {
        if (this.EntityPM.ItemCode != value) {
            this.EntityPM.ItemCode = value;
        }
    }

    get SKU() { return this.EntityPM.SKU }
    set SKU(value: string) {
        if (this.EntityPM.SKU != value) {
            this.EntityPM.SKU = value;
        }
    }

    get Remarks() { return this.EntityPM.Remarks }
    set Remarks(value: string) {
        if (this.EntityPM.Remarks != value) {
            this.EntityPM.Remarks = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.DataContext.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.ParentEntity.AddProductItemPM(this.EntityPM);
            this.SubmitCreatingNewProductItem();
        }
    }

    SubmitCreatingNewProductItem() {
        var service: CustomerPMService = new CustomerPMService();
        service.update(this.ParentEntity).subscribe((myResult: any) => {
            if (myResult) {
                if (!myResult.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    this.ValidationErrorsList = myResult.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

}
