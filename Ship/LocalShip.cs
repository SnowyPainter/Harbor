using Harbor.Cargo;
using Harbor.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Harbor.Ship
{
    public class LocalShip : Ship // save files for private, public - loggings
    {
        #region Variables
        private static string adpctest = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ADPCTEST";
        private static string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string TEST_DIR_PATH = $@"{adpctest}";
        private static string privateDftRootPath = $@"{TEST_DIR_PATH}/privates";

        private Dictionary<CargoType, string> privateSavepathByCargo;
        private string publicLogSavepath;
        private string cargoReportSavepath;
        #endregion
        #region Property Getter Setter
        public string PublicLogSavepath
        {
            get
            {
                return publicLogSavepath;
            }
            set
            {
                if (value.IsValidPath())
                    publicLogSavepath = value;
                else
                    publicLogSavepath = null;
            }
        }
        public string CargoReportSavepath
        {
            get
            {
                return cargoReportSavepath;
            }
            set
            {
                if (value.IsValidPath())
                    cargoReportSavepath = value;
                else
                    cargoReportSavepath = null;
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

        //private : report, text cargo, voice cargo, generic cargo
        //public : log

        /*
         * protected List<Report> reports; //separately save(private)
         * protected List<ILoadable> cargos; //separately save(private) 
         */
        private List<WPFLogCargo> logCargos; //separately save(public)

        //save public (log) private (pattern, emotion)
        #region Constructors
        public LocalShip(Dictionary<CargoType, string> privateSavepaths, string publicLogSavepath, string cargoReportSavepath)
        {

            privateSavepathByCargo = privateSavepaths;
            PublicLogSavepath = publicLogSavepath;
            CargoReportSavepath = cargoReportSavepath;

            //reports = new List<Report>();
            //cargos = new List<ILoadable>();
            logCargos = new List<WPFLogCargo>();
        }
        public LocalShip() : this(
                new Dictionary<CargoType, string>()
                {
                    {CargoType.GenericObject, $@"{privateDftRootPath}/generic"},
                    {CargoType.Text, $@"{privateDftRootPath}/txt"},
                    {CargoType.Voice, $@"{privateDftRootPath}/talk"},
                    {CargoType.Log, $@"{privateDftRootPath}/log"}
                },
                $@"{TEST_DIR_PATH}/logs",
                $@"{TEST_DIR_PATH}/reports"
            )
        { }

        #endregion


        /*
         * 
         * Idenify
         * cargo = last char = 'c'
         * report = last char = 'r'
         * each log = last char = 'l'
         * 
         */

        #region Loadings - Seperatly implemented for PublicLog in LocalShip
        public void LoadPublicLog(WPFLogCargo log)
        {
            if (log.IsEmpty())
                throw new CargoException(CargoExceptionMsg.Empty);
            if (log.IsLocked)
                logCargos.Add(log);
            else
                throw new CargoException(CargoExceptionMsg.NotLocked);
        }
        #endregion
        #region Unloadings - Mostly receive from parent SHIP class.

        /// <summary>
        /// Return LogCargo at index and remove.
        /// </summary>
        /// <param name="index">The index that you will remove & get</param>
        /// <returns>The LogCargo you selected</returns>
        public WPFLogCargo UnloadLogCargo(int index)
        {
            return logCargos.Pop(index);
        }

        #endregion
        #region Open from Dirs
        public IEnumerable<Report> OpenCargoReportsFiles(OpenFileFilter filter = null)
        {
            string[] filePaths = Directory.GetFiles(CargoReportSavepath, $"{(filter == null ? "*r" : filter.ToString())}.dat", SearchOption.TopDirectoryOnly);
            
            var bs = new BinarySave();
            foreach (var path in filePaths)
            {
                Debug.WriteLine(path);
                var r = bs.TransferBinary<Report>(path);
                if (r != default)
                    yield return r;
            }

        }
        public IEnumerable<WPFLogCargo> OpenPublicWPFLogCargo(OpenFileFilter logCargoFilter = null, OpenFileFilter logDatFilter = null)
        {
            string[] cargoDirs = Directory.GetDirectories(PublicLogSavepath, $"{(logCargoFilter == null ? "*" : logCargoFilter.ToString())}", SearchOption.TopDirectoryOnly);
            var bs = new BinarySave();
            foreach (var cargoDir in cargoDirs)
            {
                string[] logs = Directory.GetFiles(cargoDir, $"{(logDatFilter == null ? "" : logDatFilter.ToString())}{FileEndChar.WPFLog}.dat", SearchOption.TopDirectoryOnly);
                
                if (logs.Length <= 0)
                    continue;

                WPFLogCargo lc = new WPFLogCargo();

                foreach (var logfile in logs) //Get each logs in cargo(cargo = folder, log = .dat) only for this
                {
                    var log = bs.TransferBinary<WPFActivityLog>(logfile);
                    if (log != default)
                        lc.Load(log);
                }

                lc.Lock();
                yield return lc;
            }
        }
        public IEnumerable<DataLogCargo> OpenPublicDataLogCargo(OpenFileFilter logCargoFilter = null, OpenFileFilter logDatFilter = null)
        {
            string[] cargoDirs = Directory.GetDirectories(PublicLogSavepath, $"{(logCargoFilter == null ? "*" : logCargoFilter.ToString())}", SearchOption.TopDirectoryOnly);
            var bs = new BinarySave();
            foreach (var cargoDir in cargoDirs)
            {
                string[] logs = Directory.GetFiles(cargoDir, $"{(logDatFilter == null ? "" : logDatFilter.ToString())}{FileEndChar.DataLog}.dat", SearchOption.TopDirectoryOnly);

                if (logs.Length <= 0)
                    continue;

                DataLogCargo lc = new DataLogCargo();

                foreach (var logfile in logs) //Get each logs in cargo(cargo = folder, log = .dat) only for this
                {
                    var log = bs.TransferBinary<DataLog>(logfile);
                    if (log != default)
                        lc.Load(log);
                }

                lc.Lock();
                yield return lc;
            }
        }

        public IEnumerable<Cargo.Cargo> OpenPrivatesFiles(CargoType type, OpenFileFilter cargoFilter=null)
        {
            var bs = new BinarySave();
            var cargoDir = GetPrivateSavepath(type);
            if (Directory.Exists(cargoDir))
            {
                string[] dats = Directory.GetFiles(cargoDir, $"{(cargoFilter == null ? "" : cargoFilter.ToString())}*{FileEndChar.Cargo}.dat", SearchOption.TopDirectoryOnly);

                foreach(var dat in dats)
                {
                    yield return bs.TransferBinary<Cargo.Cargo>(dat);
                }
            }

        }

        #endregion
        #region Pulling Away

        public override void PullAwayReports() //Save cargo report
        {
            if (!Directory.Exists(CargoReportSavepath))
                Directory.CreateDirectory(CargoReportSavepath).Attributes = FileAttributes.Hidden;
            var bs = new BinarySave();

            for(int i = reports.Count-1;i >= 0;i--)
            {
                var report = reports[i];
                bs.Savepath = $@"{CargoReportSavepath}/{report.ReportedTime.ToDefault()}{FileEndChar.Report}.dat";
                bs.TransferToBinary(report);
            }

            reports.Clear();
        }
        public void PullAwayPublicLogs() //Save(dat) each **log** in cargo
        {
            if (!Directory.Exists(PublicLogSavepath))
                Directory.CreateDirectory(PublicLogSavepath);
            var bs = new BinarySave();

            for (int i = logCargos.Count - 1; i >= 0; i--)
            {
                var logCargo = logCargos[i];

                if (logCargo.PrimaryTime == null)
                    logCargo.SetPrimaryTimeNow();

                var logCargoDirPath = $@"{PublicLogSavepath}/{logCargo.PrimaryTime.GetValueOrDefault().ToDefault()}";
                if (!Directory.Exists(logCargoDirPath))
                    Directory.CreateDirectory(logCargoDirPath);

                foreach (Log.IActivityLog log in logCargo.GetLogs())
                {
                    string endchar = "";
                    if(log.Type == Log.LogType.Data)
                    {
                        endchar = FileEndChar.DataLog;
                    }
                    else if(log.Type == Log.LogType.WPFElement)
                    {
                        endchar = FileEndChar.WPFLog;
                    }
                    bs.Savepath = $@"{logCargoDirPath}/{log.Time.ToDefault()}{endchar}.dat";
                    bs.TransferToBinary<Log.IActivityLog>(log);
                }
            }
            logCargos.Clear();
        }
        public override void PullAwayCargos() //Save as Cargo
        {
            if (!Directory.Exists(privateDftRootPath)) //Create hidden root folder for private loadables
                Directory.CreateDirectory(privateDftRootPath).Attributes = FileAttributes.Hidden;
            var bs = new BinarySave();

            for (int i = cargos.Count - 1; i >= 0; i--)
            {
                Cargo.Cargo cargo = cargos[i];
                if (!Directory.Exists(privateSavepathByCargo[cargo.Type]))
                    Directory.CreateDirectory(privateSavepathByCargo[cargo.Type]).Attributes = FileAttributes.Hidden;

                if (cargo.PrimaryTime == null)
                    cargo.SetPrimaryTimeNow();

                bs.Savepath = $@"{privateSavepathByCargo[cargo.Type]}/{cargo.PrimaryTime.GetValueOrDefault().ToDefault()}{FileEndChar.Cargo}.dat";
                if (cargo.Type != CargoType.GenericObject)
                {
                    SaveCargoAsBinaryFile(bs, cargo);
                }
                
            }
            cargos.Clear();
        }
        public void PullAwayRawCargo<T>(RawCargo<T> c)
        {
            var bs = new BinarySave();
            SaveRawCargoAsBinaryFile<T> (bs, c);
        }
        public async Task PullAwayAsync()
        {
            var pullingPublic = Task.Run(new Action(() => PullAwayPublicLogs()));
            var pullingPrivate = Task.Run(new Action(() => PullAwayCargos()));
            PullAwayReports();

            await pullingPublic;
            await pullingPrivate;
        }
        #endregion
    }
}
