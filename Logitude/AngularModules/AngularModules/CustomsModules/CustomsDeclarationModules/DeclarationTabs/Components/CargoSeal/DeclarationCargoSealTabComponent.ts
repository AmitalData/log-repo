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
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';


@Component({
    moduleId: module.id,
    templateUrl: './DeclarationCargoSealTabComponent.html',
})

export class DeclarationCargoSealTabComponent extends BaseComponent implements OnInit, OnDestroy {
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
                    this.EntityPM = this.entityArgs.EntityPM;
                    this.ObjectTableName = this.entityArgs.ObjectTableName;
                    this.LoadTapagsList();
                    this.Listen();
                    this.TapagIdEdit();
                    this.IsLoaded = true;
                });
            });
        });
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
    }

    ngOnDestroy() {
        console.log("DeclarationCargoSealTabComponent:ngOnDestroy");
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
                        if (tabCode == "DCSE") {
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
                this.TapagIdEdit();
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

    TapagIdEdit() {
        var myDeclarationEditComponentController = SessionLocator.SelectedSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController;
        if (!AppTool.IsNullOrEmpty(myDeclarationEditComponentController.TapagId)) {
            if (this.tapagObslist != null && this.tapagObslist.Collection != null) {
                var item = this.tapagObslist.Collection.find(r => r.Id == myDeclarationEditComponentController.TapagId);
                if (item != null) {
                    this.EditButtonClicked(item);
                    console.log("TapagId " + myDeclarationEditComponentController.TapagId);
                    myDeclarationEditComponentController.TapagId = null;
                }
            }
        }

    }

    EditButtonClicked(item: TapagList) {

        var windowArgs: any = {};
        //windowArgs.EntityPM = response.Result;
        windowArgs.declarationPM = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 700;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/Tapag/Deficit/PaymentOrderDeficitComponent');
        this.CurrentSession.StopBusyIndicator();

    }

}
