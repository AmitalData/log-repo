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
import { CustomizationEditComponent } from './CustomizationEditComponent';

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
const defaultWindowHeight = 230;
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
    //private isDirty: boolean = false;
    GridDisabled: boolean = false;
    tabs: ObjectTableTabPM[] = [];
    orderedTabs = new ObservableCollection([]);

    public customizationEditComponent: CustomizationEditComponent;

    constructor(private loginService: LoginService)
    {
        super();
    }

    LoadTabs()
    {
        const filteredTabs = this.tabs.filter(t=>t.Changeset != 'delete');
        this.orderedTabs = new ObservableCollection(filteredTabs.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 }));
        this.ReSetOrderTabs();
    }


    private ReSetOrderTabs() {
         let indexOrder = 0;
         this.orderedTabs.Collection.forEach((tab) => {
             this.SetTabIndexOrder(tab, indexOrder++);
        });
    }


    SetTabIndexOrder(tab: ObjectTableTabPM, indexOrder: number) {
        if (tab.IndexOrder == indexOrder) return;
        tab.IndexOrder = indexOrder;
        this.SetChangeSet(tab);
        this.customizationEditComponent.IsDirty = true;
        //this.isDirty = true;

    }

    SetWindowArgs(windowArgs: any)
    {
        this.objectTableId = windowArgs.ObjectTableId;
        this.ObjectTable = window.ObjectTables.filter(x => x.Id === this.objectTableId)[0];

        this.LoadScreen();
    }

    GetTabs = () => this.tabs = window.ObjectTableTabs.filter(a => a.ObjectTableId == this.objectTableId) || [];
    IsCopied = (a: any) => this.tabs.map(t => t.OriginalTabCode).includes(a.Code);
    PushTabs = (newTableTabs: any) => this.tabs = this.tabs.concat(newTableTabs)

    private LoadScreen() {
        this.GetTabs();
        this.LoadTabs();
    }

    GetAllTabs(isRunBusyIndicator: boolean = true)
    {
        if (isRunBusyIndicator) this.CurrentSession.StartBusyIndicatorLoading();
       
        this.loginService.CurrentTenant = SessionLocator.Tenant;
        this.loginService.GetObjectTableTabs().subscribe((tabs: any) =>
        {
            this.CurrentSession.StopBusyIndicator();
            //if(!tabs || tabs.length == 0)
            //    return;

            window.ObjectTableTabs = tabs;
            this.LoadScreen();
            this.customizationEditComponent.IsDirty = false;
            if (this.customizationEditComponent.IsSaveAndClose) {
                this.customizationEditComponent.CurrentSession.CloseCurrentWindow();
                this.customizationEditComponent.IsSaveAndClose = false;
            }
            if (this.customizationEditComponent.NewSelectedMenu) {
                this.customizationEditComponent.SelectedMenu = this.customizationEditComponent.NewSelectedMenu;
            }
        });
        
    }

    OkClicked()
    {
        if (!this.customizationEditComponent.IsDirty || this.tabs.length == 0)
            return this.GetAllTabs(false);

        const tabsToUpdate = this.tabs.filter(t => t.Changeset);

        this.CurrentSession.StartBusyIndicatorSaving();
        this.tableTabsService.UpdateTabs(tabsToUpdate)
            .subscribe(arg => {
                this.CurrentSession.StopBusyIndicator();
                this.GetAllTabs();
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
        window.WindowArgs = { CustomizationMainComponent: this, IsNew:true};
        window.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddTabComponent');

        window.WindowClosed.subscribe(data=>{
            if(data){
                this.AddTab(data.entity);
            }
        });

    }

    AddTab(newTab: ObjectTableTabPM){
        newTab.IndexOrder = this.tabs.length;
        this.customizationEditComponent.IsDirty = true;
        //this.isDirty = true;
        this.tabs.push(newTab);
        this.LoadTabs();
    }

    EditTab(tab: ObjectTableTabPM)
    {

        var window = new LogitudeWindow();
        window.Width = defaultWindowWidth;
        window.Height = defaultWindowHeight;
        window.Title = editTabWindowTitle;
        window.WindowArgs = { CustomizationMainComponent: this, tab: tab};
        window.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddTabComponent');

        window.WindowClosed.subscribe(data=>{
            if (data) {
                this.customizationEditComponent.IsDirty = true;
                //this.isDirty = true;
                let editedTab = this.tabs.find(d=>d.Code == tab.Code);
                editedTab = data;
                this.LoadTabs();
            }
        });
    }


    DeleteTab(tab: ObjectTableTabPM)
    {
        tab.Changeset = 'delete';
        this.customizationEditComponent.IsDirty = true;
        //this.isDirty = true;
        this.LoadTabs();
    }


    DecOrder(currentTab: ObjectTableTabPM)
    {
        if (currentTab.IndexOrder == 0)
            return;

        const prevTab: ObjectTableTabPM = this.tabs.find(c => c.IndexOrder == currentTab.IndexOrder - 1);

        this.SetTabIndexOrder(prevTab, (prevTab.IndexOrder + 1));
        this.SetTabIndexOrder(currentTab, (currentTab.IndexOrder - 1));
        this.LoadTabs();
    }

    IncOrder(currentTab)
    {
        if (currentTab.IndexOrder == this.tabs.length - 1)
            return;

        const nextTab = this.tabs.find(c => c.IndexOrder == currentTab.IndexOrder + 1);
        this.SetTabIndexOrder(nextTab, (nextTab.IndexOrder - 1));
        this.SetTabIndexOrder(currentTab, (currentTab.IndexOrder + 1));
        this.LoadTabs();
       
    }


    public  SetChangeSet(tab) {
        tab.Changeset = tab.Changeset == 'insert' ? 'insert' : 'update';
        
    }

    Save() {
        if (this.customizationEditComponent.IsDirty) {
            this.OkClicked();
        }
        else if (this.customizationEditComponent.IsSaveAndClose) {
                this.customizationEditComponent.CurrentSession.CloseCurrentWindow();
                this.customizationEditComponent.IsSaveAndClose = false;
            }
    }
    Cancel() {
        this.GetAllTabs(false);     
    }
}
