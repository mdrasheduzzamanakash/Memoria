using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memoria.Entities.DbSet
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        
        public byte[] Image { get; set; }
    }
}


/*
public string Id { get; set; }
public int Status { get; set; } = 1;
public DateTime AddedDateAndTime { get; set; } = DateTime.UtcNow;
public DateTime UpdatedDateAndTime { get; set; } = DateTime.UtcNow;
public string? UpdatedBy { get; set; }
public string? AddedBy { get; set; }
public string? FileFormat { get; set; }
public string FirstName { get; set; }

public string LastName { get; set; }

public string Email { get; set; }

public string Password { get; set; }

public byte[] Image { get; set; }
*/