## Version history

### v2 (not released yet)

- .NET Core 3 support
- Dropped support for .NET Framework &lt; 4.7.2
- Use **Microsoft.Extensions.Logging.Abstraction** instead of **LibLog**.
- Use **Microsoft.Extensions.DependencyInjection.Abstraction** instead of **Autofac**.
- Strong-named assembly.
- Template for `dotnet new`.

### v1.1 (4/29/2019)

- fixed deployment bug

### v1.0 (4/29/2019)

- removed **NLog** dependency (wasn't used actually, but the package was still referenced)
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
