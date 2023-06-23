namespace UniANPR.Services
{
     public abstract class ThreeSCServicebase
    {
        /// <summary>
        /// Add a new data change subsciber handler to the specified subscriber dictionary
        /// assigning the next free subscriber id key
        /// </summary>
        /// <typeparam name="TDelegate">The Type of the handler delegate</typeparam>
        /// <param name="subscriberDictionary">The dictionary of delegates to add new subscriber to</param>
        /// <param name="newDelegate">The new handler for this subscriber, of type TDelegate</param>
        /// <returns></returns>
        protected static int ThreadSafeAddToSubscriberDictionary<TDelegate>(Dictionary<int, TDelegate> subscriberDictionary, TDelegate newDelegate)
        {
            int newKey = 0;

            lock (subscriberDictionary)
            {
                int[] orderedKeys = subscriberDictionary.Keys.OrderBy(x => x).ToArray();
                int searchIndex = 0;
                newKey = (orderedKeys.Length == 0) ? 1 : -1;
                while (newKey == -1 && searchIndex < orderedKeys.Length)
                {
                    if (orderedKeys[searchIndex] != searchIndex + 1)
                    {
                        newKey = searchIndex + 1;
                    }
                    searchIndex++;
                }
                if (newKey == -1)
                {
                    newKey = searchIndex + 1;
                }
                subscriberDictionary.Add(newKey, newDelegate);
            }

            return newKey;
        }

        /// <summary>
        /// Remove a subscriber from the subsciber handler dictionary
        /// </summary>
        /// <typeparam name="TDelegate">The Type of the handler delegate</typeparam>
        /// <param name="subscriberDictionary">The dictionary of delegates to remove new subscriber from</param>
        /// <param name="removeId">The subscriber id to remove</param>
        protected static void ThreadSafeRemoveFromSubscriberDictionary<TDelegate>(Dictionary<int, TDelegate> subscriberDictionary, int removeId)
        {
            lock (subscriberDictionary)
            {
                if (subscriberDictionary.ContainsKey(removeId))
                {
                    subscriberDictionary.Remove(removeId);
                }
            }
        }

        /// <summary>
        /// Add a new data change subsciber handler to the specified subscriber dictionary
        /// assigning the next free subscriber id key
        /// for when there's a context for the subsciption (ie.g. only interested in certain type of data, or specific id etc)
        /// </summary>
        /// <typeparam name="TDelegate">The Type of the handler delegate</typeparam>
        /// <typeparam name="TContext">The Type of the context</typeparam>
        /// <param name="subscriberDictionary">The dictionary of delegates to add new subscriber to</param>
        /// <param name="newDelegate">The new handler for this subscriber, of type TDelegate</param>
        /// <param name="contextDictionary"></param>
        /// <param name="thisContext"></param>
        /// <returns>New subscriber Id (to use in call to <code>ThreadSafeRemoveFromSubscriberDictionaryWithContext</code>)</returns>
        protected static int ThreadSafeAddToSubscriberDictionaryWithContext<TDelegate, TContext>(Dictionary<int, TDelegate> subscriberDictionary, Dictionary<TContext, List<int>> contextDictionary, TContext thisContext, TDelegate newDelegate)
        {
            int newKey = 0;

            lock (subscriberDictionary)
            {
                int[] orderedKeys = subscriberDictionary.Keys.OrderBy(x => x).ToArray();
                int searchIndex = 0;
                newKey = (orderedKeys.Length == 0) ? 1 : -1;
                while (newKey == -1 && searchIndex < orderedKeys.Length)
                {
                    if (orderedKeys[searchIndex] != searchIndex + 1)
                    {
                        newKey = searchIndex + 1;
                    }
                    searchIndex++;
                }
                if (newKey == -1)
                {
                    newKey = searchIndex + 1;
                }
                subscriberDictionary.Add(newKey, newDelegate);
                contextDictionary[thisContext].Add(newKey);
            }

            return newKey;
        }

        /// <summary>
        /// Remove a data change subsciber handler from the specified subscriber dictionary
        /// for when there's a context for the subsciption (ie.g. only interested in certain type of data, or specific id etc)
        /// </summary>
        /// <typeparam name="TDelegate">The Type of the handler delegate</typeparam>
        /// <typeparam name="TContext">The Type of the context</typeparam>
        /// <param name="subscriberDictionary">The dictionary of delegates to add new subscriber to</param>
        /// <param name="contextDictionary"></param>
        /// <param name="removeId">Subscriber id (obtained in call to <code>ThreadSafeAddToSubscriberDictionaryWithContext</code> to remove</param>
        protected static void ThreadSafeRemoveFromSubscriberDictionaryWithContext<TDelegate, TContext>(Dictionary<int, TDelegate> subscriberDictionary, Dictionary<TContext, List<int>> contextDictionary, int removeId)
        {
            lock (subscriberDictionary)
            {
                if (subscriberDictionary.ContainsKey(removeId))
                {
                    subscriberDictionary.Remove(removeId);
                    foreach (TContext thisKey in contextDictionary.Keys)
                    {
                        if (contextDictionary[thisKey].Contains(removeId))
                        {
                            contextDictionary[thisKey].Remove(removeId);
                        }
                    }
                }
            }
        }
    }

}
