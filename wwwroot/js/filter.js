function filterLocations() {
    var selectedCity = document.getElementById('cityFilter').value;

    if (selectedCity) {
        var url = 'https://nominatim.openstreetmap.org/search?format=json&city=' + selectedCity;

        $.ajax({
            url: url,
            type: 'GET',
            success: function (data) {
                if (data && data.length > 0) {
                    var lat = parseFloat(data[0].lat);
                    var lon = parseFloat(data[0].lon);
                    map.setView([lat, lon], 13);
                } else {
                    alert('Location not found');
                }
            },
            error: function () {
                alert('Error occurred while searching for location');
            }
        });
    } else {
        alert('Please select a city to filter.');
    }
}