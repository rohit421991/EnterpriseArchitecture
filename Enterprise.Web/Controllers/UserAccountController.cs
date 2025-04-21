using DTO.UserAccount;
using Enterprise.Contract;
using Enterprise.Manager;
using Enterprise.Manager.Auth;
using Enterprise.Manager.User;
using Enterprise.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Enterprise.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly IUserAccountService _userAccountService;
        public UserAccountController(TokenService tokenService, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserAccountService userAccountService)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _userAccountService = userAccountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            LoginResponseModel loginResponseModel = new LoginResponseModel();
            loginResponseModel.Token = string.Empty;

            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    loginResponseModel.LogInStatus = "Fail";
                    loginResponseModel.Message = "Invalid UserName/Password";
                    return Ok(loginResponseModel);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "NoRole";
                //var result = await _signInManager.PasswordSignInAsync(user, model.Password,false, false);
                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    #region Generate token

                    loginResponseModel.Token = _tokenService.GenerateToken(user.UserName, role);

                    #endregion

                    if (!string.IsNullOrEmpty(loginResponseModel.Token))
                    {
                        loginResponseModel.LogInStatus = "Success";
                        loginResponseModel.Message = "User is Authenticated successfully.";
                    }
                    else
                    {
                        loginResponseModel.LogInStatus = "Fail";
                        loginResponseModel.Message = "User is Authenticated successfully. Failed to generate token";
                    }
                }
                else
                {
                    loginResponseModel.LogInStatus = "Fail";
                    loginResponseModel.Message = "Invalid UserName/Password";
                }
                return Ok(loginResponseModel);
            }
            catch (Exception ex)
            {
                loginResponseModel.LogInStatus = "Error";
                loginResponseModel.Message = "Exception Occured";
                return Ok(loginResponseModel);
            }
        }

        [HttpPost]
        [Route("RegisterUser/")]
        public async Task<IActionResult> Register([FromBody] RegisterViewRequestModel model)
        {
            RegisterResponseModel objRegisterResponseModel = new RegisterResponseModel();

            try
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                {
                    objRegisterResponseModel.Status = "Fail";
                    objRegisterResponseModel.Message = "UserName cannot be null or empty!";
                    return Ok(objRegisterResponseModel);
                }

                var userExists = await _userManager.FindByNameAsync(model.UserName);
                if (userExists != null)
                {
                    objRegisterResponseModel.Status = "Fail";
                    objRegisterResponseModel.Message = "UserName " + model.UserName + " is already exists!";
                    return Ok(objRegisterResponseModel);
                }

                if (string.IsNullOrWhiteSpace(model.Email))
                {
                    objRegisterResponseModel.Status = "Fail";
                    objRegisterResponseModel.Message = "Email cannot be null or empty!";
                    return Ok(objRegisterResponseModel);
                }

                var emailExists = await _userManager.FindByEmailAsync(model.Email);
                if (emailExists != null)
                {
                    objRegisterResponseModel.Status = "Fail";
                    objRegisterResponseModel.Message = "Email " + model.Email + " is already exists!";
                    return Ok(objRegisterResponseModel);
                }

                var RoleModel = await _userAccountService.GetRoleByRoleName(model.RoleName);
                if (RoleModel == null)
                {
                    objRegisterResponseModel.Status = "Fail";
                    objRegisterResponseModel.Message = "Role " + model.RoleName + " doesnot exists!";
                    return Ok(objRegisterResponseModel);
                }

                if (string.IsNullOrEmpty(Convert.ToString(RoleModel.RoleId)))
                {
                    objRegisterResponseModel.Status = "Fail";
                    objRegisterResponseModel.Message = "Role " + model.RoleName + " doesnot exists!";
                    return Ok(objRegisterResponseModel);
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.MobileNumber,
                    ProfilePicURL = "\\:C",
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = true,
                    AddedBy = model.AddedBy,
                    AddedOn = DateTime.UtcNow,
                    UpdatedBy = model.AddedBy,
                    UpdatedOn = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserRoleModel userRoleModel = new UserRoleModel();
                    userRoleModel.UserId = user.Id;
                    userRoleModel.RoleId = RoleModel.RoleId;
                    userRoleModel.IsActive = true;

                    bool roleresult = await _userAccountService.AddUserRole(userRoleModel);
                    if (roleresult)
                    {
                        objRegisterResponseModel.Status = "Success";
                        objRegisterResponseModel.Message = "User registered successfully!";
                        objRegisterResponseModel.Password = model.Password;
                        return Ok(objRegisterResponseModel);
                    }
                    else
                    {
                        objRegisterResponseModel.Status = "Fail";
                        objRegisterResponseModel.Message = "Failed to register user!";
                        return Ok(objRegisterResponseModel);
                    }
                }
                else
                {
                    objRegisterResponseModel.Status = "Fail";
                    objRegisterResponseModel.Message = "User creation failed! Please check user details and try again.";
                    return Ok(objRegisterResponseModel);
                }
            }
            catch (Exception ex)
            {
                objRegisterResponseModel.Status = "Error";
                objRegisterResponseModel.Message = "User registration failed. Some error occured";
                return Ok(objRegisterResponseModel);
            }
        }
    }
}