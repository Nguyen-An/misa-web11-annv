using MISA.QLTS.Common;
using MISA.QLTS.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IAssetDL : IBaseDL<FixedAsset>
    {
        /// <summary>
        /// API Lấy mã mới tài sản
        /// </summary>
        /// <returns>Mã lớn nhất của tài sản trong bảng</returns>
        /// Created by: NVAn (20/12/2022)
        public String GetAssetNewCode();

        /// <summary>
        ///     API thêm mới một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <param name="newAssetId"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created by: NVAn (20/12/2022)
        public int InsertRecord(FixedAsset record, Guid newRecordId);

        /// <summary>
        /// API kiểm tra mã trùng
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Số mã đã có trong bảng</returns>
        /// Created by: NVAn (1/2/2023)
        public int CheckCodeCoincide(String code);
    }
}
