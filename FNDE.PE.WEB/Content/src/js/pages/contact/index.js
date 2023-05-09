var FederationContact = function () {

    var addMapComponent = null;
    var addMarker = null;
    var addGeocoder = null;

    var map = {
        init: function () {
            var lima = { lat: parseFloat(document.getElementById("latitudeval").value), lng: parseFloat(document.getElementById("longitudeval").value) };
            addMapComponent = new google.maps.Map(
                document.querySelector("#map-container"), { zoom: 12, center: lima });
            addMarker = new google.maps.Marker({
                position: addMapComponent.getCenter(),
                map: addMapComponent,
                title: "Ubicación del Club"
            });
        },
    };

    return {

        mapCallback: {
            init: function () {
                map.init();
            }
        }
    };
}();

