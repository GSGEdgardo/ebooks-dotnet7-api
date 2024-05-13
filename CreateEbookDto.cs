using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet7_api;

public class CreateEbookDto
{
    public required string Title {get;set;} = string.Empty;
    public required string Author {get;set;} = string.Empty;
    public required string Genre {get;set;} = string.Empty;
    public required string Format {get;set;} = string.Empty;
    public required int Price {get;set;}
    
}
