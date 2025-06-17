function renderGraph() {
    // Pegar dados
    const choosenDt = document.getElementById('data');
    const startDate = document.getElementById('start').value;
    const endDate = document.getElementById('end').value;

    if (!startDate || !endDate || !choosenDt.value) {
        alert('Um ou mais campos foram deixados vazios.');
        return;
    }

    const data = getData(choosenDt.value, startDate, endDate);
    console.log(data);

    if (!data)
        return

    // Selecionar gráfico
    const catChartElement = document.getElementById('catChart');
    const prdChartElement = document.getElementById('prdChart');

    if (!choosenDt.value) {
        alert('Escolha um dado para analisar.')
        return
    }

    if (catChartElement) catChartElement.style.display = 'none';
    if (prdChartElement) prdChartElement.style.display = 'none';

    if (choosenDt.value == 1) {
        renderPrdGraphic(data)
        if (prdChartElement) prdChartElement.style.display = 'block';
    } else {
        renderCatGraphic(data)
        if (catChartElement) catChartElement.style.display = 'block';
    }
}

function getData(inpValue, start, end) {
    const request = new XMLHttpRequest()
    request.open('GET', `/Sales/${inpValue}/${start}/${end}`, false)
    request.setRequestHeader('Content-Type', 'application/json')

    try {
        request.send();

        if (request.status !== 200) {
            alert('Ocorreu um erro ao coletar os dados');
            console.error(request.status + ": " + request.responseText);
            return null;
        } else {
            const jsonString = JSON.parse(request.responseText).jsonString
            return JSON.parse(jsonString);
        }

    } catch (ex) {
        alert('Falha na comunicação: ' + error.message)
        return null
    }
}

function random_rgb() {
    var o = Math.round, r = Math.random, s = 255;
    return o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s);
}

function formatCatSalesData(dataGroup) {
    let dataset = []
    dataGroup.monthSales.forEach((i) => {
        dataset.push({ x: i.time, y: i.totalSales })
    })
    dataset = dataset.sort((a, b) => a.x - b.x);

    const rgb = random_rgb();

    const catData = {
        label: dataGroup.name,
        data: dataset,
        borderWidth: 2,
        borderColor: `rgb(${rgb})`,
        backgroundColor: `rgba(${rgb}, 0.5)`,
        tension: 0.1
    }

    return catData;
}

function renderCatGraphic(data) {
    const ctx = document.getElementById('catChart');

    if (window.catChart instanceof Chart) {
        window.catChart.destroy();
    }

    let startDate = new Date(document.getElementById('start').value + 'T00:00:00');
    let endDate = new Date(document.getElementById('end').value + 'T23:59:59');

    const min = new Date(startDate.getFullYear(), startDate.getMonth(), 1);
    const max = new Date(endDate.getFullYear(), endDate.getMonth() + 1, 0);

    const datasets = []
    data.forEach((i) => {
        datasets.push(formatCatSalesData(i));
    });

    window.catChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: 'Categorias',
            datasets: datasets
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'month',
                        displayFormats: {
                            date: 'MMM-yyyy'
                        },
                        tooltipFormat: 'MMM-yyyy'
                    },
                    title: {
                        display: false
                    },
                    min: min,
                    max: max,
                    ticks: {
                        autoSkip: false,
                        maxRotation: 45,
                        minRotation: 45
                    },
                    grid: {
                        color: 'rgba(0, 0, 0, 0.1)'
                    }
                },
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Quant. Vendida',
                        color: 'black'
                    },
                    ticks: {
                        callback: function (value) {
                            return Math.floor(value);
                        }
                    }
                }
            },
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        font: {
                            size: 14
                        },
                        padding: 20
                    }
                },
                tooltip: {
                    callbacks: {
                        title: function(context) {
                            return context[0].label.replace(',', '');
                        },
                        label: function(context) {
                            return `${context.dataset.label}: ${context.parsed.y}`;
                        }
                    }
                }
            }
        }
    });
}

function renderPrdGraphic(data) {
    const ctx = document.getElementById('prdChart');
    if (!ctx) return;

    if (window.prdChart instanceof Chart) {
        window.prdChart.destroy();
    }

    // 1. Criar array combinando nomes e vendas
    const products = data.map(item => ({
        name: item.name,
        sales: item.monthSales[0]?.totalSales || 0 // Usando operador optional chaining
    }));

    // 2. Ordenar por vendas (decrescente)
    products.sort((a, b) => b.sales - a.sales);

    // 3. Separar labels e dados ordenados
    const labels = products.map(p => p.name);
    const salesData = products.map(p => p.sales);

    window.prdChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Unidades Vendidas',
                data: salesData,
                backgroundColor: 'rgba(54, 162, 235, 0.7)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: {
            indexAxis: 'y',
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                title: {
                    display: true,
                    text: 'Vendas por Produto (Ordenado)',
                    font: { size: 18 }
                },
                legend: { display: false },
                tooltip: {
                    mode: 'index',
                    callbacks: {
                        label: function (context) {
                            return `${context.dataset.label}: ${context.parsed.x}`;
                        }
                    }
                }
            },
            scales: {
                x: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Unidades Vendidas',
                        font: { weight: 'bold' }
                    }
                },
                y: {
                    ticks: {
                        autoSkip: false,
                        font: { size: 12 }
                    },
                    grid: { display: false }
                }
            },
            layout: {
                padding: { right: 20 }
            }
        }
    });
}