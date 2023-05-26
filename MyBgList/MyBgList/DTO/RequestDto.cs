using MyBgList.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyBgList.DTO;

public class RequestDto
{
    [DefaultValue(0)]
    public int PageIndex { get; set; } = 0;

    [DefaultValue(10)]
    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    [DefaultValue("Name")]
    [SortColumnValidator(typeof(BoardGameDto))]
    public string? SortColumn { get; set; } = "Name";

    [DefaultValue("ASC")]
    [SortOrderValidator]
    public string? SortOrder { get; set; } = "ASC";

    [DefaultValue(null)]
    public string? FilterQuery { get; set; } = null;
}
