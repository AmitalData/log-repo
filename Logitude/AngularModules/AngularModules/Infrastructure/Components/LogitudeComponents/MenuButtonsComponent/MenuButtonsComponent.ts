declare var window: any;
declare var System: any;
import {Component, Output, EventEmitter, OnDestroy, ChangeDetectorRef} from '@angular/core';
import {ObjectTablePM} from '../../../EntityPMs/ObjectTablePM'
import {MenuButtonPM} from '../../../EntityPMs/MenuButtonPM'
import {MenuButtonGroupPM} from '../../../EntityPMs/MenuButtonGroupPM'
import {EntityArgs} from '../../../DataContracts/EntityArgs';
import {SessionLocator} from '../../../Utilities/SessionLocator'
import {ServiceHelper} from '../../../Utilities/ServiceHelper'
import {AppTool} from '../../../Tools'
import {FeatureLocator} from '../../../Utilities/FeatureLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {MenuButtonsEvents, MenuButtonsStateChangedEventArgs} from '../../../../Infrastructure/Utilities/events/MenuButtonsEvents';
import {ObjectsLocator} from '../../../Locators/ObjectsLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { IObjectTableMenuButtonsBuilder } from '../../../Interface/IObjectTableMenuButtonsBuilder';
//import { ObjectTableMenuButtonsBuilderService } from '../../../Utilities/ObjectTableMenuButtonsBuilderService';

@Component({
    
    selector: 'MenuButtonsComponent',
    templateUrl: "./MenuButtonsComponent.html",
})

