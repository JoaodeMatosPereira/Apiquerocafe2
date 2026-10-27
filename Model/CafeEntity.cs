namespace ApiCafe.Models;

public class CafeEntity
{
    public int Id { get; set; }

    public string Marca { get; set; } = string.Empty;

    public int Volume { get; set; }

    public string Tipo { get; set; } = string.Empty;
}
