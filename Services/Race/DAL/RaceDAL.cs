using UniANPR.Utility;
using UniANPR.Models.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using ThreeSC.NetStandardLib.StandardTools.Interfaces;

namespace UniANPR.Services.Race.DAL
{
    public partial class RaceService 
    {
        private class RaceDAL
        {
            #region Private Declarations

            private IThreeSCApplicationLogger _applicationLogger;

            /// <summary>
            /// The database connection string being used by this DAL class
            /// </summary>
            private string _supervisorySystemDatabaseConnectionString;
            #endregion

            #region Constructor

            /// <summary>
            /// Create a new Data Access Layer, which does all database accessing for the service
            /// </summary>
            /// <param name="applicationLogger">Three SC Application logger to use for errors</param>
            /// <param name="supervisorySystemDatabaseConnectionString">Connection string for the abds supervisory database</param>
            internal RaceDAL(IThreeSCApplicationLogger applicationLogger, string supervisorySystemDatabaseConnectionString)
            {
                _applicationLogger = applicationLogger;
                _supervisorySystemDatabaseConnectionString = supervisorySystemDatabaseConnectionString;
            }
            #endregion

            #region Internal Methods

            /// <summary>
            /// Log a race lap
            /// </summary>
            internal void LogRaceLap(int userId, Lap_SM thisLap)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_supervisorySystemDatabaseConnectionString))
                    {
                        String query = "INSERT INTO dbo.Lap (" +
                                       "[RaceId], " +
                                       "[UserId], " +
                                       "[LapNumber], " +
                                       "[TimeCrossed]) " +
                                       "VALUES (" +
                                       "@RaceId, " +
                                       "@UserId, " +
                                       "@LapNumber, " +
                                       "@TimeCrossed)";


                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RaceId", thisLap.RaceId);
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@LapNumber", thisLap.LapNumber);
                            command.Parameters.AddWithValue("@TimeCrossed", thisLap.TimeCrossed);

                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            command.ExecuteScalar();
                        }
                    }

                }
                catch (Exception ex)
                {
                    _applicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, String.Empty, ex, null);
                }
            }

            internal List<RaceTrack_DM> GetAllRaceTracks()
            {
                List<RaceTrack_DM> theseResults;

                string query = "SELECT " +
                                "[Id]," +
                                "[TrackName] " +
                                "FROM [dbo].[RaceTrack] ";

                try
                {
                    theseResults = SQLHelper.ReadTableFromDatabaseIntoList<RaceTrack_DM>(_supervisorySystemDatabaseConnectionString, query);
                }
                catch (Exception ex)
                {
                    _applicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, $"Failed to retrieve race track definitions.", ex, null);
                    theseResults = new List<RaceTrack_DM>();
                }

                return theseResults;
            }

            #endregion
        }
    }

}
