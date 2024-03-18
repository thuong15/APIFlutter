using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace project4.model
{
	public class project4Context: DbContext
	{
		public project4Context(DbContextOptions<project4Context> options) : base(options)
		{

		}
		public DbSet<project4.model.Account>? Account { get; set; }
		public DbSet<project4.model.Answer>? Answer { get; set; }
		public DbSet<project4.model.Categories>? Categories { get; set; }
		public DbSet<project4.model.Example>? Example { get; set; }
		public DbSet<project4.model.History>? History { get; set; }
		public DbSet<project4.model.Lesson>? Lesson { get; set; }
		public DbSet<project4.model.Question>? Question { get; set; }
		public DbSet<project4.model.Topic>? Topic { get; set; }
		public DbSet<project4.model.Word>? Word { get; set; }
		//public DbSet<Accountsss> Accountsss { get; set; }
	}
}
