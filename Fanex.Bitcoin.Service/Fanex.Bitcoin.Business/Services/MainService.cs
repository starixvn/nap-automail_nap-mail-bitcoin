using System;
using Fanex.Bitcoin.Business.Models;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.Bitcoin.Business.Services
{
    public interface MainService
    {
        JobMessage Execute(bool isDaily);
    }

    public class MainServiceImp : MainService
    {
        private readonly Log log;
        private readonly DataService dataService;
        private readonly ExcelProfilerService excelService;
        private readonly DateServiceProvider dateService;
        private readonly MailProviderService mailService;

        public MainServiceImp(
            Log log,
            DateServiceProvider dateService,
            DataService dataService,
            ExcelProfilerService excelService,
            MailProviderService mailService)
        {
            this.log = log;
            this.dateService = dateService;
            this.dataService = dataService;
            this.excelService = excelService;
            this.mailService = mailService;
        }

        public JobMessage Execute(bool isDaily)
        {
            var result = new ExecuteResultModel();
            JobMessage jobMessage = new JobMessage();
            try
            {
                result = SendNotification(isDaily);
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                log.Log(e);
            }
            finally
            {
                jobMessage.Mail = result.Mail;
                jobMessage.JobStatus = result.Success ? JobStatus.Success : JobStatus.DataIsNotReady;
                jobMessage.TaskLog = new TaskLog
                {
                    LogTime = DateTime.Now,
                    Message = result.Message
                };
            }

            return jobMessage;
        }

        private ExecuteResultModel SendNotification(bool isDaily)
        {
            var date = dateService.Date;
            var data = isDaily
                ? dataService.Query(date.Yesterday, date.Yesterday)
                : dataService.Query(date.FromDate, date.ToDate);

            var fileName = excelService.Execute(isDaily).Export(data);
            var mail = mailService.Execute(isDaily).SenMail(fileName);

            return new ExecuteResultModel
            {
                Success = true,
                Message = string.Empty,
                Mail = mail
            };
        }
    }
}