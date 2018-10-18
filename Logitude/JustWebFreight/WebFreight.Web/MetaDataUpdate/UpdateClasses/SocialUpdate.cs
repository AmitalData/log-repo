using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.UpdateClasses
{
    public class SocialUpdate
    {

        //Feed
        //    FollowEntity
        //    Follower
        //    Group
        //    Post
        //    PostLike
        //    GroupMember

        //public void LoadRolesAndFeatures(int tenant)
        //{

        //    ICommonDataContext ObjectContext = CommonDataContext.GetContext(tenant);
        //    FeatureRepository FeaturesRepository = new FeatureRepository(ObjectContext);
        //    RoleFeatureRepository RoleFeaturesRepository = new RoleFeatureRepository(ObjectContext);
        //    TextCodeRepository textCodeRep = new TextCodeRepository(tenant);
        //    ObjectTabelRepository objecttableRep = new ObjectTabelRepository(tenant);
        //    ObjectTabelQuery objectTabelQuery = new ObjectTabelQuery(objecttableRep);
            
        //    List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(tenant).ToList();

        //    //ObjectTables
        //    ObjectTablePM GeneralObjectTable = objectTables.Where(d => d.Name == "General").FirstOrDefault();
        //    ObjectTablePM feedObjectTable = objectTables.Where(d => d.Name == "Feed").FirstOrDefault();
        //    ObjectTablePM postObjectTable = objectTables.Where(d => d.Name == "Post").FirstOrDefault();
        //    ObjectTablePM postLikeObjectTable = objectTables.Where(d => d.Name == "PostLike").FirstOrDefault();
        //    ObjectTablePM groupObjectTable = objectTables.Where(d => d.Name == "Group").FirstOrDefault();
        //    ObjectTablePM groupMemberObjectTable = objectTables.Where(d => d.Name == "GroupMember").FirstOrDefault();
        //    ObjectTablePM followEntityObjectTable = objectTables.Where(d => d.Name == "FollowEntity").FirstOrDefault();
        //    ObjectTablePM followerObjectTable = objectTables.Where(d => d.Name == "Follower").FirstOrDefault();

        //    Dictionary<string, Feature> TenantFeatures = FeaturesRepository.GetFeaturesByTenant(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
        //    Dictionary<string, TextCode> TextCodes = textCodeRep.GetTextCodesByTenant(tenant).ToDictionary(d => d.Code + d.Tenant + d.ObjectTableId, a => a);
        //    List<RoleFeature> TenantRoleFeatures = RoleFeaturesRepository.GetRoleFeaturesByTenant(tenant).ToList();

        //    //FeatureFeed
        //    Feature feedFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = feedObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Feed.Features.New", NameTextCodeDefaultText = "New Feed", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature feedFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = feedObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Feed.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature feedFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = feedObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Feed.Features.Edit", NameTextCodeDefaultText = "Edit Feed", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
            
        //    //FeaturePost
        //    Feature postFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = postObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Post.Features.New", NameTextCodeDefaultText = "New Post", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature postFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = postObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Post.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature postFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = postObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Post.Features.Edit", NameTextCodeDefaultText = "Edit Post", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

        //    //FeaturePostLike
        //    Feature postLikeFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = postLikeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "PostLike.Features.New", NameTextCodeDefaultText = "New PostLike", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature postLikeFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = postLikeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "PostLike.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature postLikeFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = postLikeObjectTable.Id, Tenant = tenant, NameTextCodeCode = "PostLike.Features.Edit", NameTextCodeDefaultText = "Edit PostLike", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);


        //    //FeatureGroup
        //    Feature groupFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = groupObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Group.Features.New", NameTextCodeDefaultText = "New Group", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature groupFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = groupObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Group.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature groupFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = groupObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Group.Features.Edit", NameTextCodeDefaultText = "Edit Group", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

        //    //FeatureGroupMember
        //    Feature groupMemberFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = groupMemberObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GroupMember.Features.New", NameTextCodeDefaultText = "New GroupMember", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature groupMemberFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = groupMemberObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GroupMember.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature groupMemberFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = groupMemberObjectTable.Id, Tenant = tenant, NameTextCodeCode = "GroupMember.Features.Edit", NameTextCodeDefaultText = "Edit GroupMember", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

        //    //FeatureFollowEntity
        //    Feature followEntityFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = followEntityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "FollowEntity.Features.New", NameTextCodeDefaultText = "New FollowEntity", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature followEntityFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = followEntityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "FollowEntity.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature followEntityFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = followEntityObjectTable.Id, Tenant = tenant, NameTextCodeCode = "FollowEntity.Features.Edit", NameTextCodeDefaultText = "Edit FollowEntity", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);

        //    //FeatureFollower
        //    Feature followerFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = followerObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Follower.Features.New", NameTextCodeDefaultText = "New Follower", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature followerFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = followerObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Follower.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature followerFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = followerObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Follower.Features.Edit", NameTextCodeDefaultText = "Edit Follower", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);


        //    textCodeRep.SubmitChanges();
        //    FeaturesRepository.SubmitChanges();

        //}
    }
}