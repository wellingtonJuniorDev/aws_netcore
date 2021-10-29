using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AwsCs.Services.Api.Dto.Request
{
    public class SendMailPost
    {
        /// <summary>
        /// Who ever sent the email. IMPORTANT: Must be registered on SES
        /// </summary>
        [Required]
        public string Sender { get; set; }
        /// <summary>
        /// The subject of the email
        /// </summary>
        [Required]
        public string Subject { get; set; }
        /// <summary>
        /// The main recipients
        /// </summary>
        public List<string> ReceiversAddress { get; set; }
        /// <summary>
        /// The recipients in copy
        /// </summary>
        public List<string> ReceiversInCopy { get; set; }
        /// <summary>
        /// The recipients in hidden copy
        /// </summary>
        public List<string> ReceiversInHiddenCopy { get; set; }
        /// <summary>
        /// The message content
        /// </summary>
        [Required]
        public string BodyContent { get; set; }
        /// <summary>
        /// Indicates that the content is in HTML format
        /// </summary>
        public bool IsHtml { get; set; }
    }
}
