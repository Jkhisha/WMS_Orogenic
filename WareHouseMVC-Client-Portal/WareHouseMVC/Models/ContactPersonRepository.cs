using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class ContactPersonRepository : IContactPersonRepository
    {
        WareHouseMVCContext context;

         public  ContactPersonRepository()
            : this(new WareHouseMVCContext())
        {
            
        }
         public ContactPersonRepository(WareHouseMVCContext context)
        {
            
            this.context = context;
        }



        public IQueryable<ContactPerson> All
        {
            get { return context.ContactPersons; }
        }

        public IQueryable<ContactPerson> AllIncluding(params Expression<Func<ContactPerson, object>>[] includeProperties)
        {
            IQueryable<ContactPerson> query = context.ContactPersons;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public ContactPerson Find(long id)
        {
            return context.ContactPersons.Find(id);
        }

        public void InsertOrUpdate(ContactPerson contactperson)
        {
            if (contactperson.ContactPersontID == default(long)) {
                // New entity
                context.ContactPersons.Add(contactperson);
            } else {
                // Existing entity
                context.Entry(contactperson).State = EntityState.Modified;
            }
        }

        public void Delete(long id)
        {
            var contactperson = context.ContactPersons.Find(id);
            context.ContactPersons.Remove(contactperson);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }

       


        public List<ContactPerson> FindByDepartmentID(long DepartmentID)
        {
            return context.ContactPersons.Where(d => d.DepartmentID == DepartmentID).ToList();
        }

        public ContactPerson GetByCIDandDeptIDandContactName(long _tempClientID, long _tempDepartmentID, string contactPerson)
        {
            return context.ContactPersons.Where(c => c.ClientID == _tempClientID && c.DepartmentID == _tempDepartmentID && c.ContactPersonName == contactPerson).FirstOrDefault();
        }

        public  List<ContactPerson> GetByClientId(long clientId)
        {
            return context.ContactPersons.Where(c=>c.ClientID==clientId).OrderByDescending(c=>c.ContactPersontID).ToList();
        }

        public ContactPerson GetByClientAndDepartment(long _tempClientID, long _tempDepartmentID)
        {
            return context.ContactPersons.Where(c => c.ClientID == _tempClientID && c.DepartmentID == _tempDepartmentID).OrderByDescending(x => x.ContactPersontID).FirstOrDefault();
        }
    }

    public interface IContactPersonRepository : IDisposable
    {
        IQueryable<ContactPerson> All { get; }
        IQueryable<ContactPerson> AllIncluding(params Expression<Func<ContactPerson, object>>[] includeProperties);
        ContactPerson Find(long id);
        void InsertOrUpdate(ContactPerson contactperson);
        void Delete(long id);
        void Save();
    }
}