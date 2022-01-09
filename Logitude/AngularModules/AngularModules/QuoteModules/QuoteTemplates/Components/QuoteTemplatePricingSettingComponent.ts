import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplatePM} from '../../../Quote/EntityPMs/QuoteTemplatePM';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {QuoteTemplateTextDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTextDesignPM';
import {QuoteTemplateTableDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTableDesignPM';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {QuoteTemplateTextCodePM} from '../../../Quote/EntityPMs/QuoteTemplateTextCodePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateSettingPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {QuoteTemplateTableDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService';
import {QuoteTemplateTextDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService';
import {QuoteTemplateTextDesignExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateTextDesignExtendedPMService';
import {QuoteTemplateTextCodeExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateTextCodeExtendedPMService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import { QuoteTemplateSettingData, PricesFieldSettings } from '../../../Quote/DataContracts/QuoteTemplateSettingData';
import { List } from '../../../Infrastructure/DataContracts/Dashboard/List';
@Component({
    selector: 'QuoteTemplatePricingSettingComponent',
    
    templateUrl: './QuoteTemplatePricingSettingComponent.html',
})

export class QuoteTemplatePricingSettingComponent extends BaseComponent implements OnInit {
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService;
    quoteTemplateTextDesignPMService: QuoteTemplateTextDesignPMService;
    quoteTemplateTableDesignPMService: QuoteTemplateTableDesignPMService;
    quoteTemplateTextDesignExtendedPMService: QuoteTemplateTextDesignExtendedPMService;
    quoteTemplateTextCodeExtendedPMService: QuoteTemplateTextCodeExtendedPMService;
    QuoteTemplatePM: QuoteTemplatePM;
    IsLoadPage: boolean;

    Alignment: string[] = [];
    QuoteTemplateSettingPM: QuoteTemplateSettingPM;

    public ValidationErrorsList: string[];

    TableDesignPM: QuoteTemplateTableDesignPM;
    HeaderTextDesignPM: QuoteTemplateTextDesignPM;
    RowTextDesignPM: QuoteTemplateTextDesignPM;

    QuoteTemplateTextDesignPMLists: QuoteTemplateTextDesignPM[] = [];
    QuoteTemplateTextCodePMList: QuoteTemplateTextCodePM[] = [];
    AllQuoteTemplateTextCodePMList: QuoteTemplateTextCodePM[] = [];

    TotalLabelTextDesignPM: QuoteTemplateTextDesignPM;
    TotalValueTextDesignPM: QuoteTemplateTextDesignPM;


    GroupByHeaderTextDesignPM: QuoteTemplateTextDesignPM;
    GroupByTotalTextDesignPM: QuoteTemplateTextDesignPM;

    TitleTextDesignPM: QuoteTemplateTextDesignPM;
    IsSaveQuoteTemplateTextDesignRuning: boolean = false;
    IsSaveQuoteTemplateTableDesignRuning: boolean = false;
    IsSaveQuoteTemplateTextCodeRuning: boolean = false;
    public ItemsSource: ObservableCollection;
    QuoteTemplateSectionTypeName: string = "Packages";
    IsPerContainerChange: boolean = false;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    ShowTotalPerContinerLink: boolean = false;
    ShowVATDetails :boolean = false;
    DisplayRegoinalTax: boolean = false;

    constructor() {
        super();
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();
        this.quoteTemplateTableDesignPMService = new QuoteTemplateTableDesignPMService();
        this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService();
        this.quoteTemplateTextDesignExtendedPMService = new QuoteTemplateTextDesignExtendedPMService();
        this.quoteTemplateTextCodeExtendedPMService =new QuoteTemplateTextCodeExtendedPMService();
        this.ItemsSource = new ObservableCollection([]);


        if (FeatureLocator.HasFeaturePermession("Quote", "TOTALPERCONTAINER")) this.ShowTotalPerContinerLink = true;
        if (SessionLocator.AccountingSettingPM.AllowRegionalTaxManagement) this.DisplayRegoinalTax = true;

    }

    ngOnInit() {

    }

    QuotePM: QuotePM;
    SelectedTabCode: string;
    IsRoutingRates: boolean = false;
    SetWindowArgs(args: any) {
        this.SelectedTabCode = "PRT";
        this.QuoteTemplatePM = args.QuoteTemplatePM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuotePM = args.QuotePM;

        this.FillQuoteTemplateTableSettingsData();

        if (((this.QuotePM && this.QuotePM.IsChargesByVAT) || !this.QuotePM) && FeatureLocator.HasFeaturePermession("Quote", "VATDetAILSINQUOTATION")) {
                this.ShowVATDetails = true;
            }
        


        this.IsRoutingRates = this.QuoteTemplatePM != null ? this.QuoteTemplatePM.TemplateTypeCode == "P" ? true : false : false;
        this.Alignment.push("Left"); this.Alignment.push("Center"); this.Alignment.push("Right");


        if (args.QuoteTemplateTextCodePMList) {
            this.QuoteTemplateTextCodePMList = args.QuoteTemplateTextCodePMList.filter(d => d.Area == this.QuoteTemplateSectionTypeName);
            if (this.IsRoutingRates) {
                this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != "UNITSPACKAGES" && d.TextCode != "TOTALAMOUNTS");
            }
            if (!this.ShowVATDetails) {
                this.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodePMList.filter(d => d.TextCode != "VATTYPEPACKAGES" && d.TextCode != "VATTYPECONTAINERS" && d.TextCode != "VATPERCENTAGEPACKAGES" && d.TextCode != "VATPERCENTAGECONTAINERS");
            }


            this.AllQuoteTemplateTextCodePMList = args.QuoteTemplateTextCodePMList;

            this.BuildItemsSource();
        }
        
        this.LoadData();
    }

    CountOfUsedQuoteTemplatePricesTableSettingsData: number = 0;
    private FillQuoteTemplateTableSettingsData() {
        if (!this.QuoteTemplateSettingPM) return;
        if (!this.QuoteTemplateSettingPM.QuoteTemplateSettingData) return;
        if (this.QuoteTemplateSectionTypeName == "Packages") this.FilllQuoteTemplatePricesPackagesTableSettingsData();
        else this.FilllQuoteTemplatePricesContainersTableSettingsData();
        this.CountOfUsedQuoteTemplatePricesTableSettingsData = this.QuoteTemplatePricesTableSettingsData.filter(q => q.Show).length;
    }

    private FilllQuoteTemplatePricesPackagesTableSettingsData() {
        this.QuoteTemplateSettingPM.QuoteTemplateSettingData.PricesPackagesTableSettings.forEach((item) => {
            item.Key = Guid.newGuid();
            item.Show = this.IsMustBeShown(item);
            item.DisplayTextCode = this.GetItemSettingsDataDisplayTextCode(item);
        });
        this.QuoteTemplatePricesTableSettingsData = this.QuoteTemplateSettingPM.QuoteTemplateSettingData.PricesPackagesTableSettings;
    }

    private FilllQuoteTemplatePricesContainersTableSettingsData() {
        this.QuoteTemplateSettingPM.QuoteTemplateSettingData.PricesContainersTableSettings.forEach((item) => {
            item.Key = Guid.newGuid();
            item.Show = this.IsMustBeShown(item);
            item.DisplayTextCode = this.GetItemSettingsDataDisplayTextCode(item);
        });
        this.QuoteTemplatePricesTableSettingsData = this.QuoteTemplateSettingPM.QuoteTemplateSettingData.PricesContainersTableSettings;
    }

    private IsMustBeShown(item) {
        switch (item.Code) {
            case "VATTYPEPACKAGES":
            case "VATTYPECONTAINERS":
                return this.ShowVATDetails;
                break;
            case "VATPERCENTAGEPACKAGES":
            case "VATPERCENTAGECONTAINERS":
                return this.ShowVATDetails;
                break;
            case "ISREGIONALTAXPACKAGES":
            case "ISREGIONALTAXCONTAINERS":
                return this.DisplayRegoinalTax;
                break;
            case "UNITSPACKAGES":
            case "FIXEDPRICECONTAINERS":
                return (!this.IsRoutingRates && this.QuoteTemplateSectionTypeName == 'Packages') || this.QuoteTemplateSectionTypeName != 'Packages';
                break;
            default: return true;
        }
    }

    private GetItemSettingsDataDisplayTextCode(item) {
        switch (item.Code) {
            case "HEADERPACKAGES":
            case "HEADERCONTAINERS":
                return "QuoteTemplate.S.ShowHeaderLabels";
                break;
            case "CHARGEPACKAGES":
            case "CHARGECONTAINERS":
                return "QuoteTemplate.S.ShowChargeName";
                break;
            case "CHARGECODEPACKAGES":
            case "CHARGECODECONTAINERS":
                return "QuoteTemplate.S.ShowChargeCode";
                break;
            case "MEASUREMENTPACKAGES":
            case "MEASUREMENTCONTAINERS":
                return "QuoteTemplate.S.ShowMeasurement";
                break;
            case "UNITSPACKAGES":
            case "FIXEDPRICECONTAINERS":
                return this.QuoteTemplateSectionTypeName == "Packages" ? "QuoteTemplate.S.ShowUnits" : "QuoteTemplate.S.ShowFixedPrice";
                break;
            case "UNITPRICEPACKAGES":
            case "PRICEBYCONTAINERS":
                return this.QuoteTemplateSectionTypeName == "Packages" ? "QuoteTemplate.S.ShowUnitPrice" : "QuoteTemplate.S.ShowPriceByContainer";
                break;
            case "TOTALPACKAGES":
            case "TOTALCONTAINERS":
                return "QuoteTemplate.S.ShowSaleCurrencyColumn";
                break;
            case "LOCALAMOUNTPACKAGES":
            case "LOCALAMOUNTCONTAINERS":
                return "QuoteTemplate.S.ShowLocalCurrencyColumn";
                break;
            case "CHARGEDESCRIPTIONPACKAGES":
            case "CHARGEDESCRIPTIONCONTAINERS":
                return "QuoteTemplate.S.ShowChargeDescription";
                break;
            case "CHARGENOTEPACKAGES":
            case "CHARGENOTECONTAINERS":
                return "QuoteTemplate.S.ShowChargeNote";
                break;
            case "SALEMINMAXPACKAGES":
            case "SALEMINMAXCONTAINERS":
                return "QuoteTemplate.S.ShowSaleMinMax";
                break;
            case "INCLUDEDCHARGESPACKAGES":
            case "INCLUDEDCHARGESCONTAINERS":
                return "QuoteTemplate.S.ShowIncludedCharges";
                break;
            case "ISREGIONALTAXPACKAGES":
            case "ISREGIONALTAXCONTAINERS":
                return "QuoteTemplate.S.ShowRegionalTax";
                break;
            case "VATTYPEPACKAGES":
            case "VATTYPECONTAINERS":
                return "QuoteTemplate.S.ShowVATType";
                break;
            case "VATPERCENTAGEPACKAGES":
            case "VATPERCENTAGECONTAINERS":
                return "QuoteTemplate.S.ShowVATPercentage";
                break;
        }

        if (item.Code == "ShowVATType") return this.ShowVATDetails;
        if (item.Code == "ShowVATPercentage") return this.ShowVATDetails;
        if (item.Code == "ShowRegionalTax") return this.DisplayRegoinalTax;
        if (item.Code == "ShowPrice1Key") return (!this.IsRoutingRates && this.QuoteTemplateSectionTypeName == 'Packages') || this.QuoteTemplateSectionTypeName != 'Packages';
        return true;
    }

    ArrowUpButtonClicked(currentItem: PricesFieldSettings) {
        let currentIndex: number = this.QuoteTemplatePricesTableSettingsData.indexOf(currentItem);
        var previousItem = this.QuoteTemplatePricesTableSettingsData[currentIndex - 1];
        if (currentIndex <= 0) return;
        this.QuoteTemplatePricesTableSettingsData = this.QuoteTemplatePricesTableSettingsData.filter(d => d.Code != previousItem.Code);
        let tempOrder: number = currentItem.Index;
        currentItem.Index = previousItem.Index;
        previousItem.Index = tempOrder;

        this.QuoteTemplatePricesTableSettingsData.splice(currentIndex, 0, previousItem);
        if (this.QuoteTemplateSectionTypeName == "Packages") this.QuoteTemplateSettingPM.QuoteTemplateSettingData.PricesPackagesTableSettings = this.QuoteTemplatePricesTableSettingsData;
        else this.QuoteTemplateSettingPM.QuoteTemplateSettingData.PricesContainersTableSettings = this.QuoteTemplatePricesTableSettingsData;
    }

    ArrowDownButtonClicked(currentItem: PricesFieldSettings) {
        let currentIndex: number = this.QuoteTemplatePricesTableSettingsData.indexOf(currentItem);
        var nextItem = this.QuoteTemplatePricesTableSettingsData[currentIndex + 1];
        if (currentIndex >= this.QuoteTemplatePricesTableSettingsData.length - 1) return;
        this.QuoteTemplatePricesTableSettingsData = this.QuoteTemplatePricesTableSettingsData.filter(d => d.Code != nextItem.Code);
        let tempOrder: number = currentItem.Index;
        currentItem.Index = nextItem.Index;
        nextItem.Index = tempOrder;

        this.QuoteTemplatePricesTableSettingsData.splice(currentIndex, 0, nextItem);
        if (this.QuoteTemplateSectionTypeName == "Packages") this.QuoteTemplateSettingPM.QuoteTemplateSettingData.PricesPackagesTableSettings = this.QuoteTemplatePricesTableSettingsData;
        else this.QuoteTemplateSettingPM.QuoteTemplateSettingData.PricesContainersTableSettings = this.QuoteTemplatePricesTableSettingsData;
    }

    quoteTemplatePricesTableSettingsData: PricesFieldSettings[];
    get QuoteTemplatePricesTableSettingsData() {
        return this.quoteTemplatePricesTableSettingsData;
    }
    set QuoteTemplatePricesTableSettingsData(value: PricesFieldSettings[]) {
        if (value != null) {
            this.QuoteTemplateSettingPM.IsDirty = true;
            this.quoteTemplatePricesTableSettingsData = value;
        }
    }

    BuildItemsSource() {

        
        var itemsCollection: TextCodeData[] = [];

        this.QuoteTemplateTextCodePMList.forEach((item) => {
            itemsCollection.push(new TextCodeData(item));
        })
        this.ItemsSource.AppendCollection(itemsCollection);
 
    }


    LoadData() {

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.QuoteTemplateTextDesignPMLists = [];
        this.LoadTableDesign();
    }


    LoadTextDesign() {
        var totalsLabelTextDesignId: string = (this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.TotalsPackagesLabelDesignId : this.QuoteTemplateSettingPM.TotalsContainsersLabelDesignId);

        var totalValueTextDesignId = (this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.TotalsPackagesValueDesignId : this.QuoteTemplateSettingPM.TotalsContainsersValueDesignId);

    
        var titleTextDesignId: string = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.PricingPackagesTitleDesignId : this.QuoteTemplateSettingPM.PricingContainsersTitleDesignId;


        var groupByHeaderTextDesignId: string = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.GroupByPackagesValueDesignId : this.QuoteTemplateSettingPM.GroupByContainsersValueDesignId;

        var groupByTotailTextDesignId: string = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.GroupByPackagesLabelDesignId : this.QuoteTemplateSettingPM.GroupByContainsersLabelDesignId;


   
        var ids: string = totalsLabelTextDesignId;
        ids += ("," + totalValueTextDesignId);
        ids += ("," + groupByHeaderTextDesignId);
        ids += ("," + titleTextDesignId);
        ids += ("," + groupByTotailTextDesignId);
        
        if (this.TableDesignPM) {
            ids += ("," + this.TableDesignPM.HeaderDesignId);
         ids += ("," + this.TableDesignPM.LinesDesignId);
        }

        this.quoteTemplateTextDesignExtendedPMService.GetQuoteTemplateTextDesignPMListByIds(ids, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteTemplateTextDesignPMLists = pmResponse.Result;

                if (this.TableDesignPM) {
                    this.HeaderTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.HeaderDesignId)[0];
                    if (this.HeaderTextDesignPM) {
                        this.HeaderTextDesignPM.Title = "Header";
                    }

                    this.RowTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.LinesDesignId)[0];
                    if (this.RowTextDesignPM) {
                        this.RowTextDesignPM.Title = "Rows";
                        //this.RowTextDesignPM.HideAlignment = true;
                    }
                }
                this.TotalLabelTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == totalsLabelTextDesignId)[0];
                if (this.TotalLabelTextDesignPM) {
                   
                    this.TotalLabelTextDesignPM.Title = "Label";
                    //this.TotalLabelTextDesignPM.HideAlignment = true;
                }

                this.TotalValueTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == totalValueTextDesignId)[0];
                if (this.TotalValueTextDesignPM) {
                   // this.TotalValueTextDesignPM.HideAlignment = true;
                    this.TotalValueTextDesignPM.Title = "Value";
                    this.TotalValueTextDesignPM.HideAlignment = true;
            
                }


                this.GroupByHeaderTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == groupByHeaderTextDesignId)[0];
                if (this.GroupByHeaderTextDesignPM) {
                    this.GroupByHeaderTextDesignPM.Title = "Header";
                }

                this.GroupByTotalTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == groupByTotailTextDesignId)[0];
                if (this.GroupByTotalTextDesignPM) {
                   // this.GroupByTotalTextDesignPM.HideAlignment = true;
                    this.GroupByTotalTextDesignPM.Title = "Total";
                }


                this.TitleTextDesignPM = this.QuoteTemplateTextDesignPMLists.filter(d => d.Id == titleTextDesignId)[0];
                if (this.TitleTextDesignPM) {
                    this.TitleTextDesignPM.Title = "Title";
                }

            }

            this.IsLoadPage = true;
        });


    }

   
    //Load Table Design

    LoadTableDesign() {
        var tableDesignId: string = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.PackagesTableDesignId : this.QuoteTemplateSettingPM.ContainserTableDesignId;

        this.quoteTemplateTableDesignPMService.get(tableDesignId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.TableDesignPM = pmResponse.Result;
            }
            this.LoadTextDesign();

        });
    }


    //Prop setting 
    ShowTotalInLocalCurrencyKey: string = Guid.newGuid();
    get ShowTotalInLocalCurrency() {
        var showTotalInLocalCurrency: boolean = false;
        if (this.QuoteTemplateSettingPM) showTotalInLocalCurrency = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowTotalInLocalCurrencyPackages : this.QuoteTemplateSettingPM.ShowTotalInLocalCurrencyContainers;
        return showTotalInLocalCurrency;
    }
    set ShowTotalInLocalCurrency(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowTotalInLocalCurrencyPackages = value;
            } else this.QuoteTemplateSettingPM.ShowTotalInLocalCurrencyContainers = value;
        }
    }


    ShowTotalInSaleCurrencyKey: string = Guid.newGuid();
    get ShowTotalInSaleCurrency() {
        var showTotalInSaleCurrency: boolean = false;
        if (this.QuoteTemplateSettingPM) showTotalInSaleCurrency = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowTotalInSaleCurrencyPackages : this.QuoteTemplateSettingPM.ShowTotalInSaleCurrencyContainers;
        return showTotalInSaleCurrency;
    }
    set ShowTotalInSaleCurrency(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowTotalInSaleCurrencyPackages = value;
            } else this.QuoteTemplateSettingPM.ShowTotalInSaleCurrencyContainers = value;
        }
    }

    ShowTitlePricingKey: string = Guid.newGuid();
    get ShowTitlePricing() {
        var showTitlePricing: boolean = false;
        if (this.QuoteTemplateSettingPM) showTitlePricing = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowTitlePricingPackages : this.QuoteTemplateSettingPM.ShowTitlePricingContainsers;
        return showTitlePricing;
    }
    set ShowTitlePricing(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowTitlePricingPackages = value;
            } else this.QuoteTemplateSettingPM.ShowTitlePricingContainsers = value;
        }
    }


    ShowPricesTableKey: string = Guid.newGuid();
    get ShowPricesTable() {
        var showPricesTable: boolean = false;
        if (this.QuoteTemplateSettingPM) showPricesTable = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowPricesTablePackages : this.QuoteTemplateSettingPM.ShowPricesTableContainers;
        if (!showPricesTable) this.DisablePricingSetting();
        return showPricesTable;
    }
    set ShowPricesTable(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowPricesTablePackages = value;
            } else this.QuoteTemplateSettingPM.ShowPricesTableContainers = value;


            if (!value) this.DisablePricingSetting();
        }
    }


    SplitChargesbyGroupsKey: string = Guid.newGuid();
    get SplitChargesbyGroups() {
        var splitChargesbyGroups: boolean = false;
        if (this.QuoteTemplateSettingPM) splitChargesbyGroups = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.SplitChargesbyGroupsPackages : this.QuoteTemplateSettingPM.SplitChargesbyGroupsContainers;
        return splitChargesbyGroups;
    }
    set SplitChargesbyGroups(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.SplitChargesbyGroupsPackages = value;
            } else this.QuoteTemplateSettingPM.SplitChargesbyGroupsContainers = value;

            if (!value) {
                this.ShowTotalPerChargeGroup = false;
            
            }
        }
    }


    ShowChargeCodeKey: string = Guid.newGuid();
    get ShowChargeCode() {
        var showChargeCode: boolean = false;
        if (this.QuoteTemplateSettingPM) showChargeCode = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowChargeCodePackages : this.QuoteTemplateSettingPM.ShowChargeCodeContainers;
        return showChargeCode;
    }
    set ShowChargeCode(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowChargeCodePackages = value;
            } else this.QuoteTemplateSettingPM.ShowChargeCodeContainers = value;
        }
    }



    ShowChargeNameKey: string = Guid.newGuid();
    get ShowChargeName() {
        var showChargeName: boolean = false;
        if (this.QuoteTemplateSettingPM) showChargeName = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowChargeNamePackages : this.QuoteTemplateSettingPM.ShowChargeNameContainers;
        return showChargeName;
    }
    set ShowChargeName(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowChargeNamePackages = value;
            } else this.QuoteTemplateSettingPM.ShowChargeNameContainers = value;
        }
    }

    ShowMeasurementKey: string = Guid.newGuid();
    get ShowMeasurement() {
        var showMeasurement: boolean = false;
        if (this.QuoteTemplateSettingPM) showMeasurement = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowMeasurementPackages : this.QuoteTemplateSettingPM.ShowMeasurementContainers;
        return showMeasurement;
    }
    set ShowMeasurement(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowMeasurementPackages = value;
            } else this.QuoteTemplateSettingPM.ShowMeasurementContainers = value;
        }
    }





    get ShowPrice1Label() {

        var showPrice1Label = "";
        if (this.QuoteTemplateSectionTypeName == "Packages") {
            showPrice1Label = TextCodeTranslator.Translate("QuoteTemplate.S.ShowUnits"); 
        } else showPrice1Label = TextCodeTranslator.Translate("QuoteTemplate.S.ShowFixedPrice"); 

        return showPrice1Label;
    }




  
    ShowPrice1Key: string = Guid.newGuid();
    get ShowPrice1() {
        var showPrice1: boolean = false;
        if (this.QuoteTemplateSettingPM) showPrice1 = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowUnitsPackages : this.QuoteTemplateSettingPM.ShowFixedPriceContainers;
        return showPrice1;
    }
    set ShowPrice1(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowUnitsPackages = value;
            } else this.QuoteTemplateSettingPM.ShowFixedPriceContainers = value;
        }
    }





    ShowIncludedChargesKey: string = Guid.newGuid();
    get ShowIncludedCharges() {
        var showIncludedCharges: boolean = false;
        if (this.QuoteTemplateSettingPM) showIncludedCharges = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowIncludedChargesPackages : this.QuoteTemplateSettingPM.ShowIncludedChargesContainers;
        return showIncludedCharges;
    }
    set ShowIncludedCharges(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowIncludedChargesPackages = value;
            } else this.QuoteTemplateSettingPM.ShowIncludedChargesContainers = value;
        }
    }














    get ShowPrice2Label() {

        var showPrice2Label = "";
        if (this.QuoteTemplateSectionTypeName == "Packages") {
            showPrice2Label = TextCodeTranslator.Translate("QuoteTemplate.S.ShowUnitPrice");
        } else showPrice2Label = TextCodeTranslator.Translate("QuoteTemplate.S.ShowPriceByContainer");

        return showPrice2Label;
    }

    ShowPrice2Key: string = Guid.newGuid();
    get ShowPrice2() {
        var showPrice2: boolean = false;
        if (this.QuoteTemplateSettingPM) showPrice2 = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowUnitPricePackages : this.QuoteTemplateSettingPM.ShowPriceByContainerColumn;
        return showPrice2;
    }
    set ShowPrice2(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowUnitPricePackages = value;
            } else this.QuoteTemplateSettingPM.ShowPriceByContainerColumn = value;
        }
    }

    ShowSaleCurrencyColumnKey: string = Guid.newGuid();
    get ShowSaleCurrencyColumn() {
        var showSaleCurrencyColumn: boolean = false;
        if (this.QuoteTemplateSettingPM) showSaleCurrencyColumn = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowSaleCurrencyColumnPackages : this.QuoteTemplateSettingPM.ShowSaleCurrencyColumnContainers;
        return showSaleCurrencyColumn;
    }
    set ShowSaleCurrencyColumn(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {


            if (!value) this.ShowTotalPerChargeGroup = false;
            
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowSaleCurrencyColumnPackages = value;
            } else this.QuoteTemplateSettingPM.ShowSaleCurrencyColumnContainers = value;
        }
    }


    ShowLocalCurrencyColumnKey: string = Guid.newGuid();
    get ShowLocalCurrencyColumn() {
        var showLocalCurrencyColumn: boolean = false;
        if (this.QuoteTemplateSettingPM) showLocalCurrencyColumn = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowLocalCurrencyColumnPackages : this.QuoteTemplateSettingPM.ShowLocalCurrencyColumnContainers;
        return showLocalCurrencyColumn;
    }
    set ShowLocalCurrencyColumn(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowLocalCurrencyColumnPackages = value;
            } else this.QuoteTemplateSettingPM.ShowLocalCurrencyColumnContainers = value;
        }
    }

    ShowChargeDescriptionKey: string = Guid.newGuid();
    get ShowChargeDescription() {
        var showChargeDescription: boolean = false;
        if (this.QuoteTemplateSettingPM) showChargeDescription = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowChargeDescriptionPackages : this.QuoteTemplateSettingPM.ShowChargeDescriptionContainers;
        return showChargeDescription;
    }
    set ShowChargeDescription(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowChargeDescriptionPackages = value;
            } else this.QuoteTemplateSettingPM.ShowChargeDescriptionContainers = value;
        }
    }



    ShowTotalPerChargeGroupKey: string = Guid.newGuid();
    get ShowTotalPerChargeGroup() {
        var showTotalPerChargeGroup: boolean = false;
        if (this.QuoteTemplateSettingPM) showTotalPerChargeGroup = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowTotalPerChargeGroupPackages : this.QuoteTemplateSettingPM.ShowTotalPerChargeGroupContainers;
        return showTotalPerChargeGroup;
    }
    set ShowTotalPerChargeGroup(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") this.QuoteTemplateSettingPM.ShowTotalPerChargeGroupPackages = value;
            else this.QuoteTemplateSettingPM.ShowTotalPerChargeGroupContainers = value;
        }
    }

    

    ShowChargeNoteColumnKey: string = Guid.newGuid();
    get ShowChargeNoteColumn() {
        var showChargeNoteColumn: boolean = false;
        if (this.QuoteTemplateSettingPM) showChargeNoteColumn = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowChargeNotePackages : this.QuoteTemplateSettingPM.ShowChargeNoteContainers;
        return showChargeNoteColumn;
    }
    set ShowChargeNoteColumn(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") this.QuoteTemplateSettingPM.ShowChargeNotePackages = value;
            else this.QuoteTemplateSettingPM.ShowChargeNoteContainers = value;
        }
    }


    ShowSaleMaxMinAmountColumnKey: string = Guid.newGuid();
    get ShowSaleMaxMinAmountColumn() {
        var showSaleMaxMinAmountColumn: boolean = false;
        if (this.QuoteTemplateSettingPM) showSaleMaxMinAmountColumn = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowSaleMaxMinAmountPackages : this.QuoteTemplateSettingPM.ShowSaleMaxMinAmountContainers;
        return showSaleMaxMinAmountColumn;
    }
    set ShowSaleMaxMinAmountColumn(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") this.QuoteTemplateSettingPM.ShowSaleMaxMinAmountPackages = value;
            else this.QuoteTemplateSettingPM.ShowSaleMaxMinAmountContainers = value;
        }
    }


    ShowVATTypeKey: string = Guid.newGuid();
    get ShowVATType() {
        var showVATType: boolean = false;
        if (this.QuoteTemplateSettingPM) showVATType = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowVATTypePackages : this.QuoteTemplateSettingPM.ShowVATTypeContainers;
        return showVATType;
    }
    set ShowVATType(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowVATTypePackages = value;
            } else this.QuoteTemplateSettingPM.ShowVATTypeContainers = value;
        }
    }


    ShowRegionalTAXKey: string = Guid.newGuid();
    get ShowRegionalTAX() {
        var showRegionalTAX: boolean = false;
        if (this.QuoteTemplateSettingPM) showRegionalTAX = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowRegionalTAXPackages : this.QuoteTemplateSettingPM.ShowRegionalTAXContainers;
        return showRegionalTAX;
    }
    set ShowRegionalTAX(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowRegionalTAXPackages = value;
            } else this.QuoteTemplateSettingPM.ShowRegionalTAXContainers = value;
        }
    }


    ShowVATPercentageKey: string = Guid.newGuid();
    get ShowVATPercentage() {
        var showVATPercentage: boolean = false;
        if (this.QuoteTemplateSettingPM) showVATPercentage = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowVATPercentagePackages : this.QuoteTemplateSettingPM.ShowVATPercentageContainers;
        return showVATPercentage;
    }
    set ShowVATPercentage(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") {
                this.QuoteTemplateSettingPM.ShowVATPercentagePackages = value;
            } else this.QuoteTemplateSettingPM.ShowVATPercentageContainers = value;
        }
    }



    ShowHeaderLabelsKey: string = Guid.newGuid();
    get ShowHeaderLabels() {
        var showHeaderLabels: boolean = false;
        if (this.QuoteTemplateSettingPM) showHeaderLabels = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.ShowHeaderLabelsPackages : this.QuoteTemplateSettingPM.ShowHeaderLabelsContainers;
        return showHeaderLabels;
    }
    set ShowHeaderLabels(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") this.QuoteTemplateSettingPM.ShowHeaderLabelsPackages = value;
            else this.QuoteTemplateSettingPM.ShowHeaderLabelsContainers = value;
        }
    }




    get SpaceLinesBeforeTable() {
        var spaceLinesBeforeTable: number = 1;
        if (this.QuoteTemplateSettingPM) spaceLinesBeforeTable = this.QuoteTemplateSectionTypeName == "Packages" ? this.QuoteTemplateSettingPM.SpaceLinesBeforePackages : this.QuoteTemplateSettingPM.SpaceLinesBeforeContainers;
        return spaceLinesBeforeTable;
    }
    set SpaceLinesBefore(value: number) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSectionTypeName == "Packages") this.QuoteTemplateSettingPM.SpaceLinesBeforePackages = value;
            else this.QuoteTemplateSettingPM.SpaceLinesBeforeContainers = value;
        }
    }








    DisablePricingSetting() {
        //this.ShowChargeCode = false;
        //this.ShowChargeName = false;
        //this.ShowMeasurement = false;
        //this.ShowPrice1 = false;
        //this.ShowPrice2 = false;
        //this.ShowSaleCurrencyColumn = false;
        //this.ShowLocalCurrencyColumn = false;
        //this.ShowChargeDescription = false;
        //this.ShowSaleMaxMinAmountColumn = false;
        //this.ShowHeaderLabels = false;
    
    }


   // End Prop setting 

    SaveButtonClicked() {
        this.ValidationErrorsList = [];


        var textDesignPmLists = this.QuoteTemplateTextDesignPMLists.filter(d => d.IsDirty == true);
        if (textDesignPmLists.length > 0) this.IsSaveQuoteTemplateTextDesignRuning = true;
        if (this.TableDesignPM.IsDirty) this.IsSaveQuoteTemplateTableDesignRuning = true;
     


        var textCodeDataLists: TextCodeData[] = this.ItemsSource.Collection.filter(d => d.EntityPM.IsDirty == true);
        if (textCodeDataLists.length > 0) this.IsSaveQuoteTemplateTextCodeRuning = true;



        if (this.IsSaveQuoteTemplateTextDesignRuning || this.IsSaveQuoteTemplateTableDesignRuning || this.IsSaveQuoteTemplateTextCodeRuning) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));

            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.MapIsUsedFieldToCurrentDBFields();
                    
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe((res:any) => {
                    this.QuoteTemplateSettingPM.IsDirty = false;
                    this.SaveOthers(textDesignPmLists, textCodeDataLists);
                });
            } else this.SaveOthers(textDesignPmLists, textCodeDataLists);



        } else {

            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.SaveQuoteTemplateSetting();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                if (this.IsPerContainerChange == true) {
                    this.CurrentSession.CurrentWindow.Close("Refresh");
                } else this.CurrentSession.CloseCurrentWindow();

            }

               
           
        }    






    }


    private MapIsUsedFieldToCurrentDBFields() {
        this.QuoteTemplatePricesTableSettingsData.forEach((item) => {
            if(this.InExcludedQuoteTemplatePricesPackagesTableSettingsData(item.Name)) return;
            this.QuoteTemplateSettingPM[item.Name] = item.InUse;
        });
    }
    ExcludedQuoteTemplatePricesPackagesTableSettingsData:string[]=["ShowTotalInSaleCurrencyContainers","ShowTotalInSaleCurrencyPackages","ShowTotalInLocalCurrencyContainers","ShowTotalInLocalCurrencyPackages"];
    InExcludedQuoteTemplatePricesPackagesTableSettingsData(item: string): boolean {
        return this.ExcludedQuoteTemplatePricesPackagesTableSettingsData.some(x=>x==item);
    }

    SaveOthers(textDesignPmLists: any[], textCodeDataLists:any[]) {

        if (this.IsSaveQuoteTemplateTextDesignRuning) this.SaveQuoteTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteTemplateTableDesignRuning) this.SaveQuoteTemplateTableDesign();
        if (this.IsSaveQuoteTemplateTextCodeRuning) this.SaveQuoteTemplateTextCode(textCodeDataLists);
    }

    SaveQuoteTemplateTextDesign(items: any) {

        items.forEach((item) => {item.IsDirty = false; });
           
        
        this.quoteTemplateTextDesignExtendedPMService.updateQuoteTemplateTextDesignPMs(items).subscribe((res:any) => {
            this.IsSaveQuoteTemplateTextDesignRuning = false;
            this.SaveCompleted();

        });
        
    }

    SaveQuoteTemplateTableDesign() {

        this.quoteTemplateTableDesignPMService.update(this.TableDesignPM).subscribe((res:any) => {
            this.IsSaveQuoteTemplateTableDesignRuning = false;
            this.SaveCompleted();

        });
    }

    SaveQuoteTemplateSetting() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe((res:any) => {
            this.QuoteTemplateSettingPM.IsDirty = false;
            this.SaveCompleted();

        });

    }

    SaveQuoteTemplateTextCode(items: TextCodeData[]) {

        var quoteTemplateTextCodePMLists: QuoteTemplateTextCodePM[] = [];
        items.forEach((item) => {
            if (item.EntityPM) {
                item.EntityPM.IsDirty = false;
                quoteTemplateTextCodePMLists.push(item.EntityPM);
            }
         });


        this.quoteTemplateTextCodeExtendedPMService.updateTextCodes(quoteTemplateTextCodePMLists).subscribe((res:any) => {
            this.IsSaveQuoteTemplateTextCodeRuning = false;
            this.SaveCompleted();

        });
 
    }

    ShowTotalPerContainer() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteTemplatePM = this.EntityPM;
        windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
        windowArgs.QuoteTemplateTextCodePMList = this.AllQuoteTemplateTextCodePMList;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 930;
        logWindow.Height = 610;
        logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.TotalPerContainerSettings")  ;
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/QuoteTemplateTotalPerContainerSetting");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "Refresh") {
                this.IsPerContainerChange = true;
            }
        });

    }


    SaveCompleted() {
        if (!this.IsSaveQuoteTemplateTextDesignRuning && !this.IsSaveQuoteTemplateTableDesignRuning && !this.IsSaveQuoteTemplateTextCodeRuning) {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentWindow.Close("Refresh");
         
        }

    }

    CloseButtonClicked() {


        this.CurrentSession.CloseCurrentWindow();
    }



}


