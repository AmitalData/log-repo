
import { Component, QueryList, ViewChildren } from '@angular/core';
import { GeneralDomainService } from '../../../../Infrastructure/Services/GeneralDomainService';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { ObjectFieldPM } from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import { ScreenPM } from '../../../../Infrastructure/EntityPMs/ScreenPM';
import { ScreenFieldPM } from '../../../../Infrastructure/EntityPMs/ScreenFieldPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ScreenLayoutArgs } from '../../../../Infrastructure/DataContracts/ScreenLayoutArgs';
import { LoginService } from '../../../../Infrastructure/Services/LoginService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ScreenSectionPM } from '../../../../Infrastructure/EntityPMs/ScreenSectionPM';
import { IScreenLayoutService } from '../../Interface/IScreenLayoutService';
import { MuiltSectionScreenLayoutService } from '../../ExternalService/MuiltSectionScreenLayoutService';
import { ClassicScreenLayoutService } from '../../ExternalService/ClassicScreenLayoutService';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';



declare var window;
const AddEditScreenWidth = 420;
const AddEditScreenHeight = 250;
const EditScreenTitle = "Edit Screen";
@Component({

    templateUrl: './ScreenLayoutComponent.html',
    styleUrls: ['./ScreenLayoutComponent.css']
})

export class ScreenLayoutComponent extends BaseComponent {
    public DataContext: ScreenLayoutComponent = this;
    public MyArgs: ScreenLayoutArgs = new ScreenLayoutArgs();
    public ObjecttableId: string;
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
    private ObjectTable: ObjectTablePM;
    private CurrentSession = SessionLocator.SelectedSession;
    Modified: boolean = false;
    private screenLayoutService: IScreenLayoutService;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    constructor() {
        super();
        this.myService = new EntityResourceService();
        this.myGeneralService = new GeneralDomainService();
        this.loginService = new LoginService();
    }


    public SelectedItem: ScreenItem;
    public OldItem: ScreenItem;
    SelectionChanged(Item) {
        //this.OkClicked(false);
        this.SelectedItem = Item;
        if (this.Modified) {
            this.OpenConfirmWindow();
            return;
        }
        this.GetFields();
    }

    GetFields() {
        this.OldItem = this.SelectedItem;
        let screenType = this.SelectedItem.ScreenPM.Type ? this.SelectedItem.ScreenPM.Type : "ClASSIC";
        this.LoadScreenComponent(screenType);
        this.screenLayoutService = this.GetScreenLayoutService();
        this.Modified = false;
        this.FillbanckStackFields();
        this.myGeneralService.GetScreenModificationByScreenCode(this.SelectedItem.ScreenPM.Code).subscribe((myResult: ServiceResponse) => {
            var myResponse: ServiceResponse = myResult;
            var screen: any = myResponse.Result != null ? myResponse.Result : this.SelectedItem.ScreenPM;
            this.screenLayoutService.GenerateScreen(screen);
        });
    }

    private GetScreenLayoutService() {
        return this.IsMuiltSectionScreen ? new MuiltSectionScreenLayoutService(this) : new ClassicScreenLayoutService(this);
    }

    SetWindowArgs(windowArgs: any) {
        this.ObjecttableId = windowArgs.ObjectTableID;
        this.ObjectTable = window.ObjectTables.filter(x => x.Id === this.ObjecttableId)[0];
        this.FillTableScreensCollection();
    }



    SectionScreens: SectionScreenItem[] = [];

    BuildScreenRowDetails(columnIndex: number, sectionNumber: number=null ) {
        var screenRowDetails = new ScreenRowDetails();
        screenRowDetails.ColumnIndex = columnIndex;
        var screenObjectFields = this.GetScreenObjectFields(columnIndex, sectionNumber);
        screenObjectFields.forEach(field => {
            screenRowDetails.ScreenFieldPMs.push(field);
            screenRowDetails.ObjectFieldPMs.push(window.ObjectFields.filter(a => a.FieldCode == field.ObjectFieldCode)[0]);
        });
        return screenRowDetails;
    }


