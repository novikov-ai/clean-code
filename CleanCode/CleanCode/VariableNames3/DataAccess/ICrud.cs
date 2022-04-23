using System;
using System.Collections.Generic;

namespace CleanCode.VariableNames3.DataAccess
{
    public interface ICrud<T>
    {
        public void Create(T record);
        public List<T> Read();
        public void Update();
        public bool Delete(T record);
        public List<T> Including(params string[] properties);
    }
}
