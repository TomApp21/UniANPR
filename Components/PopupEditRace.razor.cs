using DemoANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using UniANPR.Interfaces;
using UniANPR.Models.Components;
using UniANPR.Models.Services;
using UniANPR.Models.Shared;

using ThreeSC.NetStandardLib.StandardTools.Interfaces;
using Telerik.Blazor.Components;
using Telerik.Blazor;
using DemoANPR.Models.Services;
using UniANPR.Services.Race;

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
        TelerikNotification NotificationReference { get; set; }




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

            _subscriberId = thisRaceService.AddSubscriber_PendingRaceDataChanged(HandleNewPendingRaceDataReceived);
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
        { 
            if (_subscriberId != 0 )
            {
                thisRaceService.RemoveSubscriber_PendingRaceDataChanged(_subscriberId);
            }
        }

        #endregion
        


        #region Handle Pending Race Data Changed

        /// <summary>
        /// Published site data has changed (or set on first subscription)
        /// Create the map and layer data from teh published site details
        /// </summary>
        /// <param name="siteConfigurationData"></param>
        private void HandleNewPendingRaceDataReceived(List<Race_SM> pendingRaceData)
        {
            InvokeAsync(() =>
            {
                if (pendingRaceData != null)
                {
                    if (_thisRaceDetails != null)
                    {
                        List<Participant_SM> raceParticipants = pendingRaceData.Where(x => x.RaceId == _thisRaceDetails.RaceId).Select(x => x.Participants).FirstOrDefault();

                        _raceParticipantData = ConvertParticipantServiceToView(raceParticipants);

                        _participantsAwaitingApproval = _raceParticipantData.Where(x => x.Approved == null).ToList();
                        _registeredParticipants = _raceParticipantData.Where(x => x.Approved == true).ToList();

                    }
                }

                StateHasChanged();
            });
        }

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

            _thisRaceDetails = thisRace;

            if (_thisRaceDetails.RaceParticipants == null)
            {
                _thisRaceDetails.RaceParticipants = new List<Participant_VM>();
            }

            _participantsAwaitingApproval = _thisRaceDetails.RaceParticipants.Where(x => x.Approved == null).ToList();
            _registeredParticipants = _thisRaceDetails.RaceParticipants.Where(x => x.Approved == true).ToList();

            //thisThreeSCUserSessionReader.DisplayNameForUserId(CurrentUserId);

            ConfirmBtnEnabled = false;

            EditRacePopupVisible = true;

            StateHasChanged();
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
            bool blnSuccess = false;

            blnSuccess = thisRaceService.ProcessParticipantAwaitingRegistration(participantId, _thisRaceDetails.RaceId, true);

            if (blnSuccess)
            {
                ShowNotification("Participant registered successfully.", true);
                // Show Notification
            }
            else
            {
                 ShowNotification("Participant registration failed.", false);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="participantId"></param>
        private void DenyParticipant(string participantId)
        {
            bool blnSuccess = false;

            blnSuccess = thisRaceService.ProcessParticipantAwaitingRegistration(participantId, _thisRaceDetails.RaceId, false);

            if (blnSuccess)
            {
                ShowNotification("Participant denied successfully.", true);
                // Show Notification
            }
            else
            {
                 ShowNotification("Participant deny failed.", false);
            }

        }

        #endregion

        private void ShowNotification(string notificationText, bool successTheme)
        {
            NotificationReference.Show(new NotificationModel()
            {
                Text = GetLocalisedString(notificationText),
                ThemeColor = successTheme ? Telerik.Blazor.ThemeConstants.Notification.ThemeColor.Success : ThemeConstants.Notification.ThemeColor.Error,
                ShowIcon = true,
                CloseAfter = 3000
            });
        }

        private List<Participant_VM> ConvertParticipantServiceToView(List<Participant_SM> participants)
        {
            List<Participant_VM> raceParticipants = new List<Participant_VM>();
            
            raceParticipants = (from p in participants
                        select new Participant_VM()
                        {
                            ParticipantId = p.ParticipantId,
                            RaceId = p.RaceId,
                            Approved = p.Approved,
                            Numberplate = p.Numberplate,
                            ParticipantName = thisThreeSCUserSessionReader.DisplayNameForUserId(p.ParticipantId),
                            ParticipantFinished = p.ParticipantFinished
                        }).ToList();

            return raceParticipants;
        }
    }
}
