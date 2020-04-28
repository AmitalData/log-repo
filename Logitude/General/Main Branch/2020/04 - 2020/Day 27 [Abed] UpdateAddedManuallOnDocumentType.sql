update DocumentTypes set AddedManually =1 where tenant !=0 and  Code not in (select code from DocumentTypes where Tenant = 0)  

