import {Component} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentPackagePM} from '../../EntityPMs/ShipmentPackagePM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SplitShipmentService, SplitShipmentHelper, SplitPackage} from '../../Services/SplitShipmentService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './SplitShipmentComponent.html',
})

export class SplitShipmentComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string = "Shipment";
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;
    public DataSource: SplitShipmentItem[] = [];
    public ShipmentPackages: SplitShipmentItem[] = [];
    public NewShipmentPackages: SplitShipmentItem[] = [];
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.IsLCLEntity = AppTool.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.IsFCLEntity = AppTool.IsFCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
        this.SetLabels();

        this.EntityPM.ShipmentPackages.forEach(item => {
            this.DataSource.push(new SplitShipmentItem(item, this));
        });

        this.BuildItemsSource();
    }

    PackageTypeLabel:string = null;
    SetLabels() {
        this.PackageTypeLabel = this.IsLCLEntity ? "Package Type" : "Container Type";
    }
    BuildItemsSource() {
        this.ShipmentPackages = [];
        this.NewShipmentPackages = [];

        this.DataSource.forEach((item: SplitShipmentItem) => {
            if (item.IsSplit || item.IsPartialSplit) {
                this.NewShipmentPackages.push(item);
            }

            else {
                this.ShipmentPackages.push(item);
            }
        });
    }

    CancelButtonClicked() {

        // Clone All Packages
        //this.EntityPM.ShipmentPackages.forEach(item => {
        //    item.IsDirty = false;
        //});

        //this.EntityPM.IsDirty = false;

        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        if (this.NewShipmentPackages.length == 0) {
            var messageWindow = new MessageWindow();
            messageWindow.Show("Please Choose Packages");
        }

        else {
            this.CurrentSession.StartBusyIndicatorSaving();

            var helper = new SplitShipmentHelper();
            helper.OldShipmentId = this.EntityPM.Id;
            helper.Shipment = this.EntityPM;
            helper.SplitPackages = [];

            this.NewShipmentPackages.forEach((item: SplitShipmentItem) => {
                var newItem = new SplitPackage();
                newItem.Id = item.Id;
                newItem.IsSplit = item.IsSplit;
                newItem.IsPartialSplit = item.IsPartialSplit;
                newItem.ParentId = item.PartialSplitParentId;
                newItem.Quantity = item.Quantity;
                newItem.Weight = item.Weight;
                newItem.Volume = item.Volume;
                helper.SplitPackages.push(newItem);
            });

            var myService = new SplitShipmentService();

            myService.Split(helper).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    var updatedHelper: SplitShipmentHelper = myResponse.Result;

                    this.CurrentSession.CloseCurrentWindowEmit("Ok");

                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: updatedHelper.NewShipmentId, ObjectTableName: 'Shipment', BackButtonLabel: this.ObjectTableName + ": " + this.EntityPM.ShipmentNumber });                            
                        });
                }
            });
        }
    }
}
export class SplitShipmentItem {
    public EntityPM: ShipmentPackagePM;
    public IsSplit: boolean = false;
    public IsPartialSplit: boolean = false;
    public IsPartialSplitParent: boolean = false;
    public PartialSplitParentId: string = null;
    constructor(itemPM: ShipmentPackagePM, public fatherComponent: SplitShipmentComponent) {
        this.EntityPM = itemPM;
        this.Quantity = this.EntityPM.Quantity;
        this.Volume = this.EntityPM.Volume;
        this.Weight = this.EntityPM.Weight;
        this.GetImageSource();

        this.EntityPM.CommodityId
    }

    get Id() { return this.EntityPM.Id; }
    get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    get ContainerNumber() { return this.EntityPM.ContainerNumber; }
    get Description() { return this.EntityPM.Description; }
    get Commodity() {

        var myResult = "";

        if (this.EntityPM.CommodityNumber) {
            myResult = this.EntityPM.CommodityNumber;

            if (this.EntityPM.CommodityName) {
                myResult += "," + this.EntityPM.CommodityName;
            }
        }

        return myResult;
    }

