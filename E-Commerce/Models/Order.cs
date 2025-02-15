using E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public decimal TotoalPrice { get; set; }

    public DateTime? ShippedDate { get; set; }
    public DateTime? ArriveDate { get; set; }

    public virtual ICollection<ApplicationUser> Customers { get; set; } = new HashSet<ApplicationUser>();
}
