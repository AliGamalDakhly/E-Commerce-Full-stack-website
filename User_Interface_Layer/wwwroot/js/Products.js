var dTable;

$(document).ready(function () {
    LoadData();
});

function LoadData() {
    dTable = $("#ProductsTable").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetDataTable"
        },
        "columns": [
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category.name" },
            { "data": "createddate" }
        ]
    })
}