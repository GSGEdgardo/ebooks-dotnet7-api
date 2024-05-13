using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet7_api;

/// <summary>
/// Represents an eBook entity.
/// </summary>
public class EditEbookDto
{

    /// <summary>
    /// Title of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Author of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public  string Author { get; set; } = string.Empty;

    /// <summary>
    /// Genre of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public  string Genre { get; set; } = string.Empty;

    /// <summary>
    /// Format of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public  string Format { get; set; } = string.Empty;

    /// <summary>
    /// Price of the eBook.
    /// </summary>
    [Range(1, int.MaxValue)]
    public  int Price { get; set; } 

}
