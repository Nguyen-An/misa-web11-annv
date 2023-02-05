using Dapper;
using MISA.QLTS.Common;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class BaseBL<T> : IBaseDL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method

        /// <summary>
        ///  API lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>ID bản ghi muốn lấy</returns>
        /// Created by: NVAn (20/12/2022)
        public T GetRecordByID(Guid recordID)
        {
            return _baseDL.GetRecordByID(recordID);
        }

        /// <summary>
        ///  API lấy tất cả bản ghi theo id
        /// </summary>
        /// <returns>Tất cả bản ghi muốn lấy</returns>
        /// Created by: NVAn (20/12/2022)
        public List<T> GetAllRecord()
        {
            return _baseDL.GetAllRecord();
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
            if (keyword == null)
            {
                keyword = "";
            }

            if (limit == null)
            {
                limit = 50;
            }

            if (offset == null)
            {
                offset = 0;
            }

            if (departmentId == null)
            {
                departmentId = "";
            }

            if (fixedAssetCategoryId == null)
            {
                fixedAssetCategoryId = "";
            }

            PagingResult<T> result = _baseDL.GetRecordsFilterAndPaging(keyword, limit, offset, departmentId, fixedAssetCategoryId);

            return result;
        }

        /// <summary>
        /// Validate dữ liệu đầu vào data
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Danh sách validate</returns>
        public List<string> ValidateRequestData(T record)
        {
            // Validate dữ liệu đầu vào

            //Lấy toàn bộ property của class Asset
            var properties = typeof(T).GetProperties();

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
        /// API sửa 1 bản ghi theo id
        /// </summary>
        /// <param name="record">thông tin mới của bản ghi</param>
        /// <param name="RecordId">ID bản ghi cần sửa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: NVAn (20/12/2022)
        public int UpdateRecord(T record, Guid RecordId)
        {
            var numberOfAffectedRow = _baseDL.UpdateRecord(record, RecordId);

            return numberOfAffectedRow;
        }

        /// <summary>
        /// API xóa 1 tài sản theo ID
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: NVAn (20/12/2022)
        public int DeleteRecord(Guid recordId)
        {
            var numberOfAffectedRow = _baseDL.DeleteRecord(recordId);

            return numberOfAffectedRow;
        }

        #endregion

    }
}
