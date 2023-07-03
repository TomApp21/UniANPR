using DemoANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using UniANPR.Enum;
using UniANPR.Interfaces;
using UniANPR.Models.Services;
using UniANPR.Models.Shared;

namespace UniANPR.Components
{
    public partial class PopupRegisterForRace
    {
        #region 
        [Inject]
        IRaceService thisRaceService { get; set; }
        #endregion

        #region Delegate definitions

        /// <summary>
        /// Delegate called on confirmation button being pressed, handler defined in a parameter
        /// </summary>
        public delegate void OnRaceRegistrationConfirmedHandler(string numberplate, int raceId);

        /// <summary>
        /// Delegate called on cancel button being pressed, handler defined in a parameter
        /// </summary>
        public delegate void OnRaceRegistrationCancelledHandler();

        #endregion

        #region Parameter declerations

        [Parameter] 
        public OnRaceRegistrationConfirmedHandler OnRaceRegistrationConfirmed { get; set; }

        [Parameter] 
        public OnRaceRegistrationCancelledHandler OnRaceRegistrationCancelled { get; set; }

        #endregion

        #region Declarations

        public List<Race_VM> EligibleRaces { get; set; }
        public Race_VM SelectedRace { get; set; }
        List<ValueTextModel> EligibleRaceDdlData { get; set; }
        protected bool RegisterForRacePopupVisible { get; set; }
        protected bool ConfirmBtnEnabled { get; set; }
        protected bool UserIdExists { get; set; }
        private int pendingRaceSubscriberId = 0;
        private int selectedRaceId = 0;

        protected string ParticipantNumberplate { get; set; }
        /// <summary>
        /// Sets minimum speed limit depending on the geofence type selected 
        /// to prevent user from assigning 0 speed limit when zone = SLZ
        /// </summary>
        protected int MinSpeedLimit { get; set; }

        public bool ValidSubmit = false;
        #endregion

        #region Intialization
        /// <summary>
        /// Initialise static data
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
            EligibleRaces = new List<Race_VM>();
            SelectedRace = new Race_VM();


        }

        /// <summary>
        /// Do initialisation of the page after initial user renderS
        /// (get first real data, attach to events etc)
        /// </summary>
        protected override void OnFirstRender()
        { }

        /// <summary>
        /// On component being closed, unsubscribe from the service's change event
        /// </summary>
        protected override void OnDispose()
        { }

        #endregion

        #region Show Popup Handler

        /// <summary>
        /// Shows create geofence popup and initialises geofence and existing geofence keys
        /// </summary>
        /// <param name="newGeofence">Geofence model with WKB populated</param>
        /// <param name="existingGeofenceKeys">List of existing geofence keys used for validating newly entered user id</param>
        /// <returns></returns>
        public async Task ShowRaceRegistrationForm()
        {
            EligibleRaces = (from rd in thisRaceService.GetEligibleRaces(CurrentUserId)
                             select new Race_VM()
                             {
                                 RaceId = rd.RaceId,
                                 RaceTrackId = rd.RaceTrackId,
                                 RaceTrackName = thisRaceService.allTrackData.Where(x => x.TrackId == rd.RaceTrackId).FirstOrDefault().TrackName,
                                 RequiredLaps = rd.RequiredLaps,
                                 Spots = rd.Spots,
                                 StartTime = rd.StartTime,
                                 EndTime = rd.EndTime,
                                 RaceName = rd.RaceName,
                                 RaceStatus = (RaceStatus)rd.RaceStatus,
                                 RegisteredParticipants = rd.ActiveParticipants,
                             }).ToList();


            InitializeEligibleRaceDdl();
            RegisterForRacePopupVisible = true;
            ConfirmBtnEnabled = false;

            StateHasChanged();
        }

        #endregion

        #region Initialise Drop Down

        /// <summary>
        /// Sets up static data for geofence type drop down list.
        /// </summary>
        private void InitializeEligibleRaceDdl()
        {
            ValueTextModel trackDataEntry;

            EligibleRaceDdlData = new List<ValueTextModel>();

            foreach(Race_VM thisRace in EligibleRaces)
            {
                trackDataEntry = new ValueTextModel()
                {
                    DdlValueField = thisRace.RaceId,
                    DdlTextField = thisRace.RaceName
                };

                EligibleRaceDdlData.Add(trackDataEntry);
            }
        }

        #endregion

        #region Popup Actions Handler


        
        /// <summary>
        /// Formats pop-up when user changes geofence type 
        /// </summary>
        protected async void SelectedRaceChanged()
        {
            ParticipantNumberplate = string.Empty;
            SelectedRace = EligibleRaces.Where(x => x.RaceId == selectedRaceId).FirstOrDefault();
            StateHasChanged();

            //if (CurrentGeofenceDetails.GeofenceType == GeofenceType.Invalid)
            //{
            //    ConfirmBtnEnabled = false;
            //}
            //else
            //{
            //    MinSpeedLimit = (CurrentGeofenceDetails.GeofenceType == GeofenceType.SpeedLimitZone) ? 1 : 0;
            //    CurrentGeofenceDetails.SpeedLimit = (CurrentGeofenceDetails.GeofenceType == GeofenceType.SpeedLimitZone) ? 1 : 0;
            //    ConfirmBtnEnabled = (CurrentGeofenceDetails.GeofenceType == GeofenceType.SpeedLimitZone || String.IsNullOrEmpty(CurrentGeofenceDetails.UserGeofenceId) || UserIdExists ) ? false : true;
            //    StateHasChanged();
            //}
        }

        protected async void RegisterForRace()
        {
            OnRaceRegistrationConfirmed?.Invoke(ParticipantNumberplate, selectedRaceId);
            RegisterForRacePopupVisible = false;
            StateHasChanged();
        }



        /// <summary>
        /// Cancel the selected geofence and invoke handler in main component
        /// </summary>
        protected async void CancelEditing()
        {
            OnRaceRegistrationCancelled?.Invoke();
            RegisterForRacePopupVisible = false;
            EligibleRaces = new List<Race_VM>();
            StateHasChanged();
        }

        #endregion
    }
}
