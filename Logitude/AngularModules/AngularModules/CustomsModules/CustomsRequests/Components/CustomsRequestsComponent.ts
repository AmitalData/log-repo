declare var window: any;
import { Component, OnInit } from '@angular/core';
import { CustomsMenuItem, RequestSheetState } from '../../../Customs/DataContract/CustomsMenuItem';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
//import { DeclarationRestoreComponent } from '../CustomsRequests/DeclarationRestoreComponent';
import { DeclarationStatusComponent } from './DeclarationRequests/DeclarationStatusComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
///import { IRequestsSheetMassagingView } from './RequestsSheetMassagingBase';
import { BaseRequestsSheetMassaging } from './BaseRequestsSheetMassaging';
import { CommunicationLogStepListService } from '../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
import {ListComponentArgs} from '../../../Infrastructure/Args';

//import { RetrieveDeclarationComponent } from './RetrieveDeclarationComponent';

import { DeclarationRestoreRequestParams } from '../../../Customs/DataContract/RequestParams/DeclarationRestoreRequestParams';
import { DeclarationRestoreResponseData } from '../../../Customs/DataContract/ResponseData/DeclarationRestoreResponseData';

import { CustomsRequestMenuService } from '../../../Customs/Services/Others/CustomsRequestMenuService';

import { PrintRequestComponent } from './DeclarationRequests/PrintRequestComponent';

import { CustomsVendorPM } from '../../../Customs/EntityPMs/CustomsVendorPM';


@Component({
    moduleId: module.id,
    templateUrl: './CustomsRequestsComponent.html',
})


export class CustomsRequestsComponent implements OnInit {
    public DataContext: CustomsRequestsComponent = this;
    //private CustomsRequestMenuItems: CustomsMenuItem[];
    private _CustomsRequestMenuService: CustomsRequestMenuService;
    public ItemsSource: CustomsMenuItem[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService) {
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("Customs.Client").subscribe(response => {
            this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response2 => {
                this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe((resp => {
                    this._CustomsRequestMenuService = new CustomsRequestMenuService();
                    
                    //this.BuildCustomsList();
                    //this.ItemsSource = this.CustomsRequestMenuItems;
                    this.ItemsSource = this._CustomsRequestMenuService.CustomsRequestMenuItems;

                }));
            });
        });
    }

    

    Search(text: string) {
        var itemsSource = this._CustomsRequestMenuService.CustomsRequestMenuItems;
        if (AppTool.IsNullOrEmpty(text)) {
            this.ItemsSource = this._CustomsRequestMenuService.CustomsRequestMenuItems;
        }
        else {
            itemsSource = itemsSource.filter(f => f.TranslatedName != null);
            
            itemsSource = itemsSource.filter(f => f.TranslatedName.toUpperCase().includes(text.toUpperCase()) || f.ScreenName.toUpperCase().includes(text.toUpperCase()));
        }

        this.ItemsSource = itemsSource;
    }

    ItemClicked(item: CustomsMenuItem) {

        if (!AppTool.IsNullOrEmpty(item)) {

            switch (item.ScreenName) {
                case 'Vendors':
                case 'Clients': { // Query 
                    this.OpenListQueryByObjectTable(item.objectTableName); // Abdullah: fill objectTableName when u build the item
                    break;
                }
                case 'SearchVendor': {

                    this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(response => {
                        this._entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe(response => {
                            var vendor = new CustomsVendorPM();
                            vendor.Tenant = SessionLocator.Tenant;
                            vendor.VendorTypeCode = "1";

                            var args: any = {};
                            args.IsNewEntity = true;
                            args.EntityPM = vendor;
                            args.IsSearchMode = true;// yaron want to allowed to send response Even there is only VendorNum


                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 960;
                            logWindow.Height = 570;
                            logWindow.Title = TextCodeTranslator.Translate("Customs.Vendor.O.SearchVendors");
                            logWindow.WindowArgs = args;
                            logWindow.ShowCloseButton = true;
                          logWindow.Show('./CustomsModules/CustomsVendor/Components/NewEntity/NewVendorComponent');

                            logWindow.WindowClosed.subscribe(($event: any) => {

                            });
                        });
                    });

                    break;
                }
                case 'NewVendor': {

                    this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(response => {
                        this._entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe(response => {
                            var vendor = new CustomsVendorPM();
                            vendor.Tenant = SessionLocator.Tenant;
                            vendor.VendorTypeCode = "1";

                            var args: any = {};
                            args.IsNewEntity = true;
                            args.EntityPM = vendor;
                            args.IsSearchMode = false;// yaron want to allowed to send response Even there is only VendorNum
                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 960;
                            logWindow.Height = 570;
                            logWindow.Title = TextCodeTranslator.Translate("Customs.Vendor.O.New");
                            logWindow.WindowArgs = args;
                            logWindow.ShowCloseButton = true;
                          logWindow.Show('./CustomsModules/CustomsVendor/Components/Components/EditTabs/VendorEditComponent');

                            logWindow.WindowClosed.subscribe(($event: any) => {

                            });
                        });
                    });

                    break;
                }
                default: {
                    this._CustomsRequestMenuService.ShowModal(item, "",null);
                    break;
                }
            }
        }

    }

    OpenListQueryByObjectTable(objectTableName: string) {
        var listArgs = new ListComponentArgs();
        var SelectedQuery = null;

        // Get ObjectTable 
        var objectTablePM = window.ObjectTables.filter(d => d.Name == objectTableName)[0];
        if (AppTool.IsNullOrEmpty( objectTablePM )) {
            console.log("[!] No ObjectTable found for " + objectTableName);
            return;
        }

        // Get Query
        var allQueries: any[] = window.Queries.filter(x => x.ObjectTableId === objectTablePM.Id).sort((a, b) => { return a.IndexOrder - b.IndexOrder });
        if (allQueries.length == 0) {
            console.log("[!] No Queries found for " + objectTablePM.Name);
            return;
        }

        SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0))[0];

        listArgs.QueryCode = SelectedQuery.UniqueCode;
        listArgs.ObjectTableName = objectTablePM.Name;
       
        listArgs.BackButtonTitle = TextCodeTranslator.Translate("Customs.General.O.Customs"); // Customs Request--> General.MH.Customs | Customs-->Customs.General.O.Customs
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            listArgs.DisplayTitle = TextCodeTranslator.Translate(SelectedQuery.NameTextCodeCode);
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                });
        });
    }

    OpenListQueryByQueryCode(queryCode: string) {
        var listArgs = new ListComponentArgs();
        var SelectedQuery = null;

        // Get Query
        var allQueries: any[] = window.Queries.filter(x => x.UniqueCode === queryCode).sort((a, b) => { return a.IndexOrder - b.IndexOrder });
        if (allQueries.length == 0) {
            console.log("[!] No Queries found for " + queryCode);
            return;
        }
        SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0))[0];

        // Get ObjectTable 
        var objectTablePM = window.ObjectTables.filter(d => d.Id == SelectedQuery.ObjectTableId)[0];
        if (AppTool.IsNullOrEmpty(objectTablePM)) {
            console.log("[!] No ObjectTable found for query " + SelectedQuery);
            return;
        }

        listArgs.QueryCode = SelectedQuery.UniqueCode;
        listArgs.ObjectTableName = objectTablePM.Name;

        listArgs.BackButtonTitle = TextCodeTranslator.Translate("General.MH.Customs"); // Customs Request
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            listArgs.DisplayTitle = TextCodeTranslator.Translate(SelectedQuery.NameTextCodeCode);
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                });
        });
    }

   
}
