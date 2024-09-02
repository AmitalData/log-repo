import { Injectable } from "@angular/core";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { TextBoxField } from "../components/LogTexBoxFormComponent";
import { CustomMessageProgressComponent } from "CustomsModules/CustomsControls/Components/CustomMessageProgressComponent";

@Injectable()
export class AmitalAPIAddWindowService {
    public async sendToServer(isUpdate: boolean, serverCall: Promise<any>): Promise<any> {
        SessionLocator.SelectedSession.StartBusyIndicator('');
        let res: any = null;
        let err: any = null;

        try {
            res = await serverCall;
        } catch (error) {
            err = error;
        }
        SessionLocator.SelectedSession.StopBusyIndicator();

        if (!res || err) {
            const title: string = isUpdate ? TextCodeTranslator.Translate('General.O.UpdateFailed') : TextCodeTranslator.Translate('General.O.AddFailed');
            err = err || TextCodeTranslator.Translate('General.B.Erroroccured');
            if (!(err instanceof String)) {
                try {
                    const parsedError = JSON.parse(err.error);
                    err = 'Message: ' + parsedError.Message + ', InnerMessage: ' + parsedError.InnerMessage;
                } catch (error) {
                    err = JSON.stringify(err);
                }
            }

            CustomMessageProgressComponent.ShowCustomMessageProgressComponent(title, err, () => { })
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
