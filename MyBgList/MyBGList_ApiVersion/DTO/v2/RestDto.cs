using MyBgList.DTO.v1;

namespace MyBgList.DTO.v2;

public class RestDto<T>
{
    public List<LinkDto> Links { get; set; } = new List<LinkDto>();
    public T Item { get; set; } = default(T)!;
}
