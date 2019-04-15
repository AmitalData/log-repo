declare var System: any;
declare var window: any;
import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { PhysicalCheckWebService } from '../../../../../Customs/Services/WebServices/PhysicalCheckWebService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { DeclarationPM } from               '../../../../../Customs/EntityPMs/DeclarationPM';
import { SupplierInvoiceItemsTaxList } from '../../../../../Customs/EntityLists/Extended/SupplierInvoiceItemsTaxList';
//import { PhysicalCheckPMService } from '../../../../../Customs/Services/StandardPMs/PhysicalCheckPMService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../../../Infrastructure/Services/EntityListService';

@Component({

    moduleId: module.id,
    templateUrl: './ItemTaxesMoreFieldsComponent.html',
})

export class ItemTaxesMoreFieldsComponent{// implements OnInit {
    public EntityPM: SupplierInvoiceItemsTaxList;
    public ObjectTableName = "Customs.SupplierInvoiceItemsTax";
    public DataContext: ItemTaxesMoreFieldsComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        
    }

    //ngOnInit() {
    //    this.EntityPM = this.entityArgs.EntityPM;
    //}

    _IsVisible: boolean = false;
    public SetWindowArgs(windowArgs: SupplierInvoiceItemsTaxList) {
        ///console.warn(windowArgs);
        this.EntityPM = windowArgs;
        this._IsVisible = true;
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    get DefinedPerUnitMeasure() { return this.EntityPM == null ? null : this.EntityPM.DefinedPerUnitMeasure; }
    get AlternateDefinedPerUnitMeasure() { return this.EntityPM == null ? null : this.EntityPM.AlternateDefinedPerUnitMeasure; }
    get AlternateRate() { return this.EntityPM == null ? null : this.EntityPM.AlternateRate; }
    get DefinedPerUnitQuantity() { return this.EntityPM == null ? null : this.EntityPM.DefinedPerUnitQuantity; }
    get AlternateDefinedPerUnitQuant() { return this.EntityPM == null ? null : this.EntityPM.AlternateDefinedPerUnitQuant; }
    get TradeLevyNumber() { return this.EntityPM == null ? null : this.EntityPM.TradeLevyNumber; }
    get MeasurementUnitCode() { return this.EntityPM == null ? null : this.EntityPM.MeasurementUnitCode; }
    get AlternateMeasurementUnitCode() { return this.EntityPM == null ? null : this.EntityPM.AlternateMeasurementUnitCode; }
    get TotalBtlCoverageNIS() { return this.EntityPM == null ? null : this.EntityPM.TotalBtlCoverageNIS; }            

}
