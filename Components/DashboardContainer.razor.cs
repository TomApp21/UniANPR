using DemoANPR.Models.Components;

namespace UniANPR.Components
{
  public partial class DashboardContainer
    {
        #region Declarations

        private PopupCreateRace _thisCreateRacePopupRef { get; set; }


        #endregion

        #region Base Component Overrides

        /// <summary>
        /// create a local resource manager for translations using the user's language and appropriate resource files
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
        }

        protected override void OnFirstRender()
        {
        }

        protected override void OnDispose()
        {
        }

        #endregion

        private void test()
        {

        }

        #region Event Handlers

        protected void AddNewRace()
        {
            _thisCreateRacePopupRef.ShowCreateRaceForm();



        }

        #endregion

        #region Event Callbacks

        private void HandleRaceCreationConfirmed()
        {
            Race_VM raceToCreate = _thisCreateRacePopupRef.RaceData;

            // Add To DAL

        }

        private void HandleRaceCreationCancelled()
        {

        }

        #endregion


    }
}
