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

document.getElementById('savePin').addEventListener('click', function () {

    var title = document.getElementById('pinTitle').value;
    var description = document.getElementById('pinDescription').value;
    var imageInput = document.getElementById('pinImage');
    var imageFile = imageInput.files[0];
    var username = document.getElementById('currentUser').getAttribute('dataUsername');

    var newPin = {
        Owner: username,
        Latitude: lat,
        Longitude: lng,
        Title: title,
        Description: description,
    };

    var marker = L.marker([lat, lng]).addTo(map)

    if (imageFile) {
        var reader = new FileReader();
        reader.onload = function (event) {
            var imageUrl = event.target.result;
            marker.on('click', function () {
                document.getElementById('info').innerHTML = '<b>Created by: ' + newPin.Owner + '</br>Title: ' + title + '</b><br>Description: ' + description + '<br><img src="' + imageUrl + '" alt="' + title + '" style="max-width: 100%; height: auto;">';
            });
        };

        reader.readAsDataURL(imageFile);
    } else {
        marker.on('click', function () {
            document.getElementById('info').innerHTML = '<b>Created by: ' + newPin.Owner + '</br>Title: ' + title + '</b><br>Description: ' + description;
        });
    }

    //Ajax POST request to send new pin data
    $.ajax({
        url: '/Privacy',
        type: 'POST',
        dataType: 'text',
        contentType: "application/json",
        data: JSON.stringify(newPin)
    });

    var pinForm = document.getElementById('pinCreateForm');
    pinForm.style.display = 'none';
    clearForm();
    addingPin = false;
});
document.getElementById('cancelPin').addEventListener('click', function () {
    var pinForm = document.getElementById('pinCreateForm');
    clearForm();
    pinForm.style.display = 'none';
    addingPin = false;
});


map.on('click', onMapClick);