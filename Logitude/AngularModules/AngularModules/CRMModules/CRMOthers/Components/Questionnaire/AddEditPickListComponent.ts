
import { Component, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

import { AppTool, DateTool } from '../../../../Infrastructure/Tools';

import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {CustomPickListList} from '../../../../Infrastructure/EntityLists/CustomPickListList';
import {CustomPickListPM} from '../../../../Infrastructure/EntityPMs/CustomPickListPM';
import {CustomPickListPMExtendedService} from '../../../../Infrastructure/Services/ExtendedPMs/CustomPickListPMExtendedService';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';



@Component({
    moduleId: module.id,
    templateUrl: './AddEditPickListComponent.html',
    providers: [CustomPickListPMExtendedService],
})
export class AddEditPickListComponent extends BaseComponent {
    public ItemsSource: ObservableCollection;
    public CustomPickLists: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _customPickListPMExtendedService: CustomPickListPMExtendedService) {
        super();
        this.ItemsSource = new ObservableCollection([]);

    }
    CustomPickListPMLists: CustomPickListPM[] = [];
    RemoveCustomPickListPMLists: CustomPickListPM[] = [];

    IsMultipleChoice: boolean;
    IsNewMode: boolean;
    PickListCode: string = "";


    SetWindowArgs(args: any) {
        if (args != null) {
            this.IsMultipleChoice = args.IsMultipleChoice;
            this.IsNewMode = args.IsNewMode;
            this.PickListCode = !AppTool.IsNullOrEmpty(args.PickListCode) ? args.PickListCode : "";
        }

        if (!AppTool.IsNullOrEmpty(args.PickListCode)) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.LoadData();
        }

    }



    LoadData() {
        this.CustomPickListPMLists = [];
        this._customPickListPMExtendedService.GetCustomPickListsByCode(this.PickListCode, SessionLocator.Tenant).subscribe(response => {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            this.CustomPickListPMLists = response.Result;

            this.BuildItemsSource();
      

        });
    }
    BuildItemsSource() {
        this.ItemsSource.Collection = [];

        if (this.CustomPickListPMLists) {

            var itemsCollection: CustomPickListData[] = [];

            this.CustomPickListPMLists.forEach((item) => {
                itemsCollection.push(new CustomPickListData(item));
            })
            this.ItemsSource.AppendCollection(itemsCollection);
        }
    }

    private selectedItem: CustomPickListData;
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(value: CustomPickListData) {
        if (this.selectedItem != value) {
            this.selectedItem = value;
           
        }
    }


    rowChanged(event) {
        this.SelectedItem = event;
    }



    Counter: number = 0;
   
    AddPickListClicked() {
        this.Counter += 1;
        var pickList: CustomPickListPM = new CustomPickListPM();
        pickList.Code = this.PickListCode;
        pickList.IsMultipleChoice = this.IsMultipleChoice;
        pickList.Tenant = SessionLocator.Tenant;
        pickList.Id = (this.Counter + "New").toString();
        this.CustomPickListPMLists.push(pickList);
        this.BuildItemsSource();

       

    }

    RemovePickListClicked(item: CustomPickListData) {
        if (item) {

            if (!AppTool.IsNullOrEmpty(item.EntityPM.Id)) {
                if (!this.RemoveCustomPickListPMLists) this.RemoveCustomPickListPMLists = [];
                this.RemoveCustomPickListPMLists.push(item.EntityPM);
            }

            this.CustomPickListPMLists = this.CustomPickListPMLists.filter(d => d != item.EntityPM);

            this.BuildItemsSource();
        }

    }
    ValidationErrorsList: string[];
        OkButtonClicked() {
            this.ValidationErrorsList = [];

            var customPickLists: CustomPickListData[] = this.ItemsSource.Collection;

            if (customPickLists && customPickLists.length > 0) {
                customPickLists.forEach((item) => {
                    if (item.EntityPM) {
                        if (customPickLists.filter(p => p.EntityPM.Value == item.Value && p.EntityPM.Id != item.EntityPM.Id)[0]) {
                            this.ValidationErrorsList.push("Some values are duplicated!");
                        }

                        if (AppTool.IsNullOrEmpty(item.EntityPM.Value)) {
                            this.ValidationErrorsList.push("PickList value is required");
                        } else if (item.EntityPM.Value.length > 1000) {
                            this.ValidationErrorsList.push("PickList value length should be less than 1000 character");
                        }
                    }
                });

                if (this.ValidationErrorsList.length == 0) {

                    var customPickListLists: CustomPickListData[] = this.ItemsSource.Collection.filter(d => d.EntityPM.IsDirty == true);
                    if ((customPickListLists && customPickListLists.length > 0) || (this.RemoveCustomPickListPMLists && this.RemoveCustomPickListPMLists.length > 0)) {
                        this.CurrentSession.StartBusyIndicatorSaving();

                        var customPickListPMLists: CustomPickListPM[] = [];
                        customPickListLists.forEach((item) => {
                            if (item.EntityPM) {
                                if (!AppTool.IsNullOrEmpty(item.EntityPM.Id)){
                                    if (item.EntityPM.Id.includes("New")) item.EntityPM.Id = null;
                                }
                                item.EntityPM.Code = this.PickListCode;
                                item.EntityPM.IsDirty = false;
                                customPickListPMLists.push(item.EntityPM);
                            }
                        });

                        if (this.RemoveCustomPickListPMLists && this.RemoveCustomPickListPMLists.length > 0) {
                            this.RemoveCustomPickListPMLists.forEach((item) => {
                                item.IsDirty = true;
                                customPickListPMLists.push(item);
                            });

                        }

                        this._customPickListPMExtendedService.InsertupdateCustomPickLists(customPickListPMLists).subscribe(res => {
                            this.CurrentSession.StopBusyIndicator();
                            this.CurrentSession.CurrentWindow.Close(this.PickListCode);
                          CachedDataManager.RefreshTableData("CustomPickList", true);

                        });
                    }
                    else this.CloseButtonClicked();


                }
            }
        }

        CloseButtonClicked() {
            this.CurrentSession.CloseCurrentWindow();
        }



    
}


export class CustomPickListData extends BaseComponent {
    public DataContext: CustomPickListData = this;
    EntityPM: CustomPickListPM;
    get Value() {
        var value: string = "";
        if (this.EntityPM) value = this.EntityPM.Value;
        return value;
    }
    set Value(value: string) {
    
        if (this.EntityPM != null) {
            this.EntityPM.Value = value;
        }
    }



    



    constructor(entity: CustomPickListPM) {
        super();

        this.EntityPM = entity;
    }
}

