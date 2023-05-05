﻿using System.ComponentModel.DataAnnotations;

namespace MyBgList.Models;

public class BoardGames_Mechanics
{
    [Key]
    [Required]
    public int BoardGameId { get; set; }
    [Key]
    [Required]
    public int MechanicId { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
}