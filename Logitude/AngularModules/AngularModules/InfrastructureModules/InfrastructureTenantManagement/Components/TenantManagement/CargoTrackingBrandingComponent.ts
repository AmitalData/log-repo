
declare var System: any;
declare var window: any;


import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnInit, AfterViewInit } from '@angular/core';

import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TenantManagementPM } from '../../../../Infrastructure/EntityPMs/TenantManagementPM';

import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';

import { UIProperty, UIProperties } from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';

import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

@Component({


    selector: 'CargoTrackingBrandingComponent',
    templateUrl: './CargoTrackingBrandingComponent.html',


})

export class CargoTrackingBrandingComponent extends BaseComponent implements AfterViewInit, OnInit
{
    public EntityPM: TenantManagementPM;
    public myForm: FormGroup;
    public DataContext: any = this;
    IsVisibile: boolean;
    public EntityId: number;
    public BackgroundId: string;
    public ComapnylogoId: string;
    public InvertedLogoId: string;
    public BrowserIconId: string;
    public ShipmentHeaderImageId: string;
    private entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(public entityArgs: EntityArgs)
    {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.InitializeImageIds();

        this.SetColorsFromEntity();
    }

    private SetColorsFromEntity()
    {
        if (this.EntityPM.MainColor) {
            this.mainColorOpacity = this.GetOpacityFromRGBA(this.EntityPM.MainColor);
            this.mainColorCode = this.ConvertRGBAToHexColor(this.EntityPM.MainColor);
        }
        if (this.EntityPM.SecondaryColor) {
            this.secondaryColorOpacity = this.GetOpacityFromRGBA(this.EntityPM.SecondaryColor);
            this.secondaryColorCode = this.ConvertRGBAToHexColor(this.EntityPM.SecondaryColor);
        }
    }

    private InitializeImageIds()
    {
        this.BackgroundId = this.EntityPM.BackgroundId;
        this.ComapnylogoId = this.EntityPM.ComapnylogoId;
        this.InvertedLogoId = this.EntityPM.InvertedLogoId;
        this.BrowserIconId = this.EntityPM.BrowserIconId;
        this.ShipmentHeaderImageId = this.EntityPM.ShipmentHeaderImageId;
        
    }

    RemoveImage(name){
        if(name=='inverted'){
            this.EntityPM.InvertedLogoId=null;
            this.InvertedLogoId=null;
        }
        if(name=='company'){
            this.EntityPM.ComapnylogoId=null;
            this.ComapnylogoId=null;
        }
        if(name=='favicon'){
            this.EntityPM.BrowserIconId=null;
            this.BrowserIconId=null;
        }
        if(name=='bg'){
            this.EntityPM.BackgroundId=null;
            this.BackgroundId=null;
        }
        if (name == 'ShipmentHeader') {
            this.EntityPM.ShipmentHeaderImageId = null;
            this.ShipmentHeaderImageId = null;
        }
    }
    ngAfterViewInit()
    {
        this.ListenToEntitySavedEvent();
    }

