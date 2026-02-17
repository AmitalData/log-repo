declare var System: any;
declare var window: any;
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { TapagMessagesService } from '../../../../../Customs/Services/WebServices/TapagMessagesService';
import { TapagPMService } from '../../../../../Customs/Services/StandardPMs/TapagPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';;
import { TapagList } from '../../../../../Customs/EntityLists/TapagList';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';


@Component({
    moduleId: module.id,
    templateUrl: './DeclarationTapagTabComponent.html',
})

export class DeclarationTapagTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: DeclarationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;
    public CurrentEditComponentId: string;
    public tapagObslist: ObservableCollection;
    private tapagMessagesService: TapagMessagesService = new TapagMessagesService;
    private tapagPMService: TapagPMService = new TapagPMService;

    IsLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.tapagObslist = new ObservableCollection([]);

        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.TapagConnectionTable").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.Tapag").subscribe((response: any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.Deposit").subscribe((response: any) => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.DepositCondition").subscribe((response: any) => {
                            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response: any) => {
                                this.EntityResourceService.getEntityResourceByTableName("Customs.Guarantee").subscribe((response: any) => {
                                    this.EntityResourceService.getEntityResourceByTableName("Customs.GuaranteeCondition").subscribe((response: any) => {
                                        this.EntityResourceService.getEntityResourceByTableName("Customs.RequiredGuaranteeType").subscribe((response: any) => {
                                            this.EntityResourceService.getEntityResourceByTableName("Customs.Deficit").subscribe((response: any) => {
                                                this.EntityPM = this.entityArgs.EntityPM;
                                                this.ObjectTableName = this.entityArgs.ObjectTableName;
                                                this.LoadTapagsList();
                                                this.Listen();

                                                this.IsLoaded = true;
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
    }

    ngOnDestroy() {
        console.log("DeclarationTapagTabComponent:ngOnDestroy");
        this.entityArgs = null;
    }
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.LoadTapagsList();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCTP") {
                            this.LoadTapagsList();
                        }
                    }
                })
            );
        }
    }

    private LoadTapagsList() {
        this.tapagObslist = new ObservableCollection([]);

        this.tapagMessagesService.GetDeclarationTapagsLists(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.GetDeclarationTapagsListsOp_Completed(myResponse, false);
            });
    }

    private GetDeclarationTapagsListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            myResponse.Result.forEach((item) => {
                this.tapagObslist.Insert(item);
            });
        }
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    EditButtonClicked(item: TapagList) {

        if (!AppTool.IsNullOrEmpty(item)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.tapagPMService.get(item.Id).subscribe(response => {
                this.CurrentSession.StopBusyIndicator();
                switch (item.TapagTypeCode) {
                    case "1":
                        {
                            var windowArgs: any = {};
                            windowArgs.EntityPM = response.Result;
                            windowArgs.declarationPM = this.EntityPM;

                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 750;
                            logWindow.Height = 700;
                            logWindow.ShowCloseButton = true;
                            logWindow.WindowArgs = windowArgs;
                            //logWindow.Title = TextCodeTranslator.Translate("Customs.PaymentOrder.TH.Deficits");
                            logWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deficit/PaymentOrderDeficitComponent');
                            this.CurrentSession.StopBusyIndicator();
                            break;
                        }
                    case "2":
                    case "5":
                        {
                            var windowArgs: any = {};
                            windowArgs.EntityPM = response.Result;
                            windowArgs.declarationPM = this.EntityPM;

                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 750;
                            logWindow.Height = 800;
                            logWindow.ShowCloseButton = true;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deposit/PaymentOrderDepositDataComponent');                          
                            this.CurrentSession.StopBusyIndicator();
                            break;
                        }

                    case "4":
                    case "6":
                        {
                            var windowArgs: any = {};
                            windowArgs.EntityPM = response.Result;
                            windowArgs.declarationPM = this.EntityPM;

                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 750;
                            logWindow.Height = 800;
                            logWindow.ShowCloseButton = true;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.Title = TextCodeTranslator.Translate("Customs.Guarantee.O.Guarantee");
                            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Tapag/GuaranteeDataComponent');
                            this.CurrentSession.StopBusyIndicator();
                            break;
                        }

                    default:
                        {
                            var messageWindow = new MessageWindow();
                            messageWindow.Width = 350;
                            messageWindow.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                            break;
                        }
                }
                });
        }

    }

}
