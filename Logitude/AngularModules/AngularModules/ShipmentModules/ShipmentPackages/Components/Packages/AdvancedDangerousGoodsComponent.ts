import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPackagePM} from '../../../../Shipment/EntityPMs/ShipmentPackagePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';

@Component({
    moduleId: module.id,
    templateUrl: './AdvancedDangerousGoodsComponent.html',
})

export class AdvancedDangerousGoodsComponent extends BaseComponent {
    public EntityPM: ShipmentPackagePM;
    public ObjectTableName: string;
    public DataContext: AdvancedDangerousGoodsComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: ShipmentPackagePM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "ShipmentPackage";
        this.SetUIProperties();
        this.Clone();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("CeficClass", this.ObjectTableName, this.EntityPM.IsDangerous);
        this.UIProperties.SetEnabled("KelmerCode", this.ObjectTableName, this.EntityPM.IsDangerous);
        this.UIProperties.SetEnabled("EMS", this.ObjectTableName, this.EntityPM.IsDangerous);
        this.UIProperties.SetEnabled("ProperShippingName", this.ObjectTableName, this.EntityPM.IsDangerous);
        this.UIProperties.SetEnabled("MarinePollutant", this.ObjectTableName, this.EntityPM.IsDangerous);
    }
    
    get CeficClass() { return this.EntityPM.CeficClass; }
    set CeficClass(newValue: string) {
        if (this.EntityPM.CeficClass != newValue) {
            this.EntityPM.CeficClass = newValue;
        }
    }

    get KelmerCode() { return this.EntityPM.KelmerCode; }
    set KelmerCode(newValue: string) {
        if (this.EntityPM.KelmerCode != newValue) {
            this.EntityPM.KelmerCode = newValue;
        }
    }

    get EMS() { return this.EntityPM.EMS; }
    set EMS(newValue: string) {
        if (this.EntityPM.EMS != newValue) {
            this.EntityPM.EMS = newValue;
        }
    }

    get ProperShippingName() { return this.EntityPM.ProperShippingName; }
    set ProperShippingName(newValue: string) {
        if (this.EntityPM.ProperShippingName != newValue) {
            this.EntityPM.ProperShippingName = newValue;
        }
    }

    get MarinePollutant() { return this.EntityPM.MarinePollutant; }
    set MarinePollutant(newValue: boolean) {
        if (this.EntityPM.MarinePollutant != newValue) {
            this.EntityPM.MarinePollutant = newValue;
        }
    }
    
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('CeficClass');
        this.myCloner.AddField('KelmerCode');
        this.myCloner.AddField('EMS');
        this.myCloner.AddField('ProperShippingName');
        this.myCloner.AddField('MarinePollutant');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
