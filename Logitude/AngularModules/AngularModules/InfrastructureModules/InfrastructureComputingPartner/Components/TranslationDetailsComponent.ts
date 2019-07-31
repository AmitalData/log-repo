import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../Infrastructure/Tools';
import {ComputingPartnerPM} from '../../../Common/EntityPMs/ComputingPartnerPM';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ComputingPartnerTablePM} from '../../../Common/EntityPMs/ComputingPartnerTablePM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ComputingPartnerPMService} from '../../../Common/Services/StandardPMs/ComputingPartnerPMService';
import {ComputingPartnerTranslationPM} from '../../../Common/EntityPMs/ComputingPartnerTranslationPM';
import {TranslationItem} from '../../../Common/Services/CommonDomainService';

@Component({
    selector: 'TranslationDetailsComponent',
    moduleId: module.id,
    templateUrl: './TranslationDetailsComponent.html',
})

export class TranslationDetailsComponent   {

    private EntityPM: TranslationItem;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args.entityPM;
    }



  

    public get OurCode() { return this.EntityPM.OurCode; }
    public get Name() { return this.EntityPM.Name; }
    public get PartnerCode() { return this.EntityPM.PartnerCode; }
    public get CreateDate() { return this.EntityPM.CreateDate; }
    public get UpdateDate() { return this.EntityPM.UpdateDate; }
    public get CreatedByUserName() {

        return this.EntityPM.CreatedByUserName;
    }
    
    public get UpdatedByUserName() {      

        return this.EntityPM.UpdatedByUserName
    }

    public get DefaultTranslationVisibility() {
        return SessionLocator.Tenant != 0 ? true : false;
    }

    public get DefaultTranslation() {
        return this.EntityPM.DefaultTranslationPartnerCode;
    }

    public get CreateDate_Default() {
        return this.EntityPM.CreatedDateDefault;
    }

    public get UpdateDate_Default() {
        return this.EntityPM.UpdatedDateDefault;
    }

    public get CreatedByUserName_Default() {
        return this.EntityPM.CreatedByUserNameDefault;
    }

    public get UpdatedByUserName_Default() {
        return this.EntityPM.UpdatedByUserNameDefault;
    }
    
    CloseButtonClick() {
        this.CurrentSession.CloseCurrentWindow();
    }



}
