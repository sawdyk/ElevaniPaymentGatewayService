using AutoMapper;
using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Enums;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Helpers;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IBaseRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTUtilityService _jwtUtilityService;
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;
        private readonly IActivityLoggerService _activityLoggerService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IdentityConfig _identityConfig;
        public AuthenticationService(IBaseRepository<User> userRepository,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager,
            ILogger<AuthenticationService> logger, IJWTUtilityService jwtUtilityService, IMapper mapper, IOptions<JwtConfig> jwtConfig,
            IActivityLoggerService activityLoggerService, IHttpContextAccessor httpContextAccessor,
            IOptions<IdentityConfig> identityConfig)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtUtilityService = jwtUtilityService;
            _mapper = mapper;
            _jwtConfig = jwtConfig.Value;
            _activityLoggerService = activityLoggerService;
            _httpContextAccessor = httpContextAccessor;
            _identityConfig = identityConfig.Value;
        }

        public async Task<GenericResponse<LoginResponse>> AdminLoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.EmailAddress);
                if (user is null) throw new NotFoundException(RespMsgConstants.InvalidUsernamePassword);

                if (user.Status == UserStatus.Inactive) throw new GenericException(RespMsgConstants.InActiveUser);
                if (user.EmailConfirmed is false) throw new GenericException(RespMsgConstants.EmailConfirmationError);

                var signIn = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
                if (signIn.IsLockedOut) throw new GenericException(RespMsgConstants.SignInLockedOut.Replace("{minutes}", _identityConfig.DefaultLockoutTimeSpan.ToString()));
                if (!signIn.Succeeded) throw new GenericException(RespMsgConstants.InvalidUsernamePassword);

                var userRoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                if (string.IsNullOrEmpty(userRoleName)) throw new GenericException(RespMsgConstants.InvalidUsernamePassword);

                //var rolePermissions = new List<RolePermissionResponse>();
                var role = await _roleManager.FindByNameAsync(userRoleName);
                if (role.Name.Equals(nameof(UserRoles.Merchant))) throw new GenericException(RespMsgConstants.InvalidUsernamePassword);

                //if (role != null)
                //{
                //    var permissions = (await _rolePermissionQuery.ListAsync(x => x.RoleId == role.Id, true)).ToList();
                //    if (permissions != null && permissions.Count > 0)
                //    {
                //        foreach (var permission in permissions)
                //        {
                //            rolePermissions.Add(new RolePermissionResponse
                //            {
                //                PermissionGroup = permission.Permissions.PermissionGroups.Name,
                //                PermissionName = permission.Permissions.Name,
                //            });
                //        }
                //    }
                //}

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Role = _mapper.Map<RoleDto>(role);

                var accessToken = await _jwtUtilityService.GenerateAccessToken(userDto);
                var refreshToken = _jwtUtilityService.GenerateRefreshToken();

                user.LastLoginDate = DateTime.Now;
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiration = DateTime.Now.AddMinutes(_jwtConfig.RefreshExpiration);

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                LoginResponse loginResponse = new LoginResponse
                {
                    AccessToken = accessToken.Token,
                    ExpiresIn = accessToken.ExpirationTime,
                    RefreshToken = refreshToken,
                    UserDetails = userDto,
                    //RolePermissions = rolePermissions
                };

                //audit log goes here
                await _activityLoggerService.LogUserLoginActivityAsync(new AuditTrail
                {
                    UserId = user.Id,
                    IPAddress = _httpContextAccessor!.HttpContext!.Connection!.RemoteIpAddress!.ToString(),
                    Activity = ActivityTypes.Login,
                    ActivityDetails = $"Logged in from {_httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress}",
                });
                _logger.LogInformation($"User {user.FirstName} logged in successfully on {DateTime.Now}");

                return GenericResponse<LoginResponse>.Success(loginResponse, "Login successful");
            }
            catch (Exception ex)
             when (ex is GenericException || ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured on admin login >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<LoginResponse>> MerchantLoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.EmailAddress);
                if (user is null) throw new NotFoundException(RespMsgConstants.InvalidUsernamePassword);

                if (user.Status == UserStatus.Inactive) throw new GenericException(RespMsgConstants.InActiveUser);
                if (user.EmailConfirmed is false) throw new GenericException(RespMsgConstants.EmailConfirmationError);

                var signIn = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
                if (signIn.IsLockedOut) throw new GenericException(RespMsgConstants.SignInLockedOut.Replace("{minutes}", _identityConfig.DefaultLockoutTimeSpan.ToString()));
                if (!signIn.Succeeded) throw new GenericException(RespMsgConstants.InvalidUsernamePassword);

                var userRoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                if (string.IsNullOrEmpty(userRoleName)) throw new GenericException(RespMsgConstants.InvalidUsernamePassword);

                //var rolePermissions = new List<RolePermissionResponse>();
                var role = await _roleManager.FindByNameAsync(userRoleName);
                if (!role.Name.Equals(nameof(UserRoles.Merchant))) throw new GenericException(RespMsgConstants.InvalidUsernamePassword);

                //if (role != null)
                //{
                //    var permissions = (await _rolePermissionQuery.ListAsync(x => x.RoleId == role.Id, false)).ToList();
                //    if (permissions != null && permissions.Count > 0)
                //    {
                //        foreach (var permission in permissions)
                //        {
                //            rolePermissions.Add(new RolePermissionResponse
                //            {
                //                PermissionGroup = permission.Permissions.PermissionGroups.Name,
                //                PermissionName = permission.Permissions.Name,
                //            });
                //        }
                //    }
                //}

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Role = _mapper.Map<RoleDto>(role);

                var accessToken = await _jwtUtilityService.GenerateAccessToken(userDto);
                var refreshToken = _jwtUtilityService.GenerateRefreshToken();

                user.LastLoginDate = DateTime.Now;
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiration = DateTime.Now.AddMinutes(_jwtConfig.RefreshExpiration);

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                LoginResponse loginResponse = new LoginResponse
                {
                    AccessToken = accessToken.Token,
                    ExpiresIn = accessToken.ExpirationTime,
                    RefreshToken = refreshToken,
                    UserDetails = userDto,
                    //RolePermissions = rolePermissions
                };

                //audit log goes here
                await _activityLoggerService.LogUserLoginActivityAsync(new AuditTrail
                {
                    UserId = user.Id,
                    IPAddress = _httpContextAccessor!.HttpContext!.Connection!.RemoteIpAddress!.ToString(),
                    Activity = ActivityTypes.Login,
                    ActivityDetails = $"Logged in from {_httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress}",
                });
                _logger.LogInformation($"User {user.FirstName} logged in successfully on {DateTime.Now}");

                return GenericResponse<LoginResponse>.Success(loginResponse, "Login successful");
            }
            catch (Exception ex)
             when (ex is GenericException || ex is NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured on merchant login >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<GenericResponse<LoginResponse>> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                var loginResponse = await _jwtUtilityService.RefreshToken(request);
                return GenericResponse<LoginResponse>.Success(loginResponse, "Login successful");
            }
            catch (Exception ex)
            when (ex is GenericException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured at refresh token >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }
    }
}
