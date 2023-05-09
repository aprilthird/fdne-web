var Guidelines = function () {

    var editForm = null;
    var editBtn = null;

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/contenido/pautas`)
                })
                    .done(function (result) {
                        let formElements = $("#form");
                        formElements.find("[name='Id']").val(result.id);
                        $("#form .summernote").summernote("code", result.body);
                    })
                    .always(function () {
                        _app.loader.hide();
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
                    editBtn.start();
                    $(formElement).find("input, textarea").prop("disabled", true);
                    let id = $(formElement).find("input[name='Id']").val();
                    $.ajax({
                        url: _app.parseUrl(`/api/contenido/pautas/${id}`),
                        method: "put",
                        data: data
                    })
                        .always(function () {
                            editBtn.stop();
                            $(formElement).find("input, textarea").prop("disabled", false);
                        })
                        .done(function (result) {
                            _app.show.notification.edit.success();
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#alert_text").html(error.responseText);
                                $("#alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.edit.error();
                        });
                });         
            }
        },
        reset: {
            edit: function () {
                editForm.resetForm();
                $("#form").trigger("reset");
                $("#alert").removeClass("show").addClass("d-none");
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


    var summernote = {
        init: function () {
            $("#form .summernote").summernote({
                height: 200,
                popover: {
                    image: [],
                    link: [],
                    air: []
                },
                    acks: {
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
            summernote.init();
            form.load.edit();
        }
    };
}();

$(function () {
    Guidelines.init();
});