    ListenToEntitySavedEvent()
    {
        SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) =>
        {
            if (isSaveSuccess) {
                this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                this.SetUIPropertiesEnabled(this.EntityPM.EnableBranding);
            }
        });
    }

    mainColorOpacity: number = 100;
    get MainColorOpacity()
    {
        return this.mainColorOpacity;
    }
    set MainColorOpacity(value: number)
    {
        this.mainColorOpacity = value;
        this.UpdateEntityMainColor();
    }

    private mainColorCode: string;
    private UpdateEntityMainColor()
    {
        this.EntityMainColor = this.ConvertHexToRGBColor(this.MainColorCode, this.MainColorOpacity);
    }

    public get MainColorCode(): string
    {
        return this.mainColorCode;
    }
    public set MainColorCode(hexColor: string)
    {
        this.mainColorCode = hexColor;
        this.ValidateMainColorCode(hexColor);
        this.UpdateEntityMainColor();
    }

    private ValidateMainColorCode(hexColor: string)
    {
        if (!this.ValidateHexCode(hexColor, "MainColorCode"))
            this.wrongMainColor = true;
        else
            this.wrongMainColor = false;

        this.UpdateEditComponentValidationErrors();
    }

    private UpdateEditComponentValidationErrors()
    {
        if (this.wrongMainColor || this.wrongSecondaryColor) {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = false;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = ['Please enter valid color hex code'];
        } else {
            SessionLocator.SelectedSession.CurrentEditComponent.IsEditValid = true;
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
        }
    }

    get EntityMainColor()
    {
        return this.EntityPM.MainColor;
    }
    set EntityMainColor(value: string)
    {
        this.EntityPM.MainColor = value;

    }



    get EntitySecondaryColor()
    {
        return this.EntityPM.SecondaryColor;
    }
    set EntitySecondaryColor(value: string)
    {
        this.EntityPM.SecondaryColor = value;

    }

    secondaryColorOpacity: any = 100;
    get SecondaryColorOpacity()
    {
        return this.secondaryColorOpacity;
    }
    set SecondaryColorOpacity(value: number)
    {
        this.secondaryColorOpacity = value;



        this.SetEntitySecondaryColor();
    }

    wrongSecondaryColor: boolean = false;
    wrongMainColor: boolean = false;
    private secondaryColorCode: string;
    private SetEntitySecondaryColor()
    {
        this.EntitySecondaryColor = this.ConvertHexToRGBColor(this.SecondaryColorCode, this.SecondaryColorOpacity);
    }

    public get SecondaryColorCode(): string
    {
        return this.secondaryColorCode;
    }
    public set SecondaryColorCode(hexColor: string)
    {
        this.secondaryColorCode = hexColor;

        this.ValidateSecondaryColor(hexColor);
        this.SetEntitySecondaryColor();
    }




    private ValidateSecondaryColor(hexColor: string)
    {
        if (!this.ValidateHexCode(hexColor, "SecondaryColorCode"))
            this.wrongSecondaryColor = true;

        else
            this.wrongSecondaryColor = false;

        this.UpdateEditComponentValidationErrors();
    }

    get ContactEmail()
    {
        return this.EntityPM.ContactEmail;
    }
    set ContactEmail(value: string)
    {
        if (this.EntityPM.ContactEmail != value) {
            this.EntityPM.ContactEmail = value;

        }
    }
    get CustomerURL()
    {
        return this.EntityPM.CustomerURL;
    }
    set CustomerURL(value: string)
    {
        if (this.EntityPM.CustomerURL != value) {
            this.EntityPM.CustomerURL = value;

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

    get DeclarationMessage() {
        return this.EntityPM.DeclarationMessage;
    }
    set DeclarationMessage(value: string) {
        if (this.EntityPM.DeclarationMessage != value) {
            this.EntityPM.DeclarationMessage = value;

        }
    }

    public get EnableBranding()
    {
        return this.EntityPM.EnableBranding;
    }
    public set EnableBranding(value: boolean)
    {
        if (this.EntityPM.EnableBranding != value) {
            this.EntityPM.EnableBranding = value;
            this.EnableBrandingChange(value);
        }
    }

    ValidateHexCode(value: string, fieldName: string)
    {

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


    ngOnInit()
    {
        this.entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe((response: any) =>
        {
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) {

                this.SetUIPropertiesEnabled(this.EntityPM.EnableBranding);

            }
        });



    }

    BackgroundImageUploadedCompleted(code)
    {
        this.BackgroundId = code;
        this.EntityPM.BackgroundId = code;
    }
    ComapnylogoUploadedCompleted(code)
    {
        this.ComapnylogoId = code;
        this.EntityPM.ComapnylogoId = code;
    }
    InvertedLogoUploadedCompleted(code)
    {
        this.InvertedLogoId = code;
        this.EntityPM.InvertedLogoId = code;
    }
    BrowserIconUploadedCompleted(code)
    {
        this.BrowserIconId = code;
        this.EntityPM.BrowserIconId = code;
    }
    ShipmentHeaderImageUploadedCompleted(code) {
        this.ShipmentHeaderImageId = code;
        this.EntityPM.ShipmentHeaderImageId = code;
    }

    EnableBrandingChange(value: any)
    {

        this.EntityPM.UpdateByUserId = SessionInfo.LoggedUserId + "^" + SessionInfo.LoggedUserTenant.toString();
        this.EnableBranding = value;
        this.SetUIPropertiesEnabled(value);

    }

    ActivatedforDeclarationApproveChange(value: any) {
        this.EntityPM.ActivatedforDeclarationApprove = value;
    }

    SetUIPropertiesEnabled(value: boolean)
    {

        this.UIProperties.SetEnabled("MainColor", "TenantManagement", value);
        this.UIProperties.SetEnabled("SecondaryColor", "TenantManagement", value);
        this.UIProperties.SetEnabled("CustomerURL", "TenantManagement", value);
        this.UIProperties.SetEnabled("ContactEmail", "TenantManagement", value);
        this.UIProperties.SetEnabled("HideSharedlogistics", "TenantManagement", value);
    }



    ConvertHexToRGBColor(hex: string, alpha: number)
    {
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

    RGBToHex(r, g, b)
    {
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
    ConvertRGBAToHexColor(rgba: string)
    {
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
    GetOpacityFromRGBA(rgba: string)
    {
        var numbers = rgba.replace('rgba(', '').replace(')', '').replace(' ', '');
        var splittedNumbers = numbers.split(',');
        var opacity = parseFloat(splittedNumbers[3].trim());
        return opacity * 100;
    }

    openLoginPolicyPopup(){
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Login Policy";
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;

        var args = this.EntityPM;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Show('CargoLoginPolicy/CargoLoginPolicy.component.ts');
    }


}






