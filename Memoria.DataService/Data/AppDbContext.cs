using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memoria.Entities.DbSet;

namespace Memoria.DataService.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Authorization> Authorization { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Note> Notes { get; set; }  
        public virtual DbSet<NoteLabel> NoteLabels { get; set; }
        public virtual DbSet<Trash> Trashs { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
    }
}
