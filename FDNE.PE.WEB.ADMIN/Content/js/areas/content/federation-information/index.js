var FederationInformation = function () {

    var editForm = null;
    var editBtn = null;

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/contenido/informacion-general`)
                })
                    .always(function () {
                        _app.loader.hide();
                    })
                    .done(function(result) {
                        let formElements = $("#form");
                        formElements.find("[name='Mision']").val(result.mision);
                        formElements.find("[name='Vision']").val(result.vision);
                        formElements.find("[name='AboutUs']").val(result.aboutUs);
                });
            }
        },
        submit: {
            edit: function (formElement) {
                let data = $(formElement).serialize();
                editBtn.start();
                $(formElement).find("textarea").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/api/contenido/informacion-general`),
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
                        if (error.responseText) {
                            $("#alert_text").html(error.responseText);
                            $("#alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.edit.error();
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

    return {
        init: function () {
            validate.init();
            ladda.init();
            form.load.edit();
        }
    };
}();

$(function () {
    FederationInformation.init();
});