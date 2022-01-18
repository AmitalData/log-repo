import {Component, ViewChildren, QueryList, OnInit} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TariffDomainService, TariffSummery} from '../../Services/TariffDomainService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { TariffList } from '../../EntityLists/TariffList';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'CostComponent',    
    templateUrl: './CostWorkspaceComponent.html',
    providers: [EntityResourceService, TariffDomainService]
})

export class CostWorkspaceComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    public IsTariffGenerateVisible: boolean = false;
    public IsCustomsChargesVisible: boolean = false;
    public QueriesAreaHeight: number = 240;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService, private tariffDomainService: TariffDomainService) {
        this.RunComponent();
    }
    public AirFreightCount: string;
    public AirSurchargeCount: string;
    public OceanSurchargeCount: string;
    public OceanLCLFreightCount: string;
    public OceanFCLFreightCount: string;
    public OceanFCLSurchargesCount: string;
    public ImportCustomsChargesCount: string;
    public ExportCustomsChargesCount: string;
    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe((response:any) => {
            if (this.CurrentSession == null)
                this.CurrentSession = SessionLocator.SelectedSession;
            this.LoadAllScreenData();
        });
        
    }

    LoadAllScreenData() {
        this.LoadQueriesCounts();
        this.SetQueriesVisibility();
        this.LoadRecentTariffs();
    }

    LoadQueriesCounts() {
        this.tariffDomainService.GetTariffsCounts().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult: TariffSummery = myResponse.Result;

                    if (myResult != null) {
                        this.AirFreightCount = myResult.AirFreightCount > 1000 ? "1000+" : myResult.AirFreightCount.toString();
                        this.AirSurchargeCount = myResult.AirSurchargeCount > 1000 ? "1000+" : myResult.AirSurchargeCount.toString();
                        this.OceanSurchargeCount = myResult.OceanSurchargeCount > 1000 ? "1000+" : myResult.OceanSurchargeCount.toString();
                        this.OceanLCLFreightCount = myResult.OceanLCLFreightCount > 1000 ? "1000+" : myResult.OceanLCLFreightCount.toString();
                        this.OceanFCLFreightCount = myResult.OceanFCLFreightCount > 1000 ? "1000+" : myResult.OceanFCLFreightCount.toString();
                        this.OceanFCLSurchargesCount = myResult.OceanFCLSurchargesCount > 1000 ? "1000+" : myResult.OceanFCLSurchargesCount.toString();
                        this.ImportCustomsChargesCount = myResult.ImportCustomsChargesCount > 1000 ? "1000+" : myResult.ImportCustomsChargesCount.toString();
                        this.ExportCustomsChargesCount = myResult.ExportCustomsChargesCount > 1000 ? "1000+" : myResult.ExportCustomsChargesCount.toString();
                    }
                }
            }
        });
    }

    CheckPrice(type: string) {
        this._entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
            ServiceLocator.SendTotangoUserActivity("Tariff", "Price Check");

            var logWindow = new LogitudeWindow();
            logWindow.IsFillScreenHeight = true;
            logWindow.Width = 1200;
            logWindow.Title = "Price Check";
            var windowArgs: any = {};
            windowArgs.TariffType = type;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./TariffModule/Components/Workspaces/TariffSearchAirFreightPricesComponent");
        });      
    }

    public AirFreightCostVisibility: boolean = false;
    public AirSurchargesCostVisibility: boolean = false;
    public OceanLCLFreightCostVisibility: boolean = false;
    public OceanLCLSurchargesCostVisibility: boolean = false;
    public OceanFCLFreightCostVisibility: boolean = false;
    public OceanFCLSurchargesCostVisibility: boolean = false;
    public ImportCustomsCostVisibility: boolean = false;
    public ExportCustomsCostVisibility: boolean = false;
    SetQueriesVisibility() {
        if (SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "CCT")[0]) {
            this.IsCustomsChargesVisible = true;
            this.QueriesAreaHeight = 310;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.AirFreightCostTariffs")) {
            this.AirFreightCostVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.AirSurchargesCostTariffs")) {
            this.AirSurchargesCostVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.OceanLCLFreightCost")) {
            this.OceanLCLFreightCostVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.Ocean.LCL.Surcharges.Cost")) {
            this.OceanLCLSurchargesCostVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.OceanFCLFreightCost")) {
            this.OceanFCLFreightCostVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.OceanFCLSurchargesCost")) {
            this.OceanFCLSurchargesCostVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.ExportCustomsChargesCost")) {
            this.ExportCustomsCostVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.ImportCustomsChargesCost")) {
            this.ImportCustomsCostVisibility = true;
        }
    }

    public NewTariff(code: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 850;
        logWindow.Height = 550;
        var windowTitle = "";
        var typeCode = "";
        switch (code) {
            case "A": {
                windowTitle = "New Air Freight Cost";
                typeCode = "AFC";
                break;
            }
            case "OLC": {
                windowTitle = "New Ocean LCL Freight Cost";
                typeCode = "OLC";
                break;
            }
            case "AS": {
                windowTitle = "New Air Surcharges Cost";
                typeCode = "ASC";
                break;
            }
            case "OSC": {
                windowTitle = "New " + TextCodeTranslator.Translate("Tariff.Q.Ocean.LCL.Surcharges.Cost");
                typeCode = "OSC";
                break;
            }
            case "OFC": {
                windowTitle = "New " + TextCodeTranslator.Translate("Tariff.Q.OceanFCLFreightCost");
                typeCode = "OFC";
                break;
            }
            case "OFS": {
                windowTitle = "New " + TextCodeTranslator.Translate("Tariff.Q.OceanFCLSurchargesCost");
                typeCode = "OFS";
                break;
            }

            case "ICC": {
                windowTitle = "New " + TextCodeTranslator.Translate("Tariff.Q.ImportCustomsChargesCost");
                typeCode = "ICC";
                break;
            }

            case "ECC": {
                windowTitle = "New " + TextCodeTranslator.Translate("Tariff.Q.ExportCustomsChargesCost");
                typeCode = "ECC";
                break;
            }
            default: {
                break;
            }
        }

        ServiceLocator.SendTotangoUserActivity("Tariff", windowTitle);

        logWindow.Title = windowTitle;
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.LoadQueriesCounts();
        });
        logWindow.ComponentLoaded.subscribe(comp => {
            comp.SetWindowArgs({ TypeCode: typeCode });
        });
        logWindow.WindowClosed.subscribe(($event: any) => this.OnNewTariffWindowClosed($event));
        logWindow.Show('./TariffModule/Components/NewEntity/NewAirFreightCostComponent');
    }

    OnNewTariffWindowClosed(arg: any) {
        if (SessionLocator.IsExternalParams) {
            SessionLocator.ClearExternalParams();
        }

        if (arg == 'OK') {
            this.LoadAllScreenData();
        }
    }

    public ViewTariffs(code: string) {        
        var queryCode: string;
        var displayTitle: string;
        switch (code) {
            case "A": {
                queryCode = "Air Freight Cost Tariffs";
                displayTitle = "Air Freight Cost Tariffs";
                break;
            }

            case "AS": {
                queryCode = "Air Surcharges Cost Tariffs";
                displayTitle = "Air Surcharges Cost";
                break;
            }

            case "OSC": {               
                queryCode = "Ocean.LCL.Surcharges.Cost";
                displayTitle = TextCodeTranslator.Translate("Tariff.Q.Ocean.LCL.Surcharges.Cost");               
                break;
            }

            case "OLC": {
                queryCode = "Ocean LCL Freight Cost";
                displayTitle = "Ocean LCL Freight Cost";                
                break;
            }

            case "OFC": {
                queryCode = "Ocean FCL Freight Cost";
                displayTitle = "Ocean FCL Freight Cost";
                break;
            }

            case "OFS": {
                queryCode = "Ocean FCL Surcharges Cost";
                displayTitle = "Ocean FCL Surcharges Cost";
                break;
            }

            case "ICC": {
                queryCode = "Import Customs Charges Cost";
                displayTitle = "Import Customs Charges Cost";
                break;
            }

            case "ECC": {
                queryCode = "Export Customs Charges Cost";
                displayTitle = "Export Customs Charges Cost";
                break;
            }

            default: {
                break;
            }
        }

        ServiceLocator.SendTotangoUserActivity("Tariff", "View " + queryCode + " Query");

        var listArgs = new ListComponentArgs();
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = "Tariff";
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Tariff";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response: any) => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });        
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    
    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
        }
    }
    
    // Recent Tariffs
    public RecentTariffsCount: number = 0;
    public RecentTariffsList: TariffList[] = [];
    LoadRecentTariffs() {
        var tariffService: TariffDomainService = new TariffDomainService();
        tariffService.GetRecentTariffs().subscribe((myResult:any) => {
            if (myResult == null) {
                this.RecentTariffsList = [];
                this.RecentTariffsCount = 0;
            }
            else {
                this.RecentTariffsList = myResult;
                this.RecentTariffsCount = myResult.length;
            }
        });
    }

    //Edit Tariff
    EditTariff(entity: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Tariff', BackButtonLabel: 'Tariffs' });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.OnBackFromEdit();
                    this.LoadAllScreenData();
                });
            });
    }
    OnBackFromEdit() {
        SessionLocator.ClearExternalParams();
    }

}

