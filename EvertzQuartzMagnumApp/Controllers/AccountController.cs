using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    // Temporary placeholder admin credentials
    private const string AdminUsername = "admin";
    private const string AdminPassword = "admin";

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (username == AdminUsername && password == AdminPassword)
        {
            HttpContext.Session.SetString("IsAuthenticated", "true"); // Set session flag
            return RedirectToAction("Index", "Switching");
        }

        ModelState.AddModelError("", "Invalid username or password.");
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear all session variables
        return RedirectToAction("Login", "Account");
    }


}
