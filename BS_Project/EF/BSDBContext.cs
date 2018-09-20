namespace BS_Project.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BSDBContext : DbContext
    {
        public BSDBContext()
            : base("name=BSDBContext")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ImageBool> ImageBools { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<OrdersBook> OrdersBooks { get; set; }
        public virtual DbSet<OrdersDetail> OrdersDetails { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(e => e.Books)
                .WithMany(e => e.Authors)
                .Map(m => m.ToTable("AuthorsBooks").MapLeftKey("AuthorID").MapRightKey("BookID"));

            modelBuilder.Entity<Book>()
                .HasMany(e => e.OrdersDetails)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Categories)
                .WithMany(e => e.Books)
                .Map(m => m.ToTable("CategoriesBooks").MapLeftKey("BookID").MapRightKey("CategoryID"));

            modelBuilder.Entity<ImageBool>()
                .HasMany(e => e.Images)
                .WithRequired(e => e.ImageBool)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrdersBook>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<OrdersBook>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<OrdersBook>()
                .HasMany(e => e.OrdersDetails)
                .WithRequired(e => e.OrdersBook)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.PayName)
                .IsFixedLength();

            modelBuilder.Entity<UserRole>()
                .Property(e => e.Title)
                .IsFixedLength();

            modelBuilder.Entity<UserRole>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserRole)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.IDNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OrdersBooks)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
