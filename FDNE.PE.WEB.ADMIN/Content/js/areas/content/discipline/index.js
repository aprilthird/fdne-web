var disciplines = function () {

    var disciplineDatatable = null;

    var options = {
        responsive: true,
        ajax: {
            url: _app.parseUrl("/api/disciplinas"),
            dataSrc: ""
        },
        columns: [
            {
                title: "Nombre", data: "name"
            },
            {
                title: "Opciones",
                data: null,
                orderable: false,
                render: function (data, type, row) {
                    var tmp = "";
                    tmp += `<a data-id="${row.id}" class="btn btn-info btn-sm btn-edit" href="disciplinas/editar/${row.id}">`;
                    tmp += `<i class="fa fa-edit"></i></button> `;
                    return tmp;
                }
            }
        ]
    };

    var datatable = {
        init: function () {
            disciplineDatatable = $("#discipline_datatable").DataTable(options);
            this.initEvents();
        },
        reload: function () {
            disciplineDatatable.ajax.reload();
        },
        initEvents: function () {
            disciplineDatatable.on("click",
                ".btn-edit",
                function (id) {
                    window.location.href = '/contenido/Discipline/' + id
                });
        }
    };

    return {
        init: function () {
            datatable.init();
        }
    };
}();

$(function () {
    disciplines.init();
});