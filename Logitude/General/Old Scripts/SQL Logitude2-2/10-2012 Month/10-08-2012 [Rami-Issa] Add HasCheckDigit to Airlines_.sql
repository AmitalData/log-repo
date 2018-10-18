--Add New Column
alter table airlines
add HasCheckDigit bit;

--Add NOT NULL Constraint
--alter table airlines 
--with nocheck 
--add constraint HasCheckDigit_NOTNULL 
--Check (HasCheckDigit is not null)



--update previous data for this field
update Airlines
set HasCheckDigit = 'true'

--Set TRUE as the Default value for future HasCheckDigit values
alter table airlines
add constraint HasCheckDigit_airlines_Default_True 
default 'true' 
for HasCheckDigit




