
$(document).ready(function () {
    loadMemberAndBookingChart();
});

function loadMemberAndBookingChart() {
    $(".chart-spinner").show();
    $.ajax({
        url: '/Dashboard/GetMemberAndBookingChartData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            console.log(data);
            // Handle the response data
            loadMemberAndBookingsLineChart("newMembersAndBookingsChart", data);

            $(".chart-spinner").hide();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', errorThrown);
        }
    });
}


function loadMemberAndBookingsLineChart(id, data) {
    console.log(data);
    var chartColors = getChartColorsArray(id);
    // Chart options
    var options = {
        colors: chartColors,
        chart: {
            height: 350,
            //show area later
            type: 'line',
            zoom: {
                type: 'x',
                enabled: true,
                autoScaleYaxis: true
            },
        },
        stroke: {
            curve: 'smooth',
            width: 2
        },
        series: data.series,
        dataLabels: {
            enabled: false,
        },
        markers: {
            size: 6,
            strokeWidth: 0,
            hover: {
                size: 9
            }
        },
        xaxis: {
            categories: data.categories,
            labels: {
                style: {
                    colors: "#fff",
                },
            }
        },
        yaxis: {
            labels: {
                //formatter: function (val) {
                //    return val.toFixed(0);
                //},
                style: {
                    colors: "#fff",
                },
            }
        },
        legend: {
            labels: {
                colors: "#fff",
            },
        }
    };
    // Create the line chart
    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}