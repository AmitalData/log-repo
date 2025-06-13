using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class RoleFeatureRepository:IRepository<RoleFeature,string>
    {
        IAmitalCloudContext currentContext;

        public RoleFeatureRepository()
        {
            currentContext = new AmitalCloudContext();
        }

        public RoleFeatureRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }

        public RoleFeatureRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }

        public RoleFeature GetSingleRoleFeature(string id)
        {
            return (from a in context.RoleFeatures
                   where a.Id == id
                   select a).FirstOrDefault();
        }

        public RoleFeature GetRoleFeatureByRoleAndFeature(string roleId, string featureId, int tenant)
        {
            return (from a in context.RoleFeatures
                    where a.RoleId == roleId && a.FeatureId == featureId && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public RoleFeature GetRoleFeatureByRoleAndFeatureUCode(string roleId, string FeatureUniqeCode, int tenant)
        {
            return (from a in context.RoleFeatures
                    where a.RoleId == roleId && a.FeatureUniqeCode == FeatureUniqeCode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public RoleFeature GetBusinessUnitFilterRoleFeature(string myRoleId, string myFeatureId, int tenant)
        {
            RoleFeature myResult = null;

            Role myRole = (from d in context.Roles where d.Id == myRoleId select d).FirstOrDefault();

            if (myRole != null)
            {
                if (myRole.IsCustomRole)
                {
                    myResult = (from d in context.RoleFeatures
                                where
                                d.RoleId == myRoleId
                                && d.FeatureId == myFeatureId
                                && (d.Tenant == tenant || d.Tenant == 0)
                                select d).FirstOrDefault();

                    if (myResult != null)
                    {
                        if (myResult.IsDeleted)
                        {
                            myResult = null;
                        }
                    }

                    else
                    {
                        myResult = (from d in context.RoleFeatures
                                    where                                    
                                    d.RoleId == myRole.ParentRoleId
                                    && d.FeatureId == myFeatureId
                                    && (d.Tenant == tenant || d.Tenant == 0)
                                    select d).FirstOrDefault();
                    }
                }

                else
                {
                    myResult = (from d in context.RoleFeatures
                                where
                                d.RoleId == myRoleId
                                && d.FeatureId == myFeatureId
                                && (d.Tenant == tenant || d.Tenant == 0)
                                select d).FirstOrDefault();
                }
            }

            return myResult;
        }

        public IQueryable<RoleFeature> GetRoleFeaturesByTenant(int tenant)
        {
            return (from a in context.RoleFeatures
                    where a.Tenant == tenant
                    select a);
        }

        public List<RoleFeature> GetRoleFeatureByRoleIds(List<string> myRolesIds, int tenant)
        {
            List<RoleFeature> myResult = new List<RoleFeature>();

            if (myRolesIds.Count > 0)
            {
                myResult = (from a in context.RoleFeatures
                            where myRolesIds.Contains(a.RoleId) && (a.Tenant == tenant || a.Tenant == 0)
                            select a).ToList();
            }
            return myResult;
        }

        public void Add(RoleFeature entity)
        {
            context.RoleFeatures.Add(entity);
        }

        public void Remove(RoleFeature entity)
        {
            context.RoleFeatures.Attach(entity);
            context.RoleFeatures.Remove(entity);
        }

        public void Update(RoleFeature entity)
        {
            context.RoleFeatures.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RoleFeature> All()
        {
            return context.RoleFeatures.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<RoleFeature> GetMulti(IEntityKeyFields<RoleFeature,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public RoleFeature GetSingle(IEntityKeyFields<RoleFeature,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAllRoleFeaturesByRole(string roleId)
        {
            List<RoleFeature> roleFeatures = (from a in context.RoleFeatures
                                              where a.RoleId == roleId
                                                    select a).ToList();
            foreach (RoleFeature roleFeature in roleFeatures)
            {
                this.Remove(roleFeature);
            }
        }

    }
}