import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplatePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplatePM';
import {QuoteOPTemplateSettingPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {QuoteOPTemplateTextDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextDesignPM';
import {QuoteOPTemplateTableDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTableDesignPM';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {QuoteOPTemplateTextCodePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextCodePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateSettingPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {QuoteOPTemplateTableDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTableDesignPMService';
import {QuoteOPTemplateTextDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTextDesignPMService';
import {QuoteOPTemplateTextDesignExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateTextDesignExtendedPMService';
import {QuoteOPTemplateTextCodeExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateTextCodeExtendedPMService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {QuoteOPPM} from '../../../QuoteOPM/EntityPMs/QuoteOPPM';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
@Component({
    selector: 'QuoteTemplatePricingSettingComponent',
    
    templateUrl: './QuoteTemplatePricingSettingComponent.html',
})

export class QuoteTemplatePricingSettingComponent extends BaseComponent implements OnInit {
    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService;
    QuoteOPTemplateTextDesignPMService: QuoteOPTemplateTextDesignPMService;
    QuoteOPTemplateTableDesignPMService: QuoteOPTemplateTableDesignPMService;
    QuoteOPTemplateTextDesignExtendedPMService: QuoteOPTemplateTextDesignExtendedPMService;
    QuoteOPTemplateTextCodeExtendedPMService: QuoteOPTemplateTextCodeExtendedPMService;
    QuoteOPTemplatePM: QuoteOPTemplatePM;
    IsLoadPage: boolean;

    Alignment: string[] = [];
    QuoteOPTemplateSettingPM: QuoteOPTemplateSettingPM;

    public ValidationErrorsList: string[];

    TableDesignPM: QuoteOPTemplateTableDesignPM;
    HeaderTextDesignPM: QuoteOPTemplateTextDesignPM;
    RowTextDesignPM: QuoteOPTemplateTextDesignPM;

    QuoteOPTemplateTextDesignPMLists: QuoteOPTemplateTextDesignPM[] = [];
    QuoteOPTemplateTextCodePMList: QuoteOPTemplateTextCodePM[] = [];
    AllQuoteOPTemplateTextCodePMList: QuoteOPTemplateTextCodePM[] = [];

    TotalLabelTextDesignPM: QuoteOPTemplateTextDesignPM;
    TotalValueTextDesignPM: QuoteOPTemplateTextDesignPM;


    GroupByHeaderTextDesignPM: QuoteOPTemplateTextDesignPM;
    GroupByTotalTextDesignPM: QuoteOPTemplateTextDesignPM;

    TitleTextDesignPM: QuoteOPTemplateTextDesignPM;
    IsSaveQuoteOPTemplateTextDesignRuning: boolean = false;
    IsSaveQuoteOPTemplateTableDesignRuning: boolean = false;
    IsSaveQuoteOPTemplateTextCodeRuning: boolean = false;
    public ItemsSource: ObservableCollection;
    QuoteOPTemplateSectionTypeName: string = "Packages";
    IsPerContainerChange: boolean = false;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    ShowTotalPerContinerLink: boolean = false;
    ShowVATDetails :boolean = false;
    DisplayRegoinalTax: boolean = false;

    constructor() {
        super();
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();
        this.QuoteOPTemplateTableDesignPMService = new QuoteOPTemplateTableDesignPMService();
        this.QuoteOPTemplateTextDesignPMService = new QuoteOPTemplateTextDesignPMService();
        this.QuoteOPTemplateTextDesignExtendedPMService = new QuoteOPTemplateTextDesignExtendedPMService();
        this.QuoteOPTemplateTextCodeExtendedPMService =new QuoteOPTemplateTextCodeExtendedPMService();
        this.ItemsSource = new ObservableCollection([]);


        if (FeatureLocator.HasFeaturePermession("QuoteOP", "TOTALPERCONTAINER")) this.ShowTotalPerContinerLink = true;
        if (SessionLocator.AccountingSettingPM.AllowRegionalTaxManagement) this.DisplayRegoinalTax = true;

    }

    ngOnInit() {

    }

    QuoteOPPM: QuoteOPPM;
    SelectedTabCode: string;
    IsRoutingRates: boolean = false;
    SetWindowArgs(args: any) {
        this.SelectedTabCode = "PRT";
        this.QuoteOPTemplatePM = args.QuoteOPTemplatePM;
        this.QuoteOPTemplateSectionTypeName = args.QuoteOPTemplateSectionTypeName;
        this.QuoteOPTemplateSettingPM = args.QuoteOPTemplateSettingPM;
        this.QuoteOPPM = args.QuoteOPPM;

        if (((this.QuoteOPPM && this.QuoteOPPM.IsChargesByVAT) || !this.QuoteOPPM) && FeatureLocator.HasFeaturePermession("QuoteOP", "VATDetAILSINQUOTATION")) {
                this.ShowVATDetails = true;
            }
        


        this.IsRoutingRates = this.QuoteOPTemplatePM != null ? this.QuoteOPTemplatePM.TemplateTypeCode == "P" ? true : false : false;
        this.Alignment.push("Left"); this.Alignment.push("Center"); this.Alignment.push("Right");


        if (args.QuoteOPTemplateTextCodePMList) {
            this.QuoteOPTemplateTextCodePMList = args.QuoteOPTemplateTextCodePMList.filter(d => d.Area == this.QuoteOPTemplateSectionTypeName);
            if (this.IsRoutingRates) {
                this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != "UNITSPACKAGES" && d.TextCode != "TOTALAMOUNTS");
            }
            if (!this.ShowVATDetails) {
                this.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodePMList.filter(d => d.TextCode != "VATTYPEPACKAGES" && d.TextCode != "VATTYPECONTAINERS" && d.TextCode != "VATPERCENTAGEPACKAGES" && d.TextCode != "VATPERCENTAGECONTAINERS");
            }


            this.AllQuoteOPTemplateTextCodePMList = args.QuoteOPTemplateTextCodePMList;

            this.BuildItemsSource();
        }
        
        this.LoadData();
    }


    BuildItemsSource() {


        var itemsCollection: TextCodeData[] = [];

        this.QuoteOPTemplateTextCodePMList.forEach((item) => {
            itemsCollection.push(new TextCodeData(item));
        })
        this.ItemsSource.AppendCollection(itemsCollection);
 
    }


    LoadData() {

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));
        this.QuoteOPTemplateTextDesignPMLists = [];
        this.LoadTableDesign();
    }


    LoadTextDesign() {
        var totalsLabelTextDesignId: string = (this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.TotalsPackagesLabelDesignId : this.QuoteOPTemplateSettingPM.TotalsContainsersLabelDesignId);

        var totalValueTextDesignId = (this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.TotalsPackagesValueDesignId : this.QuoteOPTemplateSettingPM.TotalsContainsersValueDesignId);

    
        var titleTextDesignId: string = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.PricingPackagesTitleDesignId : this.QuoteOPTemplateSettingPM.PricingContainsersTitleDesignId;


        var groupByHeaderTextDesignId: string = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.GroupByPackagesValueDesignId : this.QuoteOPTemplateSettingPM.GroupByContainsersValueDesignId;

        var groupByTotailTextDesignId: string = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.GroupByPackagesLabelDesignId : this.QuoteOPTemplateSettingPM.GroupByContainsersLabelDesignId;


   
        var ids: string = totalsLabelTextDesignId;
        ids += ("," + totalValueTextDesignId);
        ids += ("," + groupByHeaderTextDesignId);
        ids += ("," + titleTextDesignId);
        ids += ("," + groupByTotailTextDesignId);
        
        if (this.TableDesignPM) {
            ids += ("," + this.TableDesignPM.HeaderDesignId);
         ids += ("," + this.TableDesignPM.LinesDesignId);
        }

        this.QuoteOPTemplateTextDesignExtendedPMService.GetQuoteOPTemplateTextDesignPMListByIds(ids, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteOPTemplateTextDesignPMLists = pmResponse.Result;

                if (this.TableDesignPM) {
                    this.HeaderTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.HeaderDesignId)[0];
                    if (this.HeaderTextDesignPM) {
                        this.HeaderTextDesignPM.Title = "Header";
                    }

                    this.RowTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == this.TableDesignPM.LinesDesignId)[0];
                    if (this.RowTextDesignPM) {
                        this.RowTextDesignPM.Title = "Rows";
                        //this.RowTextDesignPM.HideAlignment = true;
                    }
                }
                this.TotalLabelTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == totalsLabelTextDesignId)[0];
                if (this.TotalLabelTextDesignPM) {
                   
                    this.TotalLabelTextDesignPM.Title = "Label";
                    //this.TotalLabelTextDesignPM.HideAlignment = true;
                }

                this.TotalValueTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == totalValueTextDesignId)[0];
                if (this.TotalValueTextDesignPM) {
                   // this.TotalValueTextDesignPM.HideAlignment = true;
                    this.TotalValueTextDesignPM.Title = "Value";
                    this.TotalValueTextDesignPM.HideAlignment = 'true';
            
                }


                this.GroupByHeaderTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == groupByHeaderTextDesignId)[0];
                if (this.GroupByHeaderTextDesignPM) {
                    this.GroupByHeaderTextDesignPM.Title = "Header";
                }

                this.GroupByTotalTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == groupByTotailTextDesignId)[0];
                if (this.GroupByTotalTextDesignPM) {
                   // this.GroupByTotalTextDesignPM.HideAlignment = true;
                    this.GroupByTotalTextDesignPM.Title = "Total";
                }


                this.TitleTextDesignPM = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.Id == titleTextDesignId)[0];
                if (this.TitleTextDesignPM) {
                    this.TitleTextDesignPM.Title = "Title";
                }

            }

            this.IsLoadPage = true;
        });


    }

   
    //Load Table Design

    LoadTableDesign() {
        var tableDesignId: string = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.PackagesTableDesignId : this.QuoteOPTemplateSettingPM.ContainserTableDesignId;

        this.QuoteOPTemplateTableDesignPMService.get(tableDesignId).subscribe((res:any) => {
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
        if (this.QuoteOPTemplateSettingPM) showTotalInLocalCurrency = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowTotalInLocalCurrencyPackages : this.QuoteOPTemplateSettingPM.ShowTotalInLocalCurrencyContainers;
        return showTotalInLocalCurrency;
    }
    set ShowTotalInLocalCurrency(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowTotalInLocalCurrencyPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowTotalInLocalCurrencyContainers = value;
        }
    }


    ShowTotalInSaleCurrencyKey: string = Guid.newGuid();
    get ShowTotalInSaleCurrency() {
        var showTotalInSaleCurrency: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showTotalInSaleCurrency = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowTotalInSaleCurrencyPackages : this.QuoteOPTemplateSettingPM.ShowTotalInSaleCurrencyContainers;
        return showTotalInSaleCurrency;
    }
    set ShowTotalInSaleCurrency(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowTotalInSaleCurrencyPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowTotalInSaleCurrencyContainers = value;
        }
    }

    ShowTitlePricingKey: string = Guid.newGuid();
    get ShowTitlePricing() {
        var showTitlePricing: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showTitlePricing = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowTitlePricingPackages : this.QuoteOPTemplateSettingPM.ShowTitlePricingContainsers;
        return showTitlePricing;
    }
    set ShowTitlePricing(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowTitlePricingPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowTitlePricingContainsers = value;
        }
    }


    ShowPricesTableKey: string = Guid.newGuid();
    get ShowPricesTable() {
        var showPricesTable: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showPricesTable = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowPricesTablePackages : this.QuoteOPTemplateSettingPM.ShowPricesTableContainers;
        if (!showPricesTable) this.DisablePricingSetting();
        return showPricesTable;
    }
    set ShowPricesTable(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowPricesTablePackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowPricesTableContainers = value;


            if (!value) this.DisablePricingSetting();
        }
    }


    SplitChargesbyGroupsKey: string = Guid.newGuid();
    get SplitChargesbyGroups() {
        var splitChargesbyGroups: boolean = false;
        if (this.QuoteOPTemplateSettingPM) splitChargesbyGroups = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.SplitChargesbyGroupsPackages : this.QuoteOPTemplateSettingPM.SplitChargesbyGroupsContainers;
        return splitChargesbyGroups;
    }
    set SplitChargesbyGroups(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.SplitChargesbyGroupsPackages = value;
            } else this.QuoteOPTemplateSettingPM.SplitChargesbyGroupsContainers = value;

            if (!value) {
                this.ShowTotalPerChargeGroup = false;
            
            }
        }
    }


    ShowChargeCodeKey: string = Guid.newGuid();
    get ShowChargeCode() {
        var showChargeCode: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showChargeCode = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowChargeCodePackages : this.QuoteOPTemplateSettingPM.ShowChargeCodeContainers;
        return showChargeCode;
    }
    set ShowChargeCode(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowChargeCodePackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowChargeCodeContainers = value;
        }
    }



    ShowChargeNameKey: string = Guid.newGuid();
    get ShowChargeName() {
        var showChargeName: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showChargeName = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowChargeNamePackages : this.QuoteOPTemplateSettingPM.ShowChargeNameContainers;
        return showChargeName;
    }
    set ShowChargeName(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowChargeNamePackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowChargeNameContainers = value;
        }
    }

    ShowMeasurementKey: string = Guid.newGuid();
    get ShowMeasurement() {
        var showMeasurement: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showMeasurement = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowMeasurementPackages : this.QuoteOPTemplateSettingPM.ShowMeasurementContainers;
        return showMeasurement;
    }
    set ShowMeasurement(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowMeasurementPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowMeasurementContainers = value;
        }
    }





    get ShowPrice1Label() {

        var showPrice1Label = "";
        if (this.QuoteOPTemplateSectionTypeName == "Packages") {
            showPrice1Label = TextCodeTranslator.Translate("QuoteOPTemplate.S.ShowUnits"); 
        } else showPrice1Label = TextCodeTranslator.Translate("QuoteOPTemplate.S.ShowFixedPrice"); 

        return showPrice1Label;
    }




  
    ShowPrice1Key: string = Guid.newGuid();
    get ShowPrice1() {
        var showPrice1: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showPrice1 = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowUnitsPackages : this.QuoteOPTemplateSettingPM.ShowFixedPriceContainers;
        return showPrice1;
    }
    set ShowPrice1(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowUnitsPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowFixedPriceContainers = value;
        }
    }





    ShowIncludedChargesKey: string = Guid.newGuid();
    get ShowIncludedCharges() {
        var showIncludedCharges: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showIncludedCharges = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowIncludedChargesPackages : this.QuoteOPTemplateSettingPM.ShowIncludedChargesContainers;
        return showIncludedCharges;
    }
    set ShowIncludedCharges(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowIncludedChargesPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowIncludedChargesContainers = value;
        }
    }














    get ShowPrice2Label() {

        var showPrice2Label = "";
        if (this.QuoteOPTemplateSectionTypeName == "Packages") {
            showPrice2Label = TextCodeTranslator.Translate("QuoteOPTemplate.S.ShowUnitPrice");
        } else showPrice2Label = TextCodeTranslator.Translate("QuoteOPTemplate.S.ShowPriceByContainer");

        return showPrice2Label;
    }

    ShowPrice2Key: string = Guid.newGuid();
    get ShowPrice2() {
        var showPrice2: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showPrice2 = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowUnitPricePackages : this.QuoteOPTemplateSettingPM.ShowPriceByContainerColumn;
        return showPrice2;
    }
    set ShowPrice2(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowUnitPricePackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowPriceByContainerColumn = value;
        }
    }

    ShowSaleCurrencyColumnKey: string = Guid.newGuid();
    get ShowSaleCurrencyColumn() {
        var showSaleCurrencyColumn: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showSaleCurrencyColumn = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowSaleCurrencyColumnPackages : this.QuoteOPTemplateSettingPM.ShowSaleCurrencyColumnContainers;
        return showSaleCurrencyColumn;
    }
    set ShowSaleCurrencyColumn(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {


            if (!value) this.ShowTotalPerChargeGroup = false;
            
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowSaleCurrencyColumnPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowSaleCurrencyColumnContainers = value;
        }
    }


    ShowLocalCurrencyColumnKey: string = Guid.newGuid();
    get ShowLocalCurrencyColumn() {
        var showLocalCurrencyColumn: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showLocalCurrencyColumn = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowLocalCurrencyColumnPackages : this.QuoteOPTemplateSettingPM.ShowLocalCurrencyColumnContainers;
        return showLocalCurrencyColumn;
    }
    set ShowLocalCurrencyColumn(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowLocalCurrencyColumnPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowLocalCurrencyColumnContainers = value;
        }
    }

    ShowChargeDescriptionKey: string = Guid.newGuid();
    get ShowChargeDescription() {
        var showChargeDescription: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showChargeDescription = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowChargeDescriptionPackages : this.QuoteOPTemplateSettingPM.ShowChargeDescriptionContainers;
        return showChargeDescription;
    }
    set ShowChargeDescription(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowChargeDescriptionPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowChargeDescriptionContainers = value;
        }
    }



    ShowTotalPerChargeGroupKey: string = Guid.newGuid();
    get ShowTotalPerChargeGroup() {
        var showTotalPerChargeGroup: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showTotalPerChargeGroup = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowTotalPerChargeGroupPackages : this.QuoteOPTemplateSettingPM.ShowTotalPerChargeGroupContainers;
        return showTotalPerChargeGroup;
    }
    set ShowTotalPerChargeGroup(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") this.QuoteOPTemplateSettingPM.ShowTotalPerChargeGroupPackages = value;
            else this.QuoteOPTemplateSettingPM.ShowTotalPerChargeGroupContainers = value;
        }
    }

    

    ShowChargeNoteColumnKey: string = Guid.newGuid();
    get ShowChargeNoteColumn() {
        var showChargeNoteColumn: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showChargeNoteColumn = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowChargeNotePackages : this.QuoteOPTemplateSettingPM.ShowChargeNoteContainers;
        return showChargeNoteColumn;
    }
    set ShowChargeNoteColumn(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") this.QuoteOPTemplateSettingPM.ShowChargeNotePackages = value;
            else this.QuoteOPTemplateSettingPM.ShowChargeNoteContainers = value;
        }
    }


    ShowSaleMaxMinAmountColumnKey: string = Guid.newGuid();
    get ShowSaleMaxMinAmountColumn() {
        var showSaleMaxMinAmountColumn: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showSaleMaxMinAmountColumn = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowSaleMaxMinAmountPackages : this.QuoteOPTemplateSettingPM.ShowSaleMaxMinAmountContainers;
        return showSaleMaxMinAmountColumn;
    }
    set ShowSaleMaxMinAmountColumn(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") this.QuoteOPTemplateSettingPM.ShowSaleMaxMinAmountPackages = value;
            else this.QuoteOPTemplateSettingPM.ShowSaleMaxMinAmountContainers = value;
        }
    }


    ShowVATTypeKey: string = Guid.newGuid();
    get ShowVATType() {
        var showVATType: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showVATType = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowVATTypePackages : this.QuoteOPTemplateSettingPM.ShowVATTypeContainers;
        return showVATType;
    }
    set ShowVATType(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowVATTypePackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowVATTypeContainers = value;
        }
    }


    ShowRegionalTAXKey: string = Guid.newGuid();
    get ShowRegionalTAX() {
        var showRegionalTAX: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showRegionalTAX = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowRegionalTAXPackages : this.QuoteOPTemplateSettingPM.ShowRegionalTAXContainers;
        return showRegionalTAX;
    }
    set ShowRegionalTAX(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowRegionalTAXPackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowRegionalTAXContainers = value;
        }
    }


    ShowVATPercentageKey: string = Guid.newGuid();
    get ShowVATPercentage() {
        var showVATPercentage: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showVATPercentage = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowVATPercentagePackages : this.QuoteOPTemplateSettingPM.ShowVATPercentageContainers;
        return showVATPercentage;
    }
    set ShowVATPercentage(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") {
                this.QuoteOPTemplateSettingPM.ShowVATPercentagePackages = value;
            } else this.QuoteOPTemplateSettingPM.ShowVATPercentageContainers = value;
        }
    }



    ShowHeaderLabelsKey: string = Guid.newGuid();
    get ShowHeaderLabels() {
        var showHeaderLabels: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showHeaderLabels = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.ShowHeaderLabelsPackages : this.QuoteOPTemplateSettingPM.ShowHeaderLabelsContainers;
        return showHeaderLabels;
    }
    set ShowHeaderLabels(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") this.QuoteOPTemplateSettingPM.ShowHeaderLabelsPackages = value;
            else this.QuoteOPTemplateSettingPM.ShowHeaderLabelsContainers = value;
        }
    }




    get SpaceLinesBeforeTable() {
        var spaceLinesBeforeTable: number = 1;
        if (this.QuoteOPTemplateSettingPM) spaceLinesBeforeTable = this.QuoteOPTemplateSectionTypeName == "Packages" ? this.QuoteOPTemplateSettingPM.SpaceLinesBeforePackages : this.QuoteOPTemplateSettingPM.SpaceLinesBeforeContainers;
        return spaceLinesBeforeTable;
    }
    set SpaceLinesBefore(value: number) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSectionTypeName == "Packages") this.QuoteOPTemplateSettingPM.SpaceLinesBeforePackages = value;
            else this.QuoteOPTemplateSettingPM.SpaceLinesBeforeContainers = value;
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


        var textDesignPmLists = this.QuoteOPTemplateTextDesignPMLists.filter(d => d.IsDirty == true);
        if (textDesignPmLists.length > 0) this.IsSaveQuoteOPTemplateTextDesignRuning = true;
        if (this.TableDesignPM.IsDirty) this.IsSaveQuoteOPTemplateTableDesignRuning = true;
     


        var textCodeDataLists: TextCodeData[] = this.ItemsSource.Collection.filter(d => d.EntityPM.IsDirty == true);
        if (textCodeDataLists.length > 0) this.IsSaveQuoteOPTemplateTextCodeRuning = true;



        if (this.IsSaveQuoteOPTemplateTextDesignRuning || this.IsSaveQuoteOPTemplateTableDesignRuning || this.IsSaveQuoteOPTemplateTextCodeRuning) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));

            if (this.QuoteOPTemplateSettingPM.IsDirty) {
                this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                    this.QuoteOPTemplateSettingPM.IsDirty = false;
                    this.SaveOthers(textDesignPmLists, textCodeDataLists);
                });
            } else this.SaveOthers(textDesignPmLists, textCodeDataLists);



        } else {

            if (this.QuoteOPTemplateSettingPM.IsDirty) {
                this.SaveQuoteOPTemplateSetting();
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                if (this.IsPerContainerChange == true) {
                    this.CurrentSession.CurrentWindow.Close("Refresh");
                } else this.CurrentSession.CloseCurrentWindow();

            }

               
           
        }    






    }


    SaveOthers(textDesignPmLists: any[], textCodeDataLists:any[]) {

        if (this.IsSaveQuoteOPTemplateTextDesignRuning) this.SaveQuoteOPTemplateTextDesign(textDesignPmLists);
        if (this.IsSaveQuoteOPTemplateTableDesignRuning) this.SaveQuoteOPTemplateTableDesign();
        if (this.IsSaveQuoteOPTemplateTextCodeRuning) this.SaveQuoteOPTemplateTextCode(textCodeDataLists);
    }

    SaveQuoteOPTemplateTextDesign(items: any) {

        items.forEach((item) => {item.IsDirty = false; });
           
        
        this.QuoteOPTemplateTextDesignExtendedPMService.updateQuoteOPTemplateTextDesignPMs(items).subscribe((res:any) => {
            this.IsSaveQuoteOPTemplateTextDesignRuning = false;
            this.SaveCompleted();

        });
        
    }

    SaveQuoteOPTemplateTableDesign() {

        this.QuoteOPTemplateTableDesignPMService.update(this.TableDesignPM).subscribe((res:any) => {
            this.IsSaveQuoteOPTemplateTableDesignRuning = false;
            this.SaveCompleted();

        });
    }

    SaveQuoteOPTemplateSetting() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
        this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
            this.QuoteOPTemplateSettingPM.IsDirty = false;
            this.SaveCompleted();

        });

    }

    SaveQuoteOPTemplateTextCode(items: TextCodeData[]) {

        var QuoteOPTemplateTextCodePMLists: QuoteOPTemplateTextCodePM[] = [];
        items.forEach((item) => {
            if (item.EntityPM) {
                item.EntityPM.IsDirty = false;
                QuoteOPTemplateTextCodePMLists.push(item.EntityPM);
            }
         });


        this.QuoteOPTemplateTextCodeExtendedPMService.updateTextCodes(QuoteOPTemplateTextCodePMLists).subscribe((res:any) => {
            this.IsSaveQuoteOPTemplateTextCodeRuning = false;
            this.SaveCompleted();

        });
 
    }

    ShowTotalPerContainer() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteOPTemplatePM = this.EntityPM;
        windowArgs.QuoteOPTemplateSettingPM = this.QuoteOPTemplateSettingPM;
        windowArgs.QuoteOPTemplateTextCodePMList = this.AllQuoteOPTemplateTextCodePMList;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 930;
        logWindow.Height = 610;
        logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.TotalPerContainerSettings")  ;
        logWindow.Show("./QuoteOPModules/QuoteTemplates/Components/QuoteOPTemplateTotalPerContainerSetting");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "Refresh") {
                this.IsPerContainerChange = true;
            }
        });

    }


    SaveCompleted() {
        if (!this.IsSaveQuoteOPTemplateTextDesignRuning && !this.IsSaveQuoteOPTemplateTableDesignRuning && !this.IsSaveQuoteOPTemplateTextCodeRuning) {
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
    EntityPM: QuoteOPTemplateTextCodePM;
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

                    this.ToolTipEnglishNameMessage = TextCodeTranslator.Translate("QuoteOPTemplate.M.ValueEditedByUserMessage") + " {" + this.EntityPM.OriginalEnglishName + "}";
                    
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


    constructor(entity: QuoteOPTemplateTextCodePM) {
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
