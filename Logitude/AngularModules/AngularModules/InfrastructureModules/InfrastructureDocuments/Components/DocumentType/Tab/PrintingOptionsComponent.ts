import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {DocumentTypePM} from '../../../../../Common/EntityPMs/DocumentTypePM';
import {DocumentTypeCopyPM} from '../../../../../Common/EntityPMs/DocumentTypeCopyPM';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../../Infrastructure/Tools';

@Component({
    
    selector: 'SharedLogisticsTab',
    templateUrl: './PrintingOptionsComponent.html',
})

export class PrintingOptionsComponent extends BaseComponent implements OnInit {
    public EntityPM: DocumentTypePM;
    public DocumentTypeCopies: DocumentTypeCopyPM[];
    SelectedCopy: DocumentTypeCopyPM;
    public ComboBoxIsDisabled: boolean = false;
    public ObjectFieldFilterItems: ApiQueryFilters;
    ObjectTableId: string;
    DataContext: any = this;

    IsShowPopulateAutomaticDate : boolean = false;


    constructor(public entityArgs: EntityArgs) {
        super();

        this.SetIsShowPopulateAutomaticDate();

    }

    private SetIsShowPopulateAutomaticDate() {
        let isCustomizationToggleActive = SessionLocator.FeatureToggles.some(d => d.ToggleCode == "CUS");
        if (SessionLocator.LoggedUserPM.IsCustomerCare || isCustomizationToggleActive) {
            this.IsShowPopulateAutomaticDate = true;
        }
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM) {
            this.ObjectTableId = this.EntityPM.ObjectTableId;
            this.GetObjectFieldFilterItems();
            this.Listen();
            var iCode: string = this.EntityPM.Code;
            if (iCode) {
                iCode = iCode.toUpperCase();
                switch (iCode) {
                    case "999S":
                    case "999C":
                    case "999M":
                    case "999CI":
                    case "999MP":
                    case "999P":
                    case "ARINV":
                        {

                            if (SessionLocator.TenantPM.CountryCode == "IL" && SessionLocator.LoggedUserPM.IsCustomerCare == false) {
                                this.ComboBoxIsDisabled = true;
                                this.EntityPM.UIProperties.SetEnabled("IsDocumentOneTimePrintLimited", "DocumentType", false)
                            }

                            break;
                        }
                }
            }

            this.Run();
        }
    }




    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);


    }
    private TabSelectedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent) {

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "DTPO") {
                    if (this.ObjectTableId != this.EntityPM.ObjectTableId) {
                        this.ObjectTableId = this.EntityPM.ObjectTableId;
                        this.GetObjectFieldFilterItems();
                        this.IsRefreshPopulateDateField = !this.IsRefreshPopulateDateField;


                    }
                }
            });
        }
    }











    Run() {

        if (this.EntityPM.DocumentTypeCopies) {
            this.DocumentTypeCopies = this.EntityPM.DocumentTypeCopies;
            this.SelectedCopy = this.DocumentTypeCopies.filter(d => d.Id == this.EntityPM.LimitedPrintCopyId)[0];
        }

      


    }

    IsRefreshPopulateDateField: boolean = false;
    GetObjectFieldFilterItems() {

        this.OnSendPopulateDateFieldName = this.EntityPM.OnSendPopulateDateFieldName;
        this.OnPrintPopulateDateFieldName = this.EntityPM.OnPrintPopulateDateFieldName;
        this.OnUploadPopulateDateFieldName = this.EntityPM.OnUploadPopulateDateFieldName;

        this.ObjectFieldFilterItems = new ApiQueryFilters();
        var objectTableId: string = this.EntityPM.ObjectTableId;
        if (!objectTableId) objectTableId = "none";
        this.ObjectFieldFilterItems.addAdditionalFilter("ObjectTableId", objectTableId, null, null, "Equals", false, false, false, "string");
        this.ObjectFieldFilterItems.addAdditionalFilter("DataTypeCode", "Date", null, null, "Contains", false, false, false, "string");

    }

    OnPopulateDateFieldNameFieldChange(value , fieldType:string) {
        var objectFieldCode: string;
        if (value) objectFieldCode = value.FieldCode;

        if (fieldType == "Send") {
            if (objectFieldCode != this.EntityPM.OnSendPopulateDateFieldName) this.EntityPM.OnSendPopulateDateFieldName = objectFieldCode;
        } else if (fieldType == "Print") {
            if (objectFieldCode != this.EntityPM.OnPrintPopulateDateFieldName) this.EntityPM.OnPrintPopulateDateFieldName = objectFieldCode;
        } else if (fieldType == "Upload") {
            if (objectFieldCode != this.EntityPM.OnUploadPopulateDateFieldName) this.EntityPM.OnUploadPopulateDateFieldName = objectFieldCode;
        }
    }





    DocumentTypeCopyValueChanged(Copy:any) {

        this.EntityPM.LimitedPrintCopyId = Copy.Id;
    }




    onSendPopulateDateFieldName: string;
    get OnSendPopulateDateFieldName() {
        return this.onSendPopulateDateFieldName;
    }
    set OnSendPopulateDateFieldName(value: string) {
        if (this.onSendPopulateDateFieldName != value) this.onSendPopulateDateFieldName = value;
    }



    onPrintPopulateDateFieldName: string;
    get OnPrintPopulateDateFieldName() {
        return this.onPrintPopulateDateFieldName;
    }
    set OnPrintPopulateDateFieldName(value: string) {
        if (this.onPrintPopulateDateFieldName != value) this.onPrintPopulateDateFieldName = value;
    }


    onUploadPopulateDateFieldName: string;
    get OnUploadPopulateDateFieldName() {
        return this.onUploadPopulateDateFieldName;
    }
    set OnUploadPopulateDateFieldName(value: string) {
        if (this.onUploadPopulateDateFieldName != value) this.onUploadPopulateDateFieldName = value;
    }

 



}






