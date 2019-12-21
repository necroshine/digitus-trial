﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Digitus.Trial.Backend.Api.Security.Interfaces
{
    public interface IDatabaseProvider<T> where T: class
    {
        Task<T> GetById(int id);
        Task<IList<T>> GetAllAsync();
        Task<T> Add(T item);
        Task<bool> AddRange(IList<T> items);
        Task<T> Update(T item);
        Task<T> Delete(T item);
        Task<T> DeleteById(int id);
        Task<IList<T>> GetByFilter(dynamic filter);
    }
}
