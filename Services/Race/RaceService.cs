using UniANPR.Interfaces;
using ThreeSC.Net6Lib.BlazorTools.Services;
using UniANPR.Models.Services;
using ThreeSC.NetStandardLib.StandardTools.Interfaces;
using ABDS.Supervisory.GUI.Models.Configuration;
using UniANPR.Models.Components;
using UniANPR.Models.Services;
using UniANPR.Enum;
using UniANPR.Models.Components;
using UniANPR.Utility;
using UniANPR.Models;

namespace UniANPR.Services.Race
{
    public partial class RaceService : ThreeSCServiceBase, IHostedService, IRaceService
    {
        #region Constants

        private const string _c_FilePathForDetectedImages = "C:\\Users\\TomAppleyard\\Pictures";


        #endregion

        #region Delegate Definitions

        public delegate void RaceParticipantDataChangedDelegate(Dictionary<int, List<Participant_SM>> allRaceParticipantData);
        /// <summary>
        /// Delegate defining the callback to the client when the distribution of vehicles across the various Task Statuses has changed
        /// </summary>
        /// <param name="vehicleContainerAllocationData"></param>
        public delegate void TrackDataChangeDelegate (List<Track_SM> trackData);

        public delegate void PendingRaceDataChangeDelegate (List<Race_SM> pendingRaceData);

        public delegate void ActiveRaceLapDataChangeDelegate(List<Lap_SM> activeRaceLapData);
        
        public delegate void ActiveRaceBuildUpDataChangeDelegate(Race_SM activeRaceBuildUpData);


        #endregion

        #region Delegate & Subscriber Private Declarations

        private Dictionary<int, RaceParticipantDataChangedDelegate> _allRaceParticipantDataChangedDelegates;
        private Dictionary<int, TrackDataChangeDelegate> _allTrackDataChangedDelegates; 
        private Dictionary<int, PendingRaceDataChangeDelegate> _allPendingRaceDataChangedDelegates;

        private Dictionary<int, ActiveRaceLapDataChangeDelegate> _allActiveRaceLapDataChangeDelegates;
                private Dictionary<int, ActiveRaceBuildUpDataChangeDelegate> _allActiveRaceBuildUpDataChangeDelegates;


        #endregion

        #region Declarations
        public List<Track_SM> allTrackData { get; set; }

        public List<Lap_SM> allActiveRaceLapData { get; set; }

        public List<Race_SM> allRaceData { get; set; }
        public List<Race_SM> allPendingRaceData { get; set; }

        public List<Lap_SM> allLapData { get; set; }

        public Dictionary<int, List<Participant_SM>> allRacesParticipantsData { get; set;}
        public bool raceIsActive { get; set; }

        public Race_SM activeRace { get; set; }

        private RaceService_DAL _thisRaceService_DAL;


        
        private string folderPath;
        private FileSystemWatcher watcher;
        private TheosAPI theosAPI;
        private DirectoryInfo directoryInfo;


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

            _allRaceParticipantDataChangedDelegates = new Dictionary<int, RaceParticipantDataChangedDelegate>();
            _allPendingRaceDataChangedDelegates = new Dictionary<int, PendingRaceDataChangeDelegate>();
            _allTrackDataChangedDelegates = new Dictionary<int, TrackDataChangeDelegate>();
            _allActiveRaceLapDataChangeDelegates = new Dictionary<int, ActiveRaceLapDataChangeDelegate>();
            _allActiveRaceBuildUpDataChangeDelegates = new Dictionary<int, ActiveRaceBuildUpDataChangeDelegate>();

            theosAPI = new TheosAPI();
            directoryInfo = new DirectoryInfo(_c_FilePathForDetectedImages);


            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            raceIsActive = false;
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
            InitialiseLapData();

            StartImageWatcher();

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

        #region Image Watcher

        public void StartImageWatcher()
        {
            // Create a new instance of FileSystemWatcher
            watcher = new FileSystemWatcher();

            // Set the path of the folder to monitor
            watcher.Path = _c_FilePathForDetectedImages;

            // Filter only image files
            watcher.Filter = "*.jpg"; // You can modify the filter according to your image file types

            // Subscribe to the event handlers
            watcher.Created += OnImageDetected;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("ImageWatcher started. Waiting for new images...");
        }

