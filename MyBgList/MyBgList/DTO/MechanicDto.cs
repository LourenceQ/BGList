using System.ComponentModel.DataAnnotations;

namespace MyBgList.DTO;

public class MechanicDto
{
    [Required]
    public int Id { get; set; }

    public string? Name { get; set; }
}
