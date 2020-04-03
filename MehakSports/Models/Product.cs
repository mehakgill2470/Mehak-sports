using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MehakSports.Models
{
    public class Product
    {

        public Product()
        {
            ProductName = "";
            AvailableDate = DateTime.Now;
        }

        [ForeignKey("Brand")]
        public int BrandID { get; set; }
        public virtual Brand Brand { get; set; }

        public int MSRP { get; set; }
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public DateTime AvailableDate { get; set; }

    }
}
