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
    using PdfSharp;

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

        public async Task<Stream> CreatePdfReport(int pageNumber, int pageSize, int? orderId, string? orderDate, string? productName, string? productCategory)
        {
            var salesReportDto = await GetAll(pageNumber, pageSize, orderId, orderDate, productName, productCategory);

            // new PDF
            PdfDocument document = new();
            document.Info.Title = "Generated Sales Report";

            // Blank page with Landscape orientation
            PdfPage page = document.AddPage();
            page.Orientation = PageOrientation.Landscape;

            // Fonts
            XFont font = new("Verdana", 8);
            XFont fontTitle = new("Verdana", 10, XFontStyle.Bold);

            // XGraphics to draw
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Titles
            string[] columnTitles = { "Order ID", "Order Date", "Product", "Category", "Unit Price", "Quantity", "Name", "Shipping Address", "Billing Address", "Total Price" };
            double[] columnWidths = { 80, 80, 100, 80, 80, 60, 80, 100, 100, 80 }; // Ajusta los anchos según sea necesario
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
                int columnIndex = 0;

                // Order ID
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString(sales.OrderId.ToString(), font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Order Date
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString(sales.OrderDate?.ToString("d") ?? "N/A", font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Product Name
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString(sales.ProductName, font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Product Category
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString(sales.ProductCategory, font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Unit Price
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString(sales.UnitPrice?.ToString("C") ?? "N/A", font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Quantity
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString(sales.Quantity.ToString(), font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Name (FirstName LastName)
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString($"{sales.FirstName} {sales.LastName}", font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Shipping Address
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString(sales.ShippingAddress, font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Billing Address
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                gfx.DrawString(sales.BillingAddress, font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);
                x += columnWidths[columnIndex++];

                // Total Price
                gfx.DrawRectangle(XBrushes.LightGray, x, y, columnWidths[columnIndex], tableHeight);
                string formattedTotalPrice = sales.TotalPrice?.ToString("C") ?? "N/A";
                gfx.DrawString(formattedTotalPrice, font, XBrushes.Black, new XRect(x, y, columnWidths[columnIndex], tableHeight), XStringFormats.Center);

                y += tableHeight;
            }

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false); // Save in MemoryStream
            stream.Position = 0; // Restore stream

            return stream;
        }
    }
}