using Harbor.Cargo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harbor.Ship
{
    public abstract class Ship
    {
        public ReportFilter ReportCargoFilter { get; set; } = new ReportFilter();
        protected List<Report> reports= new List<Report>(); //separately save(private)
        protected List<Cargo.Cargo> cargos= new List<Cargo.Cargo>(); //separately save(private)

        /// <summary>
        /// If you inherit Ship, Thing you must override to work.
        /// </summary>
        /// <returns></returns>
        #region PullAway
        public abstract void PullAwayReports();
        public abstract void PullAwayCargos();
        public void SaveCargoAsBinaryFile(BinarySave bs, Cargo.Cargo cargo)
        {
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
        #endregion
        #region Loading
        public void LoadReport(Report report)
        {
            if (ReportCargoFilter.Validate(report))
                reports.Add(report);
            else
                throw new FilterException("This report doesn't meet LocalShip's filter that set");
        }
        public void LoadPrivate(Cargo.Cargo cargo)
        {
            if (cargo.IsEmpty())
                throw new CargoException(CargoExceptionMsg.Empty);
            if (cargo.IsLocked)
                cargos.Add(cargo);
            else
                throw new CargoException(CargoExceptionMsg.NotLocked);
        }
        #endregion
        #region Unloading - Report, ILoadable

        /// <summary>
        /// Return ILoadable at index and remove.
        /// </summary>
        /// <param name="index">The index that you will remove & get</param>
        /// <returns>The ILoadable you selected</returns>
        public Cargo.Cargo UnloadCargo(int index)
        {
            return cargos.Pop(index);
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
        #endregion
    }
}
