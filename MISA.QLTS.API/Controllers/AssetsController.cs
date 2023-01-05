using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MISA.QLTS.API.Entities;
using MISA.QLTS.API.Entities.DTO;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MISA.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        /// <summary>
        ///  API lấy 1 nhân viên theo id
        /// </summary>
        /// <param name="fixedAssetId"></param>
        /// <returns>Thông tin nhân viên cần lấy theo id</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpGet]
        [Route("{fixedAssetId}")]
        public IActionResult GetAssetByID([FromRoute] Guid fixedAssetId)
        {
            try
            {
                //Lấy toàn bộ property của class Asset
                var properties = typeof(Asset).GetProperties();

                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_GetAssetByID";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_FixedAssetId", fixedAssetId);

                // Khởi tạo kết nối tới Database
                string connectionString = "Server=localhost;Port=3306;Database=misa.web11.hcsn.nvan;Uid=root;Pwd=anvip123;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Thực hiện gọi vào Database để chạy store procedure
                var asset = mySqlConnection.QueryFirstOrDefault<Asset>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý kết quả trả về
                if (asset != null)
                {
                    return Ok(asset);
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
                    ErrorCode = 1,
                    DevMsg = "",
                    UserMsg = "",
                    MoreInfo = "",
                    TranceId = ""
                });
            }

        }

        /// <summary>
        ///     API lấy danh sách tài sản theo điều kiện
        /// </summary>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <param name="limit">Số lượng bản ghi 1 trang</param>
        /// <param name="offset">Vị trí bắt đầu lấy</param>
        /// <param name="departmentId">Lọc phòng ban theo Id</param>
        /// <param name="fixedAssetCategoryId">Lọc loại tài sản theo Id</param>
        /// <returns>
        /// Một đối tượng chưa thông tin:
        ///  + Tổng số bản ghi thỏa mãn
        ///  + Danh sách bản ghi trên trang
        /// </returns>
        /// Created by: NVAn(22/12/2022)
        [HttpGet]
        [Route("filter")]
        public IActionResult GetAssetFilterAndPaging(
                [FromQuery] string keyword,
                [FromQuery] int limit,
                [FromQuery] int offset,
                [FromQuery] string departmentId,
                [FromQuery] string fixedAssetCategoryId
            )
        {
            try
            {   
                if(keyword == null)
                {
                    keyword = "";
                }

                if (departmentId == null)
                {
                    departmentId = "";
                }

                if (fixedAssetCategoryId == null)
                {
                    fixedAssetCategoryId = "";
                }

                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_GetAssetPaging";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_keyword", keyword);
                parameters.Add("v_limit", limit);
                parameters.Add("v_offset", offset);
                parameters.Add("v_departmentId", departmentId);
                parameters.Add("v_fixedAssetCategoryId", fixedAssetCategoryId);

                // Khởi tạo kết nối tới Database
                string connectionString = "Server=localhost;Port=3306;Database=misa.web11.hcsn.nvan;Uid=root;Pwd=anvip123;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Thực hiện gọi vào Database để chạy store procedure
                var all = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
               
                var assets = all.Read<Asset>().ToList();
                int totalRecord = all.Read<int>().Single();

                // Xử lý kết quả trả về
                if (totalRecord > 0)
                {
                    return Ok(new PagingResult<Asset>()
                    {
                        TotalRecord = totalRecord,
                        Data = assets
                    });
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
                    ErrorCode = 1,
                    DevMsg = "Lỗi ở GetAssetFilterAndPaging",
                    UserMsg = ex.ToString(),
                    MoreInfo = "",
                    TraceId = ""
                });
            }
        }

        [HttpGet]
        [Route("fixedAssetNewCode")]
        public IActionResult GetAssetNewCode()
        {
            try
            {
                // Chuẩn bị tên procedure
                string storedProcedureName = "Proc_GetMaxFixedAssetCode";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();

                // Khởi tạo kết nối tới database
                string connectionString = "Server=localhost;Port=3306;Database=misa.web11.hcsn.nvan;Uid=root;Pwd=anvip123;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // thực hiện gọi vào Database để chạy store procedure
                var all = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                string fixedAssetCodeMax = all.Read<string>().Single();

                // Lấy phần số trong mã tài sản (ép từ string thành int)
                int assetCode = Convert.ToInt16(fixedAssetCodeMax.Split('S')[1]);

                //Tạo mã tài sản mới
                string newAssetCode = Convert.ToString(assetCode + 1);
                int totalZeroLack = 4 - newAssetCode.Length;

                for (int i = 0; i <  totalZeroLack; i++) { }
                {
                    newAssetCode = "0" + newAssetCode;
                }

                newAssetCode = "TS" + newAssetCode;

                // Xử lý kết quả trả về
                return Ok(new
                {
                    NewAssetCode = newAssetCode
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Lỗi ở GetAssetFilterAndPaging",
                    UserMsg = ex.ToString(),
                    MoreInfo = "",
                    TraceId = ""
                });
            }
        }

        /// <summary>
        /// API thêm mới 1 tài sản
        /// </summary>
        /// <returns>Id tài sản vừa thêm</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpPost]
        [Route("")]
        public IActionResult InsertAsset([FromBody] Asset asset)
        {
            try
            {
                // Validate dữ liệu đầu vào

                //Lấy toàn bộ property của class Asset
                var properties = typeof(Asset).GetProperties();

                // Kiểm tra xem property nào có attribute là Required
                var validateFailures = new List<string>();
                var newAssetId = Guid.NewGuid();
                var parameters = new DynamicParameters();
                ValidateRequestData(asset, properties, validateFailures, newAssetId, parameters);

                if (validateFailures.Count > 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = 3,
                        DevMsg = "Lỗi ở InsertAsset",
                        UserMsg = "",
                        MoreInfo = validateFailures,
                        TranceId = ""
                    });
                }

                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_InsertAsset";

                // khởi tạo kết nối tới database
                string connectionString = "Server=localhost;Port=3306;Database=misa.web11.hcsn.nvan;Uid=root;Pwd=anvip123;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Thực hiện gọi vào Database để chạy procedure
                var numberOfAffectedRow = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                if (numberOfAffectedRow > 0)
                {
                    // Thành công
                    return StatusCode(StatusCodes.Status201Created, newAssetId);
                }
                else
                {
                    // Thất bại
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = 1,
                        DevMsg = "Lỗi ở InsertAsset",
                        UserMsg = "numberOfAffectedRow: đây nè " + numberOfAffectedRow,
                        MoreInfo = "",
                        TranceId = ""
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Lỗi ở InsertAsset",
                    UserMsg = ex.ToString(),
                    MoreInfo = "",
                    TranceId = ""
                });
            }

            //
        }

        private static void ValidateRequestData(Asset asset, PropertyInfo[] properties, List<string> validateFailures, Guid newAssetId, DynamicParameters parameters)
        {
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(asset);

                var requiredAttribute = (RequiredAttribute)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
                if (requiredAttribute != null && string.IsNullOrEmpty(propertyValue.ToString()))
                {
                    validateFailures.Add(requiredAttribute.ErrorMessage);
                }
                else
                {
                    // Chuẩn bị tham số đầu vào cho stored procedure
                    parameters.Add($"v_{propertyName}", propertyValue);
                }

                var keyAttribute = (KeyAttribute)property.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();
                if (keyAttribute != null)
                {
                    parameters.Add($"v_{propertyName}", newAssetId);

                }
            }
        }

        /// <summary>
        /// API sửa 1 tài sản theo id
        /// </summary>
        /// <returns>Id tài sản vừa sửa</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpPut]
        [Route("{fixedAssetId}")]
        public IActionResult UpdateAsset([FromBody] Asset asset, [FromRoute] Guid fixedAssetId)
        {

            try
            {
                // Validate dữ liệu đầu vào

                // Lấy toàn bộ property của class Asset
                var properties = typeof(Asset).GetProperties();

                // Kiểm tra xem property nào có attribute là Required
                var validateFailures = new List<string>();
                var currenntAssetId = fixedAssetId;
                var parameters = new DynamicParameters();

                foreach (var property in properties)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(asset);

                    var requiredAttribute = (RequiredAttribute) property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

                    // Nếu property đó có attribute là Required thì check value mà FE truyển lên có null hoặc empty hay không
                    if (requiredAttribute != null && string.IsNullOrEmpty(propertyValue.ToString()))
                    {

                        validateFailures.Add(requiredAttribute.ErrorMessage);
                    }
                    else
                    {
                        // Chuẩn bị tham số đầu vào cho stored procedure
                        parameters.Add($"v_{propertyName}", propertyValue);
                    }

                    var keyAttribute = (KeyAttribute) property.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();
                    if(keyAttribute != null)
                    {
                        parameters.Add($"v_{propertyName}", currenntAssetId);
                    }

                }

                if (validateFailures.Count > 0)
                {
                    return BadRequest(new
                    {
                        ErrorCode = 3,
                        DevMsg = "Lỗi ở UpdateAsset",
                        UserMsg = "",
                        MoreInfo = validateFailures,
                        TranceId = ""
                    });
                }

                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_UpdateAsset";

                // khởi tạo kết nối tới database
                string connectionString = "Server=localhost;Port=3306;Database=misa.web11.hcsn.nvan;Uid=root;Pwd=anvip123;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Thực hiện gọi vào Database để chạy procedure
                var numberOfAffectedRow = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                if (numberOfAffectedRow > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, fixedAssetId);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = 1,
                        DevMsg = "Lỗi ở UpdateAsset",
                        UserMsg = "numberOfAffectedRow: đây nè " + numberOfAffectedRow,
                        MoreInfo = "",
                        TranceId = ""
                    });
                }

                // Thành công

                // Thất bại
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Lỗi ở InsertAsset",
                    UserMsg = ex.ToString(),
                    MoreInfo = "",
                    TranceId = ""
                });
            }
        }

        /// <summary>
        /// API xóa 1 tài sản theo id
        /// </summary>
        /// <returns>Id tài sản vừa xóa</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpDelete]
        [Route("{fixedAssetId}")]
        public IActionResult DeleteAsset([FromRoute] Guid fixedAssetId)
        {
            try
            {
                // Validate dữ liệu đầu vào

                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_DeleteAsset";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();
                parameters.Add("v_FixedAssetId", fixedAssetId);

                // khởi tạo kết nối tới database
                string connectionString = "Server=localhost;Port=3306;Database=misa.web11.hcsn.nvan;Uid=root;Pwd=anvip123;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Thực hiện gọi vào Database để chạy procedure
                var numberOfAffectedRow = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                //Xử lý kết quả trả về
                if (numberOfAffectedRow > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, fixedAssetId);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new
                    {
                        ErrorCode = 1,
                        DevMsg = "Lỗi ở DeleteAsset",
                        UserMsg = "numberOfAffectedRow: đây nè " + numberOfAffectedRow,
                        MoreInfo = "",
                        TranceId = ""
                    });
                }

                // Thành công

                // Thất bại
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 1,
                    DevMsg = "Lỗi ở InsertAsset",
                    UserMsg = ex.ToString(),
                    MoreInfo = "",
                    TranceId = ""
                });
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        /// <summary>
        /// API lấy danh sách tất cả loại phòng ban
        /// </summary>
        /// <returns>Danh sách tất cả phòng ban</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpGet]
        [Route("")]
        public IActionResult GetAllDepartment()
        {
            try
            {
                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_GetAllDepartment";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();

                // Khởi tạo kết nối tới Database
                string connectionString = "Server=localhost;Port=3306;Database=misa.web11.hcsn.nvan;Uid=root;Pwd=anvip123;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Thực hiện gọi vào Database để chạy store procedure
                var departments = mySqlConnection.Query<Department>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure).ToList();

                // Xử lý kết quả trả về
                if (departments != null)
                {
                    return Ok(departments);
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
                    ErrorCode = 1,
                    DevMsg = "Lỗi ở GetAllDepartment",
                    UserMsg = ex.ToString(),
                    MoreInfo = "",
                    TranceId = ""
                });
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AssetCategorysController : ControllerBase
    {
        /// <summary>
        /// API lấy danh sách tất cả loại tài sản
        /// </summary>
        /// <returns>Danh sách tất cả tài sản</returns>
        /// Created by: NVAn (20/12/2022)
        [HttpGet]
        [Route("")]
        public IActionResult GetAllAssetCategory()
        {
            try
            {
                // Chuẩn bị tên stored procedure
                string storedProcedureName = "Proc_GetAllAssetCategory";

                // Chuẩn bị tham số đầu vào cho stored procedure
                var parameters = new DynamicParameters();

                // Khởi tạo kết nối tới Database
                string connectionString = "Server=localhost;Port=3306;Database=misa.web11.hcsn.nvan;Uid=root;Pwd=anvip123;";
                var mySqlConnection = new MySqlConnection(connectionString);

                // Thực hiện gọi vào Database để chạy store procedure
                var assetCategories = mySqlConnection.Query<AssetCategory>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure).ToList();

                // Xử lý kết quả trả về
                if (assetCategories != null)
                {
                    return Ok(assetCategories);
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
                    ErrorCode = 1,
                    DevMsg = "Lỗi ở GetAllAssetCategory",
                    UserMsg = ex.ToString(),
                    MoreInfo = "",
                    TranceId = ""
                });
            }
        }
    }
}
