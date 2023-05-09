var Scores = function () {
    var eventId = location.pathname.match(/evento\/(.*?)\/ranking/)[1];
    var rankingId = location.pathname.match(/ranking\/(.*?)\/calificar/)[1];
    var isDouble = $("#IsDouble").val() !== "False";
    var resultDatatable = null;
    var resultForm = null;
    var resultBtn = null;

    var options = {
        responsive: true,
        paging: false,
        ajax: {
            url: "",
            dataSrc: ""
        },
        columns: [
            { title: "Jinete", data: "rider.user.fullName" },
            { title: "Caballo", data: "horse.name" },
            { title: "Club", data: "club.name" },
            {
                title: "Puntaje",
                data: null,
                orderable: false,
                render: function (data, type, row, meta) {
                    var tmp = "";
                    tmp += `<input id="score_${meta.row}" type="number" name="[${meta.row}].Score" class="form-control score-input" required min="0" value="${row.score}"/>`;
                    tmp += `<input hidden value="${row.riderId}" name="[${meta.row}].RiderId" />`;
                    tmp += `<input hidden value="${row.horseId}" name="[${meta.row}].HorseId" />`;
                    tmp += `<input hidden value="${row.clubId}" name="[${meta.row}].ClubId" />`;
                    return tmp;
                }
            }
        ]
    };

    var optionsTraining = {
        responsive: true,
        searching: false,
        paging: false,
        ajax: {
            url: "",
            dataSrc: ""
        },
        columns: [
            { title: "Jinete", data: "rider.user.fullName" },
            { title: "Caballo", data: "horse.name" },
            { title: "Club", data: "club.name" },
            {
                title: "Porcentaje",
                data: null,
                orderable: false,
                render: function (data, type, row, meta) {
                    var tmp = "";
                    tmp += `<input id="percent_${meta.row}" type="number" name="[${meta.row}].Percent" class="form-control percent-input"  required min="0" max="100" value="${row.percent}"/>`;
                    return tmp;
                }
            },
            {
                title: "Puntaje",
                data: null,
                orderable: false,
                render: function (data, type, row, meta) {
                    var tmp = "";
                    tmp += `<input id="score_${meta.row}" type="number" name="[${meta.row}].Score" class="form-control score-input" required readonly min="0" value="${row.score}"/>`;
                    tmp += `<input hidden value="${row.riderId}" name="[${meta.row}].RiderId" />`;
                    tmp += `<input hidden value="${row.horseId}" name="[${meta.row}].HorseId" />`;
                    tmp += `<input hidden value="${row.clubId}" name="[${meta.row}].ClubId" />`;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function (day) {
            if (resultDatatable !== null) {
                this.destroy();
            }
            options.ajax.url = _app.parseUrl(`/api/eventos/${eventId}/ranking/${rankingId}/dia/${day}/resultados`);
            optionsTraining.ajax.url = _app.parseUrl(`/api/eventos/${eventId}/ranking/${rankingId}/dia/${day}/resultados`);
            resultDatatable = $("#result_datatable").DataTable(isDouble ? optionsTraining : options);
            this.initEvents();
        },
        destroy: function () {
            resultDatatable.destroy();
            $("#result_datatable").empty();
            resultDatatable = null;
        },
        reload: function () {
            resultDatatable.ajax.reload();
        },
        initEvents: function () {
            resultDatatable.on("input",
                ".percent-input",
                function () {
                    let id = $(this).attr("id");
                    let number = id.replace("percent_", "");
                    let value = 0;
                    if (!isNaN(parseFloat($(this).val()))) {
                        value = parseFloat($(this).val());
                        if (value < 55) {
                            value = 0;
                        }
                        else {
                            value /= 2;
                        }
                    }
                    $(`#score_${number}`).val(value);
                });
        }
    };
    
    var select2 = {
        init: function () {
            this.days.init();
        },
        days: {
            init: function () {
                $.ajax({
                    url: _app.parseUrl(`/api/eventos/${eventId}/dias`)
                }).done(function (result) {
                    let data = $.map(result,
                        function (obj) {
                            obj.id = obj.id || obj.dayNumber;
                            obj.text = obj.text || obj.date;
                            return obj;
                        });
                    $(".select2-days")
                        .on("change", function () {
                            datatable.init($(this).val());
                        })
                        .select2({
                            data: data,
                            placeholder: "Fecha",
                            minimumResultsForSearch: -1
                        })
                        .trigger("change");
                });
            }
        }
    };

    var form = {
        submit: function (formElement) {
            let data = $(formElement).serialize();
            resultBtn.start();
            $(formElement).find("input").prop("disabled", true);
            $.ajax({
                url: _app.parseUrl(`/api/evento/${eventId}/ranking/${rankingId}/resultados/dia/${$("#day_filter").val()}/calificar`),
                method: "post",
                data: data
            })
                .always(function () {
                    resultBtn.stop();
                    $(formElement).find("input").prop("disabled", false);
                })
                .done(function (result) {
                    datatable.reload();
                    _app.show.notification.add.success();
                })
                .fail(function (error) {
                    if (error.responseText) {
                        $("#alert_text").html(error.responseText);
                        $("#alert").removeClass("d-none").addClass("show");
                    }
                    _app.show.notification.add.error();
                });
        }
    };

    var validate = {
        init: function () {
            resultForm = $("#result_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit(formElement);
                }
            });
        }
    };


    var ladda = {
        init: function () {
            resultBtn = Ladda.create($("#result_form button[type='submit']")[0]);
        }
    };

    return {
        init: function () {
            select2.init();
            ladda.init();
            validate.init();
        }
    };
}();

$(function () {
    Scores.init();
});