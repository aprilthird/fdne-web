var Coaching = function () {

    var editForm = null;
    var editBtn = null;

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/contenido/coaching-system`)
                })
                    .always(function () {
                        _app.loader.hide();
                    })
                    .done(function (result) {
                        let formElements = $("#form");
                        formElements.find("[name='FirstTab']").val(result.firstTab);
                        $("#form .firstBody").summernote("code", result.firstBody);
                        formElements.find("[name='SecondTab']").val(result.secondTab);
                        $("#form .secondBody").summernote("code", result.secondBody);
                        formElements.find("[name='ThirdTab']").val(result.thirdTab);
                        $("#form .thirdBody").summernote("code", result.thirdBody);
                        formElements.find("[name='QuarterTab']").val(result.quarterTab);
                        $("#form .quarterBody").summernote("code", result.quarterBody);
                        formElements.find("[name='FifthTab']").val(result.fifthTab);
                        $("#form .fifthBody").summernote("code", result.fifthBody);
                        formElements.find("[name='SixthTab']").val(result.sixthTab);
                        $("#form .sixthBody").summernote("code", result.sixthBody);
                    });
            }
        },
        submit: {
            edit: function (formElement) {
                let data = $(formElement).serialize();
                editBtn.start();
                $(formElement).find("textarea").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl(`/api/contenido/coaching-system`),
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
                //editForm.resetForm();
                //$("#form").trigger("reset");
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

    var summernoteFirst = {
        init: function () {
            $("#form .firstBody").summernote({
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

    var summernoteSecond = {
        init: function () {
            $("#form .secondBody").summernote({
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

    var summernoteThird = {
        init: function () {
            $("#form .thirdBody").summernote({
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

    var summernoteQuarter = {
        init: function () {
            $("#form .quarterBody").summernote({
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
    
    var summernoteFifth = {
        init: function () {
            $("#form .fifthBody").summernote({
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

    var summernoteSixth = {
        init: function () {
            $("#form .sixthBody").summernote({
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
            summernoteFirst.init();
            summernoteSecond.init();
            summernoteThird.init();
            summernoteQuarter.init();
            summernoteFifth.init();
            summernoteSixth.init();
            form.load.edit();
        }
    };
}();

$(function () {
    Coaching.init();
});