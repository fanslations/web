// This class was automatically generated with love by ST4bby 8/9/2015 3:28:51 PM.
// Read more at http://jbubriski.github.com/ST4bby/

namespace Paranovels.DataModels
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Thi.Core;
	
	[Table("Chapter")]
	public partial class Chapter : IDataModel
	{
		[NotMapped]public int ID { get { return ChapterID; } set { ChapterID = value; } }
	
		[Key, Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("ChapterID", TypeName = "int"), Display(Name="Chapter ID")]
		public int ChapterID { get; set; }
		
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
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("NovelID", TypeName = "int"), Display(Name="Novel ID")]
		public int NovelID { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Status", TypeName = "int"), Display(Name="Status")]
		public int Status { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Number", TypeName = "int"), Display(Name="Number")]
		public int Number { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Title", TypeName = "nvarchar"), Display(Name="Title"), StringLength(400)]
		public string Title { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Summary", TypeName = "nvarchar"), Display(Name="Summary"), StringLength(2000)]
		public string Summary { get; set; }
	}
}
