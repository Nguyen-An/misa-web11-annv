using Dapper;
using MISA.QLTS.Common;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Resource;
using MISA.QLTS.DL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class AssetBL : BaseBL<FixedAsset>, IAssetBL
    {
        #region Field

        private IAssetDL _assetDL;

        #endregion

        #region Constructor

        public AssetBL(IAssetDL assetDL) : base(assetDL)
        {
            _assetDL = assetDL;
        }

        #endregion


        #region Method

        /// <summary>
        /// API Lấy mã mới tài sản
        /// </summary>
        /// <returns>Mã lớn nhất của tài sản trong bảng</returns>
        /// Created by: NVAn (20/12/2022)
        public string GetAssetNewCode()
        {
            // lấy mã lớn nhất từ database
            string fixedAssetCodeMax = _assetDL.GetAssetNewCode();

            // Lấy phần số trong mã tài sản (ép từ string thành int)
            int assetCode = Convert.ToInt16(fixedAssetCodeMax.Split('S')[1]);

            //Tạo mã tài sản mới
            string newAssetCode = Convert.ToString(assetCode + 1);
            int totalZeroLack = 4 - newAssetCode.Length;

            for (int i = 0; i < totalZeroLack; i++)
            {
                newAssetCode = "0" + newAssetCode;
            }

            newAssetCode = "TS" + newAssetCode;
            
            return newAssetCode;
        }

        /// </summary>
        /// <param name="record"></param>
        /// <param name="newAssetId"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: NVAn (20/12/2022)
        public int InsertRecord(FixedAsset record, Guid newRecordId)
        {
            List<string> validateFailures = ValidateRequestData(record);

            int numberCodeReturn = CheckCodeCoincide(record.FixedAssetCode);

            //Kiểm tra mã trùng nếu số mã trả về bằng 1
            if (numberCodeReturn == 1)
            {
                return -1;
            }

            // Kiểm tra validate
            if (validateFailures.Count > 0)
            {
                return -2;
            }

            var numberOfAffectedRow = _assetDL.InsertRecord(record, newRecordId);

            return numberOfAffectedRow;
        }

        public List<string> ValidateRequestData(FixedAsset record)
        {
            // Validate dữ liệu đầu vào

            //Lấy toàn bộ property của class Asset
            var properties = typeof(FixedAsset).GetProperties();

            // Kiểm tra xem property nào có attribute là Required
            var validateFailures = new List<string>();
            var newRecordId = Guid.NewGuid();
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(record);

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
                    parameters.Add($"v_{propertyName}", newRecordId);

                }
            }

            return validateFailures;
        }

        /// <summary>
        /// API kiểm tra mã trùng
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Số mã đã có trong bảng</returns>
        /// Created by: NVAn (1/2/2023)
        public int CheckCodeCoincide(String code)
        {
            int numberCodeReturn = _assetDL.CheckCodeCoincide(code);

            return numberCodeReturn;
        }
        #endregion
    }
}
