import {  Component } from "@angular/core";
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ShipmentPM } from '../../../../Shipment/EntityPMs/ShipmentPM';

import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { ShipmentReferancePM } from "Shipment/EntityPMs/ShipmentReferancePM";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { ColumnsWidths } from "Infrastructure/Components/LogitudeComponents/LogLovV2Component";
import { FreightForwarderReferencePM } from "Shipment/EntityPMs/FreightForwarderReferencePM";
import { EntityResourceService } from "Infrastructure/Services/EntityResourceService";

@Component({    
    templateUrl: './ShipmentDataTabComponent.html',
})

export class ShipmentDataTabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public DataContext = this;
    public ColumnsWidths: ColumnsWidths[];
    public entityResourceService: EntityResourceService = new EntityResourceService();

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.InitializeShipmentReferance();

        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.Listen();

        this.ColumnsWidths = [
            { ColumnName: 'Code', Width: 40 },
            { ColumnName: 'CalculatedEnglishName', Width: 85 },
            { ColumnName: 'CalculatedLocalName', Width: 85 },
            { ColumnName: 'Address1', Width: 50 },
            { ColumnName: 'PartnerTypeName', Width: 50 },
            { ColumnName: 'CountryName', Width: 50 },
            { ColumnName: 'CityName', Width: 50 },
            { ColumnName: 'CountryCode', Width: 60 },
        ];        
    }

    InitializeShipmentReferance() {
        for (var i = 0; i < this.ShipmentReferances.length; i++) {
            let shipmentReferance = new ShipmentReferancePM();

            for (var field in this.ShipmentReferances[i]) {
                shipmentReferance[field] = this.ShipmentReferances[i][field];
            }
            this.ShipmentReferances[i] = shipmentReferance;
        }
        this.SetReferenceTypeValueState();
    }

    Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.InitializeShipmentReferance();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    OnChanged() {
        this.EntityPM.IsCustomShipment = true;
    }

    //#region Properties
    public get CustomerId() { return this.EntityPM.CustomerId; }
    public set CustomerId(newValue: string) {
        if (this.EntityPM.CustomerId != newValue) {
            this.EntityPM.CustomerId = newValue;
            this.OnChanged();
        }
    }

    public get DepartmentId() { return this.EntityPM.DepartmentId; }
    public set DepartmentId(newValue: string) {
        if (this.EntityPM.DepartmentId != newValue) {
            this.EntityPM.DepartmentId = newValue;
            this.OnChanged();
        }
    }

    public get IskaNumber() { return this.EntityPM.IskaNumber; }
    public set IskaNumber(newValue: string) {
        if (this.EntityPM.IskaNumber != newValue) {
            this.EntityPM.IskaNumber = newValue;

            if(newValue.startsWith("I") || newValue.startsWith("i")){
                this.UIProperties.SetValidity("IskaNumber", this.ObjectTableName, true, "");
            }else{
                
                this.UIProperties.SetValidity("IskaNumber", this.ObjectTableName, false, TextCodeTranslator.Translate('Shipment.O.InvalidIskaNumber'));
            }
            
            this.OnChanged();
        }
    }

    public get House() { return this.EntityPM.House; }
    public set House(newValue: string) {
        if (this.EntityPM.House != newValue) {
            this.EntityPM.House = newValue;
            this.OnChanged();
        }
    }
    public get HAWBDate() { return this.EntityPM.HAWBDate; }
    public set HAWBDate(newValue: Date) {
        if (this.EntityPM.HAWBDate != newValue) {
            this.EntityPM.HAWBDate = newValue;
            this.OnChanged();
        }
    }
    public get MainCarriageFromPortId() { return this.EntityPM.MainCarriageFromPortId; }
    public set MainCarriageFromPortId(newValue: string) {
        if (this.EntityPM.MainCarriageFromPortId != newValue) {
            this.EntityPM.MainCarriageFromPortId = newValue;
            this.OnChanged();
        }
    }
    public get IncotermId() { return this.EntityPM.IncotermId; }
    public set IncotermId(newValue: string) {
        if (this.EntityPM.IncotermId != newValue) {
            this.EntityPM.IncotermId = newValue;
            this.OnChanged();
        }
    }
    public get OriginCountryCode() { return this.EntityPM.OriginCountryCode; }
    public set OriginCountryCode(newValue: string) {
        if (this.EntityPM.OriginCountryCode != newValue) {
            this.EntityPM.OriginCountryCode = newValue;
            this.OnChanged();
        }
    }
    EditReferenceTypeValue() {

        const shipmentReferences = this.GetActiveShipmentReferances();
        if (shipmentReferences.length == 0 || (shipmentReferences.length > 0 && AppTool.IsNullOrEmpty(shipmentReferences[0].ReferenceType))) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("יש לבחור סוג אסמכתא");
            return;
        }

        //this.CurrentSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("Shipment.O.ReferenceTypeValue");
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "ok") {
                this.OnChanged();
                this.SetReferenceTypeValueState();
                // this.EntityPM.MarkAsDirty();
            }
        });
        logWindow.Show('./ShipmentModules/ShipmentTabs/Components/ShipmentData/ShipmentReferenceDetails/ShipmentReferenceDetailsComponent');   
    }

    disableReferenceTypeValue = false;
    public get ShipmentReferances() { return this.EntityPM.ShipmentReferances; }
    public set ShipmentReferances(newValue: ShipmentReferancePM[]) {
        if (this.EntityPM.ShipmentReferances != newValue) {
            this.EntityPM.ShipmentReferances = newValue;

            this.OnChanged();
            this.SetReferenceTypeValueState();
        }
    }

    SetReferenceTypeValueState() {
        this.disableReferenceTypeValue = this.GetActiveShipmentReferances().length > 1;
    }

    AddShipmentReferance() {
        var item: ShipmentReferancePM = new ShipmentReferancePM();
        item.ShipmentId = this.EntityPM.Id;
        item.Tenant = this.EntityPM.Tenant;
        item.LineNumber = 1;
        item.ChangeSetOp = "Insert";
        this.EntityPM.ShipmentReferances.push(item);
    }

    GetActiveShipmentReferances() {
        return this.ShipmentReferances.filter(entity => entity.ChangeSetOp != "Delete");
    }

    public get ReferenceType() { 
        let shipmentReferances = this.GetActiveShipmentReferances();

        if (shipmentReferances.length > 1) {
            const uniqueValues = new Set(shipmentReferances.map(item => item.ReferenceType));
            if (uniqueValues.size > 1) {
                return "SHP";
            }
            else {
                return shipmentReferances[0].ReferenceType;
            }
        }
        else {
            return shipmentReferances[0]?.ReferenceType;
        }
    }
    public set ReferenceType(newValue: string) {
        let shipmentReferances = this.GetActiveShipmentReferances();

        if (shipmentReferances.length == 0 || (shipmentReferances.length == 1 && shipmentReferances[0]?.ReferenceType != newValue)) {

            var index = 0;
            if (shipmentReferances.length == 0) {
                this.AddShipmentReferance();
            }
            else {
                index = this.ShipmentReferances.indexOf(shipmentReferances[0]);
            }

            if (index > -1) {
                this.ShipmentReferances[index].ReferenceType = newValue;
                if (!this.ShipmentReferances[index].ChangeSetOp) {
                    this.ShipmentReferances[index].ChangeSetOp = "Update";
                }
                this.EntityPM.MarkAsDirty("ShipmentReferances");
                this.OnChanged();
            }
        }
    }

    public get ReferenceValue() { 
        let shipmentReferances = this.GetActiveShipmentReferances();
        if (shipmentReferances.length > 1) {
            return "LIST";
        }
        else {
            return shipmentReferances[0]?.ReferenceValue;
        }
    }
    public set ReferenceValue(newValue: string) {
        let shipmentReferances = this.GetActiveShipmentReferances();

        if (shipmentReferances.length == 0 || (shipmentReferances.length == 1 && shipmentReferances[0]?.ReferenceValue != newValue)) {

            var index = 0;
            if (shipmentReferances.length == 0) {
                this.AddShipmentReferance();
            }
            else {
                index = this.ShipmentReferances.indexOf(shipmentReferances[0]);
            }

            if (index > -1) {
                this.ShipmentReferances[index].ReferenceValue = newValue;
                if (!this.ShipmentReferances[index].ChangeSetOp) {
                    this.ShipmentReferances[index].ChangeSetOp = "Update";
                }
                this.EntityPM.MarkAsDirty("ShipmentReferances");
                this.OnChanged();
            }
        }
    }
    //#region FreightForwarderReferences
    EditForwarderShipmentNumberValue() {
        this.entityResourceService.getEntityResourceByTableName("FreightForwarderReference").subscribe((response: any) => {   
          const freightForwarderReferences = this.GetActiveFreightForwarderReferences();
          if (freightForwarderReferences.length == 0 || (freightForwarderReferences.length > 0 && AppTool.IsNullOrEmpty(freightForwarderReferences[0].ForwarderShipmentNumber))) {
              var messageWindow: MessageWindow = new MessageWindow();
              messageWindow.Show(TextCodeTranslator.Translate("FreightForwarderReference.O.ForwarderShiptNumMust"));
              return;
          }
  
          var windowArgs: any = {};
          windowArgs.EntityPM = this.EntityPM;
  
          var logWindow = new LogitudeWindow();
          logWindow.Width = 320;
          logWindow.Height = 350;
          logWindow.Title = TextCodeTranslator.Translate("FreightForwarderReference.F.ForwarderShipmentNumber");
          logWindow.ShowCloseButton = true;
          logWindow.WindowArgs = windowArgs;
          logWindow.WindowClosed.subscribe(($event: any) => {
              if ($event == "ok") {
                  this.OnChanged();
              }
          });
          logWindow.Show('./ShipmentModules/ShipmentTabs/Components/ShipmentData/FreightForwarderReferenceDetails/FreightForwarderReferenceDetailsComponent'); 
        });  
    }
   

   
    public get FreightForwarderReferences() { return this.EntityPM.FreightForwarderReferences; }
    public set FreightForwarderReferences(newValue: FreightForwarderReferencePM[]) {
        if (this.EntityPM.FreightForwarderReferences != newValue) {
            this.EntityPM.FreightForwarderReferences = newValue;

            this.OnChanged();
        }
    } 
    public get disableFreightForwarderId() { return this.GetActiveFreightForwarderReferences().filter(x=>x.ForwarderFileConnect).length > 0 }
    public get disableForwarderShipmentNumber() { return this.ForwarderShipmentNumber=='LIST' || (this.GetActiveFreightForwarderReferences().length == 1 &&  this.GetActiveFreightForwarderReferences()[0].ForwarderFileConnect)}



    AddFreightForwarderReferance() {
        var item: FreightForwarderReferencePM = new FreightForwarderReferencePM();
        item.ShipmentId = this.EntityPM.Id;
        item.Tenant = this.EntityPM.Tenant;
        item.ForwarderFileConnect = false;
        item.LineNumber = 1;
        item.ChangeSetOp = "Insert";
        this.EntityPM.FreightForwarderReferences.push(item);
    }

    GetActiveFreightForwarderReferences() {
        return this.FreightForwarderReferences.filter(entity => entity.ChangeSetOp != "Delete");
    }
  

    public get ForwarderShipmentNumber() { 
        let freightForwarderReferences = this.GetActiveFreightForwarderReferences();
        if (freightForwarderReferences.length > 1) {
            return "LIST";
        }
        else {
            return freightForwarderReferences[0]?.ForwarderShipmentNumber;
        }
    }
    public set ForwarderShipmentNumber(newValue: string) {
        let freightForwarderReferences = this.GetActiveFreightForwarderReferences();

        if (freightForwarderReferences.length == 0 || (freightForwarderReferences.length == 1 && freightForwarderReferences[0]?.ForwarderShipmentNumber != newValue)) {

            var index = 0;
            if (freightForwarderReferences.length == 0) {
                this.AddFreightForwarderReferance();
            }
            else {
                index = this.FreightForwarderReferences.indexOf(freightForwarderReferences[0]);
            }

            if (index > -1) {
                this.FreightForwarderReferences[index].ForwarderShipmentNumber = newValue;
                if (!this.FreightForwarderReferences[index].ChangeSetOp) {
                    this.FreightForwarderReferences[index].ChangeSetOp = "Update";
                }
                this.EntityPM.MarkAsDirty("FreightForwarderReferences");
                this.OnChanged();
            }
        }
    }
    //#endregion
    public get ReferantUserId() { return this.EntityPM.ReferantUserId; }
    public set ReferantUserId(newValue: string) {
        if (this.EntityPM.ReferantUserId != newValue) {
            this.EntityPM.ReferantUserId = newValue;
            this.OnChanged();
        }
    }
    public get GrossWeight() { return this.EntityPM.GrossWeight; }
    public set GrossWeight(newValue: number) {
        if (this.EntityPM.GrossWeight != newValue) {
            this.EntityPM.GrossWeight = newValue;
            this.UpdateChargeableWeight();
            this.OnChanged();
        }
    }
    public get ChargeableWeight() { return this.EntityPM.ChargeableWeight; }
    public set ChargeableWeight(newValue: number) {
        if (this.EntityPM.ChargeableWeight != newValue) {
            this.EntityPM.ChargeableWeight = newValue;
            this.OnChanged();
        }
    }
    public get NumberOfPackages() { return this.EntityPM.NumberOfPackages; }
    public set NumberOfPackages(newValue: number) {
        if (this.EntityPM.NumberOfPackages != newValue) {
            this.EntityPM.NumberOfPackages = newValue;
            this.OnChanged();
        }
    }
    public get Volume() { return this.EntityPM.Volume; }
    public set Volume(newValue: number) {
        if (this.EntityPM.Volume != newValue) {
            this.EntityPM.Volume = newValue;
            this.EntityPM.VolumeUnitCode = "CBM";
            this.VolumetricWeight = this.EntityPM.Volume * 1000;

            this.OnChanged();
        }
    }
    public get VolumetricWeight() { return this.EntityPM.VolumetricWeight; }
    public set VolumetricWeight(newValue: number) {
        if (this.EntityPM.VolumetricWeight != newValue) {
            this.EntityPM.VolumetricWeight = newValue;

            this.Volume = this.EntityPM.VolumetricWeight / 1000;
            this.UpdateChargeableWeight();

            this.OnChanged();
        }
    }

    UpdateChargeableWeight() {
        const grossWeight = this.EntityPM.GrossWeight || 0;
        const volumetricWeight = this.EntityPM.VolumetricWeight || 0;
        this.EntityPM.ChargeableWeight = grossWeight > volumetricWeight? this.EntityPM.GrossWeight: this.EntityPM.VolumetricWeight;
    }

    public get SalesmanUserId() { return this.EntityPM.SalesmanUserId; }
    public set SalesmanUserId(newValue: string) {
        if (this.EntityPM.SalesmanUserId != newValue) {
            this.EntityPM.SalesmanUserId = newValue;
            this.OnChanged();
        }
    }
    public get TransportModeId() { return this.EntityPM.TransportModeId; }
    public set TransportModeId(newValue: string) {
        if (this.EntityPM.TransportModeId != newValue) {
            this.EntityPM.TransportModeId = newValue;
            this.OnChanged();
        }
    }

    public get CarrierCode() { return this.EntityPM.CarrierCode; }
    public set CarrierCode(newValue: string) {
        if (this.EntityPM.CarrierCode != newValue) {
            this.EntityPM.CarrierCode = newValue;
            this.OnChanged();
        }
    }
    public get Mawb() { return this.EntityPM.Mawb; }
    public set Mawb(newValue: string) {
        if (this.EntityPM.Mawb != newValue) {
            this.EntityPM.Mawb = newValue;
            this.OnChanged();
        }
    }
    public get MawbDate() { return this.EntityPM.MawbDate; }
    public set MawbDate(newValue: Date) {
        if (this.EntityPM.MawbDate != newValue) {
            this.EntityPM.MawbDate = newValue;
            this.OnChanged();
        }
    }

    public get EstimatedArrivalDate() { return this.EntityPM.EstimatedArrivalDate; }
    public set EstimatedArrivalDate(newValue: Date) {
        if (this.EntityPM.EstimatedArrivalDate != newValue) {
            this.EntityPM.EstimatedArrivalDate = newValue;
            this.OnChanged();
        }
    }
    
    public get ArrivalDate() { return this.EntityPM.ArrivalDate; }
    public set ArrivalDate(newValue: Date) {
        if (this.EntityPM.ArrivalDate != newValue) {
            this.EntityPM.ArrivalDate = newValue;
            this.OnChanged();
        }
    }

    public get PackageTypeCode() { return this.EntityPM.PackageTypeCode; }
    public set PackageTypeCode(newValue: string) {
        if (this.EntityPM.PackageTypeCode != newValue) {
            this.EntityPM.PackageTypeCode = newValue;
            this.OnChanged();
        }
    }

    public get DescriptionOfGoods() { return this.EntityPM.DescriptionOfGoods; }
    public set DescriptionOfGoods(newValue: string) {
        if (this.EntityPM.DescriptionOfGoods != newValue) {
            this.EntityPM.DescriptionOfGoods = newValue;
            this.OnChanged();
        }
    }

    public get Vessel() { return this.EntityPM.Vessel; }
    public set Vessel(newValue: string) {
        if (this.EntityPM.Vessel != newValue) {
            this.EntityPM.Vessel = newValue;
            this.OnChanged();
        }
    }

    public get FlightVoyageNumber() { return this.EntityPM.FlightVoyageNumber; }
    public set FlightVoyageNumber(newValue: string) {
        if (this.EntityPM.FlightVoyageNumber != newValue) {
            this.EntityPM.FlightVoyageNumber = newValue;
            this.OnChanged();
        }
    }
    
    public get Commodity() { return this.EntityPM.Commodity; }
    public set Commodity(newValue: string) {
        if (this.EntityPM.Commodity != newValue) {
            this.EntityPM.Commodity = newValue;
            this.OnChanged();
        }
    }

    public get FreightForwarderId() { return this.EntityPM.FreightForwarderId; }
    public set FreightForwarderId(newValue: string) {
        if (this.EntityPM.FreightForwarderId != newValue) {
            this.EntityPM.FreightForwarderId = newValue;
            this.OnChanged();
        }
    }
    
  
    
}
