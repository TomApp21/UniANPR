using ThreeSC.Net6Lib.BlazorTools.Interfaces;
using ThreeSC.Net6Lib.BlazorTools.Services;
using UniANPR.Models.ThreeSCBaseImplementations;
using ThreeSC.NetStandardLib.StandardTools.Interfaces;
using ThreeSC.NetStandardLib.StandardTools.Services;
using static UniANPR.Models.ThreeSCBaseImplementations.TestAppUserAuditEnum;

namespace UniANPR.Services
{
    public class TestService : ThreeSCServiceBase, ITestService
    {
        #region Private declarations

        /// <summary>
        /// The application logger
        /// </summary>
        private IThreeSCApplicationLogger _thisThreeSCApplicationLogger;

        /// <summary>
        /// The user audit logger
        /// </summary>
        private IThreeSCUserAuditLogger<_enmTestAppEnum> _thisThreeSCUserAuditLogger;

        /// <summary>
        /// The session manager
        /// </summary>
        private IThreeSCUserSessionManager<TestAppUser> _thisThreeSCUserSessionManager;

        #endregion

        #region Public enumerations

        public enum IdType
        {
            SegmentId,
            NodeId,
            TaskId
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new Task Service
        /// </summary>
        /// <param name="applicationLogger">The application logger to use for faults/debugs etc</param>
        /// <param name="configuration"></param>
        /// <param name="thisVehicleService">The vehicle service</param>
        public TestService(IThreeSCApplicationLogger applicationLogger, IThreeSCUserSessionManager<TestAppUser> thisThreeSCUserSessionManager, IThreeSCUserAuditLogger<_enmTestAppEnum> thisThreeSCUserAuditLogger)
        {
            _thisThreeSCApplicationLogger = applicationLogger;
            _thisThreeSCUserSessionManager = thisThreeSCUserSessionManager;
            _thisThreeSCUserAuditLogger = thisThreeSCUserAuditLogger;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create the localisation resources for a new task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskName"></param>
        /// <param name="description"></param>
        public void CreateLocalisedTask(int taskId, string taskName, string description)
        {
            _thisThreeSCUserSessionManager.CreateLocalisedIdEntry<IdType>(IdType.TaskId, taskId.ToString(), description, taskName);
            _thisThreeSCUserAuditLogger.LogUserAudit(_enmTestAppEnum.First, "TestTest", "Username@unknown.com");
        }
        #endregion
    }
}
