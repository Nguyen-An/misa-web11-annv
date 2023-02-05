using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL.AssetCategoryBL;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.DL;
using MySqlConnector;

namespace MISA.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetCategorysController : BasesController<AssetCategory>
    {
        #region Field

        private IAssetCategoryBL _assetCategoryBL;

        #endregion

        #region Constructor
        public AssetCategorysController(IAssetCategoryBL assetCategoryBL) : base(assetCategoryBL)
        {
            _assetCategoryBL = assetCategoryBL;
        }

        #endregion

        #region Method


        #endregion
    }
}
