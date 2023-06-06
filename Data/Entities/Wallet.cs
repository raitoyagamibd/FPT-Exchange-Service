using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class Wallet
{
    public Guid Id { get; set; }

    public int? Score { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
