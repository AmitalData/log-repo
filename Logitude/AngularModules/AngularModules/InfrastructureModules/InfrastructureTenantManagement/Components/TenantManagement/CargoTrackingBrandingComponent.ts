
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
    updateMainColor(event:  any) {
    this.MainColor =  event.value;
    
    }
    updateSecondaryColor(event: any) {
        this.SecondaryColor = event.value;

    }
    ngAfterViewInit() {
      
         this.Listen();
    }
    colorpicker: any;
    secondarycolor: any;
    Listen() {
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
        this.sliderValue = value;
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
    get Opacity() {
        return this.EntityPM.Opacity;
    }
    set Opacity(value: number) {
        if (this.EntityPM.Opacity != value) {
         
            this.EntityPM.Opacity = value;
          
        }
    }
    get MainColor() {
        return this.EntityPM.MainColor;
    }
    set MainColor(value: string) {
        if (this.EntityPM.MainColor != value) {
            this.ValidateHexCode(value, "MainColor");
            this.EntityPM.MainColor = value;
            this.colorpicker.value = value;
        }
    }
    get SecondaryColor() {
        return this.EntityPM.SecondaryColor;
    }
    set SecondaryColor(value: string) {
        if (this.EntityPM.SecondaryColor != value) {
            this.ValidateHexCode(value, "SecondaryColor");
            this.EntityPM.SecondaryColor = value;
            this.secondarycolor.value = value;
        }

    }
    ValidateHexCode(value:string, fieldName:string) {

        var valid: boolean = /^#[0-9A-F]{6}$/i.test(value);
        if (!valid && value != null) {
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
        var color;
        if (field == "Main") {
            color = this.colorpicker.value;
        }
        else color = this.secondarycolor.value;
        var rgbaColor = 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + value + ')';
        if (field == "Main") {
            this.colorPickerValue = rgbaColor;
        }
        else { this.secondarycolorPickerValue = rgbaColor; }
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

        this.SetUIPropertiesEnabled(value);

    }

    SetUIPropertiesEnabled(value: boolean) {

        this.EntityPM.UIProperties.SetEnabled("CustomerURL", "TenantManagement", value);
        this.EntityPM.UIProperties.SetEnabled("ContactEmail", "TenantManagement", value);
        this.EntityPM.UIProperties.SetEnabled("HideSharedlogistics", "TenantManagement", value);
    }






}






