namespace RSMFinalProjectAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.IO;
    using System.Threading.Tasks;

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
            // Generate the PDF report
            var pdfStream = await _service.CreatePdfReport(pageNumber, pageSize, orderId, orderDate, productName, productCategory);

            // Return the PDF as a stream to the client
            return File(pdfStream, "application/pdf", "SalesReport.pdf");
        }
    }
}
