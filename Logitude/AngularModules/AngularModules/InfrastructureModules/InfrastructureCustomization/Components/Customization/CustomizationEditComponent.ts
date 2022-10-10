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
declare var window: any;

@Component({

    templateUrl: './CustomizationEditComponent.html',
})

export class CustomizationEditComponent {

    private CurrentSession = SessionLocator.SelectedSession;
    public Title: string = "test";
    public IsCustomFieldsMenue: boolean = false;
    public IsObjectTableFilterEnabled: boolean;

    public IsShowStandardFields: boolean = false;
    public IsShowLabels: boolean = false;
    public IsShowScreensLayout: boolean = false;
    public IsShowCustomFields: boolean = false;
    public IsShowRules: boolean = false;
    public IsShowTabs: boolean = false;

    public SelectedMenu: CustomizationMainMenuItem;
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
    constructor() {
        this.MainMenuItems = new Array<CustomizationMainMenuItem>();
        this.MainMenuItems = this.GetCustomizationMainMenuItems();
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.IsMainSidebarCollapsed = false;
        this.MainMenuWidth = this.MainMenuWidthOpened;
        this.SetCustomizationFeaturesPermission();
    }
    GetCustomizationMainMenuItems(): CustomizationMainMenuItem[] {
        return new Array<CustomizationMainMenuItem>();
    }
    SetCustomizationFeaturesPermission() {
        this.IsShowStandardFields = FeatureLocator.HasFeaturePermession("General", "StandardFieldsCustomization");
        this.IsShowLabels = FeatureLocator.HasFeaturePermession("General", "LabelsCustomization");
        this.IsShowScreensLayout = FeatureLocator.HasFeaturePermession("General", "ScreenLayoutCustomization");
        this.IsShowCustomFields = FeatureLocator.HasFeaturePermession("General", "CustomFieldsCustomization");
        this.IsShowRules = FeatureLocator.HasFeaturePermession("General", "RulesCustomization");
        this.IsShowTabs = FeatureLocator.HasFeaturePermession("General", "TabsCustomization");
    }
}
export class CustomizationMainMenuItem {
    public TextCode: string;
    public HtmlView: string;
    public IndexOfOrder: number;
    public ObjectTableId: string;
    public ObjectTableName: string;
    public IconCode: string;
    public IconSource: string;
    public IconSelectedSource: string;
    public QuerySection: string;

    constructor(textCode: string, myIcon: string) {
        this.TextCode = textCode;
        this.IconCode = myIcon;
        this.IconSource = "./Images/Menu/" + myIcon + ".png";
        this.IconSelectedSource = "./Images/Menu/" + myIcon + ".Selected.png";
    }
}

