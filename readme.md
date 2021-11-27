# Harbor DUS - DATA UTILIZING SYSTEM
**Harbor** can be used as a **logger**, **analyzer** and **network** bridge for your servers  
![Logo](./logo.png)

# Why Harbor DUS?
1. For develope fast  
2. For collecting precious data  
3. For no complicated and meaning database query.

**Simple the reason** but also it's the **main point for developing**  

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

## Ships
### LocalShip  
---
Saves data that load into cargo where set path(individual).  
```Open...``` To read cargos from directory.  
```PullAway...``` To save cargos to directory.  
All the datas are serialized to json.
### NetworkShip  
---
As HTTP Client, send reports, etc ... to server.  
Every cargos are able to be xml byte array packet.  
All the datas are serialized to xml. (application/xml)  

# Example

Mostly ressemble name class & functions process ressemble works.  
And most methods and parameters are commented. (hovering text - visual studio)  

``` cs
LocalShip localShip = new LocalShip();
HTTPShip httpShip = new HTTPShip();
```

## LocalShip Initialize with arguments
All the paths must be Absolute path.
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
### Custom DataLog  
To implement IDataLog
``` cs
class MyLog:IDataLog {
    ...
}
```
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
    logCargo.Load(...);
}
```

## Load cargo to local ship
> Checking the cargo & data's are ok is very important.  

_Check empty first, lock it & load to Ship!_
``` cs
if (logCargo.IsEmpty()) return;
logCargo.Lock();
localShip.LoadPublicLog(logCargo);
```

## PullAway LocalShip
It's very high cost processing (every pulling away).  
So, It's important to check the cargo data collected intact.
``` cs
await localShip.PullAwayAsync();
```