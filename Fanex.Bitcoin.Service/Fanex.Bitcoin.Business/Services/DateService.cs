using System;

namespace Fanex.Bitcoin.Business.Services
{
    public interface DateService
    {
        DateTime Yesterday { get; }

        DateTime FromDate { get; }

        DateTime ToDate { get; }
    }
}