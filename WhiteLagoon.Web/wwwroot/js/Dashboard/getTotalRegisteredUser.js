
$(document).ready(function () {
    loadTotalRegisteredUserChart();
});

function loadTotalRegisteredUserChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: '/Dashboard/GetRegisteredUserChartData',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            // Handle the response data
            document.querySelector("#spanTotalRegisteredUserCount").innerHTML = data.totalCount;
            var sectionUserRegisterdRatioHtml = document.createElement("span");
            if (data.hasRatioIncreased) {
                sectionUserRegisterdRatioHtml.className = "text-success me-1";
                sectionUserRegisterdRatioHtml.innerHTML = "<i class='bi bi-arrow-up-right-circle me-1'></i><span>+" + data.increaseDecreaseAmount + "</span>";
            }
            else {
                sectionUserRegisterdRatioHtml.className = "text-danger me-1";
                sectionUserRegisterdRatioHtml.innerHTML = "<i class='bi bi-arrow-down-right-circle me-1'></i><span>+" + data.increaseDecreaseAmount + "</span>";
            }
            document.querySelector("#sectionUserRegisterdRatio").append(sectionUserRegisterdRatioHtml);
            document.querySelector("#sectionUserRegisterdRatio").append("since last month");

            loadRadialBarChart("totalRegisteredUserRadialChart", data);

            $(".chart-spinner").hide();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.error('Error:', errorThrown);
        }
    });
}

