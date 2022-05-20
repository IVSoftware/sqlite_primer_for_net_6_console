// See https://aka.ms/new-console-template for more information

using SQLite;

var appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "sqlite_hello_world");
Directory.CreateDirectory(appData);
var connectionString = Path.Combine(appData, "MyDatabase.db");

using (var cnx = new SQLiteConnection(connectionString))
{
    // This is the command that configures your database using
    // your Record class and its public {get; set;} properties.
    cnx.CreateTable<Record>();

    // So make a new Record
    var newRecord = new Record { Description = "Hello" };

    // The guid field has been populated automatically.
    // Here we're saving it so we can do a query.
    var guidToFind = newRecord.guid;

    // This puts the new record into the database.
    cnx.Insert(newRecord);

    // Retrieve the record from the database.
    List<Record> recordset;
    recordset= cnx.Query<Record>($"SELECT * FROM items WHERE guid = '{guidToFind}'");

    Console.WriteLine($"Found: {recordset[0].Description}");

    // Change a value and update the database
    newRecord.Description = $"{recordset[0].Description} [Edited]";
    cnx.Update(newRecord);


    // Retrieve the record a second time from the database
    recordset = cnx.Query<Record>($"SELECT * FROM items WHERE guid = '{guidToFind}'");

    Console.WriteLine($"Found: {recordset[0].Description}");
}

Console.ReadKey();


// The custom record class that you create will configure the database automatically.
[Table("items")]
class Record
{
    [PrimaryKey]
    public string guid { get; set; } = Guid.NewGuid().ToString().Trim().TrimStart('{').TrimEnd('}');
    public string Description { get; set; } = string.Empty;
}