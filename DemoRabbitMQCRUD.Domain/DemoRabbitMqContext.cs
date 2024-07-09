using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoRabbitMqCRUD.Domain;

public partial class DemoRabbitMqContext : DbContext
{
    public DemoRabbitMqContext()
    {
    }

    public DemoRabbitMqContext(DbContextOptions<DemoRabbitMqContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-9A9P2Q7\\SQLEXPRESS;Database=DemoRabbitMq;Trusted_Connection=True;TrustServerCertificate=True");

    public DbSet<Product> products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
