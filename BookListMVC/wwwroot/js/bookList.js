var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_books').DataTable({
        "ajax": {
            "url": "/books/getall",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "isbn", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Books/Upsert?id=${data}" class='btn btn-success btn-sm text-white' style='cursor:pointer; width:70px;'>
                                    Edit
                                </a>
                                <a class='btn btn-danger btn-sm text-white' style='cursor: pointer; width:70px;'
                                    onclick = Delete('/books/Delete?id='+${data})>
                                    Delete
                                </a>
                            </div>`
                }, "width": "20%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Delete(urlDelete) {

    swal({
        title: "Do you want to delete this Book?",
        text: "Once deleted this Book, you won't be able to recover it",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((DeleteYes) => {
        if (DeleteYes) {
            $.ajax({
                type: "DELETE",
                url: urlDelete,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.messageResult);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.messageResult);
                    }
                }
            });
        }
    });

}