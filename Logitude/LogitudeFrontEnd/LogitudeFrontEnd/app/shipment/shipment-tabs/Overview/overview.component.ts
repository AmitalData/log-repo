import {Component, OnInit, ComponentRef}  from 'angular2/core';
import {RouteParams, Router} from 'angular2/router';
import {NgStyle} from 'angular2/common';
import {ShipmentsService} from '../../services/shipment-service/shipments.service';
import {LogitudeUtilities} from "../../../infrastructure/utilities/LogitudeUtilities";
import {TextCodeTranslator} from '../../../infrastructure/utilities/TextCodeTranslator'
import {TextcodeTranslationPipe} from '../../../infrastructure/pipes/textcode-translation/textcode-translation.pipe';
import {DateTimeToDatePipe} from '../../../infrastructure/pipes/DateTimeToDatePipe';
import {DateTimeToTimePipe} from '../../../infrastructure/pipes/DateTimeToTimePipe';
import {EntityArgs} from '../../../infrastructure/data-contracts/entity-args';
import {EditControlComponent} from '../../../infrastructure/edit-control/edit-control.component';

interface Container {
    Quantity: number,
    PackageTypeName: string,
}

interface NoteItem {
    Header: string,
    Notes: string,
    HideNotes?: boolean,
}

@Component({
    templateUrl: 'Views/Shipment/Tabs/OverviewTab.html',    
    directives: [NgStyle],
    pipes: [TextcodeTranslationPipe, DateTimeToDatePipe, DateTimeToTimePipe],
})

export class OverviewComponent implements OnInit {

    private _entityId: string;
    public EntityPM: any;
    public IsDataLoaded: boolean = false;
    public IsLCLEntity: boolean = false;
    public IsFCLEntity: boolean = false;    
    public ObjectTableName: string;

    constructor(public entityArgs: EntityArgs, private _componentRef: ComponentRef, private _shipmentsService: ShipmentsService) {
        //console.log("entity args: ", this.entityArgs);
        //EditControlComponent.ActiveViewTab = this._componentRef;
    }

    // Shipment Data
    public NumberOfPackages: number = 0;
    public Volume: number = 0;
    public GrossWeight: number = 0;
    public ChargeableWeight: number = 0;
    public VolumeUnitCode: string = "CBM";
    public GrossWeightUnitCode: string = "KG";
    public ChargeableWeightUnitCode: string = "KG";
    public ContainersList: Container[];

    // Master Data
    public MasterLabel: string = "";
    public CarrierLabel: string = "";
    public CarrierNoLabel: string = "";
    public CarrierDateLabel: string = "";
    public MasterDataLableWidth: string = "70px";
    public LongMaster: string = "";
    public CarrierName: string = "";
    public CarrierNumber: string = "";
    public MasterDepartureDate: Date;

    // Money Data
    public IsByLocalCurrency: boolean = false;
    public SelectedCurrencyFilter: string = "USD";
    public ARInvoices: number = 0;
    public APInvoices: number = 0;
    public OpenReceivables: number = 0;
    public OpenPayables: number = 0;
    public Profit: number = 0;
    public TotalAR: number = 0;
    public TotalAP: number = 0;
    public ProfitColor: string = "#282E30";

    // Notes    
    public NotesList: NoteItem[];
    public HasEntityNotes: boolean = false;

