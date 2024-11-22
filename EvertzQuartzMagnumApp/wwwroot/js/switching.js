document.addEventListener("DOMContentLoaded", () => {
    const takeButtons = document.querySelectorAll(".btn-take");
    const destinationDropdown = document.getElementById("destinationDropdown");
    const sourceDropdown = document.getElementById("sourceDropdown");

    // Update sources based on the selected destination
    destinationDropdown.addEventListener("change", () => {
        const selectedDestination = destinationDropdown.value;

        // Determine the type from the selected destination
        let type = '';
        if (selectedDestination.includes('VID')) type = 'VID';
        if (selectedDestination.includes('AUD')) type = 'AUD';
        if (selectedDestination.includes('USB')) type = 'USB';

        // Filter source dropdown options based on the type
        Array.from(sourceDropdown.options).forEach(option => {
            const show = type && option.value.includes(type);
            option.style.display = show ? 'block' : 'none';
        });

        // Reset source selection to the first visible option
        const firstVisibleOption = Array.from(sourceDropdown.options).find(option => option.style.display === 'block');
        if (firstVisibleOption) {
            sourceDropdown.value = firstVisibleOption.value;
        }
    });

    // Add event listeners to Take buttons for switching
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
