using System.ComponentModel.DataAnnotations;

namespace MISA.QLTS.API.Entities
{      
    /// <summary>
    /// 
    /// </summary>
    public class Asset
    {
        /// <summary>
        /// Id tài sản
        /// </summary>
        [Key]
        public Guid FixedAssetId { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>
        [Required(ErrorMessage = "Mã tài sản không được để trống!")]
        public string FixedAssetCode { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        [Required(ErrorMessage = "Tên tài sản không được để trống!")]
        public string FixedAssetName { get; set; }

        /// <summary>
        /// Id phòng ban
        /// </summary>
        [Required(ErrorMessage = "Mã phòng ban không được để trống!")]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Id loại tài sản
        /// </summary>
        [Required(ErrorMessage = "Mã loại tài sản không được để trống!")]
        public Guid FixedAssetCategoryId { get; set; }

        /// <summary>
        /// Ngày mua
        /// </summary>
        [Required(ErrorMessage = "Ngày mua tài sản không được để trống!")]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// nguyên giá
        /// </summary>
        [Required(ErrorMessage = "Nguyên giá không được để trống!")]
        public decimal Cost { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        [Required(ErrorMessage = "Số lượng không được để trống!")]
        public int Quantity { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn (%)
        /// </summary>
        [Required(ErrorMessage = "Tỷ lệ hao mòn tài sản không được để trống!")]
        public decimal DepreciationRate { get; set; }

        /// <summary>
        /// Năm bắt đầu theo dõi tài sản trên phần mềm
        /// </summary>
        [Required(ErrorMessage = "Năm theo dõi tài sản không được để trống!")]
        public int TrackedYear { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        [Required(ErrorMessage = "Số năm sử dụng không được để trống!")]
        public int LifeTime { get; set; }

        /// <summary>
        /// Năm sử dụng
        /// </summary>
        [Required(ErrorMessage = "Năm sử dụng tài sản không được để trống!")]
        public int ProductionYear { get; set; }

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
