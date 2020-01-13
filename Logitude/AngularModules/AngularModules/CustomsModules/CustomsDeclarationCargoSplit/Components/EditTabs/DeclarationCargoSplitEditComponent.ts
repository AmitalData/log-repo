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
import { DeclarationCargoSplitPM } from '../../../../Customs/EntityPMs/DeclarationCargoSplitPM';
import { CargoSplitGeneralTabComponent } from './General/CargoSplitGeneralTabComponent';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';


@Component({
    moduleId: module.id,
    templateUrl: './DeclarationCargoSplitEditComponent.html',
    providers: [EntityArgs],
})

export class DeclarationCargoSplitEditComponent extends BaseComponent {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public EntityPM: DeclarationCargoSplitPM;
    public ObjectTableName: string = "Customs.DeclarationCargoSplit";
    public DataContext: any = this;
    public TabsItemsSource: TabItem[] = [];
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityPMService: EntityPMService, public EntityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new DeclarationCargoSplitPM();
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.DeclarationCargoSplit";
        this.BuildTabs();
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
    
           // this._IsLoaded = true;
            /// alert("this._IsLoaded");
        });
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
        }
        
    }

    //#region Tabs Code
    private timerToken: any;
    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("General", "Customs.DeclarationCargoSplit.TH.General"));
        

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

    private GENERAL: CargoSplitGeneralTabComponent = null;

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
                                './CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/General/CargoSplitGeneralTabComponent',
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
                     
                }


            }
        }
    }

    //#endregion
    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        
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
                this.CancelButtonClicked();
                return;
            }

            this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

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
                                if (customSendOptionsArgs==null) {
                                    this.CancelButtonClicked();
                                } else {

                                    CustomMessageProgressComponent
                                        .ShowProgressBar("",
                                        " ", true)
                                        .then((res) => {
                                            console.log(res);
                                            this.CancelButtonClicked();
                                        }
                                        ).catch((err) => {
                                            this.ValidationErrorsList.push(err);
                                            this.CancelButtonClicked();
                                        });

                                    var myIIGGeneralMessagesService = new IIGGeneralMessagesService();

                                    //myIIGGeneralMessagesService.PostDeclarationCargoSplitRequest("")
//                                        .subscribe((myServiceResponse: ServiceResponse) => {
  //                                      });
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
