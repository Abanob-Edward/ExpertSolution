using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Model
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public string locale { get; set; }
        public string bill_to_address_line1 { get; set; }
        public string bill_to_address_city { get; set; }
        public string bill_to_address_country { get; set; }
        public string bill_to_email { get; set; }
        public string ccEmail { get; set; }
        public string customer_lastname { get; set; }
        public string bill_to_forename { get; set; }
        public string bill_to_surname { get; set; }
        public float amount { get; set; }
        public string currency { get; set; }

        public string Notes { get; set; }
        public string ActivityName { get; set; } = "";
        public string ActivityCode { get; set; } = "";
        public string ActivitySource { get; set; } = "ExpertManagementSystem";

    }
}
