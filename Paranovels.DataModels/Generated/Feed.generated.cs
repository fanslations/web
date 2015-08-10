// This class was automatically generated with love by ST4bby 8/9/2015 3:28:51 PM.
// Read more at http://jbubriski.github.com/ST4bby/

namespace Paranovels.DataModels
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Thi.Core;
	
	[Table("Feed")]
	public partial class Feed : IDataModel
	{
		[NotMapped]public int ID { get { return FeedID; } set { FeedID = value; } }
	
		[Key, Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("FeedID", TypeName = "int"), Display(Name="Feed ID")]
		public int FeedID { get; set; }
		
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
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Url", TypeName = "nvarchar"), Display(Name="Url"), StringLength(400)]
		public string Url { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("UrlHash", TypeName = "int"), Display(Name="Url Hash")]
		public int UrlHash { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Type", TypeName = "int"), Display(Name="Type")]
		public int Type { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Status", TypeName = "int"), Display(Name="Status")]
		public int Status { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Total", TypeName = "int"), Display(Name="Total")]
		public int Total { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("LastSuccessDate", TypeName = "datetime"), Display(Name="Last Success Date")]
		public DateTime LastSuccessDate { get; set; }
	}
}
