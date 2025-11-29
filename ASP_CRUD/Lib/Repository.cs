using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASP_CRUD.Lib
{
    public class Repository
    {
        private readonly DbContext _dbContext;
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<TEntity> GetNEntities<TEntity>(int count) where TEntity : class
        {
            try
            {
                return _dbContext.Set<TEntity>().Take(count).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entities: " + ex.Message);
            }
        }
        public List<TEntity> GetEntities<TEntity>() where TEntity : class
        {
            try
            {
                return _dbContext.Set<TEntity>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entities: " + ex.Message);
            }
        }
        public TEntity GetEntityById<TEntity>(object id) where TEntity : class
        {
            try
            {
                return _dbContext.Set<TEntity>().Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving entity by ID: " + ex.Message);
            }
        }
        public bool DeleteEntity<TEntity>(object id) where TEntity : class
        {
            try
            {
                TEntity entity = _dbContext.Set<TEntity>().Find(id) ?? throw new Exception("Entity not found.");
                _dbContext.Set<TEntity>().Remove(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting entity: " + ex.Message);
            }
        }
        public bool DeleteEntity<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dbContext.Set<TEntity>().Remove(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting entity: " + ex.Message);
            }
        }
        public TEntity AddEntity<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entity);
                _dbContext.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding entity: " + ex.Message);
            }
        }
    } 
}