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


## Builds and Packages

| Channel  | Build | NuGet Package |
|----------|-------|---------------|
| Unstable | [![Build Status](https://dev.azure.com/thoemmi/TinyLittleMvvm/_apis/build/status/thoemmi.TinyLittleMvvm?branchName=develop)](https://dev.azure.com/thoemmi/TinyLittleMvvm/_build/latest?definitionId=5&branchName=develop) | [![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/TinyLittleMvvm.svg)](https://www.nuget.org/packages/TinyLittleMvvm/absoluteLatest) |
| Stable   | [![Build Status](https://dev.azure.com/thoemmi/TinyLittleMvvm/_apis/build/status/thoemmi.TinyLittleMvvm?branchName=master)](https://dev.azure.com/thoemmi/TinyLittleMvvm/_build/latest?definitionId=5&branchName=master)   | [![Nuget](https://img.shields.io/nuget/v/TinyLittleMvvm.svg)](https://www.nuget.org/packages/TinyLittleMvvm/) |


## Version history

### v1.1 (4/29/2019)

- fixed deployment bug

### v1.0 (4/29/2019)

- removed NLog dependency (wasn't used actually, but the package was still referenced)
- Allow positioning of flyouts (issue #10)
 
### v0.5 (1/14/2017)

- Added `ICancelableOnClosingHandler`

### v0.4 (1/14/2017) 

- **Breaking change:** Switched from [NLog](http://nlog-project.org/) to
  [LibLog](https://github.com/damianh/LibLog), so users of **TinyLittleMvvm** can use whatever
  logging framework they want. Users of previous versions of **TinyLittleMvvm** need to 
  add and configure logging library.

### v0.3.2 (3/31/2016) 
  
- Added documentation

### v0.3 (1/11/2015) 
  
- First usable version
