using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace project4.model
{
	[Table("QUESTION")]
	public class Question
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		public int LessonId { get; set; }

		[StringLength(maximumLength: 100)]
		public string Name { get; set; }
		[StringLength(maximumLength: 500)]
		public string Description { get; set; }
		public string CreatedBy { get; set; }
		[StringLength(maximumLength: 100)]
		public string UpdatedBy { get; set; }
		[StringLength(maximumLength: 100)]
		public string DeletedBy { get; set; }
		public DateTime? CreatedTime { get; set; }
		public DateTime? UpdatedTime { get; set; }
		public DateTime? DeletedTime { get; set; }
		public bool IsDeleted { get; set; }
		public virtual Lesson? Lessons { get; set; }
		public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

	}
}
