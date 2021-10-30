using System;
using System.ComponentModel.DataAnnotations;

namespace AwsCs.Services.Api.Dto.Request
{
    public class FirstMessaegDto
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public decimal Value { get; set; }
    }

    public class SecondMessageDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public long Cpf { get; set; }
        [Required]
        public DateTime Birth { get; set; }

    }
}
