window.onload = function () {
    $.getJSON('https://api.ipify.org?format=jsonp&callback=?', function (data) {
        if (data.ip) {
            getMapChartByIpAddress(data.ip);
        }
    });
}

function getMapChartByIpAddress(ipAddress) {
    $.ajax({
        type: "POST",
        url: '/Home/GetMapChartData',
        data: { ipAddress: ipAddress },
        success: function (response) {
            generateMapChart(response.data, response.apiKey)
        },
        error: function () {
            console.log("An error occurred during the ajax call to generate the chart!");
        }
    });
}

function generateMapChart(mapData, apiKey) {
    google.charts.load(
    'current',
    {
        'packages': ['map'],
        'mapsApiKey': apiKey
    });
    google.charts.setOnLoadCallback(drawMap);

    function drawMap() {
        var chartData = [['City', 'Name']];

        mapData.forEach(function (element) {
            var newItem = [];
            newItem.push(element.cityName);
            newItem.push(element.message);
            chartData.push(newItem);
        });

        var data = google.visualization.arrayToDataTable(chartData);

        var options = {
            showTooltip: true,
            showInfoWindow: true,
            zoom: 1,
            minZoom: 1
        };

        var map = new google.visualization.Map(document.getElementById('chart_div'));
        map.draw(data, options);
        $('#loading').hide();
        $('#mapping').show();
    };
}