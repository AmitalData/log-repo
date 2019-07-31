
declare var System: any;
declare var window: any;
import { Component, OnInit } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationCargoSplitWebService } from '../../../../../Customs/Services/WebServices/DeclarationCargoSplitWebService';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';;
import { DeclarationCargoSplitPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationCargoSplitPMService';
import { DeclarationCargoSplitPM } from '../../../../../Customs/EntityPMs/DeclarationCargoSplitPM';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';//test4
import { CargoSplitRequestParams } from '../../../../../Customs/DataContract/RequestParams/CargoSplitRequestParams';
import { INF_MSG_GenericResponseData } from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';

import { EntityPMService } from '../../../../../Infrastructure/Services/EntityPMService';

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationCargoSplitTabComponent.html',
})

export class DeclarationCargoSplitTabComponent extends BaseComponent implements OnInit {
    public EntityPM: DeclarationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;
    _EntityPMService: EntityPMService;
    public CurrentEditComponentId: string;
    public DeclarationCargoSplitList: ObservableCollection;

    private DeclarationCargoSplitWebService: DeclarationCargoSplitWebService = new DeclarationCargoSplitWebService;
    private DeclarationCargoSplitPMService: DeclarationCargoSplitPMService = new DeclarationCargoSplitPMService;
    private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
    requestParams: CargoSplitRequestParams = new CargoSplitRequestParams();
    responseData: INF_MSG_GenericResponseData = new INF_MSG_GenericResponseData();

    IsLoaded: boolean = false;

    constructor(private entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.DeclarationCargoSplitList = new ObservableCollection([]);
        this._EntityPMService = new EntityPMService();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.LoadDeclarationCargoSplits();
                this.Listen();
                this.IsLoaded = true;
            });
        });
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
    }


    private Listen() {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = SessionLocator.SelectedSession.CurrentEditComponent.ComponentId;
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                        this.LoadDeclarationCargoSplits();
                    }
                })
            );

            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == SessionLocator.SelectedSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCCS") {
                            this.LoadDeclarationCargoSplits();
                        }
                    }
                })
            );
        }
    }

    private LoadDeclarationCargoSplits() {
        this.DeclarationCargoSplitList = new ObservableCollection([]);
        //this.DeclarationCargoSplitWebService.GetDeclarationCargoSplitByDeclarationIdLists(this.EntityPM.Id, this.EntityPM.Tenant)
        this._DeclarationWebService.GetDeclarationCargoSplitByDeclarationIdList(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                this.GetDeclarationCargoSplitByDeclarationIdListsOp_Completed(myResponse, false);
                this.CargoSplitIdEdit();
            });
    }


    private GetDeclarationCargoSplitByDeclarationIdListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            //this.DeclarationCargoSplitList.InsertCollection(myResponse.Result);
            myResponse.Result.forEach((item) => {
                this.DeclarationCargoSplitList.Insert(item);
            });
        }
    }

    CargoSplitIdEdit(): any {
        var myDeclarationEditComponentController = SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController;
        if (!AppTool.IsNullOrEmpty(myDeclarationEditComponentController.CargoSplitId)) {
            if (this.DeclarationCargoSplitList != null && this.DeclarationCargoSplitList.Collection != null) {
                var item = this.DeclarationCargoSplitList.Collection.find(r => r.Id == myDeclarationEditComponentController.CargoSplitId);
                if (item != null) {
                    this.EditButtonClicked(item);
                    console.log("CargoSplitId " + myDeclarationEditComponentController.CargoSplitId);
                    myDeclarationEditComponentController.CargoSplitId = null;
                }
            }
        }
    }

    RefreshEntity() {
        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
    }

    EditButtonClickedOld(item: DeclarationCargoSplitPM) {

        var windowArgs: any = {};
        windowArgs.CurrentEntity = item;
        windowArgs.declarationPM = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 770;
        logWindow.Height = 750;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "בקשת פיצול מטען ";
        if (item != null) {
            if (!AppTool.IsNullOrEmpty(item.RequestNumber)) {
                logWindow.Title = logWindow.Title + item.RequestNumber;
            }
            if (!AppTool.IsNullOrEmpty(item.ResponseStatusName)) {
                logWindow.Title = logWindow.Title + " - " + item.ResponseStatusName;
            }
        }
        logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent');
        SessionLocator.SelectedSession.StopBusyIndicator();

        /*if (!AppTool.IsNullOrEmpty(item)) {
            //SessionLocator.SelectedSession.StartBusyIndicator("");

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    //this.showAlert = false;
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: 'Customs.DeclararionCargoSplit', BackButtonLabel: 'Declararion' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this.RefreshEntity();
                    });
                });


        }*/

    }




    EditButtonClicked(item: DeclarationCargoSplitPM) {

        var windowArgs: any = {};
        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this._EntityPMService.getSingle("Customs.DeclarationCargoSplit", item.Id).then((res: any) => {
                    res.subscribe((myResponse: any) => {

                        if (myResponse.HasError) {
                            console.log("Error while getting EntityPM", myResponse);
                        }
                        else {
                            windowArgs.CurrentEntity = myResponse.Result;
                            var logWindow = new LogitudeWindow();

                            logWindow.Width = 770;
                            logWindow.Height = 750;
                            //logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditDeclarationCargoSplit");
                            logWindow.Title = "בקשת פיצול מטען ";// + myResponse.Result != null ? ((!AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber) ? myResponse.Result.RequestNumber : null) + ((!AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName) ? " - " + myResponse.Result.ResponseStatusName : null))) : null;
                            if (myResponse.Result != null) {
                                if (!AppTool.IsNullOrEmpty(myResponse.Result.RequestNumber)) {
                                    logWindow.Title = logWindow.Title + myResponse.Result.RequestNumber;
                                }
                                if (!AppTool.IsNullOrEmpty(myResponse.Result.ResponseStatusName)) {
                                    logWindow.Title = logWindow.Title + " - " + myResponse.Result.ResponseStatusName;
                                }
                            }

                            logWindow.WindowArgs = windowArgs;
                            logWindow.ShowCloseButton = true;
                            //logWindow.IsHideHeader = true;
                            logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent');

                            logWindow.WindowClosed.subscribe(($event1: any) => {
                                //this.isEditControlOpened = false;
                                //this.OnBackFromEdit(selectedEntityId, $event);
                            });
                        }
                    });

                });
            });

        });
    }

    AddDeclarationCargoSplitCommand() {

        var newDeclarationCargoSplitPM = new DeclarationCargoSplitPM();
        newDeclarationCargoSplitPM.Tenant = this.EntityPM.Tenant;
        newDeclarationCargoSplitPM.DeclarationId = this.EntityPM.Id;
        newDeclarationCargoSplitPM.CustomFileNo = this.EntityPM.CustomFileNo;
        //this.DeclarationCargoSplitList.Insert(newDeclarationCargoSplitPM);

        this.NewDeclarationCargoSplit(newDeclarationCargoSplitPM);
    }

    NewDeclarationCargoSplit(item: DeclarationCargoSplitPM) {
        SessionLocator.SelectedSession.StartBusyIndicator("");

        var windowArgs: any = {};
        windowArgs.CurrentEntity = item;
        windowArgs.IsNewEntity = true;
        windowArgs.CustomFileNo = this.EntityPM.CustomFileNo;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 770;
        logWindow.Height = 750;
        //windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Claim.O.NewClaimsRelatedEntity");
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe((event: any) => {
            this.LoadDeclarationCargoSplits();
        });

        logWindow.IsHideHeader = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent');
        SessionLocator.SelectedSession.StopBusyIndicator();

    }
}
