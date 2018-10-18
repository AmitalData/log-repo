    SET SERVEROUTPUT ON
 begin
  DECLARE

 c_id PaymentOrders.Id%type;
   c_Tenant PaymentOrders.Tenant%TYPE;
   type t_connections is table of  PaymentOrderConnectionTables.PaymentOrderId%TYPE;
   c_paymentOrderId PaymentOrderConnectionTables.PaymentOrderId%TYPE;
   c_ConnectedEntityId PaymentOrderConnectionTables.ConnectedEntityId%TYPE;
   c_count integer := 0;
  c_AccountingCustomFile  VARCHAR2(30);
  c_customFiles VARCHAR2(30);
  
   CURSOR PaymentOrderCursor
    IS
      SELECT  Id, tenant
      FROM PaymentOrders;
     
  BEGIN
  
  OPEN PaymentOrderCursor;
  LOOP
  FETCH PaymentOrderCursor INTO c_id, c_Tenant;
  EXIT WHEN PaymentOrderCursor%notfound;
  
  begin

  select count(*)  into c_count from PaymentOrderConnectionTables where PaymentOrderId=c_id  and Tenant = c_Tenant and  ConnectedEntityCode = 'D';
  
          if(c_count >0) then
           begin
           select  ConnectedEntityId into c_ConnectedEntityId from PaymentOrderConnectionTables where PaymentOrderId = c_id and   ROWNUM =1 order by ConnectedEntityId  ASC;
           end;
           end if;
           
           
            if(c_count =1 )  and (c_ConnectedEntityId is not null)  then
  
                 begin
                 select customfileNo into c_customFiles from Declarations where Id =c_ConnectedEntityId and Tenant = c_Tenant;	    
              	 update PaymentOrders set   CustomFiles =c_customFiles  where id = c_id;
               
                end;
             
               
               else if (c_count >1)   and (c_ConnectedEntityId is not null)  then 
                  begin
                      select customfileNo into c_customFiles from Declarations where Id =c_ConnectedEntityId and Tenant = c_Tenant;	    
                  update PaymentOrders  set CustomFiles =CONCAT( c_customFiles ,'*' )   where  id = c_id;  
               --    Dbms_Output.Put_Line(c_ConnectedEntityId);
                   end;
                   end if;

    
      --   Dbms_Output.Put_Line(c_ConnectedEntityId);    
 
  end if;
  end;
    END LOOP;
    CLOSE PaymentOrderCursor;
  END;
   END;
   