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
    templateUrl: './AdvancedCommentsComponent.html',
})

export class AdvancedCommentsComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string = "Shipment";
    public DataContext = this;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(windowArgs: ShipmentPM) {
        this.EntityPM = windowArgs;
        this.Clone();
        this.SetUIProperties();
    }

    public IsEditingEnabled: boolean = false;
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
    }

    get AWBComments() { return this.EntityPM.AWBComments; }
    set AWBComments(value: string) {
        if (this.EntityPM.AWBComments != value) {
            this.EntityPM.AWBComments = value;            
        }
    }

    get AWBPrintingComments() { return this.EntityPM.AWBPrintingComments; }
    set AWBPrintingComments(value: string) {
        if (this.EntityPM.AWBPrintingComments != value) {
            this.EntityPM.AWBPrintingComments = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

        this.ValidationErrorsList = [];

        var minMaxMessage = TextCodeTranslator.Translate("General.M.MinMax");
        minMaxMessage = minMaxMessage.replace("%Minlength", "0");

        if (this.AWBComments != null && this.AWBComments.length > 195) {
            minMaxMessage = minMaxMessage.replace("%Maxlength", "195");
            this.ValidationErrorsList.push(minMaxMessage.replace("%FieldName", TextCodeTranslator.Translate("Shipment.F.AWBComments.Short")));
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('AWBComments');
        this.myCloner.AddField('AWBPrintingComments');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
