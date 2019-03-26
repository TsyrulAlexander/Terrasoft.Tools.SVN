param(
    [string] $ProcessName,
    [string] $AppFolder,
    [string] $TempAppFolder,
	[string] $NewVersionId
)

function WaitingCloseProcess {
	write-host "Please close app."
	$sec =0;
    while(Get-Process $ProcessName -ErrorAction SilentlyContinue) {		
        Start-Sleep -Milliseconds 1000;
		write-host "Waiting $sec sec."
		$sec=$sec+1;
    }
}

function MoveDirectory {
    Copy-Item -Path $TempAppFolder -Filter "*" -Destination $AppFolder -Force -Recurse -Container 
}

function ClearAppFolder {
    Remove-Item -LiteralPath $AppFolder -Force -Recurse -ErrorAction SilentlyContinue
}

function UpdateAppSetting($configPath, $settingKey, $settingValue) {
	$xml = New-Object xml;
	$xml.PreserveWhitespace = $true;
	$xml.Load($configPath);
	$latestVersionIdItem = $xml.SelectSingleNode("//appSettings/add[@key = '$settingKey']");
	$latestVersionIdItem.SetAttribute('value', $settingValue);
	$xml.Save($configPath);
}

function UpdateAppVersion {
	$configFileName = $ProcessName + ".exe.config";
	$configPath = Join-Path $AppFolder $configFileName;
	UpdateAppSetting $configPath "latestVersionId" $NewVersionId;
}

function ReopenApp {
	$appFileName = $ProcessName + ".exe";
	Start-Process -FilePath $appFileName -WorkingDirectory $AppFolder;
}

WaitingCloseProcess;
ClearAppFolder;
MoveDirectory;
UpdateAppVersion;
ReopenApp;