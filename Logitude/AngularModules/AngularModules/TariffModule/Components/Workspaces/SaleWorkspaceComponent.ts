import { Component, ViewChildren, QueryList, OnInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TariffDomainService, TariffSummery } from '../../Services/TariffDomainService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ListComponentArgs } from '../../../Infrastructure/Args';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { TariffList } from '../../EntityLists/TariffList';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'SaleComponent',
    templateUrl: './SaleWorkspaceComponent.html',
    providers: [EntityResourceService, TariffDomainService]
})

export class SaleWorkspaceComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    public IsTariffGenerateVisible: boolean = false;
    public IsCustomsChargesVisible: boolean = false;
    public IsInlandTariffsVisible: boolean = false;
    public QueriesAreaHeight: number = 240;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService, private tariffDomainService: TariffDomainService) {
        this.RunComponent();
    }
    public ImportSaleCount: string;
    public ExportSaleCount: string;
  
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
        this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe((response: any) => {
            if (this.CurrentSession == null)
                this.CurrentSession = SessionLocator.SelectedSession;
            this.LoadAllScreenData();
        });

    }

    LoadAllScreenData() {
        this.QueriesAreaHeight = 240;
        this.LoadQueriesCounts();
        this.SetQueriesVisibility();
        this.LoadRecentTariffs();
    }

    LoadQueriesCounts() {
        this.tariffDomainService.GetSaleTariffsCounts().subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult: TariffSummery = myResponse.Result;

                    if (myResult != null) {
                        this.ImportSaleCount = myResult.ImportSaleCount > 1000 ? "1000+" : myResult.ImportSaleCount.toString();
                        this.ExportSaleCount = myResult.ExportSaleCount > 1000 ? "1000+" : myResult.ExportSaleCount.toString();
                    }
                }
            }
        });
    }

    public ImportSaleVisibility: boolean = false;
    public ExportSaleVisibility: boolean = false;
  
    SetQueriesVisibility() {

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.ImportLocalChargesSale")) {
            this.ImportSaleVisibility = true;
        }

        if (FeatureLocator.HasFeaturePermession("Tariff", "Tariff.Q.ExportLocalChargesSale")) {
            this.ExportSaleVisibility = true;
        }
    }

    public NewTariff(code: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 850;
        logWindow.Height = 550;
        var windowTitle = "";
        var typeCode = "";
        switch (code) {
            case "I": {
                windowTitle = "New " + TextCodeTranslator.Translate("Tariff.Q.ImportLocalChargesSale");
                typeCode = "ICS";
                break;
            }

            case "E": {
                windowTitle = "New " + TextCodeTranslator.Translate("Tariff.Q.ExportLocalChargesSale");
                typeCode = "ECS";
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
            case "I": {
                queryCode = "Import Local Charges Sale";
                displayTitle = "Import Local Charges Sale";
                break;
            }

            case "E": {
                queryCode = "Export Local Charges Sale";
                displayTitle = "Export Local Charges Sale";
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
        tariffService.GetRecentTariffs().subscribe((myResult: any) => {
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

