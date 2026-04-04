window.onload = function () {
    const ctx = document.getElementById('myChart').getContext('2d');

    const labels = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

    const data = {
        labels: labels,
        datasets: [
            {
                label: 'Ingresos',
                data: [1200, 1900, 3000, 5000, 2300, 4200, 3900, 1200, 1900, 3000, 5000, 2300],
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                tension: 0.4,
                fill: true,
                pointStyle: 'rectRot',
                pointRadius: 6,
                pointHoverRadius: 8
            },
            {
                label: 'Gastos',
                data: [1200, 1900, 3000, 2000, 2600, 1200, 2900, 4200, 2900, 4000, 1000, 4300],
                borderColor: 'rgba(255, 99, 132, 1)',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                tension: 0.4,
                fill: true,
                pointStyle: 'circle',
                pointRadius: 6,
                pointHoverRadius: 8
            },


            // Esto deberia ser autocalculado en funcion de ingrsos y gastos
            {
                label: 'Beneficio',
                data: [1200, 1900, 1000, 2000, 3300, 4890, 7900, 10200, 7900, 2000, 4000, 2300],
                borderColor: 'rgba(54, 162, 235, 1)',
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                tension: 0.4,
                fill: true,
                pointStyle: 'triangle',
                pointRadius: 6,
                pointHoverRadius: 8
            }
        ]
    };

    const config = {
        type: 'line',
        data: data,
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                    labels: { font: { size: 14, weight: 'bold' } }
                },
                title: {
                    display: true,
                    text: 'Informe de Facturación',
                    font: { size: 22 }
                },
                tooltip: {
                    mode: 'index',
                    intersect: false,
                    backgroundColor: 'rgba(0,0,0,0.7)',
                    titleFont: { size: 16, weight: 'bold' },
                    bodyFont: { size: 14 }
                }
            },
            interaction: {
                mode: 'nearest',
                axis: 'x',
                intersect: false
            },
            scales: {
                x: { display: true, title: { display: true, text: 'Meses' } },
                y: { display: true, title: { display: true, text: 'Euros' }, beginAtZero: true }
            }
        }
    };

    // Aquí solo una vez, dentro del window.onload
    const myChart = new Chart(ctx, config);
}; // <-- este es el paréntesis/final del window.onload



