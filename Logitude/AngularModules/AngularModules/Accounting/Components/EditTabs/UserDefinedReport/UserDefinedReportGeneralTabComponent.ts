import { Component, OnInit} from '@angular/core';
import { CalculatedChartsOfAccountPM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountPM';
import { CalculatedChartsOfAccountsLinePM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountsLinePM';
import { UserDefinedReportPM } from 'Accounting/EntityPMs/UserDefinedReportPM';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { CustomFieldClass } from 'Infrastructure/DataContracts/CustomFieldClass';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AppTool } from 'Infrastructure/Tools';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    templateUrl: './UserDefinedReportGeneralTabComponent.html',
})

export class UserDefinedReportGeneralTabComponent extends BaseComponent implements OnInit{
    public EntityPM: UserDefinedReportPM;
    public ObjectTableName: string = "UserDefinedReport";
    public DataContext: UserDefinedReportGeneralTabComponent = this;
    public CalculatedChartsOfAccountItemList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    public  NoDataTextCode:string=null;
    public IsNew: boolean = false;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
     constructor(public entityArgs: EntityArgs,private entityResourceService: EntityResourceService) {
        super();
        
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
         this.EntityPM = entityArgs.EntityPM;
         this.CalculatedChartsOfAccountItemList = new ObservableCollection([]);
         this.CheckIsNewEntity();
         this.BuildLinesData();
         this.Listen();
         this.SetUIProperties();
    }
    ngOnInit() {
      
    }
    
    private CheckIsNewEntity(){
        if(!this.EntityPM.Id){
            this.EntityPM.Tenant = SessionLocator.TenantPM.Id;
         }
    }
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildLinesData();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildLinesData();
                }
            });
        }
    }
    SetUIProperties() {
       
    }
    
    OnFocus() {
        if (this.CalculatedChartsOfAccountItemList.Length == 0) {
            this.AddLine();
        }
    }
    OnRowEnded($event) {
        if (($event) == this.CalculatedChartsOfAccountItemList.Length) {
            this.AddLine();
        }
    }
    public BuildLinesData() {
        this.CalculatedChartsOfAccountItemList.Clear();
        var list = [];
        if(this.EntityPM.CalculatedChartsOfAccounts)
        this.EntityPM.CalculatedChartsOfAccounts.forEach(item => {
            list.push(new CalculatedChartsOfAccountItem(item, false, this));
        });
        this.CalculatedChartsOfAccountItemList.InsertCollection(list);
    }
 
    AddLine() {
 
    }

    LogWindowShow(title: string, itemComponent:CalculatedChartsOfAccountItem,IsNew:boolean = false) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = title;
        var myPath = "./Accounting/Components/Packages/EditTabs/UserDefinedReport/AddEditCalculatedChartsOfAccount/AddEditCalculatedChartsOfAccountComponent";
        logWindow.Width = 1000;
        logWindow.Height =500;
        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
        logWindow.WindowClosed.subscribe(s => {
            if (s!=null) {
                if(IsNew){
                   this.CalculatedChartsOfAccountItemList.Insert(itemComponent);
                }
            }
            else{
                if(!IsNew){
                    itemComponent.EntityPM.EnglishName = this.CalculatedEntityCopyEnglishName;
                    itemComponent.EntityPM.LocalName = this.CalculatedEntityCopyLocalName;
                    itemComponent.EntityPM.ChartOfAccountTypeCode = this.CalculatedEntityCopyChartTypeCode;
                    itemComponent.EntityPM.ChartOfAccountTypeLocalName = this.CalculatedEntityCopyChartTypeLocalName;
                    itemComponent.EntityPM.ChartOfAccountTypeEnglishName = this.CalculatedEntityCopyChartTypeEnglishName;
                }
            }
        })
    }

    get EnglishName() {
        if (this.EntityPM != null) {
            return this.EntityPM.EnglishName;
        }
        else
            return null;
    }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }
 
    get LocalName() {
        if (this.EntityPM != null) {
            return this.EntityPM.LocalName;
        }
        else
            return null;
    }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }
   
    get IsCancelled() {
        if (this.EntityPM != null) {
            return this.EntityPM.IsCancelled;
        }
        else
            return null;
    }
    set IsCancelled(newValue: boolean) {
        if (this.EntityPM.IsCancelled != newValue) {
            this.EntityPM.IsCancelled = newValue;
        }
    }

    public CalculatedEntityCopyEnglishName:string;
    public CalculatedEntityCopyLocalName:string;
    public CalculatedEntityCopyChartTypeCode:string;
    public CalculatedEntityCopyChartTypeEnglishName:string;
    public CalculatedEntityCopyChartTypeLocalName:string;

    public AddPeriodClicked(){
        var calculatedChartsOfAccountPM: CalculatedChartsOfAccountPM = new CalculatedChartsOfAccountPM(this.EntityPM);
        calculatedChartsOfAccountPM.Tenant = this.EntityPM.Tenant;
        var lastRow = this.CalculatedChartsOfAccountItemList.Collection[this.CalculatedChartsOfAccountItemList.Collection.length - 1];
        calculatedChartsOfAccountPM.Line = this.CalculatedChartsOfAccountItemList.Collection.length > 0 ? (lastRow.Line  + 1) : 1;
        if (!AppTool.IsNullOrEmpty(this.EntityPM) && !AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            calculatedChartsOfAccountPM.UserDefinedReportId = this.EntityPM.Id;
        }
        var line = new CalculatedChartsOfAccountItem(calculatedChartsOfAccountPM ,true, this);
        this.LogWindowShow(TextCodeTranslator.Translate("CalculatedChartsOfAccount"), line,true);
    }

    public EditPeriodClicked(item:CalculatedChartsOfAccountItem){
        this.CalculatedEntityCopyEnglishName = item.EntityPM.EnglishName;
        this.CalculatedEntityCopyLocalName = item.EntityPM.LocalName;
        this.CalculatedEntityCopyChartTypeCode =item.EntityPM.ChartOfAccountTypeCode;
        this.CalculatedEntityCopyChartTypeEnglishName =item.EntityPM.ChartOfAccountTypeEnglishName;
        this.CalculatedEntityCopyChartTypeLocalName =item.EntityPM.ChartOfAccountTypeLocalName;
        this.LogWindowShow(TextCodeTranslator.Translate("CalculatedChartsOfAccount"), item);
    }
  

}


