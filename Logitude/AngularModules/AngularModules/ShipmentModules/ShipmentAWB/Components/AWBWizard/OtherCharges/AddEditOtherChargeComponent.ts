import {Component, AfterViewInit} from '@angular/core';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {AWBWizardOtherChargeItem} from './OtherChargesTabComponent';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,

    templateUrl: './AddEditOtherChargeComponent.html',
})

export class AddEditOtherChargeComponent extends BaseComponent implements AfterViewInit {
    public ObjectTableName: string;
    public DataContext: AWBWizardOtherChargeItem;
    public ValidationErrorsList: string[] = [];
    public TenantZeroAirlineId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetDataContext(dataContext: AWBWizardOtherChargeItem) {
        this.DataContext = dataContext;
        if (this.DataContext != null) {
            this.DataContext.SetUIProperties();
        }
        this.DataContext.IsWindowMode = true;
        this.ObjectTableName = dataContext.ObjectTableName;
        this.TenantZeroAirlineId = dataContext.ShipmentPM.TenantZeroAirlineId;
        this.Clone();
    }

    ngAfterViewInit() {
       
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        if (this.DataContext.PayablePM != null) {
            Validator.TryValidateObject(this.DataContext.PayablePM, this.DataContext.ObjectTableName, errors);
        }

        else if (this.DataContext.ReceivablePM != null) {
            Validator.TryValidateObject(this.DataContext.ReceivablePM, this.DataContext.ObjectTableName, errors);
        }

        else if (this.DataContext.AWBPrintOnlyPM != null) {
            Validator.TryValidateObject(this.DataContext.AWBPrintOnlyPM, this.DataContext.ObjectTableName, errors);

            if (AppTool.IsNullOrEmpty(this.DataContext.IATACodeId)) {
                errors.push("IATA code field is required");
            }

            if (AppTool.IsNullOrEmpty(this.DataContext.PrepaidCollectId)) {
                errors.push("P/C field is required");
            }

            if (this.DataContext.CurrencyId != this.DataContext.ShipmentPM.AWBCurrencyId) {
                errors.push("Currency is not matching the shipment awb currency");
            }
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {

                this.DataContext.IsNewEntity = false;

                if (this.DataContext.AWBPrintOnlyPM != null) {
                    if (this.DataContext.ShipmentPM.ShipmentAWBPrintOnlies.indexOf(this.DataContext.AWBPrintOnlyPM) == -1) {
                        this.DataContext.ShipmentPM.AddAWBPrintOnly(this.DataContext.AWBPrintOnlyPM);
                    }

                    if (this.DataContext.fatherComponent.ItemsSource.indexOf(this.DataContext) == -1) {
                        this.DataContext.fatherComponent.ItemsSource.push(this.DataContext);
                    }
                }
            }

            this.DataContext.fatherComponent.ComputeTotals();
            this.CurrentSession.CloseCurrentWindow();
            this.DataContext.IsWindowMode = false;
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('IATACodeId');
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('PrepaidCollectId');
        this.myCloner.AddField('DueTypeCode');
        this.myCloner.AddField('CurrencyId');
        this.myCloner.AddField('MeasurementId');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddField('Amount');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddEntity(this.DataContext.PayablePM);
        this.myCloner.AddEntity(this.DataContext.ReceivablePM);
        this.myCloner.AddEntity(this.DataContext.AWBPrintOnlyPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
