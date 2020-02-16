window.onload = function () {
    $.getJSON('https://api.ipify.org?format=jsonp&callback=?', function (data) {
        if (data.ip) {
            persistNewAccess(data.ip);
        }
    });
}

function persistNewAccess(ipAddress) {
    $.ajax({
        type: "POST",
        url: '/Home/PersistNewAccess',
        data: { ipAddress: ipAddress },
        error: function () {
            console.log("An error occurred during the ajax call to persist new access!");
        }
    });
}