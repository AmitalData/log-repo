import {Component, OnInit}  from 'angular2/core';
import {RouteParams, Router} from 'angular2/router';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';
import {IconButton} from '../../../ApplicationControls/IconButton'

class PartnerItem {
    public EntityPM: any;
    public PartnerType: string;
    public PartnerName: string;
    public IsRemoveButtonVisible: boolean = false;

    constructor(entity: any, type: string) {

        this.EntityPM = entity;
        this.PartnerType = type;
        this.PartnerName = this.GetPartnerName();
        this.SetRemoveButtonVisibility();

        this.CityCell = this.City;

        if (this.State != null) {

            this.CityCell += "," + this.State;
        }

        if (this.ZipCode != null) {

            this.CityCell += "," + this.ZipCode;
        }
    }

    GetPartnerName() {
        switch (this.PartnerType) {
            case "Shipper": { return this.EntityPM.ShipperName; }
            case "Consignee": { return this.EntityPM.ConsigneeName; }
            case "Agent": { return this.EntityPM.AgentName; }
            case "Issuing Carrier's Agent": { return this.EntityPM.IssuingCarrierAgentName; }
            case "Custom Agent Export": { return this.EntityPM.CustomAgentExportName; }
            case "Custom Agent Import": { return this.EntityPM.CustomAgentImportName; }
            case "Notify1": { return this.EntityPM.Notify1Name; }
            case "Notify2": { return this.EntityPM.Notify2Name; }
            case "Shipper Not Exporter": { return this.EntityPM.ShipperNotExporterName; }
            case "Consignee Not Importer": { return this.EntityPM.ConsigneeNotImporterName; }
            case "Freight Forwarder": { return this.EntityPM.FreightForwarderName; }
            case "Coloader": { return this.EntityPM.ColoaderName; }
            case "Custom Clearance": { return this.EntityPM.CustomClearancePointName; }
            case "Consolidator": { return this.EntityPM.ConsolidatorName; }
            default: { return null; }
        }
    }
    
    SetRemoveButtonVisibility() {

        switch (this.PartnerType) {
            case "Shipper": {

                this.IsRemoveButtonVisible = (this.EntityPM.DirectionId == "E") ? false : true;
                break;
            }

            case "Consignee": {
                this.IsRemoveButtonVisible = (this.EntityPM.DirectionId == "I") ? false : true;
            }

            default: {
                this.IsRemoveButtonVisible = true;
                break;
            }
        }
    }

    public Name: string = "Ayman Company";
    public City: string = "Qalqilia";
    public State: string = "GR";
    public Country: string = "State Of Palestine";
    public Address1: string = "Address1";
    public Address2: string = "Address2";
    public ZipCode: string = "1234";
    public Phone: string = "1111111";
    public Fax: string = "22222222";
    public Contact: string = "Ayman@mail.com";
    public FlagSource: string = "images/Flags/AF.png";
    public CityCell: string;
}

@Component({
    templateUrl: 'Views/Shipment/Tabs/PartnersTab.html',
    pipes: [TextcodeTranslationPipe],
    directives: [IconButton],
})

export class PartnersComponent implements OnInit {

    private _entityId: string;
    public EntityPM: any;
    public IsDataLoaded: boolean = false;
    public ItemsCollection: PartnerItem[];

    constructor(private _router: Router, routeParams: RouteParams, private _shipmentsService: ShipmentsService) {
        this._entityId = routeParams.get('id');
    }

    BuildItemsCollection() {

        if (this.ItemsCollection == null) {
            this.ItemsCollection = new Array<PartnerItem>();
        }

        else {
            this.ItemsCollection = [];

            //A.length = 0
            //A.splice(0,A.length)
        }

        if (this.EntityPM.ShipperId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Shipper"));
        }

        if (this.EntityPM.ConsigneeId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Consignee"));
        }

        if (this.EntityPM.AgentId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Agent"));
        }

        if (this.EntityPM.IssuingCarrierAgentId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Issuing Carrier's Agent"));
        }

        if (this.EntityPM.CustomAgentExportId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Custom Agent Export"));
        }

        if (this.EntityPM.CustomAgentImportId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Custom Agent Import"));
        }

        if (this.EntityPM.Notify1Id != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Notify1"));
        }

        if (this.EntityPM.Notify2Id != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Notify2"));
        }

        if (this.EntityPM.ShipperNotExporterId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Shipper Not Exporter"));
        }

        if (this.EntityPM.ConsigneeNotImporterId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Consignee Not Importer"));
        }

        if (this.EntityPM.FreightForwarderId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Freight Forwarder"));
        }

        if (this.EntityPM.ColoaderId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Coloader"));
        }

        if (this.EntityPM.CustomClearancePointId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Custom Clearance"));
        }

        if (this.EntityPM.ConsolidatorId != null) {
            this.ItemsCollection.push(new PartnerItem(this.EntityPM, "Consolidator"));
        }
    }

    ngOnInit() {

        this.EntityPM = this._shipmentsService.getSingleShipmentByIdFromArray(this._entityId, 1);

        if (this.EntityPM != null) {

            this.IsDataLoaded = true;
            this.BuildItemsCollection();
        }
    }
}
