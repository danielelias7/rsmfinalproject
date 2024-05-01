namespace RSMFinalProjectAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using RSMFinalProjectAPI.Application.DTOs;
    using RSMFinalProjectAPI.Domain.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class SalesReportsController : ControllerBase
    {
        private readonly ISalesReportService _service;

        public SalesReportsController(ISalesReportService service)
        {
            _service = service;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> Get(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory)
        {
            return Ok(await _service.GetAll(pageNumber, pageSize, orderId, orderDate, productName, productCategory));
        }

        [HttpGet("GetPdfReport")]
        public async Task<IActionResult> GetPdfReport(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory)
        {
            await _service.CreatePdfReport(pageNumber, pageSize, orderId, orderDate, productName, productCategory);
            var response = new { message = "Pdf created successfully." };
            return Ok(response);
        }
    }
}
