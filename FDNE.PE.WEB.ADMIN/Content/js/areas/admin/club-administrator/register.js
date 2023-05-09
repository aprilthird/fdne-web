var clubAdmin = function () {

    var addForm = null;
    var addBtn = null;

    var form = {
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
                        url: _app.parseUrl("/api/administradores-de-club"),
                        method: "post",
                        data: data
                    })
                        .always(function () {
                            addBtn.stop();
                            $(formElement).find("input, select").prop("disabled", false);
                        })
                        .done(function (result) {
                            _app.show.notification.add.success();
                            document.getElementsByClassName("card")[0].style.display = "none";
                            document.getElementsByClassName("alert")[0].style.display = "block";
                        })
                        .fail(function (error) {
                            if (error.responseText) {
                                $("#error_alert_text").html(error.responseText);
                                $("#error_alert").removeClass("d-none").addClass("show");
                            }
                            _app.show.notification.add.error();
                        });
                });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            }
        }
    };

    var select2 = {
        init: function () {
            this.club.init();
            this.documentType.init();
            this.sex.init();
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
                        placeholder: "Clubes",
                        minimumResultsForSearch: -1
                    });
                });
            }
        },
        sex: {
            init: function () {
                $(".select2-sex").select2({
                    placeholder: "Sexo",
                    minimumResultsForSearch: -1
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
        }
    };

    return {
        init: function () {
            validate.init();
            ladda.init();
            select2.init();
            datepicker.init();
        }
    };
}();

$(function () {
    clubAdmin.init();
});
