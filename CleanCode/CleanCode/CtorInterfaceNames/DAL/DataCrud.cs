using System;
using System.Collections.Generic;

namespace CleanCode.CtorInterfaceNames.DAL
{
    // 3.2 (2)
    // ICrud - DataCrud
    public interface DataCrud<T>
    {
        public void Create(T record);
        public List<T> Read();
        public void Update();
        public bool Delete(T record);
        public List<T> Including(params string[] properties);
    }
}
