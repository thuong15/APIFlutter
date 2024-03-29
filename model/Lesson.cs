using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace project4.model
{
	[Table("LESSON")]
	public class Lesson
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }

		[StringLength(maximumLength: 100)]
		public String Code { get; set; }

		[StringLength(maximumLength: 100)]
		public String TopicCode { get; set; }

		[StringLength(maximumLength: 100)]
		public string Name { get; set; }
		[StringLength(maximumLength: 500)]
		public string Description { get; set; }
		[StringLength(maximumLength: 100)]
		public string Avatar { get; set; }

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
	}
}
