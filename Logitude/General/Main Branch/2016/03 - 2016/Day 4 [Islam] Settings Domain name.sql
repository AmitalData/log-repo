update Settings set DomainName = 'logitudeworld.com' where DeploymentStage = 'Simplog'  or DeploymentStage = 'dev' or DeploymentStage = 'amitalstorage'
update Settings set DomainName = 'logbox.co.il' where DeploymentStage = 'logboxwe1' 


update Settings set ProductName = 'Logitude' where DeploymentStage = 'Simplog'  or DeploymentStage = 'dev' or DeploymentStage = 'amitalstorage'
update Settings set ProductName = 'Logbox' where DeploymentStage = 'logboxwe1' 



update Settings set EmailAlertSignature = 'Created By <b>Logitude World</b>' where DeploymentStage = 'Simplog'  or DeploymentStage = 'dev' or DeploymentStage = 'amitalstorage'
update Settings set EmailAlertSignature = 'Created By <b>Unifreight Cloud Services</b>' where DeploymentStage = 'logboxwe1' 




--if (LogitudeSettings.WorkEnvironment == "cloud")
--                EnvelopeHtmlTemplate.Append("Created By <b>Unifreight Cloud Services</b>");
--            else
--                EnvelopeHtmlTemplate.Append("Created By <b>Logitude World</b>");