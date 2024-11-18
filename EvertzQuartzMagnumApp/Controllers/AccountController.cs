using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (username == "admin" && password == "password") // Example logic
        {
            return RedirectToAction("Index", "Switching");
        }

        ModelState.AddModelError("", "Invalid username or password.");
        return View();
    }

}
