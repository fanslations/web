// This class was automatically generated with love by ST4bby 8/3/2015 12:14:25 AM.
// Read more at http://jbubriski.github.com/ST4bby/

namespace Paranovels.DataModels
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Thi.Core;
	
	[Table("UserList")]
	public partial class UserList : IDataModel
	{
		[NotMapped]public int ID { get { return UserListID; } set { UserListID = value; } }
	
		[Key, Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("UserListID", TypeName = "int"), Display(Name="User List ID")]
		public int UserListID { get; set; }
		
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
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Name", TypeName = "nvarchar"), Display(Name="Name"), StringLength(100)]
		public string Name { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Description", TypeName = "nvarchar"), Display(Name="Description"), StringLength(400)]
		public string Description { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("ShareLevel", TypeName = "int"), Display(Name="Share Level")]
		public int ShareLevel { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Type", TypeName = "int"), Display(Name="Type")]
		public int Type { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Icon", TypeName = "int"), Display(Name="Icon")]
		public int Icon { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("Color", TypeName = "int"), Display(Name="Color")]
		public int Color { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsColorIcon", TypeName = "bit"), Display(Name="Is Color Icon")]
		public bool IsColorIcon { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsPlaceInFrontOfTitle", TypeName = "bit"), Display(Name="Is Place In Front Of Title")]
		public bool IsPlaceInFrontOfTitle { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsHiddenInFrontpage", TypeName = "bit"), Display(Name="Is Hidden In Frontpage")]
		public bool IsHiddenInFrontpage { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsNotifyOfNewRelease", TypeName = "bit"), Display(Name="Is Notify Of New Release")]
		public bool IsNotifyOfNewRelease { get; set; }
		
		[Required(AllowEmptyStrings = true), DisplayFormat(ConvertEmptyStringToNull = false), Column("IsAutoAddWhenRead", TypeName = "bit"), Display(Name="Is Auto Add When Read")]
		public bool IsAutoAddWhenRead { get; set; }
	}
}
