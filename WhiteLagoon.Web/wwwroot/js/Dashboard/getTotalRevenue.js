
$(document).ready(function () {
    loadTotalRevenueChart();
});

function loadTotalRevenueChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: '/Dashboard/GetTotalRevenueChartData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            // Handle the response data
            document.querySelector("#spanTotalRevenueCount").innerHTML = data.totalCount;
            var sectionRevenueRatioHtml = document.createElement("span");
            if (data.hasRatioIncreased) {
                sectionRevenueRatioHtml.className = "text-success me-1";
                sectionRevenueRatioHtml.innerHTML = "<i class='bi bi-arrow-up-right-circle me-1'></i><span>+" + data.increaseDecreaseAmount + "</span>";
            }
            else {
                sectionRevenueRatioHtml.className = "text-danger me-1";
                sectionRevenueRatioHtml.innerHTML = "<i class='bi bi-arrow-down-right-circle me-1'></i><span>+" + data.increaseDecreaseAmount + "</span>";
            }
            document.querySelector("#sectionRevenueRatio").append(sectionRevenueRatioHtml);
            document.querySelector("#sectionRevenueRatio").append("since last month");

            loadRadialBarChart("totalRevenueRadialChart", data);

            $(".chart-spinner").hide();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', errorThrown);
        }
    });
}

