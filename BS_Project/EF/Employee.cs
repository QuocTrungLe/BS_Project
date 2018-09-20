namespace BS_Project.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        [Key]
        public int EmployeID { get; set; }

        [Required]
        [StringLength(100)]
        public string EmployeName { get; set; }

        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(40)]
        public string Certificate { get; set; }

        public string Note { get; set; }

        public int AreaID { get; set; }
    }
}
