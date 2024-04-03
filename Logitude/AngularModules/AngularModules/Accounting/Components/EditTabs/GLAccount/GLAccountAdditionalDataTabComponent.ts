import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {GLAccountList} from '../../../EntityLists/GLAccountList';
import {GLAccountExtendedPMService}  from '../../../Services/ExtendedPMs/GLAccountExtendedPMService';
import {GLAccountPMService}  from '../../../Services/StandardPMs/GLAccountPMService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
declare var window: any;
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { GLAccountCurrencyExtendedPMService } from '../../../Services/ExtendedPMs/GLAccountCurrencyExtendedPMService';
import { GLAccountCurrencyPM } from '../../../EntityPMs/GLAccountCurrencyPM';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    
    templateUrl: './GLAccountAdditionalDataTabComponent.html',
 
})


export class GLAccountAdditionalDataTabComponent extends BaseComponent  {

    ObjectTableName: string = "GLAccount";
    ObjectTableId: string = window.ObjectTables.filter(f => f.Name === this.ObjectTableName)[0].Id;
    DataContext: any = this;
    ConnectedGLAccounts: ObservableCollection;
    ChildrenGLAccounts: ObservableCollection;
    glAccountExtendedListService: GLAccountExtendedListService = new GLAccountExtendedListService();
    GLAccountExtendedPMService: GLAccountExtendedPMService = new GLAccountExtendedPMService();
    gLAccountCurrencyExtendedPMService: GLAccountCurrencyExtendedPMService = new GLAccountCurrencyExtendedPMService();
    entityResourceService: EntityResourceService = new EntityResourceService();
    public ChildrenFilterItems: ApiQueryFilters;

    public entityPM: GLAccountPM = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.ConnectedGLAccounts = new ObservableCollection([]);
        this.ChildrenGLAccounts = new ObservableCollection([]);
        this.entityPM = this.entityArgs.EntityPM;
        this.BuildConnectedGLAccountsList();
        this.BuildChildrenGLAccountsList();
        this.ChildrenFilterItems = new ApiQueryFilters();
        //this.ChildrenFilterItems.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "Exclude", false, false, false, "string", false, true);
        //this.ChildrenFilterItems.addAdditionalFilter("ChartOfAccountsId", this.EntityPM.ChartOfAccountsId, null, null, "Equals", false, false, false, "string", false, true);
        //this.ChildrenFilterItems.addAdditionalFilter("ParentAccountId", this.entityPM.ParentAccountId, null, null, "IsNull", false, false, false, "string", false, true);

        //if (this.IsCustomerAccount) {
        //    this.ParentsFilterItems.addAdditionalFilter("AccountTypeCode", "2", null, null, "Equals", false, false, false, "string", false, true);
        //}

    }
//this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    BuildConnectedGLAccountsList() {


        this.GLAccountExtendedPMService.GetSplittedByCurrencyGLAccounts(this.entityPM.Id).subscribe((myResponse: ServiceResponse) => {

            if (myResponse) {
                if (!myResponse.HasError) {
                    for (let item of myResponse.Result) {
                        this.ConnectedGLAccounts.Insert(new SplittedByCurrencyAccount(item, this));
                    }
                }
            }

        });


    }

    BuildChildrenGLAccountsList() {
        this.ChildrenGLAccounts.Clear();
        this.glAccountExtendedListService.GetChildrenGLAccounts(this.entityPM.Id).subscribe((myResponse: ServiceResponse) => {

            if (myResponse) {
                if (!myResponse.HasError) {
                    for (let item of myResponse.Result) {
                        this.ChildrenGLAccounts.Insert(new GLAccountChild(item, this));
                    }
                }
            }

        });


    }

    public IsConnectedWithExistGLAccount (DisplayNumber:string):boolean{
     if(DisplayNumber.includes(this.entityPM.DisplayNumber+"\\")){
        return false;
     }
     return true;
    }

    private DisConnect(GLAccount:GLAccountPM){
        var CurrencyGLAccount:GLAccountCurrencyPM = this.MappingAndGetCurrencyGlAccount(GLAccount);
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
         this.gLAccountCurrencyExtendedPMService.Put(CurrencyGLAccount).subscribe((response: ServiceResponse) => {
         this.CurrentSession.StopBusyIndicator();
        if (response) {
            if (!response.HasError) 
            {
                this.RemoveConnectedGLAccountCurrecyLine(GLAccount);
            }
         }
 
    });
    }
    ConfirmDisConnect(GLAccount:GLAccountPM) {
        let confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate('General.B.Ok');
        confirmWindow.NoButtonText = TextCodeTranslator.Translate('General.B.Cancel');

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.DisConnect(GLAccount);
            }
        });
        confirmWindow.Show(TextCodeTranslator.Translate('GLAccount.O.WantToDisconnect'));
    }
   
   private MappingAndGetCurrencyGlAccount(GLAccount:GLAccountPM):GLAccountCurrencyPM{
        var glaccountCurrency: GLAccountCurrencyPM = new GLAccountCurrencyPM(null);
            glaccountCurrency.CurrencyId =  GLAccount.CurrencyId;
            glaccountCurrency.MainGLAccountId = this.entityPM.Id;
            glaccountCurrency.GLAccountId =  GLAccount.Id;
            glaccountCurrency.Tenant = this.entityPM.Tenant;
    
            return glaccountCurrency;
    }

