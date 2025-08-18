using Core.Application.Contracts;
using Core.Application.DTOs;
using Core.Application.Services;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Application.Implementations
{
    public class RecipientService : IRecipientService
    {
        private readonly IRecipientRepository _recipientRepository;

        public RecipientService(IRecipientRepository recipientRepository)
        {
            _recipientRepository = recipientRepository;
        }

        public async Task AddRecipientAsync(RecipientDto recipientDto)
        {
            if (recipientDto == null)
                throw new ArgumentNullException(nameof(recipientDto));

            var entity = new Recipient
            {
                Id = recipientDto.Id ?? Guid.NewGuid().ToString(),
                Name = recipientDto.Name,
                Mobile = recipientDto.Mobile,
                Email = recipientDto.Email
            };

            await _recipientRepository.AddAsync(entity);
        }

        public async Task DeleteRecipientAsync(string recipientId)
        {
            await _recipientRepository.DeleteAsync(recipientId);
        }

        public async Task<IEnumerable<RecipientDto>> GetAllRecipientsAsync()
        {
            var recipients = await _recipientRepository.GetAllAsync();

            return recipients.Select(r => new RecipientDto
            {
                Id = r.Id,
                Name = r.Name,
                Mobile = r.Mobile,
                Email = r.Email
            });
        }

        public async Task<RecipientDto> GetRecipientByIdAsync(string recipientId)
        {
            var recipient = await _recipientRepository.GetByIdAsync(recipientId);
            if (recipient == null)
                return null;

            return new RecipientDto
            {
                Id = recipient.Id,
                Name = recipient.Name,
                Mobile = recipient.Mobile,
                Email = recipient.Email
            };
        }

        public async Task UpdateRecipientAsync(RecipientDto recipientDto)
        {
            if (recipientDto == null)
                throw new ArgumentNullException(nameof(recipientDto));

            var entity = await _recipientRepository.GetByIdAsync(recipientDto.Id);
            if (entity != null)
            {
                entity.Name = recipientDto.Name;
                entity.Mobile = recipientDto.Mobile;
                entity.Email = recipientDto.Email;

                await _recipientRepository.UpdateAsync(entity);
            }
        }

        public async Task<bool> ValidateRecipientAsync(string recipientAccount)
        {
            if (string.IsNullOrWhiteSpace(recipientAccount))
                return false;

            // جستجو بر اساس موبایل یا ایمیل
            var recipients = await _recipientRepository.GetAllAsync();

            var exists = recipients.Any(r =>
                r.Mobile == recipientAccount ||
                r.Email == recipientAccount);

            return exists;
        }
    }
}
