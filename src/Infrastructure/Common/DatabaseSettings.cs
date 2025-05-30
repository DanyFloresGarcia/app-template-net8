namespace Infrastructure.Common;

public class DatabaseSettings
{
    public string Provider { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int MaxConnections { get; set; } = 100;
    public string TrustServerCertificate { get; set; } = "True";
    public string ConnectionString
    {
        get
        {
            return Provider switch
            {
                DatabaseProvider.SqlServer => $"Server={Host},{Port};Database={Database};User Id={UserId};Password={Password};TrustServerCertificate={TrustServerCertificate};",
                DatabaseProvider.PostgreSql => $"Host={Host};Port={Port};Username={UserId};Password={Password};Database={Database};Pooling=true;Maximum Pool Size={MaxConnections};",
                DatabaseProvider.MongoDb => $"mongodb://${UserId}:${Password}@{Host}:{Port}",
                DatabaseProvider.Mysql => $"Server={Host};Port={Port};Database={Database};User={UserId};Password={Password};",
                DatabaseProvider.Oracle => $"User Id={UserId};Password={Password};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Host})(PORT={Port}))(CONNECT_DATA=(SERVICE_NAME={Database})))",
                _ => throw new NotSupportedException($"Database provider '{Provider}' is not supported."),
            };
        }
    }
}

