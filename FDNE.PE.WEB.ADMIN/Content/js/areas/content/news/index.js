var News = function() {

    var newsDatatable = null;
    var addForm = null;
    var addBtn = null;
    var editForm = null;
    var editBtn = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/contenido/noticias"),
            dataSrc: ""
        },
        columns: [
            { title: "Título", data: "title" },
            { title: "Fecha de Publicación", data: "date" },
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
        init: function() {
            newsDatatable = $("#news_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function() {
            newsDatatable.ajax.reload();
        },
        initEvents: function() {
            newsDatatable.on("click",
                ".btn-edit",
                function() {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            newsDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La noticia será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/api/contenido/noticias/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La noticia ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar la noticia"
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
                    url: _app.parseUrl(`/api/contenido/noticias/${id}`)
                    })
                    .done(function(result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Title']").val(result.title);
                        $("#edit_form .summernote").summernote("code", result.body);
                        $("#edit_modal").modal("show");
                    })
                    .always(function() {
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
                        url: _app.parseUrl("/api/contenido/noticias"),
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
                        url: _app.parseUrl(`/api/contenido/noticias/${id}`),
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
            add: function() {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function() {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var validate = {
        init: function () {
            addForm = $("#add_form").validate({
                ignore: ":hidden:not(.summernote),.note-editable.panel-body",
                submitHandler: function(formElement, e) {
                    e.preventDefault();
                    form.submit.add(formElement);
                }
            });

            editForm = $("#edit_form").validate({
                ignore: ":hidden:not(.summernote),.note-editable.panel-body",
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.edit(formElement);
                }
            });
        }
    };

    var modals = {
        init: function() {
            $("#add_modal").on("hide.bs.modal",
                function() {
                    form.reset.add();
                });

            $("#edit_modal").on("hide.bs.modal",
                function() {
                    form.reset.edit();
                });
        }
    };

    var ladda = {
        init: function() {
            addBtn = Ladda.create($("#add_form button[type='submit']")[0]);
            editBtn = Ladda.create($("#edit_form button[type='submit']")[0]);
        }
    };

    var summernote = {
        init: function() {
            $("#add_form .summernote").summernote({
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
                    onChange: function(contents, $editable) {
                        // Note that at this point, the value of the `textarea` is not the same as the one
                        // you entered into the summernote editor, so you have to set it yourself to make
                        // the validation consistent and in sync with the value.
                        $(this).val($(this).summernote('isEmpty') ? "" : contents);

                        // You should re-validate your element after change, because the plugin will have
                        // no way to know that the value of your `textarea` has been changed if the change
                        // was done programmatically.
                        addForm.element($(this));
                    }
                }
            });
            $("#edit_form .summernote").summernote({
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

    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            ladda.init();
            summernote.init();
        }
    };
}();

$(function() {
    News.init();
});