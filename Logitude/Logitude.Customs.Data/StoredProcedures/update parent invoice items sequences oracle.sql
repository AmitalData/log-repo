create or replace PROCEDURE usp_UpdateParentInvoiceItemSeq(
    v_DeclarationId IN VARCHAR2 DEFAULT NULL ,
    v_Tenant        IN NUMBER DEFAULT NULL ,
    v_CounterKey    IN NUMBER DEFAULT NULL )
AS
  
  begin
  DECLARE
  
  v_SequenceNumeric NUMBER(10,0);
  v_LineNumber      NUMBER(10,0);
  
    CURSOR InvoiceItemsCursor
    IS
      SELECT LineNumber
      FROM SupplierInvoiceItems
      WHERE DeclarationId =v_DeclarationId
      AND CounterKey      =v_CounterKey
      AND Tenant          =v_Tenant
	  AND IsParent=1
      ORDER BY ORDERBYLINENO;
  BEGIN
    

    OPEN InvoiceItemsCursor;
    LOOP
    FETCH InvoiceItemsCursor INTO v_LineNumber;
     EXIT WHEN InvoiceItemsCursor%notfound;
      BEGIN
      IF v_SequenceNumeric is null then
      begin
     
         v_SequenceNumeric :=0;
         END;
        END IF;
  v_SequenceNumeric := v_SequenceNumeric + 1 ;

        UPDATE SupplierInvoiceItems
        SET SequenceNumeric = v_SequenceNumeric
        WHERE DeclarationId = v_DeclarationId
        AND LineNumber      = v_LineNumber
        AND CounterKey      = v_CounterKey;
        
      END;
    END LOOP;
    CLOSE InvoiceItemsCursor;
  END;
   END;