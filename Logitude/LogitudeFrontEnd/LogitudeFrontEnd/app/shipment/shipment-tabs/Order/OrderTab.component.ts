import {Component, OnInit}  from 'angular2/core';
import {RouteParams, Router} from 'angular2/router';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {LogitudeUtilities} from "../../../infrastructure/utilities/LogitudeUtilities";
import {TextCodeTranslator} from '../../../infrastructure/utilities/TextCodeTranslator'
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';
import {IconButton} from '../../../ApplicationControls/IconButton'

class ShipmentOrderPackageItem {
    public EntityPM: any;
    public Quantity: number;
    public Volume: number;
    public GrossWeight: number;
    public VolumetricWeight: number;
    public Length: number;
    public Width: number;
    public Height: number;
    public Dimensions: string;
    public IsContainer: boolean;
    public PackageTypeName: string;

    constructor(entity: any) {

        this.EntityPM = entity;
        this.Quantity = entity.Quantity == null ? 0 : entity.Quantity;
        this.Volume = entity.Volume == null ? 0 : entity.Volume;
        this.VolumetricWeight = entity.VolumetricWeight == null ? 0 : entity.VolumetricWeight;
        this.GrossWeight = entity.Weight == null ? 0 : entity.Weight;
        this.Length = entity.Length;
        this.Width = entity.Width;
        this.Height = entity.Height;
        this.Dimensions = entity.Length + "-" + entity.Width + "-" + entity.Height;
        this.IsContainer = entity.IsContainer;
        this.PackageTypeName = entity.PackageTypeName;
    }
}

@Component({
    templateUrl: 'Views/Shipment/Tabs/OrderTab.html',
    pipes: [TextcodeTranslationPipe],
    directives: [IconButton],
})

export class OrderComponent implements OnInit {

    private _entityId: string;
    public EntityPM: any;
    public IsDataLoaded: boolean = false;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false; 
    public TabLabel: string;
    public AddButtonLabel: string;
    public IsTipsOpened: boolean = false;
    public ItemsSource: ShipmentOrderPackageItem[];
    public VolumeUnitCode: string;    
    public DimensionsUnitCode: string;
    public GrossWeightUnitCode: string;
    public VolumeHeader: string;
    public DimensionsHeader: string;
    public GrossWeightHeader: string;

    constructor(private _router: Router, routeParams: RouteParams, private _shipmentsService: ShipmentsService) {
        this._entityId = routeParams.get('id');
    }

    SetLabels() {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            this.TabLabel = TextCodeTranslator.Translate("Shipment.S.Orders.Booking");
        }

        else {
            this.TabLabel = TextCodeTranslator.Translate("Shipment.S.Orders.Order");
        }

        if (this.IsLCLEntity) {
            this.AddButtonLabel = TextCodeTranslator.Translate("Shipment.B.Order.AddOrderPackage");
        }

        else {
            this.AddButtonLabel = "Add Order Container";
        }

        this.VolumeUnitCode = this.EntityPM.VolumeUnitCode;        
        this.DimensionsUnitCode = this.EntityPM.DimensionsUnitCode;
        this.GrossWeightUnitCode = this.EntityPM.GrossWeightUnitCode;

        this.VolumeHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Volume").replace('%UnitCode', this.VolumeUnitCode);
        this.DimensionsHeader = TextCodeTranslator.Translate("Shipment.O.Packages.Dimensions").replace('%UnitCode', this.DimensionsUnitCode);
        this.GrossWeightHeader = TextCodeTranslator.Translate("Shipment.O.Packages.GrossWeight").replace('%UnitCode', this.GrossWeightUnitCode);        
    }

    BuildOrderPackages() {

        if (this.ItemsSource == null) {
            this.ItemsSource = new Array<ShipmentOrderPackageItem>();
        }

        else {
            this.ItemsSource = [];
        }

        this.EntityPM.ShipmentOrderPackages.forEach((item) => {
            this.ItemsSource.push(new ShipmentOrderPackageItem(item));
        })
    }

    ngOnInit() {
        this.EntityPM = this._shipmentsService.getSingleShipmentByIdFromArray(this._entityId, 1);

        if (this.EntityPM != null && this.EntityPM !== undefined) {

            this.IsDataLoaded = true;
            this.IsLCLEntity = LogitudeUtilities.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = !this.IsLCLEntity;

            this.SetLabels();
            this.BuildOrderPackages();
        }
    }

    onTipsButtonClicked() {
        this.IsTipsOpened = !this.IsTipsOpened;
    }
}
