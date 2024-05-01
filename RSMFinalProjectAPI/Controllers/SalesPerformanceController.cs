namespace RSMFinalProjectAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using RSMFinalProjectAPI.Application.DTOs;
    using RSMFinalProjectAPI.Domain.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class SalesPerformanceController : ControllerBase
    {
        private readonly ISalesPerformanceService _service;

        public SalesPerformanceController(ISalesPerformanceService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get(int pageNumber, int pageSize, int? orderId, string? productName, string? productCategory)
        {
            return Ok(await _service.GetAll(pageNumber, pageSize, orderId, productName, productCategory));
        }
    }
}
