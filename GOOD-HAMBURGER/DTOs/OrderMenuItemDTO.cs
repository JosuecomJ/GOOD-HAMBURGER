using GOOD_HAMBURGER.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GOOD_HAMBURGER.DTOs
{
    // DTO for OrderMenuItem
    public class OrderMenuItemDTO
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderRequestDTOId { get; set; }

        public int OrderRequestId { get; set; }
        public required OrderRequestModel OrderRequest { get; set; }

        public int MenuItemId { get; set; }
        public required MenuItemModel MenuItem { get; set; }
    }
}
