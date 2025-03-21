using System;
using Core.Entities;

namespace Core.Interface;

public interface IGenericRepository<T> where T: BaseEntity
{
    Task<T?> GetByIdAsynd(int id);
    Task<IReadOnlyList<T>>ListAllAsyc();
    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);

    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool>SaveAllAsync();
    bool Exists(int id);
    Task<int>CountAsync(ISpecification<T> spec);

}
