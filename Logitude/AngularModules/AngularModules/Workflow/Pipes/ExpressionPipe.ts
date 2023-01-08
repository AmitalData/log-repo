import { Pipe, PipeTransform } from "@angular/core";
import { Formatter } from "Workflow/Models/Formatter";

@Pipe({
    name: "ExpressionPipe"
})

export class ExpressionPipe implements PipeTransform {

    transform(expression: string, objectFields: any) {
        if (expression) {
            let expressionVariables = expression.match(/\{(.*?)\}/g);
            if (expressionVariables && expressionVariables.length > 0) {
                expressionVariables.forEach(expressionVariable => {
                    if (expressionVariable && expressionVariable !== "") {
                        let variableCode = expressionVariable.replace("{", "").replace("}", "");

                        let fieldCode = Formatter.getFieldCode(variableCode);

                        let customObjectField = objectFields ?
                            (objectFields.find((o: any) => o.FieldCode === fieldCode && o.IsCustom) || null) :
                            null;

                        let customObjectFieldName = customObjectField ?
                            (customObjectField.FullNameTextCodeDefaultText || null) : null;

                        if (customObjectField && customObjectFieldName) {
                            let formattedExpressionVariable = expressionVariable.replace(fieldCode, customObjectFieldName.replace(/\ /gi, ""));
                            expression = expression.replace(expressionVariable, formattedExpressionVariable);
                        }
                    }
                });
            }
        }
        return expression;
    }

}