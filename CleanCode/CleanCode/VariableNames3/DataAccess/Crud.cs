using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanCode.VariableNames3.DataAccess
{
    public class Crud<T> : ICrud<T>
        where T : class
    {
        private readonly DbContext _context;

        public Crud(DbContext dbContext)
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
            // 7.5 (1) result - retrievedData 
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
            // 7.1 (4) result - isDeleted
            var isDeleted = false;

            try
            {
                // 7.5 (2) [variable table is useless]
                // var table = _context.Set<T>();
                // table.Remove(record);

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
                // 7.5 (3) set - uniqueProperties
                var uniqueProperties = new HashSet<string>();

                // 7.5 (4) prop - property
                foreach (var property in properties)
                    uniqueProperties.Add(property);

                // 7.5 (5) [variable output is useless]
                // string output = string.Join('.', properties);
                // return _context.Set<T>().Include(output).ToList();

                return _context.Set<T>().Include(string.Join('.', properties)).ToList();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }
    }
}