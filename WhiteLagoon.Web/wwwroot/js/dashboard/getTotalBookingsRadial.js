
$(document).ready(function () {
    loadTotalBookingRadialChart();
});

function loadTotalBookingRadialChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetTotalBookingRadialChartData",
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            document.querySelector("#spanTotalBookingCount").innerHTML = data.totalCount;

            var sectionCurrentCount = document.createElement("span");
            if (data.hasRatioIncreased) {
                sectionCurrentCount.className = "text-success me-1";
                sectionCurrentCount.innerHTML = '<i class="bi bi-arrow-up-right-circle me-1"></i> <span> ' + data.countInCurrentMonth + '</span>';
            }
            else {
                sectionCurrentCount.className = "text-danger me-1";
                sectionCurrentCount.innerHTML = '<i class="bi bi-arrow-down-right-circle me-1"></i> <span> ' + data.countInCurrentMonth + '</span>';
            }

            document.querySelector("#sectionBookingCount").append(sectionCurrentCount);
            document.querySelector("#sectionBookingCount").append("since last month");

            loadRadialBarChart("totalBookingsRadialChart", data);

            $(".chart-spinner").hide();
        }
    });
}

