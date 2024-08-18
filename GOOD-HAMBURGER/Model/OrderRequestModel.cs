using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GOOD_HAMBURGER.Model
{
    public class OrderRequestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? Name { get; set; }

        [JsonIgnore]
        public List<MenuItemModel> MenuItems { get; set; } = new List<MenuItemModel>();
    }


  

}
