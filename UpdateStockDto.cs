using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet7_api;

public class UpdateStockDto
{
    public required int Stock { get; set; }
}
