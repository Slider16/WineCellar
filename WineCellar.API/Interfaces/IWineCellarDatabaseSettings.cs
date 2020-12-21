namespace WineCellar.API.Interfaces
{
    public interface IWineCellarDatabaseSettings
    {
        string WinesCollectionName { get; set; }             
        string VendorsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        
    }
}
