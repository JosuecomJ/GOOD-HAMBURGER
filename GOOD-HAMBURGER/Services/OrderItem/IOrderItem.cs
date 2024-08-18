using GOOD_HAMBURGER.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GOOD_HAMBURGER.Services
{
    public interface IOrderService
    {
        Task<ResponseModel<List<OrderRequestModel>>> GetOrdersAsync();
        
    }
}
