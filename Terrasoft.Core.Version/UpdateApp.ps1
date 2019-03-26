param(
    [string] $ProcessName,
    [string] $AppFolder,
    [string] $NewAppFolder
)

function WaitingCloseProcess {
    while(Get-Process $ProcessName) {
        Start-Sleep -Milliseconds 100;
    }
}

function MoveDirectory {
    Copy-Item -Force -Recurse -Verbose -Path ($NewAppFolder + "\*") -Destination $AppFolder;
}

function ClearAppFolder {
    Remove-Item -Recurse -Confirm:$false -force -Path ($AppFolder + "\*") -Exclude "UpdateApp.ps1";
}

WaitingCloseProcess;
Read-Host -Prompt "WaitingCloseProcess complite";
ClearAppFolder;
Read-Host -Prompt "ClearAppFolder complite";
MoveDirectory;
Read-Host -Prompt "Press Enter to exit";
Write-Host "Finish";
