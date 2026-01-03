using JustTouch_Shared.Auth;
using JustTouch_Shared.Models;
using Supabase;
using Supabase.Postgrest.Models;
using System.Linq.Expressions;
using static Supabase.Postgrest.Constants;

namespace JustTouch_ApiServices.SupabaseService
{
    public class SupabaseRepository : ISupabaseRepository
    {
        private readonly Client client;

        public SupabaseRepository(Client _client)
        {
            client = _client;
        }

        public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : BaseModel, new()
        {
            var response = await client
                .From<TEntity>()
                .Get();

            return response.Models;
        }
        public async Task<List<TEntity>> GetBy<TEntity>(Expression<Func<TEntity, bool>> func) where TEntity : BaseModel, new()
        {
            var response = await client.From<TEntity>()
                                       .Where(func)
                                       .Get();
            return response.Models;
        }
        public async Task<TEntity?> GetWith<TEntity>(Expression<Func<TEntity, bool>> func, string[] includes) where TEntity : BaseModel, new()
        {
            try
            {
                var includeQuery = SupabaseHelpers.BuildSelect(includes);
                var response = await client.From<TEntity>()
                                           .Select(includeQuery)
                                           .Where(func)
                                           .Get();
                return response.Models.FirstOrDefault();
            }
            catch (Exception)
            {
                return default;
            }
        }
        public async Task<TEntity?> Insert<TEntity>(TEntity entity) where TEntity : BaseModel, new()
        {
            var response = await client.From<TEntity>().Insert(entity);
            return response.Models.FirstOrDefault();
        }
        public async Task<TEntity?> Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> func) where TEntity : BaseModel, new()
        {
            var response = await client.From<TEntity>()
                                       .Where(func)
                                       .Update(entity);
            return response.Models.FirstOrDefault();
        }
        public async Task Delete<TEntity>(Expression<Func<TEntity, bool>> func) where TEntity : BaseModel, new() => await client.From<TEntity>().Where(func).Delete();
        public async Task<AuthResponse?> SignIn(Users user)
        {
            try
            {
                var session = await client.Auth.SignIn(email: user.Email, password: user.Password);
                if (session == null) return null;

                var userData = session!.User!;
                var auth = new AuthResponse()
                {
                    User = new User()
                    {
                        Id = userData.Id!,
                        Email = userData.Email,
                        CreatedAt = userData.CreatedAt,
                        UserMetadata = userData.UserMetadata
                    },
                    Session = new Session()
                    {
                        AccessToken = session.AccessToken,
                        RefreshToken = session.RefreshToken,
                        TokenType = session.TokenType,
                        ExpiresAt = session.ExpiresAt(),
                        ExpiresIn = session.ExpiresIn
                    }
                };

                return auth;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SignUp(Users user)
        {
            var session = await client.Auth.SignUp(email: user.Email, password: user.Password);
            if (session == null)
            {
                return false;
            }
            return true;
        }
        public async Task<string?> GetFromBucket(string bucket, string folder, string name)
        {
            try
            {
                string path = $"{folder.Trim('/')}/{name.TrimStart('/')}";
                var signedUrl = await client.Storage.From(bucket).CreateSignedUrl(path, 60 * 60 * 24);
                return signedUrl;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task VoidRpc(string functionName, object payload) => await client.Rpc(functionName, new { data = payload });
        public async Task<TResponse?> ContentRpc<TResponse>(string functionName, object payload)
        {
            var response = await client.Rpc<TResponse>(functionName, new { data = payload });
            return response;
        }
    }
}
