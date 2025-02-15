using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CustomersOrders
{
    [Key]
    public int Id { get; set; }     

    [Required]
    public string CustomerId { get; set; }

    [Required]
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    [ForeignKey("CustomerId")]
    public virtual ApplicationUser Customer { get; set; }

    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; }
}
