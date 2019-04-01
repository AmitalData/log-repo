declare var window: any;
import {Component, ViewContainerRef, OnInit, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {TextCodeTranslationPipe} from '../../../../../Controls/Pipes/TextCodeTranslationPipe';
import {LogitudeListBoxComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent';
import {Http} from '@angular/http';
import {SessionInfo} from '../../../../../Infrastructure/Utilities/SessionInfo';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceArgs} from '../../../../../Infrastructure/DataContracts/ServiceArgs';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {AppTool} from '../../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    selector: 'ObjectFieldsSearch',
    templateUrl: './ObjectFieldsSearchComponent.html',
})

export class ObjectFieldsSearchComponent {

    @Output() onSelectedDataLoadedEvent = new EventEmitter();
    @Output() onUnSelectedDataLoadedEvent = new EventEmitter();
    @Output() onDataSourceChangedEvent = new EventEmitter();
    @Output() onUnselectedDataSourceChangedEvent = new EventEmitter();
    DataSource: any[];
    ObjectTableId: string;
    unselectedObjectFields: any[];
    unSelectedList: any[];
    unselected: any[];
    Fixedunselected: any[];
    ObjectTable: any;
    SearchFieldsId: string;
    public serviceArgs: ServiceArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        this.serviceArgs = new ServiceArgs();
        this.serviceArgs.http = ServiceHelper.Http;;
        if (this.CurrentSession == null) {
            this.SearchFieldsId = "ObjectFieldSearchFields_-1_-1";
        }

        else {
            this.SearchFieldsId = "ObjectFieldSearchFields_" + this.CurrentSession.GetNewId("ObjectFieldSearchFields");
        }
        //this.Run();
    }
    SetWindowArgs(args: any) {
        this.ObjectTableId = args.ObjectTableId;
        this.ObjectTable = window.ObjectTables.filter(d => d.Name == args.currentObjectTable)[0];
        this.Run();
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    }

    FillPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "Search";
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    }

    Run() {
        this.unselectedObjectFields = window.ObjectFields.filter(f => f.ObjectTableId == this.ObjectTableId && !AppTool.IsNullOrEmpty(f.PMPropertyPath) && !f.DisplayOnly);
        this.unselected = this.unselectedObjectFields;
        this.unSelectedList = this.unselected.sort((a, b) => { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1 });

        this.CD.detectChanges();
        //SelectedQueryColumnsList.ItemsSource = OrderedQueryColumnsList;
        this.Fixedunselected = this.unSelectedList;

        this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);

    }

    //txtSearch_TextChanged
    private searchText: string;
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        if (newValue != null && newValue != "") {
            this.unSelectedList = this.Fixedunselected.filter(f => TextCodeTranslator.Translate(f.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1);
        }
        else {
            this.unSelectedList = this.Fixedunselected;
        }
        this.onUnselectedDataSourceChangedEvent.emit(this.unSelectedList);
        //this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);

    }
    private selectedItem: any;
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(newValue: any) {
        this.selectedItem = newValue;
    }

    private fieldSelectedItem: any;
    public get FieldSelectedItem() { return this.fieldSelectedItem; }
    public set FieldSelectedItem(newValue: any) {
        this.fieldSelectedItem = newValue;
    }





    onSelectedItemChanged(item) {
        this.SelectedItem = item;

        this.onUnSelectedDataLoadedEvent.emit(null);
    }

    onFieldSelectedItemChanged(item) {
        this.FieldSelectedItem = item;


    }







    SaveChanges() {
        this.CurrentSession.CurrentWindow.Close(this.FieldSelectedItem.Id);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
