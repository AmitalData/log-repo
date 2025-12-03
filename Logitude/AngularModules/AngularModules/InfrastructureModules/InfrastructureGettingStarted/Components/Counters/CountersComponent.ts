declare var window: any;
import {Component, OnInit} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {CounterPM} from '../../../../Common/EntityPMs/CounterPM';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CountersDomainService} from '../../../../Common/Services/CountersDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './CountersComponent.html',
})

export class CountersComponent implements OnInit {
    public Counters: CounterItem[] = [];
    public IsDemoTenant: boolean = false;
    public IsResourcesReady: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString())) {
            if (SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com") {
                this.IsDemoTenant = true;
            }
        }
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("CounterDefinition").subscribe((res1: any) => {
            this.IsResourcesReady = true;
            this.LoadCounters();
        });
    }

    LoadCounters() {

        var myService = new CountersDomainService();
        myService.GetTenantCounters().subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {

                var myCounters: CounterItem[] = [];

                var list: CounterPM[] = myResponse.Result;

                list.forEach(item => {
                    var ObjectTable: ObjectTablePM = window.ObjectTables.filter(x => x.Id === item.ObjectTableId)[0];
                    if (ObjectTable) {
                        //if (FeatureLocator.HasFeaturePermession(ObjectTable.Name, "Module")) {

                            if (item.Code == "CNST") {
                                if (FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent")) {
                                    myCounters.push(new CounterItem(item));
                                }
                            }

                            else if (item.Code == "CUST") {
                                if (ObjectsLocator.CustomsInterfaceSettingPM != null) {
                                    if (ObjectsLocator.CustomsInterfaceSettingPM.ActivateCustomsManagementInShipments) {
                                        myCounters.push(new CounterItem(item));
                                    }
                                }
                            }

                            else {
                                myCounters.push(new CounterItem(item));
                            }
                      //  }
                    }
                });

                this.Counters = myCounters.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
            }
        });
    }

    ItemClicked(item: CounterItem) {
        if (item) {
            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = { CounterId: item.CounterPM.Id, FatherComponent: this };            
            logWindow.Title = item.Name + " Counters";           

            switch (item.Code) {
                case "HAWB": {
                    logWindow.Width = 885;
                    logWindow.Height = 520;
                    logWindow.Title = "HAWB Counters";
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterHAWBComponent");
                    break;
                }
               
                case "INVC": {
                    logWindow.IsFillScreen = true;
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterInvoiceComponent");
                    break;
                }

                case "SHIP":
                case "MAST":
                case "QUOT": {
                    logWindow.IsFillScreen = true;
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterAdvancedComponent");
                    break;
                }
                case "CADC": {
                    logWindow.IsFillScreen = true;
                    logWindow.Title = "Card Counters";
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterCardComponent");
                    break;
                }
                default: {
                    logWindow.Show("./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/EditComponents/CounterTableComponent");
                    break;
                }
            }         
        }
    }

    Close() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
export class CounterItem {
    public Code: string;
    public Name: string;
    public CounterPM: CounterPM;
    constructor(itemPM: CounterPM) {
        this.Code = itemPM.Code;
        this.Name = itemPM.Name;
        this.CounterPM = itemPM;
    }
}
