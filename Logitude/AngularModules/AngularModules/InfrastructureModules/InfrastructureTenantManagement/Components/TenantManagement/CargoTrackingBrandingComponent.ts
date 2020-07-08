
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

export class CargoTrackingBrandingComponent extends BaseComponent implements AfterViewInit, OnInit{
    public EntityPM: TenantManagementPM;
    public myForm: FormGroup;
    public DataContext:any= this;
    IsVisibile: boolean;
    public EntityId: number;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor( public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.ImageId = this.EntityPM.BackgroundId;
    }
    clickColor(color: any) {
         console.log('working.....'); 
    }
    calculateOpacity: boolean = false;
    updateMainColor(event: any) {
        if (this.EnableBranding) {
            this.MainColor = event.value;

            this.SliderValue = 0;
        }
    }
    updateSecondaryColor(event: any) {
        if (this.EnableBranding) {
            this.SecondaryColor = event.value;
            this.SecondarySliderValue = 0;
        }
    }
    ngAfterViewInit() {
      
         this.Listen();
    }
    colorpicker: any;
    secondarycolor: any;
    private SaveCompletedEvent: any = null;
    private CurrentSession = SessionLocator.SelectedSession;
    Listen() {
         if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                       
                    }
                });
        
        
        }
        this.colorpicker = document.getElementById("color");
        this.secondarycolor = document.getElementById("secondarycolor");

        if (this.MainColor != null) {

          this.colorpicker.value = this.MainColor;
        }
        if (this.SecondaryColor != null) {

            this.secondarycolor.value = this.SecondaryColor;
        }
        this.secondarycolor.addEventListener("change", () => this.updateSecondaryColor(this.secondarycolor), false);
   
    this.colorpicker.addEventListener("change", () => this.updateMainColor(this.colorpicker), false);
       // colorWell.select();
    }
    sliderValue: number = 0;
    get SliderValue() {
        return this.sliderValue;
    }
    set SliderValue(value: number) {
        this.sliderValue = value; this.calculateOpacity = true;
        this.CalculateOpacity(value, "Main");

    }

    secondarySliderValue: number = 0;
    get SecondarySliderValue() {
        return this.secondarySliderValue;
    }
    set SecondarySliderValue(value: number) {
        this.secondarySliderValue = value;
        this.CalculateOpacity(value, "Secondary");

    }
    
   
    get MainColor() {
        return (this.EntityPM.MainColor != null && this.EntityPM.MainColor.length>7) ?  "#"+this.EntityPM.MainColor.substring(3,9):this.EntityPM.MainColor;
    }
    set MainColor(value: string) {
        if (this.EntityPM.MainColor != value) {
            
            this.ValidateHexCode(value, "MainColor");
            this.EntityPM.MainColor = value;
            this.colorpicker.value = value;
        }
    }
    get MainColorOpacity() {
        return this.EntityPM.MainColorOpacity;
    }
    set MainColorOpacity(value: string) {
        if (this.EntityPM.MainColorOpacity != value) {          
            this.EntityPM.MainColorOpacity = value;
            
        }
    }
    get SecondaryColorOpacity() {
        return this.EntityPM.SecondaryColorOpacity;
    }
    set SecondaryColorOpacity(value: string) {
        if (this.EntityPM.SecondaryColorOpacity != value) {
            this.EntityPM.SecondaryColorOpacity = value;

        }
    }
   public get EnableBranding() {
        return this.EntityPM.EnableBranding;
    }
  public  set EnableBranding(value: boolean) {
        if (this.EntityPM.EnableBranding != value) {
            this.EntityPM.EnableBranding = value;
            this.EnableBrandingChange(value);
        }
    }
    get SecondaryColor() {
        return (this.EntityPM.SecondaryColor != null && this.EntityPM.SecondaryColor.length > 7) ? "#" + this.EntityPM.SecondaryColor.substring(3, 9) : this.EntityPM.SecondaryColor;
    }
    set SecondaryColor(value: string) {
        if (this.EntityPM.SecondaryColor != value) {
            this.ValidateHexCode(value, "SecondaryColor");
            this.EntityPM.SecondaryColor = value;
            this.secondarycolor.value = value;
        }

    }
    ValidateHexCode(value:string, fieldName:string) {

        var valid: boolean = /^#[0-9a-fA-F]*/i.test(value);
        if ((!valid || value.length>9) && value != null) {
            this.UIProperties.SetValidity(fieldName, "TenantManagement", false, "this is not a valid hex code");

            return false;
        }
        else {
            this.UIProperties.SetValidity(fieldName, "TenantManagement", true, null);

            return true;
        }
    }
 
    public colorPickerValue: string;
    public secondarycolorPickerValue: string;
    CalculateOpacity(value: number, field: string) {
        if (this.EnableBranding) {
            var color;
            if (field == "Main") {
                color = this.colorpicker.value;
                // value = ;
                this.MainColorOpacity = Math.round(value * 255).toString(16);
            }
            else {
                // value = Math.round(value * 255);
                this.SecondaryColorOpacity = Math.round(value * 255).toString(16);
                color = this.secondarycolor.value;
            }
            var rgbaColor = 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + value + ')';

            if (field == "Main") {
                document.documentElement.style.setProperty('--sliderBackground', rgbaColor);

            }
            else { document.documentElement.style.setProperty('--sliderBackground2', rgbaColor); }
        }
    }
    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe((response: any) => {
            this.IsVisibile = true;
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM) {

               this.SetUIPropertiesEnabled(this.EntityPM.EnableBranding);

            }
        });



    }
    public ImageId: string;
    ImageUploadedCompleted(code) {
        this.ImageId = code;
        this.EntityPM.BackgroundId = code;
    }

    EnableBrandingChange(value: any) {

        this.EntityPM.UpdateByUserId = SessionInfo.LoggedUserId + "^" + SessionInfo.LoggedUserTenant.toString();
        this.EnableBranding = value;
        this.SetUIPropertiesEnabled(value);

    }

    SetUIPropertiesEnabled(value: boolean) {
      
        this.UIProperties.SetEnabled("MainColor", "TenantManagement", value);
        this.UIProperties.SetEnabled("SecondaryColor", "TenantManagement", value);

        this.UIProperties.SetEnabled("CustomerURL", "TenantManagement", value);
        this.UIProperties.SetEnabled("ContactEmail", "TenantManagement", value);
        this.UIProperties.SetEnabled("HideSharedlogistics", "TenantManagement", value);
    }






}






