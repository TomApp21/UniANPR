using UniANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using UniANPR.Models.Shared;

namespace UniANPR.Components
{
    public partial class PopupCreateTrack
    {
        #region Delegate definitions

        /// <summary>
        /// Delegate called on confirmation button being pressed, handler defined in a parameter
        /// </summary>
        /// <param name="segments"></param>
        public delegate void OnTrackCreationConfirmedHandler();

        /// <summary>
        /// Delegate called on cancel button being pressed, handler defined in a parameter
        /// </summary>
        public delegate void OnTrackCreationCancelledHandler();

        #endregion

        #region Parameter declerations

        [Parameter] 
        public OnTrackCreationConfirmedHandler OnTrackCreationConfirmed { get; set; }

        [Parameter] 
        public OnTrackCreationCancelledHandler OnTrackCreationCancelled { get; set; }

        #endregion

        #region Declarations

        public Track_VM TrackData { get; set; }

        protected bool CreateTrackPopupVisible { get; set; }
        protected bool ConfirmBtnEnabled { get; set; }
        protected bool TrackIdExists { get; set; }

        protected List<string> allTracks { get; set; }

        public bool ValidSubmit = false;
        #endregion

        #region Intialization
        /// <summary>
        /// Initialise static data
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
            TrackData = new Track_VM();
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
        public async Task ShowCreateTrackForm()
        {
            CreateTrackPopupVisible = true;
            ConfirmBtnEnabled = false;
            TrackData = new Track_VM();
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
        /// Cancel the selected geofence and invoke handler in main component
        /// </summary>
        protected async void CancelEditing()
        {
            OnTrackCreationCancelled?.Invoke();
            CreateTrackPopupVisible = false;
            

            StateHasChanged();
        }


        async void HandleValidSubmit()
        {
            OnTrackCreationConfirmed?.Invoke();
            CreateTrackPopupVisible = false;

            StateHasChanged();
        }



        #endregion
    }
}
