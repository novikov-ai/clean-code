using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanCode.CtorInterfaceNames.DAL
{
    public class DataAccess<T> : DataCrud<T>
        where T : class
    {
        private readonly DbContext _context;

        public DataAccess(DbContext dbContext)
        {
            _context = dbContext;
        }

        public void Create(T record)
        {
            var table = _context.Set<T>();
            table.Add(record);

            Update();
        }

        public List<T> Read()
        {
            var retrievedData = new List<T>();

            var table = _context.Set<T>();
            foreach (T item in table)
                retrievedData.Add(item);

            return retrievedData;
        }

        public void Update()
        {
            _context.SaveChanges();
        }

        public bool Delete(T record)
        {
            var isDeleted = false;

            try
            {
                _context.Set<T>().Remove(record);

                Update();

                isDeleted = true;
            }
            catch (Exception e)
            {
                isDeleted = false;
            }

            return isDeleted;
        }

        public List<T> Including(params string[] properties)
        {
            try
            {
                var uniqueProperties = new HashSet<string>();

                foreach (var property in properties)
                    uniqueProperties.Add(property);

                return _context.Set<T>().Include(string.Join('.', properties)).ToList();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }
    }
}