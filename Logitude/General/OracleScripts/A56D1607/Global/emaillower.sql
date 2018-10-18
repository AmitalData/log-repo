create table z1607globalcontacts AS SELECT * FROM globalcontacts ;

set serveroutput on;

declare    
   cursor c1 is
   select globalcontacts.*,bad.semail  from globalcontacts,
(select count(*) ,globaltenantid ,lower(email ) semail
From globalcontacts  --where email != lower(email);
group by globaltenantid ,lower(email )
having count(*) >1 )  bad
where lower(globalcontacts.email) = bad.semail ;

 total_val number(6);
 
 last_semail globalcontacts.email%type;

begin
total_val := 0;

   FOR contacts in c1
   LOOP
      if (contacts.semail=last_semail) then
        contacts.email:= contacts.email || total_val;
        update globalcontacts set   globalcontacts.email =contacts.email where globalcontacts.id= contacts.id;
      else
        null;
      end if;
      
      total_val:=total_val+1;      
      last_semail :=contacts.semail;
      
      DBMS_OUTPUT.PUT_LINE(contacts.email );
   END LOOP;

END;
/
--rollback ;
COMMIT;

/



---update contacts set email=lower(email) ;
update globalcontacts set email = lower(email) ;
update contactpasswords set email = lower(email) ;


commit 
/

