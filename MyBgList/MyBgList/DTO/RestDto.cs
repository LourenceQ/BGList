namespace MyBgList.DTO;

public class RestDto<T>
{
    public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public int? RecordCount { get; set; }
    public T Data { get; set; } = default(T)!;
}