export class MenuButtonsComponent implements OnDestroy {
    private baseMetaUrlApi: string;
    public EntityPM: any;
    public ObjectTable: ObjectTablePM;
    public MenuButtonGroup: MenuButtonGroupPM;
    public MenuButtons: MenuButtonPM[];
    public ToggleButtonTop: string = "22px";
    private MenuButtonsHandler: any;
    IsDisableMenuOther: boolean = false;
    Loaded: boolean = false;
    ToggleButtonWidth:number = 60;
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    LayoutDirection: string = 'ltr';
    MenuButtonsStateChangedEvent: any;
    QuerySection: string;
    constructor(public entityArgs: EntityArgs, public cd: ChangeDetectorRef) {
        this.baseMetaUrlApi = ServiceHelper.GetLogitudeURL() + "api/ngMetaData";
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;

    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.entityArgs.EditComponent) {

            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                        if (this.MenuButtonsHandler && this.MenuButtons) {
                            this.MenuButtonsHandler.EntityPM = this.EntityPM;
                            this.MenuButtonsHandler.CheckButtonState(this.MenuButtons);
                        }
                    }
                });
            }

            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                        if (this.MenuButtonsHandler && this.MenuButtons) {
                            this.MenuButtonsHandler.EntityPM = this.EntityPM;
                            this.MenuButtonsHandler.CheckButtonState(this.MenuButtons);
                        }
                    }
                });
            }
        }

       
    }

    ngOnDestroy() {
        if (this.SaveCompletedEvent) {
            this.SaveCompletedEvent.unsubscribe();
            this.SaveCompletedEvent = null;
        }

        if (this.LoadCompletedEvent) {
            this.LoadCompletedEvent.unsubscribe();
            this.LoadCompletedEvent = null;
        }
    }

    Run(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTable = args['ObjectTable'];
        this.QuerySection = args['QuerySection'];

        
        if (this.ObjectTable && this.ObjectTable.Name == "Quote") {
            if (this.EntityPM != null) {
                if (this.EntityPM.IsQuoteDataExternal && this.EntityPM.IsQuoteDocumentExternal) {
                    this.IsDisableMenuOther = true;
                }
            }
        }

        this.LoadMenuButtons();

    }

  private LoadMenuButtons() {
    ServiceHelper.HttpClient.get(this.baseMetaUrlApi + "/getmenubuttongrouppms?tenant=" + SessionLocator.Tenant + "&objecttableid=" + this.ObjectTable.Id).subscribe((response) => {
            var pm = response[0];
            this.MenuButtonGroup = this.MapJsonToEntityPM(pm, true);
            this.BuildMenuButtons();
        });
    }

    public BuildMenuButtons() {
        this.Listen();
        this.ToggleButtonWidth = 60;
        var btns = this.MenuButtonGroup.MenuButtons.sort((a, b) => {
            if (a.Index > b.Index) {
                return 1;
            }
            else if (b.Index > a.Index) {
                return -1;
            }

            return 0

        });

        var buttons: MenuButtonPM[] = [];
        for (var i = 0; i < btns.length; i++) {
            if (btns[i].FeatureUniqeCode != null && btns[i].FeatureUniqeCode != undefined) {//if (btns[i].FeatureId != null && btns[i].FeatureId != undefined) {
                if (FeatureLocator.IsFeatureGrantedByUniqeCode(btns[i].FeatureUniqeCode)) {
                    buttons.push(btns[i]);
                }
            }
            else {
                buttons.push(btns[i]);
            }
        }


        buttons = this.GetObjectTableMenuButtonsByQuerySection(this.QuerySection);

        var objectTableName = this.ObjectTable.Name;
        if (objectTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var myComponentPath = "./" + this.ObjectTable.ClientModuleName + "/Components/MenuButtons/" + objectTableName + "MenuButtonsHandler";
        var myComponentName = AppTool.GetComponentName(myComponentPath);

        SessionLocator.DynamicLoader.GetInstance(myComponentPath).then((instance: any) => {
            if (instance) {
                this.MenuButtonsHandler = instance;
                instance.SetEntityPM(this.entityArgs);
                instance.CheckButtonState(buttons);


    
       
                for (var i = 0; i < buttons.length; i++){

                    buttons[i].Width == 0 ? buttons[i].Width = 110 : null;

                    if (buttons[i].EventCode == "Accept" || buttons[i].EventCode == "Decline") {
                        buttons[i].Width = 70;
                    }
                    if (objectTableName == "PaymentCheque" && buttons[i].EventCode == "More") {
                        this.ToggleButtonWidth = 100;
                    }

                    if (buttons[i].DisplayText == null) {
                        buttons[i].DisplayText = TextCodeTranslator.Translate(buttons[i].LabelTextCodeCode);
                    }
                    else buttons[i].DisplayText = buttons[i].DisplayText;
                    
                }

                this.MenuButtons = buttons;
                return;
            }
        });
    }
    public DisplayText: string;
    private GetObjectTableMenuButtonsByQuerySection( querySection:string) {
  
        //let objectTableMenuButtonsBuilder: IObjectTableMenuButtonsBuilder = ObjectTableMenuButtonsBuilderService.GetInstance(querySection);
        //if (objectTableMenuButtonsBuilder) {
        //  return  objectTableMenuButtonsBuilder.BuildMenuButtons({
        //        ObjectTableId: this.entityArgs.ObjectTableName,
        //        ObjectTableName: this.entityArgs.EntityPM,
        //        QuerySection: this.QuerySection,
        //        EntityPM: this.EntityPM,
        //    });
        //}
        return null;
    }

    public OnClick(button: MenuButtonPM) {
        this.MenuButtonsHandler.MenuButtonClick(button);
    }

    MapJsonToEntityPM(jsonPM: any, getCallMap: boolean = true, entityPM: MenuButtonGroupPM = null) {
        if (!entityPM) {
            entityPM = new MenuButtonGroupPM();
        }
        var jsonPMKeys = Object.keys(jsonPM);

        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        if (getCallMap) {
            entityPM.OldEntityPM = this.clone(entityPM);
        }
        else {

            entityPM.OldEntityPM = null;
        }
        this.MapMenuButtons(entityPM, jsonPM);
        return entityPM;
    }
    public clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};

        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {

            if ((jsonPMKeys[key] === "entityParentPM") || jsonPMKeys[key] === "UIProperties" || jsonPMKeys[key] === "OldEntityPM") {
                continue;
            }

            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];

        }
        return entityPM;
    }
    MapMenuButtons(entityPM: MenuButtonGroupPM, jsonPM: any, mapParent: boolean = true) {
        entityPM.MenuButtons = new Array<MenuButtonPM>();
        for (var pack in jsonPM.MenuButtons) {
            var itemJson = jsonPM.MenuButtons[pack];
            var itemPM: MenuButtonPM;
            if (mapParent) {
                itemPM = new MenuButtonPM(entityPM);
            }
            else {
                itemPM = new MenuButtonPM(null);

            }
            var pmKeys = Object.keys(itemJson);
            for (var key in pmKeys) {

                if (pmKeys[key] === "entityParentPM" || pmKeys[key] === "UIProperties") {
                    continue;
                }
                var property = pmKeys[key];
                itemPM[property] = itemJson[property];
            }
            itemPM.IsDirty = false;
            //itemPM.UIProperties = null;
            entityPM.MenuButtons.push(itemPM);
        }
    }
    
}

