function searchLocation() {
    var searchTerm = document.getElementById('searchInput').value;
    var category = document.getElementById('categoryDropdown').value;

    var url = 'https://nominatim.openstreetmap.org/search?format=json&q=' + searchTerm;

    if (category) {
        url += '&category=' + category;
    }


    $.ajax({
        url: url,
        type: 'GET',
       success: function (data) {
            if (data && data.length > 0) {
                var lat = parseFloat(data[0].lat);
                var lon = parseFloat(data[0].lon);
                map.setView([lat, lon], 13);
                // Check if bounding box data is available
                if (data[0].boundingbox) {
                    var bbox = data[0].boundingbox.map(Number);
                    var southWest = L.latLng(bbox[0], bbox[2]);
                    var northEast = L.latLng(bbox[1], bbox[3]);
                    var bounds = L.latLngBounds(southWest, northEast);
                    map.fitBounds(bounds);
                } else {
                    // Default to a zoom level of 13 if bounding box is not available
                    map.setView([lat, lon], 13);
                }
            } else {
                alert('Location not found');
            }
        },
        error: function () {
            alert('Error occurred while searching for location');
        }
   });
}