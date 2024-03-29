using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace project4.model
{
	[Table("Account")]
	public class Account
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int? ID { get; set; }
		[StringLength(maximumLength: 100)]
		public string Code { get; set; }
		[StringLength(maximumLength: 100)]
		public string UserName { get; set; }
		[StringLength(maximumLength: 100)]
		public string Password { get; set; }
		[StringLength(maximumLength: 100)]
		public string? Name { get; set; }
		[StringLength(maximumLength: 500)]
		public string? Avatar { get; set; }
		[StringLength(maximumLength: 100)]
		public string? CreatedBy { get; set; }
		[StringLength(maximumLength: 100)]
		public string? UpdatedBy { get; set; }
		[StringLength(maximumLength: 100)]
		public string? DeletedBy { get; set; }
		public DateTime? CreatedTime { get; set; }
		public DateTime? UpdatedTime { get; set; }
		public DateTime? DeletedTime { get; set; }
		public bool IsDeleted { get; set; }

	}
}
