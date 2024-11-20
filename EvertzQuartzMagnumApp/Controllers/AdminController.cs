using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

namespace EvertzQuartzMagnumApp.Controllers
{
    public class AdminController : Controller
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true // Pretty-printing option
        };

        // Admin Page
        [HttpGet]
        public IActionResult Index()
        {
            // Pass TempData message to View if it exists
            ViewBag.Message = TempData["Message"];
            return View();
        }

        // Upload Excel Method
        [HttpPost]
        public IActionResult UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Message"] = "Please upload a valid Excel file.";
                return RedirectToAction("Index");
            }

            try
            {
                using var stream = new MemoryStream();
                file.CopyTo(stream);

                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Edge Devices");

                if (worksheet == null)
                {
                    TempData["Message"] = "The uploaded file does not contain an 'Edge Devices' tab.";
                    return RedirectToAction("Index");
                }

                var data = new List<dynamic>();
                foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header row
                {
                    var mnemonicFriendlyName = row.Cell(24).GetValue<string>();
                    var mnemonicGlobal = row.Cell(25).GetValue<string>();
                    var quartzSrc = row.Cell(26).GetValue<string>();
                    var quartzDst = row.Cell(27).GetValue<string>();

                    // Skip rows where both columns 24 and 25 are blank
                    if (string.IsNullOrWhiteSpace(mnemonicFriendlyName) && string.IsNullOrWhiteSpace(mnemonicGlobal))
                    {
                        continue; // Skip this row
                    }

                    data.Add(new
                    {
                        MnemonicFriendlyName = mnemonicFriendlyName,
                        MnemonicGlobal = mnemonicGlobal,
                        QuartzSrc = quartzSrc,
                        QuartzDst = quartzDst
                    });
                }

                // Save the data to a JSON file
                var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "config.json");
                var directoryPath = Path.GetDirectoryName(jsonFilePath);

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath); // Ensure the directory exists
                }

                System.IO.File.WriteAllText(jsonFilePath, JsonSerializer.Serialize(data, _jsonOptions));

                TempData["Message"] = $"File uploaded and processed successfully. {data.Count} rows parsed.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Error processing file: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
