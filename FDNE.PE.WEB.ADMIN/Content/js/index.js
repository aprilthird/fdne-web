var Profile = function () {

    var editForm = null;
    var editBtn = null;

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/perfil`)
                })
                    .always(function () {
                        _app.loader.hide();
                    })
                    .done(function(result) {
                        let formElements = $("#form");
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='PaternalSurname']").val(result.paternalSurname);
                        formElements.find("[name='MaternalSurname']").val(result.maternalSurname);
                        formElements.find("[name='Email']").val(result.email);
                        formElements.find("[name='Document']").val(result.document);
                        formElements.find("[name='DocumentTypeId']").val(result.documentTypeId).trigger("change");
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
                $(formElement).find("textbox").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/api/perfil`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        editBtn.stop();
                        $(formElement).find("textbox").prop("disabled", false);
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
    }

    var ladda = {
        init: function () {
            editBtn = Ladda.create($("#form button[type='submit']")[0]);
        }
    };

    return {
        init: function () {
            select2.init();
            validate.init();
            ladda.init();
            form.load.edit();
        }
    };
}();

$(function () {
    Profile.init();
});