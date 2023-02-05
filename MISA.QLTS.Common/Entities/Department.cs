namespace MISA.QLTS.Common.Entities
{
    public class Department
    {
        /// <summary>
        /// Id của phòng ban
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Mã của phòng ban
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

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
