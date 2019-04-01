import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ARInvoiceStockPM } from '../../EntityPMs/ARInvoiceStockPM';
import { MenuButtonPM } from '../../../Infrastructure/EntityPMs/MenuButtonPM'
declare var window: any;

export class ARInvoiceStockMenuButtonsHandler {
    public EntityPM: ARInvoiceStockPM;
    public entityArgs: EntityArgs
    private status: boolean = false;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        //this.InitializeServices();
        //this.Listen();
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'ARInvoiceStock')[0];

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "PrintARPayment":
                            {

                                break;
                            }
                    }
                }
            }
        }
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "PrintARPayment": {
                
                break;
            }            
        }
    }
}