    private _quantity: number = null;
    public get Quantity() { return this._quantity; }
    public set Quantity(value: number) {
        if (this._quantity != value) {
            this._quantity = value;
        }
    }

    private _volume: number = null;
    public get Volume() { return this._volume; }
    public set Volume(value: number) {
        if (this._volume != value) {
            this._volume = value;
        }
    }

    private _weight: number = null;
    public get Weight() { return this._weight; }
    public set Weight(value: number) {
        if (this._weight != value) {
            this._weight = value;
        }
    }

    public PackageTypeImage: string = null;
    GetImageSource() {
        if (this.EntityPM.IsContainer) {
            this.PackageTypeImage = "./Images/CellIcons/Container.png";
        }

        else {
            this.PackageTypeImage = "./Images/CellIcons/Package.png";
        }
    }

    SplitClicked() {
        this.IsSplit = true;
        this.fatherComponent.BuildItemsSource();
    }
    CancelClicked() {
        this.IsSplit = false;

        if (this.IsPartialSplit) {

            var index = this.fatherComponent.DataSource.indexOf(this);
            if (index > -1) {
                this.fatherComponent.DataSource.splice(index, 1);
            }

            var ParentDataItem: SplitShipmentItem = this.fatherComponent.DataSource.filter(f => f.Id == this.PartialSplitParentId)[0];
            if (ParentDataItem) {

                if (ParentDataItem.Quantity && this.Quantity) {
                    ParentDataItem.Quantity += this.Quantity;
                }

                if (ParentDataItem.Volume && this.Volume) {
                    ParentDataItem.Volume += this.Volume;
                }

                if (ParentDataItem.Weight && this.Weight) {
                    ParentDataItem.Weight += this.Weight;
                }

                ParentDataItem.IsPartialSplitParent = this.fatherComponent.DataSource.filter(f => f.IsPartialSplit && f.PartialSplitParentId == this.PartialSplitParentId).length > 0 ? true : false;
            }
        }

        this.fatherComponent.BuildItemsSource();
    }
    PartialSplitClicked() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Partial Split";
        logWindow.WindowArgs = { Item: this };
        logWindow.Show('./Shipment/Components/SplitShipment/SplitPartialPackageComponent');

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.IsPartialSplitParent = true;

                    if (!AppTool.IsNullOrEmpty(this.Quantity) && !AppTool.IsNullOrEmpty(comp.Quantity)) {
                        this.Quantity = this.Quantity - comp.Quantity;
                    }

                    if (!AppTool.IsNullOrEmpty(this.Volume) && !AppTool.IsNullOrEmpty(comp.Volume)) {
                        this.Volume = this.Volume - comp.Volume;
                    }

                    if (!AppTool.IsNullOrEmpty(this.Weight) && !AppTool.IsNullOrEmpty(comp.Weight)) {
                        this.Weight = this.Weight - comp.Weight;
                    }

                    var newPackagePM: ShipmentPackagePM = new ShipmentPackagePM(null);
                    newPackagePM.PackageTypeId = this.EntityPM.PackageTypeId;
                    newPackagePM.PackageTypeCode = this.EntityPM.PackageTypeCode;
                    newPackagePM.CommodityId = this.EntityPM.CommodityId;
                    newPackagePM.CommodityName = this.EntityPM.CommodityName;
                    newPackagePM.CommodityNumber = this.EntityPM.CommodityNumber;

                    newPackagePM.Quantity = comp.Quantity;
                    newPackagePM.Volume = comp.Volume;
                    newPackagePM.Weight = comp.Weight;

                    var dataSourceItem = new SplitShipmentItem(newPackagePM, this.fatherComponent);
                    dataSourceItem.IsPartialSplit = true;
                    dataSourceItem.PartialSplitParentId = this.EntityPM.Id;

                    this.fatherComponent.DataSource.push(dataSourceItem);
                    this.fatherComponent.BuildItemsSource();
                }
            });
        });

    }
}
