using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WareHouseMVC.Models
{ 
    public class UserRepository : IUserRepository
    {

        public UserRepository()
            : this(new WareHouseMVCContext())
        {

        }
        public UserRepository(WareHouseMVCContext context)
        {

            this.context = context;
        }
        // WareHouseMVCContext context = new WareHouseMVCContext();
        WareHouseMVCContext context;
        public IQueryable<User> All
        {
            get { return context.Users; }
        }

        public IQueryable<User> AllIncluding(params Expression<Func<User, object>>[] includeProperties)
        {
            IQueryable<User> query = context.Users;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public User Find(int id)
        {
            return context.Users.Find(id);
        }

        public void InsertOrUpdate(User User)
        {
            if (User.UserId == default(Guid))
            {
                // New entity
                context.Users.Add(User);
            }
            else
            {
                // Existing entity
                context.Entry(User).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var User = context.Users.Find(id);
            context.Users.Remove(User);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        internal dynamic GetAllUser()
        {
            return context.Users.Select(h => h.Username).ToList();
        }

        public  List<User> GetAll()
        {
            return context.Users.ToList();
        }

        public  User GetUserByUserName(string userName)
        {
            return context.Users.Where(u => u.Username == userName).FirstOrDefault();
        }




        public  User GetByUserId(Guid _uId)
        {
            return context.Users.Where(u => u.UserId == _uId).FirstOrDefault();
        }
    }

    public interface IUserRepository : IDisposable
    {
        IQueryable<User> All { get; }
        IQueryable<User> AllIncluding(params Expression<Func<User, object>>[] includeProperties);
        User Find(int id);
        void InsertOrUpdate(User Test);
        void Delete(int id);
        void Save();
        List<User> GetAll();
        User GetUserByUserName(string userName);
    }
}