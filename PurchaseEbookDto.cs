using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet7_api;

public class PurchaseEbookDto
{
    public required int Id {get;set;}
    public required int Cantidad {get;set;}
    public required int Pago {get;set;}
}
