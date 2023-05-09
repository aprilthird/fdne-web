var FederationContact = function () {

    var editForm = null;
    var editBtn = null;
    var addMapComponent = null;
    var addMarker = null;
    var addGeocoder = null;


    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/contenido/contacto`)
                })
                    .always(function () {
                        _app.loader.hide();
                    })
                    .done(function (result) {
                        let formElements = $("#form");
                       
                        map.setEditTo(result.latitude, result.longitude);
                    });
            }
        },
        submit: {
            edit: function (formElement) {
                let data = $(formElement).serialize();
                editBtn.start();
                $(formElement).find("textarea").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/api/contenido/contacto`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        editBtn.stop();
                        $(formElement).find("textarea").prop("disabled", false);
                    })
                    .done(function (result) {
                        _app.show.notification.edit.success();
                    })
                    .fail(function (error) {
                        console.log(error);
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
            },
            edit: function () {
                editForm.resetForm();
            }
        }
    };

    var validate = {
        init: function () {
            editForm = $("#form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });
        }
    };

    var ladda = {
        init: function () {
            editBtn = Ladda.create($("#form button[type='submit']")[0]);
        }
    };

    var map = {
        init: function () {
            var lima = { lat: -12.0262674, lng: -77.1282085 };

            addMapComponent = new google.maps.Map(
                document.querySelector("#form .map-container"), { zoom: 12, center: lima });
  
            addGeocoder = new google.maps.Geocoder();
            editGeocoder = new google.maps.Geocoder();
    
            this.initEvents();
        },
        initEvents: function () {
            addMapComponent.addListener("idle", function () {
                _app.loader.showOnElement("#form .map-container");
                if (addMarker !== null)
                    addMarker.setMap(null);
                addMarker = new google.maps.Marker({
                    position: addMapComponent.getCenter(),
                    map: addMapComponent,
                    title: "Ubicación del Club"
                });
                let latLng = new google.maps.LatLng(addMarker.position.lat(), addMarker.position.lng());
                addGeocoder.geocode({ latLng: latLng }, function (results, status) {
                    if (status === "OK") {
                        _app.loader.hideOnElement("#form .map-container");
                        $("#form [name='Address']").val(results[0].formatted_address);
                    }
                });
                $("#form [name='Longitude']").val(addMarker.position.lng());
                $("#form [name='Latitude']").val(addMarker.position.lat());
            });
           
        },
        setEditTo: function (lat, lng) {
            let latLng = new google.maps.LatLng(lat, lng);
            if (addMarker !== null) {
                addMarker.setMap(null);
                addMarker = null;
            }
            addMarker = new google.maps.Marker({
                position: latLng,
                map: addMapComponent,
                title: "Ubicación del Club"
            });
            addMapComponent.setCenter(latLng);
        }
    };

    return {
        init: function () {
            validate.init();
            ladda.init();
            form.load.edit();
             
        },
        mapCallback: {
            init: function () {
                map.init();
            }
        }
    };
}();

$(function () {
    FederationContact.init();
});