
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
import { Alert } from 'selenium-webdriver';
import { Guid } from 'Infrastructure/Utilities/Guid';

@Component({

    templateUrl: './DeclarationExportStorageComponent.html',
})

export class DeclarationExportStorageComponent extends BaseComponent implements OnInit {
    public EntityPM: DeclarationPM = null;
    public ObjectTableName = "Customs.Declaration";
    public DataContext: this;
    _EntityPMService: EntityPMService;
    public CurrentEditComponentId: string;
    public DeclarationExportStorageList: ObservableCollection;

    
    private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;
    requestParams: CargoSplitRequestParams = new CargoSplitRequestParams();
    responseData: INF_MSG_GenericResponseData = new INF_MSG_GenericResponseData();

    IsLoaded: boolean = false;

    constructor(private entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        
        this.DeclarationExportStorageList = new ObservableCollection([]);
        this._EntityPMService = new EntityPMService();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.ExportStorage").subscribe((response: any) => {
                
                this.EntityPM = this.entityArgs.EntityPM;
                this.ObjectTableName = this.entityArgs.ObjectTableName;
                this.LoadDeclarationExportStorages();
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
                        this.LoadDeclarationExportStorages();
                    }
                })
            );

            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    
                    if (this.CurrentEditComponentId == SessionLocator.SelectedSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DESL") {
                            this.LoadDeclarationExportStorages();
                        }
                    }
                })
            );
        }
    }

    private LoadDeclarationExportStorages() {
        this.DeclarationExportStorageList = new ObservableCollection([]);
          //this.DeclarationCargoSplitWebService.GetDeclarationCargoSplitByDeclarationIdLists(this.EntityPM.Id, this.EntityPM.Tenant)
        this._DeclarationWebService.GetDeclarationExportStoragesByDeclarationIdAndExportFile(this.EntityPM.Id, this.EntityPM.ExportFile, this.EntityPM.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                 SessionLocator.SelectedSession.StopBusyIndicator();

                this.GetDeclarationExportStorageListsOp_Completed(myResponse, false);

            });
    }


    private GetDeclarationExportStorageListsOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        if (myResponse.Result != null) {

            myResponse.Result.forEach((item) => {
                this.DeclarationExportStorageList.Insert(item);
            });
        }
    }
   

    RefreshEntity() {
        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
    }




    OnRowSelected(event) {
       
        if (true)
            var selected = event;

        if (selected) {
            
            var currentScreenCode = "DESL";
            var objectTableName = "Customs.ExportStorage";

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({
                        SelectedTabCode: currentScreenCode,
                        EntityId: selected?.Id,
                        ObjectTableName: objectTableName,
                    });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
                            SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = false;
                        }
                       
                    });



                });


            return;

        }
    }


    
    

    



}
