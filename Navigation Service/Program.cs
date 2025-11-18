
using Navigation_Service;

NavigationManager navigationManager = new NavigationManager();
navigationManager.run();


// Timer Example
//using System.Timers;

// static void OnTimedEvent(object? sender, ElapsedEventArgs e)
//{
//    System.Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
//}

//System.Timers.Timer myTimer = new System.Timers.Timer(1000);
//myTimer.Elapsed += OnTimedEvent;
//myTimer.Enabled = true;
//Console.WriteLine("Timer is running. Press Enter to stop and exit...");
//Console.ReadLine();