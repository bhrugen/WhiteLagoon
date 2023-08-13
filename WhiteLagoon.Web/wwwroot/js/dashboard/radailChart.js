
function loadRadialBarChart(id, data) {
    var options = {
        chart: {
            height: 90,
            width: 90,
            type: "radialBar",
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