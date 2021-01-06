import {Component} from '@angular/core';
import {ShipmentTool} from '../../Tools';
import {AppTool} from '../../../Infrastructure/Tools';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentPickUpPM} from '../../EntityPMs/ShipmentPickUpPM';
import {ShipmentDeliveryPM} from '../../EntityPMs/ShipmentDeliveryPM';
import {ShipmentDomainService} from '../../Services/ShipmentDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SpotLightDateComponent} from './SpotLightDateComponent';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

@Component({    
    templateUrl: './ShipmentSpotlightComponent.html',
})

export class ShipmentSpotlightComponent {
    public EntityId: string;
    public EntityPM: ShipmentPM;
    public ItemsCollection: LegItem[];
    public IsInlandDomestic: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    Run(entityId: string) {
        this.EntityId = entityId;
        this.LoadShipmentPM();
    }

    private showBusyIndicator: boolean = false;
    get ShowBusyIndicator() { return this.showBusyIndicator; }
    set ShowBusyIndicator(value: boolean) {
        if (this.showBusyIndicator != value) {
            this.showBusyIndicator = value;
            this.CurrentSession.FireEvent("SpotLightDetectChanges");
        }
    }

    private LoadShipmentPM() {

        this.ShowBusyIndicator = true;

        var service = new ShipmentDomainService();

        service.GetSingleShipmentPMWithoutComposition(this.EntityId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.EntityPM = myResponse.Result;

                if (this.EntityPM) {
                    this.BuildItemsCollection();
                }
            }

            this.ShowBusyIndicator = false;
        });
    }
    private BuildItemsCollection() {
        this.ItemsCollection = [];
        this.IsInlandDomestic = ShipmentTool.IsInlandDomestic(this.EntityPM);

        //Pick Ups
        if (this.EntityPM.ShipmentPickUps.length == 0) {
            this.ItemsCollection.push(new LegItem(this, "NOP", "Start"));
        }

        else {
            var pickList: ShipmentPickUpPM[] = this.EntityPM.ShipmentPickUps;
            for (var i = 0; i < pickList.length; i++) {
                var pickIndex: string = (i == 0) ? "Start" : "Between";
                var pickItem: ShipmentPickUpPM = pickList[i];
                this.ItemsCollection.push(new LegItem(this, "Pick Up", pickIndex, pickItem, null));
            }
        }

        if (this.EntityPM.DirectionId == "E" || this.EntityPM.DirectionId == "D" || this.EntityPM.DirectionId == "R") {
            if (this.EntityPM.TransportModeId == "O" || this.EntityPM.TransportModeId == "I") {
                if (this.IsInlandDomestic) {

                }

                else {
                    this.ItemsCollection.push(new LegItem(this, "WarehouseLeg"));
                }
            }
        }

        //Pre Carriage
        if (!AppTool.IsNullOrEmpty(this.EntityPM.PreCarriageFromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.PreCarriageToPortId)) {
            this.ItemsCollection.push(new LegItem(this, "Pre Carriage"));
        }

        //Main Carriage
        this.ItemsCollection.push(new LegItem(this, "Main Carriage"));

        //Transshipment1
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1FromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.Transshipment1ToPortId)) {
            this.ItemsCollection.push(new LegItem(this, "Transshipment1"));
        }

        //Transshipment2
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2FromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.Transshipment2ToPortId)) {
            this.ItemsCollection.push(new LegItem(this, "Transshipment2"));
        }

        //Transshipment3
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3FromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.Transshipment3ToPortId)) {
            this.ItemsCollection.push(new LegItem(this, "Transshipment3"));
        }

        //On Carriage
        if (!AppTool.IsNullOrEmpty(this.EntityPM.OnCarriageFromPortId) && !AppTool.IsNullOrEmpty(this.EntityPM.OnCarriageToPortId)) {
            this.ItemsCollection.push(new LegItem(this, "On Carriage"));
        }

        // Warehouse Leg
        if (this.EntityPM.DirectionId == "I" && (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H")) {
            this.ItemsCollection.push(new LegItem(this, "WarehouseLeg"));
        }

        // Deliveries
        var allDeliveries = this.EntityPM.ShipmentDeliveries.filter(f => f.PickUpDeliveryTypeCode == "DELV");
        if (allDeliveries.length == 0) {
            this.ItemsCollection.push(new LegItem(this, "NOD", "End"));
        }
        
        else {
            for (var i = 0; i < allDeliveries.length; i++) {
                var delvIndex: string = (i == (allDeliveries.length - 1)) ? "End" : "Between";
                var delvItem: ShipmentDeliveryPM = allDeliveries[i];
                this.ItemsCollection.push(new LegItem(this, "Delivery", delvIndex, null, delvItem));
            }
        }
    }

    OpenLeg(entity: LegItem) {

        this.ItemsCollection.forEach((item) => {
            item.SetDefaultImageSource();
        });

        if (entity.IsNoData) {
            entity.IsOpenedLeg = false;
            return;
        }

        switch (entity.LegName) {
            case "Pick Up": {
                if (entity.LegIndex == "Start") {
                    entity.LegImgSrc = "./Images/SpotLightLegs/PicUp.png";
                }
                else {
                    entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                }

                entity.IsOpenedLeg = true;
                break;
            }

            case "Pre Carriage": {
                entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                entity.IsOpenedLeg = true;
                break;
            }

            case "Main Carriage": {
                entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                entity.IsOpenedLeg = true;
                break;
            }

            case "Transshipment1": {
                entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                entity.IsOpenedLeg = true;
                break;
            }

            case "Transshipment2": {
                entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                entity.IsOpenedLeg = true;
                break;
            }

            case "Transshipment3": {
                entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                entity.IsOpenedLeg = true;
                break;
            }

            case "Transshipment3": {
                entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                entity.IsOpenedLeg = true;
                break;
            }

            case "On Carriage": {
                entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                entity.IsOpenedLeg = true;
                break;
            }

            case "WarehouseLeg": {
                entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                entity.IsOpenedLeg = true;
                break;
            }

            case "Delivery": {
                if (entity.LegIndex == "End") {
                    entity.LegImgSrc = "./Images/SpotLightLegs/Delivery.png";
                }
                else {
                    entity.LegImgSrc = "./Images/SpotLightLegs/leg-opened.png";
                }

                entity.IsOpenedLeg = true;
                break;
            }

            default: {
                entity.IsOpenedLeg = false;
            }
        }
    }

    ViewEntityClicked() {
        var myCodes: string[] = [];
        myCodes.push("EAWB");
        myCodes.push("BUBK");

        if (FeatureLocator.IsPackageOneOf(myCodes)) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 600;
            logWindow.Title = ShipmentTool.GetAWBWizardHeader(this.EntityPM.ShipmentLevelCode, this.EntityPM.DirectionId);
            logWindow.WindowArgs = this.EntityId;
            logWindow.Show('./ShipmentModules/ShipmentAWB/Components/AWBWizard/AWBWizardLoadComponent');

            logWindow.WindowClosed.subscribe(($event: any) => {
                //this.BuildItemsCollection();
            });
        }

        else {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.EntityId, ObjectTableName: 'Shipment' });
                });
        }
    }

    OnDateComponentClosed(dateComponent: SpotLightDateComponent, legItem: LegItem) {
        if (dateComponent && legItem) {
            this.EntityPM = dateComponent.EntityPM;
            legItem.EntityPM = dateComponent.EntityPM;
            legItem.pickUp = dateComponent.PickUpPM;
            legItem.delivery = dateComponent.DeliveryPM;
            legItem.SetDateVisibilityProperties();            
        }
    }
}
export class LegItem {
    public LegName: string;
    public LegIndex: string;
    public EntityPM: ShipmentPM;
    public pickUp: ShipmentPickUpPM;
    public delivery: ShipmentDeliveryPM;
    private IsInlandDomestic: boolean;
    public LeftMargin: number = 20;
    public RightMargin: number = 20;
    public NoDataText: string;
    public IsNoData: boolean = false;
    public IsOpenedLeg: boolean = false;
    constructor(public fatherComponent: ShipmentSpotlightComponent, legName: string, legIndex: string = null, pickUp: ShipmentPickUpPM = null, delivery: ShipmentDeliveryPM = null) {
        this.IsInlandDomestic = ShipmentTool.IsInlandDomestic(this.fatherComponent.EntityPM);
        this.pickUp = pickUp;
        this.delivery = delivery;
        this.EntityPM = this.fatherComponent.EntityPM;

        this.LegName = legName;
        this.LegIndex = legIndex;

        if (legIndex == "Start") {
            this.LeftMargin = 5;
        }

        else if (legIndex == "End") {
            this.RightMargin = 5;
        }

        if (legName == "NOP") {
            this.NoDataText = "No pick up";
            this.IsNoData = true;
        }

        else if (legName == "NOD") {
            this.NoDataText = "No delivery";
            this.IsNoData = true;
        }

        else if (legName == "WarehouseLeg") {
            if (AppTool.IsNullOrEmpty(this.EntityPM.WarehouseLegWarehouseId)) {
                this.NoDataText = "No Warehouse";
                this.IsNoData = true;
            }
        }

        this.SetHouseVisibilityProperties();
        this.SetDateVisibilityProperties();
        this.ComputeHeader();
        this.SetDefaultImageSource();
    }

