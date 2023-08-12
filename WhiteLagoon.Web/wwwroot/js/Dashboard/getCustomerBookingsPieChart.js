
$(document).ready(function () {
    loadCustomerBookingChart();
});

function loadCustomerBookingChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: '/Dashboard/GetCustomerBookingsPieChartData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            // Handle the response data
            loadCustomerBookingsPieChart("customerBookingsPieChart", data);

            $(".chart-spinner").hide();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', errorThrown);
        }
    });
}




// Function to load the Pie chart
function loadCustomerBookingsPieChart(id, data) {
    var chartColors = getChartColorsArray(id);
    var options = {
        colors: chartColors,
        series: data.series,
        labels: data.labels,
        chart: {
            width: 380,
            type: 'pie',
        },
        stroke: {
            show: false
        },
        plotOptions: {
            pie: {
                dataLabels: {
                    offset: -10,
                },
            }
        },
        legend: {
            position: 'bottom',
            horizontalAlign: 'center',
            labels: {
                colors: "#fff",
                useSeriesColors: true
            },
        },
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 300
                },
                legend: {
                    position: 'bottom'
                },
            }
        }]
    };
    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}