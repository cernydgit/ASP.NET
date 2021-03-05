namespace Catalog.Repositories
{
    public class MongoSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }

        private string m_ConnectionString;
        public string ConnectionString
        {
            get
            {
                return string.IsNullOrEmpty(m_ConnectionString) ? $"mongodb://{Host}:{Port}" : m_ConnectionString;
            }
            set
            {
                m_ConnectionString = value;
            }
        }
    }
}
