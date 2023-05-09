var Discipline = function() {

    var disciplineDatatable = null;
    var addForm = null;
    var addBtn = null;
    var editForm = null;
    var editBtn = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/disciplinas"),
            dataSrc: ""
        },
        columns: [
            { title: "Nombre", data: "name" },
            { title: "Descripción", data: "description" },
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
            disciplineDatatable = $("#discipline_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function() {
            disciplineDatatable.ajax.reload();
        },
        initEvents: function() {
            disciplineDatatable.on("click",
                ".btn-edit",
                function() {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            disciplineDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La disciplina será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/api/disciplinas/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La disciplina ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar la disciplina"
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
                    url: _app.parseUrl(`/api/disciplinas/${id}`)
                    })
                    .done(function(result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Description']").val(result.description);
                        $("#edit_modal").modal("show");
                    })
                    .always(function() {
                        _app.loader.hide();
                    });
            }
        },
        submit: {
            add: function (formElement) {
                let data = $(formElement).serialize();
                addBtn.start();
                $(formElement).find("input, textarea").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/api/disciplinas"),
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
            },
            edit: function(formElement) {
                let data = $(formElement).serialize();
                editBtn.start();
                $(formElement).find("input, textarea").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                        url: _app.parseUrl(`/api/disciplinas/${id}`),
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
                submitHandler: function(formElement, e) {
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

    var modals = {
        init: function() {
            $("#add_modal").on("hidden.bs.modal",
                function() {
                    form.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
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

    return {
        init: function () {
            datatable.init();
            validate.init();
            modals.init();
            ladda.init();
        }
    };
}();

$(function() {
    Discipline.init();
});