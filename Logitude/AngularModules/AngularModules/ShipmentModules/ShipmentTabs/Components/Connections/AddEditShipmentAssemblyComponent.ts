import {Component} from '@angular/core';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {ShipmentAssemblyPM} from '../../../../Shipment/EntityPMs/ShipmentAssemblyPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditShipmentAssemblyComponent.html',
})

export class AddEditShipmentAssemblyComponent extends BaseComponent {
    public ShipmentPM: ShipmentPM;
    public EntityPM: ShipmentAssemblyPM;
    public ObjectTableName: string;
    public DataContext: AddEditShipmentAssemblyComponent = this;
    public IsNew: boolean;
    public ValidationErrorsList: string[] = [];
    private myCardListService: CardListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.myCardListService = new CardListService();
    }

    SetWindowArgs(windowArgs: any) {
        this.ShipmentPM = windowArgs['ShipmentPM'];
        this.EntityPM = windowArgs['EntityPM'];        
        this.ObjectTableName = "ShipmentAssembly";

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.IsNew = true;
        }

        else {
            this.IsNew = false;
        }

        this.SetLOVDependency();
        this.SetUIProperties();
        this.Clone();
    }

    public PartnerDependencyProperty1: string;
    public PartnerDependencyProperty1IsList: boolean;
    private SetLOVDependency() {
        var myDependency = "CS";
        var myDependencyIsList = false;

        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            myDependency = "CS,AG";
            myDependencyIsList = true;
        }

        this.PartnerDependencyProperty1 = myDependency;
        this.PartnerDependencyProperty1IsList = myDependencyIsList;
    }

    SetUIProperties() {

    }

    get ShipperId() { return this.EntityPM.ShipperId; }
    set ShipperId(value: string) {
        if (this.EntityPM.ShipperId != value) {
            this.EntityPM.ShipperId = value;

            this.GetShipperCard();
        }
    }

    get House() { return this.EntityPM.House; }
    set House(value: string) {
        if (this.EntityPM.House != value) {
            this.EntityPM.House = value;
        }
    }

    private GetShipperCard() {
        if (this.ShipperId == null) {
            this.EntityPM.ShipperName = null;
        }

        else {
            this.myCardListService.getSingle(this.ShipperId).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {

                    var myCard: CardList = myResponse.Result;

                    if (myCard != null) {
                        this.EntityPM.ShipperName = myCard.EnglishName;
                    }

                    else {
                        this.LoadShipperCard();
                    }
                }
            });
        }
    }
    private LoadShipperCard() {
        this.myCardListService.getSingle(this.ShipperId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {

                var myCard: CardList = myResponse.Result;

                if (myCard != null) {
                    this.EntityPM.ShipperName = myCard.EnglishName;
                }
            }
        });
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.ShipperId) && AppTool.IsNullOrEmpty(this.House)) {
            errors.push("Please fill Shipper or House");
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {
                if (this.ShipmentPM.ShipmentAssemblies.indexOf(this.EntityPM) == -1) {
                    this.ShipmentPM.AddAssembly(this.EntityPM);
                }
            }

            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ShipperId');
        this.myCloner.AddField('House');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.ShipmentPM);        
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
