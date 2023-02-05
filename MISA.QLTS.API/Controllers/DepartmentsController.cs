using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.DL;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MISA.QLTS.API.Controllers
{
    public class DepartmentsController : BasesController<Department>
    {
        #region Field

        private IDepartmentBL _departmentBL;

        #endregion

        #region Constructor

        public DepartmentsController(IDepartmentBL departmentBL) : base(departmentBL)
        {
            _departmentBL = departmentBL;        
        }

        #endregion
    }

}
