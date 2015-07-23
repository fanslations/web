using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Paranovels.Common //.Generated Resources
{
    /// <summary>
    /// This class contains resources and system settings.
    /// </summary>
    public partial class R
    {
        /// <summary>
        /// ChapterStatus auto generated class.
        /// </summary>
        public partial class ChapterStatus 
        {		
			/// <summary>
			/// Editing
			/// </summary>
			[Description("Editing")]
			public const int EDITING = 200;
				
			/// <summary>
			/// Pending
			/// </summary>
			[Description("Pending")]
			public const int PENDING = 0;
				
			/// <summary>
			/// Proofreading
			/// </summary>
			[Description("Proofreading")]
			public const int PROOFREADING = 300;
				
			/// <summary>
			/// Released
			/// </summary>
			[Description("Released")]
			public const int RELEASED = 1000;
				
			/// <summary>
			/// Translating
			/// </summary>
			[Description("Translating")]
			public const int TRANSLATING = 100;
				
			
			public const string ClassName = "ChapterStatus";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{200,"Editing"},
							
												{0,"Pending"},
							
												{300,"Proofreading"},
							
												{1000,"Released"},
							
												{100,"Translating"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// ConnectorType auto generated class.
        /// </summary>
        public partial class ConnectorType 
        {		
			/// <summary>
			/// Link translations group to data feed url
			/// </summary>
			[Description("Link translations group to data feed url")]
			public const int GROUP_FEED = 5;
				
			/// <summary>
			/// Group Glossary
			/// </summary>
			[Description("Group Glossary")]
			public const int GROUP_GLOSSARY = 7;
				
			/// <summary>
			/// Link novel to a novel author
			/// </summary>
			[Description("Link novel to a novel author")]
			public const int NOVEL_AUTHOR = 1;
				
			/// <summary>
			/// Link translations group to data feed url
			/// </summary>
			[Description("Link translations group to data feed url")]
			public const int NOVEL_FEED = 6;
				
			/// <summary>
			/// Novel Glossary
			/// </summary>
			[Description("Novel Glossary")]
			public const int NOVEL_GLOSSARY = 8;
				
			/// <summary>
			/// Link novel tracker to novel categories
			/// </summary>
			[Description("Link novel tracker to novel categories")]
			public const int SERIES_TAGCATEGORY = 3;
				
			/// <summary>
			/// Link novel tracker to novel contains
			/// </summary>
			[Description("Link novel tracker to novel contains")]
			public const int SERIES_TAGCONTAIN = 4;
				
			/// <summary>
			/// Link novel tracker to novel genres
			/// </summary>
			[Description("Link novel tracker to novel genres")]
			public const int SERIES_TAGGENRE = 2;
				
			/// <summary>
			/// Link seriesto user list
			/// </summary>
			[Description("Link seriesto user list")]
			public const int SERIES_USERLIST = 9;
				
			
			public const string ClassName = "ConnectorType";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{5,"Link translations group to data feed url"},
							
												{7,"Group Glossary"},
							
												{1,"Link novel to a novel author"},
							
												{6,"Link translations group to data feed url"},
							
												{8,"Novel Glossary"},
							
												{3,"Link novel tracker to novel categories"},
							
												{4,"Link novel tracker to novel contains"},
							
												{2,"Link novel tracker to novel genres"},
							
												{9,"Link seriesto user list"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// FeedStatus auto generated class.
        /// </summary>
        public partial class FeedStatus 
        {		
			/// <summary>
			/// Active
			/// </summary>
			[Description("Active")]
			public const int ACTIVE = 200;
				
			/// <summary>
			/// Dead
			/// </summary>
			[Description("Dead")]
			public const int DEAD = 900;
				
			/// <summary>
			/// Inactive
			/// </summary>
			[Description("Inactive")]
			public const int INACTIVE = 300;
				
			/// <summary>
			/// Pending Approval
			/// </summary>
			[Description("Pending Approval")]
			public const int PENDING = 100;
				
			
			public const string ClassName = "FeedStatus";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{200,"Active"},
							
												{900,"Dead"},
							
												{300,"Inactive"},
							
												{100,"Pending Approval"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// GroupStatus auto generated class.
        /// </summary>
        public partial class GroupStatus 
        {		
			/// <summary>
			/// Active
			/// </summary>
			[Description("Active")]
			public const int ACTIVE = 100;
				
			/// <summary>
			/// Disbanded
			/// </summary>
			[Description("Disbanded")]
			public const int DISBANDED = 900;
				
			/// <summary>
			/// Inactive
			/// </summary>
			[Description("Inactive")]
			public const int INACTIVE = 500;
				
			
			public const string ClassName = "GroupStatus";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{100,"Active"},
							
												{900,"Disbanded"},
							
												{500,"Inactive"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// InlineEditType auto generated class.
        /// </summary>
        public partial class InlineEditType 
        {		
			/// <summary>
			/// Checkboxes
			/// </summary>
			[Description("Checkboxes")]
			public const string CHECKBOXES = "checkboxes";
			/// <summary>
			/// Date
			/// </summary>
			[Description("Date")]
			public const string DATE = "date";
			/// <summary>
			/// Number
			/// </summary>
			[Description("Number")]
			public const string NUMBER = "number";
			/// <summary>
			/// Radios
			/// </summary>
			[Description("Radios")]
			public const string RADIOS = "radios";
			/// <summary>
			/// Text
			/// </summary>
			[Description("Text")]
			public const string TEXT = "text";
			/// <summary>
			/// Textarea
			/// </summary>
			[Description("Textarea")]
			public const string TEXTAREA = "textarea";
			
			public const string ClassName = "InlineEditType";
			public static readonly Dictionary<string,string> Translate = new Dictionary<string,string> {
												{"checkboxes","Checkboxes"},
							
												{"date","Date"},
							
												{"number","Number"},
							
												{"radios","Radios"},
							
												{"text","Text"},
							
												{"textarea","Textarea"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// Language auto generated class.
        /// </summary>
        public partial class Language 
        {		
			/// <summary>
			/// Chinese
			/// </summary>
			[Description("Chinese")]
			public const int CHINESE = 3;
				
			/// <summary>
			/// English
			/// </summary>
			[Description("English")]
			public const int ENGLISH = 1;
				
			/// <summary>
			/// Japanese
			/// </summary>
			[Description("Japanese")]
			public const int JAPANESE = 2;
				
			/// <summary>
			/// Korean
			/// </summary>
			[Description("Korean")]
			public const int KOREAN = 4;
				
			
			public const string ClassName = "Language";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{3,"Chinese"},
							
												{1,"English"},
							
												{2,"Japanese"},
							
												{4,"Korean"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// NovelType auto generated class.
        /// </summary>
        public partial class NovelType 
        {		
			/// <summary>
			/// Fan Fiction
			/// </summary>
			[Description("Fan Fiction")]
			public const int FAN_FICTION = 300;
				
			/// <summary>
			/// Original
			/// </summary>
			[Description("Original")]
			public const int ORIGINAL = 200;
				
			/// <summary>
			/// Translation
			/// </summary>
			[Description("Translation")]
			public const int TRANSLATION = 100;
				
			
			public const string ClassName = "NovelType";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{300,"Fan Fiction"},
							
												{200,"Original"},
							
												{100,"Translation"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// SourceTable auto generated class.
        /// </summary>
        public partial class SourceTable 
        {		
			/// <summary>
			/// Author
			/// </summary>
			[Description("Author")]
			public const int AUTHOR = 6;
				
			/// <summary>
			/// Chapter
			/// </summary>
			[Description("Chapter")]
			public const int CHAPTER = 5;
				
			/// <summary>
			/// Comment
			/// </summary>
			[Description("Comment")]
			public const int COMMENT = 7;
				
			/// <summary>
			/// Group
			/// </summary>
			[Description("Group")]
			public const int GROUP = 3;
				
			/// <summary>
			/// Novel
			/// </summary>
			[Description("Novel")]
			public const int NOVEL = 4;
				
			/// <summary>
			/// Release
			/// </summary>
			[Description("Release")]
			public const int RELEASE = 1;
				
			/// <summary>
			/// Series
			/// </summary>
			[Description("Series")]
			public const int SERIES = 2;
				
			
			public const string ClassName = "SourceTable";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{6,"Author"},
							
												{5,"Chapter"},
							
												{7,"Comment"},
							
												{3,"Group"},
							
												{4,"Novel"},
							
												{1,"Release"},
							
												{2,"Series"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// TagType auto generated class.
        /// </summary>
        public partial class TagType 
        {		
			/// <summary>
			/// Glossary
			/// </summary>
			[Description("Glossary")]
			public const int GLOSSARY = 1;
				
			/// <summary>
			/// Categories
			/// </summary>
			[Description("Categories")]
			public const int NOVEL_CATEGORY = 3;
				
			/// <summary>
			/// Contains
			/// </summary>
			[Description("Contains")]
			public const int NOVEL_CONTAIN = 4;
				
			/// <summary>
			/// Genres
			/// </summary>
			[Description("Genres")]
			public const int NOVEL_GENRE = 2;
				
			
			public const string ClassName = "TagType";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{1,"Glossary"},
							
												{3,"Categories"},
							
												{4,"Contains"},
							
												{2,"Genres"},
							
						}; // end translate dictionary
        }	
        /// <summary>
        /// TranslationStatus auto generated class.
        /// </summary>
        public partial class TranslationStatus 
        {		
			/// <summary>
			/// Active
			/// </summary>
			[Description("Active")]
			public const int ACTIVE = 100;
				
			/// <summary>
			/// Completed
			/// </summary>
			[Description("Completed")]
			public const int COMPLETED = 200;
				
			/// <summary>
			/// Dropped
			/// </summary>
			[Description("Dropped")]
			public const int DROPPED = 300;
				
			/// <summary>
			/// Hiatus
			/// </summary>
			[Description("Hiatus")]
			public const int HIATUS = 400;
				
			/// <summary>
			/// Licensed
			/// </summary>
			[Description("Licensed")]
			public const int LICENSED = 500;
				
			/// <summary>
			/// Teaser
			/// </summary>
			[Description("Teaser")]
			public const int TEASER = 700;
				
			/// <summary>
			/// Unknown
			/// </summary>
			[Description("Unknown")]
			public const int UNKNOWN = 999;
				
			
			public const string ClassName = "TranslationStatus";
			public static readonly Dictionary<int,string> Translate = new Dictionary<int,string> {
												{100,"Active"},
							
												{200,"Completed"},
							
												{300,"Dropped"},
							
												{400,"Hiatus"},
							
												{500,"Licensed"},
							
												{700,"Teaser"},
							
												{999,"Unknown"},
							
						}; // end translate dictionary
        }	
             
    }
}
 

