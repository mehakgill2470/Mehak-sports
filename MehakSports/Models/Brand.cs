using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MehakSports.Models
{
    public class Brand
    {
        public Brand()
        {
            BrandName = "";
            DateEstablished = DateTime.Now;
        }
        [Key]
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public string Website { get; set; }
        public DateTime DateEstablished { get; set; }
        public List<Product> ProductList { get; set; }
    }
}
