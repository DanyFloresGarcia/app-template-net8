namespace Domain.ValueObjects;
public partial record AuditRecord
{
    public AuditRecord(string userCreator, string hostCreator, string appCreator)
    {
        UserCreator = userCreator;
        HostCreator = hostCreator;
        AppCreator = appCreator;
    }

    private AuditRecord()
    {

    }

	public bool Asset { get; init; } = true;
    public string UserCreator { get; init; } = string.Empty;
    public string UserUpdater { get; set; } = string.Empty;
    public string HostCreator { get; init; } = string.Empty;
    public string HostUpdater { get; set; } = string.Empty;
    public DateTime DateCreated { get; init; } = DateTime.Now;
    public DateTime DateUpdate { get; set; } = DateTime.Now;
    public string AppCreator { get; init; } = string.Empty;
    public string AppUpdater { get; set; } = string.Empty;

    public static AuditRecord? Create(string userCreator, string hostCreator, string appCreator)
    {
        if (string.IsNullOrEmpty(userCreator) || string.IsNullOrEmpty(hostCreator) ||
            string.IsNullOrEmpty(appCreator))
        {
            return null;
        }

        return new AuditRecord(userCreator, hostCreator, appCreator);
    }
}