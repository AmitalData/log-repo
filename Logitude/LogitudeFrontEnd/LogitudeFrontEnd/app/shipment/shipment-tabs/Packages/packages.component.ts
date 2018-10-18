import {Component, OnInit}  from 'angular2/core';
import {RouteParams, Router} from 'angular2/router';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {LogitudeUtilities} from "../../../infrastructure/utilities/LogitudeUtilities";
import {TextCodeTranslator} from '../../../infrastructure/utilities/TextCodeTranslator'
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';
import {IconButton} from '../../../ApplicationControls/IconButton'
import { wjNg2Input, wjNg2Grid, wjNg2Core } from '../../../../wijmo/wijmo.angular2/wijmo.angular2.all';

class ShipmentPackageItem {
    public EntityPM: any;
    public Pieces: number;
    public Volume: number;
    public GrossWeight: number;
    public VolumetricWeight: number;
    public Length: number;
    public Width: number;
    public Height: number;
    public Dimensions: string;

    constructor(entity: any) {

        this.EntityPM = entity;
        this.Pieces = entity.Quantity == null ? 0 : entity.Quantity;
        this.Volume = entity.Volume == null ? 0 : entity.Volume;
        this.VolumetricWeight = entity.VolumetricWeight == null ? 0 : entity.VolumetricWeight;
        this.GrossWeight = entity.Weight == null ? 0 : entity.Weight;
        this.Length = entity.Length == null ? 0 : entity.Length;
        this.Width = entity.Width == null ? 0 : entity.Width;
        this.Height = entity.Height == null ? 0 : entity.Height;

        if (entity.Length != null && entity.Width != null && entity.Height != null) {
        this.Dimensions = entity.Length + "-" + entity.Width + "-" + entity.Height;
    }
}
}

@Component({
    templateUrl: 'Views/Shipment/Tabs/PackagesTab.html',
    pipes: [TextcodeTranslationPipe],
    directives: [IconButton, wjNg2Core.WjTooltip, wjNg2Grid.WjFlexGrid],
})

export class PackagesComponent implements OnInit {

    private _entityId: string;
    public EntityPM: any;
    public IsDataLoaded: boolean = false;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public TransportModeId: string;  
    public ItemsSource: ShipmentPackageItem[];
    public data: wijmo.collections.CollectionView;
    constructor(private _router: Router, routeParams: RouteParams, private _shipmentsService: ShipmentsService) {
        this._entityId = routeParams.get('id');

       
    }
    
    public Volume: number;
    public GrossWeight: number;    
    public ChargeableWeight: number;
    public VolumetricWeight: number;
    public NumberOfPackages: number;
    public VolumeLabel: string;
    public GrossWeightLabel: string;
    public ChargeableWeightLabel: string;
    public VolumetricWeightLabel: string;
    public IsDangerous: boolean;

    BuildItemsSource() {

        if (this.ItemsSource == null) {
            this.ItemsSource = new Array<ShipmentPackageItem>();
        }

        else {
            this.ItemsSource = [];
        }

        this.EntityPM.ShipmentPackages.forEach((item) => {
            this.ItemsSource.push(new ShipmentPackageItem(item));
        })      
        this.data = new wijmo.collections.CollectionView(this.ItemsSource);
        
        console.log(this.data);
            
    }

    public QuantityLabel: string;

    BuildSummaryData() {

        this.VolumeLabel = TextCodeTranslator.Translate("Shipment.F.Volume").replace('%VolumeCode', this.EntityPM.VolumeUnitCode);
        this.GrossWeightLabel = TextCodeTranslator.Translate("Shipment.F.GrossWeight").replace('%GrossWeightCode', this.EntityPM.GrossWeightUnitCode);
        this.VolumetricWeightLabel = TextCodeTranslator.Translate("Shipment.F.VolumetricWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);

        if (this.TransportModeId == "A") {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.ChargeableWeight").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
        }

        else {
            this.ChargeableWeightLabel = TextCodeTranslator.Translate("Shipment.F.WtMsr.Short").replace('%ChargWeightCode', this.EntityPM.ChargeableWeightUnitCode);
        }

        this.Volume = this.EntityPM.Volume == null ? 0 : this.EntityPM.Volume;
        this.GrossWeight = this.EntityPM.GrossWeight == null ? 0 : this.EntityPM.GrossWeight;
        this.VolumetricWeight = this.EntityPM.VolumetricWeight == null ? 0 : this.EntityPM.VolumetricWeight;
        this.ChargeableWeight = this.EntityPM.ChargeableWeight == null ? 0 : this.EntityPM.ChargeableWeight;        
        this.IsDangerous = this.EntityPM.IsDangerous;

        if (this.IsFCLEntity) {
            this.QuantityLabel = TextCodeTranslator.Translate("Shipment.F.NumberOfContainers");
            this.NumberOfPackages = this.EntityPM.NumberOfContainers == null ? 0 : this.EntityPM.NumberOfContainers;
        }

        else {
            this.QuantityLabel = TextCodeTranslator.Translate("Shipment.F.NumberOfPackages");
            this.NumberOfPackages = this.EntityPM.NumberOfPackages == null ? 0 : this.EntityPM.NumberOfPackages;
        }
    }

    ngOnInit() {

        this.EntityPM = this._shipmentsService.getSingleShipmentByIdFromArray(this._entityId, 1);

        if (this.EntityPM != null) {
        
            this.IsDataLoaded = true;
            this.IsLCLEntity = LogitudeUtilities.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = !this.IsLCLEntity;
            this.TransportModeId = this.EntityPM.TransportModeId;

            this.BuildItemsSource();
            this.BuildSummaryData();            
        }
    }

    Edit(myItem: ShipmentPackageItem) {
        console.log(myItem);
    }
}
