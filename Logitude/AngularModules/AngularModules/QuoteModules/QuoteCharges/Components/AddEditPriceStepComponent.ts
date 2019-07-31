import {Component} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {QuoteStepItem} from './AddEditLCLChargeComponent';
import {QuotePriceStepsPM} from '../../../Quote/EntityPMs/QuotePriceStepsPM';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditPriceStepComponent.html',
})

export class AddEditPriceStepComponent extends BaseComponent {
    public QuotePM: QuotePM;
    public DataContext: QuoteStepItem;
    public ObjectTableName: string = "QuotePriceSteps";
    public EntityPM: QuotePriceStepsPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetDataContext(dataContext: QuoteStepItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.QuotePM = dataContext.fatherComponent.QuotePM;
        this.Clone();
    }
    
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.DataContext.fatherComponent.EntityPM.QuoteChargePriceSteps.filter(d => d.Step == this.DataContext.Step).forEach((item) => {
            if (item != this.EntityPM) {
                errors.push("Price Steps list already contains Step: " + AppTool.Round(this.DataContext.Step, 2));                
            }
        });

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            if (this.DataContext.IsNew) {
                this.DataContext.fatherComponent.EntityPM.AddQuotePriceStepsPM(this.EntityPM);
            }

            this.DataContext.fatherComponent.BuildStepItemsSource();
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Step');
        this.myCloner.AddField('CostUnitPrice');
        this.myCloner.AddField('MarkupValue');
        this.myCloner.AddField('SaleUnitPrice');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.QuoteChargePM);
        this.myCloner.AddEntity(this.QuotePM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
