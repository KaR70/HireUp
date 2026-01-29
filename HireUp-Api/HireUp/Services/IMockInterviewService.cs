using HireUp.DTOs.Authentication;
namespace HireUp.Services;

    public interface IMockInterviewService
    {
        Task<IEnumerable<MockInterviewResponseDto>> GetAllAsync();
        Task<MockInterviewResponseDto> GetByIdAsync(int id);
        Task<MockInterviewResponseDto> CreateAsync(MockInterviewDto dto);
        Task<bool> UpdateAsync(int id, MockInterviewDto dto);
        Task<bool> DeleteAsync(int id);
    }

