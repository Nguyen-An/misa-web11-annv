using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.BL;
using MISA.QLTS.Common;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Resource;
using System.Runtime.Versioning;

namespace MISA.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baserBL;

        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baserBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        ///  API lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="recordId">Id bản ghi muốn lấy</param>
        /// <returns>Thông tin nhân viên cần lấy theo id</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpGet]
        [Route("{recordId}")]
        public IActionResult GetRecordByID([FromRoute] Guid recordId)
        {

            try
            {
                var record = _baserBL.GetRecordByID(recordId);

                // Xử lý kết quả trả về
                if (record != null)
                {
                    return Ok(record);
                }
                else
                {
                    return NotFound();
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

        /// <summary>
        ///  API lấy tất cả bản ghi theo id
        /// </summary>
        /// <returns>Tất cả bản ghi muốn lấy</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpGet]
        [Route("all")]
        public IActionResult GetAllRecord()
        {
            try
            {
                // Thực hiện gọi vào Database để chạy store procedure
                var records = _baserBL.GetAllRecord();

                // Xử lý kết quả trả về
                if (records != null)
                {
                    return Ok(records);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = MisaQLTSErrorCode.NotValid,
                    DevMsg = Resources.DBEmDevMsg,
                    UserMsg = Resources.DefaultUserMsg,
                    TranceId = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        ///     API lấy danh sách record theo điều kiện
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="limit">Số lượng bản ghi 1 trang</param>
        /// <param name="offset">Vị trí bắt đầu lấy</param>
        /// <param name="departmentId">Lọc phòng ban theo Id (chỉ dành cho tài sản) </param>
        /// <param name="fixedAssetCategoryId">Lọc loại tài sản theo Id (chỉ dành cho tài sản) </param>
        /// <returns>
        /// Một đối tượng chưa thông tin:
        ///  + Tổng số bản ghi thỏa mãn
        ///  + Danh sách bản ghi trên trang
        /// </returns>
        /// Created by: NVAn(22/12/2022)
        [HttpGet]
        [Route("filter")]
        public IActionResult GetRecordsFilterAndPaging(string keyword, int limit, int offset, string departmentId, string fixedAssetCategoryId)
        {
            try
            {
                var pagingResult = _baserBL.GetRecordsFilterAndPaging(keyword, limit, offset, departmentId, fixedAssetCategoryId);

                // Xử lý kết quả trả về
                if (pagingResult.TotalRecord > 0)
                {
                    return Ok(pagingResult);
                }
                else
                {
                    return Ok(new PagingResult<T>()
                    {
                        TotalRecord = 0,
                        Data = null
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

        /// <summary>
        /// API sửa 1 bản ghi theo id
        /// </summary>
        /// <param name="record">thông tin mới của bản ghi</param>
        /// <param name="RecordId">ID bản ghi cần sửa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// /// Created by: NVAn (20/12/2022)
        [HttpPut]
        [Route("{RecordId}")]
        public IActionResult UpdateRecord([FromBody] T record, [FromRoute] Guid RecordId)
        {
            try
            {
                List<string> validateFailures = _baserBL.ValidateRequestData(record);

                if (validateFailures.Count > 0)
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

                var numberOfAffectedRow = _baserBL.UpdateRecord(record, RecordId);

                //Xử lý kết quả trả về
                if (numberOfAffectedRow > 0)
                {
                    // Thành công
                    return StatusCode(StatusCodes.Status201Created, RecordId);
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
                // Server gặp lỗi
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = MisaQLTSErrorCode.Exception,
                    DevMsg = Resources.DBEmDevMsg,
                    UserMsg = Resources.DefaultUserMsg,
                    TranceId = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// API xóa 1 bản ghi
        /// </summary>
        /// <param name="RecordId">Id bản ghi cần xóa</param>
        /// <returnsId bản ghi cần xóa></returns>
        [HttpDelete]
        [Route("{RecordId}")]
        public IActionResult DeleteRecord([FromRoute] Guid RecordId)
        {
            try
            {
                var numberOfAffectedRow = _baserBL.DeleteRecord(RecordId);

                //Xử lý kết quả trả về
                if (numberOfAffectedRow > 0)
                {
                    // Thành công
                    return StatusCode(StatusCodes.Status200OK, RecordId);
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
