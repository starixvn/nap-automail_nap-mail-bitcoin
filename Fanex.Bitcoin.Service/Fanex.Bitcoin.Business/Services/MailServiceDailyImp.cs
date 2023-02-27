using Fanex.Bitcoin.Business.Models;
using Fanex.MailDelivery.Core.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanex.Bitcoin.Business.Services
{
    public class MailServiceDailyImp : MailService
    {
        private readonly DateServiceProvider dateService;

        public MailServiceDailyImp(DateServiceProvider dateService)
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
                Subject = $"[Bitcoin] - Daily Report {dateService.Date.Yesterday:MM/dd/yyyy}",
                Body = ContentDaily(fileName),
                Attachments = attachments
            };

            return mail;
        }

        private string ContentDaily(string fileName)
        {
            var mailModel = new MailModel
            {
                FileName = new FileInfo(fileName).Name,
                Yesterday = dateService.Date.Yesterday,
                FromDate = dateService.Date.FromDate,
                ToDate = dateService.Date.ToDate
            };
            var tuple = new Tuple<MailModel>(mailModel);
            var template = File.ReadAllText("Templates/Template-Daily.cshtml");
            var result = Engine.Razor.RunCompile(template, $"Template-Bitcoin-Daily", model: tuple);

            return result;
        }
    }
}