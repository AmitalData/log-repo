
declare var System: any;
declare var window: any;


import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnInit, AfterViewInit, OnDestroy, ChangeDetectorRef } from '@angular/core';

import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TenantManagementPM } from '../../../../Infrastructure/EntityPMs/TenantManagementPM';

import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

import { UIProperty, UIProperties } from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';

import { FormGroup } from '@angular/forms';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { GlobalDomainService } from '../../../../Common/Services/GlobalDomainService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';

@Component({
    selector: 'CargoTrackingBrandingComponent',
    templateUrl: './CargoTrackingBrandingComponent.html',
})

export class CargoTrackingBrandingComponent extends BaseComponent implements AfterViewInit, OnInit, OnDestroy {
    public EntityPM: TenantManagementPM;
    public myForm: FormGroup;
    public DataContext: any = this;
    IsVisibile: boolean;
    public EntityId: number;
    public BackgroundId: string;
    public MobileBackgroundId: string;
    public ComapnylogoId: string;
    public InvertedLogoId: string;
    public BrowserIconId: string;
    public ShipmentHeaderImageId: string;
    private entityResourceService: EntityResourceService = new EntityResourceService();
    public BrandingTabName = "Cargo Tracking Branding";
    private CurrentSession = SessionLocator.SelectedSession;
    public IsLogitudeEnvironment: boolean = false;
    isGenerateClicked = false;
    isGenerateEnabled = true;
    private iGlobalDomainService: GlobalDomainService;
    previousPermissionBuildMonthsValue: any;
    showPermissionBuildMonths: boolean = true;    
    permissionBuildMonthsInProcess: boolean = false;
    showPermissionBuildMonthsInProcess: boolean = true;

