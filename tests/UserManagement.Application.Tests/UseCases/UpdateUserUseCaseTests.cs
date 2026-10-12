using UserManagement.Application.DTOs.Users;
using UserManagement.Application.Tests.Fakes;
using UserManagement.Application.UseCases;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Exceptions;

namespace UserManagement.Application.Tests.UseCases;

public class UpdateUserUseCaseTests
{
    private readonly FakeUserRepository _repository;
    private readonly UpdateUserUseCase _useCase;

    public UpdateUserUseCaseTests()
    {
        _repository = new FakeUserRepository(
            new User(1, "Admin", "admin@demo.com", "hash", isActive: true),
            new User(2, "Consulta", "consulta@demo.com", "hash", isActive: true));
        _useCase = new UpdateUserUseCase(_repository);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidData_UpdatesAndReturnsUser()
    {
        var request = new UpdateUserRequest("Nuevo Nombre", "Nuevo@Demo.com", false);

        var result = await _useCase.ExecuteAsync(2, request, CancellationToken.None);

        Assert.Equal(new UserDto(2, "Nuevo Nombre", "nuevo@demo.com", false), result);
        Assert.Equal(1, _repository.UpdateCalls);
    }

    [Fact]
    public async Task ExecuteAsync_KeepingOwnEmail_DoesNotThrowConflict()
    {
        var request = new UpdateUserRequest("Consulta Editado", "consulta@demo.com", true);

        var result = await _useCase.ExecuteAsync(2, request, CancellationToken.None);

        Assert.Equal("Consulta Editado", result.Name);
        Assert.Equal(1, _repository.UpdateCalls);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserDoesNotExist_ThrowsUserNotFound()
    {
        var request = new UpdateUserRequest("Nombre", "nuevo@demo.com", true);

        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _useCase.ExecuteAsync(99, request, CancellationToken.None));

        Assert.Equal(0, _repository.UpdateCalls);
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmailBelongsToAnotherUser_ThrowsEmailAlreadyInUse()
    {
        var request = new UpdateUserRequest("Consulta", "admin@demo.com", true);

        await Assert.ThrowsAsync<EmailAlreadyInUseException>(
            () => _useCase.ExecuteAsync(2, request, CancellationToken.None));

        Assert.Equal(0, _repository.UpdateCalls);
    }

    [Theory]
    [InlineData("", "valido@demo.com")]
    [InlineData("Nombre", "no-es-un-email")]
    public async Task ExecuteAsync_WithInvalidData_ThrowsValidationAndKeepsUserUnchanged(string name, string email)
    {
        var request = new UpdateUserRequest(name, email, false);

        await Assert.ThrowsAsync<DomainValidationException>(
            () => _useCase.ExecuteAsync(2, request, CancellationToken.None));

        var user = await _repository.GetByIdAsync(2, CancellationToken.None);
        Assert.Equal("Consulta", user!.Name);
        Assert.Equal("consulta@demo.com", user.Email);
        Assert.True(user.IsActive);
        Assert.Equal(0, _repository.UpdateCalls);
    }
}
