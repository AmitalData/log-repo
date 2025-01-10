import { DocumentsFilingPM } from "Common/EntityPMs/DocumentsFilingPM";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";

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
        delete documentsFilingPM.UIProperties;        

        documentsFilingPM.DocumentsFilingMetaDataValues && documentsFilingPM.DocumentsFilingMetaDataValues
            .filter(item => (<any>item).Mandatory && (
                item.MetaDataValue == undefined ||
                item.MetaDataValue == null ||
                item.MetaDataValue == "")
            )
            .forEach((item) => errors.push(msg.replace("%FieldName", (<any>item).DocumentsMetaDataTypeEnglishName)));

        if (!documentsFilingPM.DocumentTypeId)
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Customs.CustomsDocument.F.DocumentTypeCode")));

        if (!documentsFilingPM.FileName)
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Customs.CustomsDocument.AddDocumentsTicket")));
        
        return errors;
    }
}