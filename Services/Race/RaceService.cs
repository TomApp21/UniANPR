using UniANPR.Interfaces;
using ThreeSC.Net6Lib.BlazorTools.Services;
using UniANPR.Models.Services;
using ThreeSC.NetStandardLib.StandardTools.Interfaces;
using ABDS.Supervisory.GUI.Models.Configuration;
using DemoANPR.Models.Components;
using DemoANPR.Models.Services;
using UniANPR.Enum;

namespace UniANPR.Services.Race
{
    public partial class RaceService : ThreeSCServiceBase, IHostedService, IRaceService
    {
        #region Constants
        #endregion

        #region Delegate Definitions

        /// <summary>
        /// Delegate defining the callback to the client when the distribution of vehicles across the various Task Statuses has changed
        /// </summary>
        /// <param name="vehicleContainerAllocationData"></param>
        public delegate void TrackDataChangeDelegate (List<Track_SM> trackData);

        public delegate void PendingRaceDataChangeDelegate (List<Race_SM> pendingRaceData);


        #endregion

        #region Delegate & Subscriber Private Declarations

        private Dictionary<int, TrackDataChangeDelegate> _allTrackDataChangedDelegates; 
        private Dictionary<int, PendingRaceDataChangeDelegate> _allPendingRaceDataChangedDelegates; 

        #endregion

        #region Declarations
        public List<Track_SM> allTrackData { get; set; }

        public List<Race_SM> allRaceData { get; set; }
        public List<Race_SM> allPendingRaceData { get; set; }


        private RaceService_DAL _thisRaceService_DAL;


        #endregion

        #region Private declarations

        private IThreeSCApplicationLogger _thisThreeSCApplicationLogger;
        //private IThreeSCUserAuditLogger<_enmABDSGUIUserAuditEnum

            #endregion  

        #region Properties
        #endregion

        #region Constructor

        public RaceService(IThreeSCApplicationLogger thisThreeSCApplicationLogger, IConfiguration configuration)
        {
            _thisThreeSCApplicationLogger = thisThreeSCApplicationLogger;

            UniANPROptions thisConfig = new UniANPROptions(configuration);

            _thisRaceService_DAL = new RaceService_DAL(_thisThreeSCApplicationLogger, thisConfig.DatabaseConnectionString);

            _allPendingRaceDataChangedDelegates = new Dictionary<int, PendingRaceDataChangeDelegate>();
            _allTrackDataChangedDelegates = new Dictionary<int, TrackDataChangeDelegate>();
        }
        #endregion

        #region Hosted service start stop implementations

        /// <summary>
        /// On being started by the webApp service, read all current data from the database
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// 
        /// <returns></returns>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            //InitialisePublishedGeofenceData();
            InitialiseTrackData();
            InitialiseRaceData();

            return Task.CompletedTask;
        }

        /// <summary>
        /// On being stopped by the webApp service
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region Initialise Service Data

        /// <summary>
        ///  Gets track data on start-up
        /// </summary>
        private void InitialiseTrackData()
        {
            List<RaceTrack_DM> trackData = _thisRaceService_DAL.GetAllRaceTracks();

            allTrackData = (from td in trackData
                                 select new Track_SM()
                                 {
                                     TrackId = td.Id ,
                                     TrackName = td.TrackName
                                 }).ToList();
        }

        
        /// <summary>
        ///  Gets track data on start-up
        /// </summary>
        private void InitialiseRaceData()
        {
            List<Race_DM> raceData = _thisRaceService_DAL.GetAllRaces();

            allRaceData = (from rd in raceData
                                 select new Race_SM()
                                 {
                                     RaceId = rd.RaceId,
                                     RaceTrackId = rd.RaceTrackId,
                                     RequiredLaps = rd.RequiredLaps,
                                     Spots = rd.SpotLimit,
                                     StartTime = rd.StartTime,
                                     EndTime = rd.EndTime,
                                     RaceName = rd.RaceName,
                                     RaceStatus = (RaceStatus)rd.RaceStatusEnumValue,
                                 }).ToList();
        
            // populate active participants
        }



        #endregion

        #region Private initialise geofence and segment published data

        #endregion

        #region AllVehicleIds Subscribe, call delegates and unsubscribe methods

        /// <summary>
        /// Register a delegate to receive all changes to the list of vehicle ids
        /// </summary>
        /// <param name="newAllVehicleIdsChangedHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns>The subscriber id associated with this registration</returns>
        public int AddSubscriber_TrackDataChanged(TrackDataChangeDelegate newHandler)
        {
            int subscriberId = ThreadSafeAddToSubscriberDictionary<TrackDataChangeDelegate>(_allTrackDataChangedDelegates, newHandler);

            Task.Run(() => { newHandler.Invoke(allTrackData.ToList()); });

            return subscriberId;
        }

