// This class was automatically generated with love by ST4bby 9/1/2015 4:59:57 PM.
// Read more at http://jbubriski.github.com/ST4bby/

namespace Paranovels.DataModels
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Thi.Core;
	
	[Table("Novel")]
	public partial class Novel : IDataModel
	{
		[Key, Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("NovelID", TypeName = "int"), Display(Name="Novel ID")]
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
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Title", TypeName = "nvarchar"), Display(Name="Title"), StringLength(400)]
		public string Title { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Synopsis", TypeName = "nvarchar"), Display(Name="Synopsis"), StringLength(2000)]
		public string Synopsis { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("ImageUrl", TypeName = "varchar"), Display(Name="Image Url"), StringLength(100)]
		public string ImageUrl { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Status", TypeName = "int"), Display(Name="Status")]
		public int Status { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Type", TypeName = "int"), Display(Name="Type")]
		public int Type { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("RawLanguage", TypeName = "int"), Display(Name="Raw Language")]
		public int RawLanguage { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("FinalLanguage", TypeName = "int"), Display(Name="Final Language")]
		public int FinalLanguage { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("SharingLevel", TypeName = "int"), Display(Name="Sharing Level")]
		public int SharingLevel { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsEnabledTranslator", TypeName = "bit"), Display(Name="Is Enabled Translator")]
		public bool IsEnabledTranslator { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsEnabledEditor", TypeName = "bit"), Display(Name="Is Enabled Editor")]
		public bool IsEnabledEditor { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsEnabledProofreader", TypeName = "bit"), Display(Name="Is Enabled Proofreader")]
		public bool IsEnabledProofreader { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsEnabledSuggestor", TypeName = "bit"), Display(Name="Is Enabled Suggestor")]
		public bool IsEnabledSuggestor { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsEnabledCrowdTranslation", TypeName = "bit"), Display(Name="Is Enabled Crowd Translation")]
		public bool IsEnabledCrowdTranslation { get; set; }
	}
}
