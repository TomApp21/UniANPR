using ThreeSC.Net6Lib.BlazorTools.Authorization;

namespace UniANPR.Pages
{
    [ThreeSCAuthorize(ThreeSCRole.UserAdmin)]
    public partial class UserManagement
    {
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
    }
}
