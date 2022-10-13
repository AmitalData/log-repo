import { Component } from '@angular/core';
import { GeneralDomainService, FieldsTranslations } from '../../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { ViewChildren } from '@angular/core';
import { QueryList } from '@angular/core';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { ViewChild } from '@angular/core';
import { ViewContainerRef } from '@angular/core';
import { newArray } from '@angular/compiler/src/util';
import { forEach } from 'cypress/types/lodash';
import { ICustomizationService } from '../../Interface/ICustomizationService';
import { CustomFieldsService } from '../../ExternalService/CustomFieldsService';
import { StandardFieldsService } from '../../ExternalService/StandardFieldsService';
import { ScreenLayoutService } from '../../ExternalService/ScreenLayoutService';
import { TabsService } from '../../ExternalService/TabsService';
import { RulesService } from '../../ExternalService/RulesService';
import { SubEntitiesService } from '../../ExternalService/SubEntitiesService';
declare var window: any;

@Component({

    templateUrl: './CustomizationEditComponent.html',
})

export class CustomizationEditComponent {

    private CurrentSession = SessionLocator.SelectedSession;
    public Title: string = "";
    public ObjectTableId: string = null;
    public IsCustomFieldsMenue: boolean = false;
    public IsObjectTableFilterEnabled: boolean;

    public MainMenuItems: Array<CustomizationMainMenuItem>;
    public MainMenuWidth: number = 145;
    private MainMenuWidthCollapsed: number = 45;
    private MainMenuWidthOpened: number = 145;
    LayoutDirection: string = 'ltr';

    private isMainSidebarCollapsed: boolean = false;
    public get IsMainSidebarCollapsed() { return this.isMainSidebarCollapsed; }
    public set IsMainSidebarCollapsed(value: boolean) {
        if (this.isMainSidebarCollapsed != value) {
            this.isMainSidebarCollapsed = value;
            this.MainMenuWidth = value == true ? this.MainMenuWidthCollapsed : this.MainMenuWidthOpened;
        }
    }

    private customizationFeaturesList: string[] =
        [
            "CustomFieldsCustomization",
            "StandardFieldsCustomization",
            "ScreenLayoutCustomization",
            "TabsCustomization",
            "RulesCustomization",
            "SubEntitiesCustomization"
        ];

    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild("CUSTOMFIELD", { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

    public hasMainTabHighlightColor = SessionLocator.PrivateLableSettings ? (SessionLocator.PrivateLableSettings.MainTabHighlightColor == null ? false : true) : false;
    public privateLabelClass = {
        background: SessionLocator.PrivateLableSettings ? SessionLocator.PrivateLableSettings.MainTabHighlightColor : "",
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
        this.SelectedMenu = item;
    }

    private customizationService: ICustomizationService = null;

    constructor() {
        this.RunComponent();
        this.LayoutDirection = ObjectsLocator.GlobalSetting.LayoutDirection == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.IsMainSidebarCollapsed = false;
        this.MainMenuWidth = this.MainMenuWidthOpened;
    }

    SetWindowArgs(args: any) {
        this.Title = args.Title;
        this.ObjectTableId = args.ObjectTableId;
        this.IsObjectTableFilterEnabled = args.IsObjectTableFilterEnabled;
        this.IsCustomFieldsMenue = args.IsCustomFieldsMenue;
        this.BuildCustomizationMainMenuItems();
       
    }

    BuildCustomizationMainMenuItems() {
        this.MainMenuItems = this.GetCustomizationMainMenuItems();
        if (this.MainMenuItems && this.MainMenuItems.length > 0) {
            this.SelectedMenu = this.MainMenuItems[0];
        }
    }

    GetCustomizationMainMenuItems(): CustomizationMainMenuItem[] {
        var myResult: CustomizationMainMenuItem[] = [];
        this.customizationFeaturesList.forEach(item => {
            var menuItem = this.LoadCustomizationMainMenuItem(item);
            if (menuItem) myResult.push(menuItem);
        })
        return myResult;
    }

    LoadCustomizationMainMenuItem(featureName: string): CustomizationMainMenuItem {

        var customizationMainMenuItem: CustomizationMainMenuItem;

        var args: any = {};
        args.IsObjectTableFilterEnabled = this.IsObjectTableFilterEnabled;
        args.IsCustomFieldsMenue = this.IsCustomFieldsMenue;
        args.ObjectTableId = this.ObjectTableId;

        switch (featureName) {
            case "CustomFieldsCustomization": {
                this.customizationService = new CustomFieldsService();
                //additional args
                break;
            }
            case "StandardFieldsCustomization": {
                this.customizationService = new StandardFieldsService();
                //additional args
                break;
            }
            case "ScreenLayoutCustomization": {
                this.customizationService = new ScreenLayoutService();
                //additional args
                break;
            }
            case "TabsCustomization": {
                this.customizationService = new TabsService();
                //additional args
                break;
            }
            case "RulesCustomization": {
                this.customizationService = new RulesService();
                //additional args
                break;
            }
            case "SubEntitiesCustomization": {
                this.customizationService = new SubEntitiesService();
                //additional args
                break;
            }
            default: {
                break;
            }
        }

        customizationMainMenuItem = this.customizationService.LoadCustomizationMenuItem(args);
        return customizationMainMenuItem;
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

    ChangeScreen() {
        if (!this.SelectedMenu || !this.isLoaderReady) return;
        if (this.SelectedMenu.ComponentPath == null || this.SelectedMenu.Page) return;
        
        this.ShowCustomizationMenuItemComponent();
        
    }
    
    private ShowCustomizationMenuItemComponent() {
        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedMenu.Code)[0];
        SessionLocator.DynamicLoader.Load(this.SelectedMenu.ComponentPath, myLocation.viewContainerRef)
            .then(cmpRef => {
                this.SelectedMenu.Page = cmpRef.instance;
                cmpRef.instance.SetWindowArgs(this.SelectedMenu.args);
            });
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {

    }
    SaveChangesAndClose() {
        this.CurrentSession.CloseCurrentWindow();
    }

}

export class CustomizationMainMenuItem {
    public TextCode: string;
    public Code: string;
    public HtmlView: string;
    public IconCode: string;
    public IconSource: string;
    public IconSelectedSource: string;
    public QuerySection: string;
    public ComponentPath: string;
    public Page: any = null;
    public args: any = {};

    constructor(myIcon: string) {
        this.IconCode = myIcon;
        this.IconSource = "./Images/Customization/" + myIcon + ".png";
        this.IconSelectedSource = "./Images/Customization/" + myIcon + ".Selected.png";
    }
}

