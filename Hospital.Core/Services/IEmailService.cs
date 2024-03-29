﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Core.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail);
        Task SendTakeAppointmentEmail(string toEmail);
        public Task SendConfirmCodeEmail(int ConfirmCode, string ToEmail);

    }
}
