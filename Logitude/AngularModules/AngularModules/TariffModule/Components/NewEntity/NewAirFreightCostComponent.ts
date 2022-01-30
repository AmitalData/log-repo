import {Component, OnInit} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { TariffPM } from '../../EntityPMs/TariffPM';
import { TariffPMService } from '../../Services/StandardPMs/TariffPMService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool } from '../../../Infrastructure/Tools';
import { ChargesTypeListService } from '../../../Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypeList } from '../../../Common/EntityLists/ChargesTypeList';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator'
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TariffSettingPM } from '../../../TariffModule/EntityPMs/TariffSettingPM';
import { TariffDomainService } from '../../../TariffModule/Services/TariffDomainService';
import { PackageTypeList } from '../../../Common/EntityLists/PackageTypeList';
import { PackageTypeListService } from '../../../Common/Services/StandardLists/PackageTypeListService';
import { CommonDomainService } from '../../../Common/Services/CommonDomainService';
import { TariffVersionPM } from '../../EntityPMs/TariffVersionPM';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { TariffVersionAllInChargePM } from '../../EntityPMs/TariffVersionAllInChargePM';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'NewAirFreightCostComponent',    
    templateUrl: './NewAirFreightCostComponent.html',
})

export class NewAirFreightCostComponent extends BaseComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public ObjectTableName = "Tariff";
    public EntityPM: TariffPM;
    public SelectedLocationFilter: any;
    public VisibileSurchargesArea: boolean = false;
    public VisibleFCLFreightArea: boolean = false;
    public VisibleContainerTypeAreaInOFS: boolean = false;
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public FreightChargeTypesQueryFilters: ApiQueryFilters;
    public MeasurementsQueryFilters: ApiQueryFilters;
    private chargesTypePMService: ChargesTypeListService;
    private packageTypeListService: PackageTypeListService;
    private IdProps: string[] = [];
    private UOMProps: string[] = [];
    private ContainerTypesProperties: string[] = [];
    private myService: TariffPMService;
    public PriceSteps: string;
    public PriceStepsText: string;
    public TariffCurrencyTextCode: string;
    public SellerDependancy: string = "AL";
    public CustomsBrokerDependancy: string = "AG,CG";
    public HasAContainerTypeUOM: boolean = false;
    private firstVersion: TariffVersionPM;
    public IsSellerVisible: boolean = false;
    public IsCustomsBrokerVisible: boolean = false;
    constructor() {
        super();
        this.myService = new TariffPMService();
        this.chargesTypePMService = new ChargesTypeListService();
        this.packageTypeListService = new PackageTypeListService();
        this.EntityPM = this.myService.GetNewEntityPM();
       
        this.FillChargesIDsAndUOMS();
        this.FillContainerTypeIds();     
    }

    ngOnInit() {
        this.GetTenantTariffSetting();
        this.GetBCNTMeasurementId();
    }

    private SetDefaultFreightChargeId() {
        var chargeCode = 'OFT';
        if (this.EntityPM.TypeCode == 'AFC') {
            chargeCode = 'AFT';
        }

        this.chargesTypePMService.getAllFromCache().subscribe(p => {
            var chargeType: ChargesTypeList = p.Result.filter(p => p.Code == chargeCode)[0];
            if (chargeType != null) {
                this.FreightChargeId = chargeType.Id;
            }
        });
    }

    private BCNTmeasurementId: string;
    private GetBCNTMeasurementId() {
        if (this.EntityPM.TypeCode == "OFS") {
            var commonDomainService: CommonDomainService = new CommonDomainService();
            commonDomainService.GetMeasurementIdByCode("BCNT").subscribe((res: any) => {
                if (!res.HasError) {
                    if (res.Result) {
                        this.BCNTmeasurementId = res.Result;
                    }
                }
            });
        }
    }

    SetWindowArgs(args) {
        this.EntityPM.TypeCode = args.TypeCode;

        if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
            this.VisibileSurchargesArea = true;
            this.TariffCurrencyTextCode = "Tariff.O.DefaultCurrency";
        }
        else if (this.EntityPM.TypeCode == "OFC") {
            this.VisibleFCLFreightArea = true;
            this.HasAContainerTypeUOM = true;
        }
        else {
            this.VisibileSurchargesArea = false;
            this.TariffCurrencyTextCode = "Tariff.F.CurrencyId";
        }

        if (this.EntityPM.TypeCode == "OFS") {
            this.VisibleContainerTypeAreaInOFS = true;
            this.HasAContainerTypeUOM = false;
        }
        this.SetDefaultFreightChargeId();
        this.BuildQueryFilters();
        this.BuildFreightChargesQueryFilters();
        this.SetUIProperties();
        this.CreateFirstVersion();
    }

    private CreateFirstVersion() {
        this.firstVersion = new TariffVersionPM(this.EntityPM);
        this.firstVersion.Tenant = InfraSettings.TenantPM.Id;
        this.firstVersion.Version = 1;
        this.firstVersion.IsDraft = true;
    }

    SetUIProperties() {
        var isDatesVisible: boolean = true;
        if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC") {
            isDatesVisible = false;
        }

        this.UIProperties.SetVisibility("StartDate", this.ObjectTableName, isDatesVisible);
        this.UIProperties.SetVisibility("ExpirationDate", this.ObjectTableName, isDatesVisible);

        this.UIProperties.SetRequired("StartDate", this.ObjectTableName, this.StartDate == null)
        this.UIProperties.SetRequired("ExpirationDate", this.ObjectTableName, false);

        if (this.EntityPM.TypeCode == 'AFC') {
            this.UIProperties.SetRequired("TariffProductId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.TariffProductId));
        }

        this.SetUIProperties_FreightCharges();
        this.SetUIProperties_Seller();
        this.SetUIProperties_CustomsBroker();
    }
    private SetUIProperties_FreightCharges() {
        var isFreightChargeVisible: boolean = false;
        if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OFC") {
            isFreightChargeVisible = true;
            this.UIProperties.SetRequired("FreightChargeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.FreightChargeId));
        }
        this.UIProperties.SetVisibility("FreightChargeId", this.ObjectTableName, isFreightChargeVisible);
    }
    private SetUIProperties_Seller() {
        var isSellerVisible: boolean = true;
        var isSellerRequired: boolean = false;

        if (this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
            isSellerVisible = false;
        }

        if (isSellerVisible) {
            isSellerRequired = AppTool.IsNullOrEmpty(this.SellerId);
        }

        this.IsSellerVisible = isSellerVisible;
        this.UIProperties.SetVisibility("SellerId", this.ObjectTableName, isSellerVisible);
        this.UIProperties.SetRequired("SellerId", this.ObjectTableName, isSellerRequired)
    }
    private SetUIProperties_CustomsBroker() {
        var isBrokerVisible: boolean = false;
        var isBrokerRequired: boolean = false;

        if (this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
            isBrokerVisible = true;
        }

        if (isBrokerVisible) {
            isBrokerRequired = AppTool.IsNullOrEmpty(this.CustomsBrokerId);
        }

        this.IsCustomsBrokerVisible = isBrokerVisible;
        this.UIProperties.SetVisibility("CustomsBrokerId", this.ObjectTableName, isBrokerVisible);
        this.UIProperties.SetRequired("CustomsBrokerId", this.ObjectTableName, isBrokerRequired)
    }

    FillChargesIDsAndUOMS() {
        for (var index = 1; index <= 10; index++) {
            this.IdProps.push("Surcharge" + index + "Id");
            this.UOMProps.push("Surcharge" + index + "UOM");
        }
    }

    FillContainerTypeIds() {
        for (var index = 1; index <= 5; index++) {
            this.ContainerTypesProperties.push("ContainerType" + index + "Id");
        }
    }

    BuildQueryFilters() {
        var EntityType: string = "IsAir";
        if (this.EntityPM.TypeCode == "OFC" || this.EntityPM.TypeCode == "OFS") {
            EntityType = "IsOcean";
            this.SellerDependancy = "SL";
        }

        else if (this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OLC") {
            EntityType = "IsOcean";
            this.SellerDependancy = "SL,AG,SG";
        }

        if (this.EntityPM.TypeCode == "ECC" || this.EntityPM.TypeCode == "ICC") {
            EntityType = this.EntityPM.TypeCode == "ECC" ? "IsExport" : "IsImport";
        }

        this.MeasurementsQueryFilters = new ApiQueryFilters();

        if (this.EntityPM.TypeCode == "OFS") {
            this.MeasurementsQueryFilters.addAdditionalFilter("Code", "BCNT,BTEU,FIXD", null, null, "InList", false, true, false, "string", false, true, true);
        }
        else {
            this.MeasurementsQueryFilters.addAdditionalFilter("Code", "STFE", null, null, "NotContains", false, false, false, "string", false, true, true);
        }

        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter(EntityType, true, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("ChargesGroupCode", "FRT", null, null, "NotEqual", false, false, false, "string");
        this.Validate(true);
        this.SetContainerTypeUIProperties(true);
    }

    BuildFreightChargesQueryFilters() {
        this.FreightChargeTypesQueryFilters = new ApiQueryFilters();
        var EntityType: string = "IsAir";
        if (this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OFC") {
            EntityType = "IsOcean";
        }
        this.FreightChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.FreightChargeTypesQueryFilters.addAdditionalFilter(EntityType, true, null, null, "Equals", false, false, false, "Boolean");
    }

    get Name() {
        return this.EntityPM.Name;
    }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get StartDate() {
        return this.EntityPM.StartDate;
    }
    set StartDate(value: Date) {
        if (this.EntityPM.StartDate != value) {
            this.EntityPM.StartDate = value;

            this.firstVersion.StartDate = value;
            this.SetUIProperties();
        }
    }

    get ExpirationDate() {
        return this.EntityPM.ExpirationDate;
    }
    set ExpirationDate(value: Date) {
        if (this.EntityPM.ExpirationDate != value) {
            this.EntityPM.ExpirationDate = value;

            this.firstVersion.ExpirationDate = value;
            this.SetUIProperties();
        }
    }

    get Notes() {
        return this.EntityPM.Notes;
    }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    get CurrencyId() {
        return this.EntityPM.CurrencyId;
    }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
        }
    }

    get SellerId() {
        return this.EntityPM.SellerId;
    }
    set SellerId(value: string) {
        if (this.EntityPM.SellerId != value) {
            this.EntityPM.SellerId = value;

            this.SetUIProperties_Seller();
        }
    }

    get CustomsBrokerId() {
        return this.EntityPM.CustomsBrokerId;
    }
    set CustomsBrokerId(value: string) {
        if (this.EntityPM.CustomsBrokerId != value) {
            this.EntityPM.CustomsBrokerId = value;

            this.SetUIProperties_CustomsBroker();
        }
    }

    get ContractNumber() {
        return this.EntityPM.ContractNumber;
    }
    set ContractNumber(value: string) {
        if (this.EntityPM.ContractNumber != value) {
            this.EntityPM.ContractNumber = value;
        }
    }

    get Surcharge1Id() {
        return this.EntityPM.Surcharge1Id;
    }
    set Surcharge1Id(value: string) {
        if (this.EntityPM.Surcharge1Id != value) {
            this.EntityPM.Surcharge1Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(0);
            }
        }
    }

    get Surcharge2Id() {
        return this.EntityPM.Surcharge2Id;
    }
    set Surcharge2Id(value: string) {
        if (this.EntityPM.Surcharge2Id != value) {
            this.EntityPM.Surcharge2Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(1);
            }
        }
    }

    get Surcharge3Id() {
        return this.EntityPM.Surcharge3Id;
    }
    set Surcharge3Id(value: string) {
        if (this.EntityPM.Surcharge3Id != value) {
            this.EntityPM.Surcharge3Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(2);
            }
        }
    }

    get Surcharge4Id() {
        return this.EntityPM.Surcharge4Id;
    }
    set Surcharge4Id(value: string) {
        if (this.EntityPM.Surcharge4Id != value) {
            this.EntityPM.Surcharge4Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(3);
            }
        }
    }

    get Surcharge5Id() {
        return this.EntityPM.Surcharge5Id;
    }
    set Surcharge5Id(value: string) {
        if (this.EntityPM.Surcharge5Id != value) {
            this.EntityPM.Surcharge5Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(4);
            }
        }
    }

    get Surcharge6Id() {
        return this.EntityPM.Surcharge6Id;
    }
    set Surcharge6Id(value: string) {
        if (this.EntityPM.Surcharge6Id != value) {
            this.EntityPM.Surcharge6Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(5);
            }
        }
    }

    get Surcharge7Id() {
        return this.EntityPM.Surcharge7Id;
    }
    set Surcharge7Id(value: string) {
        if (this.EntityPM.Surcharge7Id != value) {
            this.EntityPM.Surcharge7Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(6);
            }
        }
    }

    get Surcharge8Id() {
        return this.EntityPM.Surcharge8Id;
    }
    set Surcharge8Id(value: string) {
        if (this.EntityPM.Surcharge8Id != value) {
            this.EntityPM.Surcharge8Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(7);
            }
        }
    }

    get Surcharge9Id() {
        return this.EntityPM.Surcharge9Id;
    }
    set Surcharge9Id(value: string) {
        if (this.EntityPM.Surcharge9Id != value) {
            this.EntityPM.Surcharge9Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(8);
            }
        }
    }

    get Surcharge10Id() {
        return this.EntityPM.Surcharge10Id;
    }
    set Surcharge10Id(value: string) {
        if (this.EntityPM.Surcharge10Id != value) {
            this.EntityPM.Surcharge10Id = value;
            this.Validate();
            if (value != null) {
                this.SetDefaultUOM(9);
            }
        }
    }

    get Surcharge1UOM() {
        return this.EntityPM.Surcharge1UOM;
    }
    set Surcharge1UOM(value: string) {
        if (this.EntityPM.Surcharge1UOM != value) {
            this.EntityPM.Surcharge1UOM = value;
            this.Validate();
        }
    }

    get Surcharge2UOM() {
        return this.EntityPM.Surcharge2UOM;
    }
    set Surcharge2UOM(value: string) {
        if (this.EntityPM.Surcharge2UOM != value) {
            this.EntityPM.Surcharge2UOM = value;
            this.Validate();
        }
    }

    get Surcharge3UOM() {
        return this.EntityPM.Surcharge3UOM;
    }
    set Surcharge3UOM(value: string) {
        if (this.EntityPM.Surcharge3UOM != value) {
            this.EntityPM.Surcharge3UOM = value;
            this.Validate();
        }
    }

    get Surcharge4UOM() {
        return this.EntityPM.Surcharge4UOM;
    }
    set Surcharge4UOM(value: string) {
        if (this.EntityPM.Surcharge4UOM != value) {
            this.EntityPM.Surcharge4UOM = value;
            this.Validate();
        }
    }

    get Surcharge5UOM() {
        return this.EntityPM.Surcharge5UOM;
    }
    set Surcharge5UOM(value: string) {
        if (this.EntityPM.Surcharge5UOM != value) {
            this.EntityPM.Surcharge5UOM = value;
            this.Validate();
        }
    }

    get Surcharge6UOM() {
        return this.EntityPM.Surcharge6UOM;
    }
    set Surcharge6UOM(value: string) {
        if (this.EntityPM.Surcharge6UOM != value) {
            this.EntityPM.Surcharge6UOM = value;
            this.Validate();
        }
    }

    get Surcharge7UOM() {
        return this.EntityPM.Surcharge7UOM;
    }
    set Surcharge7UOM(value: string) {
        if (this.EntityPM.Surcharge7UOM != value) {
            this.EntityPM.Surcharge7UOM = value;
            this.Validate();
        }
    }

    get Surcharge8UOM() {
        return this.EntityPM.Surcharge8UOM;
    }
    set Surcharge8UOM(value: string) {
        if (this.EntityPM.Surcharge8UOM != value) {
            this.EntityPM.Surcharge8UOM = value;
            this.Validate();
        }
    }

    get Surcharge9UOM() {
        return this.EntityPM.Surcharge9UOM;
    }
    set Surcharge9UOM(value: string) {
        if (this.EntityPM.Surcharge9UOM != value) {
            this.EntityPM.Surcharge9UOM = value;
            this.Validate();
        }
    }

    get Surcharge10UOM() {
        return this.EntityPM.Surcharge10UOM;
    }
    set Surcharge10UOM(value: string) {
        if (this.EntityPM.Surcharge10UOM != value) {
            this.EntityPM.Surcharge10UOM = value;
            this.Validate();
        }
    }

    get ContainerType1Id() {
        return this.EntityPM.ContainerType1Id;
    }
    set ContainerType1Id(value: string) {
        if (this.EntityPM.ContainerType1Id != value) {
            this.EntityPM.ContainerType1Id = value;
            this.SetContainerTypeUIProperties();
        }
    }

    get ContainerType2Id() {
        return this.EntityPM.ContainerType2Id;
    }
    set ContainerType2Id(value: string) {
        if (this.EntityPM.ContainerType2Id != value) {
            this.EntityPM.ContainerType2Id = value;
            this.SetContainerTypeUIProperties();
        }
    }

    get ContainerType3Id() {
        return this.EntityPM.ContainerType3Id;
    }
    set ContainerType3Id(value: string) {
        if (this.EntityPM.ContainerType3Id != value) {
            this.EntityPM.ContainerType3Id = value;
            this.SetContainerTypeUIProperties();
        }
    }

    get ContainerType4Id() {
        return this.EntityPM.ContainerType4Id;
    }
    set ContainerType4Id(value: string) {
        if (this.EntityPM.ContainerType4Id != value) {
            this.EntityPM.ContainerType4Id = value;
            this.SetContainerTypeUIProperties();
        }
    }

    get ContainerType5Id() {
        return this.EntityPM.ContainerType5Id;
    }
    set ContainerType5Id(value: string) {
        if (this.EntityPM.ContainerType5Id != value) {
            this.EntityPM.ContainerType5Id = value;
            this.SetContainerTypeUIProperties();
        }
    }

    get TariffProductId() {
        return this.EntityPM.TariffProductId;
    }
    set TariffProductId(value: string) {
        if (this.EntityPM.TariffProductId != value) {
            this.EntityPM.TariffProductId = value;
            this.SetUIProperties();
        }

    }
    get FreightChargeId() {
        return this.EntityPM.FreightChargeId;
    }
    set FreightChargeId(value: string) {
        if (this.EntityPM.FreightChargeId != value) {
            this.EntityPM.FreightChargeId = value;
            this.SetUIProperties();
        }
    }

    Validate(initial: boolean = false) {
        for (var index = 1; index <= 10; index++) {
            if (initial) {
                if (index != 1) {
                    this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, false);
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, true);
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                    this.UIProperties.SetRequired(this.IdProps[index - 1], this.ObjectTableName, true);
                    this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, true);
                }
            }

            if (!initial) {
                if (AppTool.IsNullOrEmpty(this[this.IdProps[index - 1]])) {
                    this[this.UOMProps[index - 1]] = null;
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);

                    if (index > 1) {
                        if (!AppTool.IsNullOrEmpty(this[this.UOMProps[index - 2]]) && !AppTool.IsNullOrEmpty(this[this.IdProps[index - 2]])) {
                            this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, true);
                            this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                        }
                    }

                    else {
                        this.UIProperties.SetRequired(this.IdProps[index - 1], this.ObjectTableName, true);
                        this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, true);
                    }
                }

                else {
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, true);

                    if (index == 1) {
                        this.UIProperties.SetRequired(this.IdProps[index - 1], this.ObjectTableName, false);
                        if (AppTool.IsNullOrEmpty(this[this.UOMProps[index - 1]])) {
                            this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, true);
                        }
                        else {
                            this.UIProperties.SetRequired(this.UOMProps[index - 1], this.ObjectTableName, false);
                        }
                    }
                }
            }
        }
    }

    SetContainerTypeUIProperties(initial: boolean = false) {
        for (var index = 1; index <= 5; index++) {
            if (initial) {
                if (index != 1) {
                    this.UIProperties.SetEnabled(this.ContainerTypesProperties[index - 1], this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetEnabled(this.ContainerTypesProperties[index - 1], this.ObjectTableName, true);
                    this.UIProperties.SetRequired(this.ContainerTypesProperties[index - 1], this.ObjectTableName, true);
                }
            }
            if (!initial) {
                if (AppTool.IsNullOrEmpty(this[this.ContainerTypesProperties[index - 1]])) {

                    if (index > 1) {
                        if (!AppTool.IsNullOrEmpty(this[this.ContainerTypesProperties[index - 2]])) {
                            this.UIProperties.SetEnabled(this.ContainerTypesProperties[index - 1], this.ObjectTableName, true);
                        }
                    }
                    else {
                        this.UIProperties.SetRequired(this.ContainerTypesProperties[index - 1], this.ObjectTableName, true);
                    }
                }
                else {
                    if (index == 1 && this.HasAContainerTypeUOM) {
                        this.UIProperties.SetRequired(this.ContainerTypesProperties[index - 1], this.ObjectTableName, false);
                    }
                }
            }
        }
    }

    SetDefaultUOM(index: number) {
        this.chargesTypePMService.getSingleFromCache(this[this.IdProps[index]]).subscribe((res: any) => {
            if (!res.HasError) {
                if (res.Result) {
                    var ChargesType: ChargesTypeList = res.Result;
                    if (this.EntityPM.TypeCode == "OFS") {
                        if (!AppTool.IsNullOrEmpty(ChargesType.ContainerMeasurementId)) {
                            this[this.UOMProps[index]] = ChargesType.ContainerMeasurementId;
                        }
                        else {
                            this[this.UOMProps[index]] = this.BCNTmeasurementId;
                        }
                    }
                    else {
                        this[this.UOMProps[index]] = ChargesType.MeasurementId;
                    }
                }
            }
        });
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[] = [];

    ValidateSurcharge() {
        var IdProps: string[] = [];
        var UOMProps: string[] = [];
        var IdPropsName: string[] = [];
        var UOMPropsName: string[] = [];
        var DuplicatedChargesIds: string[] = [];
        var EmptyIndex = 1;
        var emptyLines: boolean = false;
        var FirstLineEmpty: boolean = false;
        var tempErrors: Array<string> = [];
        this.HasAContainerTypeUOM = false;

        for (var index = 1; index <= 10; index++) {
            IdProps.push("Surcharge" + index + "Id");
            UOMProps.push("Surcharge" + index + "UOM");
            IdPropsName.push("Charge Type " + index);
            UOMPropsName.push("UOM " + index);

            if (this.IdProps.filter(p => this[p + ""] == this[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && this[IdProps[index - 1]] != null)[0] != null) {
                var chargresType = this.IdProps.filter(p => this[p + ""] == this[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && this[IdProps[index - 1]] != null)[0];
                if (!DuplicatedChargesIds.includes(this[chargresType + ""])) {
                    DuplicatedChargesIds.push(this[chargresType + ""]);
                    this.chargesTypePMService.getSingleFromCache(this[chargresType + ""]).subscribe((res: any) => {
                        if (!res.HasError) {
                            var chargesTypeList: ChargesTypeList = res.Result;
                            if (res) {
                                this.ValidationErrorsList.push("Charge type " + chargesTypeList.EnglishName + " is duplicated");
                            }
                        }
                    });
                }
            }

            if (index == 1) {
                if (AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    tempErrors.push(IdPropsName[index - 1] + " is required");
                    FirstLineEmpty = true;
                }

                if (this.EntityPM.TypeCode != "OFS") {
                    if (AppTool.IsNullOrEmpty(this[UOMProps[index - 1]])) {
                        tempErrors.push(UOMPropsName[index - 1] + " is required");
                    }
                }
            }

            else {
                if (AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    if (EmptyIndex == 1) {
                        EmptyIndex = index;
                    }
                }

                if (AppTool.IsNullOrEmpty(this[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                    this.ValidationErrorsList.push(IdPropsName[index - 1] + " is filled without a UOM");
                }

                if (index == 2) {
                    if (!AppTool.IsNullOrEmpty(this[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                        if (FirstLineEmpty) {
                            emptyLines = true;
                            this.ValidationErrorsList.push("Empty Charge Lines aren't allowed between line 1 and line 2");
                            EmptyIndex = 1;
                        }
                    }
                }

                if (index >= 3) {
                    if (!AppTool.IsNullOrEmpty(this[UOMProps[index - 1]]) && !AppTool.IsNullOrEmpty(this[IdProps[index - 1]])) {
                        if (EmptyIndex != 1) {
                            emptyLines = true;
                            this.ValidationErrorsList.push("Empty Charge Lines aren't allowed between line " + (EmptyIndex - 1) + " and line " + index);
                            EmptyIndex = 1;
                        }
                        if (AppTool.IsNullOrEmpty(this[IdProps[index - 2]])) {
                            //   this.ValidationErrorsList.push("no empty line between 2 charges in line "+index+ " and "+(index-2));
                        }
                    }
                }
            }

            if (this.EntityPM.TypeCode == "OFS") {
                this.HasAContainerTypeUOM = true;
            }
        }

        if (!emptyLines) {
            tempErrors.forEach(error => {
                this.ValidationErrorsList.push(error);
            });
        }
    }

    ValidateContainerTypes() {
        var ContainerTypeIdsProperties: string[] = [];
        var ContainerTypeNamesProperties: string[] = [];
        var EmptyIndex = 1;
        var FirstLineEmpty: boolean = false;
        var emptyLines: boolean = false;
        var tempErrors: Array<string> = [];
        for (var firstIndex = 1; firstIndex <= 5; firstIndex++) {

            ContainerTypeIdsProperties.push("ContainerType" + firstIndex + "Id");
            ContainerTypeNamesProperties.push("Container Type " + firstIndex);

            for (var secondIndex = firstIndex + 1; secondIndex <= 5; secondIndex++) {
                var comparedContainer = "ContainerType" + firstIndex + "Id";
                var targetContainer = "ContainerType" + secondIndex + "Id";
                if (this[comparedContainer] != null && this[comparedContainer] == this[targetContainer]) {
                    this.packageTypeListService.getSingleFromCache(this[targetContainer + ""]).subscribe((res: any) => {
                        if (!res.HasError) {
                            var packageTypeList: PackageTypeList = res.Result;
                            if (res) {
                                this.ValidationErrorsList.push("Container type " + packageTypeList.EnglishName + " is duplicated");
                            }
                        }
                    });
                }
            }

            if (firstIndex == 1) {
                if (AppTool.IsNullOrEmpty(this[ContainerTypeIdsProperties[firstIndex - 1]])) {
                    if (this.HasAContainerTypeUOM) {
                        tempErrors.push(ContainerTypeNamesProperties[firstIndex - 1] + " is required");
                    }
                    FirstLineEmpty = true;
                }
            }

            else {
                if (AppTool.IsNullOrEmpty(this[ContainerTypeIdsProperties[firstIndex - 1]])) {
                    if (EmptyIndex == 1) {
                        EmptyIndex = firstIndex;
                    }
                }

                if (firstIndex == 2) {
                    if (!AppTool.IsNullOrEmpty(this[ContainerTypeIdsProperties[firstIndex - 1]])) {
                        if (FirstLineEmpty) {
                            emptyLines = true;
                            this.ValidationErrorsList.push("Empty Container Lines aren't allowed between line 1 and line 2");
                            EmptyIndex = 1;
                        }
                    }
                }

                if (firstIndex >= 3) {
                    if (!AppTool.IsNullOrEmpty(this[ContainerTypeIdsProperties[firstIndex - 1]])) {
                        if (EmptyIndex != 1) {
                            emptyLines = true;
                            this.ValidationErrorsList.push("Empty Container Lines aren't allowed between line " + (EmptyIndex - 1) + " and line " + firstIndex);
                            EmptyIndex = 1;
                        }
                    }
                }
            }
        }

        if (!emptyLines) {
            tempErrors.forEach(error => {
                this.ValidationErrorsList.push(error);
            });
        }
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.ValidationErrorsList);

        if (this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
            if (AppTool.IsNullOrEmpty(this.CustomsBrokerId)) {
                this.ValidationErrorsList.push("Customs Broker Field is Required");
            }
        }

        else {
            if (AppTool.IsNullOrEmpty(this.SellerId)) {
                this.ValidationErrorsList.push("Seller Field is Required");
            }
        }

        if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OFC") {
            if (AppTool.IsNullOrEmpty(this.FreightChargeId)) {
                var fieldName: string = TextCodeTranslator.Translate('Tariff.F.FreightChargeId');
                var translatedRequiredError: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
                var fieldError: string = translatedRequiredError.replace("%FieldName", fieldName);
                this.ValidationErrorsList.push(fieldError);
            }

            if (this.StartDate == null) {
                this.ValidationErrorsList.push("Start Date Field is Required");
            }

            if (this.StartDate != null && this.ExpirationDate != null) {
                if (this.ExpirationDate < this.StartDate) {
                    this.ValidationErrorsList.push("Expiration date must be less than start date");
                }
            }

            if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC") {
                if (AppTool.IsNullOrEmpty(this.PriceSteps)) {
                    this.ValidationErrorsList.push("Price Steps Field is Required");
                }
            }
            if (this.EntityPM.TypeCode == "AFC") {
                if (AppTool.IsNullOrEmpty(this.TariffProductId)) {
                    this.ValidationErrorsList.push("Product Field is Required");
                }
            }
        }

        else if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS") {
            var validator: ClassLevelValidator = new ClassLevelValidator();

            var errorsArray = validator.Validate("Tariff", this.EntityPM);
            this.ValidationErrorsList = errorsArray;
            this.ValidateSurcharge();
        }

        else if (this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
            this.ValidateSurcharge();
        }

        if (this.EntityPM.TypeCode == "OFC" || this.EntityPM.TypeCode == "OFS") {
            var validator: ClassLevelValidator = new ClassLevelValidator();
            var errorsArray = validator.Validate("Tariff", this.EntityPM);
            this.ValidationErrorsList.concat(errorsArray);
            this.ValidateContainerTypes();
        }

        if (this.ValidationErrorsList.length == 0) {
            this.EntityPM.AddTariffVersion(this.firstVersion);

            this.CurrentSession.StartBusyIndicator("Creating...");

            this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit('OK');
                }
                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }

    private tariffSetting: TariffSettingPM;
    GetTenantTariffSetting() {
        var myDomainService = new TariffDomainService();
        myDomainService.GetTenantTariffSetting().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.tariffSetting = myResponse.Result;
                if (this.tariffSetting != null) {
                    if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC") {
                        if (this.EntityPM.TypeCode == "AFC") {
                            this.PriceSteps = this.tariffSetting.AirDefaultSteps;
                            this.EntityPM.UnitOfMeasurementCode = this.tariffSetting.AirUnitOfMeasurementCode;
                        }
                        else {
                            this.PriceSteps = this.tariffSetting.LCLDefaultSteps;
                            this.EntityPM.UnitOfMeasurementCode = this.tariffSetting.LCLUnitOfMeasurementCode;
                        }

                        this.PriceStepsText = this.GetPriceSteps(this.PriceSteps);                        
                    }

                    else if (this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "OFC") {
                        this.GetAllPackages();
                    }

                    this.CurrencyId = this.tariffSetting.DefaultCurrencyId;
                }
            }
        });
    }

    private allPackageTypes: PackageTypeList[];
    private GetAllPackages() {
        this.packageTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.allPackageTypes = myResponse.Result;
                this.GetDefaultContainers();
            }
        });
    }

    private GetDefaultContainers() {
        if (this.tariffSetting != null && !AppTool.IsNullOrEmpty(this.tariffSetting.ContainerDefaults)) {
            var containersArray: string[] = this.tariffSetting.ContainerDefaults.split(',');

            if (containersArray.length > 0) {
                var index: number = 1;
                containersArray.forEach(item => {
                    var packageType: PackageTypeList = this.allPackageTypes.filter(d => d.Code == item.trim() && d.Tenant == InfraSettings.TenantPM.Id)[0];
                    if (packageType != null) {
                        this['ContainerType' + index++ + 'Id'] = packageType.Id;
                    }
                });
            }
        }
    }

    EditPriceSteps() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Price Steps";
        logWindow.WindowArgs = [this.PriceSteps, this.EntityPM.UnitOfMeasurementCode];
        logWindow.Show("./TariffModule/Components/NewEntity/TariffPriceStepsComponent");
        logWindow.ComponentLoaded.subscribe(s => {
            logWindow.WindowClosed.subscribe(d => {
                if (d != "cancel") {
                    var steps = s.DefaultPriceSteps;
                    this.EntityPM.PriceSteps = steps;
                    this.EntityPM.UnitOfMeasurementCode = s.UnitOfMeasurementCode;
                    this.PriceSteps = steps;
                    this.PriceStepsText = this.GetPriceSteps(this.PriceSteps);
                }
            });
        });
    }

    GetPriceSteps(steps: string) {
        if (steps) {
            var stpesWithSpaces = steps.split(',').join(', ');
            return stpesWithSpaces;
        }
    }

    EditAllInCharges() {
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = { TariffPM: this.EntityPM, VersionPM: this.firstVersion, IsEditingEnabled: true };
        logWindow.Title = "All-In Charges";
        logWindow.Show("./TariffModule/Components/EditTabs/Tariff/AddEditAllInChargesComponent");
        logWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.BuildAllInChargesText();
            }
        });
    }

    public AllInChargesText: string;
    private BuildAllInChargesText() {
    var allInCharges: string = null;

    if (this.firstVersion != null) {
      var codesList: TariffVersionAllInChargePM[] = [];
      var addDots:boolean = false;

      if(this.firstVersion.TariffAllInCharges.length > 4){
        codesList = this.firstVersion.TariffAllInCharges.slice(0, 4);
        addDots = true;
      }

      else {
        codesList = this.firstVersion.TariffAllInCharges;
      }

      codesList.forEach((item: TariffVersionAllInChargePM) => {
        if (AppTool.IsNullOrEmpty(allInCharges)) {
          allInCharges = item.ChargesTypeCode;
        }

        else {
          allInCharges = allInCharges + ", " + item.ChargesTypeCode;
        }
      });
    }

    if(addDots) {
      allInCharges = allInCharges + "...";
    }

    this.AllInChargesText = allInCharges;
  }
}
