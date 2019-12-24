import {Component, ViewChildren, QueryList, OnInit} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TariffDomainService, TariffSummery} from '../../Services/TariffDomainService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { TariffList } from '../../EntityLists/TariffList';

@Component({
    selector: 'CostComponent',
    moduleId: module.id,
    templateUrl: './CostWorkspaceComponent.html',
    providers: [EntityResourceService, TariffDomainService]
})

export class CostWorkspaceComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    public IsTariffGenerateVisible: boolean = false;
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
        this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe(response => {
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
                    }
                }
            }
        });
    }

    CheckPrice(type: string) {
        this._entityResourceService.getEntityResourceByTableName("TariffLine").subscribe((res1: any) => {
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

    SetQueriesVisibility() {

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
    }

    public NewTariff(code: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 850;
        logWindow.Height = 500;
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
            default: {
                break;
            }
        }

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

        switch (code) {
            case "A": {
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = "Air Freight Cost Tariffs";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Air Freight Cost Tariffs";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
                break;
            }

            case  "AS":{
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = "Air Surcharges Cost Tariffs";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Air Surcharges Cost";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
                break;
            }

            case "OSC": {
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = "Ocean.LCL.Surcharges.Cost";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = TextCodeTranslator.Translate("Tariff.Q.Ocean.LCL.Surcharges.Cost");
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
                break;
            }

            case "OLC": {
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = "Ocean LCL Freight Cost";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Ocean LCL Freight Cost";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
                break;
            }
            case "OFC": {
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = "Ocean FCL Freight Cost";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Ocean FCL Freight Cost";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
                break;
            }
            case "OFS": {
                var listArgs = new ListComponentArgs();
                listArgs.QueryCode = "Ocean FCL Surcharges Cost";
                listArgs.ObjectTableName = "Tariff";
                listArgs.DisplayTitle = "Ocean FCL Surcharges Cost";
                listArgs.BackButtonTitle = "Tariff";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(listArgs);
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                            this.CurrentSession.AddMenuReference(cmpRef);
                        });
                });
                break;
            }
            default: {
                break;
            }

        }
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
        tariffService.GetRecentTariffs().subscribe(myResult => {
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

