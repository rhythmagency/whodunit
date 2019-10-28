REM clear existing code
rmdir /S /Q TestSite\App_Data\TEMP\ClientDependency
rmdir /S /Q TestSite\App_Data\TEMP\DistCache
rmdir /S /Q TestSite\App_Plugins\whodunit
REM copy in new code
mkdir TestSite\App_Plugins\whodunit
copy /Y Whodunit\content\App_Plugins\whodunit\* TestSite\App_Plugins\whodunit
copy /Y Whodunit\bin\Debug\Whodunit.* TestSite\bin
REM touch web.config to force app restart
copy /B TestSite\web.config+,,TestSite\web.config
pause