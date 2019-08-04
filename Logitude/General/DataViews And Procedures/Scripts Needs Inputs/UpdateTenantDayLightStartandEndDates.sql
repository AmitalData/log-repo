--select DayLightStartDate,DayLightEndDate,DayLightOffset,* from Tenants 

declare @dayLightStartDate as datetime;
declare @dayLightEndDate as datetime;
declare @dayLightOffset as int;

set @dayLightStartDate = '2019-03-29'
set @dayLightEndDate = '2019-10-27'
set @dayLightOffset = 1
 
Update Tenants set DayLightStartDate = @dayLightStartDate,DayLightEndDate = @dayLightEndDate,DayLightOffset = @dayLightOffset
