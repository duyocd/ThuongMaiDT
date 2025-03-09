var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group">
                            <button onClick="viewCompany(${data})" class="btn btn-info mx-1"> 
                                <i class="bi bi-eye-fill"></i> View
                            </button>
                            <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-1"> 
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>               
                            <a onClick=Delete('/admin/company/delete/${data}') class="btn btn-danger mx-1"> 
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>`
                },
                "width": "30%"
            }
        ]
    });
}

// Hàm AJAX lấy dữ liệu chi tiết công ty
function viewCompany(id) {
    $("#companyDetailModal .modal-body").html('<div class="text-center py-3"><div class="spinner-border text-primary" role="status"></div></div>');
    $("#companyDetailModal").modal("show");

    $.ajax({
        url: `/admin/company/details/${id}`,
        type: "GET",
        success: function (data) {
            $("#companyDetailModal .modal-body").html(data);
        },
        error: function () {
            $("#companyDetailModal .modal-body").html('<p class="text-danger text-center">Failed to load company details.</p>');
        }
    });
}



function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}

