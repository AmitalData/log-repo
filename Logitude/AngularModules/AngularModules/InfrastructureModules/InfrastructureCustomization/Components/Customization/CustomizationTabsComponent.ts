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

    OrderTabs()
    {
        this.orderedTabs = new ObservableCollection(this.tabs.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 }));
    }

    SetWindowArgs(windowArgs: any)
    {
        this.objectTableId = windowArgs.ObjectTableID;
        this.ObjectTable = window.ObjectTables.filter(x => x.Id === this.objectTableId)[0];

        this.GetTenantEntityTabs();
        this.OrderTabs();
    }

    CancelClicked = () => this.CurrentSession.CloseCurrentWindow();

    private GetOriginalEntityTabs()
    {
        let newTableTabs = window.ObjectTableTabs
        .filter(a => a.ObjectTableId == this.objectTableId
            && !this.tabs.map(t=>t.OriginalTabCode).includes(a.Code)) || [];
        newTableTabs = newTableTabs.map(a => { return { ...a, Type: 'Predefined', Changeset: 'insert', Tenant: SessionLocator.Tenant  }; });
        this.SetTabsAdditionalFields(newTableTabs);
        this.tabs = this.tabs.concat(newTableTabs);


    }
    private GetTenantEntityTabs(){
        this.CurrentSession.StartBusyIndicatorSaving();
        this.tableTabsService.GetTenantTableTabsByTableId(this.objectTableId)
            .subscribe(tabs => {
                this.CurrentSession.StopBusyIndicator();

                this.tabs = tabs || [];

                this.SetScreenNames();

                this.GetOriginalEntityTabs();
                this.OrderTabs();

            }, error=>{
                alert("error happened while getting user tabs!");
                console.error(error);
            });
    }
    private SetScreenNames()
    {
        const screens: ScreenPM[] = window.Screens.filter(d => d.ObjectTableId == this.objectTableId) || [];
        for (let i = 0; i < this.tabs.length; i++) {
            let tab = this.tabs[i];
            tab.ScreenName = screens.find(a => a.Code == tab.ScreenCode)?.Name;
        }
    }

    SetTabsAdditionalFields(newTableTabs: ObjectTableTabPM[])
    {
        let maxOrder = Math.max(...this.tabs.map(t => t.IndexOrder));
        maxOrder = maxOrder == -Infinity ? -1 : maxOrder;
        const screens: ScreenPM[] = window.Screens.filter(d => d.ObjectTableId == this.objectTableId) || [];
        for (let i = 0; i < newTableTabs.length; i++) {
            let tab = newTableTabs[i];
            tab.ScreenName = screens.find(a => a.Code == tab.ScreenCode)?.Name;
            tab.OriginalTabCode = tab.Code;
            tab.IndexOrder = ++maxOrder;
        }
    }

    UpdateAllTabsAndCloseWindow()
    {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.loginService.CurrentTenant = SessionLocator.Tenant;
        this.loginService.GetObjectTableTabs().subscribe((tabs: any) =>
        {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();

            window.ObjectTableTabs = tabs;
        });
    }

    OkClicked()
    {
        if(!this.isDirty)
            return this.CancelClicked();

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
        newTab.IndexOrder = this.tabs.length;
        this.isDirty = true;
        this.tabs.push(newTab);
        this.OrderTabs();
    }

    DeleteTab(tab: ObjectTableTabPM)
    {
        tab.Changeset = 'delete';
        this.isDirty = true;
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

        this.OrderTabs();
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

        this.OrderTabs();
        this.isDirty = true;
    }

}
