// wwwroot/js/chartInterop.js
window.renderChart = (canvasId, config) => {
    const ctx = document.getElementById(canvasId).getContext('2d');
    return new Chart(ctx, config);
};

function sayHello() {
    alert("Hello");
}