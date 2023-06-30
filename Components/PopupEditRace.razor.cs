using DemoANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using UniANPR.Interfaces;
using UniANPR.Models.Components;
using UniANPR.Models.Services;
using UniANPR.Models.Shared;

using ThreeSC.NetStandardLib.StandardTools.Interfaces;

namespace UniANPR.Components
{
    public partial class PopupEditRace
    {
        #region Injections
        [Inject]
        IRaceService thisRaceService { get; set; }
        #endregion

        #region Delegate definitions

        /// <summary>
        /// Delegate called on confirmation button being pressed, handler defined in a parameter
        /// </summary>
        /// <param name="segments"></param>
        public delegate void OnRaceCreationConfirmedHandler();

        /// <summary>
        /// Delegate called on cancel button being pressed, handler defined in a parameter
        /// </summary>
        public delegate void OnRaceCreationCancelledHandler();

        #endregion

        #region Parameter declerations

        [Parameter] 
        public OnRaceCreationConfirmedHandler OnRaceCreationConfirmed { get; set; }

        [Parameter] 
        public OnRaceCreationCancelledHandler OnRaceCreationCancelled { get; set; }

        #endregion

        #region Declarations
        private int _subscriberId = 0;
        private List<Participant_VM> _raceParticipantData { get; set; }
        private List<Participant_VM> _participantsAwaitingApproval { get; set; }
        private List<Participant_VM> _registeredParticipants { get; set; }
        private Race_VM _thisRaceDetails { get; set; }
        protected bool EditRacePopupVisible { get; set; }
        protected bool ConfirmBtnEnabled { get; set; }
        protected bool UserIdExists { get; set; }

        #endregion

        #region Intialization
        /// <summary>
        /// Initialise static data
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
            _raceParticipantData = new List<Participant_VM>();
            _participantsAwaitingApproval = new List<Participant_VM>();
            _registeredParticipants = new List<Participant_VM>();
            //RaceData = new Race_VM();
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
        public async Task ShowEditRaceForm(Race_VM thisRace)
        {
            EditRacePopupVisible = true;

            _thisRaceDetails = thisRace;

            _participantsAwaitingApproval = _thisRaceDetails.RaceParticipants.Where(x => x.Approved == null).ToList();
            _registeredParticipants = _thisRaceDetails.RaceParticipants.Where(x => x.Approved == true).ToList();

            //thisThreeSCUserSessionReader.DisplayNameForUserId(CurrentUserId);

            ConfirmBtnEnabled = false;
        }

        #endregion



        #region Initialise Drop Down

        /// <summary>
        /// Sets up static data for geofence type drop down list.
        /// </summary>
        private void InitializeRaceTrackDdl()
        {
            //ValueTextModel trackDataEntry;

            //TrackDdlData = new List<ValueTextModel>();

            //foreach(Track_SM thisTrack in thisRaceService.allTrackData)
            //{
            //    trackDataEntry = new ValueTextModel()
            //    {
            //        DdlValueField = thisTrack.TrackId,
            //        DdlTextField = thisTrack.TrackName
            //    };

            //    TrackDdlData.Add(trackDataEntry);
            //}
        }

        #endregion

        #region Popup Actions Handler


        
    



        #endregion

        #region Event Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantId"></param>
        private void ApproveParticipant(string participantId)
        {



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantId"></param>
        private void DenyParticipant(string participantId)
        {



        }

        #endregion

    }
}
