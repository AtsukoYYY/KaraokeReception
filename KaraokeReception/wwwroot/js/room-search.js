(() => {
    const startInput = document.getElementById("Input_StartTime");
    const endInput = document.getElementById("Input_EndTime");
    const totalUsageTime = document.getElementById("totalUsageTime");

    if (!startInput || !endInput || !totalUsageTime) {
        return;
    }

    const updateTotalUsageTime = () => {
        const start = new Date(startInput.value);
        const end = new Date(endInput.value);
        const diffMinutes = Math.floor((end - start) / 60000);

        if (Number.isNaN(diffMinutes) || diffMinutes <= 0) {
            totalUsageTime.textContent = "-";
            return;
        }

        const hours = Math.floor(diffMinutes / 60);
        const minutes = diffMinutes % 60;
        totalUsageTime.textContent = `${hours}時間${minutes}分`;
    };

    startInput.addEventListener("change", updateTotalUsageTime);
    endInput.addEventListener("change", updateTotalUsageTime);
    updateTotalUsageTime();
})();
