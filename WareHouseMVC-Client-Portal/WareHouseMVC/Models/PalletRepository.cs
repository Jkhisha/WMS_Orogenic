using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class PalletRepository : IPalletRepository
    {
        WareHouseMVCContext context;

         public  PalletRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public PalletRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<Pallet> All
        {
            get { return context.Pallets; }
        }

        public IQueryable<Pallet> AllIncluding(params Expression<Func<Pallet, object>>[] includeProperties)
        {
            IQueryable<Pallet> query = context.Pallets;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<Pallet> allList(params Expression<Func<Pallet, object>>[] includeProperties)
        {
            IQueryable<Pallet> query = context.Pallets;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty).Take(10);
            }
            return query;
        }

        public Pallet Find(long id)
        {
            return context.Pallets.Find(id);
        }

        public void InsertOrUpdate(Pallet pallet)
        {
            if (pallet.PalletId == default(long)) {
                // New entity
                context.Pallets.Add(pallet);
            } else {
                // Existing entity
                context.Entry(pallet).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var pallet = context.Pallets.Find(id);
            context.Pallets.Remove(pallet);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

        public Pallet GetPalletList(bool status, long _wID, int count, long clientID)
        {
            Pallet finalPallet = new Pallet();


            #region Check Zone Assigned or not

            List<AutoZoneSuggention> _autoZoneList = context.AutoZoneSuggentions.Where(a => a.ClientID == clientID && a.Zone.WarehouseID == _wID).ToList();

            long tempZoneID = 0;

            if (_autoZoneList.Count > 0)
            {

                foreach (var item in _autoZoneList)
                {
                    tempZoneID = item.ZoneID;
                    Pallet pallet = new Pallet();
                    pallet = context.Pallets.Where(r => r.ZoneID == item.ZoneID).FirstOrDefault();

                    finalPallet = pallet;

                }




            }
            else
            {
                    finalPallet = context.Pallets.Where(r =>r.WarehouseID == _wID).FirstOrDefault();
                

            }


            #endregion


            //return context.Rows.Where(r => r.IsAssigned == status && r.WarehouseID==_wID).Take(count).ToList();

            return finalPallet;


        }

        public List<Pallet> GetByZoneID(long ZoneID)
        {
            return context.Pallets.Where(p => p.ZoneID == ZoneID).ToList();
        }

        public  Pallet GetByZoneIDPallet(long _tempZoneID)
        {
            return context.Pallets.Where(p => p.ZoneID == _tempZoneID).FirstOrDefault();
        }

        public List<Pallet> allList(long ZoneID)
        {
            return context.Pallets.Where(p => p.ZoneID == ZoneID).ToList();
        }



        public  Pallet GetBywareHouseIDPallet(long pt)
        {
            return context.Pallets.Where(p => p.WarehouseID == pt).FirstOrDefault();
        }
    }

    public interface IPalletRepository : IDisposable
    {
        IQueryable<Pallet> All { get; }
        IQueryable<Pallet> AllIncluding(params Expression<Func<Pallet, object>>[] includeProperties);
        Pallet Find(long id);
        void InsertOrUpdate(Pallet pallet);
        void Delete(long id);
        void Save();
    }
}