        public void StopImageWatcher()
        {
            if (watcher != null)
            {
                watcher.Created -= OnImageDetected;
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                watcher = null;
            }

            Console.WriteLine("ImageWatcher stopped.");
        }

        private async void OnImageDetected(object sender, FileSystemEventArgs e)
        {
            FileInfo file = directoryInfo.GetFiles(e.Name).FirstOrDefault();
            
            //file.CreationTimeUtc

            List<NumberPlate> detectedObjects = new List<NumberPlate>();

            DateTime timeBeforeRequest = DateTime.Now;
            detectedObjects = await theosAPI.Detect(file, confThres: 0.5f, iouThres: 0.5f, ocrModel: "small", ocrClasses: "numberplate", retries: 5, delay: 2);

            TimeSpan elapsedTime = (DateTime.Now - timeBeforeRequest);
            int ms = (int)elapsedTime.TotalMilliseconds;

            Console.WriteLine($"New image detected: {e.Name}");


            foreach (NumberPlate numberPlate in detectedObjects)
            {
                // Check if participant is included in registered race
                // 


            }







            // You can perform any desired action here when a new image is added to the folder
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
            Dictionary<int, List<Participant_SM>> allRacesParticipants = new Dictionary<int, List<Participant_SM>>();

            List<Race_DM> raceData = _thisRaceService_DAL.GetAllRaces();

            foreach (int raceId in raceData.Select(x => x.Id).ToList())
            {
                List<Participant_DM> thisRaceParticipants = _thisRaceService_DAL.GetParticipantsForRace(raceId);
                
                List<Participant_SM> thisRaceParticipants_SM = (from rp in thisRaceParticipants
                                     select new Participant_SM()
                                     {
                                         ParticipantId = rp.ParticipantId,
                                         ParticipantName = rp.ParticipantName,
                                         Numberplate = rp.Numberplate,
                                         Approved = rp.Approved,
                                         ParticipantFinished = rp.ParticipantFinished,
                                         RaceId = raceId
                                     }).ToList();

                allRacesParticipants.Add(raceId, thisRaceParticipants_SM);
            }

            allRaceData = (from rd in raceData
                                 select new Race_SM()
                                 {
                                     RaceId = rd.Id,
                                     RaceTrackId = rd.RaceTrackId,
                                     RequiredLaps = rd.RequiredLaps,
                                     Spots = rd.SpotLimit,
                                     StartTime = rd.StartTime,
                                     EndTime = rd.EndTime,
                                     RaceName = rd.RaceName,
                                     RaceStatus = (RaceStatus)rd.RaceStatusEnumValue,
                                     Participants = allRacesParticipants[rd.Id]
                                 }).ToList();

            activeRace = allRaceData.Where((x => x.StartTime.Ticks - DateTime.Now.Ticks < TimeSpan.TicksPerHour && (DateTime.Now < x.EndTime))).FirstOrDefault();

            if (activeRace != null && activeRace.StartTime > DateTime.Now.AddHours(-1))
            {
                raceIsActive = true;
            }
        }

        /// <summary>
        ///  Gets track data on start-up
        /// </summary>
        private void InitialiseLapData()
        {
            allLapData =  (from ld in _thisRaceService_DAL.GetAllLapData()
                          select new Lap_SM()
                          {
                              ParticipantId = ld.UserId,
                              RaceId = ld.RaceId,
                              LapNumber = ld.LapNumber,
                              TimeCrossed = ld.TimeCrossed
                          }).ToList();

        }



        #endregion

        #region Private initialise geofence and segment published data

        #endregion

        #region All Race Participants Data Subscribe, call delegates and unsubscribe methods

        /// <summary>
        /// Register a delegate to receive all changes to the list of vehicle ids
        /// </summary>
        /// <param name="newAllVehicleIdsChangedHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns>The subscriber id associated with this registration</returns>
        public int AddSubscriber_RaceParticipantDataChanged(RaceParticipantDataChangedDelegate newHandler)
        {
            int subscriberId = ThreadSafeAddToSubscriberDictionary<RaceParticipantDataChangedDelegate>(_allRaceParticipantDataChangedDelegates, newHandler);

            Task.Run(() => { newHandler.Invoke(allRacesParticipantsData); });

            return subscriberId;
        }

