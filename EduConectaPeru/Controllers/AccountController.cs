using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EduConectaPeru.Models;
using EduConectaPeru.Repositories.Interfaces;

namespace EduConectaPeru.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IRepository<User> userRepository,
            ILogger<AccountController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Usuario y contraseña son obligatorios");
                return View();
            }

            try
            {
                var users = await _userRepository.GetAllAsync();
                var user = users.FirstOrDefault(u =>
                    u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                    u.IsActive);

                _logger.LogInformation($"Intento de login para usuario: {username}");

                if (user == null)
                {
                    _logger.LogWarning($"Usuario no encontrado o inactivo: {username}");
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                    return View();
                }

                bool passwordValid = VerifyPassword(password, user.PasswordHash);

                if (!passwordValid)
                {
                    _logger.LogWarning($"Contraseña incorrecta para usuario: {username}");
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                    return View();
                }

                // Crear claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, user.Role ?? "User"),
                    new Claim("UserId", user.UserId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                    AllowRefresh = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation($"Login exitoso para usuario: {username}");
                TempData["SuccessMessage"] = $"Bienvenido, {user.Username}";

         
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante el login para usuario: {username}");
                ModelState.AddModelError("", "Error al procesar el inicio de sesión. Intente nuevamente.");
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name ?? "Desconocido";
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation($"Usuario cerró sesión: {username}");
            TempData["SuccessMessage"] = "Sesión cerrada correctamente";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            if (!storedHash.Contains(":"))
            {
                _logger.LogWarning("Usuario usando contraseña en texto plano. Actualizar a hash.");
                return storedHash == password;
            }

            try
            {
                var parts = storedHash.Split(':');
                if (parts.Length != 2)
                    return false;

                var salt = parts[0];
                var hash = parts[1];

                var computedHash = HashPassword(password, salt);
                return computedHash == storedHash;
            }
            catch
            {
                return false;
            }
        }

        public static string HashPassword(string password, string salt = null)
        {
            
            if (string.IsNullOrEmpty(salt))
            {
                salt = GenerateSalt();
            }

            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = salt + password;
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                var hash = Convert.ToBase64String(hashBytes);
                return $"{salt}:{hash}";
            }
        }

        private static string GenerateSalt()
        {
            var saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

    }
}