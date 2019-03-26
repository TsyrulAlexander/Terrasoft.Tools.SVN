param(
    [string] $ProcessName,
    [string] $AppFolder,
    [string] $TempAppFolder
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
    Copy-Item (Join-Path $TempAppFolder "*") -Destination $AppFolder -Filter "*" -Force -Recurse
}

function ClearAppFolder {
    Remove-Item -LiteralPath $AppFolder -Force -Recurse -ErrorAction SilentlyContinue
}
cd $TempAppFolder
WaitingCloseProcess;
ClearAppFolder;
MoveDirectory;