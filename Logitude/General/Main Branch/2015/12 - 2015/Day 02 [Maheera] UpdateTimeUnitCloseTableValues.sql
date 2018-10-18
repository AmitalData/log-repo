update SLALines set FirstResponseTimeUnit = 'OO' where FirstResponseTimeUnit = 'YY'
update SLALines set ResolveWithinTimeUnit = 'OO' where ResolveWithinTimeUnit = 'YY'
delete from TimeUnits where code = 'YY'