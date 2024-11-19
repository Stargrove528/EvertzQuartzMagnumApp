document.addEventListener("DOMContentLoaded", () => {
    const toggleButton = document.getElementById("dark-mode-toggle");
    const body = document.body;

    // Check for saved mode in localStorage
    const isDarkMode = localStorage.getItem("darkMode") === "true";

    if (isDarkMode) {
        body.classList.add("dark-mode");
        toggleButton.textContent = "Light Mode";
    }

    // Toggle Dark Mode
    toggleButton.addEventListener("click", () => {
        body.classList.toggle("dark-mode");
        const isDark = body.classList.contains("dark-mode");
        localStorage.setItem("darkMode", isDark);
        toggleButton.textContent = isDark ? "Light Mode" : "Dark Mode";
    });
});