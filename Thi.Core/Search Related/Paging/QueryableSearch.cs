using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Thi.Core
{
    /// <summary>
    /// This class will take any IQueryable and search through it recursively with minimal effort
    /// This was designed to be used with Linq-to-SQL or Linq-to-Entities, but also works on DataTables and other 'multi-column' IQueryables
    /// 
    /// This can be as general or specific as  you want it to be. For example, if you simply want to search 5 text fields, you just pass in the 5 field names, and the keywords to seach for.
    /// Or, if you want to be super specific, you can do that too:
    /// Say you want to pass in an IQueryable with the following fields: int named 'ID', DateTime named 'birthday', text named 'name' and bool named 'male'
    /// you would make following Dictionary[column_name, type]:
    ///   ColumnNamesAndTypes.Add("ID", typeof(int));
    ///   ColumnNamesAndTypes.Add("birthday", typeof(DateTime));
    ///   ColumnNamesAndTypes.Add("name", typeof(string));
    ///   ColumnNamesAndTypes.Add("male", typeof(bool));
    ///   
    /// then use a more specific overload:
    /// yourIQeryable.Search(ColumnNamesAndTypes, objectArrayOfKeywords);
    /// 
    /// the object array of keywords will only be run against corresponding column types, so if you pass in a boolean value 'True', it will only be run against the "male" column and not the others
    /// </summary>
    public static class QueryableSearch
    {
        public enum StringSearchType { Contains, Equals };


        /// <summary>
        /// Search an IQueryable's fields for the keywords (objects) - this does not iterate through levels of an object
        /// </summary>
        /// <param name="list_to_search">IQueryable to search</param>
        /// <param name="keywords">objects to search for</param>
        /// <returns>IQueryable of the inputed type filtered by the search specifications</returns>
        public static IQueryable Search(this IQueryable list_to_search, object[] keywords)
        {
            Dictionary<string, Type> dic = new Dictionary<string, Type>();
            foreach (var item in list_to_search.Take(1))
                foreach (PropertyInfo pi in item.GetType().GetProperties()) dic.Add(pi.Name, pi.PropertyType);
            return list_to_search.Search(dic, keywords);
        }

        /// <summary>
        /// Search an IQueryable's specified text fields if they contain the keywords; will not work for any fields other than string fields, if you ware using fields that are not strings, use on of the more specific overloads
        /// </summary>
        /// <param name="list_to_search">IQueryable to search</param>
        /// <param name="columns_to_search">string names of the columns to be searched within the IQueryable, may be nest reltaions as well</param>
        /// <param name="keywords">array for strings to search for</param>
        /// <returns>IQueryable of the inputed type filtered by the search specifications</returns>
        public static IQueryable Search(this IQueryable list_to_search, string[] columns_to_search, string[] keywords)
        {
            return Search(list_to_search, columns_to_search, keywords, StringSearchType.Contains);
        }

        /// <summary>
        /// Search an IQueryable's specified text fields if they contain/equal the keywords;  will not work for any fields other than string fields, if you ware using fields that are not strings, use on of the more specific overloads
        /// </summary>
        /// <param name="list_to_search">IQueryable to search</param>
        /// <param name="columns_to_search">string names of the columns to be searched within the IQueryable, may be nest relations as well</param>
        /// <param name="keywords">array for strings to search for</param>
        /// <param name="string_search_type">Whether or not the string operations use the strict 'Equals' or the broader 'Contains' method</param>
        /// <returns>IQueryable of the inputed type filtered by the search specifications</returns>
        public static IQueryable Search(this IQueryable list_to_search, string[] columns_to_search, string[] keywords, StringSearchType string_search_type)
        {
            return Search(list_to_search, MakeDictionary(columns_to_search), keywords, string_search_type);
        }

        /// <summary>
        /// Search an IQueryable's specified fields if they contain/equal the keywords; fields other than text will be forced to an '==' operator; strings will default to Contains with this overload
        /// </summary>
        /// <param name="list_to_search">IQueryable to search</param>
        /// <param name="columns_to_search">Dictionary with KeyValuePairs of [column_name, type_of_column]; such as [record_id, typeof(int)] or [birthdate, typeof(DateTime)]</param>
        /// <param name="keywords">array of objects of 'keywords' to search for, any type of objects</param>
        /// <returns>IQueryable of the inputed type filtered by the search specifications</returns>
        public static IQueryable Search(this IQueryable list_to_search, Dictionary<string, Type> columns_to_search, object[] keywords)
        {
            return Search(list_to_search, columns_to_search, keywords, StringSearchType.Contains);
        }

        /// <summary>
        /// Search an IQueryable's specified fields if they contain/equal the keywords; fields other than text will be forced to an '==' operator
        /// </summary>
        /// <param name="list_to_search">IQueryable to search</param>
        /// <param name="columns_to_search">Dictionary with KeyValuePairs of [column_name, type_of_column]; such as [record_id, typeof(int)] or [birthdate, typeof(DateTime)]</param>
        /// <param name="keywords">array of objects of 'keywords' to search for, any type of objects</param>
        /// <param name="string_search_type">Whether or not the string operations use the strict 'Equals' or the broader 'Contains' method</param>
        /// <returns>IQueryable of the inputed type filtered by the search specifications</returns>
        public static IQueryable Search(this IQueryable list_to_search, Dictionary<string, Type> columns_to_search, object[] keywords, StringSearchType string_search_type)
        {
            Dictionary<object, string> search_object_combos = new Dictionary<object, string>();

            foreach (object o in keywords)
            {
                string where_expression = string.Empty;
                foreach (KeyValuePair<string, Type> column in columns_to_search)
                {
                    if (o.GetType() == column.Value)
                    {
                        if (column.Value == typeof(string))
                            where_expression += column.Key + ".ToLower()." + (string_search_type == StringSearchType.Equals ? "Equals" : "Contains") + "(@0) || ";
                        else // any other data types will run against the keyword with a '=='
                            where_expression += column.Key + " == @0 || ";
                    }
                }
                search_object_combos.AddSearchObjectCombo(where_expression, o);
            }

            IQueryable results;
            if (search_object_combos.Count() == 0) results = null; //nothing to search
            else results = list_to_search.SearchInitial(search_object_combos.First().Value, search_object_combos.First().Key);

            if (search_object_combos.Count() > 1)
            {   // otherwise, keep use the resulting set and recursively filter it
                search_object_combos.Remove(search_object_combos.First().Key);
                foreach (KeyValuePair<object, string> combo in search_object_combos)
                    results = Search(results, combo.Value, combo.Key);
            }
            return results;
        }

        /// <summary>
        /// Adds a where_expression and object to a Dictionary used to hold the information for a search query
        /// </summary>
        /// <param name="dic">Dictionary for holding combinations of where_expressions and objects</param>
        /// <param name="where_expression">LINQ where expression</param>
        /// <param name="o">object corresponding to the where_expression</param>
        private static void AddSearchObjectCombo(this Dictionary<object, string> dic, string where_expression, object o)
        {
            // remove the last " || "
            where_expression = where_expression.Remove(where_expression.Length - 4, 4);
            dic.Add(o, where_expression);
        }

        /// <summary>
        /// Called by the public Search function recursively after the initial search is made
        /// </summary>
        /// <param name="results">Results set from the previous search</param>
        /// <param name="where_expression">LINQ where expression</param>
        /// <param name="keyword">object corresponding to the where_expression</param>
        /// <returns>IQueryable of the inputed type filtered by this search specification</returns>
        private static IQueryable Search(IQueryable results, string where_expression, object keyword)
        {
            return results.Where(where_expression, keyword);
        }

        /// <summary>
        /// This will initially search the IQueryable, if you are using Linq-to-SQL or Linq-to-Entites, this will be your only DB call
        /// </summary>
        /// <param name="list_to_search">IQueryable to search</param>
        /// <param name="where_expression">LINQ where expression</param>
        /// <param name="keyword">object corresponding to the where_expression</param>
        /// <returns>IQueryable of the inputed type filtered by this search specification</returns>
        private static IQueryable SearchInitial(this IQueryable list_to_search, string where_expression, object keyword)
        {
            return list_to_search.Where(where_expression, keyword);
        }

        /// <summary>
        /// Takes in a string[] and outputs a corresponding Dictionary[string, typeof(string)] to feed through the search
        /// </summary>
        /// <param name="columns_to_search">array of column names</param>
        /// <returns>Dictionary[string, typeof(string)]</returns>
        private static Dictionary<string, Type> MakeDictionary(string[] columns_to_search)
        {
            Dictionary<string, Type> columns = new Dictionary<string, Type>();
            foreach (string s in columns_to_search) columns.Add(s, typeof(string));
            return columns;
        }
    }
}