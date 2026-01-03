using JustTouch_ApiServices.Helpers;
using JustTouch_ApiServices.SupabaseService;
using JustTouch_Shared.Dtos;
using JustTouch_Shared.Enums;
using JustTouch_Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustTouch_ApiServices.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISupabaseRepository supabase;
        public OrderController(ISupabaseRepository _supabase)
        {
            supabase = _supabase;
        }

        [Authorize]
        [HttpGet("{code}")]
        public async Task<IActionResult> GetOrders(string code)
        {
            try
            {
                var groups = await supabase.GetBy<OrderGroup>(x => x.BranchCode == code);
                if (groups != null)
                {
                    var groupsDto = new List<OrderGroupDto>();

                    foreach (var og in groups)
                    {
                        var g = Mapper.Map<OrderGroup, OrderGroupDto>(og);
                        foreach (var o in og.Orders!)
                        {
                            var or = Mapper.Map<Orders, OrderDto>(o);
                            foreach (var d in o.OrderDetails)
                            {
                                var det = Mapper.Map<OrderDetails, OrderDetailDto>(d);
                                or.details.Add(det);
                            }
                            g.orders.Add(or);
                        }
                    }

                    return Ok(groupsDto);
                }

                var error = new ErrorResponse()
                {
                    Message = "No se encontro registros de pedidos",
                    StatusCode = 404
                };
                return NotFound(error);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPost("ChangeState")]
        public async Task<IActionResult> UpdateOrder(OrderGroupDto groupState)
        {
            try
            {
                var order = Mapper.Map<OrderGroupDto, OrderGroup>(groupState);
                await supabase.Update(order, x => x.GroupCode == order.GroupCode);
                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("Take")]
        public async Task<IActionResult> InsertOrders(OrderGroupDto orderGroup)
        {
            try
            {
                var wrappedGroup = Mapper.Map<OrderGroupDto, OrderGroup>(orderGroup);
                wrappedGroup.State = (int)OrderStateEnum.Pending;
                var responseGroup = await supabase.Insert(wrappedGroup);

                var orders = new List<Orders>();
                foreach (var o in orderGroup.orders)
                {
                    var order = Mapper.Map<OrderDto, Orders>(o);
                    foreach (var d in o.details)
                    {
                        var det = Mapper.Map<OrderDetailDto, OrderDetails>(d);
                        order.OrderDetails.Add(det);
                    }
                    orders.Add(order);
                }

                orders.ForEach(x => x.OrderGroupId = responseGroup!.IdOrderGroup);
                await supabase.VoidRpc("BulkOrders", orders);

                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
