using System;
using System.Collections.Generic;

namespace eCommerce_API.Models
{
    public partial class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Postal1 { get; set; }
        public string Postal2 { get; set; }
        public string Postal3 { get; set; }
        public string Email { get; set; }
        public string BranchHeader { get; set; }
        public string BranchFooter { get; set; }
        public bool? Activated { get; set; }
        public string BranchPosRecieptHeader { get; set; }
        public string BranchPosRecieptFooter { get; set; }
        public string TaxNum { get; set; }
        public bool SyncStock { get; set; }
        public double Seq { get; set; }
        public string DepositAccountNumber { get; set; }
    }
}
