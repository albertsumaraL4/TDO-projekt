const container = document.getElementById("carCategoryContainer");

if (container && typeof carCategories !== "undefined") {

    const allBtn = document.createElement("button");
    const allText = document.createElement("span");
    allText.className = "text";
    allText.textContent = "Wszystkie";

    allBtn.appendChild(allText);
    allBtn.className = "car-category-btn";
    allBtn.id = "carBtn_0";

    allBtn.onclick = () => {
        fetch('/CarCategory/SaveChosenCategory', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ categoryId: 0 })
        }).then(response => {
            if (response.ok) {
                window.location.href = `/Car/Index`;
            }
        });
    };

    container.appendChild(allBtn);

    carCategories.forEach(car => {


        const btn = document.createElement("button");

        const text = document.createElement("span");
        text.className = "text";
        text.textContent = car.Name;

        btn.appendChild(text);
        btn.className = "car-category-btn";
        btn.id = `carBtn_${car.Id}`;

        console.log(`${car.Name} : carBtn_${car.Id}`);

        btn.onclick = () => {
            fetch('/CarCategory/SaveChosenCategory', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ categoryId: car.Id })
            }).then(response => {
                if (response.ok) {
                    window.location.href = `/Car/Index`;
                }
            });
        };

        container.appendChild(btn);
    });
}
