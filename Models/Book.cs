using System.ComponentModel.DataAnnotations;

namespace Online_Bookstore.Models
{
	public class Book
	{
		 
        [Key]
		public int Id { get; set; }
		public string Title { get; set; }
		public string Genre { get; set; }
		public int AuthorId { get; set; }
		public Author Author { get; set; }
	}
}
