using MISA.QLTS.Common.Entities;
using MISA.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class DepartmentBL : BaseBL<Department>, IDepartmentBL
    {
        #region Field

        private IDepartmentDL _departmentDL;

        #endregion

        #region Constructor

        public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
        {
            _departmentDL = departmentDL;
        }

        #endregion

        #region Method



        #endregion
    }
}
