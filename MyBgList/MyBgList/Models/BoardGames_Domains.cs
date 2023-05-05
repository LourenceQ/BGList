using System.ComponentModel.DataAnnotations;

namespace MyBgList.Models;

public class BoardGames_Domains
{
    [Key]
    [Required]
    public int BoardGameId { get; set; }
    [Key]
    [Required]
    public int DomainId { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
}
