var Club = function () {

    var clubDatatable = null;
    var addForm = null;
    var addBtn = null;
    var editForm = null;
    var editBtn = null;
    var addMapComponent = null;
    var editMapComponent = null;
    var addMarker = null;
    var editMarker = null;
    var addGeocoder = null;
    var editGeocoder = null;

    var options = {
        responsive: false,
        ajax: {
            url: _app.parseUrl("/api/clubes"),
            dataSrc: ""
        },
        columns: [
            { title: "Nombre", data: "name" },
            { title: "Acrónimo", data: "acronym" },
            { title: "Dirección", data: "address" },
            {
                title: "Correo",
                data: "email",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Teléfono",
                data: "phoneNumber",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button>`;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            clubDatatable = $("#club_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            clubDatatable.ajax.reload();
        },
        initEvents: function () {
            clubDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            clubDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El club será eliminado permanentemente",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarlo",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: _app.parseUrl(`/api/clubes/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El club ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el club"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });

        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/clubes/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Acronym']").val(result.acronym);
                        formElements.find("[name='Email']").val(result.email);
                        formElements.find("[name='PhoneNumber']").val(result.phoneNumber);
                        map.setEditTo(result.latitude, result.longitude);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let image = $(formElement).find("[name='Base64Image']").get(0).files[0];
                _app.parseToBase64(image, function (parsedImage) {

                    let data = $(formElement).serialize();
                    if (parsedImage) {
                        data += '&Base64Image=' + parsedImage;
                   
                    }
                    addBtn.start();
                    $(formElement).find("input, textarea").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/api/clubes"),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            addBtn.stop();
                            $(formElement).find("input, textarea").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatable.reload();
                            $("#add_modal").modal("hide");
                            _app.show.notification.add.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#add_alert_text").html(error.responseText);
                                $("#add_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                });
            },
            edit: function (formElement) {
                let image = $(formElement).find("[name='Base64Image']").get(0).files[0];
                _app.parseToBase64(image, function (parsedImage) {
                    let data = $(formElement).serialize();
                    if (parsedImage) {
                        data += '&Base64Image=' + parsedImage;
                    }
                    editBtn.start();
                    $(formElement).find("input, textarea").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/api/clubes/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            editBtn.stop();
                            $(formElement).find("input, textarea").prop("disabled", false);
                        })
                        .done(function (result) {
                            datatable.reload();
                            $("#edit_modal").modal("hide");
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#edit_alert_text").html(error.responseText);
                                $("#edit_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form [name='Name']").val("");
                $("#add_form [name='Acronym']").val("");
                $("#add_form [name='Email']").val("");
                $("#add_form [name='PhoneNumber']").val("");
                map.setAddToDefault();
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            }
        }
    };
    
    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });
        }
    };

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.edit();
                });
        }
    };

    var ladda = {
        init: function () {
            addBtn = Ladda.create($("#add_form button[type='submit']")[0]);
            editBtn = Ladda.create($("#edit_form button[type='submit']")[0]);
        }
    };

    var map = {
        init: function () {
            var lima = { lat: -12.0262674, lng: -77.1282085 };
            addMapComponent = new google.maps.Map(
                document.querySelector("#add_form .map-container"), { zoom: 12, center: lima });
            editMapComponent = new google.maps.Map(
                document.querySelector("#edit_form .map-container"), { zoom: 12, center: lima });
            addGeocoder = new google.maps.Geocoder();
            editGeocoder = new google.maps.Geocoder();
            this.initEvents();
        },
        initEvents: function () {
            addMapComponent.addListener("idle", function () {
                _app.loader.showOnElement("#add_form .map-container");
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
                        _app.loader.hideOnElement("#add_form .map-container");
                        $("#add_form [name='Address']").val(results[0].formatted_address);
                    }
                });
                $("#add_form [name='Longitude']").val(addMarker.position.lng());
                $("#add_form [name='Latitude']").val(addMarker.position.lat());
            });
            editMapComponent.addListener("idle", function () {
                _app.loader.showOnElement("#edit_form .map-container");
                if (editMarker !== null)
                    editMarker.setMap(null);
                editMarker = new google.maps.Marker({
                    position: editMapComponent.getCenter(),
                    map: editMapComponent,
                    title: "Ubicación del Club"
                });
                let latLng = new google.maps.LatLng(editMarker.position.lat(), editMarker.position.lng());
                editGeocoder.geocode({ latLng: latLng }, function (results, status) {
                    if (status === "OK") {
                        _app.loader.hideOnElement("#edit_form .map-container");
                        $("#edit_form [name='Address']").val(results[0].formatted_address);
                    }
                });
                $("#edit_form [name='Longitude']").val(editMarker.position.lng());
                $("#edit_form [name='Latitude']").val(editMarker.position.lat());
            });
        },
        setEditTo: function (lat, lng) {
            let latLng = new google.maps.LatLng(lat, lng);
            if (editMarker !== null) {
                editMarker.setMap(null);
                editMarker = null;
            }
            editMarker = new google.maps.Marker({
                position: latLng,
                map: editMapComponent,
                title: "Ubicación del Club"
            });
            editMapComponent.setCenter(latLng);
        },
        setAddToDefault: function (lat, lng) {
            var lima = new google.maps.LatLng(-12.0262674, -77.1282085);
            if (addMarker !== null) {
                addMarker.setMap(null);
                addMarker = null;
            }
            addMarker = new google.maps.Marker({
                position: lima,
                map: addMapComponent,
                title: "Ubicación del Club"
            });
            addMapComponent.setCenter(lima);
        }
    };

    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            ladda.init();
        },
        mapCallback: {
            init: function () {
                map.init();
            }
        }
    };
}();

$(function () {
    Club.init();
});