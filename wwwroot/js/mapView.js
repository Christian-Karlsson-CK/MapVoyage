var map = L.map('map').setView([58.31, 15.09], 13); // Center the map and set zoom level
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
}).addTo(map);



var lat, lng;
var addingPin = false;

function onMapClick(e) {
    if (!addingPin) return;

    lat = e.latlng.lat;
    lng = e.latlng.lng;

    console.log("Latitude:", lat);
    console.log("Longitude:", lng);

    var pinForm = document.getElementById('pinCreateForm');
    pinForm.style.left = e.originalEvent.pageX + 'px';
    pinForm.style.top = e.originalEvent.pageY + 'px';
    pinForm.style.display = 'block';

}

function clearForm() {
    document.getElementById('pinTitle').value = '';
    document.getElementById('pinDescription').value = '';
    document.getElementById('pinImage').value = '';
}

document.getElementById('addPinButton').addEventListener('click', function () {
    addingPin = true;
});



document.getElementById('cancelPin').addEventListener('click', function () {
    var pinForm = document.getElementById('pinCreateForm');
    clearForm();
    pinForm.style.display = 'none';
    addingPin = false;
});


function removePin(lat, lng) {
    var pinToRemove = {
        Latitude: lat,
        Longitude: lng
    };

    fetch('/Privacy?handler=RemovePin', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(pinToRemove)
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Pin removed successfully.');
                location.reload();
            } else {
                alert('Error removing pin: ' + data.message);
            }
        })
        .catch(error => console.error('Error removing pin:', error));
}


map.on('click', onMapClick);