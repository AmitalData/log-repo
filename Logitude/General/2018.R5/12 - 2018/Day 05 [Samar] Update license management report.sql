

update Reports set ReportGroupId = (select Id from ReportGroups where Code ='ADMN') where Code = 'LICM'