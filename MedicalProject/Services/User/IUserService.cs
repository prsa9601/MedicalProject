using MedicalProject.Models;
using MedicalProject.Models.User;
using MedicalProject.Models.User.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MedicalProject.Services.User
{
    public interface IUserService
    {
        Task<ApiResult> Create(CreateUserCommand command);
        Task<ApiResult> SetImage(SetImageUserCommand command);
        Task<ApiResult> ConfirmedAccount(ConfirmedAccountUserCommand command);
        Task<ApiResult> CompletionOfInformation(CompletionOfInformationCommand command);


        Task<UserDto?> GetUserById(Guid userId);
        Task<UserFilterResult> GetUserByFilter(UserFilterParam param);
    }
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public Task<ApiResult> CompletionOfInformation(CompletionOfInformationCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult> ConfirmedAccount(ConfirmedAccountUserCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult> Create(CreateUserCommand command)
        {
            var result = await _client.PostAsJsonAsync("User/CreateUser", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> SetImage(SetImageUserCommand command)
        {
            var result = await _client.PatchAsJsonAsync("User/CreateUser", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<UserFilterResult> GetUserByFilter(UserFilterParam param)
        {
            string url = $"User/GetUserByFilter?take={param.Take}&pageId={param.PageId}";
            if (param.PhoneNumber is not null)
                url += $"&phoneNumber={param.PhoneNumber}";
            if (param.IsActive is not null)
                url += $"&isActive={param.IsActive}";

            var result = await _client.GetFromJsonAsync<ApiResult<UserFilterResult>>(url);
            return result?.Data;
        }

        public async Task<UserDto?> GetUserById(Guid userId)
        {

            var result = await _client.GetFromJsonAsync<ApiResult<UserDto?>>($"User/GetUserById?userId={userId}");
            return result?.Data;
        }


    }
}
