using Microsoft.EntityFrameworkCore;
using Online_Bookstore.Models;

namespace Online_Bookstore.Data
{
	public class ApplicationDbContext : DbContext
	{
	
			public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

			public DbSet<Book> Books { get; set; }
			public DbSet<Author> Authors { get; set; }
	}
}
