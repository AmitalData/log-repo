import { Component, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { VehiclePM } from 'Customs/EntityPMs/VehiclePM';
import { VehicleListService } from 'Customs/Services/StandardLists/VehicleListService';
import { VehiclePMService } from 'Customs/Services/StandardPMs/VehiclePMService';
import { VehicleOwnerPM } from 'Customs/EntityPMs/VehicleOwnerPM';
import { VehicleSafetyAccessoryPM } from 'Customs/EntityPMs/VehicleSafetyAccessoryPM';


@Component({

    templateUrl: './CopyRichbitComponent.html',
})


export class CopyRichbitComponent extends BaseComponent {


    entityListService: EntityListService = new EntityListService();
    DataContext: any = this;
    @Output() onQueryChangeEvent = new EventEmitter();
    filterAgrs: ApiQueryFilters;
    IsDisplayOnly: any; // html component requires this property. AOT
    @Output() Entity: EventEmitter<any> = new EventEmitter();
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    vehiclePMService: VehiclePMService = new VehiclePMService()
    ENntityPM: VehiclePM;
    private CurrentSession = SessionLocator.SelectedSession;
    public VehicleChassisNumberText = TextCodeTranslator.Translate('Customs.Vehicle.F.VehicleChassisNumber') + ":";
    constructor() {
        super();
        
        this.BuildColumns();
        this.onQueryChangeEvent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }
    public SetWindowArgs(args: any) {

        this.EntityPM = args.EntityPM;
        // this.ImporterIdentityId = this.EntityPM.ImporterIdentityId;
        // this.RichbitFileNumber = this.EntityPM.RichbitFileNumber;
        // this.VehicleChassisNumber = this.EntityPM.VehicleChassisNumber;
        //this.VehicleChassisNumber2 = this.EntityPM.VehicleChassisNumber;

      

    }



    private vehicleManufacturerCode: string;
    get VehicleManufacturerCode() { return this.vehicleManufacturerCode; }
    set VehicleManufacturerCode(value: string) {
        if (this.vehicleManufacturerCode != value) {
            this.vehicleManufacturerCode = value;

            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }


    private vehiclePoolTypeCode: string;
    get VehiclePoolTypeCode() { return this.vehiclePoolTypeCode; }
    set VehiclePoolTypeCode(value: string) {
        if (this.vehiclePoolTypeCode != value) {
            this.vehiclePoolTypeCode = value;

            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }


    private modelCode: string;
    get ModelCode() { return this.modelCode; }
    set ModelCode(value: string) {
        if (this.modelCode != value) {
            this.modelCode = value;

            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    private vehicleChassisNumber: string;
    get VehicleChassisNumber() { return this.vehicleChassisNumber; }
    set VehicleChassisNumber(value: string) {
        if (this.vehicleChassisNumber != value) {
            this.vehicleChassisNumber = value;

            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    private vehicleChassisNumber2: string;
    get VehicleChassisNumber2() { return this.vehicleChassisNumber2; }
    set VehicleChassisNumber2(value: string) {
        if (this.vehicleChassisNumber2 != value) {
            this.vehicleChassisNumber2 = value; this.VehicleChassisNumber = value;
            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    private richbitFileNumber: string;
    get RichbitFileNumber() { return this.richbitFileNumber; }
    set RichbitFileNumber(value: string) {
        if (this.richbitFileNumber != value) {
            this.richbitFileNumber = value;


            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    private importerIdentityId: string;
    get ImporterIdentityId() { return this.importerIdentityId; }
    set ImporterIdentityId(value: string) {

        if (this.importerIdentityId != value) {

            this.importerIdentityId = value;


            this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });
        }
    }

    searchValue: string;
    Search(value: string) {
        this.searchValue = value;
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() });

    }

    DataSource = {
        pageSize: 30,
        rowCount: null,
        //sortingCol: "CreateDateTime",
        sortingDir: "Ascending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;
        },
    };

    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        var filters = new ApiQueryFilters();

        filters.GetAll = false;
        filters.GetCount = true;
        filters.PageSize = 20;


        if (!AppTool.IsNullOrEmpty(this.ImporterIdentityId)) {

            filters.addAdditionalFilter("ImporterIdentityId", this.ImporterIdentityId, null, null, "Equals", false, false, false, "string");

        }

        if (!AppTool.IsNullOrEmpty(this.RichbitFileNumber)) {

            filters.addAdditionalFilter("RichbitFileNumber", this.RichbitFileNumber, null, null, "Equals", false, false, false, "string");

        }
        

        if (!AppTool.IsNullOrEmpty(this.VehicleChassisNumber)) {

            filters.addAdditionalFilter("VehicleChassisNumber", this.VehicleChassisNumber, null, null, "Equals", false, false, false, "string");

        }



        // this.CurrentSession.StopBusyIndicator();
        return this.entityListService.getByFilters("Customs.Vehicle", filters);


    }

    public columns: any[] = null;
    BuildColumns() {
        this.columns = [];
        this.columns.push({
            FieldName: 'ImporterName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Vehicle.F.ImporterName'),
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'RichbitFileNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Vehicle.F.RichbitFileNumber'),
            Styles: { width: '120px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',

            IsCustomTemplate: true

        });
        this.columns.push({
            FieldName: 'VehicleChassisNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Vehicle.F.VehicleChassisNumber'),
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true

        });
        this.columns.push({
            FieldName: 'ModelCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Vehicle.F.ModelCode'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',

        });
        this.columns.push({
            FieldName: 'VehiclePoolTypeCode',
            DataTypeCode: 'string',
            Display: TextCodeTranslator.Translate('Customs.Vehicle.F.VehiclePoolTypeCode'),
            Styles: { width: '105px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true
        });

        this.columns.push({
            FieldName: 'VehicleManufacturerCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Vehicle.F.VehicleManufacturerCode'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
        });




    }





    public SelectedRow: any = null;

    OnRowSelected(item) {
        
        this.SelectedRow = item.rowData;
        let confirm = new ConfirmWindow();
        confirm.WindowClosed.subscribe((event: any) => {
            if (confirm.Yes) {
                this.CopyRichbitDetails(this.SelectedRow)
            }
            else {
                confirm.Close();
            }

        });
        var message = this.SelectedRow.RichbitFileNumber == null ? this.SelectedRow.VehicleChassisNumber : this.SelectedRow.RichbitFileNumber;
        confirm.Show(TextCodeTranslator.Translate("Customs.Vehicle.O.OkCopyRichbit") + " " + message);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }
    CopyRichbitDetails(selectedRow: any) {

        this.CurrentSession.StartBusyIndicatorSaving();
        this.vehiclePMService.get(selectedRow.Id).subscribe(r => {
            
            if (r.Result) {
                 var row = r.Result;
                this.EntityPM.VehiclePoolTypeCode = row?.VehiclePoolTypeCode
                this.EntityPM.VehiclePriceListTypeCode = row?.VehiclePriceListTypeCode
                this.EntityPM.VehicleManufacturerCode = row?.VehicleManufacturerCode
                this.EntityPM.ModelCode = row?.ModelCode
                this.EntityPM.IsABS = row?.IsABS;
                this.EntityPM.AirBagsNumber = row?.AirBagsNumber;
                this.EntityPM.ConverterTypeCode = row?.ConverterTypeCode;
                this.EntityPM.IsArmoredVehicle = row?.IsArmoredVehicle;
                this.EntityPM.IsLoweringVehicleForInvalid = row?.IsLoweringVehicleForInvalid;
                this.EntityPM.GreenIndex = row?.GreenIndex;
                this.EntityPM.GreenIndexGroup = row?.GreenIndexGroup;
                this.EntityPM.IsStabilityControl = row?.IsStabilityControl;
                this.EntityPM.IsraelEnterDate = row?.IsraelEnterDate;
                this.EntityPM.EngineCapacity = row?.EngineCapacity;
                this.EntityPM.VehiclePowerKW = row?.VehiclePowerKW;
                this.EntityPM.VehicleTecnologyTypeCode = row?.VehicleTecnologyTypeCode;
                this.EntityPM.FuelTypeCode = row?.FuelTypeCode;
                this.EntityPM.ManufactureCountryCode = row?.ManufactureCountryCode;
                this.EntityPM.MedalNumber = row?.MedalNumber;
                this.EntityPM.CommercialNickname = row?.CommercialNickname;
                this.EntityPM.ModelDescription = row?.ModelDescription;
                this.EntityPM.NumberOfSeats = row?.NumberOfSeats;
                this.EntityPM.TotalVehicleWeight = row?.TotalVehicleWeight;
                this.EntityPM.SelfVehicleWeight = row?.SelfVehicleWeight;
                this.EntityPM.NumberOfWheels = row?.NumberOfWheels;
                this.EntityPM.VehicleManufactureDate = row?.VehicleManufactureDate;
                this.EntityPM.VehicleTypeCode = row?.VehicleTypeCode;
                this.EntityPM.TransmissionDateWithoutTax = row?.TransmissionDateWithoutTax;
                this.EntityPM.ImporterIdentityId = row?.ImporterIdentityId;
                this.EntityPM.DateOnRoadAbroad = row?.DateOnRoadAbroad;
                this.EntityPM.VehicleSafetyAccessoryPoints = row?.VehicleSafetyAccessoryPoints;
                this.EntityPM.StatusCode = row?.StatusCode;
                this.EntityPM.ConcurrencyGUID = row?.ConcurrencyGUID;
                this.EntityPM.IsThreeWheeledForReduction = row?.IsThreeWheeledForReduction;
                this.EntityPM.TaxiMedalOwner = row?.TaxiMedalOwner;
                this.EntityPM.ImporterPassportTypeCode = row?.ImporterPassportTypeCode;
                this.EntityPM.IsCBS = row?.IsCBS;
                this.EntityPM.IsSlipperClutch = row?.IsSlipperClutch;
                this.EntityPM.IsSteeringDamper = row?.IsSteeringDamper;
                this.EntityPM.IsTCS = row?.IsTCS;
                this.EntityPM.IsTPS = row?.IsTPS;
                this.EntityPM.VehicleCategory = row?.VehicleCategory;
                this.EntityPM.VehicleMaxPowerKW = row?.VehicleMaxPowerKW;
                var index=0;
                
                row.VehicleSafetyAccessories.forEach(element => {
                    index=0;
                    var VehicleSafetyAccessory = new VehicleSafetyAccessoryPM(this.EntityPM);
                    VehicleSafetyAccessory.VehicleId = this.EntityPM.Id;
                    VehicleSafetyAccessory.Tenant = element?.Tenant,
                    VehicleSafetyAccessory.LineNumber = element.LineNumber,
                    VehicleSafetyAccessory.VehicleSafetyAccessoryCode = element?.VehicleSafetyAccessoryCode,
                    VehicleSafetyAccessory.VehicleSafetyAccessoryName = element?.VehicleSafetyAccessoryName,
                    VehicleSafetyAccessory.VehicleSafAccessoryInstlTypCod = element?.VehicleSafAccessoryInstlTypCod,
                    VehicleSafetyAccessory.VehicleSafAccessoryInstlTypName = element?.VehicleSafAccessoryInstlTypName

                   
                    this.EntityPM.VehicleSafetyAccessories[index]=VehicleSafetyAccessory;index++;
                });



            }

            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindowEmit(null);
        });




        //StatusCode,ConcurrencyGUID,TaxiMedalOwner

    }
}
