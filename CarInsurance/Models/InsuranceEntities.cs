using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace CarInsurance.Models
{
    public class InsuranceEntities : DbContext
    {
        // Your DbSet properties for the tables you want to include
        public DbSet<Insuree> Insurees { get; set; }

        // Constructor to specify the connection string from Web.config
        public InsuranceEntities() : base("name=InsuranceEntities") { }
    }
}
