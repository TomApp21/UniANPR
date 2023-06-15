namespace UniANPR.Services
{
    public interface ITestService
    {
        /// <summary>
        /// Create the localisation resources for a new task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskName"></param>
        /// <param name="description"></param>
        void CreateLocalisedTask(int taskId, string taskName, string description);
    }
}
