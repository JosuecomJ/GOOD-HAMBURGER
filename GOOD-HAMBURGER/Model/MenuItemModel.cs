using GOOD_HAMBURGER.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class MenuItemModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public ItemType Type { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public bool IsExtra { get; set; }

    public int? OrderRequestId { get; set; } // Torne a chave estrangeira nullable

    public OrderRequestModel? OrderRequest { get; set; } // Relação de navegação opcional
}
