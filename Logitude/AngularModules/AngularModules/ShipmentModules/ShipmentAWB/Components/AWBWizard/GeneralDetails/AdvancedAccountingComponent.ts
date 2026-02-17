import {Component} from '@angular/core';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../../Infrastructure/Tools';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AdvancedAccountingComponent.html',
})

export class AdvancedAccountingComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext = this;
    public LabelColumnWidth: number = 160;
    public ControlColumnWidth: number = 100;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(windowArgs: ShipmentPM) {
        this.EntityPM = windowArgs;
        this.ObjectTableName = this.EntityPM.ShipmentLevelCode == "C" ? "Master" : "Shipment";
        this.Clone();
        this.SetUIProperties();
    }

    // Copy | Reset
    private myAccountingInformation1: string;
    private myAccountingInformation2: string;
    private myAccountingInformation3: string;
    private myAccountingInformation4: string;
    private myAccountingInformation5: string;
    private myAccountingInformation6: string;
    private myAccountingInformationIdentifierCode1: string;
    private myAccountingInformationIdentifierCode2: string;
    private myAccountingInformationIdentifierCode3: string;
    private myAccountingInformationIdentifierCode4: string;
    private myAccountingInformationIdentifierCode5: string;
    private myAccountingInformationIdentifierCode6: string;
    private CopyProperties() {
        this.myAccountingInformation1 = this.AccountingInformation1;
        this.myAccountingInformation2 = this.AccountingInformation2;
        this.myAccountingInformation3 = this.AccountingInformation3;
        this.myAccountingInformation4 = this.AccountingInformation4;
        this.myAccountingInformation5 = this.AccountingInformation5;
        this.myAccountingInformation6 = this.AccountingInformation6;
        this.myAccountingInformationIdentifierCode1 = this.AccountingInformationIdentifierCode1;
        this.myAccountingInformationIdentifierCode2 = this.AccountingInformationIdentifierCode2;
        this.myAccountingInformationIdentifierCode3 = this.AccountingInformationIdentifierCode3;
        this.myAccountingInformationIdentifierCode4 = this.AccountingInformationIdentifierCode4;
        this.myAccountingInformationIdentifierCode5 = this.AccountingInformationIdentifierCode5;
        this.myAccountingInformationIdentifierCode6 = this.AccountingInformationIdentifierCode6;
    }
    private ResetProperties() {
        this.AccountingInformation1 = this.myAccountingInformation1;
        this.AccountingInformation2 = this.myAccountingInformation2;
        this.AccountingInformation3 = this.myAccountingInformation3;
        this.AccountingInformation4 = this.myAccountingInformation4;
        this.AccountingInformation5 = this.myAccountingInformation5;
        this.AccountingInformation6 = this.myAccountingInformation6;
        this.AccountingInformationIdentifierCode1 = this.myAccountingInformationIdentifierCode1;
        this.AccountingInformationIdentifierCode2 = this.myAccountingInformationIdentifierCode2;
        this.AccountingInformationIdentifierCode3 = this.myAccountingInformationIdentifierCode3;
        this.AccountingInformationIdentifierCode4 = this.myAccountingInformationIdentifierCode4;
        this.AccountingInformationIdentifierCode5 = this.myAccountingInformationIdentifierCode5;
        this.AccountingInformationIdentifierCode6 = this.myAccountingInformationIdentifierCode6;
    }

    // SetUIProperties
    public IsEditingEnabled: boolean = false;
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode1", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode2", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode3", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode4", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode5", this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled("AccountingInformationIdentifierCode6", this.ObjectTableName, this.IsEditingEnabled);

        this.SetUIProperties_Field1();
        this.SetUIProperties_Field2();
        this.SetUIProperties_Field3();
        this.SetUIProperties_Field4();
        this.SetUIProperties_Field5();
        this.SetUIProperties_Field6();
    }
    private SetUIProperties_Field1() {
        var isFieldEmpty = AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode1);

        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation1", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation1", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("AccountingInformation1", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation1", this.ObjectTableName, AppTool.IsNullOrEmpty(this.AccountingInformation1) ? true : false);
        }
    }
    private SetUIProperties_Field2() {
        var isFieldEmpty = AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode2);

        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation2", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation2", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("AccountingInformation2", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation2", this.ObjectTableName, AppTool.IsNullOrEmpty(this.AccountingInformation2) ? true : false);
        }
    }
    private SetUIProperties_Field3() {
        var isFieldEmpty = AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode3);

        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation3", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation3", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("AccountingInformation3", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation3", this.ObjectTableName, AppTool.IsNullOrEmpty(this.AccountingInformation3) ? true : false);
        }
    }
    private SetUIProperties_Field4() {
        var isFieldEmpty = AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode4);

        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation4", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation4", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("AccountingInformation4", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation4", this.ObjectTableName, AppTool.IsNullOrEmpty(this.AccountingInformation4) ? true : false);
        }
    }
    private SetUIProperties_Field5() {
        var isFieldEmpty = AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode5);

        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation5", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation5", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("AccountingInformation5", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation5", this.ObjectTableName, AppTool.IsNullOrEmpty(this.AccountingInformation5) ? true : false);
        }
    }
    private SetUIProperties_Field6() {
        var isFieldEmpty = AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode6);

        if (isFieldEmpty) {
            this.UIProperties.SetEnabled("AccountingInformation6", this.ObjectTableName, false);
            this.UIProperties.SetRequired("AccountingInformation6", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("AccountingInformation6", this.ObjectTableName, this.IsEditingEnabled);
            this.UIProperties.SetRequired("AccountingInformation6", this.ObjectTableName, AppTool.IsNullOrEmpty(this.AccountingInformation6) ? true : false);
        }
    }

    // Properties
    get AccountingInformationIdentifierCode1() { return this.EntityPM.AccountingInformationIdentifierCode1; }
    set AccountingInformationIdentifierCode1(newValue: string) {
        if (this.EntityPM.AccountingInformationIdentifierCode1 != newValue) {
            this.EntityPM.AccountingInformationIdentifierCode1 = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.AccountingInformation1 = null;
            }

            this.SetUIProperties_Field1();
        }
    }

    get AccountingInformationIdentifierCode2() { return this.EntityPM.AccountingInformationIdentifierCode2; }
    set AccountingInformationIdentifierCode2(newValue: string) {
        if (this.EntityPM.AccountingInformationIdentifierCode2 != newValue) {
            this.EntityPM.AccountingInformationIdentifierCode2 = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.AccountingInformation2 = null;
            }

            this.SetUIProperties_Field2();
        }
    }

    get AccountingInformationIdentifierCode3() { return this.EntityPM.AccountingInformationIdentifierCode3; }
    set AccountingInformationIdentifierCode3(newValue: string) {
        if (this.EntityPM.AccountingInformationIdentifierCode3 != newValue) {
            this.EntityPM.AccountingInformationIdentifierCode3 = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.AccountingInformation3 = null;
            }

            this.SetUIProperties_Field3();
        }
    }

    get AccountingInformationIdentifierCode4() { return this.EntityPM.AccountingInformationIdentifierCode4; }
    set AccountingInformationIdentifierCode4(newValue: string) {
        if (this.EntityPM.AccountingInformationIdentifierCode4 != newValue) {
            this.EntityPM.AccountingInformationIdentifierCode4 = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.AccountingInformation4 = null;
            }

            this.SetUIProperties_Field4();
        }
    }

    get AccountingInformationIdentifierCode5() { return this.EntityPM.AccountingInformationIdentifierCode5; }
    set AccountingInformationIdentifierCode5(newValue: string) {
        if (this.EntityPM.AccountingInformationIdentifierCode5 != newValue) {
            this.EntityPM.AccountingInformationIdentifierCode5 = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.AccountingInformation5 = null;
            }

            this.SetUIProperties_Field5();
        }
    }

    get AccountingInformationIdentifierCode6() { return this.EntityPM.AccountingInformationIdentifierCode6; }
    set AccountingInformationIdentifierCode6(newValue: string) {
        if (this.EntityPM.AccountingInformationIdentifierCode6 != newValue) {
            this.EntityPM.AccountingInformationIdentifierCode6 = newValue;

            if (AppTool.IsNullOrEmpty(newValue)) {
                this.AccountingInformation6 = null;
            }

            this.SetUIProperties_Field6();
        }
    }

    get AccountingInformation1() { return this.EntityPM.AccountingInformation1; }
    set AccountingInformation1(newValue: string) {
        if (this.EntityPM.AccountingInformation1 != newValue) {
            this.EntityPM.AccountingInformation1 = newValue;
            this.SetUIProperties_Field1();
        }
    }

    get AccountingInformation2() { return this.EntityPM.AccountingInformation2; }
    set AccountingInformation2(newValue: string) {
        if (this.EntityPM.AccountingInformation2 != newValue) {
            this.EntityPM.AccountingInformation2 = newValue;
            this.SetUIProperties_Field2();
        }
    }

    get AccountingInformation3() { return this.EntityPM.AccountingInformation3; }
    set AccountingInformation3(newValue: string) {
        if (this.EntityPM.AccountingInformation3 != newValue) {
            this.EntityPM.AccountingInformation3 = newValue;
            this.SetUIProperties_Field3();
        }
    }

    get AccountingInformation4() { return this.EntityPM.AccountingInformation4; }
    set AccountingInformation4(newValue: string) {
        if (this.EntityPM.AccountingInformation4 != newValue) {
            this.EntityPM.AccountingInformation4 = newValue;
            this.SetUIProperties_Field4();
        }
    }

    get AccountingInformation5() { return this.EntityPM.AccountingInformation5; }
    set AccountingInformation5(newValue: string) {
        if (this.EntityPM.AccountingInformation5 != newValue) {
            this.EntityPM.AccountingInformation5 = newValue;
            this.SetUIProperties_Field5();
        }
    }

    get AccountingInformation6() { return this.EntityPM.AccountingInformation6; }
    set AccountingInformation6(newValue: string) {
        if (this.EntityPM.AccountingInformation6 != newValue) {
            this.EntityPM.AccountingInformation6 = newValue;
            this.SetUIProperties_Field6();
        }
    }

    // Commands
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

        this.ValidationErrorsList = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var minMaxMessage = TextCodeTranslator.Translate("General.M.MinMax");
        minMaxMessage = minMaxMessage.replace("%Minlength", "0");

        if (!AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode1) && AppTool.IsNullOrEmpty(this.AccountingInformation1)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation1")));
        }

        if (!AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode2) && AppTool.IsNullOrEmpty(this.AccountingInformation2)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation2")));
        }

        if (!AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode3) && AppTool.IsNullOrEmpty(this.AccountingInformation3)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation3")));
        }

        if (!AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode4) && AppTool.IsNullOrEmpty(this.AccountingInformation4)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation4")));
        }

        if (!AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode5) && AppTool.IsNullOrEmpty(this.AccountingInformation5)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation5")));
        }

        if (!AppTool.IsNullOrEmpty(this.AccountingInformationIdentifierCode6) && AppTool.IsNullOrEmpty(this.AccountingInformation6)) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation6")));
        }

        if (this.AccountingInformation1 != null && this.AccountingInformation1.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation1")));
        }

        if (this.AccountingInformation2 != null && this.AccountingInformation2.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation2")));
        }

        if (this.AccountingInformation3 != null && this.AccountingInformation3.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation3")));
        }

        if (this.AccountingInformation4 != null && this.AccountingInformation4.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation4")));
        }

        if (this.AccountingInformation5 != null && this.AccountingInformation5.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation5")));
        }

        if (this.AccountingInformation6 != null && this.AccountingInformation6.length > 34) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "34");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AccountingInformation6")));
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('AccountingInformation1');
        this.myCloner.AddField('AccountingInformation2');
        this.myCloner.AddField('AccountingInformation3');
        this.myCloner.AddField('AccountingInformation4');
        this.myCloner.AddField('AccountingInformation5');
        this.myCloner.AddField('AccountingInformation6');
        this.myCloner.AddField('AccountingInformationIdentifierCode1');
        this.myCloner.AddField('AccountingInformationIdentifierCode2');
        this.myCloner.AddField('AccountingInformationIdentifierCode3');
        this.myCloner.AddField('AccountingInformationIdentifierCode4');
        this.myCloner.AddField('AccountingInformationIdentifierCode5');
        this.myCloner.AddField('AccountingInformationIdentifierCode6');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
