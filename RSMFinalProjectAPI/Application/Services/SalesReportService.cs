namespace RSMFinalProjectAPI.Application.Services
{
    using RSMFinalProjectAPI.Application.DTOs;
    using RSMFinalProjectAPI.Application.Exceptions;
    using RSMFinalProjectAPI.Domain.Interfaces;
    using RSMFinalProjectAPI.Domain.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PdfSharp.Pdf;
    using PdfSharp.Drawing;
    using System.Diagnostics;

    public class SalesReportService : ISalesReportService
    {
        private readonly ISalesReportRepository _salesReportRepository;
        public SalesReportService(ISalesReportRepository repository)
        {
            _salesReportRepository = repository;
        }

        public async Task<IEnumerable<GetSalesReportDto>> GetAll(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory)
        {
            var salesReport = await _salesReportRepository.GetAll();

            // Filters
            if (orderId.HasValue)
            {
                salesReport = salesReport.Where(s => s.OrderId == orderId.Value);
            }

            if (orderDate != null)
            {
                DateTime fechaFiltro = DateTime.Parse(orderDate);
                salesReport = salesReport.Where(s => s.OrderDate.Value.Date == fechaFiltro.Date);
            }

            if (productName != null)
            {
                salesReport = salesReport.Where(s => s.ProductName.Contains(productName));
            }

            if (productCategory != null)
            {
                salesReport = salesReport.Where(s => s.ProductCategory == productCategory);
            }

            Console.WriteLine(orderId);

            // Pagination
            var paginatedSalesReport = salesReport
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            List<GetSalesReportDto> salesReportDto = new();

            foreach (var sales in paginatedSalesReport)
            {
                GetSalesReportDto dto = new()
                {
                    OrderId = sales.OrderId,
                    OrderDate = sales.OrderDate,
                    CustomerId = sales.CustomerId,
                    ProductId = sales.ProductId,
                    ProductName = sales.ProductName,
                    ProductCategory = sales.ProductCategory,
                    UnitPrice = sales.UnitPrice,
                    Quantity = sales.Quantity,
                    TotalPrice = sales.TotalPrice,
                    SalesPersonId = sales.SalesPersonId,
                    FirstName = sales.FirstName,
                    LastName = sales.LastName,
                    ShippingAddress = sales.ShippingAddress,
                    BillingAddress = sales.BillingAddress
                };

                salesReportDto.Add(dto);
            }
            return salesReportDto;
        }

        public async Task CreatePdfReport(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory)
        {
            var salesReportDto = await GetAll(pageNumber, pageSize, orderId, orderDate, productName, productCategory);

            // new PDF
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Generated Sales Report";

            // Blank page
            PdfPage page = document.AddPage();

            // Fonts
            XFont font = new XFont("Verdana", 10);
            XFont fontTitle = new XFont("Verdana", 12, XFontStyle.Bold);

            // XGraphics to draw
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Titles
            string[] columnTitles = { "Order ID", "Order Date", "Product", "Category", "Total Price" };
            double[] columnWidths = { 100, 100, 200, 100, 100 };
            double tableHeight = 20;

            // Draw titles
            double x = 0;
            for (int i = 0; i < columnTitles.Length; i++)
            {
                gfx.DrawRectangle(XBrushes.DarkGray, x, 0, columnWidths[i], tableHeight);
                gfx.DrawString(columnTitles[i], fontTitle, XBrushes.White, new XRect(x, 0, columnWidths[i], tableHeight), XStringFormats.Center);
                x += columnWidths[i];
            }

            // Draw content
            double y = tableHeight;
            foreach (var sales in salesReportDto)
            {
                x = 0;
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[0], tableHeight);
                gfx.DrawString(sales.OrderId.ToString(), font, XBrushes.Black, new XRect(x, y, columnWidths[0], tableHeight), XStringFormats.Center);
                x += columnWidths[0];

                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[1], tableHeight);
                gfx.DrawString(sales.OrderDate.ToString(), font, XBrushes.Black, new XRect(x, y, columnWidths[1], tableHeight), XStringFormats.Center);
                x += columnWidths[1];

                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[2], tableHeight);
                gfx.DrawString(sales.ProductName, font, XBrushes.Black, new XRect(x, y, columnWidths[2], tableHeight), XStringFormats.Center);
                x += columnWidths[2];

                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[3], tableHeight);
                gfx.DrawString(sales.ProductCategory, font, XBrushes.Black, new XRect(x, y, columnWidths[3], tableHeight), XStringFormats.Center);
                x += columnWidths[3];

                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[4], tableHeight);
                string formattedTotalPrice = sales.TotalPrice?.ToString("C") ?? "N/A";
                gfx.DrawString(formattedTotalPrice, font, XBrushes.Black, new XRect(x, y, columnWidths[4], tableHeight), XStringFormats.Center);

                y += tableHeight;
            }

            // Save
            string filename = "SalesReport.pdf";
            document.Save(filename);
            // Open
            Process.Start("explorer.exe", filename);
        }
    }
}
