using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace project4.model
{
	[Table("TOPIC")]
	public class Topic
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		public int UserId { get; set; }
		[StringLength(maximumLength: 100)]
		public string Name { get; set; }
		[StringLength(maximumLength: 500)]
		public string Description { get; set; }

		[StringLength(maximumLength: 500)]
		public string Avata { get; set; }
		[StringLength(maximumLength: 100)]
		public string CreatedBy { get; set; }
		[StringLength(maximumLength: 100)]
		public string UpdatedBy { get; set; }
		[StringLength(maximumLength: 100)]
		public string DeletedBy { get; set; }
		public DateTime? CreatedTime { get; set; }
		public DateTime? UpdatedTime { get; set; }
		public DateTime? DeletedTime { get; set; }
		public bool IsDeleted { get; set; }

		[ForeignKey("AccountId")]
		public int AccountId { get; set; }

		//public virtual Account? Account { get; set; }
		public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
	}
}
