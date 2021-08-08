import {Component, OnInit, EventEmitter}  from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {MilestonePermissiosViewModel} from './ViewModel/MilestonePermissiosViewModel';
import {CargoTrackingMilestoneExtendedService} from '../Services/Others/CargoTrackingMilestoneExtendedService';

@Component({
    selector: 'CargoTrackingMilestonesPermissiosComponent',
    templateUrl: './CargoTrackingMilestonesPermissiosComponent.html',
    inputs: ['OnCloseWindowEvent'],
    providers: [CargoTrackingMilestoneExtendedService],
})
export class CargoTrackingMilestonesPermissiosComponent implements OnInit {
    public MilestonePermissiosSelectedViewModel: any;
    myTenantList: any;
    MilestonesPermissiosLists: MilestonePermissiosViewModel[];
    OnCloseWindowEvent = new EventEmitter();
    FullComponentsVisibility: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public cargoTrackingMilestoneExtendedService: CargoTrackingMilestoneExtendedService) {
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
        this.myTenantList = [];

        this.cargoTrackingMilestoneExtendedService.getAll().subscribe((res: ServiceResponse) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.myTenantList = pmResponse.Result;
                this.BuildData();
            }
             this.CurrentSession.StopBusyIndicator();
        });
    }

    mySearchText: string;
    BuildData() {
        var myList = [];
        this.MilestonesPermissiosLists = [];
        if (!this.mySearchText) {
            myList = this.myTenantList;
        }
        else {
            myList = this.myTenantList.filter(d=> (d.Code && d.Code.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.EnglishName && d.EnglishName.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1) || (d.LocalName && d.LocalName.toLowerCase().indexOf(this.mySearchText.toLowerCase()) > -1));
        }
        myList =  this.SortItemSource(myList);
        myList.forEach((item) => {
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
        this.myTenantList = [];
        this.MilestonesPermissiosLists.forEach((item) => {
            if (item.entityPM.IsDirty) {
                this.myTenantList.push(item.entityPM);
            }
        });
        if (this.myTenantList.length > 0) {
            this.cargoTrackingMilestoneExtendedService.update(this.myTenantList).subscribe((res: ServiceResponse) => {
                this.CloseButtonClicked();
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
