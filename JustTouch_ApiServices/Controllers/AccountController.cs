using JustTouch_ApiServices.Helpers;
using JustTouch_ApiServices.Services;
using JustTouch_ApiServices.SupabaseService;
using JustTouch_Shared.Dtos;
using JustTouch_Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustTouch_ApiServices.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ISupabaseRepository supabase;
        private readonly MailService mailing;

        public AccountController(ISupabaseRepository _supabase, MailService _mailing)
        {
            mailing = _mailing;
            supabase = _supabase;
        }


        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(UserDto userDto)
        {
            try
            {

                var user = Mapper.Map<UserDto, Users>(userDto);
                var signedIn = await supabase.SignIn(user);
                if (signedIn == null)
                {
                    var error = new ErrorResponse()
                    {
                        Message = "No se encontro un usuario registrado",
                        StatusCode = 404
                    };
                    return NotFound(error);
                }
                var session = new SessionDto()
                {
                    email = user.Email,
                    accessToken = signedIn.Session!.AccessToken!,
                    refreshToken = signedIn.Session.RefreshToken!,
                    expiresIn = signedIn.Session.ExpiresIn,
                    expiresAt = signedIn.Session.ExpiresAt,
                    tokenType = signedIn.Session.TokenType!,
                };
                return Ok(session);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterService(UserDto newUser)
        {
            try
            {
                newUser.password = RandomString.GenerateRandomString();
                newUser.accountKey = RandomString.GenerateRandomString();
                var data =  Mapper.Map<UserDto, Users>(newUser);
                var user = await supabase.Insert(data);
                var error = new ErrorResponse()
                {
                    Message = "No pudimos registrar el usuario. Vuelva a intentar nuevamente o aguarde a que el problema se solucione.",
                    StatusCode = 400
                };
                if (user == null) return BadRequest(error);

                await mailing.SendWelcomeMail(user);
                var signup = await supabase.SignUp(user);
                if (!signup)
                {
                    await supabase.Delete<Users>(x => x.IdUser == user.IdUser);
                    return BadRequest(error);
                }

                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string Email)
        {
            try
            {
                var data = await supabase.GetBy<Users>(x => x.Email == Email);
                var user = data.FirstOrDefault();
                if (user == null)
                {
                    var error = new ErrorResponse()
                    {
                        Message = "No hemos podido identificar el usuario",
                        StatusCode = 400
                    };
                    return BadRequest(error);
                }

                user.IsConfirmed = true;
                var response = await supabase.Update(user, x => x.Email == Email);
                if (response == null)
                {
                    var error = new ErrorResponse()
                    {
                        Message = "Algo salio mal en la validación, por favor vuelva a intentar desde el enlace de registro",
                        StatusCode = 400
                    };
                    await supabase.Delete<Users>(x => x.IdUser == user.IdUser);
                    return BadRequest(error);
                }
                var signin = await supabase.SignIn(user);
                return Ok(signin);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet("{email}")]
        public async Task<IActionResult> GetAccountData(string email)
        {
            try
            {
                var data = await supabase.GetBy<Users>(x => x.Email == email);
                var user = data.FirstOrDefault();
                if (user == null)
                {
                    var error = new ErrorResponse()
                    {
                        Message = "No se encontro la cuenta",
                        StatusCode = 404
                    };
                    return BadRequest(error);
                }

                if (user.IsConfirmed == false)
                {
                    var error = new ErrorResponse()
                    {
                        Message = "Debe validar el mail para poder obtener la información de la cuenta",
                        StatusCode = 401
                    };
                    return BadRequest(error);
                }

                var userDto = Mapper.Map<Users, UserDto>(user);
                var franchises = new List<FranchiseDto>();
                var branches = new List<BranchDto>();

                foreach (var f in user.Franchises!)
                {
                    foreach (var b in f.Branches!)
                    {
                        var branchDto = Mapper.Map<Branch, BranchDto>(b);
                        branches.Add(branchDto);
                    }

                    var franchiseDto = Mapper.Map<Franchise, FranchiseDto>(f);
                    franchiseDto.branches = branches;
                    franchises.Add(franchiseDto);
                }

                var accountData = new AccountDto()
                {
                    userData = userDto,
                    franchises = franchises,
                };

                return Ok(accountData);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAccount(AccountDto account)
        {
            try
            {
                var user = Mapper.Map<UserDto, Users>(account.userData!);
                var updatedUser = await supabase.Update(user, x => x.Email == user.Email);
                var error = new ErrorResponse()
                {
                    Message = "No pudimos actualizar todos los datos de tu cuenta",
                    StatusCode = 400
                };

                if (updatedUser == null) return BadRequest(error);

                var newFranchises = account.franchises.Where(x => x.id == 0).ToList();
                var oldFranchises = account.franchises.Where(x => x.id == 1).ToList();

                if (newFranchises.Count > 0)
                {
                    foreach (var f in newFranchises)
                    {
                        var fr = Mapper.Map<FranchiseDto, Franchise>(f);
                        fr.UserId = user.IdUser;
                        fr.FranchiseCode = RandomString.GenerateRandomString();
                        var newFr = await supabase.Insert(fr);

                        if (newFr == null) return BadRequest(error);

                        foreach (var b in f.branches)
                        {
                            var br = Mapper.Map<BranchDto, Branch>(b);
                            br.FranchiseId = newFr.IdFranchise;
                            br.BranchCode = RandomString.GenerateRandomString();
                            var newBr = await supabase.Insert(br);
                            if (newBr == null) return BadRequest(error);
                        }

                    }
                }

                if (oldFranchises.Count > 0)
                {
                    foreach (var f in oldFranchises)
                    {
                        var fr = Mapper.Map<FranchiseDto, Franchise>(f);
                        var udFr = await supabase.Update(fr, x => x.FranchiseCode == fr.FranchiseCode);
                        if (udFr == null) return BadRequest(error);

                        var newBranches = f.branches.Where(x => x.id == 0).ToList();
                        var oldBranches = f.branches.Where(x => x.id == 1).ToList();

                        if (newBranches.Count > 0)
                        {
                            foreach (var b in newBranches)
                            {
                                var br = Mapper.Map<BranchDto, Branch>(b);
                                br.FranchiseId = udFr.IdFranchise;
                                br.BranchCode = RandomString.GenerateRandomString();

                                var newBr = await supabase.Insert(br);
                                if (newBr == null) return BadRequest(error);
                            }
                        }

                        if (oldBranches.Count > 0)
                        {
                            foreach (var b in oldBranches)
                            {
                                var br = Mapper.Map<BranchDto, Branch>(b);
                                var udBr = await supabase.Update(br, x => x.BranchCode == br.BranchCode);
                                if (udBr == null) return BadRequest(error);
                            }
                        }

                    }
                }

                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