export class CalculatedChartsOfAccountItem extends BaseComponent {
    public DataContext: CalculatedChartsOfAccountItem = this;
    public EntityPM: CalculatedChartsOfAccountPM;
    public ParentEntityPM: UserDefinedReportPM;
    public ObjectTableName: string = "CalculatedChartsOfAccount";
    public CalculatedChartsOfAccountsLineItemList: ObservableCollection;
    public IsNewEntity: boolean = false;
    public isRTL: boolean = false;

    constructor(entityPM: CalculatedChartsOfAccountPM, isNew: boolean, public fatherComponent) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityPM;
        this.EntityPM.EntityParentPM  = fatherComponent.EntityPM;
        this.CalculatedChartsOfAccountsLineItemList = new ObservableCollection([]);
        this.ParentEntityPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.BuildLinesData();
    }

 
 
    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
        }
    }

    get Line() {
        if (this.EntityPM != null) {
            return this.EntityPM.Line;
        }
        else
            return null;
    }
    set Line(newValue: number) {
        if (this.EntityPM.Line != newValue) {
            this.EntityPM.Line = newValue;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(newValue: string) {
        if (this.EntityPM.LocalName != newValue) {
            this.EntityPM.LocalName = newValue;
        }
    }

    get IsCancelled() { return this.EntityPM.IsCancelled; }
    set IsCancelled(newValue: boolean) {
        if (this.EntityPM.IsCancelled != newValue) {
            this.EntityPM.IsCancelled = newValue;
        }
    }

    get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    set UpdatedByUserId(newValue: string) {
        if (this.EntityPM.UpdatedByUserId != newValue) {
            this.EntityPM.UpdatedByUserId = newValue;
        }
    }

    get ChartOfAccountTypeCode() { return this.EntityPM.ChartOfAccountTypeCode; }
    set ChartOfAccountTypeCode(newValue: string) {
        if (this.EntityPM.ChartOfAccountTypeCode != newValue) {
            this.EntityPM.ChartOfAccountTypeCode = newValue;
        }
    }

    get ChartOfAccountTypeName() { return this.isRTL ? this.EntityPM.ChartOfAccountTypeLocalName : this.EntityPM.ChartOfAccountTypeEnglishName;  }

    get UpdatedByUserName() { return  this.isRTL ? this.EntityPM.UpdatedByLocalName : this.EntityPM.UpdatedByEnglishName; }

    get CreatedByUserName() { return this.isRTL ? this.EntityPM.CretedByLocalNameName : this.EntityPM.CreatedByEnglishName;  }
  
    get CreatedByUserId() { return this.EntityPM.CreatedByUserId; }
    set CreatedByUserId(newValue: string) {
        if (this.EntityPM.CreatedByUserId != newValue) {
            this.EntityPM.CreatedByUserId = newValue;
        }
    }

    get UpdatedDateTime() { return this.EntityPM.UpdatedDateTime; }
    set UpdatedDateTime(newValue: Date) {
        if (this.EntityPM.UpdatedDateTime != newValue) {
            this.EntityPM.UpdatedDateTime = newValue;
        }
    }

    get CreateDateTime() { return this.EntityPM.CreateDateTime; }
    set CreateDateTime(newValue: Date) {
        if (this.EntityPM.CreateDateTime != newValue) {
            this.EntityPM.CreateDateTime = newValue;
        }
    }


    public BuildLinesData() {
        this.CalculatedChartsOfAccountsLineItemList.Clear();
        var list = [];
        if(this.EntityPM.CalculatedChartsOfAccountLines)
        this.EntityPM.CalculatedChartsOfAccountLines.forEach(item => {
            list.push(new CalculatedChartsOfAccountsLineItem(item, false, this));
        });
        this.CalculatedChartsOfAccountsLineItemList.InsertCollection(list);
    }
 
 
}
 

export class CalculatedChartsOfAccountsLineItem extends BaseComponent {
    public DataContext: CalculatedChartsOfAccountsLineItem = this;
    public EntityPM: CalculatedChartsOfAccountsLinePM;
    public ParentEntityPM: CalculatedChartsOfAccountPM;
    public ObjectTableName: string = "CalculatedChartsOfAccountsLine";
    public IsNewEntity: boolean = false;
    public isRTL: boolean = false;

    constructor(entityPM: CalculatedChartsOfAccountsLinePM, isNew: boolean, public fatherComponent) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityPM;
        this.ParentEntityPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
    }
 
 
    get IsCancelled() { return this.EntityPM.IsCancelled; }
    set IsCancelled(newValue: boolean) {
        if (this.EntityPM.IsCancelled != newValue) {
            this.EntityPM.IsCancelled = newValue;
        }
    }

    get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    set UpdatedByUserId(newValue: string) {
        if (this.EntityPM.UpdatedByUserId != newValue) {
            this.EntityPM.UpdatedByUserId = newValue;
        }
    }

    get GLAccountName() { return this.isRTL ? this.EntityPM.GLAccountLocalName : this.EntityPM.GLAccountEnglishName;  }

    get ChartOfAccountName() { return this.isRTL ? this.EntityPM.ChartOfAccountLocalName : this.EntityPM.ChartOfAccountEnglishName;  }
 
    get LineTypetName() { return this.isRTL ? this.EntityPM.LineTypeLocalName : this.EntityPM.LineTypeEnglishName;  }

    get UpdatedByUserName() { return  this.isRTL ? this.EntityPM.UpdatedByLocalName : this.EntityPM.UpdatedByEnglishName; }

    get CreatedByUserName() { return this.isRTL ? this.EntityPM.CreatedByLocalName : this.EntityPM.CreatedByEnglishName;  }
  
    get GLAccountId() { return this.EntityPM.GLAccountId; }
    set GLAccountId(newValue: string) {
        if (this.EntityPM.GLAccountId != newValue) {
            this.EntityPM.GLAccountId = newValue;
        }
    }

    get LineTypeCode() { return this.EntityPM.LineTypeCode; }
    set LineTypeCode(newValue: string) {
        if (this.EntityPM.LineTypeCode != newValue) {
            this.EntityPM.LineTypeCode = newValue;
        }
    }

    get Line() {
        if (this.EntityPM != null) {
            return this.EntityPM.Line;
        }
        else
            return null;
    }
    set Line(newValue: number) {
        if (this.EntityPM.Line != newValue) {
            this.EntityPM.Line = newValue;
        }
    }

    get ChartOfAccountId() { return this.EntityPM.ChartOfAccountId; }
    set ChartOfAccountId(newValue: string) {
        if (this.EntityPM.ChartOfAccountId != newValue) {
            this.EntityPM.ChartOfAccountId = newValue;
        }
    }

    get CreatedByUserId() { return this.EntityPM.CreatedByUserId; }
    set CreatedByUserId(newValue: string) {
        if (this.EntityPM.CreatedByUserId != newValue) {
            this.EntityPM.CreatedByUserId = newValue;
        }
    }

    get UpdatedDateTime() { return this.EntityPM.UpdatedDateTime; }
    set UpdatedDateTime(newValue: Date) {
        if (this.EntityPM.UpdatedDateTime != newValue) {
            this.EntityPM.UpdatedDateTime = newValue;
        }
    }

    get CreateDateTime() { return this.EntityPM.CreateDateTime; }
    set CreateDateTime(newValue: Date) {
        if (this.EntityPM.CreateDateTime != newValue) {
            this.EntityPM.CreateDateTime = newValue;
        }
    }

 
}

