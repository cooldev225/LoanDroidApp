 function initMap() {
     var myLatLng = {
         lat: 18.7357,
         lng: -70.1627
     };
     var map = new google.maps.Map(document.getElementById('googleMap'), {
         zoom: 8,
         center: myLatLng,
         scrollwheel: false,

     });
     var image = 'images/home/map-pin.png';
     var marker = new google.maps.Marker({
         position: myLatLng,
         map: map,
         //icon: image,
         title: 'Hello World!'

     });
 }