Connect(){
    if (this.entityPM.IsMultiCurrency) {
        this.ConnectSplitIfNotDirty();
    }
    else {
        this.ShowValidationMessageWindow("GLAccounts.O.MultiCurrencyForSplitted");
    }
}
    private ConnectSplitIfNotDirty() {
        if (!this.entityPM.IsDirty) {
            var windowArgs: any = {};
            windowArgs.EntityPM = this.entityPM;
            windowArgs.ConnectedGLAccounts = this.ConnectedGLAccounts.Collection;
            var windowTitle = TextCodeTranslator.Translate("GLAccount.O.ConnectToAnExisting");
            var logWindow = new LogitudeWindow();
            logWindow.Width = 400;
            logWindow.Height = 250;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.AddNewConnectedGLAccountCurrecyLine(comp);
                    }
                });
            });

            this.entityResourceService.getEntityResourceByTableName("GLAccountCurrency").subscribe((response: any) => {
                logWindow.Show('./Accounting/Components/EditTabs/GLAccount/ConnectWithGLAccountComponent');
            });
        }
        else {
            this.ShowValidationMessageWindow("GLAccounts.O.UnsavedChangesSaveBeforeContinue");
        }
    }

    Add() {

        if (this.entityPM.IsMultiCurrency) {
            this.AddSplitIfNotDirty();
        }
        else {
            this.ShowValidationMessageWindow("GLAccounts.O.MultiCurrencyForSplitted");
        }
    }

    private AddSplitIfNotDirty() {
        if (!this.entityPM.IsDirty) {
            var windowArgs: any = {};
            windowArgs.EntityPM = this.entityPM;
            var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewConnectedGLAccount");

            var logWindow = new LogitudeWindow();
            logWindow.Width = 400;
            logWindow.Height = 300;

            logWindow.Title = windowTitle;

            logWindow.ShowCloseButton = false;
            windowArgs.Parent = this;
            logWindow.WindowArgs = windowArgs;
            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        this.AddNewLine(comp);
                    }
                });
            });
            logWindow.Show('./Accounting/Components/EditTabs/GLAccount/NewConnectedGLAccountComponent');
        }
        else {
            this.ShowValidationMessageWindow("GLAccounts.O.UnsavedChangesSaveBeforeContinue");
        }
    }

    private ShowValidationMessageWindow(message: string) {
        var msg = new MessageWindow();
        msg.RTL = true;
        msg.Show(TextCodeTranslator.Translate(message));
    }

    AddNewLine(data:any) {
        if (data.accountPM) {
           // if (!this.entityPM.ConnectedItems) this.entityPM.ConnectedItems = "";
            this.ConnectedGLAccounts.Insert(new SplittedByCurrencyAccount(data.accountPM, this));
          //  this.entityPM.ConnectedItems = this.entityPM.ConnectedItems + data.accountPM.CurrencyCode + ",";
              this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }

    }

    AddNewConnectedGLAccountCurrecyLine(data:any) {
        if (data.SelectedGLAccount) {
            this.ConnectedGLAccounts.Insert(new SplittedByCurrencyAccount(data.SelectedGLAccount, this));
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }

    }
    RemoveConnectedGLAccountCurrecyLine(GLAccount:GLAccountPM) {
         if (GLAccount) {
            this.ConnectedGLAccounts.Remove(this.ConnectedGLAccounts.Collection.filter(s=>s.DisplayNumber == GLAccount.DisplayNumber)[0]);
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }

    }
    Choose() {
       
        var args: any = {};
        args.GLAccount = this.entityPM;
       
        
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;
         logitudeWindow.WindowArgs = args;
        logitudeWindow.Title= this.ObjectTableName + " Search";
        logitudeWindow.Show('./Accounting/Components/EditTabs/GLAccount/GLAccountSearchWindowComponent');

        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.OnSearchWindowClosed(comp);
                }
            });
        });
      
    }
    OnSearchWindowClosed(args: any) {
        if (args.ValidationErrorsList.length == 0) {

            this.BuildChildrenGLAccountsList();
        }
    }

  

    SetMouseHoverRow(item: GLAccountChild, isRowHover: boolean) {
        if (item) {
            item.IsRowHover = isRowHover;
        }
    }

}


