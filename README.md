# TwinGet

A package manager for TwinCAT libraries

## Self-hosted runners

This project CI pipeline uses self-hosted runner. Following are the required software and settings for the build jobs to succeed:

* The runner must be configured with these tags `self-hosted`, `Windows`, `X64`, `TwinCAT`
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
* [GitHub runner](https://github.com/actions/runner/releases)
* A service account for running GitHub runner as a service
* The service account must be added to the "Administrators" group, for example, using the following command,
  
  ```powershell
  Add-LocalGroupMember -Group "Administrators" -Member <windows_account_name>
  ```
