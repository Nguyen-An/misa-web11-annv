namespace MISA.QLTS.Common.Entities
{
    public class AssetCategory
    {
        /// <summary>
        /// Id loại tài sản
        /// </summary>
        public Guid FixedAssetCategoryId { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public string FixedAssetCategoryCode { get; set; }

        /// <summary>
        /// Tên loại tài sản
        /// </summary>
        public string FixedAssetCategoryName { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn (%)
        /// </summary>
        public float DepreciationRate { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public int LifeTime { get; set; }
        
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
}