        /// <summary>
        /// Remove specified subscriber to AllVehicleIdsChanged
        /// </summary>
        /// <param name="subscriberId">The subscriber id associated with this registration</param>
        public void RemoveSubscriber_RaceParticipantChanged(int subscriberId)
        {
            ThreadSafeRemoveFromSubscriberDictionary<RaceParticipantDataChangedDelegate>(_allRaceParticipantDataChangedDelegates, subscriberId);
        }


        /// <summary>
        /// Inform all subscribers of a change
        /// </summary>
        private void SendToAllRaceParticipantDataChangedDelegates()
        {
            lock (_allRaceParticipantDataChangedDelegates)
            {
                foreach (int subscriberId in _allRaceParticipantDataChangedDelegates.Keys)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            _allRaceParticipantDataChangedDelegates[subscriberId].Invoke(allRacesParticipantsData);
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


        #region Track data Subscribe, call delegates and unsubscribe methods

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


        #region Pending Race Subscribe, call delegates and unsubscribe methods

        /// <summary>
        /// Register a delegate to receive all changes to the list of vehicle ids
        /// </summary>
        /// <param name="newAllVehicleIdsChangedHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns>The subscriber id associated with this registration</returns>
        public int AddSubscriber_PendingRaceDataChanged(PendingRaceDataChangeDelegate newHandler)
        {
            int subscriberId = ThreadSafeAddToSubscriberDictionary<PendingRaceDataChangeDelegate>(_allPendingRaceDataChangedDelegates, newHandler);

            Task.Run(() => { newHandler.Invoke(allRaceData.Where(x => x.StartTime > DateTime.Now && (x.StartTime.Ticks - DateTime.Now.Ticks > TimeSpan.TicksPerHour)).ToList()); });

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

        #region Active Race (Lap) Data change Subscribe, call delegates and unsubscribe methods

        /// <summary>
        /// Register a delegate to receive all changes to the list of vehicle ids
        /// </summary>
        /// <param name="newAllVehicleIdsChangedHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns>The subscriber id associated with this registration</returns>
        public int AddSubscriber_ActiveRaceLapDataChanged(ActiveRaceLapDataChangeDelegate newHandler)
        {
            int subscriberId = ThreadSafeAddToSubscriberDictionary<ActiveRaceLapDataChangeDelegate>(_allActiveRaceLapDataChangeDelegates, newHandler);

            Task.Run(() => { newHandler.Invoke(allLapData.Where(x => x.RaceId == activeRace.RaceId).ToList()); });

            return subscriberId;
        }

        /// <summary>
        /// Remove specified subscriber to AllVehicleIdsChanged
        /// </summary>
        /// <param name="subscriberId">The subscriber id associated with this registration</param>
        public void RemoveSubscriber_ActiveRaceLapDataChanged(int subscriberId)
        {
            ThreadSafeRemoveFromSubscriberDictionary<ActiveRaceLapDataChangeDelegate>(_allActiveRaceLapDataChangeDelegates, subscriberId);
        }


        /// <summary>
        /// Inform all subscribers of a change
        /// </summary>
        private void SendToAllActiveRaceLapDataChangedDelegates()
        {
            lock (_allActiveRaceLapDataChangeDelegates)
            {
                foreach (int subscriberId in _allActiveRaceLapDataChangeDelegates.Keys)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            _allActiveRaceLapDataChangeDelegates[subscriberId].Invoke(allLapData.Where(x => x.RaceId == activeRace.RaceId).ToList());
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

        #region Active Race (Build-Up) Data change subscribe, call delegates, and unsubscribe method

                /// <summary>
        /// Register a delegate to receive all changes to the list of vehicle ids
        /// </summary>
        /// <param name="newAllVehicleIdsChangedHandler">Delegate that will get called on Add and on all changes until unsubscribed</param>
        /// <returns>The subscriber id associated with this registration</returns>
        public int AddSubscriber_ActiveRaceBuildUpDataChanged(ActiveRaceBuildUpDataChangeDelegate newHandler)
        {
            int subscriberId = ThreadSafeAddToSubscriberDictionary<ActiveRaceBuildUpDataChangeDelegate>(_allActiveRaceBuildUpDataChangeDelegates, newHandler);

            Task.Run(() => { newHandler.Invoke(activeRace); });

            return subscriberId;
        }

        /// <summary>
        /// Remove specified subscriber to AllVehicleIdsChanged
        /// </summary>
        /// <param name="subscriberId">The subscriber id associated with this registration</param>
        public void RemoveSubscriber_ActiveRaceBuildUpDataChanged(int subscriberId)
        {
            ThreadSafeRemoveFromSubscriberDictionary<ActiveRaceBuildUpDataChangeDelegate>(_allActiveRaceBuildUpDataChangeDelegates, subscriberId);
        }


        /// <summary>
        /// Inform all subscribers of a change
        /// </summary>
        private void SendToAllActiveRaceBuildUpDataChangedDelegates()
        {
            lock (_allActiveRaceBuildUpDataChangeDelegates)
            {
                foreach (int subscriberId in _allActiveRaceBuildUpDataChangeDelegates.Keys)
                {
                    Task.Run(() =>
                    {
                        try
                        {
                            _allActiveRaceBuildUpDataChangeDelegates[subscriberId].Invoke(activeRace);
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

        #region

        public Race_SM GetActiveRace()
        {
            Race_SM activeRace = new Race_SM();
            activeRace = allRaceData.Where((x => x.StartTime.Ticks - DateTime.Now.Ticks < TimeSpan.TicksPerHour && (DateTime.Now < x.EndTime))).FirstOrDefault();

            return activeRace;
        }

        /// <summary>
        /// Gets list of races that user could register for.
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        public List<Race_SM> GetEligibleRaces(string participantId)
        {
            List<Race_SM> eligibleRaces = allRaceData.Where(x => x.StartTime > DateTime.Now.AddHours(1) && !x.Participants.Select(p => p.ParticipantId).ToList().Contains(participantId)).ToList();

            return eligibleRaces;
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
                InitialiseTrackData();

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

        public bool ProcessParticipantAwaitingRegistration(string participantId, int raceId, bool approveRacer)
        {
            bool blnSuccess = false;

            blnSuccess = _thisRaceService_DAL.ProcessParticipantAwaitingRegistration(participantId, raceId, approveRacer);

            if (blnSuccess)
            {
                // If accepted, want to create a default entry into lap table for first lap
                // ------------------------------------------------------------------------
                if (approveRacer)
                {
                    _thisRaceService_DAL.AddStartingLapForParticipant(participantId, raceId);
                }


                InitialiseRaceData();
                SendToAllPendingRaceDataChangedDelegates();
            }


            return blnSuccess;
        }
        
        public bool RegisterParticipantForRace(string numberPlate, string participantId, int raceId)
        {
            // check if numberplate alr\already exists for that race

            bool blnSuccess = false;

            blnSuccess = _thisRaceService_DAL.RegisterParticipantForRace(participantId, raceId, numberPlate);


            if (blnSuccess)
            {
                InitialiseRaceData();
                SendToAllPendingRaceDataChangedDelegates();
            }

            return blnSuccess;

        }



        #endregion

        #region Utility

        public TimeSpan CalculateFastestLapForRacer(List<Lap_VM> lapData)
        {
           return lapData.OrderByDescending(x => x.LastLapTime).Select(x => x.LastLapTime).FirstOrDefault();
        }

        public TimeSpan CalculateLastLapTime(List<Lap_VM> lapData)
        {
           return lapData.OrderByDescending(x => x.LapNumber).Select(x => x.LastLapTime).FirstOrDefault();
        }



        //private string RemoveSpecialCharacters(this string str) 
        //{
        //   StringBuilder sb = new StringBuilder();
        //   foreach (char c in str) {
        //      if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_') {
        //         sb.Append(c);
        //      }
        //   }
        //   return sb.ToString();
        //}

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //YourCode
            // Check pending races for active race

            if (!raceIsActive)
            {
                activeRace = allRaceData.Where((x => x.StartTime.Ticks - DateTime.Now.Ticks < TimeSpan.TicksPerHour && (DateTime.Now < x.EndTime))).FirstOrDefault();
                
                if (activeRace != null)
                {
                    SendToAllPendingRaceDataChangedDelegates();
                    SendToAllActiveRaceBuildUpDataChangedDelegates();

                    raceIsActive = true;
                    // Send to all active race delegates
                }



            }
        }


        #endregion

    }
}
