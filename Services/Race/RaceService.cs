using UniANPR.Interfaces;
using ThreeSC.Net6Lib.BlazorTools.Services;
using UniANPR.Models.Services;

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

        #endregion

        #region Delegate & Subscriber Private Declarations

        private Dictionary<int, TrackDataChangeDelegate> _allTrackDataChangedDelegates; 
        #endregion

        #region Declarations
        private List<Track_SM> _allTrackNameData { get; set; }

        private RaceService_DAL _thisRaceService_DAL;


        #endregion

        #region Properties
        #endregion

        #region Constructor
        #endregion

        #region Hosted service start stop implementations

        /// <summary>
        /// On being started by the webApp service, read all current data from the database
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            //InitialisePublishedGeofenceData();
            InitialiseHistoricRaceData();

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

        #region Private initialise geofence and segment published data
        /// <summary>
        /// Inityialise published geofence data for GUI and for vehicles
        /// on startup or on re-publish
        /// </summary>
        private void InitialisePublishedGeofenceData()
        {
            //List<Geofence_DM> geofenceData = _thisSiteDetailsDAL.GetAllGeofence(true);

            //_publishedSiteConfigurationDetails.GeofenceDict = (from gd in geofenceData
            //                                                   select new Geofence()
            //                                                   {
            //                                                       UserGeofenceId = gd.UserGeofenceId,
            //                                                       VehicleGeofenceId = gd.VehicleGeofenceId,
            //                                                       IsArchived = gd.IsArchived,
            //                                                       IsContained = gd.IsContained,
            //                                                       SegmentClash = gd.SegmentClash,
            //                                                       SpeedLimit = gd.SpeedLimit,
            //                                                       WellKnownBinary = gd.WellKnownBinary,
            //                                                       GeofenceType = (GeofenceType)gd.GeofenceType,
            //                                                   }).ToDictionary(key => key.UserGeofenceId);




        }

        /// <summary>
        /// Initialise published segment data for GUI and for vehicles
        /// on startup or on re-publish
        /// </summary>
        private void InitialiseHistoricRaceData()
        {
            //List<Segment_DM> segmentData = _thisSiteDetailsDAL.GetAllSegments(true);

            //_publishedSiteConfigurationDetails.SegmentDict = (from sd in segmentData
            //                                                  orderby sd.Id
            //                                                  select new Segment()
            //                                                  {
            //                                                      UserSegmentId = sd.UserSegmentId,
            //                                                      VehicleSegmentId = sd.VehicleSegmentId,
            //                                                      SegmentType = (SegmentType)sd.SegmentType,
            //                                                      LeftWidth = sd.LeftWidth,
            //                                                      RightWidth = sd.RightWidth,
            //                                                      SpeedLimit = sd.SpeedLimit,
            //                                                      IsArchived = sd.IsArchived,
            //                                                      PublishTimeUTC = sd.PublishTimeUTC,
            //                                                      WellKnownBinary = sd.WellKnownBinary,
            //                                                  }).ToDictionary(k => k.UserSegmentId);



        }
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

            Task.Run(() => { newHandler.Invoke(_allTrackNameData.ToList()); });

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
                            _allTrackDataChangedDelegates[subscriberId].Invoke(_allTrackNameData.ToList());
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
            return _allTrackNameData.Select(x => x.TrackName).Contains(newTrackName) ? true : false;
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


        #endregion


    }
}
