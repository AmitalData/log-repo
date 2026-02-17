import {Component} from '@angular/core';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AWBWizardOCIItem} from './OCITabComponent';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {AWBOCIPM} from '../../../../../Shipment/EntityPMs/AWBOCIPM';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,

    templateUrl: './AddEditOCIComponent.html',
})

export class AddEditOCIComponent extends BaseComponent {
    public EntityPM: AWBOCIPM;
    public ObjectTableName: string;
    public DataContext: AWBWizardOCIItem;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    
    SetDataContext(dataContext: AWBWizardOCIItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext.IsWindowMode = true;
        this.DataContext.SetUIProperties();
        this.ObjectTableName = dataContext.ObjectTableName;
        this.Clone();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.DataContext.IsWindowMode = false;
        this.CurrentSession.CloseCurrentWindow();       
    }
    
    OkButtonClicked() {
                        
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.DataContext.CountryId) && AppTool.IsNullOrEmpty(this.DataContext.AWBCustomsInformationCode) && AppTool.IsNullOrEmpty(this.DataContext.AWBInformationCode)) {
            errors.push("You must fill one of the fields (Country or Information or CustomsInformation)");
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.DataContext.IsNewEntity || this.EntityPM.IsAWBWizardDefault) {

                this.DataContext.IsNewEntity = false;
                this.EntityPM.IsAWBWizardDefault = false;

                if (this.DataContext.ShipmentPM.AWBOCIPMs.indexOf(this.EntityPM) == -1) {
                    this.DataContext.ShipmentPM.AddOCI(this.EntityPM);
                    this.DataContext.fatherComponent.BuildData(); 
                }
            }

            this.DataContext.IsWindowMode = false;
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('CountryId');
        this.myCloner.AddField('AWBInformationCode');
        this.myCloner.AddField('AWBCustomsInformationCode');
        this.myCloner.AddField('SupplementaryCustomsInfo');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
