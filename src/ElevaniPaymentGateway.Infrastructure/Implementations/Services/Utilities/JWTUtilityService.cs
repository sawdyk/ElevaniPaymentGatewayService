using AutoMapper;
using ElevaniPaymentGateway.Core.Configs;
using ElevaniPaymentGateway.Core.Constants;
using ElevaniPaymentGateway.Core.Entities;
using ElevaniPaymentGateway.Core.Exceptions;
using ElevaniPaymentGateway.Core.Models;
using ElevaniPaymentGateway.Core.Models.Dto;
using ElevaniPaymentGateway.Core.Models.Request;
using ElevaniPaymentGateway.Core.Models.Response;
using ElevaniPaymentGateway.Infrastructure.Interfaces.EfRepository;
using ElevaniPaymentGateway.Infrastructure.Interfaces.Services.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ElevaniPaymentGateway.Infrastructure.Implementations.Services.Utilities
{
    public class JWTUtilityService : IJWTUtilityService
    {
        private readonly ILogger<JWTUtilityService> _logger;
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<User> _userRepository;
        public JWTUtilityService(ILogger<JWTUtilityService> logger, IOptions<JwtConfig> jwtConfig,
           UserManager<User> userManager, RoleManager<Role> roleManager,
           IMapper mapper, IBaseRepository<User> userRepository)
        {
            _logger = logger;
            _jwtConfig = jwtConfig.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<JwtResponse> GenerateMerchantAuthenticationAccessToken(Merchant merchant)
        {
            try
            {
                var claim = new[]
                {
                    new Claim(PaymentClaimTypesHelpers.MerchantId, merchant.Id),
                    new Claim(PaymentClaimTypesHelpers.Name, merchant.Name),
                    new Claim(PaymentClaimTypesHelpers.Slug, merchant.Slug),
                    new Claim(PaymentClaimTypesHelpers.PaymentGateway, merchant.PaymentGateway.ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiresIn = DateTime.Now.AddDays(_jwtConfig.AccessExpiration);

                var jwtToken = new JwtSecurityToken(
                    _jwtConfig.Issuer,
                    _jwtConfig.Audience,
                    claim,
                    expires: expiresIn,
                    signingCredentials: credentials
                );

                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                return new JwtResponse { Token = token, ExpirationTime = expiresIn };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<JwtResponse> GenerateAccessToken(UserDto user)
        {
            try
            {
                var claim = new[]
                {
                    new Claim(ClaimTypesHelpers.UserId, user.Id.ToString()),
                    new Claim(ClaimTypesHelpers.FirstName, user.FirstName),
                    new Claim(ClaimTypesHelpers.LastName, user.LastName),
                    new Claim(ClaimTypesHelpers.EmailAddress, user.Email),
                    new Claim(ClaimTypesHelpers.PhoneNumber, user.PhoneNumber),
                    new Claim(ClaimTypesHelpers.Role, user.Role.Name),
                    new Claim(ClaimTypesHelpers.Jti, Guid.NewGuid().ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiresIn = DateTime.Now.AddMinutes(_jwtConfig.AccessExpiration);

                var jwtToken = new JwtSecurityToken(
                    _jwtConfig.Issuer,
                    _jwtConfig.Audience,
                    claim,
                    expires: expiresIn,
                    signingCredentials: credentials
                );

                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                return new JwtResponse { Token = token, ExpirationTime = expiresIn };
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public async Task<LoginResponse> RefreshToken(RefreshTokenRequest request)
        {
            try
            {
                ClaimsPrincipal principal = GetPrincipalFromExpiredToken(request.AccessToken);
                string userId = principal.Claims.Where(x => x.Type == ClaimTypesHelpers.UserId).FirstOrDefault()?.Value ?? "";

                var user = await _userManager.FindByIdAsync(userId);
                if (user is null) throw new SecurityTokenException("Invalid access token");

                if (!user.RefreshToken.Equals(request.RefreshToken)) throw new SecurityTokenException("Invalid refresh token");
                // Ensure that the refresh token that we got from storage is not yet expired.
                if (DateTime.Now > user.RefreshTokenExpiration) throw new SecurityTokenException("Refresh token has expired");

                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                var customerRole = await _roleManager.FindByNameAsync(role);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Role = _mapper.Map<RoleDto>(customerRole);

                var accessToken = await GenerateAccessToken(userDto);
                var refreshToken = GenerateRefreshToken();

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
                    //UserPermissions = userPermissions
                };

                return loginResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured >> {ex.Message} | stack trace >> {ex.StackTrace} " +
                   $"| inner exception >> {ex.InnerException} | source >> {ex.Source}");
                throw new UnhandledException(RespMsgConstants.UnhandledException);
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key)),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
