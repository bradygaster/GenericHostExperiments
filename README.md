# GenericHost Experiments

After some interesting discussion with my teammates I took a look at the docs for the .NET Core [`GenericHost`](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.1) object to develop some ideas on how I would use it, specifically for Azure scenarios. 

This repository includes some experimental extensions atop the `GenericHost` object. 

## Version 0.0.1.0

This first iteration resulted in adding an extension method to add [Azure Storage](https://docs.microsoft.com/en-us/azure/storage/common/storage-introduction) support. 