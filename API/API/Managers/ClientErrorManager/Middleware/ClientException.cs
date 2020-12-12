using System;

namespace API.Managers.CClientErrorManager.Middleware
{
    public class ClientException : Exception
    {
        public string Id { get; private set; }
        public object AdditionalInfo { get; private set; }
        public ClientException(string id, object additionalInfo = null) {
            Id = id;
            AdditionalInfo = additionalInfo;
        }
    }
}