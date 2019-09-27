# Massif Licensing Tools for .NET

A simple addin sample for Autodesk Inventor 2020, that demonstrates the use of the Massif Licensing SDK for .NET, to query license information for assets hosted on the **Massif Licensing Platform.**
Please note that this SDK should not be used for publishing your addins. To publish your apps on the store, we need to run them through our build process which secures your code. Once you have your
addin running and tested, you can submit it to Massif to have it secured, and published on the platform.

---

### Prerequisites:

* **Massif.Licensing NuGet package** - Available through the NuGet package manager in Visual Studio.
* **CPS Desktop App** must be installed, and user logged in, to be able to provide the asset information.
* Port 3002 must be open to allow the .NET licensing tools to access the **CPS Desktop App** local web server. If you want to use a non-standard port, then that's ok, but you will need to provide the custom port when instantiating the Connector object.
* .NET Framework 4.7.2 or above.

---

### What you get:

* **Addin boilerplate** - Sample Visual Studio 2017 project, for an addin for Inventor that should you get up and running quickly with a simple toolbar button that performs a license check.   

---

### How to use this:

1. Clone this github repo to your machine.
2. Load the solution into Visual Studio (we suggest Visual Studio 2017 Community Edition.)
3. Install the **Massif.Licensing** NuGet package, using the NuGet package manager in Visual Studio.
4. Add the following statement to the top of your code `using Massif.Licensing;`
5. Instantiate a `Massif.Licensing.Connector` object using either the default constructor, or the overload which allows for a custom port to be passed in. If you are using a custom port, the CPS Desktop App will have to be configured appropriately to allow it (not currently a user-configurable setting.)
6. Call the `GetAssetInfo()` method and pass in a product code (supplied by Massif), to get the `AssetInfo` object which includes asset information from the **CPS Desktop App.**
7. Read the `LicenseActive` property (boolean) to determine whether there is a valid and active license for the asset or not.
8. In the published addin, we will perform additional checks to provide a higher level of IP protection.

### Example code:

```
using Massif.Licensing;

...

Connector licConn = new Connector();
Connector.IAssetInfo asset = licConn.GetAssetInfo("ABCXYZ123");

***Use asset.LicenseActive to block or allow access to functions in your app here***

Console.WriteLine(asset.Message);
```

---

## License

See `EULA.html` in the repo.

Copyright 2019 - **Massif Limited.**
