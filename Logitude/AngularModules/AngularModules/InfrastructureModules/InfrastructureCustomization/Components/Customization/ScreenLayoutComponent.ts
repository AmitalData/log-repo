import {Component} from '@angular/core';
import {GeneralDomainService} from '../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {ScreenPM} from '../../../../Infrastructure/EntityPMs/ScreenPM';
import {ScreenFieldPM} from '../../../../Infrastructure/EntityPMs/ScreenFieldPM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ScreenLayoutArgs} from '../../../../Infrastructure/DataContracts/ScreenLayoutArgs';
import {LoginService} from '../../../../Infrastructure/Services/LoginService';
import {Headers} from '@angular/http';


declare var window;
@Component({
    moduleId: module.id,
    templateUrl: './ScreenLayoutComponent.html',
})

export class ScreenLayoutComponent extends BaseComponent {
    public DataContext: ScreenLayoutComponent = this;
    public MyArgs: ScreenLayoutArgs = new ScreenLayoutArgs();
    private ObjecttableId: string;
    private myService: EntityResourceService;
    public TableScreensCollection: Array<ScreenItem> = [];
    public ObjectTableName: string;
    //public TabsList: ObservableCollection;
    //public CountText: number = 0;
    public ValidationErrorsList: Array<String> = [];
    public banckStackFields: Array<ObjectFieldPM> = [];
    public AllbanckStackFields: Array<ObjectFieldPM> = [];
    public currentScreenFields: Array<any> = [];
    public currentObjectFields: Array<ObjectFieldPM> = [];
    public ScreenRows: Array<ScreenRowDetails> = [];
    private myGeneralService: GeneralDomainService;
    loginService: LoginService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.myService = new EntityResourceService();
        this.myGeneralService = new GeneralDomainService();
        this.loginService = new LoginService(); 
        //this.TabsList = new ObservableCollection([]);            
    }

    //private searchText: string;
    //public get SearchText() { return this.searchText; }
    //public set SearchText(value: string) {
    //    if (this.searchText != value) {
    //        this.searchText = value;
    //        this.TabsList.Clear();
    //        if (value != null)
    //            this.TabsList.InsertCollection(this.TranslationList.filter(f => (f.Code.toLowerCase().indexOf(value.toLowerCase()) > -1) || f.DefaultText.toLowerCase().indexOf(value.toLowerCase()) > -1 || f.TranslatedText.toLowerCase().indexOf(value.toLowerCase()) > -1));
    //        else
    //            this.TabsList.InsertCollection(this.TranslationList);
    //        if (this.TabsList.Collection != null)
    //            this.CountText = this.TabsList.Collection.length;
    //        else
    //            this.CountText = 0;
    //    }
    //}


    //public SelectedRow: any = null;
    //OnRowSelected(itemComponent: any) {
    //    this.SelectedRow = itemComponent;
    //}

    public SelectedItem: ScreenItem;
    SelectionChanged(Item) {
        //this.OkClicked(false);
        this.SelectedItem = Item;
        this.FillbanckStackFields();
        this.myGeneralService.GetScreenModificationByScreenId(Item.ScreenPM.Id).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (myResponse.Result != null) {
                this.GenerateScreen(myResponse.Result);
            }
            else {
                this.GenerateScreen(Item.ScreenPM);
            } 
        });
        
    }

    SetWindowArgs(windowArgs: any) {
        this.ObjecttableId = windowArgs.ObjectTableID;
        this.FillTableScreensCollection();
    }

    GenerateScreen(Item: any) {
        this.ScreenRows = [];
        for (var i = 0; i < Item.NumberOfColumns; i++) {
            var RDetails = new ScreenRowDetails();
            RDetails.ColumnIndex = i;
            var myFields = this.currentScreenFields.filter(a => a.Column == i).sort((a, b) => { return a.Row - b.Row });
            myFields.forEach(field => {
                if (RDetails.ScreenFieldPMs == null) {
                    RDetails.ScreenFieldPMs = [];
                }
                if (RDetails.ObjectFieldPMs == null) {
                    RDetails.ObjectFieldPMs = [];
                }
                RDetails.ScreenFieldPMs.push(field);
                var myOField = window.ObjectFields.filter(a => a.FieldCode == field.ObjectFieldCode)[0];
                RDetails.ObjectFieldPMs.push(myOField);

            });
            this.ScreenRows.push(RDetails);
        }
    }

    FillTableScreensCollection() {
        this.TableScreensCollection = [];
        var table = window.ObjectTables.filter(d => d.Id == this.ObjecttableId)[0];
        var Screens: ScreenPM[] = window.Screens.filter(d => d.ObjectTableId == this.ObjecttableId);
        if (Screens.length > 0) {
            Screens.forEach(screen => {
                var name = screen.Name;
                if (table.Name == "Shipment") {
                    if (screen.Code.indexOf("Master") > -1) {
                        name += " (Master)";
                    }
                }
                var item = new ScreenItem();
                item.ScreenPM = screen;
                item.Name = name;
                this.TableScreensCollection.push(item);
            });

            this.SelectionChanged(this.TableScreensCollection[0]);
        }
    }

    FillbanckStackFields() {
        this.banckStackFields = [];
        this.AllbanckStackFields = [];
        var table = window.ObjectTables.filter(d => d.Id == this.ObjecttableId)[0];
        this.currentObjectFields = window.ObjectFields.filter(d => d.ObjectTableId == this.ObjecttableId && !d.IsCustomFilter && d.PMPropertyPath != null && d.DataTypeCode != null && !d.IsMulti);//.Where(o => !d.IsCustomFilter && d.PMPropertyPath != null).OrderBy(f => f.FullNameTextCodeDefaultText).ToList();
        this.currentObjectFields = this.currentObjectFields.sort((a, b) => { return (a.FullNameTextCodeDefaultText.toLowerCase() === b.FullNameTextCodeDefaultText.toLowerCase()) ? 0 : (a.FullNameTextCodeDefaultText.toLowerCase() < b.FullNameTextCodeDefaultText.toLowerCase()) ? -1 : 1 });//.OrderBy(f => f.FullNameTextCodeDefaultText).ToList();
        if (this.SelectedItem) {
            this.currentScreenFields = window.ScreenFields.filter(sf => sf.Tenant == SessionLocator.Tenant && sf.ScreenId == this.SelectedItem.ScreenPM.Id);
            if (this.currentScreenFields.length == 0) {
                this.currentScreenFields = window.ScreenFields.filter(sf => sf.Tenant == 0 && sf.ScreenId == this.SelectedItem.ScreenPM.Id);
            }
        }
        else {
            this.currentScreenFields = [];
        }
       
        this.currentObjectFields.forEach(objectField => {
            if (this.currentScreenFields.filter(sf => sf.ObjectFieldCode == objectField.FieldCode).length == 0) {
                //if (objectField.DataTypeCode != null && !objectField.IsMulti) {
                this.banckStackFields.push(objectField);
                this.AllbanckStackFields.push(objectField);
                //}
            }

        });

        //this.SelectionChanged(this.ListBoxItemSource[0]);      
    }

    ListBoxSelectionMethod(Item: CodeNameClass) {
        //this.TabsList.Clear();
        //this.CountText = 0;
        //var myService: GeneralDomainService = new GeneralDomainService();
        //myService.GetTranslationsByParam(Item.Code, this.ObjecttableId, InfraSettings.TenantPM.Language).subscribe((myResult: ServiceResponse) => {
        //    if (myResult) {
        //        this.TranslationList = myResult.Result;           
        //        this.TabsList.InsertCollection(myResult.Result);
        //        this.CountText = myResult.Result.length;
        //    }
        //});       
    }

    CancelClicked() { this.CurrentSession.CloseCurrentWindow(); }
    public authHeader;
    OkClicked(CloseWindow: boolean = true) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
        var ScreenId = this.SelectedItem.ScreenPM.Id;
        //this.MyArgs.RemovedScreenFields = [];
        this.MyArgs.ScreenFields = [];
        var Columns = 0;
        var Rows = 0;
        this.ScreenRows.forEach(sItem => {
            Columns++;
            if (sItem.ScreenFieldPMs) {
                sItem.ScreenFieldPMs.forEach(myfield => {
                    Rows++;
                    ScreenId = myfield.ScreenId;
                    this.MyArgs.ScreenFields.push(myfield);
                });
            } 
        });
        this.MyArgs.Columns = Columns;
        this.MyArgs.Rows = Rows;//Math.ceil(Rows / Columns);
        this.MyArgs.ScreenId = ScreenId;//this.SelectedItem.ScreenPM.Id;

        this.myGeneralService.updateScreenFields(this.MyArgs).subscribe(myResult => {
            this.authHeader = new Headers();
            this.authHeader.append('Content-Type', 'application/json');
            this.authHeader.append('Accept', 'application/json');
            this.loginService.AuthHeader = this.authHeader;
            this.loginService.CurrentTenant = SessionLocator.Tenant;
            this.loginService.GetScreenFields().subscribe(myResult => {
                if (myResult != null) { 
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (CloseWindow == true) {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                    window.ScreenFields = myResult;
                    this.currentScreenFields = window.ScreenFields.filter(sf => sf.Tenant == SessionLocator.Tenant && sf.ScreenId == ScreenId);
                    if (this.currentScreenFields.length == 0) {
                        this.currentScreenFields = window.ScreenFields.filter(sf => sf.Tenant == 0 && sf.ScreenId == ScreenId);
                    }
                }
                this.loginService.GetScreens().subscribe(myScreensResult => {
                    window.Screens = myScreensResult;
                }); 
            }); 
        });
    }

    OnMyMouseDown(event) {
        console.log("Here we Go ..");
    }

    OnMyMouseUp(event) {
        if (this.IsMouseOver == true) {
            console.log("Here we Go Up..");
        }
    }
    IsMouseOver: boolean = false;
    onMyDragEnter($event) {
        event.preventDefault();
    }

    onMyDrop(event: DragEvent, item: ScreenRowDetails, row: number) {
       
        var id = event.dataTransfer.getData("Id");
        var myitem: ObjectFieldPM = this.banckStackFields.filter(d => d.Id == id)[0];
       
        if (myitem) {
            var test = window.ObjectFields.filter(a => a.Id == myitem.Id)[0];
            if (AppTool.IsNullOrEmpty(test.ObjectTableName)) {
                var table = window.ObjectTables.filter(d => d.Id == test.ObjectTableId)[0];
                window.ObjectFields.filter(a => a.Id == myitem.Id)[0].ObjectTableName = table.Name;
            }
            this.banckStackFields = this.banckStackFields.filter(d => d.Id != id);
            this.AllbanckStackFields = this.AllbanckStackFields.filter(d => d.Id != id);
            if (this.ScreenRows) {
                var Rows = this.ScreenRows.filter(a => a.ColumnIndex == item.ColumnIndex)[0];//.push(item); 
                if (Rows.ScreenFieldPMs == null) {
                    Rows.ScreenFieldPMs = [];
                }
                if (Rows.ObjectFieldPMs == null) {
                    Rows.ObjectFieldPMs = [];
                }
                var screenField = new ScreenFieldPM();
                screenField.Column = item.ColumnIndex;
                screenField.ObjectFieldId = myitem.Id;
                screenField.ScreenId = this.SelectedItem.ScreenPM.Id;
                screenField.Tenant = SessionLocator.Tenant;
                screenField.Row = Rows.ScreenFieldPMs.length;
                screenField.ObjectFieldCode = myitem.FieldCode;

                Rows.ScreenFieldPMs.push(screenField);
                Rows.ObjectFieldPMs.push(myitem);

            }
        }
        else {
            var Rows = this.ScreenRows.filter(a => a.ColumnIndex == item.ColumnIndex)[0];
            this.ScreenRows.forEach(sItem => {
                if (sItem.ObjectFieldPMs) {
                    var temp = sItem.ObjectFieldPMs.filter(a => a.Id == id);
                    if (temp.length > 0) {
                        var SField = sItem.ScreenFieldPMs.filter(a => a.ObjectFieldId == id)[0];
                        sItem.ObjectFieldPMs = sItem.ObjectFieldPMs.filter(a => a.Id != id);
                        sItem.ScreenFieldPMs = sItem.ScreenFieldPMs.filter(a => a.ObjectFieldId != id);
                        SField.Column = item.ColumnIndex;
                        SField.Row = Rows.ScreenFieldPMs ? Rows.ScreenFieldPMs.length : 0;
                        if (Rows.ScreenFieldPMs == null) {
                            Rows.ScreenFieldPMs = [];
                        }
                        if (Rows.ObjectFieldPMs == null) {
                            Rows.ObjectFieldPMs = [];
                        }
                        Rows.ObjectFieldPMs.push(temp[0]);
                        Rows.ScreenFieldPMs.push(SField);
                    }
                    if (sItem.ColumnIndex == item.ColumnIndex) {
                        sItem.ScreenFieldPMs.forEach(myfield => {
                            myfield.Row = sItem.ScreenFieldPMs.indexOf(myfield);
                        });
                    }
                }

            });
        }
    }

    OnObjectFieldDragStart(event, item) {
        if (item) {
            event.dataTransfer.setData("Id", item.Id);
        }
    }

    OnScreenFieldDragStart(event, item1) {
        if (item1) {
            event.dataTransfer.setData("Id", item1.Id);
        }
    }

    SearchTextChanged(value) {
        if (AppTool.IsNullOrEmpty(value)) {
            this.banckStackFields = this.AllbanckStackFields
        }
        else {
            this.banckStackFields = this.AllbanckStackFields.filter(f => (f.FullNameTextCodeDefaultText.toLowerCase().indexOf(value.toLowerCase()) > -1));
        }
    }

    OnDeleteField(item) {
        this.AllbanckStackFields.push(item);
        this.ScreenRows.forEach(sItem => {
            if (sItem.ObjectFieldPMs) {
                var temp = sItem.ObjectFieldPMs.filter(a => a.Id == item.Id);
                if (temp.length > 0) {
                    sItem.ObjectFieldPMs = sItem.ObjectFieldPMs.filter(a => a.Id != item.Id);
                    var myItem = sItem.ScreenFieldPMs.filter(a => a.ObjectFieldCode == item.FieldCode)[0];
                    sItem.ScreenFieldPMs = sItem.ScreenFieldPMs.filter(a => a.ObjectFieldCode != item.FieldCode);
                    if (this.MyArgs.RemovedScreenFields == null) {
                        this.MyArgs.RemovedScreenFields = [];
                    }
                    this.MyArgs.RemovedScreenFields.push(myItem);
                }

                sItem.ScreenFieldPMs.forEach(myfield => {
                    myfield.Row = sItem.ScreenFieldPMs.indexOf(myfield);
                });
            }

        });
    }
}




export class ScreenItem extends BaseComponent {
    constructor() { super(); }
    public Name: string;
    public ScreenPM: ScreenPM;

}

export class ScreenRowDetails {
    constructor() { }
    public ScreenFieldPMs: ScreenFieldPM[];
    public ObjectFieldPMs: ObjectFieldPM[];
    ColumnIndex: number;

}
