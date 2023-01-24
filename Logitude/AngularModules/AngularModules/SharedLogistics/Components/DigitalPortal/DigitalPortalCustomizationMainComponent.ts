import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { ViewChildren } from '@angular/core';
import { QueryList } from '@angular/core';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { ViewChild } from '@angular/core';
import { ViewContainerRef } from '@angular/core';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({

    templateUrl: './DigitalPortalCustomizationMainComponent.html',
})

export class DigitalPortalCustomizationMainComponent {

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

    public IsDirty: boolean = false;
    public NewSelectedMenu: CustomizationMainMenuItem;

    constructor() {
        this.RunComponent();
        this.LayoutDirection = ObjectsLocator.GlobalSetting.LayoutDirection == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
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
        }
        else {
            this.IsSaveButtonVisible = true;
        }

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
