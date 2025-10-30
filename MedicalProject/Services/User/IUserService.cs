using MedicalProject.Models;
using MedicalProject.Models.User;
using MedicalProject.Models.User.DTOs;

namespace MedicalProject.Services.User
{
    public interface IUserService
    {
        Task<ApiResult> Create(CreateUserCommand command);
        Task<ApiResult> Edit(EditUserCommand command);
        Task<ApiResult<string>> Remove(Guid userId);
        Task<ApiResult> ChangePassword(ChangePasswordCommand command);
        Task<ApiResult<Guid>> CreateForAdmin(CreateUserForAdminCommand command);
        Task<ApiResult> SetImage(SetImageUserCommand command);
        Task<ApiResult> ConfirmedAccount(ConfirmedAccountUserCommand command);
        Task<ApiResult> ChangeActivityAccount(ChangeActivityUserAccountCommand command);
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

        public async Task<ApiResult> CompletionOfInformation(CompletionOfInformationCommand command)
        {
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(command.userId.ToString()), "userId");
            form.Add(new StringContent(command.nationalityCode.ToString()), "nationalityCode");
            form.Add(
                    new StreamContent(command.nationalCardPhoto.OpenReadStream()),
                    "nationalCardPhoto",
                    command.nationalCardPhoto.FileName
                );
            form.Add(
                    new StreamContent(command.birthCertificatePhoto.OpenReadStream()),
                    "birthCertificatePhoto",
                    command.birthCertificatePhoto.FileName
                );


            var result = await _client.PostAsync("User/CompletionOfInformation", form);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }
    

        public async Task<ApiResult> ConfirmedAccount(ConfirmedAccountUserCommand command)
        {
            var result = await _client.PostAsJsonAsync("User/ConfirmedAccount", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> Create(CreateUserCommand command)
        {
            var result = await _client.PostAsJsonAsync("User/CreateUser", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> SetImage(SetImageUserCommand command)
        {
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(command.userId.ToString()), "userId");
            form.Add(
                    new StreamContent(command.userAccountImage.OpenReadStream()),
                    "userAccountImage",
                    command.userAccountImage.FileName
                );


            var result = await _client.PatchAsync("User/SetImageUser", form);
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

        public async Task<ApiResult<Guid>> CreateForAdmin(CreateUserForAdminCommand command)
        {
            var result = await _client.PostAsJsonAsync("User/CreateUserForAdmin", command);
            return await result.Content.ReadFromJsonAsync<ApiResult<Guid>>();
        }

        public async Task<ApiResult> ChangeActivityAccount(ChangeActivityUserAccountCommand command)
        {
            var result = await _client.PatchAsJsonAsync("User/ChangeActivityAccount", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult> Edit(EditUserCommand command)
        {
            var result = await _client.PatchAsJsonAsync("User/EditUser", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }

        public async Task<ApiResult<string>> Remove(Guid userId)
        {
            var result = await _client.DeleteAsync($"User/RemoveUser?userId={userId}");
            return await result.Content.ReadFromJsonAsync<ApiResult<string>>();
        }

        public async Task<ApiResult> ChangePassword(ChangePasswordCommand command)
        {
            var result = await _client.PostAsJsonAsync("User/ChangePassword", command);
            return await result.Content.ReadFromJsonAsync<ApiResult>();
        }
    }
}
