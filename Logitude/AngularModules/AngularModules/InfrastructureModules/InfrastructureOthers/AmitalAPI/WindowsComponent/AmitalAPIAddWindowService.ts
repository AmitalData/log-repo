import { Injectable } from "@angular/core";
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
        const error = this.getValidationErrors(fields, data);
        return error.length === 0;
    }

    getValidationErrors(fields: TextBoxField[], data: any): fieldsError[] {
        const errors: fieldsError[] = [];

        fields.filter(f => data[f.name] === undefined).forEach(field => { data[field.name] = ''; });
        fields.forEach(field => (<UIProperties>data.UIProperties).SetRequired(field.name, null, field.type != 'boolean'));
        fields.forEach(field => {
            const value: any = data[field.name];
            const emptyValue: boolean = !!(value === undefined || value  === '' || value === null);
            const requiredError: boolean = field.type != 'boolean' && emptyValue && field.required;
            const typeError: boolean = field.type === 'number' && !emptyValue && isNaN(value);
            const error = requiredError || typeError;

            if (error) {
                const errorMessage: string =  TextCodeTranslator.Translate(requiredError ? 'Customs.General.O.RequiredFields' : 'General.O.InvalidInput');
                errors.push({ fieldName: field.name, error: errorMessage });
            }

            field.error = error;
            (<UIProperties>data.UIProperties).SetRequired(field.name, null, error);
        });

        return errors;
    }
}

export type fieldsError = {
    fieldName: string;
    error: string;
};
