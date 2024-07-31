using System.ComponentModel.DataAnnotations;
using System;
using PelicanManagementUi.Enums;

namespace PelicanManagementUi.ViewModels.Common.Pagination
{
    public class PaginationViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Searchkey { get; set; }
        [EnumDataType(typeof(FilterType))]
        public FilterType? FilterType { get; set; } = Enums.FilterType.Desc;
    }
}
