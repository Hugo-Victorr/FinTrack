using FinTrack.Database.Repository;
using FinTrack.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace FinTrack.Database.EFDao
{
    public abstract class BaseDao<TEntity>(FintrackDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected FintrackDbContext _context = context;
        //protected IRepositoryCache<TEntity>? _cache = cache;

        protected abstract Task ValidateEntityForInsert(params TEntity[] obj);

        protected abstract Task ValidateEntityForUpdate(params TEntity[] obj);

        //protected abstract IRepositoryCache<TEntity>? GetCache();

        public virtual async Task<int> AddAsync(params TEntity[] obj)
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            await ValidateEntityForInsert(obj);
            dbSet.AddRange(obj);
            int result = await _context.SaveChangesAsync();
            return result;
        }

        public virtual async Task<int> UpdateAsync(params TEntity[] obj)
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            foreach (TEntity entity in obj)
            {
                TEntity? existingEntity = await dbSet
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id.Equals(entity.Id) && e.DeletedAt == null);

                if (existingEntity == null)
                    continue;

                await ValidateEntityForUpdate(entity);
                entity.UpdatedAt = DateTime.Now.ToUniversalTime();
                _ = dbSet.Update(entity);
            }

            int result = await _context.SaveChangesAsync();

            //if (result > 0)
            //    _cache?.RemoveEntity(obj);

            return result;
        }

        public virtual async Task<List<TEntity>> AllAsync(bool track = false)
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            List<TEntity> list = track ?
                await dbSet.Where(d => d.DeletedAt == null).ToListAsync() :
                await dbSet.Where(d => d.DeletedAt == null).AsNoTracking().ToListAsync();
            return list;
        }

        public virtual async Task<List<TEntity>> AllAsync(
                QueryOptions options,
                bool track = false,
                params Expression<Func<TEntity, object>>[] includes)
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            IQueryable<TEntity> query = dbSet.Where(d => d.DeletedAt == null);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Apply filters dynamically
            if (options.Filters is not null && options.Filters.Count > 0)
            {
                foreach (var (key, value) in options.Filters)
                {
                    PropertyInfo? prop = typeof(TEntity).GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (prop is not null)
                    {
                        var parameter = Expression.Parameter(typeof(TEntity), "e");
                        var property = Expression.Property(parameter, prop);
                        Expression filterExpression;

                        // Use Like for string properties, equality for others
                        if (prop.PropertyType == typeof(string))
                        {
                            // Build expression: e => EF.Functions.Like(e.Property, "%value%")
                            var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
                                nameof(DbFunctionsExtensions.Like),
                                new[] { typeof(DbFunctions), typeof(string), typeof(string) }
                            )!;
                            var functions = Expression.Property(null, typeof(EF), nameof(EF.Functions));
                            var pattern = Expression.Constant($"%{value}%");
                            filterExpression = Expression.Call(likeMethod, functions, property, pattern);
                        }
                        else
                        {
                            // For non-string types, use equality comparison
                            object? convertedValue = null;
                            try
                            {
                                // Try to convert the string value to the property type
                                if (prop.PropertyType == typeof(Guid))
                                {
                                    convertedValue = Guid.Parse(value);
                                }
                                else if (prop.PropertyType.IsEnum)
                                {
                                    convertedValue = Enum.Parse(prop.PropertyType, value, true);
                                }
                                else
                                {
                                    convertedValue = Convert.ChangeType(value, prop.PropertyType);
                                }
                            }
                            catch
                            {
                                // If conversion fails, skip this filter
                                continue;
                            }

                            if (convertedValue != null)
                            {
                                var constant = Expression.Constant(convertedValue, prop.PropertyType);
                                filterExpression = Expression.Equal(property, constant);
                            }
                            else
                            {
                                continue;
                            }
                        }

                        var lambda = Expression.Lambda<Func<TEntity, bool>>(filterExpression, parameter);
                        query = query.Where(lambda);
                    }
                }
            }

            // Apply sorting dynamically
            if (!string.IsNullOrWhiteSpace(options._Sort))
            {
                var prop = typeof(TEntity).GetProperty(options._Sort,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "e");
                    var property = Expression.Property(parameter, prop);
                    var keySelector = Expression.Lambda(property, parameter);

                    if (options._Order != null && options._Order.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                        query = Queryable.OrderByDescending(query, (dynamic)keySelector);
                    else
                        query = Queryable.OrderBy(query, (dynamic)keySelector);
                }
            }

            // Apply pagination
            if (options._Start.HasValue)
                query = query.Skip(options._Start.Value);
            if (options._End.HasValue)
                query = query.Take(options._End.Value);

            if (!track)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async Task<int> DeleteAsync(params TEntity[] obj)
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            foreach (TEntity item in obj)
            {
                item.DeletedAt = item.UpdatedAt = DateTime.Now.ToUniversalTime();
            }
            dbSet.UpdateRange(obj);
            int result = await _context.SaveChangesAsync();

            //if (result > 0)
            //    _cache?.RemoveEntity(obj);

            return result;
        }

        public virtual async Task<int> DeleteAsync(params object[] keys)
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            foreach (object item in keys)
            {
                TEntity? entity = await dbSet.FirstOrDefaultAsync(p => p.Id.Equals(item));
                if (entity != null)
                {
                    entity.DeletedAt = entity.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _ = dbSet.Update(entity);
                    //_cache?.RemoveEntity(entity);
                }
            }
            int result = await _context.SaveChangesAsync();
            return result;
        }

        public virtual async Task<TEntity?> FindAsync(
            object key,
            bool track = false,
            params Expression<Func<TEntity, object>>[] includes)
        {
            TEntity? entity = null;

            //if (key != null && _cache != null && !track)
            //{
            //    entity = _cache.GetEntity(key.ToString()!);
            //    if (entity != null)
            //        return entity;
            //}

            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            entity = track ?
                await dbSet.FindAsync(key) :
                await dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.Id.Equals(key));

            IQueryable<TEntity> query = dbSet.Where(e => e.DeletedAt == null);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (!track)
                query = query.AsNoTracking();

            entity = await query.FirstOrDefaultAsync(e => e.Id.Equals(key));

            return entity;
        }

        public async Task<int> RestoreAsync(params TEntity[] obj)
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            foreach (TEntity item in obj)
            {
                item.DeletedAt = null;
                item.UpdatedAt = DateTime.Now.ToUniversalTime();
            }
            dbSet.UpdateRange(obj);
            int result = await _context.SaveChangesAsync();

            //if (result > 0)
            //    _cache?.RemoveEntity(obj);

            return result;
        }
    }
}
