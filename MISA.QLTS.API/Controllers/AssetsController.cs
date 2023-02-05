using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entities;
using Enum = MISA.QLTS.Common.Entities.MisaQLTSErrorCode;
using MISA.QLTS.Common;
using MISA.QLTS.Common.Resource;

namespace MISA.QLTS.API.Controllers
{
    public class AssetsController : BasesController<FixedAsset>
    {
        #region Field

        private IAssetBL _assetBL;

        #endregion

        #region Constructor

        public AssetsController(IAssetBL assetBL) : base(assetBL)
        {
            _assetBL = assetBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API Lấy mã mới tài sản
        /// </summary>
        /// <returns>Mã lớn nhất của tài sản trong bảng</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpGet]
        [Route("new-code")]
        public IActionResult GetNewCode()
        {
            try
            {
                var newCode = _assetBL.GetAssetNewCode();

                return Ok(new
                {
                    NewRecordCode = newCode
                });
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = MisaQLTSErrorCode.Exception,
                    DevMsg = Resources.DefaultUserMsg,
                    UserMsg = Resources.DefaultUserMsg,
                    TranceId = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// API thêm mới 1 bản ghi
        /// </summary>
        /// <returns>Id tài sản vừa thêm</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpPost]
        [Route("")]
        public IActionResult InsertRecord([FromBody] FixedAsset record)
        {
            try
            {
                var newRecordId = Guid.NewGuid();

                var numberOfAffectedRow = _assetBL.InsertRecord(record, newRecordId);

                //Xử lý kết quả trả về
                if (numberOfAffectedRow > 0)
                {
                    // Thành công
                    return StatusCode(StatusCodes.Status201Created, newRecordId);
                }
                else if (numberOfAffectedRow == -1)
                {   
                    // Trùng mã
                    return BadRequest(new
                    {
                        ErrorCode = MisaQLTSErrorCode.ValidateCodeError,
                        DevMsg = Resources.ValidateCodeErrorDevMsg,
                        UserMsg = Resources.ValidateCodeErrorUserMsg,
                        TranceId = HttpContext.TraceIdentifier
                    });
                }
                else if (numberOfAffectedRow == -2)
                {
                    // Validate thất bại
                    return BadRequest(new
                    {
                        ErrorCode = MisaQLTSErrorCode.ValidateError,
                        DevMsg = Resources.ValidateErrorDevMsg,
                        UserMsg = Resources.ValidateUserMsg,
                        TranceId = HttpContext.TraceIdentifier
                    });
                }
                else
                {
                    // Thất bại
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        ErrorCode = MisaQLTSErrorCode.BadRequest,
                        DevMsg = Resources.DBEmDevMsg,
                        UserMsg = Resources.DefaultUserMsg,
                        TranceId = HttpContext.TraceIdentifier
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = MisaQLTSErrorCode.Exception,
                    DevMsg = Resources.DBEmDevMsg,
                    UserMsg = Resources.DefaultUserMsg,
                    TranceId = HttpContext.TraceIdentifier
                });
            }
        }
        #endregion
    }
}
