using Harbor.Cargo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Harbor.Ship
{
    public class LocalShip : Ship // save files for private, public - loggings
    {
        #region Variables
        private static string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        private Dictionary<CargoType, string> privateSavepathByCargo;
        private string? publicDataSavepath = "./";
        #endregion
        #region Property Getter Setter
        public string? PublicDataSavepath
        {
            get
            {
                return publicDataSavepath;
            }
            set
            {
                if (value != null && value.IsValidPath())
                    publicDataSavepath = value;
                else
                    publicDataSavepath = null;
            }
        }

        public void SetPrivateSavepath(CargoType cargo, string path)
        {
            if (path.IsValidPath())
                privateSavepathByCargo[cargo] = path;
        }
        public string GetPrivateSavepath(CargoType cargo)
        {
            return privateSavepathByCargo[cargo];
        }
        #endregion

        //private : log cargo, text cargo, voice cargo, generic cargo
        //public : log cargo
        #region Constructors
        public LocalShip(Dictionary<CargoType, string> privateSavepaths, string publicLogSavepath)
        {
            privateSavepathByCargo = privateSavepaths;
            PublicDataSavepath = publicLogSavepath;

            privateCargos = new List<Cargo.Cargo>();
            publicCargos = new List<DataCargo>();
        }
        public LocalShip() : this(
                new Dictionary<CargoType, string>()
                {
                    {CargoType.GenericObject, $@"{documents}/generic"},
                    {CargoType.Text, $@"{documents}/txt"},
                    {CargoType.Voice, $@"{documents}/talk"},
                    {CargoType.Log, $@"{documents}/log"}
                },
                $@"{documents}/logs"
            )
        { }

        #endregion


        /*
         * 
         * Idenify
         * cargo = last char = 'c'
         * each log = last char = 'l'
         * 
         */

        #region Open from Dirs
        /// <summary>
        /// Open logs and group to cargo, and return.
        /// </summary>
        /// <param name="openDir">Principal, PublicLogSavepath</param>
        /// <param name="logCargoFilter"></param>
        /// <param name="logDatFilter"></param>
        /// <returns></returns>
        public List<DataCargo> OpenPublicDataCargo(string openDir, OpenFileFilter logCargoFilter, OpenFileFilter logDatFilter)
        {
            var logCargos = new List<DataCargo>();
            string[] cargoDirs = Directory.GetDirectories(openDir, logCargoFilter.ToString(), SearchOption.TopDirectoryOnly);
            var bs = new JsonSaves();
            foreach (var cargoDir in cargoDirs)
            {
                string[] logs = Directory.GetFiles(cargoDir, $"{logDatFilter.ToString()}{FileEndChar.DataLog}.dat", SearchOption.TopDirectoryOnly);

                if (logs.Length <= 0)
                    continue;

                DataCargo lc = new DataCargo();

                foreach (var logfile in logs) //Get each logs in cargo(cargo = folder, log = .dat) only for this
                {
                    var log = bs.TransferToObject<Data>(logfile);
                    if (log != default)
                        lc.Load(log);
                }

                lc.Lock();
                logCargos.Add(lc);
            }
            return logCargos;
        }
        public List<Cargo.Cargo> OpenPrivatesFiles(CargoType type, OpenFileFilter cargoFilter)
        {
            List<Cargo.Cargo> cargos = new List<Cargo.Cargo>();
            var bs = new JsonSaves();
            var cargoDir = GetPrivateSavepath(type);
            if (Directory.Exists(cargoDir))
            {
                //get all files in cargoFilter.ToString() which end char is Cargo(=privates)
                string[] dats = Directory.GetFiles(cargoDir, $"{cargoFilter.ToString()}*{FileEndChar.Cargo}.dat", SearchOption.TopDirectoryOnly);

                foreach(var dat in dats)
                {
                    var val = bs.TransferToObject<Cargo.Cargo>(dat);
                    if (val == null) continue;
                    cargos.Add(val);
                }
            }
            return cargos;

        }
        #endregion
        #region Pulling Away
        public void PullAwayPublicData()
        {
            if (PublicDataSavepath != null && !Directory.Exists(PublicDataSavepath))
                Directory.CreateDirectory(PublicDataSavepath);
            var bs = new JsonSaves();

            for (int i = publicCargos.Count - 1; i >= 0; i--)
            {
                var logCargo = publicCargos[i];

                if (logCargo.PrimaryTime == null)
                    logCargo.SetPrimaryTimeNow();

                var logCargoDirPath = $@"{PublicDataSavepath}/{logCargo.PrimaryTime.GetValueOrDefault().ToDefault()}";
                if (!Directory.Exists(logCargoDirPath))
                    Directory.CreateDirectory(logCargoDirPath);

                foreach (var log in logCargo.GetDatas())
                {
                    var dt = DateTime.Now.ToString("mmssffffff");
                    bs.SaveToObject(log, $@"{logCargoDirPath}/{dt}{FileEndChar.DataLog}.dat");
                }
            }
            publicCargos.Clear();
        }
        public override void PullAwayPrivateCargos()
        {
            if (!Directory.Exists(documents)) //Create hidden root folder for private loadables
                Directory.CreateDirectory(documents).Attributes = FileAttributes.Hidden;
            var bs = new JsonSaves();

            for (int i = privateCargos.Count - 1; i >= 0; i--)
            {
                Cargo.Cargo cargo = privateCargos[i];
                if (!Directory.Exists(privateSavepathByCargo[cargo.Type]))
                    Directory.CreateDirectory(privateSavepathByCargo[cargo.Type]).Attributes = FileAttributes.Hidden;

                if (cargo.PrimaryTime == null)
                    cargo.SetPrimaryTimeNow();
                var path = $@"{privateSavepathByCargo[cargo.Type]}/{cargo.PrimaryTime.GetValueOrDefault().ToDefault()}{FileEndChar.Cargo}.dat";

                switch (cargo.Type)
                {
                    case Cargo.CargoType.GenericObject:
                        bs.SaveToObject(cargo as Cargo.RawCargo, path);
                        break;
                    case Cargo.CargoType.Text:
                        bs.SaveToObject(cargo as Cargo.TextCargo, path);
                        break;
                    case Cargo.CargoType.Voice:
                        bs.SaveToObject(cargo as Cargo.VoiceCargo, path);
                        break;
                    case Cargo.CargoType.Log:
                        bs.SaveToObject(cargo as Cargo.DataCargo, path);
                        break;
                }

            }
            privateCargos.Clear();
        }

        public async Task PullAwayAsync()
        {
            var pullingPublic = Task.Run(new Action(() => PullAwayPublicData()));
             PullAwayPrivateCargos();

            await pullingPublic;
        }
        #endregion
    }
}
