namespace MISA.QLTS.Common.Entities
{
    /// <summary>
    /// Dữ liệ trả về khi phân trang
    /// </summary>
    public class PagingResult<T>
    {
        /// <summary>
        /// Tổng số bản ghi thỏa mãn điều kiện
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// Danh sách bản ghi
        /// </summary>
        public List<T> Data { get; set; }
    }
} 
