import { Component } from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent'; 
//import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
//import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
//import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';
//import {Guid} from '../../../Infrastructure/Utilities/Guid';

@Component({
    moduleId: module.id,
    selector: 'DWFilterSettings',
    templateUrl: './DWFilterSettings.html',
})

export class DWFilterSettings extends BaseComponent  {
    public ObjectTableName: string = "TenantManagement";
    DataContext: any = this;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = []; 
    SystemSupportEnabledKey: string = "";
    DistributorSupportEnabledKey: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super(); 
    }
    SetWindowArgs(args: any) { 
        this.IsSetDefaults = args.IsSetDefaults; 
        this.IsMandatoryFilter = args.IsMandatoryFilter; 
    }
   
    IsMandatoryFilter: boolean = false;
    IsSetDefaults: boolean = false;
    MandatoryFilterChecked(value) {
        this.IsMandatoryFilter = value; 
    }

    SetDefaultsChecked(value) {
        this.IsSetDefaults = value;

    }
    
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit(this.IsMandatoryFilter + "," + this.IsSetDefaults);
        //if ((!this.IsSystemSupportEnabledCheck && this.IsSystemSupportEnabled) || (!this.IsDistributorSupportEnabledCheck && this.IsDistributorSupportEnabled)) {
        //    this.ShowConfirmationWindow();
        //}

        //else {
        //    this.SaveChanges();
        //}            
    }
     
    

}
