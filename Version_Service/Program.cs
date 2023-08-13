using System;
using System.ServiceProcess;
using System.IO;
using System.Timers;
using System.Runtime.InteropServices;

public partial class FileCheckerService : ServiceBase
{
    // Import the function from the C++ DLL.
    [DllImport("FileCheckDLL.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int CheckFiles(string file1Path, string file2Path);

    // Declares a timer that will periodically trigger an event to execute the copy function.
    private Timer _timer;

    // Paths for the two files we're potentially copying and a destination path for copying.
    private string _file1Path = @"C:\Version Control Service\V1";
    private string _file2Path = @"C:\Version Control Service\V2";
    private string _destinationPath = @"C:\Version Control Service Build\Test Final";

    // Default constructor for the service.
    public FileCheckerService()
    {

    }

    // This method is called when the service starts.
    protected override void OnStart(string[] args)
    {
        // Initializes the timer to trigger every 60,000 ms.
        _timer = new Timer(60000);

        // When the timer elapses, the ExecuteCopy method is called.
        _timer.Elapsed += ExecuteCopy;

        // Start the timer.
        _timer.Start();
    }

    // This method is called when the service stops.
    protected override void OnStop()
    {
        // Stops the timer and releases its resources.
        _timer.Stop();
        _timer = null;
    }

    // This method will execute the copy function based on the decision from the C++ DLL.
    private void ExecuteCopy(object sender, ElapsedEventArgs e)
    {
        int result = CheckFiles(_file1Path, _file2Path);

        switch (result)
        {
            case 1: // File1 has "V2" and File2 has "V1"
                File.Copy(_file1Path, Path.Combine(_destinationPath, "File1.txt"), true);
                break;
            case 2: // File2 has "V2" and File1 has "V1"
                File.Copy(_file2Path, Path.Combine(_destinationPath, "File2.txt"), true);
                break;
        }
    }

    // Entry Point for app when not run as a service.
    static void Main()
    {
        ServiceBase[] ServiceToRun;
        ServiceToRun = new ServiceBase[]
        {
            new FileCheckerService()
        };
        ServiceBase.Run(ServiceToRun);
    }
}
