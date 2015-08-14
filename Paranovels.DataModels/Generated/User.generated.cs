// This class was automatically generated with love by ST4bby 8/14/2015 4:20:13 PM.
// Read more at http://jbubriski.github.com/ST4bby/

namespace Paranovels.DataModels
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Thi.Core;
	
	[Table("User")]
	public partial class User : IDataModel
	{
		[Key, Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("UserID", TypeName = "int"), Display(Name="User ID")]
		public int ID { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("InsertedBy", TypeName = "int"), Display(Name="Inserted By")]
		public int InsertedBy { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("InsertedDate", TypeName = "smalldatetime"), Display(Name="Inserted Date")]
		public DateTime InsertedDate { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("UpdatedBy", TypeName = "int"), Display(Name="Updated By")]
		public int UpdatedBy { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("UpdatedDate", TypeName = "smalldatetime"), Display(Name="Updated Date")]
		public DateTime UpdatedDate { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsDeleted", TypeName = "bit"), Display(Name="Is Deleted")]
		public bool IsDeleted { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Username", TypeName = "nvarchar"), Display(Name="Username"), StringLength(512)]
		public string Username { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Email", TypeName = "nvarchar"), Display(Name="Email"), StringLength(512)]
		public string Email { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("FirstName", TypeName = "nvarchar"), Display(Name="First Name"), StringLength(100)]
		public string FirstName { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("LastName", TypeName = "nvarchar"), Display(Name="Last Name"), StringLength(100)]
		public string LastName { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Location", TypeName = "varchar"), Display(Name="Location"), StringLength(16)]
		public string Location { get; set; }
	}
}
