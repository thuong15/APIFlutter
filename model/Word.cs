using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace project4.model
{
	[Table("WORD")]
	public class Word
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		[StringLength(maximumLength: 100)]
		public string Code { get; set; }

		[StringLength(maximumLength: 100)]
		public string LessonCode { get; set; }

		[StringLength(maximumLength: 255)]
		public string NameVN { get; set; }
		[StringLength(maximumLength: 255)]
		public string NameEN { get; set; }

		[StringLength(maximumLength: 255)]
		public string Avatar { get; set; }
		public int Count { get; set; }
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
