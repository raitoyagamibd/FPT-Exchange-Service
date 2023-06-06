using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Station
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<ProductActivy> ProductActivies { get; set; } = new List<ProductActivy>();

    public virtual ICollection<ProductTransfer> ProductTransferStationIdformNavigations { get; set; } = new List<ProductTransfer>();

    public virtual ICollection<ProductTransfer> ProductTransferStationIdtoNavigations { get; set; } = new List<ProductTransfer>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
