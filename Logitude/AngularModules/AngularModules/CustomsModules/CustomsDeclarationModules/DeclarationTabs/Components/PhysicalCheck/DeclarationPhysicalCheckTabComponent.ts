
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
import { PhysicalCheckWebService } from '../../../../../Customs/Services/WebServices/PhysicalCheckWebService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';;
import { PhysicalCheckPMService } from '../../../../../Customs/Services/StandardPMs/PhysicalCheckPMService';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';



@Component({
    moduleId: module.id,
    templateUrl: './DeclarationPhysicalCheckTabComponent.html',
})

export class DeclarationPhysicalCheckTabComponent extends BaseComponent implements OnInit {
    public EntityPM: DeclarationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;

    public CurrentEditComponentId: string;
    public physicalCheckList: ObservableCollection;

    private physicalCheckWebService: PhysicalCheckWebService = new PhysicalCheckWebService;
    private physicalCheckPMService: PhysicalCheckPMService = new PhysicalCheckPMService;

    IsLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.physicalCheckList = new ObservableCollection([]);

        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PhysicalCheck").subscribe(response => {
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.LoadPhysicalChecks();
                this.Listen();
                this.IsLoaded = true;
            });
        });
    }

    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
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
                        this.LoadPhysicalChecks();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCPC") {
                            //this.LoadPhysicalChecks();
                        }
                    }
                })
            );
        }
    }

    private LoadPhysicalChecks() {
        this.physicalCheckWebService.GetPhysicalCheckByDeclarationIdLists(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.GetPhysicalCheckByDeclarationIdListsOp_Completed(myResponse, false);
            });
    }

    private GetPhysicalCheckByDeclarationIdListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {
            this.physicalCheckList.InsertCollection(myResponse.Result);
            //myResponse.Result.forEach((item) => {
            //    this.physicalCheckList.Insert(item);
            //});
        }
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    EditButtonClicked(item) {
        if (!AppTool.IsNullOrEmpty(item)) {
            //this.CurrentSession.StartBusyIndicator("");


            var miri = false;
            if (miri){
                this.physicalCheckPMService.get(item.Id).subscribe(response => {
                    var windowArgs: any = {};
                    windowArgs.EntityPM = response.Result;
                    windowArgs.declarationPM = this.EntityPM;

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 1030;
                    logWindow.Height = 600;
                    //windowArgs.IsDisplayOnly = this.IsDisplayOnly; /// to check?
                    //logWindow.ShowCloseButton = false;
                    logWindow.WindowArgs = windowArgs;
                    logWindow.WindowClosed.subscribe(($event: any) => {
                        this.RefreshEntity();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    });
                    logWindow.Show('./CustomsModules/CustomsPhysicalCheck/Components/EditTabs/General/PhysicalCheckGeneralTabComponent');

                    this.CurrentSession.StopBusyIndicator();
                });
                
            }  
            //var logWindow = new LogitudeWindow();
            //logWindow.Width = 1030;
            //logWindow.Height = 600;
            //logWindow.ShowEditComponent(item.id, "Customs.PhysicalCheck");
            ////logWindow.InjectEditComponent(item.id, "Customs.PhysicalCheck", logWindow);
            //logWindow.WindowClosed.subscribe(($event: any) => {
            //    this.RefreshEntity();
            //    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
            //});

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    //this.showAlert = false;
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: item.Id, ObjectTableName: 'Customs.PhysicalCheck', BackButtonLabel: 'Declararion' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        this.RefreshEntity();
                    });
                });
            

        }

    }
}
