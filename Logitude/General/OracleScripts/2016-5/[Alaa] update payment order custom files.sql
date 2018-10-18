SET SERVEROUTPUT ON
DECLARE 

   c_id PaymentOrders.Id%type;
   c_Tenant PaymentOrders.Tenant%TYPE;
   type t_connections is table of  PaymentOrderConnectionTables.PaymentOrderId%TYPE;
   c_paymentOrderId PaymentOrderConnectionTables.PaymentOrderId%TYPE;
   c_ConnectedEntityId PaymentOrderConnectionTables.ConnectedEntityId%TYPE;
   c_count integer := 0;
  c_AccountingCustomFile  VARCHAR2(30);
  c_customFiles VARCHAR2(30);
  
  CURSOR PaymentOrderConnections  is
     SELECT  Id, Tenant from PaymentOrders; 
BEGIN
OPEN PaymentOrderConnections;
   LOOP
      FETCH PaymentOrderConnections into c_id, c_Tenant;
      EXIT WHEN PaymentOrderConnections%notfound;
      
      BEGIN
      
      
        select count(*) into c_count  from PaymentOrderConnectionTables where PaymentOrderId =c_id and Tenant = c_Tenant and ConnectedEntityCode = 'D';


 
   -- select AccountingCustomFile into c_AccountingCustomFile from NewPaymentOrders where Id = c_id and Tenant = c_Tenant;


  select  ConnectedEntityId into c_ConnectedEntityId from PaymentOrderConnectionTables where PaymentOrderId = c_id AND ROWNUM <= 1  ORDER BY ConnectedEntityId ASC ;

Dbms_Output.Put_Line('im before if count');
if(c_count > 1) then 
BEGIN
  Dbms_Output.Put_Line('im >1');
		 update PaymentOrders  set CustomFiles = ((select customfileNo from Declarations where Id = c_ConnectedEntityId and Tenant =c_Tenant) + '*') where id = c_id;
		END;
	
  	elsif (c_count = 1) then
    BEGIN
    Dbms_Output.Put_Line(c_Tenant);

    select customfileNo into c_customFiles from Declarations where Id =c_ConnectedEntityId and Tenant = c_Tenant;
	    
  	 update PaymentOrders set 
     CustomFiles =c_customFiles  where id = c_id;
     
  
 
END;

      END  IF;
      END;
      
      END LOOP;
 
      CLOSE PaymentOrderConnections;
      
     END;
     