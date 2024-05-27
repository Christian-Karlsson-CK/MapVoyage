//function searchLocation() {
//    var searchTerm = document.getElementById('searchInput').value;
//    var url = 'https://nominatim.openstreetmap.org/search?format=json&q=' + searchTerm;

//    $.ajax({
//        url: url,
//        type: 'GET',
//        success: function (data) {
//            if (data && data.length > 0) {
//                var lat = parseFloat(data[0].lat);
//                var lon = parseFloat(data[0].lon);
//                map.setView([lat, lon], 13);
//            } else {
//                alert('Location not found');
//            }
//        },
//        error: function () {
//            alert('Error occurred while searching for location');
//        }
//    });
//}
function searchLocation() {
    var category = $("#categoryDropdown").val();
    var query = $("#searchInput").val();

    $.ajax({
        url: `/api/location/search?category=${category}&query=${query}`,
        method: "GET",
        success: function (data) {
            // Clear existing markers
            map.eachLayer(function (layer) {
                if (layer instanceof L.Marker) {
                    map.removeLayer(layer);
                }
            });

            // Add new markers
            data.locations.forEach(location => {
                L.marker([location.lat, location.lng]).addTo(map)
                    .bindPopup(`<b>${location.name}</b><br>${location.type}`);
            });

            // Center map to the first location
            if (data.locations.length > 0) {
                map.setView([data.locations[0].lat, data.locations[0].lng], 13);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error fetching location data:", error);
        }
    });
}