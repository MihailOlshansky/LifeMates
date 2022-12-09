using LifeMates.Domain.Models.Users;
using LifeMates.Infrastructure.Constant;
using LifeMates.Storage.SharedKernel.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LifeMates.Storage.Repositories;

public class UserCredentialsRepository : IUserCredentialsRepository
{
    private readonly UserManager<UserCredentials> _userManager;
    private readonly SignInManager<UserCredentials> _signInManager;
    private readonly LifematesDbContext _dbContext;

    public UserCredentialsRepository(
        UserManager<UserCredentials> userManager, 
        SignInManager<UserCredentials> signInManager, LifematesDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }

    public Task<bool> Exists(string email, CancellationToken cancellationToken)
    {
        return _dbContext.UsersCredentials.AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<UserCredentials?> FindByEmail(string email, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        var credentials = await _userManager.FindByEmailAsync(email);
        
        return credentials;
    }

    public async Task<UserCredentials> FindById(string id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        
            var credentials = await _userManager.FindByIdAsync(id);

            return credentials;
        }

    public async Task<bool> CheckPasswordSignIn(UserCredentials userCredentials, string password, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        var result = await _signInManager.CheckPasswordSignInAsync(userCredentials, password, false);

        return result.Succeeded;
    }

    public Task SetRefreshToken(UserCredentials userCredentials, string refreshToken, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        return _userManager.SetAuthenticationTokenAsync(
            userCredentials,
            Constants.Tokens.Providers.LifeMates,
            Constants.Tokens.Types.RefreshToken,
            refreshToken);
    }

    public async Task<bool> RemoveRefreshToken(UserCredentials userCredentials, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        var result = await _userManager.RemoveAuthenticationTokenAsync(
            userCredentials,
            Constants.Tokens.Providers.LifeMates,
            Constants.Tokens.Types.RefreshToken);

        return result.Succeeded;
    }

    public async Task<string> GetRefreshToken(UserCredentials userCredentials, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
        
        var currentRefreshToken = await _userManager.GetAuthenticationTokenAsync(
            userCredentials,
            Constants.Tokens.Providers.LifeMates,
            Constants.Tokens.Types.RefreshToken);

        return currentRefreshToken;
    }

    public async Task<bool> Create(UserCredentials userCredentials, string password, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
        
        var result = await _userManager.CreateAsync(userCredentials, password);

        return result.Succeeded;
    }

    public Task<bool> IsInRole(UserCredentials userCredentials, string role, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }

        return _userManager.IsInRoleAsync(userCredentials, role);
    }
}