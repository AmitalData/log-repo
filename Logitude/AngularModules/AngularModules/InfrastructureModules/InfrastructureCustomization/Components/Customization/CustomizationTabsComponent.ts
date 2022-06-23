import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { ObjectTableTabPM } from 'Infrastructure/EntityPMs/ObjectTableTabPM';
import { ScreenPM } from 'Infrastructure/EntityPMs/ScreenPM';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { TableTabService } from 'Infrastructure/Services/ExtendedPMs/TableTabService';
import { LoginService } from 'Infrastructure/Services/LoginService';

type tab = {
    Changset: 'insert' | 'update' | 'delete' | null;
    CustomOrder: number;
    ScreenId: string;
    Type: 'Custom' | 'Predefined';
    Screen: string;
    Name: string;
}

declare var window;
const defaultWindowWidth = 450;
const defaultWindowHeight = 200;
const newTabWindowTitle = "New Tab";
@Component({

    templateUrl: './CustomizationTabsComponent.html',
    styleUrls: ['./CustomizationTabsComponent.css']
})

export class CustomizationTabsComponent extends BaseComponent
{
    public DataContext: CustomizationTabsComponent = this;
    public ObjecttableId: string;
    public ObjectTableName: string;
    public ValidationErrorsList: Array<String> = [];
    private ObjectTable: ObjectTablePM;
    private CurrentSession = SessionLocator.SelectedSession;
    tableTabsService = new TableTabService();

    GridDisabled: boolean = false;
    tabs: ObjectTableTabPM[] = [];
    orderedTabs = new ObservableCollection([]);

    constructor(private loginService: LoginService)
    {
        super();
    }

    OrderTabs()
    {
        this.orderedTabs = new ObservableCollection(this.tabs.sort((a, b) => { return (a.CustomOrder === b.CustomOrder) ? 0 : (a.CustomOrder < b.CustomOrder) ? -1 : 1 }));
    }

    SetWindowArgs(windowArgs: any)
    {
        this.ObjecttableId = windowArgs.ObjectTableID;
        this.ObjectTable = window.ObjectTables.filter(x => x.Id === this.ObjecttableId)[0];

        this.GetEntityTabs();
        this.OrderTabs();
    }

    CancelClicked = () => this.CurrentSession.CloseCurrentWindow();

    private GetEntityTabs()
    {
        const tableTabs = window.ObjectTableTabs.filter(a => a.ObjectTableId == this.ObjecttableId) || [];
        this.tabs = tableTabs.map(a => { return { ...a, Type: 'Predefined' }; });
        this.SetTabsAdditionalFields();


    }
    SetTabsAdditionalFields()
    {
        const screens: ScreenPM[] = window.Screens.filter(d => d.ObjectTableId == this.ObjecttableId) || [];

        for (let i = 0; i < this.tabs.length; i++) {
            let tab = this.tabs[i];
            tab.ScreenName = screens.find(a => a.Id == tab.ScreenId)?.Name;
            tab.Name = tab.TabNameTextCodeDefaultText;
            tab.CustomOrder = tab.CustomOrder || i;
        }
    }

    UpdateAllTabsAndCloseWindow()
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.loginService.GetObjectTableTabs().subscribe((tabs: any) =>
        {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();

            window.ObjectTableTabs = tabs;
        });
    }

    OkClicked()
    {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.tableTabsService.UpdateTabs(this.tabs)
            .subscribe(arg => {
                this.CurrentSession.StopBusyIndicator();
                this.UpdateAllTabsAndCloseWindow();
            }, error=>{
                alert("error happened!");
            });
    }

    NewTabClicked()
    {
        var window = new LogitudeWindow();
        window.Width = defaultWindowWidth;
        window.Height = defaultWindowHeight;
        window.Title = newTabWindowTitle;
        window.WindowArgs = {ViewModel: this};
        window.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddTabComponent');

        window.WindowClosed.subscribe(data=>{
            if(data){
                this.AddTab(data.entity);
            }
        });

    }

    AddTab(newTab: ObjectTableTabPM){
        newTab.CustomOrder = this.tabs.length;

        this.tabs.push(newTab);
        this.OrderTabs();
    }

    DeleteTab(tab)
    {

    }
    DecOrder(currentTab)
    {
        if (currentTab.CustomOrder == 0)
            return;

        const prevTab = this.tabs.find(c => c.CustomOrder == currentTab.CustomOrder - 1);
        prevTab.CustomOrder = prevTab.CustomOrder + 1;
        currentTab.CustomOrder = currentTab.CustomOrder - 1;

        this.OrderTabs();
    }
    IncOrder(currentTab)
    {
        if (currentTab.CustomOrder == this.tabs.length - 1)
            return;

        const nextTab = this.tabs.find(c => c.CustomOrder == currentTab.CustomOrder + 1);
        nextTab.CustomOrder = nextTab.CustomOrder - 1;
        currentTab.CustomOrder = currentTab.CustomOrder + 1;

        this.OrderTabs();
    }

}
