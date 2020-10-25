using ADPC.Cargo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ADPC.Ship
{
    public class LocalShip:IShip // save files for private, public - loggings
    {
        

        private static string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string TEST_DIR_PATH = $@"{documents}";//$@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\ADPCTEST";
        private static string privateDftRootPath = $@"{TEST_DIR_PATH}/Privates";
        
        private Dictionary<CargoType, string> privateSavepathByCargo;
        private string publicLogSavepath;
        private string cargoReportSavepath;

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

        private Stack<CargoReport> reports; //separately save(private)
        private Stack<ILoadable> cargos; //separately save(private)

        private Stack<LogCargo> logCargos; //separately save(public)

        //save public (log) private (pattern, emotion)
        #region Constructors
        public LocalShip(Dictionary<CargoType, string> privateSavepaths, string publicLogSavepath, string cargoReportSavepath)
        {

            privateSavepathByCargo = privateSavepaths;
            PublicLogSavepath = publicLogSavepath;
            CargoReportSavepath = cargoReportSavepath;

            reports = new Stack<CargoReport>();
            cargos = new Stack<ILoadable>();
            logCargos = new Stack<LogCargo>();
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
        
        #region Loadings

        public void LoadPrivate(ILoadable cargo)
        {
            cargos.Push(cargo);
        }
        public void LoadPublicLog(LogCargo log)
        {
            if (log.IsLocked)
                logCargos.Push(log);
            else
                throw new CargoException("Cargo is not locked");
        }
        public void LoadReports(CargoReport report)
        {
            reports.Push(report);
        }

        #endregion
        #region Unloadings
        
        /*
         * Unloading을 해야하는가?
         * 
         * 답 : 
         */

        #endregion
        #region Pulling Away
        public void PullAwayCargoReports() //Save cargo report
        {
            if (!Directory.Exists(cargoReportSavepath))
                Directory.CreateDirectory(cargoReportSavepath);
            var bs = new BinarySaver();

            while(reports.Count > 0)
            {
                var report = reports.Pop();
                bs.Savepath = $@"{cargoReportSavepath}/{report.ReportedTime.ToDefault()}.dat";
                bs.TransferToBinary(report);
            }
        }
        public void PullAwayPublicLogs() //Save each log in cargo
        {
            if (!Directory.Exists(publicLogSavepath))
                Directory.CreateDirectory(publicLogSavepath);
            var bs = new BinarySaver();
            
            while (logCargos.Count > 0)
            {
                var logCargo = logCargos.Pop();
                if (logCargo.PrimaryTime == null)
                    logCargo.SetPrimaryTimeOnce(DateTime.Now); //Must be preserve!

                var logCargoDirPath = $@"{publicLogSavepath}/{logCargo.PrimaryTime.GetValueOrDefault().ToDefault()}";
                if (!Directory.Exists(logCargoDirPath))
                    Directory.CreateDirectory(logCargoDirPath);
                
                foreach(Log.IActivityLog log in logCargo.GetLogs())
                {
                    bs.Savepath = $@"{logCargoDirPath}/{log.Time.ToDefault()}.dat";
                    bs.TransferToBinary(log);
                }

            }
        }
        public void PullAwayPrivateCargos() //Save as Cargo
        {
            if (!Directory.Exists(privateDftRootPath)) //Create hidden root folder for private loadables
                Directory.CreateDirectory(privateDftRootPath).Attributes = FileAttributes.Hidden;
            var bs = new BinarySaver();
            while (cargos.Count > 0)
            {
                ILoadable cargo = cargos.Pop();
                if (!Directory.Exists(privateSavepathByCargo[cargo.Type]))
                    Directory.CreateDirectory(privateSavepathByCargo[cargo.Type]).Attributes = FileAttributes.Hidden;

                if (cargo.PrimaryTime == null)
                    cargo.SetPrimaryTimeOnce(DateTime.Now); //Must be preserve!

                bs.Savepath = $@"{privateSavepathByCargo[cargo.Type]}/{cargo.PrimaryTime.GetValueOrDefault().ToDefault()}.dat";
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

            
        }
        public async Task PullAwayAsync()
        {
            var pullingPublic = Task.Run(new Action(() => PullAwayPublicLogs()));
            var pullingPrivate = Task.Run(new Action(() => PullAwayPrivateCargos() ));
            PullAwayCargoReports();

            await pullingPublic;
            await pullingPrivate;
        }
        #endregion
        /* 
         
         */
    }
}