    public IsNoDeptDateVisible: boolean = false;
    public IsActualDeptVisible: boolean = false;
    public IsEstimateDeptVisible: boolean = false;
    public IsNoArrivDateVisible: boolean = false;
    public IsActualArrivVisible: boolean = false;
    public IsEstimateArrivVisible: boolean = false;
    public SetDateVisibilityProperties() {
        var isNoDeptDateVisible: boolean = false;
        var isActualDeptVisible: boolean = false;
        var isEstimateDeptVisible: boolean = false;
        var isNoArrivDateVisible: boolean = false;
        var isActualArrivVisible: boolean = false;
        var isEstimateArrivVisible: boolean = false;

        //Departure
        if (this.DepartedDate_Actual == null) {
            if (this.DepartedDate_Estimate == null) {
                isNoDeptDateVisible = true;
            }

            else {
                isEstimateDeptVisible = true;
            }
        }

        else {
            isActualDeptVisible = true;
        }

        //Arrival
        if (this.ArrivalDate_Actual == null) {
            if (this.ArrivalDate_Estimate == null) {
                isNoArrivDateVisible = true;
            }

            else {
                isEstimateArrivVisible = true;
            }
        }

        else {
            isActualArrivVisible = true;
        }

        this.IsEstimateDeptVisible = isEstimateDeptVisible;
        this.IsActualDeptVisible = isActualDeptVisible;
        this.IsNoDeptDateVisible = isNoDeptDateVisible;
        this.IsNoArrivDateVisible = isNoArrivDateVisible;
        this.IsActualArrivVisible = isActualArrivVisible;
        this.IsEstimateArrivVisible = isEstimateArrivVisible;
    }

