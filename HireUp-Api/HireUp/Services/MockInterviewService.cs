using HireUp.Database;
using HireUp.DTOs.Authentication;
using Microsoft.EntityFrameworkCore;

using HireUp.Entities;


namespace HireUp.Services;

public class MockInterviewService : IMockInterviewService
    {
        private readonly ApplicationDbContext _context;

        public MockInterviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MockInterviewResponseDto>> GetAllAsync()
        {
            return await _context.MockInterviews
                .Select(i => new MockInterviewResponseDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    
                    CreatedAt = i.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<MockInterviewResponseDto> GetByIdAsync(int id)
        {
            var interview = await _context.MockInterviews.FindAsync(id);
            if (interview == null) return null;

            return new MockInterviewResponseDto
            {
                Id = interview.Id,
                Title = interview.Title,
               
                CreatedAt = interview.CreatedAt
            };
        }

        public async Task<MockInterviewResponseDto> CreateAsync(MockInterviewDto dto)
        {
            var interview = new MockInterview
            {
                Title = dto.Title,
               
            };

            _context.MockInterviews.Add(interview);
            await _context.SaveChangesAsync();

            return new MockInterviewResponseDto
            {
                Id = interview.Id,
                Title = interview.Title,
               
                CreatedAt = interview.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, MockInterviewDto dto)
        {
            var interview = await _context.MockInterviews.FindAsync(id);
            if (interview == null) return false;

            interview.Title = dto.Title;
           

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var interview = await _context.MockInterviews.FindAsync(id);
            if (interview == null) return false;

            _context.MockInterviews.Remove(interview);
            await _context.SaveChangesAsync();
            return true;
        }
    }

