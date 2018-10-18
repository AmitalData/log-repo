
DECLARE 

   c_id Declarations.Id%type;
   c_Tenant Declarations.Tenant%TYPE;
 c_number CONSIGNMENTS.MANIFESTNUMBER%TYPE;
   
  
  CURSOR DeclarationHawb  is
     SELECT  Id, Tenant from Declarations; 
BEGIN
OPEN DeclarationHawb;
   LOOP
      FETCH DeclarationHawb into c_id, c_Tenant;
      EXIT WHEN DeclarationHawb%notfound;
      
      BEGIN

 select  ManifestNumber into c_number from CONSIGNMENTS where declarationId = c_id AND ROWNUM <= 1 ;
 update Declarations set CourierHAWB  = c_number;

END;
      END LOOP;
 
      CLOSE DeclarationHawb;
      
     END;
     
     