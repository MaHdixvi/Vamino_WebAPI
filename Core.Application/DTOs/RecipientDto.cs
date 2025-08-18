using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOs
{
    /// <summary>
    /// DTO for recipient information.
    /// </summary>
    public class RecipientDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
