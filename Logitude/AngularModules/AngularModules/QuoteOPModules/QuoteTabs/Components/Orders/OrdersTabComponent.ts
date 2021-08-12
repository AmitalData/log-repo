import { Component, OnInit, AfterViewInit, ViewChild, OnDestroy} from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteUtilities} from '../../../../Quote/Utilities/QuoteUtilities';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTool} from '../../../../Quote/Tools';
import { QuoteSettingPM } from '../../../../Quote/EntityPMs/QuoteSettingPM';
import { QuoteDomainService } from '../../../../Quote/Services/QuoteDomainService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';
import { ServiceLocator } from 'Infrastructure/Locators/ServiceLocator';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'OrdersTabComponent',
    templateUrl: './OrdersTabComponent.html',
})

export class OrdersTabComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {
   
    public EntityPM: QuotePM;
    public DataContext: OrdersTabComponent = this;
    public ObjectTableName: string = "QuoteOP";
    public QuoteSetting: QuoteSettingPM = null;
    public TransportModeId: string;
    public IsQuoteClosedAutomaticallyEnabled: boolean;

    @ViewChild(ChildDirective) Child: ChildDirective;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.GetQuoteSetting();
        this.Listen();
        this.ListenPropertyChanged();
    }

    private GetQuoteSetting() {
        var quoteDomainService = new QuoteDomainService();
        quoteDomainService.GetQuoteSettings().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError == false) {
                if (myResponse.Result) {
                    if (myResponse.Result.Id) {
                        this.QuoteSetting = myResponse.Result;
                    }
                }
            }
        });
    }

    ngOnInit() {
        if (this.EntityPM) {
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.SetUIProperties();
            this.SetLabels();
        }
    }

    ngAfterViewInit() {
        if (this.EntityPM) {
            this.InitalizeFeatureOfClosedAutomatically();
            SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.Child.Location)
                .then(cmpRef => {
                    cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, "QuoteOP.GeneralTabScreen");
                });
        }
    }

    public ChargeableWeightUnitCodeLabel: string = null;
    SetLabels() {
        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("QuoteOP.F.ChargeableWeightUnitCode");
        }

        else {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator.Translate("QuoteOP.F.ChargeableWeightUnitCode.Short");
        }
    }

    private TabSelectedEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private PropertyChangedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.ListenPropertyChanged();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.ListenPropertyChanged();
                }
            });

            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "QTOR") {
                    this.SetUIProperties_DimFactor();
                    this.SetUIProperties_DimensionsUnitCode();
                }
            });
        }
    }
    private ListenPropertyChanged() {
        if (this.PropertyChangedEvent) {
            AppTool.KillEventEmitter(this.PropertyChangedEvent);
            this.PropertyChangedEvent = null;
        }

        this.PropertyChangedEvent = this.EntityPM.PropertyChanged.subscribe(s => {
            if (s) {
                if (s.PropertyName == "ValueOfGoods") {
                    QuoteTool.OnQuoteQuantitiesChanged(this.EntityPM);
                }
            }
        });
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.PropertyChangedEvent);
    }

    public IsLCLQuote: boolean = false;
    public IsQuoteEditEnabled = true;
    public IsDimFactorVisible: boolean = false
    SetUIProperties() {
        this.IsLCLQuote = QuoteUtilities.IsLCLQuote(this.EntityPM);
        this.IsQuoteEditEnabled = QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);

        this.SetUIProperties_DimFactor();
        this.SetUIProperties_EntityClosed();
        this.SetUIProperties_AutomaticallyClosed();
        this.SetUIProperties_DimensionsUnitCode();
    }
    private SetUIProperties_DimFactor() {
        var isDimFactorVisibile = false;

        if (!AppTool.IsNullOrEmpty(this.DimensionsUnitCode)) {
            if (this.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }

        this.IsDimFactorVisible = isDimFactorVisibile;
        this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    }
    private SetUIProperties_EntityClosed() {
        this.EntityPM.UIProperties.SetEnabled("IncotermId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("MoveTypeId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("ExpirationDays", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("StartDate", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("ExpirationDate", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("ChargeableWeightUnitCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("Ratio", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("DimFactor", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("TransitTime", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.EntityPM.UIProperties.SetEnabled("DepartureFrequency", this.ObjectTableName, this.IsQuoteEditEnabled);

        ServiceLocator.RulesValidator.ApplyAllConditionalBlockFieldRules(this.EntityPM, this.ObjectTableName);
    }
    private SetUIProperties_AutomaticallyClosed() {
        this.UIProperties.SetEnabled("AutomaticallyCloseDate", this.ObjectTableName, (this.IsAutomaticallyClosed && this.IsQuoteEditEnabled && this.IsQuoteClosedAutomaticallyEnabled));
        this.UIProperties.SetEnabled("AutomaticallyCloseDays", this.ObjectTableName, (this.IsAutomaticallyClosed && this.IsQuoteEditEnabled && this.IsQuoteClosedAutomaticallyEnabled));
        this.UIProperties.SetEnabled("IsAutomaticallyClosed", this.ObjectTableName, (this.IsQuoteEditEnabled && this.IsQuoteClosedAutomaticallyEnabled));
    }

    public DimensionsDependencyProperty1: string = null;
    public DimensionsDependencyProperty1IsList: boolean = false;
    private SetUIProperties_DimensionsUnitCode() {
        var isFieldEnabled: boolean = false;

        if (this.IsQuoteEditEnabled && this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }

        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }

        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }

        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    }
    private InitalizeFeatureOfClosedAutomatically() {
        this.IsQuoteClosedAutomaticallyEnabled = FeatureLocator.HasFeaturePermession("QuoteOP", "QuoteClosedAutomatically");
        this.UIProperties.SetEnabled("AutomaticallyCloseDate", this.ObjectTableName, this.IsQuoteClosedAutomaticallyEnabled);
        this.UIProperties.SetEnabled("AutomaticallyCloseDays", this.ObjectTableName, this.IsQuoteClosedAutomaticallyEnabled);
        this.UIProperties.SetEnabled("IsAutomaticallyClosed", this.ObjectTableName, this.IsQuoteClosedAutomaticallyEnabled);
        this.SetUIProperties_AutomaticallyClosed();
    }

    // Measurment Units
    public MeasurmentsButtonToolTip: string = TextCodeTranslator.Translate("QuoteOP.B.Details.MeasurmentsSettings");
    IsMeasurmentsHidden: boolean = true;
    MeasurmentsSettingsClicked() {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;

        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("QuoteOP.B.Details.MeasurmentsSettings");
        }

        else {
            this.MeasurmentsButtonToolTip = TextCodeTranslator.Translate("QuoteOP.B.Details.HideMeasurmentsSettings");
        }
    }

    get GrossWeightUnitCode() { return this.EntityPM.GrossWeightUnitCode; }
    set GrossWeightUnitCode(newValue: string) {
        if (this.EntityPM.GrossWeightUnitCode != newValue) {
            this.EntityPM.GrossWeightUnitCode = newValue;
            this.OnMeasurmentsSettingsChanged();
        }
    }
    
    //get SpecialServiceId() { return this.EntityPM.SpecialServiceId; }
    //set SpecialServiceId(newValue: string) {
    //    if (this.EntityPM.SpecialServiceId!= newValue) {
    //        this.EntityPM.SpecialServiceId= newValue;

    //        this.ComputeDimFactor();
    //        this.OnMeasurmentsSettingsChanged();
    //    }
    //}

    get ChargeableWeightUnitCode() { return this.EntityPM.ChargeableWeightUnitCode; }
    set ChargeableWeightUnitCode(newValue: string) {
        if (this.EntityPM.ChargeableWeightUnitCode != newValue) {
            this.EntityPM.ChargeableWeightUnitCode = newValue;

            this.ComputeDimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get DimensionsUnitCode() { return this.EntityPM.DimensionsUnitCode; }
    set DimensionsUnitCode(newValue: string) {
        if (this.EntityPM.DimensionsUnitCode != newValue) {
            this.EntityPM.DimensionsUnitCode = newValue;

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get VolumeUnitCode() { return this.EntityPM.VolumeUnitCode; }
    set VolumeUnitCode(newValue: string) {
        if (this.EntityPM.VolumeUnitCode != newValue) {
            this.EntityPM.VolumeUnitCode = newValue;

            this.EntityPM.DimensionsUnitCode = AppTool.GetDimentionsCodeFromVolumeCode(newValue);

            this.ComputeDimFactor();
            this.SetUIProperties_DimFactor();
            this.SetUIProperties_DimensionsUnitCode();
            this.OnMeasurmentsSettingsChanged();
        }
    }

    get Ratio() { return this.EntityPM.Ratio; }
    set Ratio(newValue: number) {
        if (this.EntityPM.Ratio != newValue) {
            this.EntityPM.Ratio = newValue;

            this.ComputeDimFactor();
            QuoteUtilities.OnQuoteRatioChanged(this.EntityPM);
        }
    }

    get DimFactor() { return this.EntityPM.DimFactor; }
    set DimFactor(newValue: number) {
        if (this.EntityPM.DimFactor != newValue) {
            this.EntityPM.DimFactor = newValue;

            this.EntityPM.Ratio = AppTool.GetRatioFromDimFactor(this.DimFactor, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
            QuoteUtilities.OnQuoteRatioChanged(this.EntityPM);
        }
    }

    private ComputeDimFactor() {
        this.EntityPM.DimFactor = AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    }
    private OnMeasurmentsSettingsChanged() {
        QuoteUtilities.RecalculateQuoteFields(this.EntityPM);
    }

    //Quote Details
    get IncotermId() { return this.EntityPM.IncotermId; }
    set IncotermId(newValue: string) {
        if (this.EntityPM.IncotermId != newValue) {
            this.EntityPM.IncotermId = newValue;
        }
    }

    get MoveTypeId() { return this.EntityPM.MoveTypeId; }
    set MoveTypeId(newValue: string) {
        if (this.EntityPM.MoveTypeId != newValue) {
            this.EntityPM.MoveTypeId = newValue;
        }
    }

    get ExpirationDays() { return this.EntityPM.ExpirationDays; }
    set ExpirationDays(newValue: number) {
        if (this.EntityPM.ExpirationDays != newValue) {
            this.EntityPM.ExpirationDays = newValue;

            if (newValue == null) {
                this.ExpirationDate = null;
            }

            else {
                this.SetExpirationDate();
            }
        }
    }

    private SetExpirationDate() {
        if (this.EntityPM.ExpirationDays == null && this.EntityPM.ExpirationDate == null) { }
        else {
            var date = DateTool.AddDays(this.StartDate, this.ExpirationDays);
            if (date == null) {
                this.EntityPM.ExpirationDays = null;
            }
            else {
                if (this.ExpirationDate == null || (this.ExpirationDate != null && this.ExpirationDate.valueOf() != date.valueOf())) {
                    this.ExpirationDate = date;
                }
            }
        }
    }

    get ExpirationDate() { return this.EntityPM.ExpirationDate; }
    set ExpirationDate(newValue: Date) {
        if (this.EntityPM.ExpirationDate != newValue) {
            this.EntityPM.ExpirationDate = newValue;

            if (newValue == null) {
                this.EntityPM.ExpirationDays = null;
            }

            else {
                var days = this.GetDaysBetweenDates(newValue, this.StartDate);
                if (this.ExpirationDays != days) {
                    this.EntityPM.ExpirationDays = days;
                }
            }
        }
    }

    get StartDate() { return this.EntityPM.StartDate; }
    set StartDate(newValue: Date) {
        if (this.EntityPM.StartDate != newValue) {
            this.EntityPM.StartDate = newValue;

            if (newValue == null) {
                this.EntityPM.ExpirationDays = null;
            }
            else {
                this.SetExpirationDate();
            }
        }
    }

    get IsAutomaticallyClosed() { return this.EntityPM.IsAutomaticallyClosed; }
    set IsAutomaticallyClosed(newValue: boolean) {
        if (this.EntityPM.IsAutomaticallyClosed != newValue) {
            this.EntityPM.IsAutomaticallyClosed = newValue;

            if (newValue) {
                var todayDate = DateTool.GetCurrentDateAsUtc();
                var closeDays = this.QuoteSetting != null ? this.QuoteSetting.AutomaticallyCloseDays : 30;
                this.EntityPM.AutomaticallyCloseDays = closeDays;
                this.EntityPM.AutomaticallyCloseDate = DateTool.AddDays(todayDate, closeDays);
            }

            else {
                this.EntityPM.AutomaticallyCloseDays = null;
                this.EntityPM.AutomaticallyCloseDate = null;
                this.EntityPM.QuoteClosingReasonCode = null;
                this.EntityPM.QuoteClosingReasonId = null;
            }

            this.SetUIProperties_AutomaticallyClosed();
        }
    }

    get AutomaticallyCloseDays() { return this.EntityPM.AutomaticallyCloseDays; }
    set AutomaticallyCloseDays(newValue: number) {
        if (this.EntityPM.AutomaticallyCloseDays != newValue) {
            this.EntityPM.AutomaticallyCloseDays = newValue;

            if (newValue == null) {
                this.EntityPM.AutomaticallyCloseDate = null;
            }

            else {
                var date = DateTool.GetDateByDay(newValue);

                if (this.AutomaticallyCloseDate.valueOf() != date.valueOf()) {
                    this.EntityPM.AutomaticallyCloseDate = date;
                }
            }
        }
    }

    get AutomaticallyCloseDate() { return this.EntityPM.AutomaticallyCloseDate; }
    set AutomaticallyCloseDate(newValue: Date) {
        if (this.EntityPM.AutomaticallyCloseDate != newValue) {
            this.EntityPM.AutomaticallyCloseDate = newValue;

            if (newValue == null) {
                this.EntityPM.AutomaticallyCloseDays = null;
            }

            else {
                var todayDate = DateTool.GetCurrentDateAsUtc();
                var days = this.GetDaysBetweenDates(newValue, todayDate);

                if (this.AutomaticallyCloseDays != days) {
                    this.EntityPM.AutomaticallyCloseDays = days;
                }
            }
        }
    }
    private GetDaysBetweenDates(date1: Date, date2: Date) {
        var myResult: number = 0;

        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {

                date1 = this.TruncateTime(date1);
                date2 = this.TruncateTime(date2);

                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = d1.getTime() - d2.getTime();
                var Daysdiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                myResult = Daysdiff;
            }
        }

        return myResult;
    }
    private TruncateTime(date: Date) {
        var myResult: Date = null;

        if (date != null) {
            var myResult: Date = new Date(date.toString());
            myResult.setUTCHours(0);
            myResult.setUTCMinutes(0);
            myResult.setUTCSeconds(0);
        }

        return myResult;
    }

    get DepartureFrequency() { return this.EntityPM.DepartureFrequency; }
    set DepartureFrequency(newValue: string) {
        if (this.EntityPM.DepartureFrequency != newValue) {
            this.EntityPM.DepartureFrequency = newValue;
        }
    }

    get TransitTime() { return this.EntityPM.TransitTime; }
    set TransitTime(newValue: string) {
        if (this.EntityPM.TransitTime != newValue) {
            this.EntityPM.TransitTime = newValue;
        }
    }
}
