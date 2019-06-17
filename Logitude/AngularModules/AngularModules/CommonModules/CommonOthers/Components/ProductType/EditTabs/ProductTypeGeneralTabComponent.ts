
import {Component} from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {ProductTypePM} from '../../../../../Common/EntityPMs/ProductTypePM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
@Component({
    selector: 'ProductTypeGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './ProductTypeGeneralTabComponent.html',
})

export class ProductTypeGeneralTabComponent extends BaseComponent {

    public EntityPM: ProductTypePM;
    public ObjectTableName: string = "ProductType";
    private CurrentSession = SessionLocator.SelectedSession;

    DataContext: ProductTypeGeneralTabComponent=this;
    constructor(public entityArgs: EntityArgs) {
        super();
       
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;

    }

    ngOnInit() {
        this.SetUIPropertiesEnabled();
    }



    SetUIPropertiesEnabled() {
    
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Name", this.ObjectTableName, false);
    }



    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
    
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }


    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;

        }
    }



    get QuotationDefaultTemplateId() { return this.EntityPM.QuotationDefaultTemplateId; }
    set QuotationDefaultTemplateId(value: string) {
        if (this.EntityPM.QuotationDefaultTemplateId != value) {
            this.EntityPM.QuotationDefaultTemplateId = value;
        }
    }


    get RoutingRQuoteDefaultTemplateId() { return this.EntityPM.RoutingRQuoteDefaultTemplateId; }
    set RoutingRQuoteDefaultTemplateId(value: string) {
        if (this.EntityPM.RoutingRQuoteDefaultTemplateId != value) {
            this.EntityPM.RoutingRQuoteDefaultTemplateId = value;
        }
    }
}
