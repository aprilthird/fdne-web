var Season = function () {

    var seasonDatatable = null;
    var addForm = null;
    var editForm = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/temporadas"),
            dataSrc: ""
        },
        columns: [
            { title: "Año", data: "year" },
            { title: "Nombre", data: "name" },
            { title: "Inicio", data: "startDate" },
            { title: "Fin", data: "endDate" },
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
            seasonDatatable = $("#season_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            seasonDatatable.ajax.reload();
        },
        initEvents: function() {
            seasonDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });

            seasonDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "La temporada será eliminada permanentemente",
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
                                    url: _app.parseUrl(`/api/temporadas/${id}`),
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "La temporada ha sido eliminada con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar la temporada"
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
                    url: _app.parseUrl(`/api/temporadas/${id}`)
                    })
                    .done(function(result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Year']").val(result.year);
                        formElements.find("[name='StartDate']").datepicker("setEndDate", result.endDate);
                        formElements.find("[name='EndDate']").datepicker("setStartDate", result.startDate);
                        formElements.find("[name='StartDate']").datepicker("update", result.startDate).trigger("change");
                        formElements.find("[name='EndDate']").datepicker("update", result.endDate).trigger("change");
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
                $(formElement).find("input").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/api/temporadas"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        addBtn.stop();
                        $(formElement).find("input").prop("disabled", false);
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
            edit: function (formElement) {
                let data = $(formElement).serialize();
                editBtn.start();
                $(formElement).find("input").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/api/temporadas/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        editBtn.stop();
                        $(formElement).find("input").prop("disabled", false);
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
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form [name='StartDate']").datepicker("update", "now");
                $("#add_form [name='EndDate']").datepicker("update", "now");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='StartDate']").datepicker("update", "now");
                $("#edit_form [name='EndDate']").datepicker("update", "now");
            }
        }
    };

    var validate = {
        init: function () {
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

    var modals = {
        init: function () {
            $("#add_modal").on("hidden.bs.modal",
                function () {
                    form.reset.add();
                });

            $("#edit_modal").on("hidden.bs.modal",
                function () {
                    form.reset.edit();
                });
        }
    };

    var datepicker = {
        init: function() {
            $("#add_form [name='StartDate']").datepicker({
                endDate: $("#add_form [name='EndDate']").val()
            }).on("changeDate", function (e) {
                $(this).valid();
                $("#add_form [name='EndDate']").datepicker("setStartDate", e.date);
            });
            $("#add_form [name='EndDate']").datepicker({
                startDate: $("#add_form [name='StartDate']").val()
            }).on("changeDate", function (e) {
                $(this).valid();
                $("#add_form [name='StartDate']").datepicker("setEndDate", e.date);
            });
            $("#edit_form [name='StartDate']").datepicker({
                endDate: $("#edit_form [name='EndDate']").val()
            }).on("changeDate", function (e) {
                $(this).valid();
                $("#edit_form [name='EndDate']").datepicker("setStartDate", e.date);
            });
            $("#edit_form [name='EndDate']").datepicker({
                startDate: $("#edit_form [name='StartDate']").val()
            }).on("changeDate", function (e) {
                $(this).valid();
                $("#edit_form [name='StartDate']").datepicker("setEndDate", e.date);
            });
            $(".datepicker").datepicker("setDate", "now");
        }
    };

    var ladda = {
        init: function () {
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
            datepicker.init();
        }
    };
}();

$(function () {
    Season.init();
});