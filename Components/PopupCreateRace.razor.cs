using DemoANPR.Models.Components;
using Microsoft.AspNetCore.Components;
using UniANPR.Models.Shared;

namespace UniANPR.Components
{
    public partial class PopupCreateRace
    {
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
        public async Task ShowCreateRaceForm()
        {
            InitializeRaceTrackDdl();




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
            ValueTextModel geofenceTypeEntry;

            //GeofenceTypeDdlData = new List<ValueTextModel>();
            //foreach (GeofenceType geofenceType in Enum.GetValues(typeof(GeofenceType)))
            //{
            //    if (geofenceType != GeofenceType.Invalid)
            //    {
            //        if (!isEngineer && geofenceType == GeofenceType.Aoz)
            //        {
            //            // Don't allow user to select AOZ if in supervisor role
            //        }
            //        else
            //        {
            //            geofenceTypeEntry = new ValueTextModel()
            //            {
            //                DdlValueField = (int)geofenceType,
            //                DdlTextField = GetLocalisedStringFromEnum<GeofenceType>(geofenceType)
            //            };

            //            GeofenceTypeDdlData.Add(geofenceTypeEntry);
            //        }
            //    }
            //}
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
            RaceData = null;
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
