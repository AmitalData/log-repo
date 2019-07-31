import {Component} from '@angular/core';
import {GeneralDomainService, FieldsTranslations, FieldsUpdateHelper} from '../../../../Infrastructure/Services/GeneralDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObjectTablePM} from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import {ObjectFieldPM} from '../../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {TextCodePM} from '../../../../Infrastructure/EntityPMs/TextCodePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
declare var window;

@Component({
    moduleId: module.id,
    templateUrl: './ObjectLabelsComponent.html',
})

export class ObjectLabelsComponent extends BaseComponent {
    public DataContext: ObjectLabelsComponent = this;
    public DefaultText: string;   
    public DirtyItems: FieldsTranslations[];
    private ObjecttableId: string;
    private myService: EntityResourceService;
    private TranslationList: Array<FieldsTranslations> = [];    
    public ListBoxItemSource: Array<CodeNameClass> = [];
    public  ObjectTableName: string;
    public TabsList: ObservableCollection;
    public CountText: number = 0;
    public ObjectTablePM: ObjectTablePM;
    public ValidationErrorsList: Array<String> = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.myService = new EntityResourceService();   
        this.TabsList = new ObservableCollection([]);   
        this.DirtyItems = [];
        this.EntityPM = new FieldsTranslations();
    }

    public get Singular() { return this.EntityPM.TranslatedText; };
    public get Plural() { return this.EntityPM.TranslatedTextPlural; };

    public set Singular(value: string) { if (this.EntityPM.TranslatedText != value) this.EntityPM.TranslatedText = value; }
    public set Plural(value: string) { if (this.EntityPM.TranslatedTextPlural != value) this.EntityPM.TranslatedTextPlural = value; }

    private searchText: string;
    public get SearchText() { return this.searchText; }
    public set SearchText(value: string) {
        if (this.searchText != value) {
            this.searchText = value;
            this.FillList();  
        }
    }
    

    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }

    public SelectedItem: CodeNameClass;
    SelectionChanged(Item) {
        this.SelectedItem = Item;
        this.ListBoxSelectionMethod(Item);
    }
    public EntityPM: FieldsTranslations;
    SetWindowArgs(windowArgs: FieldsTranslations) {
       this.ObjecttableId = windowArgs.ObjectTableID;
       this.DefaultText = windowArgs.DefaultText;
       this.Singular = windowArgs.TranslatedText;
       this.Plural = windowArgs.TranslatedTextPlural;
       this.ObjectTableName = windowArgs.ObjectTableName;
       this.UIProperties.SetEnabled("DefaultText", this.ObjectTableName, false);
       this.ObjectTablePM = window.ObjectTables.filter(d => d.Id == this.ObjecttableId)[0];
       this.EntityPM = windowArgs;
        this.FillListBox();
    }

   FillListBox() {
       this.ListBoxItemSource = [];
           var list: Array<any> = window.TextCodes.filter(d => d.ObjectTableId == this.ObjecttableId);
           this.ListBoxItemSource.push(new CodeNameClass("All", "All"));
           if (list != null) {
               if (list.filter(d => d.TextCodeTypeCode == "TH")[0]) { this.ListBoxItemSource.push(new CodeNameClass("TH", "Tab Headers")) };
               if (list.filter(d => d.TextCodeTypeCode == "B")[0]) { this.ListBoxItemSource.push(new CodeNameClass("B", "Buttons And Actions")) };
               if (list.filter(d => d.TextCodeTypeCode == "H")[0]) { this.ListBoxItemSource.push(new CodeNameClass("H", "Help Text")) };
               if (list.filter(d => d.TextCodeTypeCode == "M")[0]) { this.ListBoxItemSource.push(new CodeNameClass("M", "Messages")) };
               if (list.filter(d => d.TextCodeTypeCode == "MH")[0]) { this.ListBoxItemSource.push(new CodeNameClass("MH", "Menu Headers")) };
               if (list.filter(d => d.TextCodeTypeCode == "MC")[0]) { this.ListBoxItemSource.push(new CodeNameClass("MC", "Maintenance")) };
               if (list.filter(d => d.TextCodeTypeCode == "Q")[0]) { this.ListBoxItemSource.push(new CodeNameClass("Q", "Queries")) };
               if (list.filter(d => d.TextCodeTypeCode == "S")[0]) { this.ListBoxItemSource.push(new CodeNameClass("S", "Screens")) };
               if (list.filter(d => d.TextCodeTypeCode == "L")[0]) { this.ListBoxItemSource.push(new CodeNameClass("L", "Links")) };
               if (list.filter(d => d.TextCodeTypeCode == "O")[0]) { this.ListBoxItemSource.push(new CodeNameClass("O", "Others")) };
               if (list.filter(d => d.TextCodeTypeCode == "G")[0]) { this.ListBoxItemSource.push(new CodeNameClass("G", "General")) };
           }
           this.SelectionChanged(this.ListBoxItemSource[0]);      
   }
   private list: FieldsTranslations[];
   ListBoxSelectionMethod(Item: CodeNameClass) {
       this.TabsList.Clear();
       this. list =  [];

       this.CountText = 0;
       var myService: GeneralDomainService = new GeneralDomainService();
       this.CurrentSession.StartBusyIndicatorLoading();

       myService.GetTranslationsByParam(Item.Code, this.ObjecttableId, InfraSettings.TenantPM.Language).subscribe((myResult: ServiceResponse) => {
           if (myResult) {
               this.list = myResult.Result;
               this.FillList();
               this.CurrentSession.StopBusyIndicator();
           }
       });       
   }

   private FillList() {
       var temp: FieldsTranslationsItem[] = [];

       if (AppTool.IsNullOrEmpty(this.SearchText)) {
           this.list.forEach((item) => {
               temp.push(new FieldsTranslationsItem(item,this));
           })
       }

       else {
           this.list.filter(d => !AppTool.IsNullOrEmpty(d.DefaultText) && d.DefaultText.toUpperCase().startsWith(this.SearchText.toUpperCase())
               || !AppTool.IsNullOrEmpty(d.TranslatedText) && d.TranslatedText.toUpperCase().startsWith(this.SearchText.toUpperCase())
               || !AppTool.IsNullOrEmpty(d.Code) && d.Code.toUpperCase().startsWith(this.SearchText.toUpperCase()))
               .forEach((item) => {
                   temp.push(new FieldsTranslationsItem(item,this));
               });
       }

       this.TabsList.InsertCollection(temp);
       this.CountText = this.TabsList.Length;
   }

   CancelClicked() { this.CurrentSession.CloseCurrentWindow(); }


   SaveClicked() {
       if (this.DirtyItems.length > 0) {
           this.CurrentSession.StartBusyIndicatorSaving();

           var myServiceHelper = new FieldsUpdateHelper();
           myServiceHelper.Tenant = SessionLocator.Tenant;
           myServiceHelper.Items = this.DirtyItems;

           var generalService: GeneralDomainService = new GeneralDomainService();
           generalService.UpdateFieldsTranslations(myServiceHelper).subscribe((myResponse: ServiceResponse) => {
               if (myResponse.HasError) {
                   this.CurrentSession.StopBusyIndicator();
               }

               else {
                   this.CurrentSession.CloseCurrentWindowEmit("Ok");
                   this.CurrentSession.StopBusyIndicator();
               }
           });
       }
   }
   OkClicked() {
       this.ValidationErrorsList = [];
       if (AppTool.IsNullOrEmpty(this.Singular)) {
           this.ValidationErrorsList.push("Singular is Required");
       }
       if (AppTool.IsNullOrEmpty(this.Plural)) {
           this.ValidationErrorsList.push("Plural is Required");
       }


       if (this.DirtyItems.indexOf(this.EntityPM) == -1) 
           this.DirtyItems.push(this.EntityPM);
             
       if (this.ValidationErrorsList.length == 0)
           this.SaveClicked();
   }
}




export class FieldsTranslationsItem extends BaseComponent {
    public DataContext: FieldsTranslationsItem = this;
    public Entity: FieldsTranslations;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entity: FieldsTranslations, public fatherComponent: ObjectLabelsComponent) {
        super();
        this.Entity = entity;
    }

    get Code() { return this.Entity.Code; }    
    get DefaultText() { return this.Entity.DefaultText; }
    get TranslatedText() { return this.Entity.TranslatedText; }
    set TranslatedText(newValue: string) {
        if (this.Entity.TranslatedText != newValue) {
            this.Entity.TranslatedText = newValue;

            if (this.fatherComponent.DirtyItems.indexOf(this.Entity) == -1) {
                this.fatherComponent.DirtyItems.push(this.Entity);
            }
        }
    }

    get TranslatedTextPlural() { return this.Entity.TranslatedTextPlural; }
    set TranslatedTextPlural(newValue: string) {
        if (this.Entity.TranslatedTextPlural != newValue) {
            this.Entity.TranslatedTextPlural = newValue;

            if (this.fatherComponent.DirtyItems.indexOf(this.Entity) == -1) {
                this.fatherComponent.DirtyItems.push(this.Entity);
            }
        }
    }
}
