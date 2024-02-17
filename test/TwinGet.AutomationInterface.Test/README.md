# TwinGet.AutomationInterface.Test

When using [`TestProject`](\TestUtils\TestProject.cs), always make sure to use it before, for example, `AutomationInterface`, so that `AutomationInterface` dispose before `TestProject` does. This way, the solution is closed and `TestProject` can remove all the temporary files and folders.

This also apply to any other instances that access resources created by `TestProject`. They all need to stop prior to `TestProject` disposal, otherwise it won't be able to fully clean up temporary resources it created.
