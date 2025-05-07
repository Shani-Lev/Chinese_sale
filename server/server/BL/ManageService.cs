using AutoMapper;
using Mono.TextTemplating;
using server.BL.Interface;
using server.DAL.Interface;
using server.Models;
using server.Models.DTO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;

namespace server.BL
{
    public class ManageService : IManageService
    {
        private IMannagerDal mannagerDal;
        private ITicketDalMannager ticketDalMannager;
        private IStatusDal statusDal;
        private IMapper mMapper;
        private ILogger<ManageService> logger;

        public ManageService(IMannagerDal mannagerDal, IMapper mMapper, IStatusDal statusDal, ILogger<ManageService> logger, ITicketDalMannager ticketDalMannager)
        {
            this.mannagerDal = mannagerDal;
            this.mMapper = mMapper;
            this.statusDal = statusDal;
            this.logger = logger;
            this.ticketDalMannager = ticketDalMannager;
        }

        async public Task<List<TicketDTOm_After>> GetWinners()
        {
            try
            {
                return await mannagerDal.GetWinners();
            }
            catch (Exception ex)
            {
                logger.LogError("error on getting winners: " + ex);
                throw new Exception("An error on getting winners", ex);
            }

        }

        public async Task<UserDTOResualt> SetLottery(int GiftId)
        {
            try
            {
                return await mannagerDal.SetLottery(GiftId);
            }
            catch (Exception ex)
            {
                logger.LogError("error on setting lottory: " + ex);
                throw new Exception("An error on setting lottory", ex);
            }

        }

        public async Task SetLottery()
        {
            try
            {
                await mannagerDal.SetLottery();
            }
            catch (Exception ex)
            {
                logger.LogError("error on setting lottory: " + ex);
                throw new Exception("An error on setting lottory", ex);
            }

        }

        async public Task<List<RevenueReport>> GetRevenue()
        {
            try
            {
                var tickets = await mannagerDal.GetRevenue();

                var revenues = tickets
                    .GroupBy(t => t.Gift)
                    .Select(g => new RevenueReport
                    {
                        Gift = mMapper.Map<GiftDTOResualt>(g.Key),
                        Sales = g.Count(),
                        Sum = g.Count() * g.First().Gift.Price,
                        WinnerId = g.Key.UserWinnerId
                    }).ToList();

                int totalSum = 0;
                int totalSales = 0;
                foreach (var revenue in revenues)
                {
                    totalSum += revenue.Sum;
                    totalSales += revenue.Sales;
                }

                revenues.Add(new RevenueReport() { Gift = null, Sum = totalSum, Sales = totalSales });


                return revenues;
            }
            catch (Exception ex)
            {
                logger.LogError("error on setting lottory: ");
                throw new Exception("An error on setting lottory");
            }
        }

        public async Task<Status> GetStatus()
        {
            try
            {
                return await statusDal.Get();
            }
            catch (Exception ex)
            {
                throw new Exception("An error on getting status", ex);
            }
        }

        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public async Task SetStatus(DateTime nextTime, int type)
        {
            await semaphore.WaitAsync();
            try
            {
                logger.LogDebug("set statuse function");

                //if (DateTime.Compare(nextTime, DateTime.Now)< 0)
                //{
                //    //nextTime = ConvertHebrewDateToGregorian(nextTime.ToString()); 
                //    if (DateTime.Compare(nextTime, DateTime.Now) < 0)
                //    {
                //        logger.LogError("not valid next time: " + nextTime);
                //        throw new ArgumentException("the next time is not valid");
                //    }
                //}

                Status status = await statusDal.Get();
                SystemStatus currentStatus = (SystemStatus)Enum.Parse(typeof(SystemStatus), status.Text);

                var values = (SystemStatus[])Enum.GetValues(typeof(SystemStatus));

                int num;

                if (type == 0)
                {
                    num = ((Array.IndexOf(values, currentStatus) + 1) % 3);
                }
                else
                {
                    num = ((Array.IndexOf(values, currentStatus)));
                }

                SystemStatus nextStatuse = values[num];

                await statusDal.Set(nextStatuse, nextTime);
                logger.LogInformation("setting statuse succesfuly");
                if (num == 2)
                {
                    try
                    {
                        await SetLottery();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("error on setting lottory: " + ex);
                        throw new Exception("An error on setting lottory", ex);
                    }
                }
                if (num == 0)
                {
                    try
                    {
                        await ticketDalMannager.RemoveAll();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("error on setting lottory: " + ex);
                        throw new Exception("An error on setting lottory", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error on setting status: " + ex);
                throw new Exception("An error on setting status", ex);
            }
            finally
            {
                semaphore.Release();
            }
        }

    }
}
