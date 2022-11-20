import { Component } from '@angular/core';
import { ScreenExtendedService } from '../../../../Infrastructure/Services/ExtendedPMs/ScreenExtendedService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
declare var window: any;
import { ScreenPM } from '../../../../Infrastructure/EntityPMs/ScreenPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTableTabPM } from 'Infrastructure/EntityPMs/ObjectTableTabPM';

const valdationMessageOfName = 'Please fill the tab name';
const validationMessageOfScreen = 'Please select screen';
@Component({

    templateUrl: './AddTabComponent.html',
})

export class AddTabComponent extends BaseComponent {
    dataContext = this;
    args: any;
    ValidationErrorsList: any[];
    public TableTab: ObjectTableTabPM;
    objectTable;
    screens: ScreenPM[] = [];
    IsNew: boolean = true;
    private customizationMainComponent: any;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.UIProperties.SetRequired("Code", "ObjectField", true);
    }

    private InitializeTab(tab: ObjectTableTabPM = null)
    {
        this.TableTab = tab || new ObjectTableTabPM();
        this.TableTab.ObjectTableName = this.objectTable.Name;
        this.TableTab.Tenant = tab ? tab.Tenant : SessionLocator.Tenant;
        this.TableTab.ObjectTableId = this.objectTable.Id;
        this.TableTab.Type = tab?.Type || 'Custom';
        this.TableTab.Name = tab?.Name || tab?.TabNameTextCodeDefaultText;
        this.TableTab.Changeset = this.IsNew ? "insert" : tab?.Changeset;
        this.TableTab.HideTabNameInScreen = tab ? tab.HideTabNameInScreen : true;

        this.Name = this.TableTab.Name;
        this.ScreenCode = this.TableTab.ScreenCode;
        this.ScreenName = this.TableTab.ScreenName;
        this.DisplayTabNameInScreen = !this.TableTab.HideTabNameInScreen;

    }


    SetWindowArgs(args: any) {
        this.customizationMainComponent = args.CustomizationMainComponent;
        this.IsNew = args.IsNew;
        this.objectTable = window.ObjectTables.filter(x => x.Id === this.customizationMainComponent.objectTableId)[0];
        this.InitializeTab(args.tab);
        this.screens = window.Screens.filter(d => d.Type == "LIGHTENING" && d.ObjectTableId == this.customizationMainComponent.objectTableId && !d.Inactive );
        this.SetSelectedScreen();


    }


    private SetSelectedScreen()
    {
        if (this.TableTab.ScreenCode)
            this.SelectedScreen = this.screens.find(s => s.Code == this.TableTab.ScreenCode);
    }


    private name: string;
    get Name() { return this.name; }
    set Name(newValue: string) {
        if (this.name != newValue) {
            this.name = newValue;
        }
    }

    private screenCode: string;
    get ScreenCode() { return this.screenCode; }
    set ScreenCode(newValue: string) {
        if (this.screenCode != newValue) {
            this.screenCode = newValue;
        }
    }

    private screenName: string;
    get ScreenName() { return this.screenName; }
    set ScreenName(newValue: string) {
        if (this.screenName != newValue) {
            this.screenName = newValue;
        }
    }



    SelectedScreen;
    ScreenChanged(screen: ScreenPM){
        this.SelectedScreen = screen;
        this.ScreenCode = screen?.Code;
        this.ScreenName = screen?.Name;
    }

    private displayTabNameInScreen: boolean = false;
    get DisplayTabNameInScreen() { return this.displayTabNameInScreen; }
    set DisplayTabNameInScreen(value: boolean) {
        if (this.displayTabNameInScreen != value) {
            this.displayTabNameInScreen = value;
        }
    }

    SaveButtonClicked() {
        let errors = [];
        if (!this.Name)
            errors.push(valdationMessageOfName);


        if(!this.ScreenCode && this.TableTab.Type == 'Custom')
            errors.push(validationMessageOfScreen);

        if(errors.length > 0)
            return this.ValidationErrorsList = errors;

        this.MapTabFields();
         this.customizationMainComponent.SetChangeSet(this.TableTab);

        this.CurrentSession.CloseCurrentWindowData({entity: this.TableTab});
    }

  private  MapTabFields() {
      this.TableTab.Name = this.Name;
      this.TableTab.ScreenCode = this.ScreenCode;
      this.TableTab.ScreenName = this.ScreenName;
      this.TableTab.HideTabNameInScreen = !this.DisplayTabNameInScreen;
    }



    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



}
