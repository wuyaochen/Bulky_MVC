
$(document).ready(function () {
    loadDataTable();
});

document.addEventListener('DOMContentLoaded', function () {
    new DataTable('#tblData', {
        ajax: {
            url: '/admin/product/getall',
            dataSrc: 'data'
        },
        columns: [
            { data: 'title', width: '20%' },
            { data: 'isbn', width: '15%' },
            { data: 'listPrice', width: '10%' },
            { data: 'author', width: '20%' },
            { data: 'category.name', width: '15%' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                                    <i class="bi bi-pencil-square"></i> Edit
                                </a>
                                <a href="/admin/product/delete?id=${data}" class="btn btn-danger mx-2">
                                    <i class="bi bi-trash-fill"></i> Delete
                                </a>
                            </div>`;
                },
                width: '20%'
            }
        ]
    });
});