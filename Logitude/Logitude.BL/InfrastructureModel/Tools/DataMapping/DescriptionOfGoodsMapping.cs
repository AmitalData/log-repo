using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DescriptionOfGoodsMapping
    {
        public static void MapEntity(DescriptionOfGoodsPM descriptionOfGoodsPM, DescriptionOfGoods descriptionOfGoods, bool isNewState)
        {
            descriptionOfGoods.AddedManually = descriptionOfGoodsPM.AddedManually;
            descriptionOfGoods.Name = descriptionOfGoodsPM.Name;
            descriptionOfGoods.InActive = descriptionOfGoodsPM.InActive;
            descriptionOfGoods.DescriptionOfGood = descriptionOfGoodsPM.DescriptionOfGood;
            descriptionOfGoods.Tenant = descriptionOfGoodsPM.Tenant;
        }
    }
}