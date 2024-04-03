export interface CreateNewExportDeclarationDetails {
    ExportFileNumber: string,
    Customer: string,
    Partner: string,
    DeclarationOfficeHandlCode: string, //בית מכס מטפל
    ExportDeclarationOfficeCode: string, //בית מכס מייצא
    ExporterNumber: string, // מספר יצואן 
    DeclarationTypeCode: string,//סוג הצהרה
    ProcedureCurrentCode: string,//   קוד סוג תהליך
    TransferExporterCode: string,//  קוד סוג יצואן מעביר
    DeclarationDocumentTypeCode: string,// סוג הצהרה קשורה
    DeclarationDocumentId: string,// מספר הצהרה קשורה
    DestinationCountryCode: string,// ארץ יעד
    AutonomyRegionTypeCode: string,// קוד איזור אוטונומיה
    IsExporterConfirmation: string,// אישור יצואן
    RecipientName: string,//שם מקבל
    RecipientAddress: string,//כתובת מקבל
    RecipientIssueCountryCode: string,//מדינת המקבל
    TransportType: string,// סוג משלוח
    CargoType: string,// מזהה מטען
    FirstCargoID: string,//  מזהה מטען ראשון
    SecondCargoID: string,//מזהה מטען שני
    ThirdCargoID: string,//מזהה מטען שלישי
    FinalDestinationPortCode: string,// קוד נמל יעד
    LoadingPortCode: string,// נמל טעינה
    UnloadingPortCode: string,//נמל פריקה
    CargoDescription : string,//תאור טובין
    StorageSite: string,// אתר מסירה
    RecieverWareHouse: string,// קוד אתר המכלה
    InternalTransition: string,// מעבר פנימי
    IsDangerousGoods: string,// חומר מסוכן
    CheckBox: string,// שדר ללא משגור
    Save: string,



}

