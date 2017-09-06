# InternetOutageMonitor
A small Windows Service that logs when your computer has an internet connection.
## Installing
To install this service, you must have Visual Studio 2017 Community or later installed.
Please note that the service starts before the computer connects to a network, and will always report being initially disconnected from the internet.
### Building
1. Download or clone the repository locally into the Projects folder of Visual Studio 2017.
2. Open InternetOutageMonitor.sln in Visual Studio 2017.
3. Right click on the InternetOutageMonitor project in the Solution Explorer.
4. In the context menu, select Build.
### Adding InstallUtil to the Path
1. Ensure that the folder containing InstallUtil.exe is in the system path.
2. If it is not in the path, go to <b>Control Panel</b>><b>System and Security</b>><b>System</b> and click on <b>Advanced System Settings</b>.
3. Switch tabs to <b>Advanced</b> on the System Properties window that has opened, and press <b>Environment Variables...</b>
4. In the System Variables pane, highlight Path, then press Edit.
5. Press New and add <code>C:\Windows\Microsoft.NET\Framework\v4.0.30319</code> as the new entry.
### Installing InternetOutageMonitor Manually
1. Open the command prompt as an Administrator.
2. Navigate to the InternetOutageMonitor project folder.
3. Navigate to <code>InternetOutageMonitor\InternetOutageMonitor\bin\Debug</code>.
4. Enter <code>InstallUtil.exe .\InternetOutageMonitor.exe</code>.
5. If the installation is successful, restart your computer to start the service.
### Finding Connection Information
Internet connection status is logged in <code>C:\Windows\SysWOW64\OutageLog.txt</code>.
### Uninstalling
1. Open the command prompt as an Administrator.
2. Navigate to the location of <code>InternetOutageMonitor.exe</code> (see Installing InternetOutageMonitor Manually, steps 1-3).
3. Enter <code>InstallUtil.exe /u .\InternetOutageMonitor.exe</code>.
4. Restart your computer.
