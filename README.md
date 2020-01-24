Zebra Savanna Data Services C# Sample Code
==========================================

This sample code is meant to be used with the [Savanna CSharp SDK](https://github.com/Zebra/Savanna-CSharp-SDK).  It shows basic demos of the main Data Services APIs and how to use the native C# SDK to access them.  For use with Web, Windows, Android, or iOS.  See below for setup of the DataWedge Profile in Zebra Android devices to work with the SDK.

For more information on these APIs, go to [Developer.zebra.com/apis](https://developer.zebra.com/apis) or visit the [Forums](https://developer.zebra.com/forum/search?keys=&field_zebra_curated_tags_tid%5B%5D=273)

API Key
-------

To get an API key to work with these APIs, use the [Getting Started Guide](https://developer.zebra.com/gsg) and select the Barcode Intelligence product.  

Android Device Setup
--------------------

Create a DataWedge profile:

* Associate the app com.zebra.savanna.sample
* Configure scanner settings to decode desired barcode types
* Enable intent output
* Set intent action to `com.zebra.SCAN`
* Change intent delivery to Broadcast intent
