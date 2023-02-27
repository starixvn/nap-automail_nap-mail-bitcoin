using Fanex.Bitcoin.Business.Models;
using Fanex.MailDelivery.Core.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanex.Bitcoin.Business.Services
{
    public class MailServiceMonthlyImp : MailService
    {
        private readonly DateServiceProvider dateService;

        public MailServiceMonthlyImp(DateServiceProvider dateService)
        {
            this.dateService = dateService;
        }

        public Mail SenMail(string fileName)
        {
            var attachments = new List<MailAttachment>
            {
                new MailAttachment(fileName, FileExtension.MS_EXCEL)
            };

            var mail = new Mail
            {
                Title = "ONEworks Auto-Generated Report Administrator",
                Subject = $"[Bitcoin] - Monthly Report {dateService.Date.FromDate:MM/dd/yyyy} - {dateService.Date.ToDate:MM/dd/yyyy}",
                Body = ContentMonthly(fileName),
                Attachments = attachments
            };

            return mail;
        }

        private string ContentMonthly(string fileName)
        {
            var mailModel = new MailModel
            {
                FileName = new FileInfo(fileName).Name,
                Yesterday = dateService.Date.Yesterday,
                FromDate = dateService.Date.FromDate,
                ToDate = dateService.Date.ToDate
            };
            var tuple = new Tuple<MailModel>(mailModel);
            var template = File.ReadAllText("Templates/Template-Monthly.cshtml");
            var result = Engine.Razor.RunCompile(template, $"Ttemplate-Bitcoin-Monthly", model: tuple);

            return result;
        }
    }
}