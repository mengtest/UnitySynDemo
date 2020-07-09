set dst=batDemoCopy2
set src=batDemo

if not exist %dst% (
	md %dst%
) 

mklink /d %cd%\%dst%\ProjectSettings %cd%\%src%\ProjectSettings
mklink /d %cd%\%dst%\Assets %cd%\%src%\Assets
mklink /d %cd%\%dst%\Bundle %cd%\%src%\Bundle

pause