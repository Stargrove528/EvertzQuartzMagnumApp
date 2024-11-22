using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EvertzQuartzMagnumApp.Controllers
{
    public class SwitchingController : Controller
    {
        // Path to the configuration JSON file
        private readonly string _jsonFilePath;
        private readonly ILogger<SwitchingController> _logger;

        // Constructor: Accepts the logger as a dependency
        public SwitchingController(ILogger<SwitchingController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "config.json");
        }

        // GET: Switching/Index
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation("Checking for configuration file at: {JsonFilePath}", _jsonFilePath);

            if (!System.IO.File.Exists(_jsonFilePath))
            {
                ViewBag.ErrorMessage = "Configuration file not found. Please upload the file in the Admin section.";
                _logger.LogWarning("Configuration file not found at {JsonFilePath}.", _jsonFilePath);
                return View();
            }

            try
            {
                var jsonData = System.IO.File.ReadAllText(_jsonFilePath);
                _logger.LogInformation("Configuration file successfully read.");

                var configData = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
                if (configData == null || !configData.Any())
                {
                    ViewBag.ErrorMessage = "The configuration file is empty or invalid.";
                    _logger.LogWarning("The configuration file at {JsonFilePath} is empty or invalid.", _jsonFilePath);
                    return View();
                }

                // Filter destinations (e.g., only those with "DST" in specific columns)
                var destinations = configData.Where(entry =>
                    entry.TryGetProperty("MnemonicFriendlyName", out var name) &&
                    name.GetString()?.Contains("DST-") == true
                ).ToList();

                // Filter sources (e.g., only those with "SRC" in specific columns)
                var sources = configData.Where(entry =>
                    entry.TryGetProperty("MnemonicFriendlyName", out var name) &&
                    name.GetString()?.Contains("SRC-") == true
                ).ToList();

                // Pass both filtered destinations and sources to the view
                ViewBag.Destinations = destinations;
                ViewBag.Sources = sources;

                _logger.LogInformation("Filtered destinations: {CountDst}, sources: {CountSrc}.", destinations.Count, sources.Count);
                return View();
            }
            catch (JsonException ex)
            {
                ViewBag.ErrorMessage = "Error parsing the configuration file. Please ensure it is valid JSON.";
                _logger.LogError(ex, "JSON parsing error for configuration file at {JsonFilePath}.", _jsonFilePath);
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error loading configuration file: {ex.Message}";
                _logger.LogError(ex, "Unexpected error loading configuration file at {JsonFilePath}.", _jsonFilePath);
                return View();
            }
        }


        // POST: Switching/Take
        [HttpPost]
        public IActionResult Take([FromBody] SwitchRequest request)
        {
            // Validate the input
            if (string.IsNullOrWhiteSpace(request.Source) || string.IsNullOrWhiteSpace(request.Destination))
            {
                _logger.LogWarning("Invalid switch request received. Source: {Source}, Destination: {Destination}", request.Source, request.Destination);
                return Json(new { success = false, message = "Invalid source or destination." });
            }

            try
            {
                var success = SimulateSwitchCommand(request.Source, request.Destination);
                _logger.LogInformation("Switch command executed. Source: {Source}, Destination: {Destination}, Success: {Success}", request.Source, request.Destination, success);

                return Json(new
                {
                    success,
                    message = success ? "Switch completed successfully." : "Failed to complete the switch."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing switch request. Source: {Source}, Destination: {Destination}", request.Source, request.Destination);
                return Json(new { success = false, message = $"Error processing switch: {ex.Message}" });
            }
        }

        // Simulated switch command logic (replace with actual implementation for Magnum/Quartz)
        private static bool SimulateSwitchCommand(string source, string destination)
        {
            Console.WriteLine($"Switching Destination {destination} to Source {source}");
            return true; // Simulate a successful switch
        }

        // Request model for switching
        public class SwitchRequest
        {
            public string Source { get; set; } = string.Empty; // Default to empty
            public string Destination { get; set; } = string.Empty; // Default to empty
        }
    }
}