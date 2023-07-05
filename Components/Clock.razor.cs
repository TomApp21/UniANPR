using ThreeSC.Net6Lib.BlazorTools.Components;
using ThreeSC.NetStandardLib.StandardTools.Interfaces;

namespace UniANPR.Components
{
    public partial class Clock
    {
        #region Declerations
        /// <summary>
        /// Current localized time
        /// </summary>
        protected string CurrentTime { get; set; } = String.Empty;

        protected bool _abort { get; set; }

        private Thread _ClockUpdateThread;
        #endregion

        #region Intialization
        /// <summary>
        /// Initialise static data
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
            _abort = false;
            _ClockUpdateThread = new Thread(RunClock);
            _ClockUpdateThread.Start();
        }

        /// <summary>
        /// Do initialisation of the page after initial user render
        /// (get first real data, attach to events etc)
        /// </summary>
        protected override void OnFirstRender()
        {
        }

        /// <summary>
        /// On component being closed, unsubscribe from the service's change event
        /// </summary>
        protected override void OnDispose()
        {
            _abort = true;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Update the clock on the main page
        /// </summary>
        private async void RunClock()
        {
            try
            {
                while (!_abort)
                {
                    if (!_abort)
                    {
                        await InvokeAsync(() => {
                            CurrentTime = DateTime.Now.ToShortTimeString();
                            StateHasChanged();
                        });

                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                thisThreeSCApplicationLogger.LogUnexpectedException(enmUniqueueLogCode.NotApplicable, string.Empty, ex, CurrentUserId);
            }
        }
        #endregion
    }

}
