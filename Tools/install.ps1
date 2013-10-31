param($installPath, $toolsPath, $package, $project)

$project.ProjectItems.Item("libmp3lame.dll").Properties.Item("CopyToOutputDirectory").Value = 1