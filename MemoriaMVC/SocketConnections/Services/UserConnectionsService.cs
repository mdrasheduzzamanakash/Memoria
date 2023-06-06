namespace MemoriaMVC.SocketConnections.Services
{
    public class UserConnectionsService
    {

        private readonly Dictionary<string, List<string>> _userConnectionsMap;

        public UserConnectionsService()
        {
            _userConnectionsMap = new Dictionary<string, List<string>>();
        }

        public void AddConnection(string userId, string connectionId)
        {
            if (!_userConnectionsMap.ContainsKey(userId))
            {
                _userConnectionsMap[userId] = new List<string>();
            }

            _userConnectionsMap[userId].Add(connectionId);
        }

        public void RemoveConnection(string userId, string connectionId)
        {
            if (_userConnectionsMap.ContainsKey(userId))
            {
                _userConnectionsMap[userId].Remove(connectionId);

                if (_userConnectionsMap[userId].Count == 0)
                {
                    _userConnectionsMap.Remove(userId);
                }
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            if (_userConnectionsMap.TryGetValue(userId, out var connections))
            {
                return connections;
            }

            return null;
        }
    }
}
