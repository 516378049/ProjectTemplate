namespace WebApplication2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        //public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<foods> foods { get; set; }
        public virtual DbSet<goods> goods { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<ratings> ratings { get; set; }
        public virtual DbSet<sellers> sellers { get; set; }
        public virtual DbSet<sellerSet> sellerSet { get; set; }
        public virtual DbSet<supports> supports { get; set; }
        public virtual DbSet<EFTableTest> EFTableTest { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EFTableTest>()
                .Property(e => e.name)
                .IsUnicode(false);
        }
    }
}
