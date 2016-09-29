﻿

using System;
using DSLNG.PEAR.Services.Interfaces;
using DSLNG.PEAR.Services.Responses;
using DSLNG.PEAR.Data.Persistence;
using System.Linq;
using System.Data.Entity;
using DSLNG.PEAR.Data.Entities;
using DSLNG.PEAR.Data.Entities.Der;
using DSLNG.PEAR.Services.Requests.DerLoadingSchedule;
using DSLNG.PEAR.Services.Responses.DerLoadingSchedule;

namespace DSLNG.PEAR.Services
{
    public class DerLoadingScheduleService : BaseService, IDerLoadingScheduleService
    {
        public DerLoadingScheduleService(IDataContext dataContext) : base(dataContext) { }

        public GetDerLoadingSchedulesResponse Get(GetDerLoadingSchedulesRequest request)
        {
            var response = new GetDerLoadingSchedulesResponse();
            var prevPeriod = request.Periode.Value.AddDays(-1);
            var derLoadingScheduleQuery = DataContext.DerLoadingSchedules.Include(x => x.VesselSchedules);
            if (request.StrictDate) {
                derLoadingScheduleQuery = derLoadingScheduleQuery.Where(x => x.Period == request.Periode);
            } else {
                derLoadingScheduleQuery = derLoadingScheduleQuery.Where(x => x.Period == request.Periode || x.Period == prevPeriod);
            }
            var derLoadingSchedule = derLoadingScheduleQuery.FirstOrDefault();
            if (derLoadingSchedule == null)
            {
                return response;
            }
            if (derLoadingSchedule.Period == request.Periode) response.ExistValueTime = "now";
            else response.ExistValueTime = "prev";
            var ids = derLoadingSchedule.VesselSchedules.Select(x => x.Id).ToArray();
            var query = DataContext.VesselSchedules
            .Include(x => x.Buyer)
            .Include(x => x.Vessel)
            .Include(x => x.Vessel.Measurement)
            .Select(x => new
            {
                id = x.Id,
                NextLoadingSchedules = x.NextLoadingSchedules.Where(y => y.CreatedAt <= request.Periode).OrderByDescending(y => y.CreatedAt).Take(1).ToList(),
                Buyer = x.Buyer,
                Vessel = x.Vessel,
                ETA = x.ETA,
                ETD = x.ETD,
                Location = x.Location,
                SalesType = x.SalesType,
                Type = x.Type,
                VesselType = x.Vessel.Type,
                IsActive = x.IsActive,
                Cargo = x.Cargo,
                Measurement = x.Vessel.Measurement.Name,
                Capacity = x.Vessel.Capacity
            });
            response.VesselSchedules = query.Where(x => ids.Contains(x.id)).OrderByDescending(x => x.ETD).Select(
                    x => new GetDerLoadingSchedulesResponse.VesselScheduleResponse
                    {
                        id = x.id,
                        Remark = x.NextLoadingSchedules.Count == 1 ? x.NextLoadingSchedules.FirstOrDefault().Remark : null,
                        RemarkDate = x.NextLoadingSchedules.Count == 1 ? x.NextLoadingSchedules.FirstOrDefault().CreatedAt : (DateTime?)null,
                        Buyer = x.Buyer.Name,
                        Vessel = x.Vessel.Name,
                        ETA = x.ETA,
                        ETD = x.ETD,
                        Location = x.Location,
                        SalesType = x.SalesType,
                        Type = x.Type,
                        IsActive = x.IsActive,
                        Cargo = x.Cargo,
                        VesselType = x.VesselType,
                        Measurement = x.Measurement,
                        Capacity = x.Capacity
                    }
                ).ToList();
            return response;
        }

        public BaseResponse SaveSchedules(int[] ids, DateTime date)
        {
            try
            {
                var derLoadingSchedule = DataContext.DerLoadingSchedules.Include(x => x.VesselSchedules).FirstOrDefault(x => x.Period == date);
                if (derLoadingSchedule != null)
                {
                    foreach (var schedule in derLoadingSchedule.VesselSchedules.ToList())
                    {
                        derLoadingSchedule.VesselSchedules.Remove(schedule);
                    }
                }
                else
                {
                    derLoadingSchedule = new DerLoadingSchedule { Period = date };
                }

                foreach (var id in ids)
                {
                    var schedule = DataContext.VesselSchedules.Local.FirstOrDefault(x => x.Id == id);
                    if (schedule == null)
                    {
                        schedule = new VesselSchedule { Id = id };
                        DataContext.VesselSchedules.Attach(schedule);
                    }
                    derLoadingSchedule.VesselSchedules.Add(schedule);
                }
                DataContext.DerLoadingSchedules.Add(derLoadingSchedule);
                DataContext.SaveChanges();
                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "You have been successfully save choosen schedules"
                };
            }
            catch (Exception e)
            {
                return new BaseResponse { Message = e.Message };
            }
        }
    }
}
