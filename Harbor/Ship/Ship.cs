using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harbor.Ship
{
    public abstract class Ship
    {
        protected List<Cargo.DataCargo> publicCargos = new List<Cargo.DataCargo>(); //separately save(private)
        protected List<Cargo.Cargo> privateCargos = new List<Cargo.Cargo>();
        /// <summary>
        /// If you inherit Ship, Thing you must override to work.
        /// </summary>
        /// <returns></returns>
        #region PullAway
        public abstract void PullAwayPrivateCargos();

        #endregion
        #region Loading
        public void LoadPrivate(Cargo.Cargo cargo)
        {
            if (cargo.IsEmpty())
                throw new Cargo.CargoException(Cargo.CargoExceptionMsg.Empty);
            if (cargo.IsLocked)
                privateCargos.Add(cargo);
            else
                throw new Cargo.CargoException(Cargo.CargoExceptionMsg.NotLocked);
        }
        public void LoadPublic(Cargo.DataCargo cargo)
        {
            if (cargo.IsEmpty())
                throw new Cargo.CargoException(Cargo.CargoExceptionMsg.Empty);
            if (cargo.IsLocked)
                publicCargos.Add(cargo);
            else
                throw new Cargo.CargoException(Cargo.CargoExceptionMsg.NotLocked);
        }

        #endregion
        #region Unloading
        /// <summary>
        /// index and remove.
        /// </summary>
        /// <param name="index">Cargo belong to index, you will remove</param>
        public void UnloadPublicCargo(int index)
        {
            if (index < 0 || index >= publicCargos.Count()) return;

            publicCargos.RemoveAt(index);
        }
        public void UnloadPrivateCargo(int index)
        {
            if (index < 0 || index >= privateCargos.Count()) return;

            privateCargos.RemoveAt(index);
        }
        #endregion
    }
}
