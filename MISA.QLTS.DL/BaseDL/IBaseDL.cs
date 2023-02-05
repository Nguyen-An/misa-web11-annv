using MISA.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IBaseDL<T>
    {
        #region Method

        /// <summary>
        ///  API lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns>ID bản ghi muốn lấy</returns>
        /// Created by: NVAn (20/12/2022)
        public T GetRecordByID(Guid recordID);

        /// <summary>
        ///  API lấy tất cả bản ghi theo id
        /// </summary>
        /// <returns>Tất cả bản ghi muốn lấy</returns>
        /// Created by: NVAn (20/12/2022)
        public List<T> GetAllRecord();

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
        public PagingResult<T> GetRecordsFilterAndPaging(string keyword, int limit, int offset, string departmentId, string fixedAssetCategoryId);
        #endregion

        /// <summary>
        /// API sửa 1 bản ghi theo id
        /// </summary>
        /// <param name="record">thông tin mới của bản ghi</param>
        /// <param name="RecordId">ID bản ghi cần sửa</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// /// Created by: NVAn (20/12/2022)
        public int UpdateRecord(T record, Guid RecordId);

        /// <summary>
        /// API xóa 1 bản ghi
        /// </summary>
        /// <param name="RecordId">Id bản ghi cần xóa</param>
        /// <returnsId bản ghi cần xóa></returns>
        /// Created by: NVAn (20/12/2022)
        public int DeleteRecord(Guid recordId);
    }
}