export class TextCodeData extends BaseComponent {
    public DataContext: TextCodeData = this;
    EntityPM: QuoteTemplateTextCodePM;
    ToolTipEnglishNameMessage: string="";
    ToolTipLocalNameMessage: string = "";

    get EnglishName() {
        var englishName: string = "";
        if (this.EntityPM) englishName = this.EntityPM.EnglishName;
        return englishName;
    }
    set EnglishName(value: string) {
        if (this.EntityPM != null) {
            this.EntityPM.EnglishName = value;
        }
    }



    get LocalName() {
        var localName: string = "";
        if (this.EntityPM) localName = this.EntityPM.LocalName;
        return localName;
    }
    set LocalName(value: string) {
        if (this.EntityPM != null) {
            this.EntityPM.LocalName = value;
        }
    }







    get IsShowRestoreOriginalEnglishName() {
        var isShowRestoreOriginalEnglishName: boolean = false;
        this.ToolTipEnglishNameMessage = "";
        if (this.EntityPM) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginalEnglishName)) {
                if (this.EntityPM.OriginalEnglishName != this.EntityPM.EnglishName) {
                    isShowRestoreOriginalEnglishName = true;

                    this.ToolTipEnglishNameMessage = TextCodeTranslator.Translate("QuoteTemplate.M.ValueEditedByUserMessage") + " {" + this.EntityPM.OriginalEnglishName + "}";
                    
                }

            }
        }
        return isShowRestoreOriginalEnglishName;
    }
   



    get IsShowRestoreOriginalLocalName() {
        var isShowRestoreOriginalLocalName: boolean = false;
        this.ToolTipLocalNameMessage = "";
        if (this.EntityPM) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.OriginalLocalName)) {
                if (this.EntityPM.OriginalLocalName != this.EntityPM.LocalName) {
                    isShowRestoreOriginalLocalName = true;

                    this.ToolTipLocalNameMessage = "Value edited by user, double click to reset to {" + this.EntityPM.OriginalLocalName+ "}";

         
                }

            }
        }
        return isShowRestoreOriginalLocalName;
    }


    constructor(entity: QuoteTemplateTextCodePM) {
        super();

        this.EntityPM = entity;

        
    }

    RestoreEnglishName(item: TextCodeData) {
        item.EnglishName = item.EntityPM.OriginalEnglishName;
    }

    RestoreLocalName(item: TextCodeData) {
        item.LocalName = item.EntityPM.OriginalLocalName;
    }
}
