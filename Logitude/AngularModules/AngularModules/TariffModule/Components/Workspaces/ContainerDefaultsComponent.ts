import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { ClassLevelValidator } from '../../../Infrastructure/Validators/ClassLevelValidator'
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TariffSettingPM } from '../../../TariffModule/EntityPMs/TariffSettingPM';
import { PackageTypeList } from '../../../Common/EntityLists/PackageTypeList';
import { PackageTypeListService } from '../../../Common/Services/StandardLists/PackageTypeListService';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'ContainerDefaultsComponent',
    
    templateUrl: './ContainerDefaultsComponent.html',
})

export class ContainerDefaultsComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public EntityPM: TariffSettingPM;
    private myPackageTypeListService: PackageTypeListService;
    constructor() {
        super();
        this.myPackageTypeListService = new PackageTypeListService();
    }
    
    SetWindowArgs(args) {
        this.EntityPM = args;

        this.LoadContainers();        
        this.Clone();
    }
    
    private allPackageTypes: PackageTypeList[];
    private LoadContainers() {
        this.myPackageTypeListService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.allPackageTypes = myResponse.Result;
                this.FillContainersIds();
            }
        });
    }

    private FillContainersIds() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ContainerDefaults)) {
            var containersArray: string[] = this.EntityPM.ContainerDefaults.split(',');

            if (containersArray.length > 0) {
                var index: number = 1;

                containersArray.forEach(item => {
                    var packageType: PackageTypeList = this.allPackageTypes.filter(d => d.Code == item.trim())[0];
                    if (packageType != null) {
                        this['ContainerType' + index  + 'Id'] = packageType.Id;
                    }

                    index++;
                });                
            }
        }
    }

    private containerType1Id: string;
    get ContainerType1Id() {
        return this.containerType1Id;
    }
    set ContainerType1Id(value: string) {
        if (this.containerType1Id != value) {
            this.containerType1Id = value;
            this.GetContainerTypeCode(value, 1);
        }
    }

    private containerType2Id: string;
    get ContainerType2Id() {
        return this.containerType2Id;
    }
    set ContainerType2Id(value: string) {
        if (this.containerType2Id != value) {
            this.containerType2Id = value;
            this.GetContainerTypeCode(value, 2);
        }
    }

    private containerType3Id: string;
    get ContainerType3Id() {
        return this.containerType3Id;
    }
    set ContainerType3Id(value: string) {
        if (this.containerType3Id != value) {
            this.containerType3Id = value;
            this.GetContainerTypeCode(value, 3);
        }
    }

    private containerType4Id: string;
    get ContainerType4Id() {
        return this.containerType4Id;
    }
    set ContainerType4Id(value: string) {
        if (this.containerType4Id != value) {
            this.containerType4Id = value;
            this.GetContainerTypeCode(value, 4);
        }
    }

    private containerType5Id: string;
    get ContainerType5Id() {
        return this.containerType5Id;
    }
    set ContainerType5Id(value: string) {
        if (this.containerType5Id != value) {
            this.containerType5Id = value;
            this.GetContainerTypeCode(value, 5);
        }
    }

    private containerType1Code: string;
    private containerType2Code: string;
    private containerType3Code: string;
    private containerType4Code: string;
    private containerType5Code: string;
    private GetContainerTypeCode(id: string, index: number) {
        if (AppTool.IsNullOrEmpty(id)) {
            this['containerType' + index + 'Code'] = null;
        }

        else {
            var containerType: PackageTypeList = this.allPackageTypes.filter(d => d.Id == id)[0];
            if (containerType != null) {
                this['containerType' + index + 'Code'] = containerType.Code;
            }
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.ContainerType1Id) && AppTool.IsNullOrEmpty(this.ContainerType2Id) && AppTool.IsNullOrEmpty(this.ContainerType3Id)
            && AppTool.IsNullOrEmpty(this.ContainerType4Id) && AppTool.IsNullOrEmpty(this.ContainerType5Id)) {
            this.ValidationErrorsList.push("Please select at least one container type");
        }

        this.ValidateContainerTypes();

        if (this.ValidationErrorsList.length == 0) {
            var containers: string = this.containerType1Code;

            if (!AppTool.IsNullOrEmpty(this.containerType2Code)) {
                containers = containers + ", " + this.containerType2Code;
            }

            if (!AppTool.IsNullOrEmpty(this.containerType3Code)) {
                containers = containers + ", " + this.containerType3Code;
            }

            if (!AppTool.IsNullOrEmpty(this.containerType4Code)) {
                containers = containers + ", " + this.containerType4Code;
            }

            if (!AppTool.IsNullOrEmpty(this.containerType5Code)) {
                containers = containers + ", " + this.containerType5Code;
            }

            this.EntityPM.ContainerDefaults = containers;
            this.CurrentSession.CloseCurrentWindowEmit('OK');
        }
    }

    ValidateContainerTypes() {
        var ContainerTypeIdsProperties: string[] = [];
        var ContainerTypeNamesProperties: string[] = [];
        var EmptyIndex = 1;
        var FirstLineEmpty: boolean = false;
        var emptyLines: boolean = false;
        var tempErrors: Array<string> = [];

        for (var firstIndex = 1; firstIndex <= 5; firstIndex++) {
            ContainerTypeIdsProperties.push("ContainerType" + firstIndex + "Id");
            ContainerTypeNamesProperties.push("Container Type " + firstIndex);
            for (var secondIndex = firstIndex + 1; secondIndex <= 5; secondIndex++) {
                var comparedContainer = "ContainerType" + firstIndex + "Id";
                var targetContainer = "ContainerType" + secondIndex + "Id";

                if (this[comparedContainer] != null && this[comparedContainer] == this[targetContainer]) {
                    var packageTypeList = this.allPackageTypes.filter(d => d.Id == this[targetContainer + ""])[0];
                    if (packageTypeList) {
                        this.ValidationErrorsList.push("Container Type " + packageTypeList.EnglishName + " is duplicated");
                    }                    
                }
            }

            if (firstIndex == 1) {
                if (AppTool.IsNullOrEmpty(this[ContainerTypeIdsProperties[firstIndex - 1]])) {                    
                    FirstLineEmpty = true;
                }
            }

            else {
                if (AppTool.IsNullOrEmpty(this[ContainerTypeIdsProperties[firstIndex - 1]])) {
                    if (EmptyIndex == 1) {
                        EmptyIndex = firstIndex;
                    }
                }

                if (firstIndex == 2) {
                    if (!AppTool.IsNullOrEmpty(this[ContainerTypeIdsProperties[firstIndex - 1]])) {
                        if (FirstLineEmpty) {
                            emptyLines = true;
                            this.ValidationErrorsList.push("Empty Container Lines aren't allowed between line 1 and line 2");
                            EmptyIndex = 1;
                        }
                    }
                }

                if (firstIndex >= 3) {
                    if (!AppTool.IsNullOrEmpty(this[ContainerTypeIdsProperties[firstIndex - 1]])) {
                        if (EmptyIndex != 1) {
                            emptyLines = true;
                            this.ValidationErrorsList.push("Empty Container Lines aren't allowed between line " + (EmptyIndex - 1) + " and line " + firstIndex);
                            EmptyIndex = 1;
                        }
                    }
                }
            }
        }

        if (!emptyLines) {
            tempErrors.forEach(error => {
                this.ValidationErrorsList.push(error);
            });
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('ContainerDefaults');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
