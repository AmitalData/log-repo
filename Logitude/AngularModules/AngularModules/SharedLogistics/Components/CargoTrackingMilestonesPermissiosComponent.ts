import {Component, OnInit, EventEmitter}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {MilestonePermissiosViewModel} from './ViewModel/MilestonePermissiosViewModel';
import {CargoTrackingMilestoneExtendedService} from '../Services/Others/CargoTrackingMilestoneExtendedService';
import { CargoTrackingTenantMilestoneDefinitionExtendedService } from '../Services/Others/CargoTrackingTenantMilestoneDefinitionExtendedService';
import { CargoTenantMilestoneDefinitionPM } from '../../Common/EntityPMs/CargoTenantMilestoneDefinitionPM';

@Component({
    selector: 'CargoTrackingMilestonesPermissiosComponent',
    templateUrl: './CargoTrackingMilestonesPermissiosComponent.html',
    inputs: ['OnCloseWindowEvent'],
    providers: [CargoTrackingMilestoneExtendedService, CargoTrackingTenantMilestoneDefinitionExtendedService],
})
export class CargoTrackingMilestonesPermissiosComponent implements OnInit {
    public MilestonePermissiosSelectedViewModel: any;
    zeroTenantList: any;
    myTenantList: any;
    MilestonesPermissiosLists: MilestonePermissiosViewModel[];
    OnCloseWindowEvent = new EventEmitter();
    FullComponentsVisibility: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public cargoTrackingMilestoneExtendedService: CargoTrackingMilestoneExtendedService, public cargoTrackingTenantMilestoneDefinitionExtendedService: CargoTrackingTenantMilestoneDefinitionExtendedService) {
        this.CurrentSession.StartBusyIndicatorLoading();
    }

    ngOnInit() {
        this.OnCloseWindowEvent.subscribe(($event: any) => {
            this.SaveButtonClicked();
        });

        this.Run();
    }

    Run() {
        this.LoadTenantData();
    }

    LoadTenantData() {
        this.zeroTenantList = [];

        this.cargoTrackingMilestoneExtendedService.getAll().subscribe((res: ServiceResponse) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.zeroTenantList = pmResponse.Result;
                this.cargoTrackingTenantMilestoneDefinitionExtendedService.getAll().subscribe((respo: ServiceResponse) => {
                    var cargoTenantMilestoneDefinitionPMResponse: ServiceResponse = respo;
                    if (!cargoTenantMilestoneDefinitionPMResponse.HasError) {
                        this.myTenantList = cargoTenantMilestoneDefinitionPMResponse.Result;
                        this.BuildData();
                    }
                    this.CurrentSession.StopBusyIndicator();
                });
            }
        });
    }

    mySearchText: string;
    BuildData() {
        var zeroList = [];
        this.MilestonesPermissiosLists = [];
        if (!this.mySearchText) {
            zeroList = this.zeroTenantList;
        }
        else {
            zeroList = this.zeroTenantList.filter(d=> (d.Code && d.Code.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.EnglishName && d.EnglishName.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.LocalName && d.LocalName.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1));
        }
        zeroList = this.SortItemSource(zeroList);

        zeroList.forEach((item) => {
            let cargoTenantMilestoneDefinition = this.myTenantList.filter(f => f.Code == item.Code)[0];
            if (cargoTenantMilestoneDefinition) {
                item.CustomerChooseIsChecked = cargoTenantMilestoneDefinition.IsCustomerView;
            }
            this.MilestonesPermissiosLists.push(new MilestonePermissiosViewModel(item));
        });
    }

    SortItemSource(items: any) {
        items.sort((a, b) => {
            if (a.EnglishName.toLowerCase() < b.EnglishName.toLowerCase()) {
                return -1;
            }
            else if (a.EnglishName.toLowerCase() > b.EnglishName.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });

        return items;
    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();
        if (this.MilestonesPermissiosLists.length > 0) {
            this.myTenantList = [];
            this.MilestonesPermissiosLists.forEach((item) => {
                let cargoTenantMilestoneDefinition = new CargoTenantMilestoneDefinitionPM();
                cargoTenantMilestoneDefinition.Code = item.entityPM.Code;
                cargoTenantMilestoneDefinition.Tenant = SessionLocator.Tenant;
                cargoTenantMilestoneDefinition.IsCustomerView = item.CustomerChooseIsChecked;
                this.myTenantList.push(cargoTenantMilestoneDefinition);
            });
            this.cargoTrackingTenantMilestoneDefinitionExtendedService.update(this.myTenantList).subscribe((res: ServiceResponse) => {
                if (!res.HasError) {
                    this.CloseButtonClicked();
                }
            });
        }
        else {
            this.CloseButtonClicked();
        }
    }

    onSearchTextChangeEvent(searchText) {
        if (!searchText) searchText = "";
        this.mySearchText = searchText;
        this.BuildData();
    }

    SetWindowArgs(args: any) {
        this.FullComponentsVisibility = true;
    }
}
