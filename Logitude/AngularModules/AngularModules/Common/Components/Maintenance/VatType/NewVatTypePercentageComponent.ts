import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {VatTypePM} from '../../../EntityPMs/VatTypePM';
import {VatTypePercentagePM} from '../../../EntityPMs/VatTypePercentagePM';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'NewChargesTypeComponent',
    moduleId: module.id,
    templateUrl: './NewVatTypePercentageComponent.html',
})

export class NewVatTypePercentageComponent extends BaseComponent {
    public EntityPM: VatTypePercentagePM = null;
    public VatTypePM: VatTypePM;
    public DataContext = this;
    public ObjectTableName: string = "VatTypePercentage";
    public ValidationErrorsList: string[] = [];
    public IsNewEntity: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.VatTypePM = args['VatTypePM'];
        this.IsNewEntity = args['IsNewEntity'];
        this.Clone();
    }

    public get Percentage() { return this.EntityPM.Percentage; }
    public set Percentage(value: number) {
        if (this.EntityPM.Percentage != value) {
            this.EntityPM.Percentage = value;
        }
    }

    public get FromDate() { return this.EntityPM.FromDate; }
    public set FromDate(value: Date)
    {
        if (this.EntityPM.FromDate != value) {
            this.EntityPM.FromDate = value
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.IsNewEntity) {
                this.VatTypePM.AddVatTypePercentagePM(this.EntityPM);
            }

            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Percentage');
        this.myCloner.AddField('FromDate');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.VatTypePM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }  
}
