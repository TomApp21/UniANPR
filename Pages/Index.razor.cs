using ThreeSC.NetStandardLib.StandardTools.Models;

using ThreeSC.Net6Lib.BlazorTools.Authorization;
using ThreeSC.Net6Lib.BlazorTools.Components.VersionHistory;
using ThreeSC.Net6Lib.BlazorTools.Components.UserManagement;
using UniANPR.Models.ThreeSCBaseImplementations;
using Microsoft.AspNetCore.Components;
using ThreeSC.Net6Lib.BlazorTools.Interfaces;
using ThreeSC.Net6Lib.BlazorTools.Models.ServiceModels;
using static UniANPR.Services.TestService;
using UniANPR.Services;
using ThreeSC.Net6Lib.BlazorTools.Components;
using ThreeSC.Net6Lib.BlazorTools.Models;

namespace UniANPR.Pages
{
    [ThreeSCAuthorize(ThreeSCRole.SuperAdmin, ThreeSCRole.User)]
    public partial class Index
    {
        #region Private constants
        private const string _c_PODUserGuidePath = @"documents\PortalUserGuide.pdf";
        #endregion

        /// <summary>
        /// The test service
        /// </summary>
        [Inject] private ITestService _thisTestService { get; set; }

        protected List<PossibleCultureDefinitionModel> _allCultures { get; set; }
        protected string selectedOption { get; set; }

        protected VersionHistoryPopUp thisVersionHistoryPopUp;
        protected ThreeSCUserPersonalProfilePopup<TestAppUser> thisThreeSCUserPersonalProfilePopup { get; set; }

        #region Help Declerations and methods
        /// <summary>
        /// Holds information for componenthelp popup
        /// </summary>
        protected HelpPopupDefinitionModel thisHelpPopupDefintion { get; set; }

        /// <summary>
        /// Ref to the help popup
        /// </summary>
        protected ThreeSCHelpButtonAndPopup thisHelpButtonPopup { get; set; }

        /// <summary>
        /// Show help popup
        /// </summary>
        protected void ShowHelpPopup()
        {
            // Show the window
            // ---------------
            thisHelpButtonPopup.ShowHelp();
        }
        #endregion

        #region Base Component Overrides

        /// <summary>
        /// create a local resource manager for translations using the user's language and appropriate resource files
        /// </summary>
        protected override void InitialiseComponentStaticData()
        {
            _allCultures = thisThreeSCUserSessionReader.GetPossibleCultureCodes();
            if (_allCultures.Count > 0)
            {
                selectedOption = _allCultures.FirstOrDefault().CultureDisplayName;
            }
            else
            {
                selectedOption = String.Empty;
            }

            // Set up the help popup details
            // -----------------------------
            thisHelpPopupDefintion = new HelpPopupDefinitionModel() { DocumentPath = _c_PODUserGuidePath, PageNumber = 3, PopupWindowString = "POD Current Shift" };
        }

        protected override void OnFirstRender()
        {
            string fred = GetLocalisedString("banana");
        }

        protected override void OnDispose()
        {
        }

        #endregion

        protected void ShowVersionWindow()
        {
            //int taskId = 7;

            //string taskIdString = FindLocalisedString(IdType.TaskId, "Task" + taskId.ToString());

            //_thisTestService.CreateLocalisedTask(taskId, "Task" + taskId.ToString(), "Task #" + taskId.ToString());

            //taskIdString = FindLocalisedString(IdType.TaskId, "Task" + taskId.ToString());

            thisVersionHistoryPopUp.Show();
        }

        protected void ShowEditPrefWindow()
        {
            thisThreeSCUserPersonalProfilePopup.ShowPopup(CurrentUserId);
        }

        protected void ShowUserGuidePopup()
        {
            // Show the window
            // ---------------
            thisHelpButtonPopup.ShowHelp();
        }
    }
}
