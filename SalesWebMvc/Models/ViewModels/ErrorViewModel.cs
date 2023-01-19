using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; } = default!;
        public string Message { get; set; } = default!;

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}