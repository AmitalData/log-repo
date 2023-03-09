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
    ENntityPM: VehiclePM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

        super();
        this.EntityPM = this;
        //this.CurrentSession.StartBusyIndicatorLoading();
        this.BuildColumns();

        //this.SetWindowArgs(this.CurrentSession.CurrentEditComponent);
        this.onQueryChangeEvent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });

    }
    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        
        this.EntityPM = args.EntityPM;
        this.ImporterIdentityId = this.EntityPM.ImporterIdentityId;
        this.RichbitFileNumber = this.EntityPM.RichbitFileNumber;
        this.VehicleChassisNumber = this.EntityPM.VehicleChassisNumber;

        // this.cdr.detectChanges();

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
            FieldName: 'ImporterIdentityId',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate('Customs.Vehicle.F.ImporterIdentityId'),
            Styles: { width: '120px' },
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
        confirm.Show(TextCodeTranslator.Translate("Customs.Vehicle.O.OkCopyRichbit") + " " + this.SelectedRow.RichbitFileNumber);
    }


    CopyRichbitDetails(selectedRow:any){
        this.CurrentSession.StartBusyIndicatorSaving();
            
        this.EntityPM.VehiclePoolTypeCode=selectedRow?.VehiclePoolTypeCode
        this.EntityPM.VehiclePriceListTypeCode=selectedRow?.VehiclePriceListTypeCode
        this.EntityPM.VehicleManufacturerCode=selectedRow?.VehicleManufacturerCode
        this.EntityPM.ModelCode=selectedRow?.ModelCode
        this.EntityPM.IsABS=selectedRow?.IsABS;
        this.EntityPM.AirBagsNumber=selectedRow?.AirBagsNumber;
        this.EntityPM.ConverterTypeCode=selectedRow?.ConverterTypeCode;
        this.EntityPM.IsArmoredVehicle=selectedRow?.IsArmoredVehicle;
        this.EntityPM.IsLoweringVehicleForInvalid=selectedRow?.IsLoweringVehicleForInvalid;
        this.EntityPM.GreenIndex=selectedRow?.GreenIndex;
        this.EntityPM.GreenIndexGroup=selectedRow?.GreenIndexGroup;
        this.EntityPM.IsStabilityControl=selectedRow?.IsStabilityControl;
        this.EntityPM.IsraelEnterDate=selectedRow?.IsraelEnterDate;
        this.EntityPM.EngineCapacity=selectedRow?.EngineCapacity;
        this.EntityPM.VehiclePowerKW=selectedRow?.VehiclePowerKW;
        this.EntityPM.VehicleTecnologyTypeCode=selectedRow?.VehicleTecnologyTypeCode;
        this.EntityPM.FuelTypeCode=selectedRow?.FuelTypeCode;
        this.EntityPM.ManufactureCountryCode=selectedRow?.ManufactureCountryCode;
        this.EntityPM.MedalNumber=selectedRow?.MedalNumber;
        this.EntityPM.CommercialNickname=selectedRow?.CommercialNickname;
        this.EntityPM.ModelDescription=selectedRow?.ModelDescription;
        this.EntityPM.NumberOfSeats=selectedRow?.NumberOfSeats;
        this.EntityPM.TotalVehicleWeight=selectedRow?.TotalVehicleWeight;
        this.EntityPM.SelfVehicleWeight=selectedRow?.SelfVehicleWeight;
        this.EntityPM.NumberOfWheels=selectedRow?.NumberOfWheels;
        this.EntityPM.VehicleManufactureDate=selectedRow?.VehicleManufactureDate;
        this.EntityPM.VehicleTypeCode=selectedRow?.VehicleTypeCode;
        this.EntityPM.TransmissionDateWithoutTax=selectedRow?.TransmissionDateWithoutTax;
        this.EntityPM.ImporterIdentityId=selectedRow?.ImporterIdentityId;
        this.EntityPM.DateOnRoadAbroad=selectedRow?.DateOnRoadAbroad;
        this.EntityPM.VehicleSafetyAccessoryPoints=selectedRow?.VehicleSafetyAccessoryPoints;
        this.EntityPM.StatusCode=selectedRow?.StatusCode;
        this.EntityPM.ConcurrencyGUID=selectedRow?.ConcurrencyGUID;
        this.EntityPM.IsThreeWheeledForReduction=selectedRow?.IsThreeWheeledForReduction;
        this.EntityPM.TaxiMedalOwner=selectedRow?.TaxiMedalOwner;
        this.EntityPM.ImporterPassportTypeCode=selectedRow?.ImporterPassportTypeCode;
        this.EntityPM.IsCBS=selectedRow?.IsCBS;
        this.EntityPM.IsSlipperClutch=selectedRow?.IsSlipperClutch;
        this.EntityPM.IsSteeringDamper=selectedRow?.IsSteeringDamper;
        this.EntityPM.IsTCS=selectedRow?.IsTCS;
        this.EntityPM.IsTPS=selectedRow?.IsTPS;
        this.EntityPM.VehicleCategory=selectedRow?.VehicleCategory;
        this.EntityPM.VehicleMaxPowerKW=selectedRow?.VehicleMaxPowerKW;
      
        this.CurrentSession.StopBusyIndicator();


//StatusCode,ConcurrencyGUID,TaxiMedalOwner

    }
}
