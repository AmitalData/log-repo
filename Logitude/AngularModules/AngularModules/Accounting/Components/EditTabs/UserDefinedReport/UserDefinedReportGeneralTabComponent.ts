import { Component, OnInit} from '@angular/core';
import { CalculatedChartsOfAccountPM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountPM';
import { CalculatedChartsOfAccountsLinePM } from 'Accounting/EntityPMs/CalculatedChartsOfAccountsLinePM';
import { UserDefinedReportPM } from 'Accounting/EntityPMs/UserDefinedReportPM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';

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
         this.BuildLinesData();
         this.Listen();
         this.SetUIProperties();
    }
    ngOnInit() {
      
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
    

    public BuildLinesData() {
        this.CalculatedChartsOfAccountItemList.Clear();
        var list = [];
        this.EntityPM.CalculatedChartsOfAccounts.forEach(item => {
            list.push(new CalculatedChartsOfAccountItem(item, true, this));
        });
        this.CalculatedChartsOfAccountItemList.InsertCollection(list);
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
        this.CalculatedChartsOfAccountsLineItemList = new ObservableCollection([]);
        this.ParentEntityPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.BuildLinesData();
    }
 
    get EnglishName() { return this.EntityPM.EnglishName; }
    set InterestBaseStartDate(newValue: string) {
        if (this.EntityPM.EnglishName != newValue) {
            this.EntityPM.EnglishName = newValue;
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
        this.EntityPM.CalculatedChartsOfAccountLines.forEach(item => {
            list.push(new CalculatedChartsOfAccountsLineItem(item, true, this));
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

