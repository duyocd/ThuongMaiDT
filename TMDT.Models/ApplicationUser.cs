using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMDT.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        //nếu là khách hàng thì sẽ không có companyid còn nếu là người dùng của một công ty thì sẽ có id công ty
        public int? CompanyId { get; set; }
        //Thêm khóa ngoài cho người dùng
        [ForeignKey("CompanyId")] // ID công ty là thuộc tính điều hướng cho thuộc công ty (Company) này 
        [ValidateNever]
        public Company? Company { get; set; }
    }
}
