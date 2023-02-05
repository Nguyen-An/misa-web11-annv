using Dapper;
using MISA.QLTS.Common;
using MISA.QLTS.Common.Const;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.DL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class AssetDL : BaseDL<FixedAsset>, IAssetDL
    {
        /// <summary>
        /// API Lấy mã mới tài sản
        /// </summary>
        /// <returns>Mã lớn nhất của tài sản trong bảng</returns>
        /// Created by: NVAn (20/12/2022)
        public string GetAssetNewCode()
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

            return fixedAssetCodeMax;
        }
        /// </summary>
        /// <param name="record"></param>
        /// <param name="newAssetId"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: NVAn (20/12/2022)
        public int InsertRecord(FixedAsset record, Guid newRecordId)
        {
            //Lấy toàn bộ property của class Asset
            var properties = typeof(FixedAsset).GetProperties();

            // Chuẩn bị tên stored procedure
            string storedProcedureName = String.Format(ProcedureName.PROC_INSERT_RECORD, typeof(FixedAsset).Name);
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
                    parameters.Add($"v_{propertyName}", newRecordId);

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
        /// API kiểm tra mã trùng
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Số mã đã có trong bảng</returns>
        /// Created by: NVAn (1/2/2023)
        public int CheckCodeCoincide(String code)
        {
            // Chuẩn bị tên stored procedure
            string storedProcedureName = String.Format(ProcedureName.PROC_GET_CODE_BY_CODE, typeof(FixedAsset).Name);

            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add($"v_FixedAssetCode", code);

            String codeReturn;
            // khởi tạo kết nối tới database
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi vào Database để chạy procedure
                codeReturn = mySqlConnection.QueryFirstOrDefault<String>(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            if (String.Compare(codeReturn, code, true) == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
