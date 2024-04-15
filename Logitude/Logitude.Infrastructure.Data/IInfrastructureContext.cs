using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data; 
using Logitude.Infrastructure.Data.EntityMapping;

namespace Logitude.Infrastructure.Data
{

    public interface IInfrastructureContext : IContext
    {
   
       	 IDbSet<AuditLog> AuditLogs { get; }
		 IDbSet<BatchTaskExecution> BatchTaskExecutions { get; }
		 IDbSet<BatchTaskExecutionStatus> BatchTaskExecutionStatus { get; }
		 IDbSet<BIFoldersPermission> BIFoldersPermissions { get; }
		 IDbSet<BIReport> BIReports { get; }
		 IDbSet<BIReportFolder> BIReportFolders { get; }
		 IDbSet<BIReportsExecutionLog> BIReportsExecutionLogs { get; }
		 IDbSet<BIReportsType> BIReportsTypes { get; }
		 IDbSet<BusinessProcessQueue> BusinessProcessQueues { get; }
		 IDbSet<BusinessRole> BusinessRoles { get; }
		 IDbSet<ContainerSetting> ContainerSettings { get; }
		 IDbSet<DigitalFieldSecurity> DigitalFieldSecurities { get; }
		 IDbSet<DigitalPortalLanguage> DigitalPortalLanguages { get; }
		 IDbSet<DigitalPortalScreen> DigitalPortalScreens { get; }
		 IDbSet<DigitalPreDefinedComponent> DigitalPreDefinedComponents { get; }
		 IDbSet<DigitalProfile> DigitalProfiles { get; }
		 IDbSet<DigitalTextCode> DigitalTextCodes { get; }
		 IDbSet<FeatureToggle> FeatureToggles { get; }
		 IDbSet<LastRunDetail> LastRunDetails { get; }
		 IDbSet<LBPTeamMember> LBPTeamMembers { get; }
		 IDbSet<PriceStep> PriceSteps { get; }
		 IDbSet<SatisfactionSurvey> SatisfactionSurveys { get; }
		 IDbSet<SharedLogisticsSetting> SharedLogisticsSettings { get; }
		 IDbSet<Team> Teams { get; }
		 IDbSet<TeamMemberBusinessRole> TeamMemberBusinessRoles { get; }
		 IDbSet<Toggle> Toggles { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}