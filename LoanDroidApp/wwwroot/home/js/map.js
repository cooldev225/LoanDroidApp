 function initMap() {
     var myLatLng = {
         lat: 23.0225,
         lng: 72.5714
     };
     var map = new google.maps.Map(document.getElementById('googleMap'), {
         zoom: 8,
         center: myLatLng,
         scrollwheel: false,

     });
     var image = 'images/map-pin.png';
     var marker = new google.maps.Marker({
         position: myLatLng,
         map: map,
         icon: image,
         title: 'Hello World!'

     });
 }