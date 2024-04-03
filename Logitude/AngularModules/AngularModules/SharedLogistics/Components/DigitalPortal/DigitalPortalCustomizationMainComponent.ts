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
import { DigitalLanguageSettingsService } from 'Infrastructure/Services/WebServices/DigitalLanguageSettingsService';

@Component({
    templateUrl: './DigitalPortalCustomizationMainComponent.html',
    providers: [DigitalLanguageSettingsService]
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
    ObjectTablesFilterFullList: { id: number, name: string }[] = [];
    DigitalProfileFilterFullList: { id: number, name: string, code: string }[] = [];
    ObjectTablesFilterList: { id: number, name: string }[] = [];
    DigitalProfileFilterList: { id: number, name: string, code: string }[] = [];
    DigitalDisplayLanguageslList: DisplayLanguageItem[] = [];

    constructor(public _digitalLanguageSettingsService: DigitalLanguageSettingsService) {
        this.Initialize();
        this.RunComponent();
        this.LayoutDirection = ObjectsLocator.GlobalSetting.LayoutDirection == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.BuildCustomizationMainMenuItems();
    }

    Initialize() {
        this.DigitalProfileFilterFullList = [];
        this.ObjectTablesFilterFullList = [];
        this.DigitalProfileFilterList = [];
        this.ObjectTablesFilterList = [];
        this.digitalTextService = new DigitalTextService();
    }

    ngOnInit() {
        this.FillDigitalProfiles();
        this.FillDisplayLanguages();
    }
    private FillDigitalProfiles() {
        this.digitalTextService.GetDigitalProfileName(SessionLocator.Tenant).subscribe((myResult) => {
            if (!myResult.HasError) {
                this.DigitalProfileFilterFullList = [];
                var profiles = myResult.Result;
                
                profiles.forEach(item => {
                    this.DigitalProfileFilterFullList.push({ "id": item.Id, "name": item.Name, "code": item.Code });
                });

                this.DigitalProfileFilterList = this.DigitalProfileFilterFullList;
                this.selectedProfileItem = this.DigitalProfileFilterFullList[0];
                if (this.SelectedMenu.Page) {
                    this.SelectedMenu.Page.ProfileId = this.selectedProfileItem.id;
                    this.SelectedMenu.Page.ProfileCode = this.selectedProfileItem.code;
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
                this.ObjectTablesFilterFullList = [];
                var objectTables = myResult.Result;
                if (this.SelectedMenu.Code != "ChageLabels") {
                    objectTables = objectTables.filter(a => a.ObjectTableName != "General");
                }
                
                objectTables.forEach(item => {
                    this.ObjectTablesFilterFullList.push({ "id": item.ObjectTableId, "name": item.ObjectTableName});
                });

                this.ObjectTablesFilterList = this.ObjectTablesFilterFullList;
                this.selectedObjectTableItem = this.ObjectTablesFilterFullList[0];
                if (this.SelectedMenu.Page) {
                    this.SelectedMenu.Page.ObjectTableId = this.selectedObjectTableItem.id;
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
        this.selectedMenu.LanguageCode = this.selectedDisplayLanguage ? this.selectedDisplayLanguage.code : 'EN';
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

        if (this.SelectedMenu.Code == "ScreenLayout") 
        {
            this.IsSaveButtonVisible = false;
            this.IsObjectTablesFilterVisible = false;
        }
        else {
            this.IsSaveButtonVisible = true;
            this.IsObjectTablesFilterVisible = true;
        }

        if (!this.SelectedMenu || !this.isLoaderReady) return;

        this.FillDigitalProfileFiltersList();

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
                this.FillDigitalProfileFiltersList();
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

   
    private selectedObjectTableItem: any;
    get SelectedObjectTableItem() { return this.selectedObjectTableItem; }
    set SelectedObjectTableItem(value) {
        if (this.selectedObjectTableItem != value) {
            this.selectedObjectTableItem = value;
            this.SelectedMenu.Page.ObjectTableId = value.id;
            this.SelectedMenu.LanguageCode = this.selectedDisplayLanguage ? this.selectedDisplayLanguage.code : 'EN';
            this.SelectedMenu.Page.BuildItemsSource();
        }
    }

    private FillObjectTablesFiltersList() {
        this.ObjectTablesFilterList = this.ObjectTablesFilterFullList;
        if (this.SelectedMenu.Code != "ChageLabels") {
            this.ObjectTablesFilterList = this.ObjectTablesFilterFullList.filter(a => a.name != "General");
        }

        this.selectedObjectTableItem = this.ObjectTablesFilterList[0];

        if (this.SelectedMenu.Page) {
            this.SelectedMenu.Page.ObjectTableId = this.selectedObjectTableItem.id;
            this.SelectedMenu.Page.BuildItemsSource();
        }
    }

   
    private selectedProfileItem: any;
    get SelectedProfileItem() { return this.selectedProfileItem; }
    set SelectedProfileItem(value) {
        if (this.selectedProfileItem != value) {
            this.selectedProfileItem = value;
            this.SelectedMenu.Page.ProfileId = value.id;
            this.SelectedMenu.Page.ProfileCode = value.code;
            this.SelectedMenu.LanguageCode = this.selectedDisplayLanguage ? this.selectedDisplayLanguage.code : 'EN';

            if (this.SelectedMenu.Code == "ScreenLayout") {
                this.SelectedMenu.Page.GetDefaultScreens();
            }
            else {
                this.SelectedMenu.Page.BuildItemsSource();
            }
        }
    }

    private FillDigitalProfileFiltersList() {
        this.DigitalProfileFilterList = this.DigitalProfileFilterFullList;
        if (this.SelectedMenu.Code != "ChageLabels") {
            this.DigitalProfileFilterList = this.DigitalProfileFilterFullList.filter(a => a.code != "CM");
        }

        this.selectedProfileItem = this.DigitalProfileFilterList[0];

        if (this.SelectedMenu.Page) {
            this.SelectedMenu.Page.ProfileId = this.selectedProfileItem.id;
            this.SelectedMenu.Page.ProfileCode = this.selectedProfileItem.code;
        }

        if (this.SelectedMenu.Code == "ScreenLayout" && this.SelectedMenu.Page) {
            this.SelectedMenu.Page.GetDefaultScreens();
        }
        else {
            this.FillObjectTablesFiltersList();
        }

    }

    private selectedDisplayLanguage: DisplayLanguageItem;
    get SelectedDisplayLanguage() { return this.selectedDisplayLanguage; }
    set SelectedDisplayLanguage(value) {
        if (this.selectedDisplayLanguage != value) {
            this.selectedDisplayLanguage = value;
            this.SelectedMenu.LanguageCode = value ? value.code : 'EN';
            if (this.SelectedMenu.Code != "ScreenLayout") {
                this.SelectedMenu.Page.BuildItemsSource();
            }
        }
    }

    private FillDisplayLanguages() {
        this._digitalLanguageSettingsService.GetDigitalLanguages().subscribe((myResult) => {
            if (!myResult.HasError) {
                this.DigitalDisplayLanguageslList = [];
                var languagesResult = myResult && myResult.Result ? myResult.Result : [];

                languagesResult.forEach(item => {
                    this.DigitalDisplayLanguageslList.push(
                        {
                            "name": item.Name,
                            "code": item.Code,
                            "displayText": item.DisplayText
                        }
                    );
                });


                this.selectedDisplayLanguage = this.DigitalDisplayLanguageslList[0];
            }
        });
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
    public LanguageCode: string;

    constructor() {

    }
}

export class DisplayLanguageItem {
    public name: string;
    public code: string;
    public displayText: string;

    constructor() {

    }
}
