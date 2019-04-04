import {Component} from '@angular/core';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {RoutingsTabComponent} from './RoutingsTabComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentTool, RoutingHelper} from '../../../../Shipment/Tools';
import {PortList} from '../../../../Common/EntityLists/PortList';
import {PortListService} from '../../../../Common/Services/StandardLists/PortListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditHouseRoutingComponent.html',
})

export class AddEditHouseRoutingComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext = this;
    public TransportModeId: string = null;
    public ValidationErrorsList: string[] = [];
    public FatherComponent: RoutingsTabComponent;
    private myPortListService: PortListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.myPortListService = new PortListService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.FatherComponent = args['FatherComponent'];
        this.TransportModeId = this.EntityPM.TransportModeId;
        this.SetLabels();
        this.SetUIProperties();
        this.Clone();
    }

    public FromTextCodeLabel: string = null;
    public ToTextCodeLabel: string = null;
    SetLabels() {
        switch (this.TransportModeId) {
            case "A": {
                this.FromTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.Gateway");
                this.ToTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.Destination");
                break;
            }

            case "O": {
                this.FromTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.LoadingPort");
                this.ToTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.DischargePort");
                break;
            }

            case "I": {
                this.FromTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.From");
                this.ToTextCodeLabel = TextCodeTranslator.Translate("Shipment.O.Routings.To");
                break;
            }
        }
    }

    public IsEditingEnabled: boolean = false;
    SetUIProperties() {
        var isEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled("MainCarriageFromPortId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled("MainCarriageToPortId", this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetRequired("MainCarriageFromPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MainCarriageFromPortId) ? true : false);
        this.UIProperties.SetRequired("MainCarriageToPortId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.MainCarriageToPortId) ? true : false);
        this.IsEditingEnabled = isEditingEnabled;
    }

    get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    set MainCarriageFromPortId(value: string) {
        if (this.EntityPM.MainCarriageFromPortId != value) {
            this.EntityPM.MainCarriageFromPortId = value;
            this.EntityPM.FromPortId = value;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        RoutingHelper.MainCarriageFromPortChanged(this.EntityPM, list);
                    }
                });
            }
        }
    }

    get MainCarriageToPortId() { return this.EntityPM.MainCarriageToPortId; }
    set MainCarriageToPortId(value: string) {
        if (this.EntityPM.MainCarriageToPortId != value) {
            this.EntityPM.MainCarriageToPortId = value;
            this.EntityPM.ToPortId = value;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                RoutingHelper.FinalDestinationPortChanged(this.EntityPM, null);
            }

            else {
                this.myPortListService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: PortList = myResponse.Result;
                        RoutingHelper.FinalDestinationPortChanged(this.EntityPM, list);
                    }
                });
            }
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (AppTool.IsNullOrEmpty(this.MainCarriageFromPortId)) {
            errors.push(msg.replace("%FieldName", this.FromTextCodeLabel));
        }

        if (AppTool.IsNullOrEmpty(this.MainCarriageToPortId)) {
            errors.push(msg.replace("%FieldName", this.ToTextCodeLabel));
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.FatherComponent.BuildItemsCollection();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this);
        this.myCloner.AddField('FromPortId');
        this.myCloner.AddField('ToPortId');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
