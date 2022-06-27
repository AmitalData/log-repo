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
    screens: any[] = [];
    IsNew: boolean = true;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.UIProperties.SetRequired("Code", "ObjectField", true);
    }

    private InitializeTab()
    {
        this.TableTab = new ObjectTableTabPM();
        this.TableTab.ObjectTableName = this.objectTable.Name;
        this.TableTab.Tenant = SessionLocator.Tenant;
        this.TableTab.ObjectTableId = this.objectTable.Id;
        this.TableTab.Changeset = 'insert';
        this.TableTab.Type = 'Custom';
    }

    GetEntityScreens(){
        this.CurrentSession.StartBusyIndicatorLoading();
        this.screensService.GetEntityScreens(this.objectTable.Id)
            .subscribe((screens:ScreenPM[]) => {
                this.screens = screens.filter(a=>a.Type=='LIGHTENING');
                this.CurrentSession.StopBusyIndicator();
            });

    }

    SetWindowArgs(args: any) {
        this.args = args.ViewModel;
        this.objectTable = window.ObjectTables.filter(x => x.Id === this.args.objectTableId)[0];
        this.InitializeTab();
        this.GetEntityScreens();

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

        if(!this.TableTab.ScreenCode)
            errors.push(validationMessageOfScreen);

        if(errors.length > 0)
            return this.ValidationErrorsList = errors;


        this.CurrentSession.CloseCurrentWindowData({entity: this.TableTab});
    }

    HandleException() {

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


}
