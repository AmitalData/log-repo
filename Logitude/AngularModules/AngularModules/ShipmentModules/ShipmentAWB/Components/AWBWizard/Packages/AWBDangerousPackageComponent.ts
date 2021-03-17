import {Component} from '@angular/core';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    

    templateUrl: './AWBDangerousPackageComponent.html',
})

export class AWBDangerousPackageComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext: AWBDangerousPackageComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: ShipmentPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = this.EntityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.SetUIProperties();
        this.Clone();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("DangerousClassNumber", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousUnNumber", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousPackagingGroup", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousIMDGCode", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousFlashPoint", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("DangerousMaterialDescription", this.ObjectTableName, this.IsDangerous);
        this.UIProperties.SetEnabled("EmergencyContactId", this.ObjectTableName, this.IsDangerous);
    }

    get EmergencyContactId() { return this.EntityPM.EmergencyContactId; }
    set EmergencyContactId(value: string) {
        if (this.EntityPM.EmergencyContactId != value) {
            this.EntityPM.EmergencyContactId = value;
        }
    }

    get IsDangerous() { return this.EntityPM.IsDangerous; }
    set IsDangerous(newValue: boolean) {
        if (this.EntityPM.IsDangerous != newValue) {
            this.EntityPM.IsDangerous = newValue;
            this.SetUIProperties();

            if (!newValue) {
                this.DangerousClassNumber = null;
                this.DangerousUnNumber = null;
                this.DangerousPackagingGroup = null;
                this.DangerousIMDGCode = null;
                this.DangerousFlashPoint = null;
                this.DangerousMaterialDescription = null;
            }
        }
    }

    get DangerousClassNumber() { return this.EntityPM.DangerousClassNumber; }
    set DangerousClassNumber(newValue: string) {
        if (this.EntityPM.DangerousClassNumber != newValue) {
            this.EntityPM.DangerousClassNumber = newValue;
        }
    }

    get DangerousUnNumber() { return this.EntityPM.DangerousUnNumber; }
    set DangerousUnNumber(newValue: string) {
        if (this.EntityPM.DangerousUnNumber != newValue) {
            this.EntityPM.DangerousUnNumber = newValue;
        }
    }

    get DangerousPackagingGroup() { return this.EntityPM.DangerousPackagingGroup; }
    set DangerousPackagingGroup(newValue: string) {
        if (this.EntityPM.DangerousPackagingGroup != newValue) {
            this.EntityPM.DangerousPackagingGroup = newValue;
        }
    }

    get DangerousIMDGCode() { return this.EntityPM.DangerousIMDGCode; }
    set DangerousIMDGCode(newValue: string) {
        if (this.EntityPM.DangerousIMDGCode != newValue) {
            this.EntityPM.DangerousIMDGCode = newValue;
        }
    }

    get DangerousFlashPoint() { return this.EntityPM.DangerousFlashPoint; }
    set DangerousFlashPoint(newValue: string) {
        if (this.EntityPM.DangerousFlashPoint != newValue) {
            this.EntityPM.DangerousFlashPoint = newValue;
        }
    }

    get DangerousMaterialDescription() { return this.EntityPM.DangerousMaterialDescription; }
    set DangerousMaterialDescription(newValue: string) {
        if (this.EntityPM.DangerousMaterialDescription != newValue) {
            this.EntityPM.DangerousMaterialDescription = newValue;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        var errors: string[] = [];

        if (!AppTool.IsNullOrEmpty(this.DangerousFlashPoint) && this.DangerousFlashPoint.length > 8) {
            errors.push("Flash Point field max length is 8");
        }

        if (!AppTool.IsNullOrEmpty(this.DangerousIMDGCode) && this.DangerousIMDGCode.length > 4) {
            errors.push("IMDG Code field max length is 4");
        }

        if (!AppTool.IsNullOrEmpty(this.DangerousUnNumber) && this.DangerousUnNumber.length > 4) {
            errors.push("Un Number field max length is 4");
        }

        if (!AppTool.IsNullOrEmpty(this.DangerousClassNumber) && this.DangerousClassNumber.length > 10) {
            errors.push("Class Number field max length is 10");
        }

        if (!AppTool.IsNullOrEmpty(this.DangerousPackagingGroup) && this.DangerousPackagingGroup.length > 10) {
            errors.push("Packaging Group field max length is 10");
        }

        if (!AppTool.IsNullOrEmpty(this.DangerousMaterialDescription) && this.DangerousMaterialDescription.length > 100) {
            errors.push("Material Description field max length is 100");
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindow();
        }
    }


    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('IsDangerous');
        this.myCloner.AddField('DangerousClassNumber');
        this.myCloner.AddField('DangerousUnNumber');
        this.myCloner.AddField('DangerousPackagingGroup');
        this.myCloner.AddField('DangerousIMDGCode');
        this.myCloner.AddField('DangerousFlashPoint');
        this.myCloner.AddField('DangerousMaterialDescription');
        this.myCloner.AddField('EmergencyContactId');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
