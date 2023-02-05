using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Const
{
    public class ProcedureName
    {
        /// <summary>
        /// Template cho procedure lấy bản ghi theo ID
        /// </summary>
        public static string PROC_GET_BY_ID = "Proc_Get{0}ByID";

        /// <summary>
        /// Template cho procedure lấy tất cả bản ghi 
        /// </summary>
        public static string PROC_GET_ALL = "Proc_GetAll{0}";

        /// <summary>
        /// Template cho procedure lấy bản ghi theo phân trang
        /// </summary>
        public static string PROC_GET_PAGING = "Proc_Get{0}Paging";

        /// <summary>
        /// Template cho procedure lấy mã mới cho bản ghi
        /// </summary>
        public static string PROC_GET_NEWCODE = "Proc_GetMax{0}Code";

        /// <summary>
        /// Template cho procedure thêm mới một bản ghi
        /// </summary>
        public static string PROC_INSERT_RECORD = "Proc_Insert{0}";

        /// <summary>
        /// Template cho procedure cập nhật một bản ghi
        /// </summary>
        public static string PROC_UPDATE_RECORD = "Proc_Update{0}";

        /// <summary>
        /// Template cho procedure xóa một bản ghi
        /// </summary>
        public static string PROC_DELETE_RECORD = "Proc_Delete{0}";

        /// <summary>
        /// Template cho procedure lấy mã trùng
        /// </summary>
        public static string PROC_GET_CODE_BY_CODE = "Proc_Get{0}CodeByCode";
    }
}
