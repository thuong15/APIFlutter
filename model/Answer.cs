using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace project4.model
{
	[Table("ANSWER")]
	public class Answer
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		[StringLength(maximumLength: 100)]
		public String Code { get; set; }

		[StringLength(maximumLength: 100)]
		public String QuestionCode { get; set; }

		[StringLength(maximumLength: 100)]
		public string Name { get; set; }
		public bool IsTrue { get; set; }
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
