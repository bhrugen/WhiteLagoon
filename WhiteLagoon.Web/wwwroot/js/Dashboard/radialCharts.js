
// Function to load the RadialBar chart
function loadRadialBarChart(id, data) {

    var options = {
        chart: {
            type: 'radialBar',
            width: 90,
            height: 90,
            sparkline: {
                enabled: true
            },
            offsetY: -10,
        },
        series: data.series,
        plotOptions: {
            radialBar: {
                dataLabels: {
                    value: {
                        offsetY: -10,
                    }
                }
            }
        },
        labels: [""]
    };

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}

