using UniANPR.Components;

namespace UniANPR.Pages
{
    public partial class Dashboard
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

        }

        private void HandleRaceCreationCancelled()
        {

        }

        #endregion


    }
}
