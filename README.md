# TinyLittleMvvm

This is a small MVVM library I wrote, because I implemented the same stuff over and over
again in several projects.

I tried [Caliburn Micro](http://caliburnmicro.com/), [MVVM Light](http://www.mvvmlight.net/)
and others, but none of them met my demands satisfactory.

My requirements were:

- as small as possible, as big as necessary
- no magic strings in XAML code, just ordinary (and well supported)
  [WPF binding](http://wpftutorial.net/DataBindingOverview.html)
- an MVVM friendly API for [MahApps.Metro](http://mahapps.com/)
- [Inversion-of-Control](http://martinfowler.com/articles/injection.html) with ViewModel-First
  approach

The main purpose of this library is to speed up the development of small to midsize WPF
applications using [MahApps.Metro](http://mahapps.com/).

With the second major release the focus shifts somewhat. The main package, **TinyLittleMvvm**, concentrates
on making WPF more MVVM-friendly. All features related to [MahApps.Metro](http://mahapps.com/) have been
moved to a separate package, **TinyLittleMvvm.MahAppsMetro**.

## Features

**TinyLittleMvvm** provides following features:

- An straight-forward implementation of
  [`INotifyPropertyChanged`](http://msdn.microsoft.com/library/system.componentmodel.inotifypropertychanged)
- The omnipresent [RelayCommand](http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030)s by [Josh Smith](http://joshsmithonwpf.wordpress.com/about/)
  plus an async implementation
- Support for MahApps.Metro's [Dialogs](http://mahapps.com/controls/dialogs.html) and
  [Flyouts](http://mahapps.com/controls/flyouts.html)
- View and ViewModel resolution using [`IServiceProvider`](https://docs.microsoft.com/en-us/dotnet/api/system.iserviceprovider)
- Support for the generic host, introduced in .NET Core 2.1
- Logging via [`ILogger`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger)
- Easy-to-use project templates for both plain WPF and MahApps.Metro applications.

## Builds and Packages

| Channel  | Build | NuGet Package |
|----------|-------|---------------|
| Unstable | [![Build](https://github.com/thoemmi/TinyLittleMvvm/actions/workflows/build.yaml/badge.svg?branch=develop)](https://github.com/thoemmi/TinyLittleMvvm/actions/workflows/build.yaml?branch=develop) | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/TinyLittleMvvm.svg)](https://www.nuget.org/packages/TinyLittleMvvm/absoluteLatest) |
| Stable   | [![Build](https://github.com/thoemmi/TinyLittleMvvm/actions/workflows/build.yaml/badge.svg?branch=master)](https://github.com/thoemmi/TinyLittleMvvm/actions/workflows/build.yaml?branch=master)   | [![Nuget](https://img.shields.io/nuget/v/TinyLittleMvvm.svg)](https://www.nuget.org/packages/TinyLittleMvvm/) |
