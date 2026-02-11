using Navigation_Service;
using Serilog;

ILogger logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();


var gpsSource = new UdpSource(Constants.GNSS_PORT,logger);
var nmeaParser = new Navigation_Service.NmeaParser(gpsSource,logger);
var gpsDevice = new GNSSDevice(nmeaParser,logger);




//gpsSource.Start();
//gpsDevice.ConnectSource(gpsSource);

var devices = new List<INavigationDevice> { gpsDevice };

NavigationManager navigationManager = new NavigationManager(logger, devices);
navigationManager.run();

