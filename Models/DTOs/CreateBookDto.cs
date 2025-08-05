using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookLibrarySystem.Models.DTOs
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Format { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsPhysical { get; set; }
        public bool IsPublished { get; set; }
        public int? PublisherId { get; set; }
        public List<int>? AuthorIds { get; set; }
        public List<int>? GenreIds { get; set; }
        // For new names from frontend
        public List<string>? Authors { get; set; }
        public List<string>? Genres { get; set; }
        public string? Publisher { get; set; }
        public IFormFile CoverImage { get; set; } // For file upload
        public string? CoverImageUrl { get; set; } // For URL
        public bool InLibraryAccess { get; set; }
    }
}
