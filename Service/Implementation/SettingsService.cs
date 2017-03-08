using Model;
using System.Data;
using Dapper.Contrib.Extensions;
using Dapper;
namespace Service
{
    class SettingsService : ISettingsService
    {
        IDbConnection Connection;
        public Settings Get()
        {
            return Connection.QueryFirst<Settings>("Select * from Settings");
        }

        public void Update(Settings settings)
        {
            Connection.Update(settings);
        }

        public SettingsService(IDbConnection connection)
        {
            Connection = connection;
        }
    }
}
