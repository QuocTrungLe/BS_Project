namespace BS_Project.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        [Key]
        public int UserID { get; set; }

        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }
    }
}
