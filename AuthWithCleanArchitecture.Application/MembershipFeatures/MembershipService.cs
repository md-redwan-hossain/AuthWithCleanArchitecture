using System.Security.Claims;
using AuthWithCleanArchitecture.Application.AuthCryptographyFeatures;
using AuthWithCleanArchitecture.Application.Common.Providers;
using AuthWithCleanArchitecture.Application.MembershipFeatures.DataTransferObjects;
using AuthWithCleanArchitecture.Application.MembershipFeatures.Outcomes;
using AuthWithCleanArchitecture.Domain.MembershipEntities;
using AuthWithCleanArchitecture.Domain.MembershipEntities.ValueObjects;
using Mapster;
using SharpOutcome;

namespace AuthWithCleanArchitecture.Application.MembershipFeatures;

public class MembershipService : IMembershipService
{
    private readonly IAppUnitOfWork _appUnitOfWork;
    private readonly IGuidProvider _guidProvider;
    private readonly IJwtProvider _jwtProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAuthCryptographyService _authCryptographyService;


    public MembershipService(IAppUnitOfWork appUnitOfWork, IGuidProvider guidProvider, IJwtProvider jwtProvider,
        IDateTimeProvider dateTimeProvider, IAuthCryptographyService authCryptographyService)
    {
        _appUnitOfWork = appUnitOfWork;
        _guidProvider = guidProvider;
        _jwtProvider = jwtProvider;
        _dateTimeProvider = dateTimeProvider;
        _authCryptographyService = authCryptographyService;
    }

    public async Task<ValueOutcome<AppUser, SignUpBadOutcome>> SignUpAsync(AppUserSignUpRequest dto)
    {
        var exists = await _appUnitOfWork.AppUserRepository.GetOneAsync(
            filter: x => x.UserName == dto.UserName,
            subsetSelector: x => x.UserName
        );

        if (string.IsNullOrEmpty(exists) is false) return SignUpBadOutcome.Duplicate;

        var entity = new AppUser
        {
            Id = new AppUserId { Data = _guidProvider.SortableGuid() },
            FullName = dto.FullName,
            UserName = dto.UserName,
            PasswordHash = await _authCryptographyService.HashPasswordAsync(dto.Password),
            CreatedAtUtc = _dateTimeProvider.CurrentUtcTime,
            ConcurrencyToken = _guidProvider.SortableGuid()
        };

        await _appUnitOfWork.AppUserRepository.CreateAsync(entity);
        await _appUnitOfWork.SaveAsync();
        return entity;
    }

    public async Task<ValueOutcome<string, LoginBadOutcome>> LoginAsync(AppUserLoginRequest dto)
    {
        var entity = await _appUnitOfWork.AppUserRepository.GetOneAsync(
            filter: x => x.UserName == dto.UserName,
            subsetSelector: x => new { x.Id, x.PasswordHash, x.IsVerified }
        );

        if (string.IsNullOrEmpty(entity?.PasswordHash)) return LoginBadOutcome.UserNotFound;

        var passwordMatched = await _authCryptographyService.VerifyPasswordAsync(dto.Password, entity.PasswordHash);
        if (passwordMatched is false) return LoginBadOutcome.PasswordNotMatched;

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, entity.Id.Data.ToString()),
            new Claim("IsVerified", entity.IsVerified.ToString())
        ];

        return _jwtProvider.GenerateJwt(claims);
    }

    public async Task<ValueOutcome<AppUser, ProfileBadOutcome>> ReadProfileAsync(AppUserId id)
    {
        var entity = await _appUnitOfWork.AppUserRepository.GetOneAsync(
            filter: x => x.Id == id
        );

        if (entity is null) return ProfileBadOutcome.UserNotFound;
        if (entity.IsBanned) return ProfileBadOutcome.Banned;
        if (entity.IsLockedOut) return ProfileBadOutcome.LockedOut;

        return entity;
    }


    public async Task<ValueOutcome<AppUser, ProfileBadOutcome>> UpdateProfileAsync(AppUserId id,
        AppUserProfileUpdateRequest dto)
    {
        var result = await ReadProfileAsync(id);

        if (result.TryPickGoodOutcome(out var entity, out var err) is false)
        {
            return err;
        }

        await dto.BuildAdapter().AdaptToAsync(entity);

        await _appUnitOfWork.AppUserRepository.UpdateAsync(entity);
        await _appUnitOfWork.SaveAsync();

        return entity;
    }
}