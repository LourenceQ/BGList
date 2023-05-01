namespace MyBGList_ApiVersion.DTO.v2;

public class RestDto<T>
{
    public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    public T Data { get; set; } = default!;
}
