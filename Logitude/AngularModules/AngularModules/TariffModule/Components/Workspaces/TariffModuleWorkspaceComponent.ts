import {Component, ViewChildren, QueryList,OnInit} from '@angular/core';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TariffDomainService, TariffSummery } from '../../Services/TariffDomainService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ListComponentArgs } from '../../../Infrastructure/Args';

@Component({
    selector: 'TariffModuleWorkspaceComponent',
    moduleId: module.id,
    templateUrl: './TariffModuleWorkspaceComponent.html',
    providers: [EntityResourceService, TariffDomainService],
})

export class TariffModuleWorkspaceComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    constructor(private _entityResourceService: EntityResourceService, private tariffDomainService: TariffDomainService) {
        this.RunComponent();
    }
    public AirFreightCount: string;

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
        if (this.CurrentSession == null)
            this.CurrentSession = SessionLocator.SelectedSession;
        this.InitComponent();
    }

    LoadAllScreenData() {
        this.LoadQueriesCounts();
    }


    LoadQueriesCounts() {
        this.tariffDomainService.GetTariffsCounts().subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var myResult: TariffSummery = myResponse.Result;

                        if (myResult != null) {
                            this.AirFreightCount = myResult.AirFreightCount > 1000 ? "1000+" : myResult.AirFreightCount.toString();
                             }
                    }
                }
            });
        
    }

    public NewTariff(code: string) {
        if (code == "A") {
            this._entityResourceService.getEntityResourceByTableName("Tariff", 0).subscribe(response => {

                var windowTitle = "New Air Freight Cost";

                var logWindow = new LogitudeWindow();
                logWindow.Width = 850;
                logWindow.Height = 500;
                logWindow.Title = windowTitle;
                logWindow.WindowClosed.subscribe(($event: any) => {
                    this.LoadQueriesCounts();
                });
                logWindow.Show('./TariffModule/Components/NewEntity/NewAirFreightCostComponent');
            });
        }
    }

    public ViewTariffs(code: string) {
        if (code = "A") {
            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = "AFTA";
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
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private InitComponent() {
        this.LoadAllScreenData();
    }

    private selectedItem: string;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(newValue: string) {
        if (this.selectedItem != newValue) {
            this.selectedItem = newValue;
        }
    }

  
}

