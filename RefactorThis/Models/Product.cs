
using Newtonsoft.Json;
using refactor_this.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace refactor_me.Models
{
    public class Product
    {

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value")]
        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public bool IsNew { get; set; }

    }
}