    BuildCargoData() {
        this.NumberOfPackages = this.EntityPM.NumberOfPackages == null ? 0 : this.EntityPM.NumberOfPackages;
        this.Volume = this.EntityPM.Volume == null ? 0 : this.EntityPM.Volume;
        this.GrossWeight = this.EntityPM.GrossWeight == null ? 0 : this.EntityPM.GrossWeight;
        this.ChargeableWeight = this.EntityPM.ChargeableWeight == null ? 0 : this.EntityPM.ChargeableWeight;
        this.VolumeUnitCode = this.EntityPM.VolumeUnitCode;
        this.GrossWeightUnitCode = this.EntityPM.GrossWeightUnitCode;
        this.ChargeableWeightUnitCode = this.EntityPM.ChargeableWeightUnitCode;

        if (this.IsFCLEntity) {

            if (this.ContainersList == null) {
                this.ContainersList = new Array<Container>();
            }

            else {
                this.ContainersList = [];
            }

            this.EntityPM.ShipmentPackages.forEach((item) => {
                this.ContainersList.push({ Quantity: item.Quantity, PackageTypeName: "test" });
            })

            //var myArray = new Array<MyClass>();
            //var myArray: MyClass[] = [];
            //var myArray = <MyClass[]>[];

            //List < ByPckageType > bcntList = (from item in entityPM.ShipmentPackages where item.IsContainer group item by new { item.PackageTypeId } into g select new ByPckageType { PackageTypeId = g.Key.PackageTypeId, Quantity = g.Sum(s => s.Quantity), MeasurmentId = PackageTypeDataProvider.GetCachedList<PackageTypeList>().Where(d => d.Id == g.Key.PackageTypeId).FirstOrDefault().MeasurementId, }).ToList();
            //foreach(ByPckageType item in bcntList)
            //{
            //    FCLClass newItem = new FCLClass() { Quantity = (int)item.Quantity };
            //    PackageTypeList list = PackageTypeDataProvider.GetCachedList<PackageTypeList>().Where(d => d.Id == item.PackageTypeId).FirstOrDefault();
            //    if (list != null) {
            //        newItem.PackageTypeName = list.EnglishName;
            //    }
            //    ContainersList.Add(newItem);
            //}
        }

        this.LongMaster = this.EntityPM.LongMaster;
        this.CarrierName = this.EntityPM.MainCarriageCarrierName;
        this.CarrierNumber = this.EntityPM.MainCarriageCarrierNumber;
        this.MasterDepartureDate = (this.EntityPM.MainCarriageATD != null) ? this.EntityPM.MainCarriageATD : this.EntityPM.MainCarriageETD;

        switch (this.EntityPM.TransportModeId) {

            case "A": {
                this.MasterDataLableWidth = "70px";
                this.MasterLabel = TextCodeTranslator.Translate("Shipment.O.Overview.MAWB");
                this.CarrierLabel = TextCodeTranslator.Translate("Shipment.O.Overview.Flight");
                this.CarrierNoLabel = TextCodeTranslator.Translate("Shipment.O.Overview.FlightNo");
                this.CarrierDateLabel = TextCodeTranslator.Translate("Shipment.O.Overview.FlightDate");                
                break;
            }

            case "O": {
                this.MasterDataLableWidth = "80px";
                this.MasterLabel = TextCodeTranslator.Translate("Shipment.O.Overview.OBL");
                this.CarrierLabel = TextCodeTranslator.Translate("Shipment.O.Overview.ShippingLine");
                this.CarrierNoLabel = TextCodeTranslator.Translate("Shipment.O.Overview.VoyageNo");
                this.CarrierDateLabel = TextCodeTranslator.Translate("Shipment.O.Overview.VoyageDate");
                break;
            }

            case "I": {
                this.MasterDataLableWidth = "80px";
                this.MasterLabel = TextCodeTranslator.Translate("Shipment.O.Overview.CMR/RWB#");
                this.CarrierLabel = TextCodeTranslator.Translate("Shipment.O.Overview.Trucker");
                this.CarrierNoLabel = TextCodeTranslator.Translate("Shipment.O.Overview.TruckNo");
                this.CarrierDateLabel = TextCodeTranslator.Translate("Shipment.O.Overview.TruckerDate");
                break;
            }
        }
    }

