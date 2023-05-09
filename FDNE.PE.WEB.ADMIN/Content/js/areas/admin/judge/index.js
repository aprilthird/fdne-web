var judge = function () {

    var judgeDatatable = null;
    var addForm = null;
    var addBtn = null;
    var editForm = null;
    var editBtn = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/jueces"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Documento",
                data: null,
                render: function (data, type, row) {
                    return `${row.document} (${row.documentType.name})`;
                }
            },
            { title: "Juez", data: "fullName" },
            { title: "Telefono", data: "phoneNumber" },
            {
                title: "Sexo",
                data: null,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.sex)
                        tmp = '<p>Mujer</p>';
                    else
                        tmp = '<p>Hombre</p>';
                    return tmp;
                }
            },           
            { title: "Fecha de Nacimiento", data: "birthDate" },
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
            judgeDatatable = $("#judge_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            judgeDatatable.ajax.reload();
        },
        initEvents: function () {
            judgeDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            judgeDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El juez será eliminado permanentemente",
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
                                    url: _app.parseUrl(`/api/jueces/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El juez ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar al juez"
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
                    url: _app.parseUrl(`/api/jueces/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='UserName']").val(result.userName);
                        formElements.find("[name='Email']").val(result.email);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='PaternalSurname']").val(result.paternalSurname);
                        formElements.find("[name='MaternalSurname']").val(result.maternalSurname);
                        formElements.find("[name='Document']").val(result.document);
                        formElements.find("[name='DocumentTypeId']").val(result.documentTypeId).trigger("change");
                        formElements.find("[name='PhoneNumber']").val(result.phoneNumber);
                        formElements.find("[name='Sex']").val(result.sex).trigger("change");
                        formElements.find("[name='BirthDate']").datepicker("update", result.birthDate).trigger("change");
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
                        url: _app.parseUrl("/api/jueces"),
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
                        url: _app.parseUrl(`/api/jueces/${id}`),
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
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.documentType.init();
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
        }
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                rules: {
                    Base64Image: { // Este es el name o id de tu input file que quieres validar
                        fileSizeMBs: 20
                    }
                },
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                rules: {
                    Base64Image: { // Este es el name o id de tu input file que quieres validar
                        fileSizeMBs: 20
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
            $("#add_form [name='DocumentTypeId']").attr("id", "add_documentTypeId");
            $("#edit_form [name='DocumentTypeId']").attr("id", "edit_documentTypeId");
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            modals.init();
            select2.init();
            datepicker.init();
            ladda.init();
        }
    };
}();

$(function () {
    judge.init();
});
