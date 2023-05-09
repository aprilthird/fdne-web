var Event = function () {
    var calendarComponent = null;
    var addBtn = null;
    var addForm = null;
    var editBtn = null;
    var editForm = null;
    var cancelBtn = null;

    var options = {
        header: {
            left: "prev,next today",
            center: "title",
            right: "month"
        },
        buttonText: {
            today: "Hoy",
            month: "Vista Mensual",
            week: "Vista Semanal",
            day: "Vista Diaria"
        },
        firstDay: 1,
        editable: true,
        eventLimit: true,
        monthNames: _app.constants.calendar.monthNames,
        dayNames: _app.constants.calendar.dayNames,
        monthNamesShort: _app.constants.calendar.monthNamesShort,
        dayNamesShort: _app.constants.calendar.dayNamesShort,
        events: [],
        eventClick: function (event) {
            form.load.edit(event.id, function () {
                form.change.editToDetailControls(event.editable);
                $("#edit_modal").modal("show");
            });
        }
    };

    var fullCalendar = {
        init: function () {
            if (calendarComponent !== null) {
                $("#calendar").fullCalendar("destroy");
                $("#calendar").html("");
                calendarComponent = null;
            }
            $.ajax({
                url: _app.parseUrl("/api/eventos")
            }).done(function (result) {
                let events = [];
                result.forEach(function (e, index) {
                    let startDate = moment(e.startTime, "DD/MM/YYYY");
                    let startParsedDate = startDate.format("YYYY-MM-DD");
                    let endDate = moment(e.endTime, "DD/MM/YYYY").add(1, "days");
                    let endParsedDate = endDate.format("YYYY-MM-DD");
                    events.push({
                        id: e.id,
                        title: `${e.acronym}`,
                        description: e.club.name,
                        start: startParsedDate,
                        end: endParsedDate,
                        allDay: true,
                        editable: e.editable
                    });
                });
                options.events = events;
                calendarComponent = $("#calendar").fullCalendar(options);
            });
        }
    };

    var form = {
        load: {
            edit: function (id, postEdit) {
                _app.loader.show();
                $.ajax({
                    url: _app.parseUrl(`/api/eventos/${id}`)
                })
                    .done(function (result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='Acronym']").val(result.acronym);
                        formElements.find("[name='SeasonId']").val(result.seasonId).trigger("change");
                        formElements.find("[name='DisciplineId']").val(result.disciplineId).trigger("change");
                        formElements.find("[name='ClubId']").val(result.clubId).trigger("change");
                        formElements.find("[name='StartTime']").datepicker("setEndDate", result.endTime);
                        formElements.find("[name='EndTime']").datepicker("setStartDate", result.startTime);
                        formElements.find("[name='StartTime']").datepicker("update", result.startTime).trigger("change");
                        formElements.find("[name='EndTime']").datepicker("update", result.endTime).trigger("change");
                        postEdit();
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
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/api/eventos"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        addBtn.stop();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
                        fullCalendar.init();
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
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/api/eventos/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        fullCalendar.init();
                        editBtn.stop();
                        $(formElement).find("input, select").prop("disabled", false);
                    })
                    .done(function (result) {
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
        delete: function () {
            let id = $("#edit_form [name='Id']").val();
            Swal.fire({
                title: "¿Está seguro?",
                text: "El evento será eliminada permanentemente",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Sí, eliminarlo",
                confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                cancelButtonText: "Cancelar",
                showLoaderOnConfirm: true,
                allowOutsideClick: () => !swal.isLoading(),
                preConfirm: () => {
                    return new Promise((resolve) => {
                        $.ajax({
                            url: _app.parseUrl(`/api/eventos/${id}`),
                            type: "delete",
                            success: function (result) {
                                fullCalendar.init();
                                Swal.fire({
                                    type: "success",
                                    title: "Completado",
                                    text: "El evento ha sido eliminado con éxito",
                                    confirmButtonText: "Excelente",
                                    onClose: () => {
                                        $("#edit_modal").modal("hide");
                                    }
                                });
                            },
                            error: function (errormessage) {
                                swal({
                                    type: "error",
                                    title: "Error",
                                    confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                    confirmButtonText: "Entendido",
                                    text: "Ocurrió un error al intentar eliminar el evento"
                                });
                            }
                        });
                    });
                }
            });
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
                $("#add_form [name='StartTime']").datepicker("update", "now");
                $("#add_form [name='EndTime']").datepicker("update", "now");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
                $("#edit_form [name='StartTime']").datepicker("update", "now");
                $("#edit_form [name='EndTime']").datepicker("update", "now");
            }
        },
        change: {
            editToDetailControls: function (editable) {
                $("#edit_modal").find("input, select").prop("disabled", true);
                if (editable !== false) {
                    $("#edit_modal").find(".btn-edit").show();
                    $("#edit_modal").find(".btn-delete").show();
                }
                else {
                    $("#edit_modal").find(".btn-edit").hide();
                    $("#edit_modal").find(".btn-delete").hide();
                }
                $("#edit_modal").find(".btn-cancel").hide();
                $("#edit_modal").find(".btn-save").hide();
            },
            editToDetail: function () {
                cancelBtn.start();
                $(".btn-save").prop("disabled", true);
                $("#edit_modal").find("input, select").prop("disabled", true);
                form.load.edit($("#edit_form [name='Id']").val(), function () {
                    form.change.editToDetailControls(true);
                    cancelBtn.stop();
                    $(".btn-save").prop("disabled", false);
                });
            },
            detailToEdit: function () {
                $("#edit_modal").find("input, select").prop("disabled", false);
                $("#edit_modal").find(".btn-edit").hide();
                $("#edit_modal").find(".btn-delete").hide();
                $("#edit_modal").find(".btn-cancel").show();
                $("#edit_modal").find(".btn-save").show();
            }
        }
    };

    var select2 = {
        init: function () {
            this.disciplines.init();
            this.seasons.enable.init();
            this.clubes.init();
        },
        seasons: {
            enable: {
                init: function () {
                    $.ajax({
                        url: _app.parseUrl("/api/temporadas/habiles")
                    }).done(function (result) {
                        let active = result.find(obj => obj.isActive);
                        let selectedId = active ? active.id : null;
                        let data = $.map(result,
                            function (obj) {
                                obj.id = obj.id || obj.id;
                                obj.text = obj.text || obj.year;
                                return obj;
                            });
                        $(".select2-seasons").select2({
                            data: data,
                            placeholder: "Temporada",
                            minimumResultsForSearch: -1
                        });
                        $(".select2-seasons").val(selectedId).trigger("change");
                    });
                }
            }
        },
        disciplines: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl("/api/disciplinas")
                }).done(function (result) {
                    let data = $.map(result,
                        function (obj) {
                            obj.id = obj.id || obj.id;
                            obj.text = obj.text || obj.name;
                            return obj;
                        });
                    $(".select2-disciplines")
                        .select2({
                            data: data,
                            placeholder: "Disciplina",
                            minimumResultsForSearch: -1
                        }).trigger("change");
                });
            }
        },
        clubes: {
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
                    $(".select2-clubes")
                        .select2({
                            data: data,
                            placeholder: "Club",
                            minimumResultsForSearch: -1
                        }).trigger("change");
                });
            }
        }
    };

    var datepicker = {
        init: function () {
            $("#add_form [name='StartTime']").datepicker({
                endDate: $("#add_form [name='EndTime']").val()
            }).on("changeDate", function (e) {
                //$(this).valid();
                $("#add_form [name='EndTime']").datepicker("setStartDate", e.date);
            });
            $("#add_form [name='EndTime']").datepicker({
                startDate: $("#add_form [name='StartTime']").val()
            }).on("changeDate", function (e) {
                $("#add_form [name='StartTime']").datepicker("setEndDate", e.date);
            });
            $("#edit_form [name='StartTime']").datepicker({
                endDate: $("#edit_form [name='EndTime']").val()
            }).on("changeDate", function (e) {
                $("#edit_form [name='EndTime']").datepicker("setStartDate", e.date);
            });
            $("#edit_form [name='EndTime']").datepicker({
                startDate: $("#edit_form [name='StartTime']").val()
            }).on("changeDate", function (e) {
                $("#edit_form [name='StartTime']").datepicker("setEndDate", e.date);
            });
            $(".datepicker").datepicker("setDate", "now");
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

    var ladda = {
        init: function () {
            addBtn = Ladda.create($("#add_form button[type='submit']")[0]);
            editBtn = Ladda.create($("#edit_form button[type='submit']")[0]);
            cancelBtn = Ladda.create($("#edit_form .btn-cancel")[0]);
        }
    };

    var events = {
        init: function () {
            $("#add_form [name='SeasonId']").attr("id", "add_seasonId");
            $("#edit_form [name='SeasonId']").attr("id", "edit_seasonId");
            $("#add_form [name='DisciplineId']").attr("id", "add_disciplineId");
            $("#edit_form [name='DisciplineId']").attr("id", "edit_disciplineId");
            $("#add_form [name='ClubId']").attr("id", "add_clubId");
            $("#edit_form [name='ClubId']").attr("id", "edit_clubId");
            $(".btn-edit").on("click", function () {
                form.change.detailToEdit();
            });
            $(".btn-cancel").on("click", function () {
                form.change.editToDetail();
            });
            $(".btn-delete").on("click", function () {
                form.delete();
            });
        }
    };


    return {
        init: function () {
            events.init();
            fullCalendar.init();
            ladda.init();
            modals.init();
            select2.init();
            datepicker.init();
            validate.init();
        }
    };
}();

$(function () {
    Event.init();
});
