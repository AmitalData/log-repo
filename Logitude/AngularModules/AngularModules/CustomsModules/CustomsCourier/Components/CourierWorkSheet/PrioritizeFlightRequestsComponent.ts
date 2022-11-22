declare var window: any;
import { Component, EventEmitter, Output, Input, OnInit, ElementRef, AfterViewInit, OnDestroy } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';

import { CustomsRequestsSheetWebService } from 'Customs/Services/WebServices/CustomsRequestsSheetWebService';
import { CustomsSettingListService } from 'Customs/Services/StandardLists/CustomsSettingListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { interval, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CustomsRequestsSheetExtendedListService } from 'Customs/Services/ExtendedLists/CustomsRequestsSheetExtendedListService';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { CustomsRequestsSheetsComponent } from 'CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent';
import { CustomsRequestsSheetStatusListService } from 'Customs/Services/StandardLists/CustomsRequestsSheetStatusListService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { CustomsRequestsSheetStatusList } from 'Customs/EntityLists/CustomsRequestsSheetStatusList';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { CourierMasterPM } from 'Customs/EntityPMs/CourierMasterPM';

//////////////////////////////////////////////////////////////////


@Component({
    selector: 'PrioritizeFlightRequestsComponent',

    templateUrl: './PrioritizeFlightRequestsComponent.html',
    providers: [CustomsRequestsSheetExtendedListService]
})


export class PrioritizeFlightRequestsComponent
    extends BaseComponent
    implements OnInit {
    private ngUnsubscribe = new Subject();

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    _Id: string = Guid.newGuid();

    //////<<<<<<<<<<<<<<<<<<Request/Query Property

  

    MyRequestOnly: boolean;
    CorrelationId: string;
    CustomFileNo: string;
    InterfaceTypeCode: string;
    SearchFields: string;
    EntityReference: string;
    Title: string;
    RequestStatusString: string;
   // _AllCustomsRequestsSheetStatusListVM: CustomsRequestsSheetStatusListVM[];
    IsRestored: boolean;
    //IsDCA?: boolean;
    _SelectedDCAValue: string = 'ALL';//'ALL';//DCA//!DCA

    //////Request/Query Property>>>>>>>>>>>>>>>>>>>>>>>>>>>

    private _entityListService: EntityListService;

    //public DataContext: CustomsRequestsSheetsComponent = this;
    public ObjectTableName: string = "Customs.CustomsRequestsSheet";
    public columns: any[] = null;
    public columnsStatistics: any[] = null;


    _MySearchText = "Search";
    _CustomsRequestsSheetStatusListService: CustomsRequestsSheetStatusListService;
    public _TranslationLoaded: boolean = false;
    _AllCRSSChecked: boolean
    FiltersSectionVisibility: boolean = true;
    StatisticsVisibility: boolean = false;
    RefreshButtonVisibility: boolean;
    CloseButtonVisibility: boolean;//?????
    selectStatusesHeight: string;
    //_stratSearch: boolean = true;
  
    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();
    isReAnAnalysis: boolean;
    courierMasterPM:CourierMasterPM

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private _CD: ChangeDetectorRef, public customsRequestsSheetExtendedListService: CustomsRequestsSheetExtendedListService) {
        super();
        this._CustomsRequestsSheetStatusListService = new CustomsRequestsSheetStatusListService();
       // this._AllCustomsRequestsSheetStatusListVM = [];
        console.log("12....");

    }

    SetWindowArgs(args) {
        if (args != null) {
   
                this.courierMasterPM=args.CourierMasterPM
        }
    }

    ngOnInit() {
        this.InitScreen()
    }
    ngOnDestroy() {
        console.log("CustomsRequestsSheetsComponent:ngOnDestroy");
        this.entityArgs = null;
        this._CD = null;
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }
    InitScreen() {
        //this.CurrentSession.StartBusyIndicator("");
        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsRequestsSheet", 0).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("CommunicationLog", 0).subscribe((response: any) => {
                this._entityResourceService.getEntityResourceByTableName("Customs.Declaration", 0).subscribe((response: any) => {                 
                    this._entityListService = new EntityListService();

                    interval(1000 * 3).pipe(takeUntil(this.ngUnsubscribe)).subscribe(() => this.GetStatisticsByCourierDeclarations());
               
                        this._TranslationLoaded = true;
                        var isDestroyed: boolean = this._CD['destroyed'];
                        if (!isDestroyed) {
                            this._CD.detectChanges();
                        }
                        // this.CRSSearch();
                        this.GetStatisticsByCourierDeclarations()

                   
                });

            });



        })
    }

    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    customsRequestsSheetSummary = new Array<CustomsRequestsSheetSummary>();
    IsTherecustomsRequestsSheetSummary = false;
    SumRequests = 0;
    
    GetStatisticsByCourierDeclarations() {
        
        this.customsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString()).subscribe((response: ServiceResponse) => {
            
            var customsSetting = response.Result;
            //if (!AppTool.IsNullOrEmpty(customsSetting) && customsSetting.CompanyType == "B") {
                this.StatisticsVisibility = !this.CurrentSession?.CurrentEditComponent?.EntityPM;;
                var service = new CustomsRequestsSheetWebService();
                var statistics = service.GetStatisticsByCourierDeclarations(this.courierMasterPM.Id).subscribe((response: any) => {
                    
                    if (response.Result != null) {
                        this.SumRequests = 0;
                        this.customsRequestsSheetSummary = response.Result;
                        for (var request of (this.customsRequestsSheetSummary as any[])) {
                            this.SumRequests += request.count;
                            this.IsTherecustomsRequestsSheetSummary = true; 
                        }
                    }
                });
           // }
        });

    }


  
   

    // CRSSearch() {
    //     this.GetStatisticsByCourierDeclarations();
       
    //     //this.CurrentSession.StartBusyIndicator("");
    //     setTimeout(() => {
    //         this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    //     }, 10);

    //     //this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); 
    // }

    // filterAgrs: ApiQueryFilters;    

    // RefreshButtonClicked() {
    //     this.CRSSearch();
    // }
    ChangePriority($event){
        var logitudeWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.CourierMasterPM = this.courierMasterPM;
        windowArgs.InterfaceTypeCode = $event.InterfaceTypeCode;
        logitudeWindow.Width = 350;
        logitudeWindow.Height = 250;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = "תעדוף בקשות";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/UpdatePriorityComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            //this.RefreshButtonClicked();
        }); 
    }
}

////////////////////////////////////////
// export class CustomsRequestsSheetStatusListVM {
//     constructor(public MyItem: CustomsRequestsSheetStatusList, isdeclaration?: boolean, isReAnAnalysis?: boolean) {
//         var Code = MyItem.Code;
//         if (!isReAnAnalysis) {
//             if (Code == "1" || Code == "2" || Code == "3" || Code == "4" || Code == "5" || Code == "21" || Code == "99") {
//                 this.IsChecked = true;
//             }

//             if (//declarationPM != null
//                 isdeclaration
//                 && Code != "99") {

//                 this.IsChecked = true;
//             }
//         }
//     }
//     IsChecked: boolean;
// }
export class CustomsRequestsSheetSummary {
    id: string;
    count: number;
    InterfaceTypeName: string;
}
//////////////////////////////////////////////////
