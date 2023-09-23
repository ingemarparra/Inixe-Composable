# Composable Application for WPF

A simple proof of concept application that implements a plug-in enabled WPF application with Autofac.

Even if the application depends on Autofac. It's still possible to use other IoC.

The main objective is to load dynamically assemblies and insert their XAML templates in the the existing application on demand.



# Table of Contents

* [Prerequisites](#prerequisites)
* [Features](#features)
* [Building from Source](#building-from-source)
* [License](#license)

# Features

* WPF with plugin framework based on Autofac
* Plugins can be downloaded dynamically depending on security token configuration.
* Plugins can be added from multiple locations.
* Core services and functionality can be shared from the host application into the plugins.


# Prerequisites

* .NET 6.0 SDK version 6 or above
* Visual Studio Code, Visual Studio 2019 or above
* Powershell 7.0 or above
* AWS CLI v2

# Building from Source

```pwsh
dotnet build Inixe.Composable.sln -c Release
```

# License

You can find the License terms [here](LICENSE)
