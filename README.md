# Whodunit
An Umbraco plugin which adds a changelog dashboard: https://our.umbraco.org/projects/backoffice-extensions/whodunit/

Here is the first thing you see when you open the "History Report" tab:

![Whodunit Tab](https://github.com/rhythmagency/whodunit/raw/master/src/assets/screenshots/report.png "Whodunit Tab")

Once you click "Generate Report", you will see a "Download Report" link:

![Whodunit Download](https://github.com/rhythmagency/whodunit/raw/master/src/assets/screenshots/download.png "Whodunit Download")

Once you click the download link, you can open the CSV file, which will look something like this:

![Whodunit CSV](https://github.com/rhythmagency/whodunit/raw/master/src/assets/screenshots/csv.png "Whodunit CSV")

Keep in mind there may be no entries to being with. Whodunit only tracks changes once it's been installed.

# Building the Umbraco Package
To build the Umbraco package, follow these steps:
* Build the solution in Visual Studio using the "Release" configuration.
* Run "grunt package".
* The Umbraco package is the ZIP file that gets created in the "dist" folder.

# Contributing
If you would like to contribute changes to Whodunit, here is how you can develop locally:
* Run "grunt" (the default task will copy the files to the test website).
* Rebuild the solution. This will run NuGet package restore.
* Run the test website. This will run the Umbraco installer.
* Once you have finished installing Umbraco, you should be able to log in and see the "History Report" dashboard.

Note that Whodunit was developed in Visual Studio 2015.