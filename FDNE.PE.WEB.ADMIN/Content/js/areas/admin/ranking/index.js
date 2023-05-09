var Ranking = function () {

    var rankingDatatable = null;
    var binomialDatatable = null;
    var addForm = null;
    var editForm = null;
    var binomialForm = null;
    var binomialBtn = null;
    var addBtn = null;
    var editBtn = null;
    
    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/rankings"),
            dataSrc: "",
            data: function(parameter) {
                parameter.seasonId = $("#season_filter").val();
                parameter.disciplineId = $("#discipline_filter").val();
                parameter.categoryId = $("#category_filter").val();
                parameter.levelId = $("#level_filter").val();
            }
        },
        columns: [
            { title: "Nombre", data: "name" },
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
                    tmp += `<button data-id="${row.id}" class="btn btn-info btn-sm btn-edit">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-danger btn-sm btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button> `;
                    tmp += `<button data-id="${row.id}" class="btn btn-primary btn-sm btn-binomials">`;
                    tmp += `<i class="fa fa-users"></i> Binomios</button>`;
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
            { title: "Jinete", data: "rider.user.fullName" },
            { title: "Caballo", data: "horse.name" },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<button data-ranking="${row.rankingId}" data-rider="${row.riderId}" data-horse="${row.horseId}" class="btn btn-danger btn-sm btn-delete">`;
                    tmp += `<i class="fa fa-trash"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            rankingDatatable = $("#ranking_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            rankingDatatable.ajax.reload();
        },
        initEvents: function () {
            rankingDatatable.on("click",
                ".btn-edit",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    form.load.edit(id);
                });
            rankingDatatable.on("click",
                ".btn-binomials",
                function () {
                    let $btn = $(this);
                    var id = $btn.data("id");
                    $("#binomials_modal").one("shown.bs.modal",
                        function () {
                            datatable.binomials.init(id);
                        });
                    $("#RankingId").val(id);
                    $("#binomials_modal").modal("show");
                });
            rankingDatatable.on("click",
                ".btn-delete",
                function () {
                    let $btn = $(this);
                    let id = $btn.data("id");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El ranking será eliminado permanentemente",
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
                                    url: `/api/rankings/${id}`,
                                    type: "delete",
                                    success: function (result) {
                                        datatable.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El ranking ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el ranking"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
        },
        binomials: {
            init: function (rankingId) {
                options2.ajax.url = _app.parseUrl(`/api/rankings/${rankingId}/binomios`);
                binomialDatatable = $("#binomial_datatable").DataTable(options2);
                this.initEvents();
            },
            initEvents: function () {
                binomialDatatable.on("click", ".btn-delete", function () {
                    let $btn = $(this);
                    let rankingId = $btn.data("ranking");
                    let horseId = $btn.data("horse");
                    let riderId = $btn.data("rider");
                    Swal.fire({
                        title: "¿Está seguro?",
                        text: "El binomio será eliminado del ranking permanentemente",
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
                                    url: `/api/binomios/${rankingId}/${horseId}/${riderId}`,
                                    type: "delete",
                                    success: function (result) {
                                        datatable.binomials.reload();
                                        swal({
                                            type: "success",
                                            title: "Completado",
                                            text: "El binomio ha sido eliminado con éxito",
                                            confirmButtonText: "Excelente"
                                        });
                                    },
                                    error: function (errormessage) {
                                        swal({
                                            type: "error",
                                            title: "Error",
                                            confirmButtonClass: "btn btn-danger m-btn m-btn--custom",
                                            confirmButtonText: "Entendido",
                                            text: "Ocurrió un error al intentar eliminar el binomio"
                                        });
                                    }
                                });
                            });
                        }
                    });
                });
            },
            destroy: function () {
                binomialDatatable.destroy();
                $('#binomial_datatable').empty();
                binomialDatatable = null;
            },
            reload: function () {
                binomialDatatable.ajax.reload();
            }
        }
    };

    var form = {
        load: {
            edit: function (id) {
                _app.loader.show();
                $.ajax({
                        url: `/api/rankings/${id}`
                    }).done(function(result) {
                        let formElements = $("#edit_form");
                        formElements.find("[name='Id']").val(result.id);
                        formElements.find("[name='Name']").val(result.name);
                        formElements.find("[name='DisciplineId']").val(result.disciplineId).trigger("change");
                        select2.defaultValues.categoryId = result.categoryId;
                        select2.defaultValues.levelId = result.levelId;
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
                $(formElement).find("input, select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/api/rankings"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        addBtn.stop();
                        $(formElement).find("input, select").prop("disabled", false);
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
                $(formElement).find("input, select").prop("disabled", true);
                let id = $(formElement).find("input[name='Id']").val();
                $.ajax({
                    url: _app.parseUrl(`/api/rankings/${id}`),
                    method: "put",
                    data: data
                })
                    .always(function () {
                        editBtn.stop();
                        $(formElement).find("input, select").prop("disabled", false);
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
            },
            binomial: function (formElement) {
                let data = $(formElement).serialize();
                binomialBtn.start();
                $(formElement).find("select").prop("disabled", true);
                $.ajax({
                    url: _app.parseUrl("/api/binomios"),
                    method: "post",
                    data: data
                })
                    .always(function () {
                        binomialBtn.stop();
                        $(formElement).find("select").prop("disabled", false);
                    })
                    .done(function (result) {
                        datatable.binomials.reload();
                        form.reset.binomial();
                        _app.show.notification.add.success();
                    })
                    .fail(function (error) {
                        if (error.responseText) {
                            $("#binomial_alert_text").html(error.responseText);
                            $("#binomial_alert").removeClass("d-none").addClass("show");
                        }
                        _app.show.notification.add.error();
                    });
            }
        },
        reset: {
            add: function () {
                addForm.resetForm();
                $("#add_form").trigger("reset");
                $("#add_alert").removeClass("show").addClass("d-none");
            },
            edit: function () {
                editForm.resetForm();
                $("#edit_form").trigger("reset");
                $("#edit_alert").removeClass("show").addClass("d-none");
            },
            binomial: function () {
                let rankingId = $("#RankingId").val();
                binomialForm.resetForm();
                $("#binomial_alert").removeClass("show").addClass("d-none");
                $("#RankingId").val(rankingId);
            }
        }
    };
    
    var select2 = {
        init: function () {
            this.disciplines.init();
            this.seasons.init();
            this.seasons.enable.init();
            this.clubs.init();
        },
        defaultValues: {
            categoryId: null,
            levelId: null
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
            },
            init: function() {
                $.ajax({
                    url: _app.parseUrl("/api/temporadas")
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
                    $("#season_filter").val(selectedId).trigger("change");
                });
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
                    $(".select2-disciplines:not(#discipline_filter)")
                        .on("change",
                            function() {
                                let formId = $(this).closest("form").attr("id");
                                let categorySelector = `#${formId} .select2-categories`;
                                let levelSelector = `#${formId} .select2-levels`;
                                select2.categories.init(categorySelector, $(this).val());
                                select2.levels.init(levelSelector, $(this).val());
                            });
                    $("#discipline_filter").on("change",
                        function () {
                            let value = $(this).val();
                            value = value === "Todas" ? null : value;
                            select2.categories.init("#category_filter", value, true);
                            select2.levels.init("#level_filter", value, true);
                            datatable.reload();
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
        categories: {
            init: function (selector, filter, needGeneric) {
                selector = selector || ".select2-categories";
                $(selector).empty();
                if (!filter) {
                    let data = needGeneric ? [{ id: "Todas", text: "Todas" }] : [];
                    $(selector).select2({
                        data: data,
                        placeholder: "Categoría",
                        minimumResultsForSearch: -1
                    });
                } else {
                    $.ajax({
                        url: _app.parseUrl(`/api/categorias?disciplineId=${filter}`)
                    }).done(function(result) {
                        let data = $.map(result,
                            function(obj) {
                                obj.id = obj.id || obj.id;
                                obj.text = obj.text || obj.name;
                                return obj;
                            });
                        if (needGeneric) {
                            data.unshift({ id: "Todas", text: "Todas" });
                        }
                        $(selector).select2({
                            data: data,
                            placeholder: "Categoría",
                            dropdownParent: $(selector).closest(".modal-content, .card-body"),
                            allowClear: true
                        });
                        if (select2.defaultValues.categoryId) {
                            $(selector).val(select2.defaultValues.categoryId).trigger("change");
                            select2.defaultValues.categoryId = null;
                        }
                    });
                }
            }
        },
        levels: {
            init: function (selector, filter, needGeneric) {
                selector = selector || ".select2-levels";
                $(selector).empty();
                if (!filter) {
                    let data = needGeneric ? [{ id: "Todos", text: "Todos" }] : [];
                    $(selector).select2({
                        data: data,
                        placeholder: "Nivel",
                        minimumResultsForSearch: -1
                    });
                } else {
                    $.ajax({
                        url: _app.parseUrl(`/api/niveles?disciplineId=${filter}`)
                    }).done(function(result) {
                        let data = $.map(result,
                            function(obj) {
                                obj.id = obj.id || obj.id;
                                obj.text = obj.text || obj.name;
                                return obj;
                            });
                        if (needGeneric) {
                            data.unshift({ id: "Todos", text: "Todos" });
                        }
                        $(selector).select2({
                            data: data,
                            placeholder: "Nivel",
                            dropdownParent: $(selector).closest(".modal-content, .card-body"),
                            allowClear: true
                        });
                        if (select2.defaultValues.levelId) {
                            $(selector).val(select2.defaultValues.levelId).trigger("change");
                            select2.defaultValues.levelId = null;
                        }
                    });
                }
            }
        },
        riders: {
            init: function (clubId) {
                $(".select2-riders").empty();
                $.ajax({
                    url: _app.parseUrl(`/api/jinetes?clubId=${clubId}`)
                }).done(function (result) {
                    let data = $.map(result,
                        function (obj) {
                            obj.id = obj.id || obj.id;
                            obj.text = obj.text || obj.user.fullName;
                            return obj;
                        });
                    $(".select2-riders").select2({
                        data: data,
                        placeholder: "Jinete",
                        dropdownParent: $("#binomials_modal .modal-body")
                    });
                });
            }
        },
        horses: {
            init: function (clubId) {
                $(".select2-horses").empty();
                $.ajax({
                    url: _app.parseUrl(`/api/caballos?clubId=${clubId}`)
                }).done(function (result) {
                    let data = $.map(result,
                        function (obj) {
                            obj.id = obj.id || obj.id;
                            obj.text = obj.text || obj.name;
                            return obj;
                        });
                    $(".select2-horses").select2({
                        data: data,
                        placeholder: "Caballo",
                        dropdownParent: $("#binomials_modal .modal-body")
                    });
                });
            }
        },
        clubs: {
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
                    $(".select2-clubs").on("change", function () {
                        select2.horses.init($(this).val());
                        select2.riders.init($(this).val());
                    });
                    $(".select2-clubs").select2({
                        data: data,
                        placeholder: "Club",
                        dropdownParent: $("#binomials_modal .modal-body")
                    }).trigger("change");
                });
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

            binomialForm = $("#binomial_form").validate({
                submitHandler: function (formElement, e) {
                    e.preventDefault();
                    form.submit.binomial(formElement);
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
            $("#binomials_modal").on("hidden.bs.modal",
                function () {
                    form.reset.binomial();
                    datatable.binomials.destroy();
                });
        }
    };

    var ladda = {
        init: function () {
            addBtn = Ladda.create($("#add_form button[type='submit']")[0]);
            editBtn = Ladda.create($("#edit_form button[type='submit']")[0]);
            binomialBtn = Ladda.create($("#binomial_form button[type='submit']")[0]);
        }
    };

    var events = {
        init: function() {
            $("#category_filter, #level_filter, #season_filter").on("change",
                function() {
                    datatable.reload();
                });
            $("#add_form [name='DisciplineId']").attr("id", "add_disciplineId");
            $("#edit_form [name='DisciplineId']").attr("id", "edit_disciplineId");
            $("#add_form [name='CategoryId']").attr("id", "add_categoryId");
            $("#edit_form [name='CategoryId']").attr("id", "edit_categoryId");
            $("#add_form [name='LevelId']").attr("id", "add_levelId");
            $("#edit_form [name='LevelId']").attr("id", "edit_levelId");
        }
    };

    return {
        init: function () {
            events.init();
            datatable.init();
            validate.init();
            select2.init();
            modals.init();
            ladda.init();
        }
    };
}();

$(function () {
    Ranking.init();
});