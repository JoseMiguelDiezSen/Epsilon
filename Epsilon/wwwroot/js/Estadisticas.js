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
                data: [800, 1200, 1900, 3000, 1800, 2500, 2200],
                borderColor: 'rgba(255, 99, 132, 1)',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                tension: 0.4,
                fill: true,
                pointStyle: 'circle',
                pointRadius: 6,
                pointHoverRadius: 8
            },
            {
                label: 'Beneficio',
                data: [400, 700, 1100, 2000, 500, 1700, 1700],
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

//window.onload = function () {
//    const ctx = document.getElementById('myChart1').getContext('2d');

//    const labels = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

//    const data = {
//        labels: labels,
//        datasets: [
//            {
//                label: 'Ingresos',
//                data: [1200, 1900, 3000, 5000, 2300, 4200, 3900, 1200, 1900, 3000, 5000, 2300],
//                borderColor: 'rgba(75, 192, 192, 1)',
//                backgroundColor: 'rgba(75, 192, 192, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'rectRot',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            },
//            {
//                label: 'Gastos',
//                data: [800, 1200, 1900, 3000, 1800, 2500, 2200],
//                borderColor: 'rgba(255, 99, 132, 1)',
//                backgroundColor: 'rgba(255, 99, 132, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'circle',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            },
//            {
//                label: 'Beneficio',
//                data: [400, 700, 1100, 2000, 500, 1700, 1700],
//                borderColor: 'rgba(54, 162, 235, 1)',
//                backgroundColor: 'rgba(54, 162, 235, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'triangle',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            }
//        ]
//    };

//    const config = {
//        type: 'line',
//        data: data,
//        options: {
//            responsive: true,
//            plugins: {
//                legend: {
//                    position: 'top',
//                    labels: { font: { size: 14, weight: 'bold' } }
//                },
//                title: {
//                    display: true,
//                    text: 'Informe de Facturación',
//                    font: { size: 22 }
//                },
//                tooltip: {
//                    mode: 'index',
//                    intersect: false,
//                    backgroundColor: 'rgba(0,0,0,0.7)',
//                    titleFont: { size: 16, weight: 'bold' },
//                    bodyFont: { size: 14 }
//                }
//            },
//            interaction: {
//                mode: 'nearest',
//                axis: 'x',
//                intersect: false
//            },
//            scales: {
//                x: { display: true, title: { display: true, text: 'Meses' } },
//                y: { display: true, title: { display: true, text: 'Euros' }, beginAtZero: true }
//            }
//        }
//    };

//    // Aquí solo una vez, dentro del window.onload
//    const myChart = new Chart(ctx, config);
//}; // <-- este es el paréntesis/final del window.onload

//window.onload = function () {
//    const ctx = document.getElementById('myChart2').getContext('2d');

//    const labels = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

//    const data = {
//        labels: labels,
//        datasets: [
//            {
//                label: 'Ingresos',
//                data: [1200, 1900, 3000, 5000, 2300, 4200, 3900, 1200, 1900, 3000, 5000, 2300],
//                borderColor: 'rgba(75, 192, 192, 1)',
//                backgroundColor: 'rgba(75, 192, 192, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'rectRot',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            },
//            {
//                label: 'Gastos',
//                data: [800, 1200, 1900, 3000, 1800, 2500, 2200],
//                borderColor: 'rgba(255, 99, 132, 1)',
//                backgroundColor: 'rgba(255, 99, 132, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'circle',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            },
//            {
//                label: 'Beneficio',
//                data: [400, 700, 1100, 2000, 500, 1700, 1700],
//                borderColor: 'rgba(54, 162, 235, 1)',
//                backgroundColor: 'rgba(54, 162, 235, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'triangle',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            }
//        ]
//    };

//    const config = {
//        type: 'line',
//        data: data,
//        options: {
//            responsive: true,
//            plugins: {
//                legend: {
//                    position: 'top',
//                    labels: { font: { size: 14, weight: 'bold' } }
//                },
//                title: {
//                    display: true,
//                    text: 'Informe de Facturación',
//                    font: { size: 22 }
//                },
//                tooltip: {
//                    mode: 'index',
//                    intersect: false,
//                    backgroundColor: 'rgba(0,0,0,0.7)',
//                    titleFont: { size: 16, weight: 'bold' },
//                    bodyFont: { size: 14 }
//                }
//            },
//            interaction: {
//                mode: 'nearest',
//                axis: 'x',
//                intersect: false
//            },
//            scales: {
//                x: { display: true, title: { display: true, text: 'Meses' } },
//                y: { display: true, title: { display: true, text: 'Euros' }, beginAtZero: true }
//            }
//        }
//    };

//    // Aquí solo una vez, dentro del window.onload
//    const myChart = new Chart(ctx, config);
//}; // <-- este es el paréntesis/final del window.onload

//window.onload = function () {
//    const ctx = document.getElementById('myChart3').getContext('2d');

//    const labels = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'];

//    const data = {
//        labels: labels,
//        datasets: [
//            {
//                label: 'Ingresos',
//                data: [1200, 1900, 3000, 5000, 2300, 4200, 3900, 1200, 1900, 3000, 5000, 2300],
//                borderColor: 'rgba(75, 192, 192, 1)',
//                backgroundColor: 'rgba(75, 192, 192, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'rectRot',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            },
//            {
//                label: 'Gastos',
//                data: [800, 1200, 1900, 3000, 1800, 2500, 2200],
//                borderColor: 'rgba(255, 99, 132, 1)',
//                backgroundColor: 'rgba(255, 99, 132, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'circle',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            },
//            {
//                label: 'Beneficio',
//                data: [400, 700, 1100, 2000, 500, 1700, 1700],
//                borderColor: 'rgba(54, 162, 235, 1)',
//                backgroundColor: 'rgba(54, 162, 235, 0.2)',
//                tension: 0.4,
//                fill: true,
//                pointStyle: 'triangle',
//                pointRadius: 6,
//                pointHoverRadius: 8
//            }
//        ]
//    };

//    const config = {
//        type: 'line',
//        data: data,
//        options: {
//            responsive: true,
//            plugins: {
//                legend: {
//                    position: 'top',
//                    labels: { font: { size: 14, weight: 'bold' } }
//                },
//                title: {
//                    display: true,
//                    text: 'Informe de Facturación',
//                    font: { size: 22 }
//                },
//                tooltip: {
//                    mode: 'index',
//                    intersect: false,
//                    backgroundColor: 'rgba(0,0,0,0.7)',
//                    titleFont: { size: 16, weight: 'bold' },
//                    bodyFont: { size: 14 }
//                }
//            },
//            interaction: {
//                mode: 'nearest',
//                axis: 'x',
//                intersect: false
//            },
//            scales: {
//                x: { display: true, title: { display: true, text: 'Meses' } },
//                y: { display: true, title: { display: true, text: 'Euros' }, beginAtZero: true }
//            }
//        }
//    };

//    // Aquí solo una vez, dentro del window.onload
//    const myChart = new Chart(ctx, config);
//}; // <-- este es el paréntesis/final del window.onload



