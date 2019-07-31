import {Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';

import {CustomsVendorPM} from '../../../../Customs/EntityPMs/CustomsVendorPM';

@Component({
    moduleId: module.id,
    templateUrl: './VendorEditComponent.html',
    providers: [EntityArgs],

})

export class VendorEditComponent extends BaseComponent {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public EntityPM: CustomsVendorPM;
    public ObjectTableName: string = "Customs.CustomsVendor";
    public DataContext: any = this;
    public TabsItemsSource: TabItem[] = [];
    public IsNewEntity: boolean = false;
    public ValdationErrorList: any[];

    public entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.BuildTabs();
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.EntityPM;
            this.IsNewEntity = args.IsNewEntity;
        }

        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.CustomsVendor";
    }

    //#region Tabs Code
    private timerToken: any;
    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("General", "Customs.Vendor.TH.General"));
        this.TabsItemsSource.push(new TabItem("COMMUNICATION", "Customs.Vendor.TH.Communications"));
        this.TabsItemsSource.push(new TabItem("EVENTS", "Customs.Vendor.TH.Events"));
        this.TabsItemsSource.push(new TabItem("REQUESTSHEET", "General.MH.CustomsRequestsSheets"));

        this.timerToken = setTimeout(() => {
            this.SelectedTabCode = "General"; // to ensure the component was painted
        }, 100);
    }


    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    private GENERAL: any = null;
    private COMMUNICATION: any = null;
    private EVENTS: any = null;
    private REQUESTSHEET: any = null;

    private CustomsRequestsSheets: any = null;

    public SelectedTab: TabItem;
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {

                    case "General": {
                        if (this.GENERAL == null) {
                          SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsVendor/Components/EditTabs/General/VendorGeneralTabComponent',
                                myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.GENERAL = cmpRef.instance;
                                    this.GENERAL.SetTabArgs({ EntityPM: this.EntityPM, IsNewEntity: this.IsNewEntity });
                                    this.GENERAL.FillValidationErrorList.subscribe((response: any) => {
                                        this.ValdationErrorList = response;
                                    });
                                });
                        }
                        break;
                    }
                    //case "Communications": {
                    //    if (this.Communications == null) {
                    //        SessionLocator.DynamicLoader.Load('./Customs/Components/Vendors/EditTabs/Communications/VendorCommunicationsTabComponent',
                    //            myLocation.viewContainerRef)
                    //            .then(cmpRef => {
                    //                this.Communications = cmpRef.instance;
                    //                this.Communications.SetTabArgs({ EntityPM: this.EntityPM, IsNewEntity: this.IsNewEntity});
                    //            });
                    //    }
                    //    break;
                    //}

                    case "COMMUNICATION": {
                        if (this.COMMUNICATION == null) {

                            this.entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(response => {
                                SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.COMMUNICATION = cmpRef.instance;
                                        this.COMMUNICATION.IsTitleHidden = false;
                                        this.COMMUNICATION.InitTab();

                                    });
                            });

                        }
                        break;

                    }

                    case "EVENTS": {
                        if (this.EVENTS == null) {
                            SessionLocator.DynamicLoader.Load("./Common/Components/Events/EventsTabComponent", myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.EVENTS = cmpRef.instance;

                                });


                        }
                        break;

                    }

                    case "REQUESTSHEET": {
                        if (this.REQUESTSHEET == null) {
                            SessionLocator.DynamicLoader.Load("./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent", myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.REQUESTSHEET = cmpRef.instance;
                                    this.REQUESTSHEET.IsTitleHidden = false;
                                    this.REQUESTSHEET.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");


                                });


                        }
                        break;
                    }
                }


            }
        }
    }

    //#endregion

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }
}

class TabItem { constructor(public code: string, public textCode: string) { } }
