var officers = function () {

    var officerDatatable = null;

    var imgForm = null;
    var editImgBtn = null;

    var addForm = null;
    var addBtn = null;

    var editForm = null;
    var editBtn = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/federation-officers/oficial"),
            dataSrc: ""
        },
        columns: [
            { title: "Nombre", data: "name" },
            { title: "Categoria", data: "categoryName" },
            {
                title: "Disiplina",
                data: null,
                render: function (data, type, row) {
                    var tmp = "";
                    if (row.disciplinaName)
                        tmp = '<p>Salto</p>';
                    else
                        tmp = '<p>Adiestramiento</p>';
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
            officerDatatable = $("#officer_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            officerDatatable.ajax.reload();
        },
        initEvents: function () {
            officerDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            officerDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La persona será eliminada permanentemente",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonText: "Sí, eliminarla",
                        confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                        cancelButtonText: "Cancelar",
                        showLoaderOnConfirm: true,
                        allowOutsideClick: () => !swal.isLoading(),
                        preConfirm: () => {
                            return new Promise((resolve) => {
                                $.ajax({
                                    url: _app.parseUrl(`/api/federation-officers/oficial/${id}`),
                                    type: "delete",
                                    data: {
                                        id: id
                                    },
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La persona ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar la persona"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
        }
    };

    var formImage = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/federation-officers`)
                })
                    .always(function () {
                        _app.loader.hide();
                    })
                    .done(function (result) {
                        let formElements = $("#edit_form_image");
                        formElements.find("[name='Id']").val(result.id);
                    });
            }
        },
        submit: {
            edit: function (formElement) {
                let file = $(formElement).find("[name='Base64Image']").get(0).files[0];

                _app.parseToBase64(file, function (parsedFile) {
                    let data = $(formElement).serialize();
                    if (parsedFile) {
                        data += '&Base64Image=' + parsedFile;
                    }
                    editImgBtn.start();
                    $(formElement).find("input").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/api/federation-officers/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            editImgBtn.stop();
                            $(formElement).find("input").prop("disabled", false);
                        })
                        .done(function (result) {
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            console.log(error);
                            _app.show.notification.edit.error();
                        });
                });

            }
        },
        reset: {
            edit: function () {
                imgForm.resetForm();
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/federation-officers/oficial/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='CategoryName']").val(result.categoryName);
                        formElements.find("[name='DisciplinaName']").val(result.disciplinaName);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                addBtn.start();
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/api/federation-officers/oficial"),
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
                        console.log(error);
                        _app.show.notification.add.error();
                    });
            },
            edit: function (formElement) {
                let data = $(formElement).serialize();
                editBtn.start();
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/api/federation-officers/oficial/${id}`),
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
            imgForm = $("#edit_form_image").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    formImage.submit.edit(formElement);
                }
            });

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

    var ladda = {
        init: function () {
            editImgBtn = Ladda.create($("#edit_form_image button[type='submit']")[0]);
            addBtn = Ladda.create($("#add_form button[type='submit']")[0]);
            editBtn = Ladda.create($("#edit_form button[type='submit']")[0]);
        }
    };

    return {
        init: function () {
            validate.init();
            ladda.init();
            datatable.init();
            formImage.load.edit();
        }
    };

}();

$(function () {
    officers.init();
});