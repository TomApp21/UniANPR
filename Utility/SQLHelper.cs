using FastMember;
using Microsoft.Data.SqlClient;

namespace UniANPR.Utility
{
    public static class SQLHelper
    {
        /// <summary>
        /// Static helper method to execute a sql select command and popoulate a list of objects of types specified in the type parameter 
        /// </summary>
        /// <typeparam name="TModel">Type to populate list of instances of</typeparam>
        /// <param name="databaseConnectionString">Database connection string to execute the SQL Select statement on</param>
        /// <param name="sqlSelect">Sql select statement string to execute to get the data</param>
        /// <returns>
        /// List of types populated with data retrieved from the sql select statement
        /// </returns>
        /// <remarks>
        /// Select column names must match the class type's property names, which must be public
        /// </remarks>
        public static List<TModel> ReadTableFromDatabaseIntoList<TModel>(string databaseConnectionString, string sqlSelect) where TModel : class, new()
        {
            var thisList = new List<TModel>();

            using (var dbConnection = new SqlConnection(databaseConnectionString))
            {

                dbConnection.Open();
                using (var cmd = new SqlCommand(sqlSelect, dbConnection))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        thisList.Add(SQLHelper.ConvertSqlReaderResultToModel<TModel>(reader));
                    }
                }
            }

            return thisList;
        }

        
        /// <summary>
        /// Static helper method to popoulate an object of types specified in the type parameter from the given sql data reader
        /// </summary>
        /// <typeparam name="TModel">Type to populate instances of</typeparam>
        /// <param name="thisSqlReader">Sql Data Reader to get to get the data from</param>
        /// <returns>
        /// Instance of type populated with data retrieved from the sql data reader
        /// </returns>
        /// <remarks>
        /// Sql column names must match the class type's property names, which must be public
        /// </remarks>        
        public static TModel ConvertSqlReaderResultToModel<TModel>(this SqlDataReader thisSqlReader) where TModel : class, new()
        {
            Type type = typeof(TModel);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new TModel();

            for (int i = 0; i < thisSqlReader.FieldCount; i++)
            {
                if (!thisSqlReader.IsDBNull(i))
                {
                    string fieldName = thisSqlReader.GetName(i);
                    //var thisMember = (from m in members
                    //                  where m.Name.ToLower() == fieldName.ToLower()// && !m.Type.IsClass
                    //                  select m).FirstOrDefault();
                    //if (thisMember != null)
                    //{
                    //    accessor[t, fieldName] = thisSqlReader.GetValue(i);
                    //}
                    if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                    {
                        accessor[t, fieldName] = thisSqlReader.GetValue(i);
                    }
                }
            }

            return t;
        }
    }
}
