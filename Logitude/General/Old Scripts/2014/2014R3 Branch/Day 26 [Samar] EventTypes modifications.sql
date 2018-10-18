
update EventTypes 
set iscustomerview = 1, IsSharedLogisticsEnabled = 1
where (code = 'DELY' or code = 'BKD' or code = 'DAMG' or code = 'CUDL' or code = 'CUIN' 
or code = 'PICD' or code = 'RCS' or code = 'T2AR' or code = 'T3AR' or code = 'PRCA' or code = 'ARR' or code = 'T1AR' or code = 'ONCA'
or code = 'T2DP' or code = 'T1DP' or code = 'DEP' or code = 'ONCD' or code = 'T3DP' or code = 'PRCD' or code = 'CCD' or code = 'PIOD' or code = 'DELD' or code = 'ITR'
or code = 'RSH' or code = 'TRG' or code = 'STR')

update EventTypes
set IsCustomerView = 0
where IsSharedLogisticsEnabled = 0