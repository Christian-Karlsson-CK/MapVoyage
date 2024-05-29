var markers = [];

// Log LoadedPins to verify the data
console.log('LoadedPins:', LoadedPins);

// Since LoadedPins is already defined in the HTML, you don't need to fetch it again
LoadedPins.forEach(pin => {
    if (pin.Latitude && pin.Longitude && pin.Title) {
        var marker = L.marker([pin.Latitude, pin.Longitude]).addTo(map).bindPopup(pin.Title);
        markers.push({ marker: marker, category: 'pins' });
        console.log(`Added pin: ${pin.Title} at (${pin.Latitude}, ${pin.Longitude})`);
    } else {
        console.warn('Invalid pin data:', pin);
    }
});
function updateSidebar(pin) {
    var sidebarContent = `<h3>${pin.Title}</h3>
                         <p><b>Description:</b> ${pin.Description}</p>
                         <p><b>Owner:</b> ${pin.Owner}</p>`;
    if (pin.ImageLink) {
        sidebarContent += `<img src="${pin.ImageLink}" alt="${pin.Title}" style="max-width:100%;"><br>`;
    }
    document.getElementById('info').innerHTML = sidebarContent;
}


// category works ; the foundpin does not work
function searchLocation() {
    var searchTerm = document.getElementById('searchInput').value;
    var category = document.getElementById('categoryDropdown').value;

    console.log(`Searching for: ${searchTerm} in category: ${category}`);

    if (category === "pins") {
        // Add more debugging logs
        console.log('LoadedPins:', LoadedPins);

        var foundPin = LoadedPins.find(pin => {
            // Debug log to identify the problematic entry
            console.log('Checking pin:', pin);
            return pin.Title && pin.Title.toLowerCase().includes(searchTerm);
        });

        if (foundPin) {
            console.log(`Found pin: ${foundPin.Title} at (${foundPin.Latitude}, ${foundPin.Longitude})`);
            map.setView([foundPin.Latitude, foundPin.Longitude], 15);
            updateSidebar(foundPin);
        } else {
            alert('Destination not found');
            console.error('Destination not found');
        }
    } else {
        var url = 'https://nominatim.openstreetmap.org/search?format=json&q=' + searchTerm;
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

}


