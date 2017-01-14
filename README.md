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
applications using [MahApps.Metro](http://mahapps.com/). Additionally it uses 
[LibLog](https://github.com/damianh/LibLog) for logging and [Autofac](http://autofac.org/) 
as the IoC container. (I could have used 
[CommonServiceLocator](https://commonservicelocator.codeplex.com/), but since I use Autofac
in most of my applications I didn't bother).

## Features

**TinyLittleMvvm** provides following features:

- An straight-forward implementation of 
  [`INotifyPropertyChanged`](http://msdn.microsoft.com/library/system.componentmodel.inotifypropertychanged)
- The omnipresent [RelayCommand](http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030)s by [Josh Smith](http://joshsmithonwpf.wordpress.com/about/)
  plus an async implementation
- Support for [Dialogs](http://mahapps.com/controls/dialogs.html) and
  [Flyouts](http://mahapps.com/controls/flyouts.html)

## NuGet package

[![NuGet](https://img.shields.io/nuget/v/TinyLittleMvvm.svg?style=flat-square)](https://www.nuget.org/packages/TinyLittleMvvm/)

[https://www.nuget.org/packages/TinyLittleMvvm/](https://www.nuget.org/packages/TinyLittleMvvm/)

## Version history

### v0.4

- **Breaking change:** Switched from [NLog](http://nlog-project.org/) to
  [LibLog](https://github.com/damianh/LibLog), so users of **TinyLittleMvvm** can use whatever
  logging framework they want. Users of previous versions of **TinyLittleMvvm** need to 
  add and configure logging library.
  

## Build service

**TinyLittleMvvm** is built by [MyGet](https://www.myget.org/) and [AppVeyor](http://www.appveyor.com/).

[![tools MyGet Build Status](https://www.myget.org/BuildSource/Badge/tools?identifier=96929ad0-295a-499c-b636-9ffa12346465)](https://www.myget.org/)
[![Build status](https://ci.appveyor.com/api/projects/status/8pu570b0pm7uw63c/branch/master?svg=true)](https://ci.appveyor.com/project/thoemmi/tinylittlemvvm/branch/master)
