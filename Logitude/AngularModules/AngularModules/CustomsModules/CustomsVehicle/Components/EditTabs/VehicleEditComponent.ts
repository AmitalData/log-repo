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
import { EntityPMService } from '../../../../Infrastructure/Services/EntityPMService';
import { VehiclePM } from '../../../../Customs/EntityPMs/VehiclePM';
import { VehicleGeneralComponent } from './VehicleGeneralComponent';
import { VehicleMoreDetailsTabComponent } from './VehicleMoreDetailsTabComponent';
import { VehiclesOwnersAndSafetyTabComponent } from './VehiclesOwnersAndSafetyTabComponent';
import { UpdateDeleteVehicleRequestParams } from '../../../../Customs/DataContract/RequestParams/UpdateDeleteVehicleRequestParams';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomsDocumentsComponent } from '../../../CustomsDocuments/Components/CustomsDocumentsComponent';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';


@Component({
    
    templateUrl: './VehicleEditComponent.html',
    providers: [EntityArgs],
})

export class VehicleEditComponent extends BaseComponent {
  public right: any;

    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public EntityPM: VehiclePM;
    public ObjectTableName: string = "Customs.Vehicle";
    public DataContext: any = this;
    public TabsItemsSource: TabItem[] = [];
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityPMService: EntityPMService, public EntityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new VehiclePM();
        let t = this.EntityPM.VehicleOwners.length;//force load ?!?!?
        let t1 = this.EntityPM.AddVehicleSafetyAccessory.length;//force load ?!?!?
        //this.entityArgs = new EntityArgs();
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.Vehicle";
        this.BuildTabs();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response:any) => {
           // this._IsLoaded = true;
            /// alert("this._IsLoaded");
        });
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            //ListComponent  --args = NewEntityArgs {Perspective: null, QueryNameTextCode: "Customs.Vehicle.Q.Vehicles" }
            

            //this.EntityPM = args.EntityPM;
            //this.IsNewEntity = args.IsNewEntity;
        }
        
    }

    //#region Tabs Code
    private timerToken: any;
    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("General", "Customs.Vehicle.TH.General"));
        this.TabsItemsSource.push(new TabItem("VehicleMoreDetailsTabComponent", "Customs.Vehicle.TH.MoreDetails"));
        this.TabsItemsSource.push(new TabItem("VehiclesOwnersAndSafetyTabComponent", "Customs.Vehicle.TH.OwnersAndSafety"));
        this.TabsItemsSource.push(new TabItem("CustomsDocumentsComponent", "Customs.Vehicle.TH.CustomDocuments"));

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

    private GENERAL: VehicleGeneralComponent = null;
    private MORE: VehicleMoreDetailsTabComponent = null;
    private SAFETY: VehiclesOwnersAndSafetyTabComponent = null;
    private CUSTOMDOCUMENTS: CustomsDocumentsComponent = null;


    private CustomsRequestsSheets: any = null;

    public SelectedTab: TabItem;
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {

                    case "General": {
                        if (this.GENERAL == null) {
                            SessionLocator.DynamicLoader.Load(
                                './CustomsModules/CustomsVehicle/Components/EditTabs/VehicleGeneralComponent',
                                myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.GENERAL = cmpRef.instance;
                                    this.GENERAL.SetTabArgs({ EntityPM: this.EntityPM, IsNewEntity: this.IsNewEntity });
                                    this.GENERAL.FillValidationErrorList.subscribe((response: any) => {
                                        this.ValidationErrorsList = response;
                                    });
                                });
                        }
                        break;
                    }
                    case "VehicleMoreDetailsTabComponent": {
                        if (this.MORE == null) {
                            SessionLocator.DynamicLoader.Load(
                                './CustomsModules/CustomsVehicle/Components/EditTabs/VehicleMoreDetailsTabComponent',
                                myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.MORE = cmpRef.instance;
                                    this.MORE.SetTabArgs({ EntityPM: this.EntityPM, IsNewEntity: this.IsNewEntity });
                                    this.MORE.FillValidationErrorList.subscribe((response: any) => {
                                        this.ValidationErrorsList = response;
                                    });
                                });
                        }
                        break;
                    }

                    case "VehiclesOwnersAndSafetyTabComponent": {
                        if (this.SAFETY == null) {
                            SessionLocator.DynamicLoader.Load(
                                './CustomsModules/CustomsVehicle/Components/EditTabs/VehiclesOwnersAndSafetyTabComponent',
                                myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.SAFETY = cmpRef.instance;
                                    this.SAFETY.SetTabArgs({ EntityPM: this.EntityPM, IsNewEntity: this.IsNewEntity });
                                    this.SAFETY.FillValidationErrorList.subscribe((response: any) => {
                                        this.ValidationErrorsList = response;
                                    });
                                });
                        }
                        break;
                    }


                    case "CustomsDocumentsComponent": {
                        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                            var messageWindow = new MessageWindow();
                            messageWindow.Width = 400;
                            messageWindow.Height = 200;
                          
                            messageWindow.Show("יש לשמור רכבית טרם צירוף מסמכים");
                     
                            break;
                        }
                        if (this.CUSTOMDOCUMENTS == null) {
                            SessionLocator.DynamicLoader.Load(
                                './CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent',
                                myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.CUSTOMDOCUMENTS = cmpRef.instance;
                                  
                                });
                        }
                        break;
                    }
                        
                }


            }
        }
    }

    //#endregion
    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        //ev.Option;
        //ev.ForcePersonalSign
        //ev.RequestVIA
        
        this.SaveEntityChanges(customSendOptionsArgs);
       
        
        

    }
    OkButtonClicked() {
        //this.CurrentSession.CloseCurrentWindowEmit("Ok");
        this.SaveEntityChanges(null);
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }
    private SaveEntityChanges(customSendOptionsArgs) {
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.ValidationErrorsList = [];
            if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

                //if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                //this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);

                this.entityPMService.update(this.ObjectTableName, this.EntityPM).then((res: any) => {
                    res.subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                            //this.SaveCompleted.emit(false);
                        }

                        else {
                            this.EntityPM = myResponse.Result;
                            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

                                var myErrors: string[] = [];
                                myErrors.push("this.EntityPM.Id is null");
                                this.ValidationErrorsList = myErrors;
                            } else {
                                if (customSendOptionsArgs == null) {

                                    this.SelectedTabCode = "General";
                                    this.SelectionChanged();
                                    // this.CancelButtonClicked();
                                } else {
                                    var currRequestParams = new UpdateDeleteVehicleRequestParams();///Force new GUID On Each Send !!
                                    currRequestParams.LoggingEnabled = true;
                                    currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
                                    currRequestParams.Tenant = SessionLocator.Tenant;

                                    currRequestParams.VehicleId = this.EntityPM.Id;
                                    currRequestParams.IsDelete = false;



                                    CustomMessageProgressComponent
                                        .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                                            "שליחת מסר עדכון פרטי רכב", true)
                                        .then((res) => {
                                            console.log(res);
                                            this.CancelButtonClicked();
                                        }
                                        ).catch((err) => {
                                            this.ValidationErrorsList.push(err);
                                            this.CancelButtonClicked();
                                        });

                                    var myIIGGeneralMessagesService = new IIGGeneralMessagesService();

                                    myIIGGeneralMessagesService.PostVehicleRequest(currRequestParams)
                                        .subscribe((myServiceResponse: ServiceResponse) => {
                                            //this.CurrentSession.StopBusyIndicator();

                                            //this.ResponseData = myServiceResponse.Result;
                                            //this.OnMassageDisplayMethod();
                                        });
                                }

                            }
                        }

                    }, error => {
                        this.CurrentSession.StopBusyIndicator();
                        var myErrors: string[] = [];
                        myErrors.push(error.message);
                        this.ValidationErrorsList = myErrors;
                        //this.SaveCompleted.emit(false);
                    });
                });
            //}
                return;
            }

            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

            //if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
                //this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);

                this.entityPMService.insert(this.ObjectTableName, this.EntityPM).then((res: any) => {
                    res.subscribe((myResponse: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                            //this.SaveCompleted.emit(false);
                        }

                        else {
                            this.EntityPM = myResponse.Result;
                            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

                                var myErrors: string[] = [];
                                myErrors.push("this.EntityPM.Id is null");
                                this.ValidationErrorsList = myErrors;
                            } else {
                                if (customSendOptionsArgs == null) {

                                    this.SelectedTabCode = "General";
                                    this.SelectionChanged();
                                   // this.CancelButtonClicked();
                                } else {
                                    var currRequestParams = new UpdateDeleteVehicleRequestParams();///Force new GUID On Each Send !!
                                    currRequestParams.LoggingEnabled = true;
                                    currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
                                    currRequestParams.Tenant = SessionLocator.Tenant;

                                    currRequestParams.VehicleId = this.EntityPM.Id;
                                    currRequestParams.IsDelete = false;



                                    CustomMessageProgressComponent
                                        .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
                                        "שליחת מסר עדכון פרטי רכב", true)
                                        .then((res) => {
                                            console.log(res);
                                             this.CancelButtonClicked();
                                        }
                                        ).catch((err) => {
                                            this.ValidationErrorsList.push(err);
                                            this.CancelButtonClicked();
                                        });

                                    var myIIGGeneralMessagesService = new IIGGeneralMessagesService();

                                    myIIGGeneralMessagesService.PostVehicleRequest(currRequestParams)
                                        .subscribe((myServiceResponse: ServiceResponse) => {
                                            //this.CurrentSession.StopBusyIndicator();

                                            //this.ResponseData = myServiceResponse.Result;
                                            //this.OnMassageDisplayMethod();
                                        });
                                }

                            }
                        }

                    }, error => {
                        this.CurrentSession.StopBusyIndicator();
                        var myErrors: string[] = [];
                        myErrors.push(error.message);
                        this.ValidationErrorsList = myErrors;
                        //this.SaveCompleted.emit(false);
                    });
                });
            //}

    }
}

class TabItem { constructor(public code: string, public textCode: string) { } }
