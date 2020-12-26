# Harbor DUS - DATA UTILIZING SYSTEM
**Harbor** can be used as a **logger**, **analyzer** and **network** bridge for your servers

# Harbor(.NET Standard 2.0) Implementation
1. .NET Core & .NET 5 
2. .NET Framework 4.6.1
3. .Mono 5.4
4. Xamarin.iOS 10.14
5. Xamarin.Mac 3.8
6. Xamarin Android 8.0
7. UWP 10.0.16299
8. Unity 2018.1

# Documentation

_Sentences which have '*' of its head are not implemented_

## Ships
### LocalShip
Saves data that load into cargo where set path(individual).  
### NetworkShip 
Package data to protobuf binary.
* As MQTT Client, send reports to server.
* As HTTP Client, send reports, etc ... to server.

## Analyzing
Basic loss, cost & activating functions are supported and it also can be customized.
* Classifier models such as knn, lr, etc ...
* Text analyzing models & dataset
* Analyzing by user logs must be optimized for the program it used be.  

There is no data that suits all situations for logs.
