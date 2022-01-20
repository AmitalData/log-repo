import { Component, OnDestroy,AfterViewInit } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TariffPM } from '../../../../TariffModule/EntityPMs/TariffPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ChargesTypeListService } from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import { ChargesTypeList } from '../../../../Common/EntityLists/ChargesTypeList';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { TariffDomainService } from '../../../../TariffModule/Services/TariffDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CommonDomainService } from '../../../../Common/Services/CommonDomainService';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { CardList } from '../../../../Common/EntityLists/CardList';

@Component({
    
    templateUrl: './TariffGeneralTabComponent.html',
})

export class TariffGeneralTabComponent extends BaseComponent implements OnDestroy, AfterViewInit {
    public EntityPM: TariffPM;
    public ObjectTableName: string = "Tariff";
    public DataContext = this;
    public VisibileSurchargesArea: boolean = false;
    private IdProps: string[] = [];
    private UOMProps: string[] = [];
    private containersProps: string[] = [];
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public MeasurementsQueryFilters: ApiQueryFilters;
    public ValidationErrorsList: string[] = [];
    private chargesTypePMService: ChargesTypeListService;
    public SellerDependancy: string = "AL";
    public TariffCurrencyTextCode: string = "Tariff.F.CurrencyId";
    public IsContainersAreaVisible: boolean = false;
    private draftVersion: TariffVersionPM;
    public IsSellerVisible: boolean = false;
    public IsCustomsBrokerVisible: boolean = false;
    public CustomsBrokerDependancy: string = "AG,CG";
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.chargesTypePMService = new ChargesTypeListService();
        this.draftVersion = this.EntityPM.TariffVersions.filter(d => d.IsDraft)[0];

        this.BuildQueryFilters();

