


CREATE TABLE SupplierInvoiceItemsCerTemp ( DeclarationId VARCHAR2(15 CHAR) NOT NULL,
InvoiceCounterKey NUMBER(10,0) NOT NULL, 
LineNumber NUMBER(10,0) NOT NULL,
ItemCertificateCounterKey NUMBER(10,0) NOT NULL, 
CertificateNumber VARCHAR2(35 CHAR) , 
Tenant NUMBER(10,0) NOT NULL, 
ReqConfirmationTypeCode VARCHAR2(4 CHAR) , 
CertificateExemptionTypeCode VARCHAR2(3 CHAR) , 
AttachmentTypeCode VARCHAR2(3 CHAR) , 
ResConfirmationTypeCode VARCHAR2(4 CHAR) , 
CustomsAttachmentID VARCHAR2(35 CHAR) , 
SequenceNumeric NUMBER(10,0) NOT NULL )
  ;
create or replace PROCEDURE usp_CopySuppInvoiceItemCers(
    v_SourceDeclarationId IN VARCHAR2,
    v_TargetDeclarationId IN VARCHAR2,
    v_Tenant              IN NUMBER,
    cv_1 OUT SYS_REFCURSOR )
AS
BEGIN
delete from SupplierInvoiceItemsCerTemp;
  INSERT
  INTO SupplierInvoiceItemsCerTemp(
      DeclarationId
           ,InvoiceCounterKey
           ,LineNumber
           ,ItemCertificateCounterKey
           ,CertificateNumber
           ,Tenant
           ,ReqConfirmationTypeCode
           ,CertificateExemptionTypeCode
           ,AttachmentTypeCode
           ,ResConfirmationTypeCode
           ,CustomsAttachmentID
           ,SequenceNumeric
    )
    (SELECT   DeclarationId
           ,InvoiceCounterKey
           ,LineNumber
           ,ItemCertificateCounterKey
           ,CertificateNumber
           ,Tenant
           ,ReqConfirmationTypeCode
           ,CertificateExemptionTypeCode
           ,AttachmentTypeCode
           ,ResConfirmationTypeCode
           ,CustomsAttachmentID
           ,SequenceNumeric
      FROM SupplierInvioceItemCertificats
      WHERE DeclarationId = v_SourceDeclarationId
      AND Tenant          = v_Tenant
    );
  UPDATE SupplierInvoiceItemsCerTemp SET DeclarationId = v_TargetDeclarationId;
  OPEN cv_1 FOR SELECT * FROM SupplierInvoiceItemsCerTemp ;
  INSERT
  INTO SupplierInvioceItemCertificats(
      DeclarationId
           ,InvoiceCounterKey
           ,LineNumber
           ,ItemCertificateCounterKey
           ,CertificateNumber
           ,Tenant
           ,ReqConfirmationTypeCode
           ,CertificateExemptionTypeCode
           ,AttachmentTypeCode
           ,ResConfirmationTypeCode
           ,CustomsAttachmentID
           ,SequenceNumeric
    )
    ( SELECT   DeclarationId
           ,InvoiceCounterKey
           ,LineNumber
           ,ItemCertificateCounterKey
           ,CertificateNumber
           ,Tenant
           ,ReqConfirmationTypeCode
           ,CertificateExemptionTypeCode
           ,AttachmentTypeCode
           ,ResConfirmationTypeCode
           ,CustomsAttachmentID
           ,SequenceNumeric FROM SupplierInvoiceItemsCerTemp
    );

END;