    public IsHouseNotConnectedVisibile: boolean = false;
    public IsHouseConnectedVisibile: boolean = false;
    public IsHouseVisible: boolean = true;
    private SetHouseVisibilityProperties() {
        var isHouseNotConnectedVisibile: boolean = false;
        var isHouseConnectedVisibile: boolean = false;
        var isHouseVisible: boolean = true;

        switch (this.LegName) {
            case "Pick Up":
            case "Delivery": {
                isHouseNotConnectedVisibile = false;
                isHouseConnectedVisibile = false;
                isHouseVisible = true;
                break;
            }

            default:
                {
                    if (this.EntityPM.ShipmentLevelCode == "H") {
                        isHouseVisible = false;

                        if (AppTool.IsNullOrEmpty(this.EntityPM.MasterShipmentDataId)) {
                            isHouseNotConnectedVisibile = true;
                        }

                        else {
                            isHouseConnectedVisibile = true;
                        }
                    }

                    break;
                }
        }

        this.IsHouseNotConnectedVisibile = isHouseNotConnectedVisibile;
        this.IsHouseConnectedVisibile = isHouseConnectedVisibile;
        this.IsHouseVisible = isHouseVisible;
    }

    public Header: string;
    private ComputeHeader() {
        if (this.pickUp != null) {
            var str: string[] = this.pickUp.PickUpDeliveryNumber.split('/');
            this.Header = this.LegName + " " + str[1];
        }

        else if (this.delivery != null) {
            var str: string[] = this.delivery.PickUpDeliveryNumber.split('/');
            this.Header = this.LegName + " " + str[1];
        }

        else {
            this.Header = this.LegName;


            if (this.Header == "NOP") {
                this.Header = "Pick Up";
            }

            else if (this.Header == "NOD") {
                this.Header = "Delivery";
            }

            else if (this.Header == "WarehouseLeg") {
                this.Header = "Warehouse";
            }
        }
    }

    public LegImgSrc: string;
    public SetDefaultImageSource() {
        switch (this.LegName) {
            case "NOP": {
                this.LegImgSrc = "./Images/SpotLightLegs/No-PickUp.png";
                break;
            }

            case "Pick Up": {
                if (this.LegIndex == "Start") {
                    this.LegImgSrc = "./Images/SpotLightLegs/No-PickUp.png";
                }
                else {
                    this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                }
                break;
            }

            case "Pre Carriage": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                break;
            }

            case "Main Carriage": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                break;
            }

