document.addEventListener("DOMContentLoaded", () => {
    const toggleCheckbox = document.getElementById("toggle_checkbox");

    // Set the toggle state based on local storage
    const isDarkMode = localStorage.getItem("darkMode") === "true";
    toggleCheckbox.checked = isDarkMode;
    document.body.classList.toggle("dark-mode", isDarkMode);

    // Add event listener for toggle
    toggleCheckbox.addEventListener("change", () => {
        const isNowDark = toggleCheckbox.checked;
        document.body.classList.toggle("dark-mode", isNowDark);

        // Save the state to local storage
        localStorage.setItem("darkMode", isNowDark);

        // Save to session via API
        fetch('/Account/ToggleDarkMode', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ isDarkMode: isNowDark })
        });
    });
});
