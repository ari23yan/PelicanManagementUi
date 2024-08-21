using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersManagementUi.Enums
{
    public enum FilterType
    {
        [Display(Name = "Filter By Created Date Desc")]
        Desc,
        [Display(Name = "Filter By Created Date Asc")]
        Asc,

    }
}
