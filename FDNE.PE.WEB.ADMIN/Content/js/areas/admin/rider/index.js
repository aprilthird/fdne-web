var rider = function () {

    var riderDatatable = null;
    var addForm = null;
    var addBtn = null;
    var editForm = null;
    var editBtn = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/jinetes"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Documento",
                data: null,
                render: function (data, type, row) {
                    return `${row.user.document} (${row.user.documentType.name})`;
                }
            },
            { title: "Jinete", data: "user.fullName" },
            { title: "Telefono", data: "user.phoneNumber" },            
            {
                title: "Sexo",
                data: null,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.user.sex)
                        tmp = '<p>Mujer</p>';
                    else
                        tmp = '<p>Hombre</p>';
                    return tmp;
                }
            },   
            { title: "Fecha de Nacimiento", data: "user.birthDate" },
            {
                title: "Club",
                data: "clubs",
                render: function (data, type, row) {
                    let tmp = "";
                    $.each(data, (i, e) => {
                        tmp += e.name + (data.length > 1 ? i - 1 === data.length ? ", " : " y " : "");
                    });
                    return tmp;
                }
            },
            {
                title: "Activo",
                data: null,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.isActive)
                        tmp = '<p>Activo</p>';
                    else
                        tmp = '<p>Inactivo</p>';
                    return tmp;
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
            riderDatatable = $("#rider_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            riderDatatable.ajax.reload();
        },
        initEvents: function () {
            riderDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            riderDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El jinete será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/api/jinetes/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El jinete ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el jinete"
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
                    url: _app.parseUrl(`/api/jinetes/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='User.UserName']").val(result.user.userName);
                        formElements.find("[name='User.Email']").val(result.user.email);
                        formElements.find("[name='User.Name']").val(result.user.name);
                        formElements.find("[name='User.PaternalSurname']").val(result.user.paternalSurname);
                        formElements.find("[name='User.MaternalSurname']").val(result.user.maternalSurname);
                        formElements.find("[name='User.Document']").val(result.user.document);
                        formElements.find("[name='User.PhoneNumber']").val(result.user.phoneNumber);
                        formElements.find("[name='User.Sex']").val(result.user.sex).trigger("change");
                        formElements.find("[name='User.BirthDate']").datepicker("update", result.user.birthDate).trigger("change");
                        formElements.find("[name='ClubIds']").val(result.clubIds).trigger("change");
                        formElements.find("[name='DocumentTypeId']").val(result.documentTypeId).trigger("change");
                        formElements.find("[name='IsActive']").prop("checked", result.isActive).trigger("change");
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let file = $(formElement).find("[name='Base64Image']").get(0).files[0];      
                _app.parseToBase64(file, function (parsedFile) {
                    let data = $(formElement).serialize();
                    if (parsedFile) {
                        data += '&Base64Image=' + parsedFile;
                    }

                    addBtn.start();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/api/jinetes"),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            addBtn.stop();
                            $(formElement).find("input, select").prop("disabled", false);
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
                let file = $(formElement).find("[name='Base64Image']").get(0).files[0];    
                _app.parseToBase64(file, function (parsedFile) {
                    let data = $(formElement).serialize();
                    if (parsedFile) {
                        data += '&Base64Image=' + parsedFile;
                    }

                    editBtn.start();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/api/jinetes/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            editBtn.stop();
                            $(formElement).find("input, select").prop("disabled", false);
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
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form .select2-document-types").attr("selectedIndex", 0).trigger("change");
                $("#add_form .select2-clubs").val([]).trigger("change");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form .select2-document-types").attr("selectedIndex", 0).trigger("change");
                $("#edit_form .select2-clubs").val([]).trigger("change");
            }
        }
    };

    var select2 = {
        init: function () {
            this.documentType.init();
            this.club.init();
        },
        documentType: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/api/tipos-de-documento")
                }).done(function (result) {
                    let data = $.map(result,
                        function (obj) {
                            obj.id = obj.id || obj.id;
                            obj.text = obj.text || obj.name;
                            return obj;
                        });
                    $(".select2-document-types").select2({
                        data: data,
                        placeholder: "Tipos de Documento",
                        minimumResultsForSearch: -1
                    });
                });
            }
        },
        club: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/api/clubes")
                }).done(function (result) {
                    let data = $.map(result,
                        function (obj) {
                            obj.id = obj.id || obj.id;
                            obj.text = obj.text || obj.name;
                            return obj;
                        });
                    $(".select2-clubs").select2({
                        data: data,
                        placeholder: "Clubes"
                    });
                });
            }
        }
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                rules: {
                    "User.Password": {
                        pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&._-]).{6,}$/
                    }
                },
                messages: {
                    "User.Password": {
                        pattern: "La contraseña debe contener al menos un dígito, una mayúscula, minúscula, un caracter especial y ser de 6 caracteres de largo como mínimo."
                    }
                },
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                rules: {
                    "User.Password": {
                        pattern: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&._-]).{6,}$/
                    }
                },
                messages: {
                    "User.Password": {
                        pattern: "La contraseña debe contener al menos un dígito, una mayúscula, minúscula, un caracter especial y ser de 6 caracteres de largo como mínimo."
                    }
                },
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

    var datepicker = {
        init: function () {
            $(".datepicker").datepicker();
        }
    };

    var ladda = {
        init: function () {
            addBtn = Ladda.create($("#add_form button[type='submit']")[0]);
            editBtn = Ladda.create($("#edit_form button[type='submit']")[0]);
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='ClubIds']").attr("id", "add_clubIds");
            $("#edit_form [name='ClubIds']").attr("id", "edit_clubIds");
            $("#add_form [name='User.DocumentTypeId']").attr("id", "add_documentTypeId");
            $("#edit_form [name='User.DocumentTypeId']").attr("id", "edit_documentTypeId");
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            modals.init();
            ladda.init();
            select2.init();
            datepicker.init();
        }
    };
}();

$(function () {
    rider.init();
});