export class SplittedByCurrencyAccount extends BaseComponent {
    entityPM: GLAccountPM;
    parent: GLAccountAdditionalDataTabComponent;
    GLAccountPMService: GLAccountPMService = new GLAccountPMService();
    DataContext: any = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entity: GLAccountPM, Parent: GLAccountAdditionalDataTabComponent) {
        super();
        this.entityPM = entity;
        this.parent = Parent;
        //this.DisplayNumber = Parent.entityPM.DisplayNumber + "\\" + this.CurrencyCode;
    }

    public get CurrencyCode()
    {
        return this.entityPM.CurrencyCode;
    }

    public get CurrencyId() {
        return this.entityPM.CurrencyId;
    }


    public get DisplayNumber() { return this.entityPM.DisplayNumber; }
    public set DisplayNumber(value: string) {
        this.entityPM.DisplayNumber = value;
    }

    public get BalanceInLocalCurrency() { return this.entityPM.BalanceInLocalCurrency; }
    public set BalanceInLocalCurrency(value: number) {
        this.entityPM.BalanceInLocalCurrency = value;
    }

    public get Inactive()
    {
        return this.entityPM.Inactive;
    }

    public set Inactive(value: boolean) {
        this.entityPM.Inactive = value;
    } 

    InactiveChecked(checked: boolean, balanceInLocalCurrency: number )
    {
     
        if (checked) {

            if (this.BalanceInLocalCurrency != null) {

                if (this.BalanceInLocalCurrency != 0) {
                    this.AddValidationErrorIfThereTransactionsConnectedToSplitGLAccount(balanceInLocalCurrency);
                    this.entityPM.Inactive = false;
                    this.entityPM.Type = "ACTIVE";
                }

                else {
                    this.entityPM.Inactive = true;
                    this.entityPM.Type = "INACTIVE";
                }
            }
        }
        else {
           
            this.entityPM.Inactive = false;
            this.entityPM.Type = "ACTIVE";
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.Saving"));
            
            this.GLAccountPMService.update(this.entityPM).subscribe((myResponse: ServiceResponse) => {

                if (myResponse) {
                    if (!myResponse.HasError) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }

                this.CurrentSession.StopBusyIndicator();

            });

    }

    private AddValidationErrorIfThereTransactionsConnectedToSplitGLAccount(balanceInLocalCurrency: number) {
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.GLABalanceNotEqual0"));
    }

    ConnectedGLAccountClicked(item: any) {
        this.EditEntity("GLAccount", this.entityPM.Id, null, "GAGC");

    }

    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {


        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe((res:any) => {


            this.parent.BuildChildrenGLAccountsList();
        });

    }

}

export class GLAccountChild extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    entityPM: GLAccountPM;
    parent: GLAccountAdditionalDataTabComponent;
    GLAccountPMService: GLAccountPMService = new GLAccountPMService();
    DataContext: any = this;
    IsRowHover: boolean;
    constructor(entity: GLAccountPM, Parent: GLAccountAdditionalDataTabComponent) {
        super();
        this.entityPM = entity;
        this.parent = Parent;
      
    }

    public get DisplayNumber() {
        return this.entityPM.DisplayNumber;
    }


    public get LocalName() {
        return this.entityPM.LocalName;
    }



    GLAccountHyperlinkClicked(item: any) {
        this.EditEntity("GLAccount", this.entityPM.Id, null, "GAGC");

    }

    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {


        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe((res:any) => {


            this.parent.BuildChildrenGLAccountsList();
        });

    }


    DisconnectClicked(item: any) {

        this.parent.glAccountExtendedListService.SetParentAccountId(this.entityPM.Id, "null," + this.entityPM.Id).subscribe((myResponse: ServiceResponse) => {

            if (myResponse) {
                if (!myResponse.HasError) {
                    this.parent.BuildChildrenGLAccountsList();
                }
            }

            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }





}



