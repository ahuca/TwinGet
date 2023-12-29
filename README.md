[![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](https://github.com/ahuca/TwinGet/blob/main/LICENSE)
[![example workflow](https://github.com/ahuca/TwinGet/actions/workflows/CI.yml/badge.svg?branch=main)](https://github.com/ahuca/TwinGet/actions/workflows/CI.yml?query=branch%3Amain)

# TwinGet

A package manager for TwinCAT libraries

## Self-hosted runners

This project CI pipeline uses self-hosted runner. Following are the required software and settings for the build jobs to succeed:

* The runner must be configured with these tags `self-hosted`, `Windows`, `X64`, `TwinCAT`
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
* [GitHub runner](https://github.com/actions/runner/releases)
* A service account for running GitHub runner as a service
* The service account must be added to the group "Distributed COM Users", for example, using the following command,
  
  ```powershell
  Add-LocalGroupMember -Group "Distributed COM Users" -Member <windows_account_name>
  ```
