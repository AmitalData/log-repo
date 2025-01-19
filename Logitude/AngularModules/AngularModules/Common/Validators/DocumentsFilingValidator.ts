import { DocumentsFilingPM } from "Common/EntityPMs/DocumentsFilingPM";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

const objectTableName = "DocumentsFiling";
export class DocumentsFilingValidator {
    public Validate(documentsFilingPM: DocumentsFilingPM): string[] {
        let errors: string[] = [];

        if ((<any>documentsFilingPM)._isDFComponent)
            errors = errors.concat(this.validateDFComponent(documentsFilingPM));

        return errors;
    }

    private validateDFComponent(documentsFilingPM: DocumentsFilingPM): string[] {
        const msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        const errors: string[] = [];

        documentsFilingPM.DocumentsFilingMetaDataValues && documentsFilingPM.DocumentsFilingMetaDataValues.forEach((item) => {
            const isRequired: boolean = (<any>item).Mandatory && (item.MetaDataValue == undefined || item.MetaDataValue == null || item.MetaDataValue == "");
            
            ['', ''].forEach(() => {
                (<any>item).UIProperties.SetRequired('MetaDataValue', 'DocumentsFilingMetaDataValue', isRequired)
        });
            if (isRequired)
                errors.push(msg.replace("%FieldName", (<any>item).DocumentsMetaDataTypeEnglishName))
        });

        documentsFilingPM.UIProperties.SetRequired("DocumentTypeId", objectTableName, !documentsFilingPM.DocumentTypeId);
        if (!documentsFilingPM.DocumentTypeId) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Customs.CustomsDocument.F.DocumentTypeCode")));
        }

        if (!documentsFilingPM.FileName)
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Customs.CustomsDocument.AddDocumentsTicket")));

        return errors;
    }
}
