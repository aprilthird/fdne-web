var fei = function () {

    var feiDatatable = null;

    var feiForm = null;
    var feiBtn = null;

    var addForm = null;
    var addBtn = null;

    var editForm = null;
    var editBtn = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/federation-fei/archivos"),
            dataSrc: ""
        },
        columns: [
            { title: "Nombre", data: "name" },
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
            officerDatatable = $("#fei_datatable").DataTable(options);
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
                        text: "El archivo será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/api/federation-fei/archivos/${id}`),
                                    type: "delete",
                                    data: {
                                        id: id
                                    },
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El archivo ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el archivo"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
        }
    };

    var formFirst = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/federation-fei`)
                })
                    .always(function () {
                        _app.loader.hide();
                    })
                    .done(function (result) {
                        let formElements = $("#edit_form_fei");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Title']").val(result.title);
                        formElements.find("[name='SubTitle']").val(result.subTitle);

                        $("#edit_form_fei .summernote").summernote("code", result.body);
                    });
            }
        },
        submit: {
            edit: function (formElement) {
                let image = $(formElement).find("[name='Base64Image']").get(0).files[0];
                let file = $(formElement).find("[name='Base64File']").get(0).files[0];

                _app.parseToBase64(image, function (parsedImage) {

                    _app.parseToBase64(file, function (parsedFile) {

                        let data = $(formElement).serialize();
                        if (parsedFile) {
                            data += '&Base64Image=' + parsedImage;
                            data += '&Base64File=' + parsedFile;
                        }
                        feiBtn.start();
                        $(formElement).find("input, textarea").prop("disabled", true);
                        let id = $(formElement).find("input[name='Id']").val();
                        $.ajax({
                            url: _app.parseUrl(`/api/federation-fei/${id}`),
                            method: "put",
                            data: data
                        })
                            .always(function () {
                                feiBtn.stop();
                                $(formElement).find("input, textarea").prop("disabled", false);
                            })
                            .done(function (result) {
                                _app.show.notification.edit.success();
                            })
                            .fail(function (error) {
                                console.log(error);
                                _app.show.notification.edit.error();
                            });

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

    var summernote = {
        init: function () {
            $("#edit_form_fei .summernote").summernote({
                height: 200,
                popover: {
                    image: [],
                    link: [],
                    air: []
                },
                callbacks: {
                    // Register the `onChnage` callback in order to listen to the changes of the
                    // Summernote editor. You can also use the event `summernote.change` to handle
                    // the change event as follow:
                    //   myElement.summernote()
                    //     .on("summernote.change", function(event, contents, $editable) {
                    //       // ...
                    //     });
                    onChange: function (contents, $editable) {
                        // Note that at this point, the value of the `textarea` is not the same as the one
                        // you entered into the summernote editor, so you have to set it yourself to make
                        // the validation consistent and in sync with the value.
                        $(this).val($(this).summernote('isEmpty') ? "" : contents);

                        // You should re-validate your element after change, because the plugin will have
                        // no way to know that the value of your `textarea` has been changed if the change
                        // was done programmatically.
                        editForm.element($(this));
                    }
                }
            });
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/federation-fei/archivos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {

                let file = $(formElement).find("[name='Base64File']").get(0).files[0];

                _app.parseToBase64(file, function (parsedFile) {

                    let data = $(formElement).serialize();
                    if (parsedFile) {
                        data += '&Base64File=' + parsedFile;
                    }
                    addBtn.start();
                    $(formElement).find("input, select").prop("disabled", true);
                    $.ajax({
                        url: _app.parseUrl("/api/federation-fei/archivos"),
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
                });               
            },
        edit: function (formElement) {
      
                let file = $(formElement).find("[name='Base64File']").get(0).files[0];
                _app.parseToBase64(file, function (parsedFile) {

                    let data = $(formElement).serialize();
                    if (parsedFile) {
                        data += '&Base64File=' + parsedFile;
                    }
                    editBtn.start();
                    $(formElement).find("input, select").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/api/federation-fei/archivos/${id}`),
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
            feiForm = $("#edit_form_fei").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    formFirst.submit.edit(formElement);
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
            feiBtn = Ladda.create($("#edit_form_fei button[type='submit']")[0]);
            addBtn = Ladda.create($("#add_form button[type='submit']")[0]);
            editBtn = Ladda.create($("#edit_form button[type='submit']")[0]);
        }
    };


    return {
        init: function () {
            validate.init();
            ladda.init();
            summernote.init();
            formFirst.load.edit();

            datatable.init();
        }
    };
}();

$(function () {
    fei.init();
});