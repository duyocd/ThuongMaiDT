using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDT.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Company = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        //trạng thái xử lý đơn hàng
        public const string StatusPending = "Chờ xử lý";
        public const string StatusApproved = "Đã duyệt";
        public const string StatusInProcess = "Đang xử lý";
        public const string StatusShipped = "Đã giao hàng";
        public const string StatusCancelled = "Đã hủy";
        public const string StatusRefunded = "Đã hoàn tiền";

        public const string PaymentStatusPending = "Chờ thanh toán";
        public const string PaymentStatusApproved = "Đã thanh toán";
        public const string PaymentStatusDelayedPayment = "Được duyệt trả chậm";
        public const string PaymentStatusRejected = "Bị từ chối";

        //session số lương sản phẩm cho giỏ 
        public const string SessionCart = "SesionShoppingCart";

	}
}
