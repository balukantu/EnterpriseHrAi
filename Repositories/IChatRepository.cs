using HrAi.Api.Models;

namespace HrAi.Api.Repositories;

public interface IChatRepository
{
    Task<ChatSession> GetOrCreateSessionAsync(Guid? chatSessionId, int employeeId);
    Task<List<ChatMessage>> GetRecentMessagesAsync(Guid chatSessionId, int count);
    Task AddMessageAsync(Guid chatSessionId, string role, string content);
}