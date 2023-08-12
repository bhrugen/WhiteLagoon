﻿
$(document).ready(function () {
    loadTotalBookingChart();
});

function loadTotalBookingChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: '/Dashboard/GetTotalBookingsChartData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            // Handle the response data
            document.querySelector("#spanTotalBookingCount").innerHTML = data.totalCount;
            var sectionBookingRatioHtml = document.createElement("span");
            if (data.hasRatioIncreased) {
                sectionBookingRatioHtml.className = "text-success me-1";
                sectionBookingRatioHtml.innerHTML = "<i class='bi bi-arrow-up-right-circle me-1'></i><span>+" + data.increaseDecreaseAmount + "</span>";
            }
            else {
                sectionBookingRatioHtml.className = "text-danger me-1";
                sectionBookingRatioHtml.innerHTML = "<i class='bi bi-arrow-down-right-circle me-1'></i><span>+" + data.increaseDecreaseAmount + "</span>";
            }
            document.querySelector("#sectionBookingRatio").append(sectionBookingRatioHtml);
            document.querySelector("#sectionBookingRatio").append("since last month");

            loadRadialBarChart("totalBookingsRadialChart", data);

            $(".chart-spinner").hide();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', errorThrown);
        }
    });
}


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

