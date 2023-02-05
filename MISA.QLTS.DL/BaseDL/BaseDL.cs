using Dapper;
using MISA.QLTS.Common;
using MISA.QLTS.Common.Const;
using MISA.QLTS.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        /// <summary>
        ///  API lấy tất cả bản ghi theo id
        /// </summary>
        /// <returns>Tất cả bản ghi muốn lấy</returns>
        /// Created by: NVAn (20/12/2022)
        public List<T> GetAllRecord()
        {
            // Chuẩn bị tên stored procedure
            string storedProcedureName = String.Format(ProcedureName.PROC_GET_ALL, typeof(T).Name);

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();

            // Khởi tạo kết nối tới Database
            List<T> record;
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào Database để chạy store procedure
                record = mySqlConnection.Query<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }

            return record;
        }

        /// <summary>
        ///  API lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>ID bản ghi muốn lấy</returns>
        /// Created by: NVAn (20/12/2022)
        public T GetRecordByID(Guid recordID)
        {
            // Chuẩn bị tên stored procedure
            string storedProcedureName = String.Format(ProcedureName.PROC_GET_BY_ID, typeof(T).Name);

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add($"v_{typeof(T).Name}Id", recordID);

            // Khởi tạo kết nối tới Database
            T record;
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào Database để chạy store procedure
                record = mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return record;
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
        public PagingResult<T> GetRecordsFilterAndPaging(string keyword, int limit, int offset, string departmentId, string fixedAssetCategoryId)
        {
            // Chuẩn bị tên stored procedure
            string storedProcedureName = String.Format(ProcedureName.PROC_GET_PAGING, typeof(T).Name); 

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("v_keyword", keyword);
            parameters.Add("v_limit", limit);
            parameters.Add("v_offset", offset);
            parameters.Add("v_departmentId", departmentId);
            parameters.Add("v_fixedAssetCategoryId", fixedAssetCategoryId);


            List<T> records;
            int totalRecord;

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào Database để chạy store procedure
                var all = mySqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                records = all.Read<T>().ToList();
                totalRecord = all.Read<int>().Single();
            }

            return new PagingResult<T>()
            {
                TotalRecord = totalRecord,
                Data = records
            };
        }

        /// <summary>
        /// API sửa 1 bản ghi theo id
        /// </summary>
        /// <param name="record">thông tin mới của bản ghi</param>
        /// <param name="RecordId">ID bản ghi cần sửa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// /// Created by: NVAn (20/12/2022)
        public int UpdateRecord(T record, Guid RecordId)
        {
            // Lấy toàn bộ property của class Asset
            var properties = typeof(T).GetProperties();

            // Chuẩn bị tên stored procedure
            string storedProcedureName = String.Format(ProcedureName.PROC_UPDATE_RECORD, typeof(T).Name);
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);

                // Chuẩn bị tham số đầu vào cho stored procedure
                parameters.Add($"v_{propertyName}", propertyValue);

                var keyAttribute = (KeyAttribute)property.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault();
                if (keyAttribute != null)
                {
                    parameters.Add($"v_{propertyName}", RecordId);
                }
            }

            int numberOfAffectedRow;
            // khởi tạo kết nối tới database
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào Database để chạy procedure
                numberOfAffectedRow = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return numberOfAffectedRow;
        }

        /// <summary>
        /// API xóa 1 bản ghi
        /// </summary>
        /// <param name="RecordId">Id bản ghi cần xóa</param>
        /// <returnsId bản ghi cần xóa></returns>
        /// Created by: NVAn (20/12/2022)
        public int DeleteRecord(Guid recordId)
        {
            // Chuẩn bị tên stored procedure
            string storedProcedureName = String.Format(ProcedureName.PROC_DELETE_RECORD, typeof(T).Name); 

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add($"v_{typeof(T).Name}Id", recordId);

            int numberOfAffectedRow;
            // khởi tạo kết nối tới database
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào Database để chạy procedure
                numberOfAffectedRow = mySqlConnection.Execute(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            return numberOfAffectedRow;
        }
    }
}
