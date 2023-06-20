using UniANPR.Interfaces;
using ThreeSC.Net6Lib.BlazorTools.Services;

namespace UniANPR.Services.Race
{
    public partial class RaceService : ThreeSCServiceBase, IRaceService, IHostedService
    {
        #region Constants
        #endregion

        #region Delegate Definitions
        #endregion

        #region Delegate & Subscriber Private Declarations
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




    }
}
