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

    //Sending pin data on map click, used for testing purposes.
    /*
    document.getElementById('owner').value = "a owner";
    document.getElementById('latitude').value = lat;
    document.getElementById('longitude').value = lng;
    document.getElementById('title').value = "a title";
    document.getElementById('description').value = "a description";
    document.getElementById('SendPinDataForm').submit();
    */

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


    var marker = L.marker([lat, lng]).addTo(map)

    if (imageFile) {
        var reader = new FileReader();
        reader.onload = function (event) {
            var imageUrl = event.target.result;

            marker.on('click', function () {
                document.getElementById('info').innerHTML = '<b>' + title + '</b><br>' + description + '<br><img src="' + imageUrl + '" alt="' + title + '" style="max-width: 100%; height: auto;">';
            });
        };

        reader.readAsDataURL(imageFile);
    } else {
        marker.on('click', function () {
            document.getElementById('info').innerHTML = '<b>' + title + '</b><br>' + description;
        });
    }

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