import { Pipe, PipeTransform } from "@angular/core";
import { Formatter } from "Workflow/Utilities/Formatter";
import { ObjectFields } from "Workflow/Utilities/ObjectFields";
import { ObjectTables } from "Workflow/Utilities/ObjectTables";

@Pipe({
    name: "ExpressionPipe"
})

export class ExpressionPipe implements PipeTransform {

    transform(expression: string) {
        if (expression) {
            let expressionVariables = expression.match(/\{(.*?)\}/g);
            if (expressionVariables && expressionVariables.length > 0) {
                expressionVariables.forEach(expressionVariable => {
                    if (expressionVariable && expressionVariable !== "") {
                        let variableCode = expressionVariable.replace("{", "").replace("}", "");

                        let fieldCode = Formatter.getFieldCode(variableCode);

                        let customObjectField = ObjectFields.getAll().find((o: any) => o.FieldCode === fieldCode && o.IsCustom) || null

                        let customObjectFieldName = customObjectField ?
                            (customObjectField.FullNameTextCodeDefaultText || null) : null;

                        if (customObjectField && customObjectFieldName) {
                            let objectTableDisplayName = ObjectTables.getDisplayNameById(customObjectField.ObjectTableId);
                            if (objectTableDisplayName) {
                                objectTableDisplayName = Formatter.removeSpaces(objectTableDisplayName);
                                let formattedExpressionVariable = expressionVariable.replace(fieldCode, (objectTableDisplayName + "." + customObjectFieldName.replace(/\ /gi, "")));
                                expression = expression.replace(expressionVariable, formattedExpressionVariable);
                            }
                        }
                    }
                });
            }
        }
        return expression;
    }

}