using JustTouch_Shared.Auth;
using JustTouch_Shared.Models;
using Supabase.Postgrest.Models;
using System.Linq.Expressions;

namespace JustTouch_ApiServices.SupabaseService
{
    public interface ISupabaseRepository
    {
        Task<List<TEntity>> GetAll<TEntity>() where TEntity : BaseModel, new();

        Task<List<TEntity>> GetBy<TEntity>(Expression<Func<TEntity, bool>> func)where TEntity : BaseModel, new();

        Task<TEntity?> GetWith<TEntity>(Expression<Func<TEntity, bool>> func, string[] includes) where TEntity : BaseModel, new();

        Task<TEntity?> Insert<TEntity>(TEntity entity) where TEntity : BaseModel, new();

        Task<TEntity?> Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> func) where TEntity : BaseModel, new();

        Task Delete<TEntity>(Expression<Func<TEntity, bool>> func) where TEntity : BaseModel, new();


        Task<AuthResponse?> SignIn(Users user);
        Task<bool> SignUp(Users user);
        Task<string?> GetFromBucket(string bucket, string folder, string name);
        Task<string> UploadFile(string folder, Stream fileStream, string originalFileName);

        Task VoidRpc(string functionName, object payload);
        Task<TResponse?> ContentRpc<TResponse>(string functionName, object payload);
    }
}
