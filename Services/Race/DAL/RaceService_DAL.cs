using UniANPR.Utility;
using UniANPR.Models.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using ThreeSC.NetStandardLib.StandardTools.Interfaces;
using DemoANPR.Models.Services;

namespace UniANPR.Services.Race
{
    public partial class RaceService
    {
        private class RaceService_DAL
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
            internal RaceService_DAL(IThreeSCApplicationLogger applicationLogger, string supervisorySystemDatabaseConnectionString)
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


            internal List<Race_DM> GetAllRaces()
            {
                List<Race_DM> theseResults;

                string query = "SELECT " +
                                "[Id]," +
                                "[RaceTrackId], " +
                                "[SpotLimit]," +
                                "[StartTime], " +
                                "[EndTime]," +
                                "[RaceStatusEnumValue], " +
                                "[RaceName]," +
                                "[RequiredLaps] " +
                                "FROM [dbo].[Race] ";

                try
                {
                    theseResults = SQLHelper.ReadTableFromDatabaseIntoList<Race_DM>(_supervisorySystemDatabaseConnectionString, query);
                }
                catch (Exception ex)
                {
                    _applicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, $"Failed to retrieve races.", ex, null);
                    theseResults = new List<Race_DM>();
                }

                return theseResults;
            }

            /// <summary>
            /// Schedule a race
            /// </summary>
            internal bool ScheduleNewRace(Race_SM thisRace)
            {
                bool blnSuccess = false;

                try
                {
                    using (SqlConnection connection = new SqlConnection(_supervisorySystemDatabaseConnectionString))
                    {
                        String query = "INSERT INTO dbo.Race (" +
                                       "[RaceTrackId], " +
                                       "[SpotLimit], " +
                                       "[StartTime], " +
                                       "[EndTime], " +
                                       "[RaceStatusEnumValue], " +
                                       "[RaceName], " +
                                       "[RequiredLaps])" +
                                       "VALUES (" +
                                       "@RaceTrackId, " +
                                       "@SpotLimit, " +
                                       "@StartTime, " +
                                       "@EndTime, " +
                                       "@RaceStatusEnumValue, " +
                                       "@RaceName, " +
                                       "@RequiredLaps)";


                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@RaceTrackId", thisRace.RaceTrackId);
                            command.Parameters.AddWithValue("@SpotLimit", thisRace.Spots);
                            command.Parameters.AddWithValue("@StartTime", thisRace.StartTime);
                            command.Parameters.AddWithValue("@EndTime", thisRace.EndTime);
                            command.Parameters.AddWithValue("@RaceStatusEnumValue", (int)thisRace.RaceStatus);
                            command.Parameters.AddWithValue("@RaceName", thisRace.RaceName);
                            command.Parameters.AddWithValue("@RequiredLaps", thisRace.RequiredLaps);


                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            command.ExecuteScalar();
                            blnSuccess = true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _applicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, String.Empty, ex, null);
                }

                return blnSuccess;
            }

            /// <summary>
            /// Log a race lap
            /// </summary>
            internal bool AddTrack(string trackName)
            {
                bool blnSuccess = false;
                try
                {
                    using (SqlConnection connection = new SqlConnection(_supervisorySystemDatabaseConnectionString))
                    {
                        String query = "INSERT INTO dbo.TrackName (" +
                                       "[TrackName]) " +
                                       "VALUES (" +
                                       "@TrackName)";


                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@TrackName", trackName);

                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            command.ExecuteScalar();
                            blnSuccess = true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _applicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, String.Empty, ex, null);
                }
                return blnSuccess;
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

            //internal List<Participant_DM> GetParticipantsAwaitingRegistrationForRace(int raceId)
            //{
            //    List<Participant_DM> theseResults;

            //    string query = "SELECT " +
            //                    "[ParticipantId]" +
            //                    "FROM [dbo].[RaceTrack] " +
            //                    $"WHERE [RaceId] = {raceId} ";
            //    try
            //    {
            //        theseResults = SQLHelper.ReadTableFromDatabaseIntoList<Participant_DM>(_supervisorySystemDatabaseConnectionString, query);
            //    }
            //    catch (Exception ex)
            //    {
            //        _applicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, $"Failed to retrieve race track definitions.", ex, null);
            //        theseResults = new List<Participant_DM>();
            //    }

            //    return theseResults;
            //}


            internal List<Participant_DM> GetParticipantsForRace(int raceId)
            {
                List<Participant_DM> theseResults;

                string query = "SELECT " +
                                "[ParticipantId] " +
                                "[Approved] " +
                                "FROM [dbo].[RaceTrack] " +
                                $"WHERE [RaceId] = {raceId} ";
                try
                {
                    theseResults = SQLHelper.ReadTableFromDatabaseIntoList<Participant_DM>(_supervisorySystemDatabaseConnectionString, query);
                }
                catch (Exception ex)
                {
                    _applicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, $"Failed to retrieve race track definitions.", ex, null);
                    theseResults = new List<Participant_DM>();
                }

                return theseResults;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="participantId"></param>
            /// <param name="raceId"></param>
            /// <param name="approveRacer"></param>
            /// <returns></returns>
            internal bool ProcessParticipantAwaitingRegistration(int participantId, int raceId, bool approveRacer)
            {
                bool blnSuccess = false;

                using (SqlConnection connection = new SqlConnection(_supervisorySystemDatabaseConnectionString))
                {
                    connection.Open();

                    try
                    {

                        SqlCommand processParticipantCommand = connection.CreateCommand();
                        processParticipantCommand.CommandText = $"UPDATE [dbo].[Participant] SET Approved = @Approved WHERE ParticipantId=@ParticipantId AND RaceId=@VehicleId";
                        processParticipantCommand.Parameters.AddWithValue("@Approved", approveRacer);
                        processParticipantCommand.Parameters.AddWithValue("@ParticipantId", participantId);
                        processParticipantCommand.Parameters.AddWithValue("@RaceId", raceId);
                        processParticipantCommand.ExecuteScalar();

                        blnSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        _applicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, $"unexpected error creating vehicleId{vehicleId}", ex, useridForLogging);
                    }
                }

                return blnSuccess;

                #endregion
            }

        }
    }
}
