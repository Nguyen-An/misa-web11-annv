namespace MISA.QLTS.Common.Entities
{
    public enum MisaQLTSErrorCode
    {
        /// <summary>
        /// Lỗi server
        /// </summary>
        Exception = 0,

        /// <summary>
        /// Dữ liệu đầu vào bị trống
        /// </summary>
        RequiredValueIsEmpty = 1,

        /// <summary>
        /// Không tìm thấy dữ liệu
        /// </summary>
        NotValid = 2,

        /// <summary>
        /// Validate lỗi
        /// </summary>
        ValidateError = 3,

        /// <summary>
        /// Yêu cầu đầu vào không hợp lệ
        /// </summary>
        BadRequest = 4,

        /// <summary>
        /// Mã tài sản đã tồn tại
        /// </summary>
        ValidateCodeError = 5,
        
        /// <summary>
        /// BL trả về Validate sai
        /// </summary>
        BLReturnValidate = 6,

        /// <summary>
        /// BL trả về trùng mã
        /// </summary>
        BLReturnCheckCode = 7,

    }
}
