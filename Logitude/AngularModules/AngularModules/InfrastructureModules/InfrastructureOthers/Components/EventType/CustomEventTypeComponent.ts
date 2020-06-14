
import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import { EventTypePM } from '../../../../Infrastructure/EntityPMs/EventTypePM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
declare var window: any;

@Component({

    selector: 'CustomEventTypeComponent',
    templateUrl: './CustomEventTypeComponent.html',
})

export class CustomEventTypeComponent {



    constructor() {

    }



    IsDisable: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: EventTypePM;
    public ObjectTableName: string;
    public IsNewEntityCall: boolean;
    public IsDataReady: boolean = false;
    public ObjectFieldFilterItems: ApiQueryFilters;
    public IsRefreshComponent: boolean;
    public IsEnable: boolean = false;
    public IsCustomerCare: boolean = false;

    public Run(args: any) {
        this.EntityPM = args['DataContext'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsNewEntityCall = args['IsNewEntityCall'];
        this.IsCustomerCare = SessionLocator.LoggedUserPM.IsCustomerCare;

        if (this.EntityPM) {

            this.IsDataReady = true;
            if (!this.EntityPM.ObjectTableId) {
                this.IsDisable = true;
               
            }

        }

    }

    CustomObjectFieldChange(value) {

        var customFieldValue: string;
        if (value) customFieldValue = value.FieldCode;

        if (customFieldValue != this.EntityPM.CustomField) {
            this.EntityPM.CustomField = customFieldValue;

        }
    }


    GetObjectFieldFilterItems() {
        this.CustomField = this.EntityPM.CustomField;
        this.ObjectFieldFilterItems = new ApiQueryFilters();

        var objectTableId: string = this.ObjectTableId;

        if (!objectTableId) objectTableId = "none";

        this.ObjectFieldFilterItems.addAdditionalFilter("ObjectTableId", objectTableId, null, null, "Equals", false, false, false, "string");
        this.ObjectFieldFilterItems.addAdditionalFilter("DataTypeCode", "Date", null, null, "Contains", false, false, false, "string");
        this.ObjectFieldFilterItems.addAdditionalFilter("IsCustom", true, null, null, "Equals", false, false, false, "boolean");



    }


    ObjectTableId: string;
    GetObjectTableId() {

        if (this.EntityPM) {
            if (this.EntityPM.ObjectTableId != this.ObjectTableId) {
                if (this.ObjectTableId) {
                    this.EntityPM.CustomField = null;
                    this.IsRefreshComponent = !this.IsRefreshComponent;
                }
                this.ObjectTableId = this.EntityPM.ObjectTableId;
                this.CustomField = null;


                var objectTable = window.ObjectTables.filter(x => x.Id === this.EntityPM.ObjectTableId)[0];
                if (objectTable) {

                    this.IsDisable = !objectTable.AllowCustomFields ? true : false;
                    if (objectTable.Name == "Shipment" || objectTable.Name == "Master")
                        this.IsEnable = true;
                    else
                        this.IsEnable = false;
                }
                this.GetObjectFieldFilterItems();

            }

        }
        return this.ObjectTableId;
    }



    customField: string;
    get CustomField() {
        this.GetObjectTableId();
        return this.customField;
    }
    set CustomField(value: string) {

        if (this.customField != value) {
            this.customField = value;
        }
    }




}
