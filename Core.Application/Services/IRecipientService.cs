using Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Services
{
    /// <summary>
    /// Service for managing recipients (users who receive notifications, payments, etc.)
    /// </summary>
    public interface IRecipientService
    {
        /// <summary>
        /// Get recipient details by ID.
        /// </summary>
        Task<RecipientDto> GetRecipientByIdAsync(string recipientId);

        /// <summary>
        /// Get all recipients.
        /// </summary>
        Task<IEnumerable<RecipientDto>> GetAllRecipientsAsync();

        /// <summary>
        /// Add a new recipient.
        /// </summary>
        Task AddRecipientAsync(RecipientDto recipient);

        /// <summary>
        /// Update recipient information.
        /// </summary>
        Task UpdateRecipientAsync(RecipientDto recipient);

        /// <summary>
        /// Delete a recipient by ID.
        /// </summary>
        Task DeleteRecipientAsync(string recipientId);
        Task<bool> ValidateRecipientAsync(string recipientAccount);
    }
}
