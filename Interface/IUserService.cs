
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(UserDto userDto); // Use UserDto here
        Task<bool> UpdateUserAsync(int id, UserDto userDto); // Optional: Add DTO for updates
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int id);
    }
