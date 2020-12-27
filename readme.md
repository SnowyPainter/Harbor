# Harbor DUS - DATA UTILIZING SYSTEM
**Harbor** can be used as a **logger**, **analyzer** and **network** bridge for your servers  
![Logo](./logo.png)

# Why Harbor DUS?
1. For developing fast  
2. For collecting precious data  
3. _For no stress_

**Simple the reason** but also it's the **main point for developing**  

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

## Abstract Classes
### **Harbor.Cargo.Cargo**
#### Properties
1. IsLocked (boolean)
2. Type (CargoType)
3. ***PrimaryTime*** (DateTime nullable)

The ***PrimaryTime*** is very worth data for who collects.  
> Preferably ***PrimaryTime*** shouldn't be changed periodically.  

It is implemented ```ToPacket```  
All the custom cargo have to inherit Harbor.Cargo.Cargo.

## Cargos(Cargo/Cargos.cs), *Report(ML/Report.cs)
### *Report
---
#### Properties
1. PNP
2. Emotion
3. **ReportedTime**
---
### RawCargo
It has a byte array. so it is called ```RawCargo```  
Can contain anything which could be byte array.   
And the others ...  

---
1. LogCargo
2. TextCargo
3. *VoiceCargo
---

## Ships
### LocalShip  
---
Saves data that load into cargo where set path(individual).  
```Open...``` To read cargos from directory.  
```PullAway...``` To save cargos to directory.  
All the datas are serialized to binary.
### NetworkShip  
---
*As MQTT Client, send reports to server.  
As HTTP Client, send reports, etc ... to server.  
Every cargos are able to be xml byte array packet.  
All the datas are serialized to xml. (application/xml)  
## Analyzing
Basic loss, cost & activating functions are supported and it also can be customized.
* Classifier models such as knn, lr, etc ...
* Text analyzing models & dataset
* Analyzing by user logs must be optimized for the program it used be.  

There is no data that suits all situations for logs.

## Program.cs
Just a utility code.
```PreventSleepOrPowerSaving```, ```CheckInternetConnection``` ... etc  

# Example

Mostly ressemble name class & functions process ressemble works.  
And most methods and parameters are commented. (hovering text - visual studio)  

``` cs
LocalShip localShip = new LocalShip();
HTTPShip httpShip = new HTTPShip();
LogCargo mainLogCargo = new LogCargo();
```

## LocalShip Initialize with arguments 
``` cs
new LocalShip(
    new Dictionary<CargoType, string>() //Hidden folders
    {
        {CargoType.GenericObject, $@"{anypath}/generic_stuff"},
        {CargoType.Text, $@"{anypath2}/txts_on_app"},
        {CargoType.Voice, $@"{anypath3}/talks"},
        {CargoType.Log, $@"{anypath2}/logdir"}
    },
    $@"{documents}/your_logs", //Public folder
    $@"{TEST_DIR_PATH}/private_reports", //Hidden folder
);
```

## Add log router to every buttons
``` cs
public MainWindow()
{
    InitializeComponent();

    foreach (Button button in UserInteraction.Children.OfType<Button>())
    {
        button.Click += LogRoute;
    }
}
private void LogRoute(object sender, RoutedEventArgs e)
{
    mainLogCargo.Load(DateTime.Now, sender as Button, Mouse.GetPosition(this));
}
```

## Load cargo to local ship
> Checking the cargo & data's are ok is very important.  

_Check empty first, lock it & load to Ship!_
``` cs
if (mainLogCargo.IsEmpty()) return;
mainLogCargo.Lock();
localShip.LoadPublicLog(mainLogCargo);
```

## PullAway LocalShip
It's very high cost processing (every pulling away).  
So, It's important to check the cargo data collected intact.
``` cs
await localShip.PullAwayAsync();
```