    constructor(
        private cd: ChangeDetectorRef,
        public entityArgs: EntityArgs) {

        super();
        this.EntityPM = entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.iGlobalDomainService = new GlobalDomainService();
        this.InitializeImageIds();
        this.SetColorsFromEntity();
        this.CheckDigtialPortalAddsOnPackage();
        this.setPreviousPermissionBuildMonthsValue();
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;

    private setPreviousPermissionBuildMonthsValue() {
        this.previousPermissionBuildMonthsValue = this.PermissionBuildMonths;
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.setPreviousPermissionBuildMonthsValue();
                    this.CustomerURL = this.EntityPM.CustomerURL;
                    this.iGlobalDomainService.UpdateTenantManagementJS(this.EntityPM);
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.CustomerURL = this.EntityPM.CustomerURL;
                    this.iGlobalDomainService.UpdateTenantManagementJS(this.EntityPM);
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
       
    }

    private SetColorsFromEntity() {

        if (this.EntityPM.MainColor) {
            this.mainColorOpacity = this.GetOpacityFromRGBA(this.EntityPM.MainColor);
            this.mainColorCode = this.ConvertRGBAToHexColor(this.EntityPM.MainColor);
        }

        if (this.EntityPM.SecondaryColor) {
            this.secondaryColorOpacity = this.GetOpacityFromRGBA(this.EntityPM.SecondaryColor);
            this.secondaryColorCode = this.ConvertRGBAToHexColor(this.EntityPM.SecondaryColor);
        }

        if (this.EntityPM.TertiaryColor) {
            this.tertiaryColorOpacity = this.GetOpacityFromRGBA(this.EntityPM.TertiaryColor);
            this.tertiaryColorCode = this.ConvertRGBAToHexColor(this.EntityPM.TertiaryColor);
        }
    }

    SetBrandingTabName() {
        this.BrandingTabName = "Cargo Tracking Branding";
        this.IsLogitudeEnvironment = false;

        if (FeatureLocator.HasFeaturePermession("General", "SHLOGDIGITALPORTAL")) {
            this.BrandingTabName = TextCodeTranslator.Translate("TenantManagement.TH.LogitudeDigitalBranding");
            this.IsLogitudeEnvironment = true;
        }
    }

    CheckDigtialPortalAddsOnPackage() {
        this.BrandingTabName = "Cargo Tracking Branding";
        this.IsLogitudeEnvironment = false;

        this.iGlobalDomainService.CheckDigitalPortalAddsOn(this.EntityPM.Id).subscribe((result: any) => {
            var addOnPackage = result.Result;
            if (addOnPackage != null) {
                this.BrandingTabName = TextCodeTranslator.Translate("TenantManagement.TH.LogitudeDigitalBranding");
                this.IsLogitudeEnvironment = true;
                this.SetCustomerURLProperties(this.EntityPM.EnableBranding);
            }
        });
    }

    private InitializeImageIds() {
        this.BackgroundId = this.EntityPM.BackgroundId;
        this.MobileBackgroundId = this.EntityPM.MobileBackgroundId;
        this.ComapnylogoId = this.EntityPM.ComapnylogoId;
        this.InvertedLogoId = this.EntityPM.InvertedLogoId;
        this.BrowserIconId = this.EntityPM.BrowserIconId;
        this.ShipmentHeaderImageId = this.EntityPM.ShipmentHeaderImageId;

    }

    RemoveImage(name) {
        if (name == 'inverted') {
            this.EntityPM.InvertedLogoId = null;
            this.InvertedLogoId = null;
        }
        if (name == 'company') {
            this.EntityPM.ComapnylogoId = null;
            this.ComapnylogoId = null;
        }
        if (name == 'favicon') {
            this.EntityPM.BrowserIconId = null;
            this.BrowserIconId = null;
        }
        if (name == 'bg') {
            this.EntityPM.BackgroundId = null;
            this.BackgroundId = null;
        }
        if (name == 'mbg') {
            this.EntityPM.MobileBackgroundId = null;
            this.MobileBackgroundId = null;
        }
        if (name == 'ShipmentHeader') {
            this.EntityPM.ShipmentHeaderImageId = null;
            this.ShipmentHeaderImageId = null;
        }
    }
    ngAfterViewInit() {
        this.ListenToEntitySavedEvent();
    }

    ListenToEntitySavedEvent() {
        SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            if (isSaveSuccess) {
                this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                this.SetUIPropertiesEnabled(this.EntityPM.EnableBranding);
            }
        });
    }

    mainColorOpacity: number = 100;
    get MainColorOpacity() {
        return this.mainColorOpacity;
    }
    set MainColorOpacity(value: number) {
        this.mainColorOpacity = value;
        this.UpdateEntityMainColor();
    }

    private mainColorCode: string;
    private UpdateEntityMainColor() {
        this.EntityMainColor = this.ConvertHexToRGBColor(this.MainColorCode, this.MainColorOpacity);
    }

    public get MainColorCode(): string {
        return this.mainColorCode;
    }
    public set MainColorCode(hexColor: string) {
        this.mainColorCode = hexColor;
        this.ValidateMainColorCode(hexColor);
        this.UpdateEntityMainColor();
    }

    get PermissionBuildMonths() {
        return this.EntityPM.PermissionBuildMonths;
    }

    set PermissionBuildMonths(value: any) {
        this.setPermissionBuildMonths(value);    
    }

    private async setPermissionBuildMonths(value: any) {
        if(this.permissionBuildMonthsInProcess) return;

        if ((value > 84 || value < 0) && (value !== null && value !== '')) {
            this.permissionBuildMonthsInProcess = true;
            await this.showPopupMessage(TextCodeTranslator.Translate("TenantManagement.TH.PermissionBuildMonthsLimit"));
            this.permissionBuildMonthsInProcess = false;
            this.PermissionBuildMonths = this.previousPermissionBuildMonthsValue;
            this.showPermissionBuildMonthsInProcess = false;
            this.cd.detectChanges();
            this.showPermissionBuildMonthsInProcess = true;
            this.cd.detectChanges();
        } else {
            this.EntityPM.PermissionBuildMonths = value;
            this.cd.detectChanges();
        }
        this.permissionBuildMonthsInProcess = false;
    }


    private async showPopupMessage(message: string) {
        const win:MessageWindow = new MessageWindow();
        win.Show(message);
        await new Promise<void>(resolve => win.WindowClosed.subscribe(()=> resolve()));
    }

    private ValidateMainColorCode(hexColor: string) {
        if (!this.ValidateHexCode(hexColor, "MainColorCode"))
            this.wrongMainColor = true;
        else
            this.wrongMainColor = false;

        this.UpdateEditComponentValidationErrors();
    }

    private UpdateEditComponentValidationErrors() {
        if (this.wrongMainColor || this.wrongSecondaryColor) {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = false;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = ['Please enter valid color hex code'];
        } else {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = true;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
        }
    }

    get EntityMainColor() {
        return this.EntityPM.MainColor;
    }
    set EntityMainColor(value: string) {
        this.EntityPM.MainColor = value;

    }



    get EntitySecondaryColor() {
        return this.EntityPM.SecondaryColor;
    }
    set EntitySecondaryColor(value: string) {
        this.EntityPM.SecondaryColor = value;

    }

    secondaryColorOpacity: any = 100;
    get SecondaryColorOpacity() {
        return this.secondaryColorOpacity;
    }
    set SecondaryColorOpacity(value: number) {
        this.secondaryColorOpacity = value;



        this.SetEntitySecondaryColor();
    }

    get EntityTertiaryColor() {
        return this.EntityPM.TertiaryColor;
    }
    set EntityTertiaryColor(value: string) {
        this.EntityPM.TertiaryColor = value;
    }

    tertiaryColorOpacity: any = 100;
    get TertiaryColorOpacity() {
        return this.tertiaryColorOpacity;
    }
    set TertiaryColorOpacity(value: number) {
        this.tertiaryColorOpacity = value;
        this.SetEntityTertiaryColor();
    }

    wrongSecondaryColor: boolean = false;
    wrongMainColor: boolean = false;
    wrongTertiaryColor: boolean = false;

    private secondaryColorCode: string;
    private SetEntitySecondaryColor() {
        this.EntitySecondaryColor = this.ConvertHexToRGBColor(this.SecondaryColorCode, this.SecondaryColorOpacity);
    }

    public get SecondaryColorCode(): string {
        return this.secondaryColorCode;
    }
    public set SecondaryColorCode(hexColor: string) {
        this.secondaryColorCode = hexColor;

        this.ValidateSecondaryColor(hexColor);
        this.SetEntitySecondaryColor();
    }

    private ValidateSecondaryColor(hexColor: string) {
        if (!this.ValidateHexCode(hexColor, "SecondaryColorCode"))
            this.wrongSecondaryColor = true;

        else
            this.wrongSecondaryColor = false;

        this.UpdateEditComponentValidationErrors();
    }

    private tertiaryColorCode: string;
    private SetEntityTertiaryColor() {
        var hex = this.TertiaryColorCode;
        if (AppTool.IsNullOrEmpty(this.TertiaryColorCode))
            hex = '#ffffff';
        this.EntityTertiaryColor = this.ConvertHexToRGBColor(hex, this.TertiaryColorOpacity);
    }

    public get TertiaryColorCode(): string {
        return this.tertiaryColorCode;
    }
    public set TertiaryColorCode(hexColor: string) {
        this.tertiaryColorCode = hexColor;

        this.ValidateTertiaryColor(hexColor);
        this.SetEntityTertiaryColor();
    }

    private ValidateTertiaryColor(hexColor: string) {
        if (!this.ValidateHexCode(hexColor, "TertiaryColorCode"))
            this.wrongTertiaryColor = true;

        else
            this.wrongTertiaryColor = false;

        this.UpdateEditComponentValidationErrors();
    }

    get ContactEmail() {
        return this.EntityPM.ContactEmail;
    }
    set ContactEmail(value: string) {
        if (this.EntityPM.ContactEmail != value) {
            this.EntityPM.ContactEmail = value;

        }
    }
    get CustomerURL() {
        return this.EntityPM.CustomerURL;
    }
    set CustomerURL(value: string) {
        if (this.EntityPM.CustomerURL != value) {
            this.EntityPM.CustomerURL = value;
            this.SetCustomerURLProperties(this.EntityPM.EnableBranding);
        }
    }

    get ActivatePrivateSite() {
        return this.EntityPM.ActivatePrivateSite;
    }
    set ActivatePrivateSite(value: boolean) {
        if (this.EntityPM.ActivatePrivateSite != value) {
            this.EntityPM.ActivatePrivateSite = value;

        }
    }

    get ActivatedforDeclarationApprove() {
        return this.EntityPM.ActivatedforDeclarationApprove;
    }
    set ActivatedforDeclarationApprove(value: boolean) {
        if (this.EntityPM.ActivatedforDeclarationApprove != value) {
            this.EntityPM.ActivatedforDeclarationApprove = value;

        }
    }
    get ShowMoneyOrder() {
        return this.EntityPM.ShowMoneyOrder;
    }
    set ShowMoneyOrder(value: boolean) {
        if (this.EntityPM.ShowMoneyOrder != value) {
            this.EntityPM.ShowMoneyOrder = value;

        }
    }
    get CargoTrackingPublicShowEvents() {
        return this.EntityPM.CargoTrackingPublicShowEvents;
    }
    set CargoTrackingPublicShowEvents(value: boolean) {
        if (this.EntityPM.CargoTrackingPublicShowEvents != value) {
            this.EntityPM.CargoTrackingPublicShowEvents = value;

        }
    }
    get CargoTrackingPrivateShowEvents() {
        return this.EntityPM.CargoTrackingPrivateShowEvents;
    }
    set CargoTrackingPrivateShowEvents(value: boolean) {
        if (this.EntityPM.CargoTrackingPrivateShowEvents != value) {
            this.EntityPM.CargoTrackingPrivateShowEvents = value;

        }
    }
    get DeclarationMessage() {
        return this.EntityPM.DeclarationMessage;
    }
    set DeclarationMessage(value: string) {
        if (this.EntityPM.DeclarationMessage != value) {
            this.EntityPM.DeclarationMessage = value;

        }
    }

    public get EnableBranding() {
        return this.EntityPM.EnableBranding;
    }
    public set EnableBranding(value: boolean) {
        if (this.EntityPM.EnableBranding != value) {
            this.EntityPM.EnableBranding = value;
            this.EnableBrandingChange(value);
        }
    }

    public get EnableExportToExcel() {
        return this.EntityPM.EnableExportToExcel;
    }
    public set EnableExportToExcel(value: boolean) {
        if (this.EntityPM.EnableExportToExcel != value) {
            this.EntityPM.EnableExportToExcel = value;
        }
    }
    get SearchAbsoluteValuePublic() {
        return this.EntityPM.SearchAbsoluteValuePublic;
    }
    set SearchAbsoluteValuePublic(value: boolean) {
        if (this.EntityPM.SearchAbsoluteValuePublic != value) {
            this.EntityPM.SearchAbsoluteValuePublic = value;

        }
    }
    ValidateHexCode(value: string, fieldName: string) {

        const regex = new RegExp('^#([a-fA-F0-9]{6})$');
        var valid: boolean = regex.test(value);
        if ((!valid || value.length > 9 || value.length < 7) && value != null) {
            this.UIProperties.SetValidity(fieldName, "TenantManagement", false, "this is not a valid hex code");
            return false;
        }
        else {
            this.UIProperties.SetValidity(fieldName, "TenantManagement", true, null);
            return true;
        }
    }


    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe((response: any) => {
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) {

                this.SetUIPropertiesEnabled(this.EntityPM.EnableBranding);

            }
        });



    }

    BackgroundImageUploadedCompleted(code) {
        this.BackgroundId = code;
        this.EntityPM.BackgroundId = code;
    }
    MobileBackgroundImageUploadedCompleted(code) {
        this.MobileBackgroundId = code;
        this.EntityPM.MobileBackgroundId = code;
    }
    AreBackgroundImageDimensionsValid(value) {
        if (value) {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = true;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
        } else {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = false;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = ['Invalid Image Dimensions, the Valid Dimension are 1920 X 1080'];
        }
    }
    AreMobileBackgroundImageDimensionsValid(value) {
        if (value) {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = true;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
        } else {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = false;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = ['Invalid Image Dimensions, the Valid Dimension are 360 X 640 or 414 X 896'];
        }
    }
    ComapnylogoUploadedCompleted(code) {
        this.ComapnylogoId = code;
        this.EntityPM.ComapnylogoId = code;
    }
    InvertedLogoUploadedCompleted(code) {
        this.InvertedLogoId = code;
        this.EntityPM.InvertedLogoId = code;
    }
    BrowserIconUploadedCompleted(code) {
        this.BrowserIconId = code;
        this.EntityPM.BrowserIconId = code;
    }
    ShipmentHeaderImageUploadedCompleted(code) {
        this.ShipmentHeaderImageId = code;
        this.EntityPM.ShipmentHeaderImageId = code;
    }

    EnableBrandingChange(value: any) {

        this.EntityPM.UpdateByUserId = SessionInfo.LoggedUserId + "^" + SessionInfo.LoggedUserTenant.toString();
        this.EnableBranding = value;
        this.SetUIPropertiesEnabled(value);

    }

    EnableExportToExcelChange(value: any) {

        this.EntityPM.UpdateByUserId = SessionInfo.LoggedUserId + "^" + SessionInfo.LoggedUserTenant.toString();
        this.EnableExportToExcel = value;

    }

    ActivatedforDeclarationApproveChange(value: any) {
        this.EntityPM.ActivatedforDeclarationApprove = value;
    }
    ShowMoneyOrderChange(value: any) {
        this.EntityPM.ShowMoneyOrder = value;
    }
    CargoTrackingPublicShowEventsChange(value: any) {
        this.EntityPM.CargoTrackingPublicShowEvents = value;
    }
    CargoTrackingPrivateShowEventsChange(value: any) {
        this.EntityPM.CargoTrackingPrivateShowEvents = value;
    }
    SetUIPropertiesEnabled(value: boolean) {

        this.UIProperties.SetEnabled("MainColor", "TenantManagement", value);
        this.UIProperties.SetEnabled("SecondaryColor", "TenantManagement", value);
        this.UIProperties.SetEnabled("ContactEmail", "TenantManagement", value);
        this.UIProperties.SetEnabled("HideSharedlogistics", "TenantManagement", value);
        this.SetCustomerURLProperties(value);
    }

    SetCustomerURLProperties(isBranding: boolean) {
        this.UIProperties.SetEnabled("CustomerURL", "TenantManagement", isBranding);
        if (this.IsLogitudeEnvironment) {
            this.UIProperties.SetRequired("CustomerURL", "TenantManagement", isBranding && AppTool.IsNullOrEmpty(this.CustomerURL));
            this.UIProperties.SetEnabled("CustomerURL", "TenantManagement", false);
            this.isGenerateEnabled = AppTool.IsNullOrEmpty(this.CustomerURL) ? true : false;
        }
    }

    ConvertHexToRGBColor(hex: string, alpha: number) {
        if (hex && hex.length >= 7) {
            const r = parseInt(hex.slice(1, 3), 16);
            const g = parseInt(hex.slice(3, 5), 16);
            const b = parseInt(hex.slice(5, 7), 16);

            if (alpha) {
                return `rgba(${r}, ${g}, ${b}, ${alpha / 100})`;
            } else {
                return `rgb(${r}, ${g}, ${b})`;
            }
        } else {
            return 'rgba(0,0,0,1)'; 
        }
    }

    RGBToHex(r, g, b) {
        r = r.toString(16);
        g = g.toString(16);
        b = b.toString(16);

        if (r.length == 1)
            r = "0" + r;
        if (g.length == 1)
            g = "0" + g;
        if (b.length == 1)
            b = "0" + b;

        return "#" + r + g + b;
    }
    ConvertRGBAToHexColor(rgba: string) {
        var numbers = rgba.replace('rgba(', '').replace(')', '').replace(' ', '');
        var splittedNumbers = numbers.split(',');
        var red = parseInt(splittedNumbers[0].trim());
        var green = parseInt(splittedNumbers[1].trim());
        var blue = parseInt(splittedNumbers[2].trim());
        var opacity = parseInt(splittedNumbers[3].trim());

        var r = red.toString(16);
        var g = green.toString(16);
        var b = blue.toString(16);

        if (r.length == 1)
            r = "0" + r;
        if (g.length == 1)
            g = "0" + g;
        if (b.length == 1)
            b = "0" + b;

        return "#" + r + g + b;
    }
    GetOpacityFromRGBA(rgba: string) {
        var numbers = rgba.replace('rgba(', '').replace(')', '').replace(' ', '');
        var splittedNumbers = numbers.split(',');
        var opacity = parseFloat(splittedNumbers[3].trim());
        return opacity * 100;
    }

    openLoginPolicyPopup() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Login Policy";
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 250;

        var args = this.EntityPM;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/CargoLoginPolicy/CargoLoginPolicyComponent');
    }

    onGenerateClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Generate Sub domain";
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        var args = this.EntityPM;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/SubDomainGenerateComponent');
        logitudeWindow.WindowClosed.subscribe(s => {
            if (s) {
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
        });
    }
}






