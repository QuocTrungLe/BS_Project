namespace BS_Project.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Payment")]
    public partial class Payment
    {
        [Key]
        public int PayedID { get; set; }

        [StringLength(10)]
        public string PayName { get; set; }

        public string Note { get; set; }

        public int? OrderID { get; set; }
    }
}
