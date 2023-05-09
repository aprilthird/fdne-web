var History = function () {

    var historyDatatable = null;

    var imgForm = null;
    var editImgBtn = null;

    var editForm = null;
    var editBtn = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/federation-historia/periodos"),
            dataSrc: ""
        },
        columns: [
            { title: "Año", data: "year" },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            officerDatatable = $("#history_datatable").DataTable(options);
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
        }
    };

    var formImage = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/federation-historia`)
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
                        url: _app.parseUrl(`/api/federation-historia/${id}`),
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
                    url: _app.parseUrl(`/api/federation-historia/periodos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Year']").val(result.year);
                        $("#edit_modal .summernote").summernote("code", result.body);
                        $("#edit_modal").modal("show");
                    })
                    .always(function () {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            edit: function (formElement) {
                let data = $(formElement).serialize();
                editBtn.start();
                $(formElement).find("input, textarea").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/api/federation-historia/periodos/${id}`),
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
                        console.log(error);
                        _app.show.notification.edit.error();
                    });
            }
        },
        reset: {
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
            editBtn = Ladda.create($("#edit_form button[type='submit']")[0]);
        }
    };

    var summernote = {
        init: function () {
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
            validate.init();
            ladda.init();
            formImage.load.edit();
            datatable.init();
            summernote.init();
        }
    };
}();

$(function () {
    History.init();
});