        /// <summary>
        /// Remove specified subscriber to AllVehicleIdsChanged
        /// </summary>
        /// <param name="subscriberId">The subscriber id associated with this registration</param>
        public void RemoveSubscriber_TrackDataChanged(int subscriberId)
        {
            ThreadSafeRemoveFromSubscriberDictionary<TrackDataChangeDelegate>(_allTrackDataChangedDelegates, subscriberId);
        }


        /// <summary>
        /// Inform all subscribers of a change
        /// </summary>
        private void SendToAllTrackDataChangedDelegates()
        {
            lock (_allTrackDataChangedDelegates)
            {
                foreach (int subscriberId in _allTrackDataChangedDelegates.Keys)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            _allTrackDataChangedDelegates[subscriberId].Invoke(allTrackData.ToList());
                        }
                        catch (Exception ex)
                        {
                            //_thisThreeSCApplicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, "subscriberId[" + subscriberId.ToString() + "]", ex, null);
                        }
                    });
                }
            }
        }
        #endregion


        #region AllVehicleIds Subscribe, call delegates and unsubscribe methods

        /// <summary>
        /// Register a delegate to receive all changes to the list of vehicle ids
        /// </summary>
        /// <param name="newAllVehicleIdsChangedHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns>The subscriber id associated with this registration</returns>
        public int AddSubscriber_PendingRaceDataChanged(PendingRaceDataChangeDelegate newHandler)
        {
            int subscriberId = ThreadSafeAddToSubscriberDictionary<PendingRaceDataChangeDelegate>(_allPendingRaceDataChangedDelegates, newHandler);

            Task.Run(() => { newHandler.Invoke(allRaceData.Where(x => x.StartTime > DateTime.Now).ToList()); });

            return subscriberId;
        }

        /// <summary>
        /// Remove specified subscriber to AllVehicleIdsChanged
        /// </summary>
        /// <param name="subscriberId">The subscriber id associated with this registration</param>
        public void RemoveSubscriber_PendingRaceDataChanged(int subscriberId)
        {
            ThreadSafeRemoveFromSubscriberDictionary<PendingRaceDataChangeDelegate>(_allPendingRaceDataChangedDelegates, subscriberId);
        }


        /// <summary>
        /// Inform all subscribers of a change
        /// </summary>
        private void SendToAllPendingRaceDataChangedDelegates()
        {
            lock (_allPendingRaceDataChangedDelegates)
            {
                foreach (int subscriberId in _allPendingRaceDataChangedDelegates.Keys)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            _allPendingRaceDataChangedDelegates[subscriberId].Invoke(allRaceData.Where(x => x.StartTime > DateTime.Now).ToList());
                        }
                        catch (Exception ex)
                        {
                            //_thisThreeSCApplicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, "subscriberId[" + subscriberId.ToString() + "]", ex, null);
                        }
                    });
                }
            }
        }
        #endregion


        #region Validation Checks

        /// <summary>
        /// Checks track name store in service to see if track already exists
        /// </summary>
        /// <param name="newTrackName"></param>
        /// <returns></returns>
        public bool CheckIfTrackNameExists(string newTrackName)
        {
            return allTrackData.Select(x => x.TrackName).Contains(newTrackName) ? true : false;
        }

        #endregion


        #region DAL Calls
        /// <summary>
        /// Adds Track to database
        /// </summary>
        /// <param name="trackName"></param>
        /// <returns></returns>
        public bool AddNewTrack(string trackName)
        {
            bool blnSuccess = _thisRaceService_DAL.AddTrack(trackName);

            if (blnSuccess)
            {
                SendToAllTrackDataChangedDelegates();
            }

            return blnSuccess;
        }

        public bool ScheduleNewRace(Race_VM raceToCreate)
        {
            bool blnSuccess = false;

            Race_SM thisRace = new Race_SM()
            {
                RaceName = raceToCreate.RaceName,
                RequiredLaps = raceToCreate.RequiredLaps,
                Spots = raceToCreate.Spots,
                StartTime = raceToCreate.StartTime,
                EndTime = raceToCreate.EndTime,
                RaceTrackId = raceToCreate.RaceTrackId,
                RaceStatus = Enum.RaceStatus.RegistrationActive
            };

            blnSuccess = _thisRaceService_DAL.ScheduleNewRace(thisRace);

            if (blnSuccess)
            {
                InitialiseRaceData();
                SendToAllPendingRaceDataChangedDelegates();
            }

            return blnSuccess;
        }

        public bool ProcessParticipantAwaitingRegistration(int participantId, int raceId, bool approveRacer)
        {
            bool blnSuccess = false;

            blnSuccess = _thisRaceService_DAL.ProcessParticipantAwaitingRegistration(participantId, raceId, approveRacer);

            if (blnSuccess)
            {

            }

            return blnSuccess;
        }

        #endregion


    }
}
