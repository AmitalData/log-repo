create or replace procedure clob2vc
  ( ptab in user_tables.table_name%type 
    , pcol in user_tab_columns.column_name%type
    , pcol_size in number
    , migrate_data in boolean := true )
is
begin
    if migrate_data
    then
        execute immediate 'alter table '||ptab
                    ||' add tmp_col varchar2('|| pcol_size|| ')';
        execute immediate             
                    'update '||ptab
                    ||' set tmp_col = substr('||pcol||',1,'||pcol_size||')';
    end if;
    execute immediate 'alter table '||ptab
                ||' drop column '|| pcol;

    if migrate_data
    then
        execute immediate 'alter table '||ptab
                    ||' rename column tmp_col to '|| pcol;
    else
        execute immediate 'alter table '||ptab
                    ||' add '||pcol||' varchar2('|| pcol_size|| ')';
    end if;
end;
/