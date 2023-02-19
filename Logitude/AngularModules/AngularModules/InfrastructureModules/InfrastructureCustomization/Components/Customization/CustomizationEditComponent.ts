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
import { extend, forEach } from 'cypress/types/lodash';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { CustomizationMainMenuItem } from './CustomizationMenuItems/CustomizationMainMenuItem';
import { CustomFieldsMainMenuItem } from './CustomizationMenuItems/CustomFieldsMainMenuItem';
import { StandardFieldsMainMenuItem } from './CustomizationMenuItems/StandardFieldsMainMenuItem';
import { ScreenLayoutMainMenuItem } from './CustomizationMenuItems/ScreenLayoutMainMenuItem';
import { TabsMainMenuItem } from './CustomizationMenuItems/TabsMainMenuItem';
import { RulesMainMenuItem } from './CustomizationMenuItems/RulesMainMenuItem';
import { SubEntitiesMainMenuItem } from './CustomizationMenuItems/SubEntitiesMainMenuItem';
import { QueriesMainMenuItem } from './CustomizationMenuItems/QueriesMainMenuItem';
declare var window: any;

@Component({

    templateUrl: './CustomizationEditComponent.html',
})

export class CustomizationEditComponent {

    public CurrentSession = SessionLocator.SelectedSession;
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

    public IsDirty: boolean = false;
    public NewSelectedMenu: CustomizationMainMenuItem;
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

    public IsSubEntity: boolean = false;

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
        this.IsSubEntity = args.IsSubEntity;
        this.BuildCustomizationMainMenuItems();
       
    }

    BuildCustomizationMainMenuItems() {
        this.MainMenuItems = this.GetCustomizationMainMenuItems().filter(item => item.IsVisible == true);
        if (this.MainMenuItems && this.MainMenuItems.length > 0) {
            this.SelectedMenu = this.MainMenuItems[0];
        }
    }

    GetCustomizationMainMenuItems(): CustomizationMainMenuItem[] {
        var myResult: CustomizationMainMenuItem[] = [];
        var args: any = {};
        args.IsObjectTableFilterEnabled = this.IsObjectTableFilterEnabled;
        args.IsCustomFieldsMenue = this.IsCustomFieldsMenue;
        args.ObjectTableId = this.ObjectTableId;
        args.IsSubEntity = this.IsSubEntity;

        myResult.push(new CustomFieldsMainMenuItem(args));
        myResult.push(new StandardFieldsMainMenuItem(args));
        myResult.push(new ScreenLayoutMainMenuItem(args));
        myResult.push(new TabsMainMenuItem(args));
        myResult.push(new RulesMainMenuItem(args));
        myResult.push(new SubEntitiesMainMenuItem(args));
        myResult.push(new QueriesMainMenuItem(args));

        return myResult;
    }

    filterMainMenuItems() {
        return this.MainMenuItems.filter(i => i.IsVisible)
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

}


