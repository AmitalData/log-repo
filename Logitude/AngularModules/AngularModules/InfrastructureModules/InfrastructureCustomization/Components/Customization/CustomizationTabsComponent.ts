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
    IndexOrder: number;
    ScreenId: string;
    Type: 'Custom' | 'Predefined';
    Screen: string;
    Name: string;
}

declare var window;
const defaultWindowWidth = 450;
const defaultWindowHeight = 200;
const newTabWindowTitle = "New Tab";
const editTabWindowTitle = "Edit Tab";
@Component({

    templateUrl: './CustomizationTabsComponent.html',
    styleUrls: ['./CustomizationTabsComponent.css']
})

export class CustomizationTabsComponent extends BaseComponent
{
    public DataContext: CustomizationTabsComponent = this;
    public objectTableId: string;
    public ObjectTableName: string;
    public ValidationErrorsList: Array<String> = [];
    private ObjectTable: ObjectTablePM;
    private CurrentSession = SessionLocator.SelectedSession;
    tableTabsService = new TableTabService();
    isDirty: boolean = false;
    GridDisabled: boolean = false;
    tabs: ObjectTableTabPM[] = [];
    orderedTabs = new ObservableCollection([]);

    constructor(private loginService: LoginService)
    {
        super();
    }

    LoadTabs()
    {
        const filteredTabs = this.tabs.filter(t=>t.Changeset != 'delete');
        this.orderedTabs = new ObservableCollection(filteredTabs.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 }));
    }

    SetWindowArgs(windowArgs: any)
    {
        this.objectTableId = windowArgs.ObjectTableID;
        this.ObjectTable = window.ObjectTables.filter(x => x.Id === this.objectTableId)[0];

        this.GetTabs();
        this.LoadTabs();
    }

    GetTabs = () => this.tabs = window.ObjectTableTabs.filter(a => a.ObjectTableId == this.objectTableId) || [];
    CancelClicked = () => this.CurrentSession.CloseCurrentWindow();
    IsCopied = (a: any) => this.tabs.map(t => t.OriginalTabCode).includes(a.Code);
    PushTabs = (newTableTabs: any) => this.tabs = this.tabs.concat(newTableTabs)

    GetAllTabsAndCloseWindow()
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.loginService.CurrentTenant = SessionLocator.Tenant;
        this.loginService.GetObjectTableTabs().subscribe((tabs: any) =>
        {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();

            if(!tabs || tabs.length == 0)
                return;

            window.ObjectTableTabs = tabs;
        });
    }

    OkClicked()
    {
        if(!this.isDirty || this.tabs.length == 0)
            return this.CancelClicked();

        const tabsToUpdate = this.tabs.filter(t=>t.Changeset);

        this.CurrentSession.StartBusyIndicatorSaving();
        this.tableTabsService.UpdateTabs(tabsToUpdate)
            .subscribe(arg => {
                this.CurrentSession.StopBusyIndicator();
                this.GetAllTabsAndCloseWindow();
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
        newTab.IndexOrder = this.tabs.length;
        this.isDirty = true;
        this.tabs.push(newTab);
        this.LoadTabs();
    }

    EditTab(tab: ObjectTableTabPM)
    {

        var window = new LogitudeWindow();
        window.Width = defaultWindowWidth;
        window.Height = defaultWindowHeight;
        window.Title = editTabWindowTitle;
        window.WindowArgs = {ViewModel: this, tab: tab};
        window.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddTabComponent');

        window.WindowClosed.subscribe(data=>{
            if(data){
                this.isDirty = true;
                let editedTab = this.tabs.find(d=>d.Code == tab.Code);
                editedTab = data;
                this.LoadTabs();
            }
        });
    }
    DeleteTab(tab: ObjectTableTabPM)
    {
        tab.Changeset = 'delete';
        this.isDirty = true;
        this.LoadTabs();
    }
    DecOrder(currentTab)
    {
        if (currentTab.IndexOrder == 0)
            return;

        const prevTab = this.tabs.find(c => c.IndexOrder == currentTab.IndexOrder - 1);
        prevTab.IndexOrder = prevTab.IndexOrder + 1;
        currentTab.IndexOrder = currentTab.IndexOrder - 1;

        prevTab.Changeset = prevTab.Changeset == 'insert' ? 'insert' : 'update';
        currentTab.Changeset = currentTab.Changeset == 'insert' ? 'insert' : 'update';

        this.LoadTabs();
        this.isDirty = true;
    }
    IncOrder(currentTab)
    {
        if (currentTab.IndexOrder == this.tabs.length - 1)
            return;

        const nextTab = this.tabs.find(c => c.IndexOrder == currentTab.IndexOrder + 1);
        nextTab.IndexOrder = nextTab.IndexOrder - 1;
        currentTab.IndexOrder = currentTab.IndexOrder + 1;


        nextTab.Changeset = nextTab.Changeset == 'insert' ? 'insert' : 'update';
        currentTab.Changeset = currentTab.Changeset == 'insert' ? 'insert' : 'update';

        this.LoadTabs();
        this.isDirty = true;
    }

}
