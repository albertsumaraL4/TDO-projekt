document.addEventListener("DOMContentLoaded", function () {

    const blockedDates = window.blockedDates || [];
    const blockedSet = new Set(blockedDates);

    const today = new Date();
    const tomorrow = new Date();
    tomorrow.setDate(today.getDate() + 1);

    function isRangeValid(start, end) {
        const s = new Date(start);
        const e = new Date(end);

        for (let d = new Date(s); d <= e; d.setDate(d.getDate() + 1)) {
            const dayStr = d.toISOString().slice(0, 10);
            if (blockedSet.has(dayStr)) {
                return false;
            }
        }
        return true;
    }

    const endPicker = flatpickr("#endDate", {
        dateFormat: "Y-m-d",
        defaultDate: tomorrow,
        minDate: tomorrow,
        disable: blockedDates
    });

    const startPicker = flatpickr("#startDate", {
        dateFormat: "Y-m-d",
        defaultDate: today,
        minDate: today,
        disable: blockedDates,
        onChange: function (selectedDates) {
            if (selectedDates.length === 0) return;

            const minEnd = new Date(selectedDates[0].getTime() + 86400000);
            endPicker.set("minDate", minEnd);

            if (endPicker.selectedDates.length > 0) {
                if (!isRangeValid(selectedDates[0], endPicker.selectedDates[0])) {
                    alert("Wybrany zakres nachodzi na istniejącą rezerwację!");
                    endPicker.clear();
                }
            }
        }
    });

    endPicker.config.onChange.push(function (selectedDates) {
        if (startPicker.selectedDates.length === 0 || selectedDates.length === 0) return;

        if (!isRangeValid(startPicker.selectedDates[0], selectedDates[0])) {
            alert("Wybrany zakres nachodzi na istniejącą rezerwację!");
            endPicker.clear();
        }
    });

});
