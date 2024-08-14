import { Injectable } from "@angular/core";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { TextBoxField } from "../components/LogTexBoxFormComponent";

@Injectable()
export class AmitalAPIAddWindowService {
    public async sendToServer(isUpdate: boolean, serverCall: Promise<any>): Promise<any> {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        let res: any = null;

        try {
            res = await serverCall;
        } catch (error) { }
        SessionLocator.SelectedSession.StopBusyIndicator();

        if (!res) {
            const win = new MessageWindow();
            win.ShowErrorIcon = true;
            win.Show(isUpdate ? TextCodeTranslator.Translate('General.O.UpdateFailed') : TextCodeTranslator.Translate('General.O.AddFailed'))
        }

        return res;
    }

    chekFormValidation(fields: TextBoxField[], data: any): boolean {        
        let hasError = false;
        fields.filter(f => data[f.name] === undefined).forEach(field => { data[field.name] = ''; });
        fields.forEach(field => (<UIProperties>data.UIProperties).SetRequired(field.name, null, field.type != 'boolean'));
        fields.forEach(field => {
            const required = field.type != 'boolean' && (data[field.name] === undefined || data[field.name] === '' || data[field.name] === null);
            (<UIProperties>data.UIProperties).SetRequired(field.name, null, required);
            field.error = required;
            hasError = hasError || required;
        });
        return !hasError;
    }
}
