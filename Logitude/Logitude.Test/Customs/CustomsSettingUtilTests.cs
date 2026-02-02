using Logitude.Customs.BL.Utils;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Test.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;

namespace Logitude.Test.Customs
{
    [TestClass]
    public class CustomsSettingUtilTests
    {
        [TestMethod]
        public void GetSufix_ForProductionTenant_ReturnsProductionSuffix()
        {
            Mock.Arrange(() => CustomsSettingUtil.GetCustomsDeploymentStage(Arg.IsAny<int>()))
                .Returns(CustomsDeploymentStage.Production);

            try
            {
                var suffix = CustomsSettingUtil.GetSufix(TenantFixture.DefaultTenantId);
                Assert.AreEqual(".PRD.xml", suffix);
            }
            finally
            {
                Mock.Arrange(() => CustomsSettingUtil.GetCustomsDeploymentStage(Arg.IsAny<int>()))
                    .CallOriginal();
            }
        }

        [TestMethod]
        public void GetSufix_ForPrePilotTenant_UsesPilotSuffix()
        {
            Mock.Arrange(() => CustomsSettingUtil.GetCustomsDeploymentStage(Arg.IsAny<int>()))
                .Returns(CustomsDeploymentStage.PrePilot);

            try
            {
                var suffix = CustomsSettingUtil.GetSufix(TenantFixture.DefaultTenantId);
                Assert.AreEqual(".PLT.xml", suffix);
            }
            finally
            {
                Mock.Arrange(() => CustomsSettingUtil.GetCustomsDeploymentStage(Arg.IsAny<int>()))
                    .CallOriginal();
            }
        }

        [TestMethod]
        public void GetSufix_ForUnknownStage_ReturnsDefaultSuffix()
        {
            Mock.Arrange(() => CustomsSettingUtil.GetCustomsDeploymentStage(Arg.IsAny<int>()))
                .Returns(CustomsDeploymentStage.None);

            try
            {
                var suffix = CustomsSettingUtil.GetSufix(TenantFixture.DefaultTenantId);
                Assert.AreEqual(".KHL.xml", suffix);
            }
            finally
            {
                Mock.Arrange(() => CustomsSettingUtil.GetCustomsDeploymentStage(Arg.IsAny<int>()))
                    .CallOriginal();
            }
        }
    }
}

