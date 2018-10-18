delete from PackageFeatures where FeatureId = (select Id from Features where Code = 'Ticket.Menu')
delete from RoleFeatures where FeatureId = (select Id from Features where Code = 'Ticket.Menu')
delete from Features where Code = 'Ticket.Menu'