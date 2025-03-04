﻿using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code should be minimum 3 characters.")]
        [MaxLength(3, ErrorMessage = "Code should be maximum 3 characters.")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name should be less than 100 characters.")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
