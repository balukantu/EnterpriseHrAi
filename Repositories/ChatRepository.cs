using HrAi.Api.Data;
using HrAi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HrAi.Api.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly HrAiDbContext _context;

    public ChatRepository(HrAiDbContext context)
    {
        _context = context;
    }

    public async Task<ChatSession> GetOrCreateSessionAsync(Guid? chatSessionId, int employeeId)
    {
        if (chatSessionId.HasValue)
        {
            var existingSession = await _context.ChatSessions
                .FirstOrDefaultAsync(x =>
                    x.ChatSessionId == chatSessionId.Value &&
                    x.EmployeeId == employeeId);

            if (existingSession != null)
                return existingSession;
        }

        var newSession = new ChatSession
        {
            ChatSessionId = Guid.NewGuid(),
            EmployeeId = employeeId,
            Title = "HR AI Chat",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.ChatSessions.AddAsync(newSession);

        // Important: save immediately so ChatSession exists before adding messages
        await _context.SaveChangesAsync();

        return newSession;
    }

    public async Task<List<ChatMessage>> GetRecentMessagesAsync(Guid chatSessionId, int count)
    {
        return await _context.ChatMessages
            .Where(x => x.ChatSessionId == chatSessionId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(count)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task AddMessageAsync(Guid chatSessionId, string role, string content)
    {
        var session = await _context.ChatSessions
            .FirstOrDefaultAsync(x => x.ChatSessionId == chatSessionId);

        if (session == null)
            throw new InvalidOperationException($"Chat session not found: {chatSessionId}");

        session.UpdatedAt = DateTime.UtcNow;

        await _context.ChatMessages.AddAsync(new ChatMessage
        {
            ChatSessionId = chatSessionId,
            Role = role,
            Content = content,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }
}