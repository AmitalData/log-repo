
-- run on main database
update contacts set contacts.email = RTRIM(contacts.Email)
update contacts set contacts.email = LTRIM(contacts.Email)

select * from contacts where email like(' %') or email like('% ') 

-- run on global database
update globalcontacts set globalcontacts.email = LTRIM(globalcontacts.Email)
update globalcontacts set globalcontacts.email = RTRIM(globalcontacts.Email)

update ContactPasswords set ContactPasswords.email = LTRIM(ContactPasswords.Email)
update ContactPasswords set ContactPasswords.email = RTRIM(ContactPasswords.Email)


select * from globalcontacts where email like(' %') or email like('% ')  
select * from ContactPasswords where email like(' %') or email like('% ')  


