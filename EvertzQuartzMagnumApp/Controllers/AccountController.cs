using Microsoft.AspNetCore.Mvc;

namespace EvertzQuartzMagnumApp.Controllers // Replace with your actual project name
{
    public class AccountController : Controller
    {
        // Temporary placeholder admin credentials
        // private const string AdminUsername = "admin";
        // private const string AdminPassword = "admin";

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "admin" && password == "admin") // Example validation
            {
                HttpContext.Session.SetString("Username", username);
                HttpContext.Session.SetString("IsAuthenticated", "true"); // Set IsAuthenticated

                // Retrieve Dark Mode preference (default to false if not set)
                var isDarkMode = HttpContext.Session.GetString("DarkMode") == "true";
                HttpContext.Session.SetString("DarkMode", isDarkMode ? "true" : "false");

                return RedirectToAction("Index", "Switching");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        [HttpPost]
        public IActionResult ToggleDarkMode([FromBody] bool isDarkMode)
        {
            Console.WriteLine($"ToggleDarkMode called. IsDarkMode: {isDarkMode}");
            HttpContext.Session.SetString("DarkMode", isDarkMode ? "true" : "false");
            return Ok();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session variables
            return RedirectToAction("Login", "Account");
        }
    }
}
