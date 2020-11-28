using Harbor.Cargo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Harbor.Ship
{
    public class LocalShip : IShip // save files for private, public - loggings
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
        public ReportFilter ReportCargoFilter { get; set; }
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

        private List<Report> reports; //separately save(private)
        private List<ILoadable> cargos; //separately save(private)
        private List<LogCargo> logCargos; //separately save(public)

        //save public (log) private (pattern, emotion)
        #region Constructors
        public LocalShip(Dictionary<CargoType, string> privateSavepaths, string publicLogSavepath, string cargoReportSavepath)
        {

            privateSavepathByCargo = privateSavepaths;
            PublicLogSavepath = publicLogSavepath;
            CargoReportSavepath = cargoReportSavepath;

            reports = new List<Report>();
            cargos = new List<ILoadable>();
            logCargos = new List<LogCargo>();
            ReportCargoFilter = new ReportFilter();
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

        #region Loadings

        public void LoadPrivate(ILoadable cargo)
        {
            if (cargo.IsEmpty())
                throw new CargoException(CargoExceptionMsg.Empty);
            if (cargo.IsLocked)
                cargos.Add(cargo);
            else
                throw new CargoException(CargoExceptionMsg.NotLocked);
        }
        public void LoadPublicLog(LogCargo log)
        {
            if (log.IsEmpty())
                throw new CargoException(CargoExceptionMsg.Empty);
            if (log.IsLocked)
                logCargos.Add(log);
            else
                throw new CargoException(CargoExceptionMsg.NotLocked);
        }
        public void LoadReport(Report report)
        {
            if (ReportCargoFilter.Validate(report))
                reports.Add(report);
            else
                throw new FilterException("This report doesn't meet LocalShip's filter that set");
        }

        #endregion
        #region Unloadings

        /// <summary>
        /// UnloadReport return report & remove that
        /// 
        /// WARN : It removes the report at the index
        /// </summary>
        /// <param name="index">The index that you will remove and get</param>
        /// <returns>One of the element at index</returns>
        public Report UnloadReport(int index)
        {
            return reports.Pop(index);
        }
        /// <summary>
        /// UnloadReports return List of CargoReport & remove all of conformed the filter
        /// 
        /// WARN : It removes all the reports conformed the filter
        /// </summary>
        /// <param name="filter">Remove by filter. If filter.validate(r) true, then remove.</param>
        /// <returns>CargoReports List conformed the filter passed</returns>
        public IEnumerable<Report> UnloadReports(ReportFilter filter)
        {
            var reportsByFilter = reports.Where(r => filter.Validate(r));
            reports.RemoveAll(r => filter.Validate(r));

            return reportsByFilter;
        }
        /// <summary>
        /// Return ILoadable at index and remove.
        /// </summary>
        /// <param name="index">The index that you will remove & get</param>
        /// <returns>The ILoadable you selected</returns>
        public ILoadable UnloadCargo(int index)
        {
            return cargos.Pop(index);
        }
        /// <summary>
        /// Return LogCargo at index and remove.
        /// </summary>
        /// <param name="index">The index that you will remove & get</param>
        /// <returns>The LogCargo you selected</returns>
        public LogCargo UnloadLogCargo(int index)
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
                var r = bs.TransferBinary<Report>(path);
                if (r != default)
                    yield return r;
            }

        }
        public IEnumerable<LogCargo> OpenPublicLogFiles(OpenFileFilter logCargoFilter = null, OpenFileFilter logDatFilter = null)
        {
            string[] cargoDirs = Directory.GetDirectories(PublicLogSavepath, $"{(logCargoFilter == null ? "*" : logCargoFilter.ToString())}", SearchOption.TopDirectoryOnly);
            var bs = new BinarySave();
            foreach (var cargoDir in cargoDirs)
            {
                string[] logs = Directory.GetFiles(cargoDir, $"{(logDatFilter == null ? "*l" : logDatFilter.ToString())}.dat", SearchOption.TopDirectoryOnly);
                
                if (logs.Length <= 0) //Check Empty Cargo <- But Already blocked at Loading to ship.
                    continue;

                LogCargo lc = new LogCargo();

                foreach (var logfile in logs) //Get each logs in cargo(cargo = folder, log = .dat) only for this
                {
                    var log = bs.TransferBinary<Log.IActivityLog>(logfile);
                    if (log != default)
                        lc.Load(log);
                }

                lc.Lock();
                yield return lc;
            }
        }

        public IEnumerable<ILoadable> OpenPrivatesFiles(CargoType type, OpenFileFilter cargoFilter=null)
        {
            var bs = new BinarySave();
            var cargoDir = GetPrivateSavepath(type);
            if (Directory.Exists(cargoDir))
            {
                string[] dats = Directory.GetFiles(cargoDir, $"{(cargoFilter == null ? "*c" : cargoFilter.ToString())}.dat", SearchOption.TopDirectoryOnly);

                foreach(var dat in dats)
                {
                    yield return bs.TransferBinary<ILoadable>(dat);
                }
            }

        }

        #endregion
        #region Pulling Away

        public void PullAwayCargoReports() //Save cargo report
        {
            if (!Directory.Exists(CargoReportSavepath))
                Directory.CreateDirectory(CargoReportSavepath).Attributes = FileAttributes.Hidden;
            var bs = new BinarySave();

            for(int i = reports.Count-1;i >= 0;i--)
            {
                var report = reports[i];
                bs.Savepath = $@"{CargoReportSavepath}/{report.ReportedTime.ToDefault()}r.dat";
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
                    logCargo.SetPrimaryTimeOnce(DateTime.Now); //Must be preserve!

                var logCargoDirPath = $@"{PublicLogSavepath}/{logCargo.PrimaryTime.GetValueOrDefault().ToDefault()}";
                if (!Directory.Exists(logCargoDirPath))
                    Directory.CreateDirectory(logCargoDirPath);

                foreach (Log.IActivityLog log in logCargo.GetLogs())
                {
                    bs.Savepath = $@"{logCargoDirPath}/{log.Time.ToDefault()}l.dat";
                    bs.TransferToBinary<Log.IActivityLog>(log);
                }
            }
            logCargos.Clear();
        }
        public void PullAwayPrivateCargos() //Save as Cargo
        {
            if (!Directory.Exists(privateDftRootPath)) //Create hidden root folder for private loadables
                Directory.CreateDirectory(privateDftRootPath).Attributes = FileAttributes.Hidden;
            var bs = new BinarySave();

            for (int i = cargos.Count - 1; i >= 0; i--)
            {
                ILoadable cargo = cargos[i];
                if (!Directory.Exists(privateSavepathByCargo[cargo.Type]))
                    Directory.CreateDirectory(privateSavepathByCargo[cargo.Type]).Attributes = FileAttributes.Hidden;

                if (cargo.PrimaryTime == null)
                    cargo.SetPrimaryTimeOnce(DateTime.Now); //Must be preserve!

                bs.Savepath = $@"{privateSavepathByCargo[cargo.Type]}/{cargo.PrimaryTime.GetValueOrDefault().ToDefault()}c.dat";
                switch (cargo.Type)
                {
                    case CargoType.GenericObject:
                        bs.TransferToBinary(cargo as RawCargo);
                        break;
                    case CargoType.Text:
                        bs.TransferToBinary(cargo as TextCargo);
                        break;
                    case CargoType.Voice:
                        bs.TransferToBinary(cargo as VoiceCargo);
                        break;
                    case CargoType.Log:
                        bs.TransferToBinary(cargo as LogCargo);
                        break;
                }
            }
            cargos.Clear();

        }
        public async Task PullAwayAsync()
        {
            var pullingPublic = Task.Run(new Action(() => PullAwayPublicLogs()));
            var pullingPrivate = Task.Run(new Action(() => PullAwayPrivateCargos()));
            PullAwayCargoReports();

            await pullingPublic;
            await pullingPrivate;
        }
        #endregion
    }
}