    BuildMoneyData() {

        this.ARInvoices = 0;
        this.APInvoices = 0;
        this.OpenReceivables = 0;
        this.OpenPayables = 0;
        this.Profit = 0;

        if (this.EntityPM != null && this.EntityPM !== undefined) {

            if (this.IsByLocalCurrency) {
                this.OpenPayables = this.EntityPM.OpenPayablesInLocalCurrency;
                this.OpenReceivables = this.EntityPM.OpenReceivablesInLocalCurrency;

                if (this.EntityPM.ShipmentARInvoices != null) {

                    var myField: number = 0;
                    for (var i = 0; i < this.EntityPM.ShipmentARInvoices.length; i++) {
                        myField += this.EntityPM.ShipmentARInvoices[i].AmountInLocalCurrency;
                    }

                    this.ARInvoices = myField;
                }

                if (this.EntityPM.ShipmentAPInvoices != null) {

                    var myField: number = 0;
                    for (var i = 0; i < this.EntityPM.ShipmentAPInvoices.length; i++) {
                        myField += this.EntityPM.ShipmentAPInvoices[i].GrandTotalInLocalCurrency;
                    }

                    this.ARInvoices = myField;
                }

                this.Profit = this.EntityPM.ProfitInLocalCurrency;
            }

            else {

                this.OpenPayables = this.EntityPM.OpenPayablesInProfitCurrency;
                this.OpenReceivables = this.EntityPM.OpenReceivablesInProfitCurrency;

                if (this.EntityPM.ShipmentARInvoices != null) {

                    var myField: number = 0;
                    for (var i = 0; i < this.EntityPM.ShipmentARInvoices.length; i++) {
                        myField += this.EntityPM.ShipmentARInvoices[i].AmountInProfitCurrency;
                    }

                    this.ARInvoices = myField;
                }

                if (this.EntityPM.ShipmentAPInvoices != null) {

                    var myField: number = 0;
                    for (var i = 0; i < this.EntityPM.ShipmentAPInvoices.length; i++) {
                        myField += this.EntityPM.ShipmentAPInvoices[i].GrandTotalInProfitCurrency;
                    }

                    this.ARInvoices = myField;
                }

                this.Profit = this.EntityPM.ProfitInProfitCurrency;
            }

            this.TotalAR = this.ARInvoices + this.OpenReceivables;
            this.TotalAP = this.APInvoices + this.OpenPayables;

            if (this.Profit > 0) {
                this.ProfitColor = "#009161";
            }

            else if (this.Profit < 0) {
                this.ProfitColor = "#E53030";
            }

            else {
                this.ProfitColor = "#282E30";
            }
        }
    }

    BuildNotesList() {

    // replace the (\r) with (\n)
        var myEntityNotes: string;
        if (this.EntityPM.Notes == null) {
            this.HasEntityNotes = false;
            this.NotesList = [{ Header: "Shipment", Notes: this.EntityPM.Notes, HideNotes: true }];
        }

        else {
            this.HasEntityNotes = true;
            this.NotesList = [{ Header: "Shipment", Notes: this.EntityPM.Notes }];
        }

        myEntityNotes = this.EntityPM.ShipperNote;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Shipper", Notes: myEntityNotes });
        }

        myEntityNotes = this.EntityPM.ConsigneeNote;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Consignee", Notes: myEntityNotes });
        }

        myEntityNotes = this.EntityPM.AgentNote;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Agent", Notes: myEntityNotes });
        }

        myEntityNotes = this.EntityPM.CustomAgentExportNote;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Custom Agent Export", Notes: myEntityNotes });
        }

        myEntityNotes = this.EntityPM.CustomAgentImportNote;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Custom Agent Import", Notes: myEntityNotes });
        }

        myEntityNotes = this.EntityPM.Notify1Note;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Notify1", Notes: myEntityNotes });
        }

        myEntityNotes = this.EntityPM.Notify2Note;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Notify2", Notes: myEntityNotes });
        }

        myEntityNotes = this.EntityPM.ConsigneeNotImporterNote;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Consignee Not Importer", Notes: myEntityNotes });
        }

        myEntityNotes = this.EntityPM.ShipperNotExporterNote;
        if (myEntityNotes != null && myEntityNotes != "") {
            this.NotesList.push({ Header: "Shipper Not Exporter", Notes: myEntityNotes });
        }
    }

    ngOnInit()
    {        
        //this.EntityPM = this._shipmentsService.getSingleShipmentByIdFromArray(this._entityId, 1);
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
               
        if (this.EntityPM != null && this.EntityPM !== undefined) {

            this.IsDataLoaded = true;
            this.IsLCLEntity = LogitudeUtilities.IsLCLEntity(this.EntityPM.TransportModeId, this.EntityPM.ShipmentTypeId);
            this.IsFCLEntity = !this.IsLCLEntity;

            this.BuildCargoData();
            this.BuildMoneyData();
            this.BuildNotesList();            
        }       
    }

    onSelectCurrency(myArgs: string) {
        this.SelectedCurrencyFilter = myArgs;

        if (myArgs == "NIS") {
            this.IsByLocalCurrency = true;
        }

        else {
            this.IsByLocalCurrency = false;
                
        }

        this.BuildMoneyData();
    }
}
