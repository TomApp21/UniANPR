using DemoANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using UniANPR.Interfaces;
using UniANPR.Models.Services;
using UniANPR.Models.Shared;

namespace UniANPR.Components
{
    public partial class PopupCreateRace
    {
        #region 
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

        public Race_VM RaceData { get; set; }

        protected bool CreateRacePopupVisible { get; set; }
        protected bool ConfirmBtnEnabled { get; set; }
        protected bool UserIdExists { get; set; }

        protected List<ValueTextModel> TrackDdlData { get; set; }

        private int _subscriberId = 0;

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
            RaceData = new Race_VM();

            _subscriberId = thisRaceService.AddSubscriber_TrackDataChanged(HandleNewTrackDataReceived);

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
            if (_subscriberId != 0)
            {
                thisRaceService.RemoveSubscriber_TrackDataChanged(_subscriberId);
            }
            
        }

        #endregion

        #region Track Data Changed Handler 

        private void HandleNewTrackDataReceived(List<Track_SM> newTrackData)
        {
            InvokeAsync(() =>
            {
                ValueTextModel trackDataEntry;

                TrackDdlData = new List<ValueTextModel>();

                foreach(Track_SM thisTrack in newTrackData)
                {
                    trackDataEntry = new ValueTextModel()
                    {
                        DdlValueField = thisTrack.TrackId,
                        DdlTextField = thisTrack.TrackName
                    };

                    TrackDdlData.Add(trackDataEntry);
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
        public async Task ShowCreateRaceForm()
        {
            //InitializeRaceTrackDdl();
            RaceData = new Race_VM();


            CreateRacePopupVisible = true;
            ConfirmBtnEnabled = false;

        }

        #endregion

        #region Initialise Drop Down

        /// <summary>
        /// Sets up static data for geofence type drop down list.
        /// </summary>
        private void InitializeRaceTrackDdl()
        {
            ValueTextModel trackDataEntry;

            TrackDdlData = new List<ValueTextModel>();

            foreach(Track_SM thisTrack in thisRaceService.allTrackData)
            {
                trackDataEntry = new ValueTextModel()
                {
                    DdlValueField = thisTrack.TrackId,
                    DdlTextField = thisTrack.TrackName
                };

                TrackDdlData.Add(trackDataEntry);
            }
        }

        #endregion

        #region Popup Actions Handler


        
        /// <summary>
        /// Formats pop-up when user changes geofence type 
        /// </summary>
        protected async void SelectedTrackTypeChanged()
        {
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

        /// <summary>
        /// Enables/Disables confirm button depending on the input values in the popup
        /// </summary>
        private void SpotsChanged()
        {
            //ConfirmBtnEnabled = (CurrentGeofenceDetails.SpeedLimit > 0 && (!String.IsNullOrEmpty(CurrentGeofenceDetails.UserGeofenceId)) && !UserIdExists) ? true : false;
        }

        ///// <summary>
        ///// Enables/Disables confirm button depending on the input values in the popup
        ///// </summary>
        private void LapsChanged()
        {
            //ConfirmBtnEnabled = (CurrentGeofenceDetails.SpeedLimit > 0 && (!String.IsNullOrEmpty(CurrentGeofenceDetails.UserGeofenceId)) && !UserIdExists) ? true : false;
        }



        /// <summary>
        /// Cancel the selected geofence and invoke handler in main component
        /// </summary>
        protected async void CancelEditing()
        {
            OnRaceCreationCancelled?.Invoke();
            CreateRacePopupVisible = false;
            RaceData = new Race_VM();
            StateHasChanged();
        }


        async void HandleValidSubmit()
        {
            OnRaceCreationConfirmed?.Invoke();
            CreateRacePopupVisible = false;

            StateHasChanged();
        }



        #endregion
    }
}
