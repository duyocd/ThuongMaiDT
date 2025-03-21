var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }

});


function loadDataTable(status) {
    dataTable = $('#tblData1').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { data: 'id', "width": "10%", "title": "Mã đơn" },
            { data: 'name', "width": "20%", "title": "Tên khách hàng" },
            { data: 'phoneNumber', "width": "17%", "title": "Số điện thoại" },
            { data: 'applicationUser.email', "width": "15%", "title": "Email" },
            { data: 'orderStatus', "width": "18%", "title": "Trạng thái đơn hàng" },
            { data: 'orderTotal', "width": "10%", "title": "Tổng tiền" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/customer/order/details?orderId=${data}" class="btn btn-primary mx-2"> 
                    <i class="bi bi-eye-fill"></i> </a>               
                    </div>`
                },
                "width": "10%",
                "title": "Hành động"
            }
        ],
        "language": {
            "lengthMenu": "Hiển thị _MENU_ mục",
            "zeroRecords": "Không tìm thấy dữ liệu",
            "info": "Hiển thị _START_ đến _END_ của _TOTAL_ mục",
            "infoEmpty": "Không có dữ liệu",
            "infoFiltered": "(lọc từ _MAX_ tổng số mục)",
            "search": "Tìm kiếm:",
            "paginate": {
                "first": "Đầu",
                "last": "Cuối",
                "next": "Tiếp",
                "previous": "Trước"
            }
        }
    });
}