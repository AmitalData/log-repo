import { FullAccountingSettingList } from "Accounting/EntityLists/FullAccountingSettingList";
import { GLAccountList } from "Accounting/EntityLists/GLAccountList";
import { FullAccountingSettingListService } from "Accounting/Services/StandardLists/FullAccountingSettingListService";
import { GLAccountListService } from "Accounting/Services/StandardLists/GLAccountListService";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

export class GLAccountSecurityLevelService{

    public static CheckLevel(glaccountId: string){

        return new Promise(resolve =>
        {
            var settingsService = new FullAccountingSettingListService();
            settingsService.getSingle(SessionLocator.Tenant+'').subscribe((response: any) =>
            {
                var settings = response.Result;
                if (settings.IsSecurityLevelActivated) {
                    var loggedUserSecurityLevel = SessionLocator.LoggedUserPM.SecurityLevel || 1;
                    var glaccountService = new GLAccountListService();
                    glaccountService.getSingle(glaccountId).subscribe((response: any) =>
                    {
                        var glaccount = response.Result;
                        var hasAccess = (glaccount.ChartOfAccountSecurityLevel <= loggedUserSecurityLevel || glaccount.ChartOfAccountSecurityLevel == null);
                        resolve(hasAccess);
                    });
                }else{
                    resolve(true);
                }
            });
        });
    }

    public static OpenGLAccountEditWindow(entityId: string, defaultSelectedTabCode: string = 'GATR') {

        GLAccountSecurityLevelService.CheckLevel(entityId)
        .then(hasAccess=>{

            if(hasAccess){
                GLAccountSecurityLevelService.OpenGLaccountWindow(entityId, defaultSelectedTabCode);
            }else{
                GLAccountSecurityLevelService.ShowSecurityBockingMessage();
            }
        });
    }

    private static OpenGLaccountWindow(entityId: string, defaultSelectedTabCode: string)
    {
        var editWindow = new LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, 'GLAccount', defaultSelectedTabCode);
    }

    public static ShowSecurityBockingMessage()
    {
        var messageWindow = new MessageWindow();
        messageWindow.Width = 460;
        messageWindow.Height = 190;
        messageWindow.Title = TextCodeTranslator.Translate("General.O.Warning");
        // messageWindow.ShowIcon = false;
        messageWindow.Show(TextCodeTranslator.Translate("GLAccount.O.SecurityLevelHiddenItem"));
    }
}