            case "Transshipment1": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                break;
            }

            case "Transshipment2": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                break;
            }

            case "Transshipment3": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                break;
            }

            case "On Carriage": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                break;
            }

            case "WarehouseLeg": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                break;
            }

            case "NOD": {
                this.LegImgSrc = "./Images/SpotLightLegs/No-Delivery.png";
                break;
            }

            case "Delivery": {
                if (this.LegIndex == "End") {
                    this.LegImgSrc = "./Images/SpotLightLegs/No-Delivery.png";
                }
                else {
                    this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                }
                break;
            }
        }

        this.IsOpenedLeg = false;
    }

    OnMouseOver() {
        if (this.IsNoData) {
            return;
        }

        switch (this.LegName) {
            case "Pick Up": {
                if (this.LegIndex == "Start") {
                    this.LegImgSrc = "./Images/SpotLightLegs/PickUp-hover.png";
                }
                else {
                    this.LegImgSrc = "./Images/SpotLightLegs/leg-closed-hover.png";
                }
                break;
            }

            case "Pre Carriage":
            case "Main Carriage":
            case "Transshipment1":
            case "Transshipment2":
            case "Transshipment3":
            case "On Carriage":
            case "WarehouseLeg": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed-hover.png";
                break;
            }

            case "Delivery": {
                if (this.LegIndex == "End") {
                    this.LegImgSrc = "./Images/SpotLightLegs/Delivery-hover.png";
                }

                else {
                    this.LegImgSrc = "./Images/SpotLightLegs/leg-closed-hover.png";
                }

                break;
            }
        }
    }

    OnMouseLeave() {
        if (this.IsNoData) {
            return;
        }

        switch (this.LegName) {
            case "Pick Up": {
                if (this.LegIndex == "Start") {
                    this.LegImgSrc = "./Images/SpotLightLegs/No-PickUp.png";
                }
                else {
                    this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                }
                break;
            }

            case "Pre Carriage":
            case "Main Carriage":
            case "Transshipment1":
            case "Transshipment2":
            case "Transshipment3":
            case "On Carriage":
            case "WarehouseLeg": {
                this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                break;
            }

            case "Delivery": {
                if (this.LegIndex == "End") {
                    this.LegImgSrc = "./Images/SpotLightLegs/No-Delivery.png";
                }
                else {
                    this.LegImgSrc = "./Images/SpotLightLegs/leg-closed.png";
                }
                break;
            }
        }
    }

    public get CarrierName() {
        switch (this.LegName) {
            case "Pick Up": { return this.pickUp.CarrierName; }
            case "Delivery": { return this.delivery.CarrierName; }
            case "Pre Carriage": { return this.EntityPM.PreCarriageCarrierName; }
            case "Main Carriage": { return this.EntityPM.MainCarriageCarrierName; }
            case "Transshipment1": { return this.EntityPM.Transshipment1CarrierName; }
            case "Transshipment2": { return this.EntityPM.Transshipment2CarrierName; }
            case "Transshipment3": { return this.EntityPM.Transshipment3CarrierName; }
            case "On Carriage": { return this.EntityPM.OnCarriageCarrierName; }
            case "WarehouseLeg": { return this.EntityPM.WarehouseLegTerminalName; }
            default: { return null; }
        }
    }

    public get CarrierNumber() {
        switch (this.LegName) {
            case "Pick Up": { return this.pickUp.CarrierNumber; }
            case "Delivery": { return this.delivery.CarrierNumber; }
            case "Pre Carriage": { return this.EntityPM.PreCarriageCarrierNumber; }
            case "Main Carriage": { return this.IsInlandDomestic ? this.EntityPM.TruckNumber : this.EntityPM.MainCarriageCarrierNumber; }
            case "Transshipment1": { return this.EntityPM.Transshipment1CarrierNumber; }
            case "Transshipment2": { return this.EntityPM.Transshipment2CarrierNumber; }
            case "Transshipment3": { return this.EntityPM.Transshipment3CarrierNumber; }
            case "On Carriage": { return this.EntityPM.OnCarriageCarrierNumber; }
            default: { return null; }
        }
    }

    public get FromCountryCode() {
        switch (this.LegName) {
            case "Pick Up":
                {
                    if (!AppTool.IsNullOrEmpty(this.pickUp.FromPortId)) {
                        return this.pickUp.FromPortCountryCode;
                    }

                    else {
                        return this.pickUp.FromAddressCountryCode;
                    }
                }

            case "Delivery":
                {
                    if (!AppTool.IsNullOrEmpty(this.delivery.FromPortId)) {
                        return this.delivery.FromPortCountryCode;
                    }

                    else {
                        return this.delivery.FromAddressCountryCode;
                    }
                }

            case "WarehouseLeg":
                {
                    return this.EntityPM.WarehouseLegAddressCountryCode;
                }

            case "Pre Carriage": { return this.EntityPM.PreCarriageFromPortCountryCode; }
            case "Main Carriage": { return this.IsInlandDomestic ? this.EntityPM.FromPartnerCountryCode : this.EntityPM.MainCarriageFromPortCountryCode; }
            case "Transshipment1": { return this.EntityPM.Transshipment1FromPortCountryCode; }
            case "Transshipment2": { return this.EntityPM.Transshipment2FromPortCountryCode; }
            case "Transshipment3": { return this.EntityPM.Transshipment3FromPortCountryCode; }
            case "On Carriage": { return this.EntityPM.OnCarriageFromPortCountryCode; }
            default: { return null; }
        }
    }

    public get FromCountryName() {
        switch (this.LegName) {
            case "Pick Up":
                {
                    if (!AppTool.IsNullOrEmpty(this.pickUp.FromPortId)) {
                        return this.pickUp.FromPortCountryName;
                    }

                    else {
                        return this.pickUp.FromAddressCountryName;
                    }
                }

            case "Delivery":
                {
                    if (!AppTool.IsNullOrEmpty(this.delivery.FromPortId)) {
                        return this.delivery.FromPortCountryName;
                    }

                    else {
                        return this.delivery.FromAddressCountryName;
                    }
                }

            case "WarehouseLeg":
                {
                    return this.EntityPM.WarehouseLegAddressCountryName;
                }

            case "Pre Carriage": { return this.EntityPM.PreCarriageFromPortCountryName; }
            case "Main Carriage": { return this.IsInlandDomestic ? this.EntityPM.FromPartnerCountryName : this.EntityPM.MainCarriageFromPortCountryName; }
            case "Transshipment1": { return this.EntityPM.Transshipment1FromPortCountryName; }
            case "Transshipment2": { return this.EntityPM.Transshipment2FromPortCountryName; }
            case "Transshipment3": { return this.EntityPM.Transshipment3FromPortCountryName; }
            case "On Carriage": { return this.EntityPM.OnCarriageFromPortCountryName; }
            default: { return null; }
        }
    }

    public get FromPortCode() {
        switch (this.LegName) {
            case "Pick Up":
                {
                    if (!AppTool.IsNullOrEmpty(this.pickUp.FromPortId)) {
                        return this.pickUp.FromPortCode;
                    }

                    else {
                        if (!AppTool.IsNullOrEmpty(this.pickUp.FromAddressId)) {
                            return this.pickUp.FromAddressCity;
                        }

                        return this.pickUp.FromAddressCountryCode;
                    }
                }

            case "Delivery":
                {
                    if (!AppTool.IsNullOrEmpty(this.delivery.FromPortId)) {
                        return this.delivery.FromPortCode;
                    }

                    else {
                        if (!AppTool.IsNullOrEmpty(this.delivery.FromAddressId)) {
                            return this.delivery.FromAddressCity;
                        }

                        return this.delivery.FromAddressCountryCode;
                    }
                }

            case "WarehouseLeg":
                {
                    return this.EntityPM.WarehouseLegAddressCountryCode;
                }

            case "Pre Carriage": { return this.EntityPM.PreCarriageFromPortCode; }
            case "Main Carriage": { return this.EntityPM.MainCarriageFromPortCode; }
            case "Transshipment1": { return this.EntityPM.Transshipment1FromPortCode; }
            case "Transshipment2": { return this.EntityPM.Transshipment2FromPortCode; }
            case "Transshipment3": { return this.EntityPM.Transshipment3FromPortCode; }
            case "On Carriage": { return this.EntityPM.OnCarriageFromPortCode; }
            default: { return null; }
        }
    }

    public get FromPortName() {
        switch (this.LegName) {
            case "Pick Up":
                {
                    if (!AppTool.IsNullOrEmpty(this.pickUp.FromPortId)) {
                        return this.pickUp.FromPortName;
                    }

                    else {
                        return this.pickUp.FromAddressCountryName;
                    }
                }

            case "Delivery":
                {
                    if (!AppTool.IsNullOrEmpty(this.delivery.FromPortId)) {
                        return this.delivery.FromPortName;
                    }

                    else {
                        return this.delivery.FromAddressCountryName;
                    }
                }

            case "WarehouseLeg":
                {
                    return this.EntityPM.WarehouseLegAddressCountryName;
                }

            case "Pre Carriage": { return this.EntityPM.PreCarriageFromPortName; }
            case "Main Carriage": { return this.IsInlandDomestic ? this.EntityPM.FromPartnerCity : this.EntityPM.MainCarriageFromPortName; }
            case "Transshipment1": { return this.EntityPM.Transshipment1FromPortName; }
            case "Transshipment2": { return this.EntityPM.Transshipment2FromPortName; }
            case "Transshipment3": { return this.EntityPM.Transshipment3FromPortName; }
            case "On Carriage": { return this.EntityPM.OnCarriageFromPortName; }
            default: { return null; }
        }
    }

    public get ToCountryCode() {
        switch (this.LegName) {
            case "Pick Up":
                {
                    if (!AppTool.IsNullOrEmpty(this.pickUp.ToPortId)) {
                        return this.pickUp.ToPortCountryCode;
                    }

                    else {
                        return this.pickUp.ToAddressCountryCode;
                    }
                }

            case "Delivery":
                {
                    if (!AppTool.IsNullOrEmpty(this.delivery.ToPortId)) {
                        return this.delivery.ToPortCountryCode;
                    }

                    else {
                        return this.delivery.ToAddressCountryCode;
                    }
                }

            case "Pre Carriage": { return this.EntityPM.PreCarriageToPortCountryCode; }
            case "Main Carriage": { return this.IsInlandDomestic ? this.EntityPM.ToPartnerCountryCode : this.EntityPM.MainCarriageToPortCountryCode; }
            case "Transshipment1": { return this.EntityPM.Transshipment1ToPortCountryCode; }
            case "Transshipment2": { return this.EntityPM.Transshipment2ToPortCountryCode; }
            case "Transshipment3": { return this.EntityPM.Transshipment3ToPortCountryCode; }
            case "On Carriage": { return this.EntityPM.OnCarriageToPortCountryCode; }
            default: { return null; }
        }
    }

    public get ToCountryName() {
        switch (this.LegName) {
            case "Pick Up":
                {
                    if (!AppTool.IsNullOrEmpty(this.pickUp.ToPortId)) {
                        return this.pickUp.ToPortCountryName;
                    }

                    else {
                        return this.pickUp.ToAddressCountryName;
                    }
                }

            case "Delivery":
                {
                    if (!AppTool.IsNullOrEmpty(this.delivery.ToPortId)) {
                        return this.delivery.ToPortCountryName;
                    }

                    else {
                        return this.delivery.ToAddressCountryName;
                    }
                }

            case "Pre Carriage": { return this.EntityPM.PreCarriageToPortCountryName; }
            case "Main Carriage": { return this.IsInlandDomestic ? this.EntityPM.ToPartnerCountryName : this.EntityPM.MainCarriageToPortCountryName; }
            case "Transshipment1": { return this.EntityPM.Transshipment1ToPortCountryName; }
            case "Transshipment2": { return this.EntityPM.Transshipment2ToPortCountryName; }
            case "Transshipment3": { return this.EntityPM.Transshipment3ToPortCountryName; }
            case "On Carriage": { return this.EntityPM.OnCarriageToPortCountryName; }
            default: { return null; }
        }
    }

    public get ToPortCode() {
        switch (this.LegName) {
            case "Pick Up":
                {
                    if (!AppTool.IsNullOrEmpty(this.pickUp.ToPortId)) {
                        return this.pickUp.ToPortCode;
                    }

                    else {
                        if (!AppTool.IsNullOrEmpty(this.pickUp.ToAddressId)) {
                            return this.pickUp.ToAddressCity;
                        }

                        return this.pickUp.ToAddressCountryCode;
                    }
                }

            case "Delivery":
                {
                    if (!AppTool.IsNullOrEmpty(this.delivery.ToPortId)) {
                        return this.delivery.ToPortCode;
                    }

                    else {
                        if (!AppTool.IsNullOrEmpty(this.delivery.ToAddressId)) {
                            return this.delivery.ToAddressCity;
                        }

                        return this.delivery.ToAddressCountryCode;
                    }
                }

            case "Pre Carriage": { return this.EntityPM.PreCarriageToPortCode; }
            case "Main Carriage": { return this.EntityPM.MainCarriageToPortCode; }
            case "Transshipment1": { return this.EntityPM.Transshipment1ToPortCode; }
            case "Transshipment2": { return this.EntityPM.Transshipment2ToPortCode; }
            case "Transshipment3": { return this.EntityPM.Transshipment3ToPortCode; }
            case "On Carriage": { return this.EntityPM.OnCarriageToPortCode; }
            default: { return null; }
        }
    }

    public get ToPortName() {
        switch (this.LegName) {
            case "Pick Up":
                {
                    if (!AppTool.IsNullOrEmpty(this.pickUp.ToPortId)) {
                        return this.pickUp.ToPortName;
                    }

                    else {
                        return this.pickUp.ToAddressCountryName;
                    }
                }

            case "Delivery":
                {
                    if (!AppTool.IsNullOrEmpty(this.delivery.ToPortId)) {
                        return this.delivery.ToPortName;
                    }

                    else {
                        return this.delivery.ToAddressCountryName;
                    }
                }

            case "Pre Carriage": { return this.EntityPM.PreCarriageToPortName; }
            case "Main Carriage": { return this.IsInlandDomestic ? this.EntityPM.ToPartnerCity : this.EntityPM.MainCarriageToPortName; }
            case "Transshipment1": { return this.EntityPM.Transshipment1ToPortName; }
            case "Transshipment2": { return this.EntityPM.Transshipment2ToPortName; }
            case "Transshipment3": { return this.EntityPM.Transshipment3ToPortName; }
            case "On Carriage": { return this.EntityPM.OnCarriageToPortName; }
            default: { return null; }
        }
    }

    public get DepartedDate_Actual() {
        switch (this.LegName) {
            case "Pick Up": { return this.pickUp.ATD; }
            case "Delivery": { return this.delivery.ATD; }
            case "Pre Carriage": { return this.EntityPM.PreCarriageATD; }
            case "Main Carriage": { return this.EntityPM.MainCarriageATD; }
            case "Transshipment1": { return this.EntityPM.Transshipment1ATD; }
            case "Transshipment2": { return this.EntityPM.Transshipment2ATD; }
            case "Transshipment3": { return this.EntityPM.Transshipment3ATD; }
            case "On Carriage": { return this.EntityPM.OnCarriageATD; }
            case "WarehouseLeg": { return this.EntityPM.WarehouseLegActualEntryDate; }
            default: { return null; }
        }
    }
    public set DepartedDate_Actual(newValue: Date) {
        switch (this.LegName) {
            case "Pick Up":
                {
                    this.pickUp.ATD = newValue;

                    var followUpLegName: string = this.LegName.replace(" ", "") + "Departure";
                    this.SetActualMethod(followUpLegName + this.pickUp.PickUpDeliveryNumber);
                    this.SetActualMethod(followUpLegName + this.pickUp.PickUpDeliveryNumber + this.pickUp.PickUpDeliveryNumber);
                    break;
                }

            case "Delivery":
                {
                    this.delivery.ATD = newValue;

                    var followUpLegName: string = this.LegName.replace(" ", "") + "Departure";
                    this.SetActualMethod(followUpLegName + this.delivery.PickUpDeliveryNumber);
                    this.SetActualMethod(followUpLegName + this.delivery.PickUpDeliveryNumber + this.delivery.PickUpDeliveryNumber);
                    break;
                }

            case "Pre Carriage":
                {
                    this.EntityPM.PreCarriageATD = newValue;
                    this.SetActualMethod("PreCarriageDeparture");
                    break;
                }

            case "Main Carriage":
                {
                    this.EntityPM.MainCarriageATD = newValue;
                    this.SetActualMethod("MainCarriageDeparture");
                    break;
                }

            case "Transshipment1":
                {
                    this.EntityPM.Transshipment1ATD = newValue;
                    this.SetActualMethod("Transshipment1Departure");
                    break;
                }

            case "Transshipment2":
                {
                    this.EntityPM.Transshipment2ATD = newValue;
                    this.SetActualMethod("Transshipment2Departure");
                    break;
                }

            case "Transshipment3":
                {
                    this.EntityPM.Transshipment3ATD = newValue;
                    this.SetActualMethod("Transshipment2Departure");
                    break;
                }

            case "On Carriage":
                {
                    this.EntityPM.OnCarriageATD = newValue;
                    this.SetActualMethod("OnCarriageDeparture");
                    break;
                }


            case "WarehouseLeg": 
                {
                    this.EntityPM.WarehouseLegActualEntryDate = newValue;
                    this.SetActualMethod("WarehouseEntry");
                    break;
                }

            default: { break; }
        }

        this.SetDateVisibilityProperties();
    }

    public get DepartedDate_Estimate() {
        switch (this.LegName) {
            case "Pick Up": { return this.pickUp.ETD; }
            case "Delivery": { return this.delivery.ETD; }
            case "Pre Carriage": { return this.EntityPM.PreCarriageETD; }
            case "Main Carriage": { return this.EntityPM.MainCarriageETD; }
            case "Transshipment1": { return this.EntityPM.Transshipment1ETD; }
            case "Transshipment2": { return this.EntityPM.Transshipment2ETD; }
            case "Transshipment3": { return this.EntityPM.Transshipment3ETD; }
            case "On Carriage": { return this.EntityPM.OnCarriageETD; }
            case "WarehouseLeg": { return this.EntityPM.WarehouseLegExpectedEntryDate; }
            default: { return null; }
        }
    }
    public set DepartedDate_Estimate(newValue: Date) {
        switch (this.LegName) {
            case "Pick Up": { this.pickUp.ETD = newValue; break; }
            case "Delivery": { this.delivery.ETD = newValue; break; }
            case "Pre Carriage": { this.EntityPM.PreCarriageETD = newValue; break; }
            case "Main Carriage": { this.EntityPM.MainCarriageETD = newValue; break; }
            case "Transshipment1": { this.EntityPM.Transshipment1ETD = newValue; break; }
            case "Transshipment2": { this.EntityPM.Transshipment2ETD = newValue; break; }
            case "Transshipment3": { this.EntityPM.Transshipment3ETD = newValue; break; }
            case "On Carriage": { this.EntityPM.OnCarriageETD = newValue; break; }
            case "WarehouseLeg": { this.EntityPM.WarehouseLegExpectedEntryDate = newValue; break; }
            default: { break; }
        }

        this.SetDateVisibilityProperties();
    }

    public get ArrivalDate_Actual() {
        switch (this.LegName) {
            case "Pick Up": { return this.pickUp.ATA; }
            case "Delivery": { return this.delivery.ATA; }
            case "Pre Carriage": { return this.EntityPM.PreCarriageATA; }
            case "Main Carriage": { return this.EntityPM.MainCarriageATA; }
            case "Transshipment1": { return this.EntityPM.Transshipment1ATA; }
            case "Transshipment2": { return this.EntityPM.Transshipment2ATA; }
            case "Transshipment3": { return this.EntityPM.Transshipment3ATA; }
            case "On Carriage": { return this.EntityPM.OnCarriageATA; }
            case "WarehouseLeg": { return this.EntityPM.WarehouseLegActualReleaseDate; }
            default: { return null; }
        }
    }
    public set ArrivalDate_Actual(newValue: Date) {
        switch (this.LegName) {
            case "Pick Up":
                {
                    this.pickUp.ATA = newValue;
                    var followUpLegName: string = this.LegName.replace(" ", "") + "Arrival";
                    this.SetActualMethod(followUpLegName + this.pickUp.PickUpDeliveryNumber);
                    this.SetActualMethod(followUpLegName + this.pickUp.PickUpDeliveryNumber + this.pickUp.PickUpDeliveryNumber);
                    break;
                }

            case "Delivery":
                {
                    this.delivery.ATA = newValue;
                    var followUpLegName: string = this.LegName.replace(" ", "") + "Arrival";
                    this.SetActualMethod(followUpLegName + this.delivery.PickUpDeliveryNumber);
                    this.SetActualMethod(followUpLegName + this.delivery.PickUpDeliveryNumber + this.delivery.PickUpDeliveryNumber);
                    break;
                }

            case "Pre Carriage":
                {
                    this.EntityPM.PreCarriageATA = newValue;
                    this.SetActualMethod("PreCarriageArrival");
                    break;
                }

            case "Main Carriage":
                {
                    this.EntityPM.MainCarriageATA = newValue;
                    this.SetActualMethod("MainCarriageArrival");
                    break;
                }

            case "Transshipment1":
                {
                    this.EntityPM.Transshipment1ATA = newValue;
                    this.SetActualMethod("Transshipment1Arrival");
                    break;
                }

            case "Transshipment2":
                {
                    this.EntityPM.Transshipment2ATA = newValue;
                    this.SetActualMethod("Transshipment2Arrival");
                    break;
                }

            case "Transshipment3":
                {
                    this.EntityPM.Transshipment3ATA = newValue;
                    this.SetActualMethod("Transshipment3Arrival");
                    break;
                }

            case "On Carriage":
                {
                    this.EntityPM.OnCarriageATA = newValue;
                    this.SetActualMethod("OnCarriageArrival");
                    break;
                }

            case "WarehouseLeg":
                {
                    this.EntityPM.WarehouseLegActualReleaseDate = newValue;
                    this.SetActualMethod("WarehouseRelease");
                    break;
                }

            default: { break; }
        }

        this.SetDateVisibilityProperties();
    }

    public get ArrivalDate_Estimate() {
        switch (this.LegName) {
            case "Pick Up": { return this.pickUp.ETA; }
            case "Delivery": { return this.delivery.ETA; }
            case "Pre Carriage": { return this.EntityPM.PreCarriageETA; }
            case "Main Carriage": { return this.EntityPM.MainCarriageETA; }
            case "Transshipment1": { return this.EntityPM.Transshipment1ETA; }
            case "Transshipment2": { return this.EntityPM.Transshipment2ETA; }
            case "Transshipment3": { return this.EntityPM.Transshipment3ETA; }
            case "On Carriage": { return this.EntityPM.OnCarriageETA; }
            case "WarehouseLeg": { return this.EntityPM.WarehouseLegExpectedReleaseDate; }
            default: { return null; }
        }
    }
    public set ArrivalDate_Estimate(newValue: Date) {
        switch (this.LegName) {
            case "Pick Up": { this.pickUp.ETA = newValue; break; }
            case "Delivery": { this.delivery.ETA = newValue; break; }
            case "Pre Carriage": { this.EntityPM.PreCarriageETA = newValue; break; }
            case "Main Carriage": { this.EntityPM.MainCarriageETA = newValue; break; }
            case "Transshipment1": { this.EntityPM.Transshipment1ETA = newValue; break; }
            case "Transshipment2": { this.EntityPM.Transshipment2ETA = newValue; break; }
            case "Transshipment3": { this.EntityPM.Transshipment3ETA = newValue; break; }
            case "On Carriage": { this.EntityPM.OnCarriageETA = newValue; break; }
            case "WarehouseLeg": { this.EntityPM.WarehouseLegExpectedReleaseDate = newValue; break; }
            default: { break; }
        }

        this.SetDateVisibilityProperties();
    }

    private SetActualMethod(s: string) {
        //var followUp: ShipmentFollowUpPM = this.EntityPM.FollowUps.filter(d => !d.Done && d.LegType == s)[0];        

        //if (followUp != null) {
        //    followUp.Done = true;
        //}
    }
}
