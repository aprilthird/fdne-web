var Result = function () {

    var eventDatatable = null;
    var rankingDatatable = null;
    var eventId = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/eventos"),
            dataSrc: "",
            data: function (parameter) {
                parameter.disciplineId = $("#discipline_filter").val();
                parameter.clubId = $("#club_filter").val();
                parameter.startTime = $("#start_time_filter").val();
                parameter.endTime = $("#end_time_filter").val();
                parameter.past = true;
            }
        },
        columns: [
            { title: "Nombre", data: "name" },
            { title: "Acrónimo", data: "season.name" },
            { title: "Temporada", data: "season.name" },
            { title: "Disciplina", data: "discipline.name" },
            { title: "Club", data: "club.name" },
            { title: "Inicio", data: "startTime" },
            { title: "Fin", data: "endTime" },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" data-season="${row.seasonId}" data-discipline="${row.disciplineId}" class="btn btn-primary btn-sm btn-rankings">`;
                    tmp += `<i class="fa fa-bar-chart"></i> Rankings</button>`;
                    return tmp;
                }
            }
        ]
    };

    var options2 = {
        responsive: true,
        ajax: {
            url: "",
            dataSrc: ""
        },
        columns: [
            { title: "Temporada", data: "season.name" },
            { title: "Disciplina", data: "discipline.name" },
            {
                title: "Categoría",
                data: "category.name",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Nivel",
                data: "level.name",
                render: function (data) {
                    return data || "---";
                }
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-id="${row.id}" class="btn btn-success btn-sm btn-score">`;
                    tmp += `<i class="fa fa-trophy"></i> Calificar</button> `;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            eventDatatable = $("#event_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            eventDatatable.ajax.reload();
        },
        initEvents: function () {
            eventDatatable.on("click",
                ".btn-rankings",
                function () {
                    let $btn = $(this);
                    var id = $btn.data("id");
                    let seasonId = $btn.data("season");
                    let disciplineId = $btn.data("discipline");
                    eventId = id;
                    $("#rankings_modal").one("shown.bs.modal",
                        function () {
                            datatable.rankings.init(seasonId, disciplineId);
                        });
                    $("#rankings_modal").modal("show");
                });
        },
        rankings: {
            init: function (seasonId, disciplineId) {
                options2.ajax.url = _app.parseUrl(`/api/rankings?seasonId=${seasonId}&disciplineId=${disciplineId}`);
                rankingDatatable = $("#ranking_datatable").DataTable(options2);
                this.initEvents();
            },
            initEvents: function () {
                rankingDatatable.on("click",
                    ".btn-score",
                    function () {
                        let $btn = $(this);
                        var rankingId = $btn.data("id");
                        location.href = `/gestion/resultados/evento/${eventId}/ranking/${rankingId}/calificar`;
                    });
            },
            destroy: function () {
                rankingDatatable.destroy();
                $('#ranking_datatable').empty();
                rankingDatatable = null;
            },
            reload: function () {
                datatable.rankings.reload();
            }
        }
    };
    
    var select2 = {
        init: function () {
            this.disciplines.init();
            this.clubes.init();
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
                        })
                        .trigger("change");
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
            $("#start_time_filter").datepicker({
                endDate: $("#end_time_filter").val()
            }).on("changeDate", function (e) {
                $("#end_time_filter").datepicker("setStartDate", e.date);
            });
            $("#end_time_filter").datepicker({
                startDate: $("#start_time_filter").val()
            }).on("changeDate", function (e) {
                $("#start_time_filter").datepicker("setEndDate", e.date);
            });
        }
    };


    var modals = {
        init: function () {
            $("#rankings_modal").on("hidden.bs.modal",
                function () {
                    datatable.rankings.destroy();
                    eventId = null;
                });
        }
    };

    var events = {
        init: function () {
            $("#discipline_filter, #club_filter, #start_time_filter, #end_time_filter").on("change",
                function () {
                    if (eventDatatable === null) {
                        datatable.init();
                    }
                    else {
                        datatable.reload();
                    }
                });
        }
    };

    return {
        init: function () {
            events.init();
            select2.init();
            modals.init();
            datepicker.init();
        }
    };
}();

$(function () {
    Result.init();
});