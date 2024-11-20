document.addEventListener("DOMContentLoaded", () => {
    const takeButtons = document.querySelectorAll(".btn-take");

    takeButtons.forEach(button => {
        button.addEventListener("click", () => {
            const listItem = button.closest("li");
            const sourceSelect = listItem.querySelector(".source-select");
            const destination = sourceSelect.dataset.destination;
            const source = sourceSelect.value;

            fetch('/Switching/Take', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ source, destination })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert(`Switched Destination ${destination} to Source ${source} successfully!`);
                    } else {
                        alert(`Failed to switch: ${data.message}`);
                    }
                })
                .catch(error => {
                    console.error("Error:", error);
                    alert("An error occurred while switching.");
                });
        });
    });
});