        if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS" || this.EntityPM.TypeCode == "ICC" || this.EntityPM.TypeCode == "ECC") {
            this.FillChargesIDsAndUOMS();
            this.VisibileSurchargesArea = true;
            this.TariffCurrencyTextCode = "Tariff.O.DefaultCurrency";
        }

        if (this.EntityPM.TypeCode == "OFC" || this.EntityPM.TypeCode == "OFS") {
            this.FillContainersIDs();
            this.IsContainersAreaVisible = true;
        }

        this.Listen();
        this.SetUIProperties();
        this.GetBCNTMeasurementId();
    }

    private BCNTmeasurementId: string;
    private GetBCNTMeasurementId() {
        if (this.EntityPM.TypeCode == "OFS") {
            var commonDomainService: CommonDomainService = new CommonDomainService();
            commonDomainService.GetMeasurementIdByCode("BCNT").subscribe((res:any) => {
                if (!res.HasError) {
                    if (res.Result) {
                        this.BCNTmeasurementId = res.Result;
                    }
                }
            });
        }
    }

    SetUIProperties() {
        this.SetUIProperties_Seller();
        this.SetUIProperties_CustomsBroker();
        this.CheckCurrancyEnabledProperty();
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

    CheckCurrancyEnabledProperty() {
        var service: TariffDomainService = new TariffDomainService();
        service.GetAllVersionsWithLinesForTariff(this.EntityPM.Id).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
               var versions = response.Result;
                if (this.EntityPM.TypeCode == "AFC" || this.EntityPM.TypeCode == "OLC" || this.EntityPM.TypeCode == "OFC") {
                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
                    if (versions != null) {
                        if (versions != null && versions.length > 1) {
                            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                        }
                        else {
                            var version = versions[0];
                            if (version == null || (version != null && version.IsDraft)) {
                                var hasTariffLines = false;
                                versions.forEach(item => {
                                    if (item.TariffLines != null && item.TariffLines.length > 0) {
                                        hasTariffLines = true;
                                    }
                                });
                                if (hasTariffLines) {
                                    this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                                }
                            }
                            else if (version != null && !version.IsDraft) {
                                this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
                            }
                        }
                    }
                }
                if (this.EntityPM.TypeCode == "AFC") {
                    this.UIProperties.SetRequired("TariffProductId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.TariffProductId));
                }
            }
        }); 
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

        else if (this.EntityPM.TypeCode == "ECC" || this.EntityPM.TypeCode == "ICC") {
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
    }

    FillChargesIDsAndUOMS() {
        for (var index = 1; index <= 10; index++) {
            this.IdProps.push("Surcharge" + index + "Id");
            this.UOMProps.push("Surcharge" + index + "UOM");
        }
    }
    FillContainersIDs() {
        for (var index = 1; index <= 5; index++) {
            this.containersProps.push("ContainerType" + index + "Id");
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

    private SetDefaultCurrency(index: number) {        
        if (this.draftVersion != null) {
            this.draftVersion.TariffLines.forEach(item => {
                if (item.IsDifferentCurrenciesPerCharge) {
                    if (AppTool.IsNullOrEmpty(this["Surcharge" + index + "Id"])) {
                        item["Surcharge" + index + "CurrencyId"] = null;
                    }

                    else {
                        item["Surcharge" + index + "CurrencyId"] = this.CurrencyId;
                    }
                }
            });
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

            this.SetDefaultCurrency(1);
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

            this.SetDefaultCurrency(2);
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

            this.SetDefaultCurrency(3);
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

            this.SetDefaultCurrency(4);
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

            this.SetDefaultCurrency(5);
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

            this.SetDefaultCurrency(6);
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

            this.SetDefaultCurrency(7);
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

            this.SetDefaultCurrency(8);
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

            this.SetDefaultCurrency(9);
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

            this.SetDefaultCurrency(10);
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

    //Containers
    get ContainerType1Id() {
        return this.EntityPM.ContainerType1Id;
    }
    set ContainerType1Id(value: string) {
        if (this.EntityPM.ContainerType1Id != value) {
            this.EntityPM.ContainerType1Id = value;
            this.ValidateContainers();
        }
    }

    get ContainerType2Id() {
        return this.EntityPM.ContainerType2Id;
    }
    set ContainerType2Id(value: string) {
        if (this.EntityPM.ContainerType2Id != value) {
            this.EntityPM.ContainerType2Id = value;
            this.ValidateContainers();
        }
    }

    get ContainerType3Id() {
        return this.EntityPM.ContainerType3Id;
    }
    set ContainerType3Id(value: string) {
        if (this.EntityPM.ContainerType3Id != value) {
            this.EntityPM.ContainerType3Id = value;
            this.ValidateContainers();
        }
    }

    get ContainerType4Id() {
        return this.EntityPM.ContainerType4Id;
    }
    set ContainerType4Id(value: string) {
        if (this.EntityPM.ContainerType4Id != value) {
            this.EntityPM.ContainerType4Id = value;
            this.ValidateContainers();
        }
    }

    get ContainerType5Id() {
        return this.EntityPM.ContainerType5Id;
    }
    set ContainerType5Id(value: string) {
        if (this.EntityPM.ContainerType5Id != value) {
            this.EntityPM.ContainerType5Id = value;
            this.ValidateContainers();
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
        for (var index = 1; index <= 10; index++) {
            IdProps.push("Surcharge" + index + "Id");
            UOMProps.push("Surcharge" + index + "UOM");

            IdPropsName.push("Charge Type " + index);
            UOMPropsName.push("UOM " + index);
            if (this.IdProps.filter(p => this[p + ""] == this[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && this[IdProps[index - 1]] != null)[0] != null) {
                var chargresType = this.IdProps.filter(p => this[p + ""] == this[IdProps[index - 1]] && (p + "" != IdProps[index - 1] + "") && this[IdProps[index - 1]] != null)[0];
                if (!DuplicatedChargesIds.includes(this[chargresType + ""])) {
                    DuplicatedChargesIds.push(this[chargresType + ""]);
                    this.chargesTypePMService.getSingleFromCache(this[chargresType + ""]).subscribe((res:any) => {
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

                if (AppTool.IsNullOrEmpty(this[UOMProps[index - 1]])) {
                    tempErrors.push(UOMPropsName[index - 1] + " is required");
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
        }

        if (!emptyLines) {
            tempErrors.forEach(error => {
                this.ValidationErrorsList.push(error);
            });
        }
    }

    ngAfterViewInit() {
        if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS") {
            this.Validate(true);
        }
        if (this.EntityPM.TypeCode == "OFC" || this.EntityPM.TypeCode == "OFS") {
            this.ValidateContainers(true);
        }        
    }

    private firstIndex: number = 0;
    Validate(initial: boolean = false) {
        var entered = false;
        for (var index = 1; index <= 10; index++) {
            if (initial) {
                if (!AppTool.IsNullOrEmpty(this[this.IdProps[index - 1]])) {
                    this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, false);
                    this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                }

                else {
                    if (!entered) {
                        entered = true;
                        this.firstIndex = index;

                        this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, true);
                        this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetEnabled(this.IdProps[index - 1], this.ObjectTableName, false);
                        this.UIProperties.SetEnabled(this.UOMProps[index - 1], this.ObjectTableName, false);
                    }
                }
            }

            else {
                if (index>= this.firstIndex) {
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
    }

    private containerIndex: number = 0;
    ValidateContainers(initial: boolean = false) {
        var entered = false;
        for (var index = 1; index <= 5; index++) {
            if (initial) {
                if (!AppTool.IsNullOrEmpty(this[this.containersProps[index - 1]])) {
                    this.UIProperties.SetEnabled(this.containersProps[index - 1], this.ObjectTableName, false);
                }

                else {
                    if (!entered) {
                        entered = true;
                        this.containerIndex = index;
                        this.UIProperties.SetEnabled(this.containersProps[index - 1], this.ObjectTableName, true);
                    }
                    else {
                        this.UIProperties.SetEnabled(this.containersProps[index - 1], this.ObjectTableName, false);
                    }
                }
            }

            else {
                if (index >= this.containerIndex) {
                    if (AppTool.IsNullOrEmpty(this[this.containersProps[index - 1]])) {
                        
                        if (index > 1) {
                            if (!AppTool.IsNullOrEmpty(this[this.containersProps[index - 2]])) {
                                this.UIProperties.SetEnabled(this.containersProps[index - 1], this.ObjectTableName, true);
                            }
                        }
                        else {
                            this.UIProperties.SetRequired(this.containersProps[index - 1], this.ObjectTableName, true);
                        }
                    }
                    else {
                        if (index == 1) {
                            this.UIProperties.SetRequired(this.containersProps[index - 1], this.ObjectTableName, false);
                        }
                    }
                }
            }
        }
    }

    private SaveCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    for (var i = 1; i <= 10; i++) {
                        this["isSurcharges" + i + "Added"] = false;
                    }

                    if (this.EntityPM.TypeCode == "ASC" || this.EntityPM.TypeCode == "OSC" || this.EntityPM.TypeCode == "OFS") {
                        this.Validate(true);
                    }
                    if (this.EntityPM.TypeCode == "OFC" || this.EntityPM.TypeCode == "OFS") {
                        this.ValidateContainers(true);
                    }
                    
                    this.CheckCurrancyEnabledProperty();
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;

            this.SetDifferentCurrencies();
        }
    }

    get SellerId() { return this.EntityPM.SellerId; }
    set SellerId(value: string) {
        if (this.EntityPM.SellerId != value) {
            this.EntityPM.SellerId = value;

            this.SetUIProperties_Seller();
        }
    }

    private seller: CardList;
    get Seller() { return this.seller; }
    set Seller(value: CardList) {
        if (this.seller != value) {
            this.seller = value;

            if (value) {
                this.EntityPM.SellerName = value.EnglishName;
            }

            else {
                this.EntityPM.SellerName = null;
            }
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

    private customsBroker: CardList;
    get CustomsBroker() { return this.customsBroker; }
    set CustomsBroker(value: CardList) {
        if (this.customsBroker != value) {
            this.customsBroker = value;

            if (value) {
                this.EntityPM.CustomsBrokerName = value.EnglishName;
            }

            else {
                this.EntityPM.CustomsBrokerName = null;
            }
        }
    }

    get ContractNumber() { return this.EntityPM.ContractNumber; }
    set ContractNumber(value: string) {
        if (this.EntityPM.ContractNumber != value) {
            this.EntityPM.ContractNumber = value;
        }
    }

    private SetDifferentCurrencies() {
        for (var i = 1; i <= 10; i++) {
            if (this["isSurcharges" + i + "Added"]) {
                if (this["Surcharge" + i + "Id"]) {
                    this.SetDefaultCurrency(i);
                }
            }
        }
    }
}
