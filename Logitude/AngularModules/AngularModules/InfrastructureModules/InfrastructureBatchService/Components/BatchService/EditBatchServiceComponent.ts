import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BatchServiceItemClass} from './BatchServicesComponent';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BatchServicesDefinitionPMService} from '../../../../Infrastructure/Services/ExtendedPMs/BatchServicesDefinitionPMService';

@Component({
    moduleId: module.id,
    templateUrl: './EditBatchServiceComponent.html',
})

export class EditBatchServiceComponent extends BaseComponent {
    public ObjectTableName: string = "BatchServicesDefinition";
    public DataContext: BatchServiceItemClass;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetDataContext(dataContext: BatchServiceItemClass) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        
        this.Clone();
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            var service: BatchServicesDefinitionPMService = new BatchServicesDefinitionPMService();

            service.update(this.EntityPM).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindow();
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });            
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('NumberOfThreads');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
