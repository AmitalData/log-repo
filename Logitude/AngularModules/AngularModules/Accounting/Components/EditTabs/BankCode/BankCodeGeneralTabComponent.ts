
import {Component}  from '@angular/core';

import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {BankCodePM} from '../../../EntityPMs/BankCodePM';


@Component({
    moduleId: module.id,
    templateUrl: './BankCodeGeneralTabComponent.html',
})

export class BankCodeGeneralTabComponent extends BaseComponent {
    public EntityPM: BankCodePM = null;
    public ObjectTableName = "BankCode";
    public DataContext = this;
    EntityId: string;
    ImageId: string;
    constructor(private entityArgs: EntityArgs) {
        super();
        
        this.EntityPM = entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.ImageId = this.EntityPM.LogoId;
       
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;

         
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;


        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;


        }
    }

    get LogoId() { return this.EntityPM.LogoId; }
    set LogoId(value: string) {
        if (this.EntityPM.LogoId != value) {
            this.EntityPM.LogoId = value;


        }
    }

    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;


        }
    }


    InactiveChecked(checked: boolean) {
     
        if (checked) {
            this.Inactive = true;
        }
        else this.Inactive = false;
    }



    ImageUploadedCompleted(code) {
        this.ImageId = code;
        this.EntityPM.LogoId = code;
    }

}