    private GetScreenObjectFields(columnIndex: number, sectionNumber: number) {
        var screenObjectFields = this.currentScreenFields.filter(a => a.Column == columnIndex);
        if (sectionNumber) {
            screenObjectFields = screenObjectFields.filter(a => a.SectionNumber == sectionNumber);
        }
        return this.Clone(screenObjectFields.sort((a, b) => { return a.Row - b.Row }));
    }




    FillTableScreensCollection(changeSelection = true) {
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
                item.Type = screen.Type;
                this.TableScreensCollection.push(item);
            });

            if(changeSelection)
                this.SelectionChanged(this.TableScreensCollection[0]);
        }
    }


    public AddScreenItem(screen: ScreenPM) {
        var screenItem = new ScreenItem();
        screenItem.ScreenPM = screen;
        screenItem.Name = screen.Name;
        screenItem.Type = screen.Type;
        this.TableScreensCollection.push(screenItem);
        window.Screens.push(screen);
        this.SelectionChanged(screenItem);
        this.Modified = true;

    }


    FillbanckStackFields() {
        this.banckStackFields = [];
        this.AllbanckStackFields = [];
        const table: ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.ObjecttableId)[0];
        if (table.Name === "Master") {
            const shipmentObjectTable = window.ObjectTables.filter(x => x.Name === "Shipment")[0];
            this.currentObjectFields = window.ObjectFields.filter(d => d.ObjectTableId == shipmentObjectTable.Id && !d.IsCustomFilter && d.PMPropertyPath != null && d.DataTypeCode != null && !d.IsMulti);//.Where(o => !d.IsCustomFilter && d.PMPropertyPath != null).OrderBy(f => f.FullNameTextCodeDefaultText).ToList();

        }
        else {

            this.currentObjectFields = window.ObjectFields.filter(d => d.ObjectTableId == this.ObjecttableId && !d.IsCustomFilter && d.PMPropertyPath != null && d.DataTypeCode != null && !d.IsMulti);//.Where(o => !d.IsCustomFilter && d.PMPropertyPath != null).OrderBy(f => f.FullNameTextCodeDefaultText).ToList();

        }
        this.currentObjectFields = this.currentObjectFields.sort((a, b) => { return (a.FullNameTextCodeDefaultText.toLowerCase() === b.FullNameTextCodeDefaultText.toLowerCase()) ? 0 : (a.FullNameTextCodeDefaultText.toLowerCase() < b.FullNameTextCodeDefaultText.toLowerCase()) ? -1 : 1 });//.OrderBy(f => f.FullNameTextCodeDefaultText).ToList();
        if (this.SelectedItem) {
            this.currentScreenFields = window.ScreenFields.filter(sf => sf.Tenant == SessionLocator.Tenant && sf.ScreenCode == this.SelectedItem.ScreenPM.Code);
            if (this.currentScreenFields.length == 0) {
                this.currentScreenFields = window.ScreenFields.filter(sf => sf.Tenant == 0 && sf.ScreenCode == this.SelectedItem.ScreenPM.Code);
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

        this.screenLayoutService.BuildScreenUpdateArgs();
        this.MyArgs.ScreenId = this.OldItem.ScreenPM.Id;
        this.MyArgs.ScreenCode = this.OldItem.ScreenPM.Code;
        this.myGeneralService.updateScreenFields(this.MyArgs).subscribe((myResult: ServiceResponse) => {
            this.authHeader = new Headers();
            this.authHeader.append('Content-Type', 'application/json');
            this.authHeader.append('Accept', 'application/json');
            this.loginService.AuthHeader = this.authHeader;
            this.loginService.CurrentTenant = SessionLocator.Tenant;
            this.loginService.GetScreenFields().subscribe((myResult: any) => {
                if (myResult != null) {
                    this.UpdateWindowFields(myResult);
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (CloseWindow == true) {
                        this.CurrentSession.CloseCurrentWindow();
                    } else {
                        this.GetFields();
                    }
                }
                this.loginService.GetScreens().subscribe((myScreensResult: any) => {
                    window.Screens = myScreensResult;
                });
            });
        });
    }


    GetModifySections() {
        var sections: any[] = [];
        this.SectionScreens.forEach(sectionScreen => {

            if (!sectionScreen.Section.ChangeSetOp && sectionScreen.Section.IsDirty) {
                sectionScreen.Section.ChangeSetOp = "Update";
            }
            if (sectionScreen.Section.ChangeSetOp) sections.push(sectionScreen.Section);

        });
        return sections;


    }
    prevSelectedItem;
    GetScreensFromDB(){
        const authHeader = new Headers();
        authHeader.append('Content-Type', 'application/json');
        authHeader.append('Accept', 'application/json');
        this.loginService.AuthHeader = authHeader;
        this.loginService.CurrentTenant = SessionLocator.Tenant;
        this.loginService.GetScreens().subscribe((myScreensResult: any) =>
        {
            window.Screens = myScreensResult;
            this.prevSelectedItem = this.SelectedItem;
            this.FillTableScreensCollection(false);
            const editedItem = this.TableScreensCollection.find(s=>s.ScreenPM.Id == this.prevSelectedItem.ScreenPM.Id);
            this.SelectionChanged(editedItem)
        });
    }


    UpdateWindowFields(screenFields) {
        window.ScreenFields = screenFields;
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

    get IsMuiltSectionScreen (){
        var screen = this.OldItem ? this.OldItem : this.SelectedItem;
        return screen && screen.Type == 'LIGHTENING';
    }

    onMyDrop(event: DragEvent, screenRowDetails: ScreenRowDetails, column: number, sectionNumber: number = null) {
        let screenRows = this.screenLayoutService.GetScreenRows(sectionNumber);
        this.Modified = true;
        let objectFieldId = event.dataTransfer.getData("Id");
        let fieldCode = event.dataTransfer.getData("FieldCode");

        let position = this.GetElementPosition(event, screenRowDetails);
        let myitem: ObjectFieldPM = this.banckStackFields.filter(d => d.Id == objectFieldId)[0];

        if (myitem) {
            let objectField = window.ObjectFields.filter(a => a.Id == myitem.Id)[0];
            if (AppTool.IsNullOrEmpty(objectField.ObjectTableName)) {
                var table = window.ObjectTables.filter(d => d.Id == objectField.ObjectTableId)[0];
                window.ObjectFields.filter(a => a.Id == myitem.Id)[0].ObjectTableName = table.Name;
            }
            this.banckStackFields = this.banckStackFields.filter(d => d.Id != objectFieldId);
            this.AllbanckStackFields = this.AllbanckStackFields.filter(d => d.Id != objectFieldId);
            if (screenRows) {

                var rows = screenRows.filter(a => a.ColumnIndex == screenRowDetails.ColumnIndex)[0];//.push(item);
                if (rows.ScreenFieldPMs == null) {
                    rows.ScreenFieldPMs = [];
                }
                if (rows.ObjectFieldPMs == null) {
                    rows.ObjectFieldPMs = [];
                }
                var screenField = new ScreenFieldPM();
                screenField.Column = screenRowDetails.ColumnIndex;
                screenField.ObjectFieldId = myitem.Id;
                screenField.ScreenId = this.SelectedItem.ScreenPM.Id;
                screenField.ScreenCode = this.SelectedItem.ScreenPM.Code;
                screenField.Tenant = SessionLocator.Tenant;
                screenField.Row = position;
                screenField.ObjectFieldCode = myitem.FieldCode;
                if (this.IsMuiltSectionScreen) screenField.SectionNumber = sectionNumber
                rows.ScreenFieldPMs.splice(position, 0, screenField);
                rows.ObjectFieldPMs.splice(position, 0, myitem);

            }
        }
        else {

            this.screenLayoutService.ChangeScreenFieldPosition
                ({
                    SectionNumber: sectionNumber,
                    ObjectFieldId: objectFieldId,
                    FieldCode: fieldCode,
                    Rows: screenRows.filter(a => a.ColumnIndex == screenRowDetails.ColumnIndex)[0],
                    ScreenRowDetails: screenRowDetails,
                    Position: position
                });

        }
    }
    public ChangeScreenFieldPosition(screenRow  , args: any) {

        if (screenRow.ObjectFieldPMs) {

            let temp = screenRow.ObjectFieldPMs.filter(a => a.Id == args.ObjectFieldId);
            if (temp.length > 0) {
                let screenField = screenRow.ScreenFieldPMs.filter(a => a.ObjectFieldCode == args.FieldCode)[0];
                screenRow.ObjectFieldPMs = screenRow.ObjectFieldPMs.filter(a => a.Id != args.ObjectFieldId);
                screenRow.ScreenFieldPMs = screenRow.ScreenFieldPMs.filter(a => a.ObjectFieldCode != args.FieldCode);
                screenField.Column = args.ScreenRowDetails.ColumnIndex;
                screenField.Row = args.Position;
                if (this.IsMuiltSectionScreen) screenField.SectionNumber = args.SectionNumber;

                if (args.Rows.ScreenFieldPMs == null) {
                    args.Rows.ScreenFieldPMs = [];
                }
                if (args.Rows.ObjectFieldPMs == null) {
                    args.Rows.ObjectFieldPMs = [];
                }
                args.Rows.ObjectFieldPMs.splice(args.Position, 0, temp[0]);
                args.Rows.ScreenFieldPMs.splice(args.Position, 0, screenField);

            }
            if (screenRow.ColumnIndex == args.ScreenRowDetails.ColumnIndex) {
                screenRow.ScreenFieldPMs.forEach(myfield => {
                    myfield.Row = screenRow.ScreenFieldPMs.indexOf(myfield);
                });
            }
        }
    }


    private GetElementPosition(event: DragEvent, screenRowDetails: ScreenRowDetails): number {
        var itemLists: HTMLElement[] = Array.from(document.getElementsByName('listItem'));
        itemLists = itemLists?.filter(x => Number(x.getAttribute("column")) == screenRowDetails.ColumnIndex)
        if (!itemLists || itemLists.length == 0) return 0;

        var droppedYPosition = Number(event.y);
        if (droppedYPosition < Number(itemLists[0].getBoundingClientRect().top)) return 0;

        var closestElement: HTMLElement;
        itemLists.forEach(element => {
            if (droppedYPosition > Number(element.getBoundingClientRect().top)) closestElement = element;
        });
        if (!closestElement) return screenRowDetails.ScreenFieldPMs.length;

        var position = Number(closestElement.getAttribute("row"));
        return this.IsLastItem(position, screenRowDetails) ? position + 1 : position;
    }

    IsLastItem(position: number, screenRowDetails: any) {
        return position + 1 == (screenRowDetails.ScreenFieldPMs?.length ?? 0);
    }

    OnObjectFieldDragStart(event, item) {
        if (item) {
            event.dataTransfer.setData("Id", item.Id);
            event.dataTransfer.setData("FieldCode", item.FieldCode);
        }
    }

    OnScreenFieldDragStart(event: DragEvent, item1) {
        if (item1) {
            event.dataTransfer.setData("Id", item1.Id);
            event.dataTransfer.setData("FieldCode", item1.FieldCode);
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

    DeleteSectionFields(editedSection: SectionScreenItem){
        editedSection.ScreenRows.forEach(row=>{
            row.ObjectFieldPMs.forEach(field=>{
                this.OnDeleteField(field,editedSection.Section.Number);
            });
        });
    }

    OnDeleteField(item, sectionNumber:number=null ) {
        this.Modified = true;
        this.AllbanckStackFields.push(item);


        var screenRows = this.screenLayoutService.GetScreenRows(sectionNumber);

        screenRows.forEach(sItem => {
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


    public HideSectionAreaClicked(sectionScreen: SectionScreenItem ) {
        sectionScreen.HideSectionArea = !sectionScreen.HideSectionArea;
    }

    Clone(list: any): any {
        return JSON.parse(JSON.stringify(list));
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
                this.OkClicked(false);
                return;
            }
            if (confirmWindow.No) {
                this.GetFields();
                return;
            }
            this.SelectedItem = this.OldItem;
        });
    }

    NewScreenButtonClicked() {

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 420;
        logitudeWindow.Height = 250;
        logitudeWindow.Title = "New Screen"
        let windowArgs: any = {};
        windowArgs.ScreenLayoutComponent = this;
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditScreenComponent');
    }


    NewSectionButtonClick() {
        var screenSection: ScreenSectionPM = this.GetNewInstanceFromScreenSectionPM();
        var sectionScreen: SectionScreenItem = new SectionScreenItem(screenSection);
        this.ScreenRows = [];
        for (var i = 0; i < this.SelectedItem.ScreenPM.NumberOfColumns; i++) {
            var screenRowDetails = this.BuildScreenRowDetails(i, screenSection.Number);
            this.ScreenRows.push(screenRowDetails);
        }

        sectionScreen.ScreenRows = this.ScreenRows;
        sectionScreen.IsNew = true;
        this.SectionScreens.push(sectionScreen);

    }

    EditSelectedScreen(){
        const logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = AddEditScreenWidth;
        logitudeWindow.Height = AddEditScreenHeight;
        logitudeWindow.Title = EditScreenTitle
        logitudeWindow.WindowArgs = {
            ScreenLayoutComponent: this,
            Screen: this.SelectedItem.ScreenPM
        };
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/Customization/AddEditScreenComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.GetScreensFromDB();
        });
    }




    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer(screenType: string) {
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.LoadScreenComponent(screenType), 1);
        }
    }

    LoadScreenComponent(screenType: string) {

        if (!this.AllLocations || this.AllLocations.length == 0 || !this.AllLocations.toArray().filter(d => d.Code == screenType)[0]) {
            this.RunComponentTimer(screenType);
            return;
        }

        this.LoadScreen(screenType);

    }

    LoadScreen(screenType: string) {

        if (this.AllLocations && this.AllLocations.length > 0) {
            let myGeneratedComponentLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == screenType)[0];
            if (myGeneratedComponentLocation != null) {
                myGeneratedComponentLocation.viewContainerRef.clear();
                SessionLocator.DynamicLoader.Load(this.GetScreenComponentPath(screenType), myGeneratedComponentLocation.viewContainerRef)
                    .then(cmpRef => { cmpRef.instance.Run(this);});
            }
        }
    }

    GetScreenComponentPath(screenType:string) {
        if (screenType == "LIGHTENING") {
            return './InfrastructureModules/InfrastructureCustomization/Components/Customization/Screen/LighteningScreenComponent';
        }

        return './InfrastructureModules/InfrastructureCustomization/Components/Customization/Screen/ClassicScreenComponent';

    }

    private GetNewInstanceFromScreenSectionPM() {
        var screenSection: ScreenSectionPM = new ScreenSectionPM();
        screenSection.ScreenCode = this.SelectedItem.ScreenPM.Code;
        screenSection.Tenant = SessionLocator.Tenant;
        screenSection.CreatedByUserId = SessionLocator.LoggedUserId;
        screenSection.Number = (this.SectionScreens.length + 1);
        screenSection.ChangeSetOp = "Insert";
        return screenSection;
    }


    private AddScreenField(screenRowDetails: ScreenRowDetails, sectionScreen: SectionScreenItem) {
        screenRowDetails.ScreenFieldPMs.forEach(screenField => {
            this.MyArgs.ScreenFields.push(screenField);
            this.MyArgs.Rows += 1;
            if (sectionScreen)
                sectionScreen.Section.NumberOfRows += 1;
        });
    }


}


export class SectionScreenItem extends BaseComponent {
    constructor(screenSection: ScreenSectionPM) {
        super();
        this.Section = screenSection;
    }

    public Section: ScreenSectionPM;
    public ScreenRows: Array<ScreenRowDetails> = [];
    public HideSectionArea: boolean;
    public IsChange: boolean;
    public IsNew: boolean;
    get Inactive(): boolean {
        return this.Section?.Inactive;
    }



}

export class ScreenItem extends BaseComponent {
    constructor() { super(); }
    public Name: string;
    public Type: string;

    public ScreenPM: ScreenPM;

}

export class ScreenRowDetails {
    constructor() {
        this.ScreenFieldPMs = [];
        this.ObjectFieldPMs = [];

    }

    public ScreenFieldPMs: ScreenFieldPM[];
    public ObjectFieldPMs: ObjectFieldPM[];
    ColumnIndex: number;

}




