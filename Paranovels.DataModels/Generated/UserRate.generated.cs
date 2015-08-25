// This class was automatically generated with love by ST4bby 8/25/2015 4:37:48 PM.
// Read more at http://jbubriski.github.com/ST4bby/

namespace Paranovels.DataModels
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Thi.Core;
	
	[Table("UserRate")]
	public partial class UserRate : IDataModel
	{
		[Key, Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("UserRateID", TypeName = "int"), Display(Name="User Rate ID")]
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
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("UserID", TypeName = "int"), Display(Name="User ID")]
		public int UserID { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("SourceID", TypeName = "int"), Display(Name="Source ID")]
		public int SourceID { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("SourceTable", TypeName = "int"), Display(Name="Source Table")]
		public int SourceTable { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Type", TypeName = "int"), Display(Name="Type")]
		public int Type { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Rate", TypeName = "int"), Display(Name="Rate")]
		public int Rate { get; set; }
	}
}
