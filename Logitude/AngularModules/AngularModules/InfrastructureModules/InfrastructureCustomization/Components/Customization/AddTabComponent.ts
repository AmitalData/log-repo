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
    private screensService: ScreenExtendedService = new ScreenExtendedService();
    ValidationErrorsList: any[];
    public TableTab: ObjectTableTabPM;
    objectTable;
    screens: ScreenPM[] = [];
    IsNew: boolean = true;

    prevName: string;
    prevScreenCode: string;
    prevScreenName: string;

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
        this.TableTab.Changeset = tab ? 'update' : 'insert';
        this.TableTab.Type = tab?.Type || 'Custom';
        this.TableTab.Name = tab?.Name || tab?.TabNameTextCodeDefaultText;

        if(tab){
            this.prevName = tab.Name;
            this.prevScreenCode = tab.ScreenCode;
            this.prevScreenName = tab.ScreenName;
        }
    }



    SetWindowArgs(args: any) {
        this.args = args.ViewModel;
        this.objectTable = window.ObjectTables.filter(x => x.Id === this.args.objectTableId)[0];
        this.InitializeTab(args.tab);
        this.screens = window.Screens.filter(d=>d.Type == "LIGHTENING" && d.ObjectTableId ==this.args.objectTableId );
        this.SetSelectedScreen();


    }


    private SetSelectedScreen()
    {
        if (this.TableTab.ScreenCode)
            this.SelectedScreen = this.screens.find(s => s.Code == this.TableTab.ScreenCode);
    }

    get Name() { return this.TableTab ? this.TableTab.Name:""; }
    set Name(newValue: string) {
        if (this.TableTab.Name != newValue) {
            this.TableTab.Name = newValue;
        }
    }


    SelectedScreen;
    ScreenChanged(screen: ScreenPM){
        this.SelectedScreen = screen;
        this.TableTab.ScreenCode = screen?.Code;
        this.TableTab.ScreenName = screen?.Name;

    }



    SaveButtonClicked() {
        let errors = [];

        if (!this.TableTab.Name)
            errors.push(valdationMessageOfName);

        if(!this.TableTab.ScreenCode && this.TableTab.Type == 'Custom')
            errors.push(validationMessageOfScreen);

        if(errors.length > 0)
            return this.ValidationErrorsList = errors;


        this.CurrentSession.CloseCurrentWindowData({entity: this.TableTab});
    }

    HandleException() {

    }

    CancelButtonClicked() {
        this.revertChanges();

        this.CurrentSession.CloseCurrentWindow();
    }



    private revertChanges()
    {
        this.TableTab.ScreenCode = this.prevScreenCode;
        this.TableTab.ScreenName = this.prevScreenName;
        this.TableTab.Name = this.prevName;
    }
}
