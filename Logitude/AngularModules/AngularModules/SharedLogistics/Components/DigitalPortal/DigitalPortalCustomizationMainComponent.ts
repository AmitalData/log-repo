import { Component, OnInit} from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ViewChildren } from '@angular/core';
import { QueryList } from '@angular/core';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { ViewChild } from '@angular/core';
import { ViewContainerRef } from '@angular/core';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { DigitalTextService } from '../../../Infrastructure/Services/WebServices/DigitalTextService'

@Component({

    templateUrl: './DigitalPortalCustomizationMainComponent.html',
})

export class DigitalPortalCustomizationMainComponent implements OnInit {

    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild("CUSTOMFIELD", { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

    public CurrentSession = SessionLocator.SelectedSession;
    public Title: string = "";
    public ObjectTableId: string = null;
    public IsCustomFieldsMenue: boolean = false;
    public IsObjectTableFilterEnabled: boolean;
    public IsSaveButtonVisible: boolean = true;
    public MainMenuItems: Array<CustomizationMainMenuItem>;
    public MainMenuWidth: number = 145;
    LayoutDirection: string = 'ltr';
    private digitalTextService: DigitalTextService;
    public IsDirty: boolean = false;
    public NewSelectedMenu: CustomizationMainMenuItem;
    public IsObjectTablesFilterVisible: boolean = false;

    constructor() {
        this.DigitalProfileFilterList = [];
        this.ObjectTablesFilterList = [];
        this.digitalTextService = new DigitalTextService();
        this.RunComponent();
        this.LayoutDirection = ObjectsLocator.GlobalSetting.LayoutDirection == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.BuildCustomizationMainMenuItems();
    }

    ngOnInit() {
        this.FillDigitalProfiles();
    }
    private FillDigitalProfiles() {
        this.digitalTextService.GetDigitalProfileName(SessionLocator.Tenant).subscribe((myResult) => {
            if (!myResult.HasError) {
                this.DigitalProfileFilterList = [];
                var profiles = myResult.Result;
                
                profiles.forEach(item => {
                    this.DigitalProfileFilterList.push(new CodeNameClass(item.Id, item.Name, item.Code));
                });

                this.selectedProfileItem = this.DigitalProfileFilterList[0];
                if (this.SelectedMenu.Page) {
                    this.SelectedMenu.Page.ProfileId = this.selectedProfileItem.Code;
                    this.SelectedMenu.Page.ProfileCode = this.selectedProfileItem.LocalName;
                }

                if (this.SelectedMenu.Code == "ScreenLayout" && this.SelectedMenu.Page) {
                    this.SelectedMenu.Page.GetDefaultScreens();
                }
                else {
                    this.FillObjectTables();
                }
            }
        });
    }

    private FillObjectTables() {
        this.digitalTextService.GetDigitalProfilesObjetTables().subscribe((myResult) => {
            if (!myResult.HasError) {
                this.ObjectTablesFilterList = [];
                var objectTables = myResult.Result;
                if (this.SelectedMenu.Code != "ChageLabels") {
                    objectTables = objectTables.filter(a => a.ObjectTableName != "General");
                }
                objectTables.forEach(item => {
                    this.ObjectTablesFilterList.push(new CodeNameClass(item.ObjectTableName, item.ObjectTableId));
                });

                this.selectedObjectTableItem = this.ObjectTablesFilterList[0];
                if (this.SelectedMenu.Page) {
                    this.SelectedMenu.Page.ObjectTableId = this.selectedObjectTableItem.Name;
                    this.SelectedMenu.Page.BuildItemsSource();
                }
            }
        });
    }

    BuildCustomizationMainMenuItems() {
        this.MainMenuItems = this.GetCustomizationMainMenuItems();
        if (this.MainMenuItems && this.MainMenuItems.length > 0) {
            this.SelectedMenu = this.MainMenuItems[0];
        }
    }

    GetCustomizationMainMenuItems(): CustomizationMainMenuItem[] {
        var myResult: CustomizationMainMenuItem[] = [];

        var args = new CustomizationMainMenuItem();
        args.TextCode = "Labels Management";
        args.Code = "ChageLabels";
        args.ComponentPath = "./SharedLogistics/Components/DigitalPortal/DigitalPortalCustomizationChageLabelsComponent";
        myResult.push(args);

        //args = new CustomizationMainMenuItem();
        //args.TextCode = "Translate Labels";
        //args.Code = "TranslateLabels";
        //args.ComponentPath = "./SharedLogistics/Components/DigitalPortal/DigitalPortalCustomizationTranslateLabelsComponent";
        //myResult.push(args);

        args = new CustomizationMainMenuItem();
        args.TextCode = "Fields Management";
        args.Code = "ShowHideFields";
        args.ComponentPath = "./SharedLogistics/Components/DigitalPortal/DigitalPortalCustomizationShowHideFieldsComponent";
        myResult.push(args);

        args = new CustomizationMainMenuItem();
        args.TextCode = "Screen Layout";
        args.Code = "ScreenLayout";
        args.ComponentPath = "./SharedLogistics/Components/DigitalPortal/DigitalPortalCustomizationScreenLayoutComponent";
        myResult.push(args);

        args = new CustomizationMainMenuItem();
        args.TextCode = "Sub Objects";
        args.Code = "SubObjects";
        args.ComponentPath = "./SharedLogistics/Components/DigitalPortal/DigitalPortalCustomizationSubObjectsComponent";
        myResult.push(args);

        return myResult;
    }

    private isLoaderReady: boolean = false;
    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isLoaderReady = true;
                this.ChangeScreen();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private selectedMenu: CustomizationMainMenuItem;
    public get SelectedMenu() {
        return this.selectedMenu;
    }
    public set SelectedMenu(value: any) {
        this.selectedMenu = value;
        this.ChangeScreen();
    }
    public SelectionChanged(item: any) {
        if (this.SelectedMenu == item) return;
        if (this.IsDirty) {
            this.NewSelectedMenu = item;
            this.OpenConfirmWindow();
            return;
        }
        this.SelectedMenu = item;
    }

    ChangeScreen() {

        if (this.SelectedMenu.Code == "ScreenLayout") {
            this.IsSaveButtonVisible = false;
            this.IsObjectTablesFilterVisible = false;
        }
        else {
            this.IsSaveButtonVisible = true;
            this.IsObjectTablesFilterVisible = true;
        }

       
        this.FillDigitalProfileFiltersList();

        if (!this.SelectedMenu || !this.isLoaderReady) return;

        if (this.SelectedMenu.ComponentPath == null || (this.SelectedMenu.Page && !this.SelectedMenu.Page.IsChange)) return;

        this.ShowCustomizationMenuItemComponent();
    }

    private ShowCustomizationMenuItemComponent() {
        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedMenu.Code)[0];

        if (myLocation != null && this.SelectedMenu.Page && this.SelectedMenu.Page.IsChange) {
            myLocation.viewContainerRef.clear();
        }

        SessionLocator.DynamicLoader.Load(this.SelectedMenu.ComponentPath, myLocation.viewContainerRef)
            .then(cmpRef => {
                this.SelectedMenu.Page = cmpRef.instance;
                cmpRef.instance.customizationEditComponent = this;
                cmpRef.instance.SetWindowArgs(this.SelectedMenu.screenArgs);
            });
    }

    OpenConfirmWindow() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 450;
        confirmWindow.Height = 190;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.NoButtonText = "Don't Save";
        confirmWindow.YesButtonText = "Save ";
        confirmWindow.CancelButtonText = "Cancel";
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
        confirmWindow.Show("This Screen has unsaved changes. Do you want to save it?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SelectedMenu.Page.Save();
                return;
            }
            if (confirmWindow.No) {
                this.SelectedMenu.Page.Cancel();
                return;
            }
        });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public IsSaveAndClose: boolean = false;

    OkButtonClicked() {
        this.NewSelectedMenu = null;
        this.IsSaveAndClose = false;
        this.SelectedMenu.Page.Save();
    }

    SaveChangesAndClose() {
        this.NewSelectedMenu = null;
        this.IsSaveAndClose = true;
        this.SelectedMenu.Page.Save();
    }

    public ObjectTablesFilterList: CodeNameClass[];
    private selectedObjectTableItem: CodeNameClass;
    get SelectedObjectTableItem() { return this.selectedObjectTableItem; }
    set SelectedObjectTableItem(value: CodeNameClass) {
        if (this.selectedObjectTableItem != value) {
            this.selectedObjectTableItem = value;
            this.SelectedMenu.Page.ObjectTableId = value.Name;
            this.SelectedMenu.Page.BuildItemsSource();
        }
    }

    private FillObjectTablesFiltersList() {
        if (this.SelectedMenu.Code != "ChageLabels") {
            this.ObjectTablesFilterList = this.ObjectTablesFilterList.filter(a => a.Code != "General");
            this.selectedObjectTableItem = this.ObjectTablesFilterList[0];
        }

        if (this.SelectedMenu.Page) {
            this.SelectedMenu.Page.ObjectTableId = this.selectedObjectTableItem.Name;
            this.SelectedMenu.Page.BuildItemsSource();
        }
    }

    public DigitalProfileFilterList: CodeNameClass[];
    private selectedProfileItem: CodeNameClass;
    get SelectedProfileItem() { return this.selectedProfileItem; }
    set SelectedProfileItem(value: CodeNameClass) {
        if (this.selectedProfileItem != value) {
            this.selectedProfileItem = value;
            this.SelectedMenu.Page.ProfileId = value.Code;
            this.SelectedMenu.Page.ProfileCode = value.LocalName;

            if (this.SelectedMenu.Code == "ScreenLayout") {
                this.SelectedMenu.Page.GetDefaultScreens();
            }
            else {
                this.SelectedMenu.Page.BuildItemsSource();
            }
        }
    }

    private FillDigitalProfileFiltersList() {

        if (this.SelectedMenu.Code != "ChageLabels") {
            this.DigitalProfileFilterList = this.DigitalProfileFilterList.filter(a => a.LocalName != "CM");
            this.selectedProfileItem = this.DigitalProfileFilterList[0];
        }
        if (this.SelectedMenu.Page) {
            this.SelectedMenu.Page.ProfileId = this.selectedProfileItem.Code;
            this.SelectedMenu.Page.ProfileCode = this.selectedProfileItem.LocalName;
        }

        if (this.SelectedMenu.Code == "ScreenLayout" && this.SelectedMenu.Page) {
            this.SelectedMenu.Page.GetDefaultScreens();
        }
        else {
            this.FillObjectTablesFiltersList();
        }

    }
}

export class CustomizationMainMenuItem {
    public TextCode: string;
    public Code: string;
    public HtmlView: string;
    public QuerySection: string;
    public ComponentPath: string;
    public Page: any = null;
    public screenArgs: any = {};
    public IsVisible: boolean;

    constructor() {

    }
}
