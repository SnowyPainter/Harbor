using System;

namespace ADPC
{
    public enum CargoType
    {
        Text,
        Voice,
        Log
    }
    public class ActivityLog // data of activity log. log mean user moving path.
    {

    }
    public class Crane //collect data help
    {
    }
    public class RawCargo //A raw data 
    {
        //Type cargo... text, move log (app active move)
        //Type string, movelog array ... etc  Content { get; set; }
    }
    public class CargoReport //Analyzed cargo by cargoType
    {
        // func) positive or negetive
        // func) fast or slow
        // func) overlap logging (user can't find something useful)
        // func) .. etc
    }

    public class AnalyzeMake // rawcargo -> cargo report
    {

    }

    public class PacketMake //cargo report -> protobuf
    {

    }

    public class CargoShip //send to server & save local
    {
        //set logging server.
        //set local saving path etc...
    }
}
