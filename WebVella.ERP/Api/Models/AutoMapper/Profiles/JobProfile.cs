﻿using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Dynamic;
using System.Linq;
using WebVella.ERP.Jobs;

namespace WebVella.ERP.Api.Models.AutoMapper.Profiles
{
	public class JobProfile : Profile
	{
		protected override void Configure()
		{
			Mapper.CreateMap<DataRow, Job>().ConvertUsing(source => JobConvert(source));
			Mapper.CreateMap<DataRow, SchedulePlan>().ConvertUsing(source => SchedulePlanConvert(source));
		}

		private static Job JobConvert(DataRow src)
		{
			if (src == null)
				return null;

			Job job = new Job();

			job.Id = (Guid)src["id"];
			job.TypeId = (Guid)src["type_id"];
			job.Type = JobManager.JobTypes.FirstOrDefault(t => t.Id == job.TypeId);
			job.TypeName = (string)src["type_name"];
			job.Assembly = (string)src["assembly"];
			job.CompleteClassName = (string)src["complete_class_name"];
			job.MethodName = (string)src["method_name"];
			if (!string.IsNullOrWhiteSpace((string)src["attributes"]))
				job.Attributes = JsonConvert.DeserializeObject<ExpandoObject>((string)src["attributes"]);
			job.Status = (JobStatus)(int)src["status"];
			job.Priority = (JobPriority)(int)src["priority"];
			if (src["started_on"] != DBNull.Value)
				job.StartedOn = (DateTime?)src["started_on"];
			if (src["finished_on"] != DBNull.Value)
				job.FinishedOn = (DateTime?)src["finished_on"];
			if (src["aborted_by"] != DBNull.Value)
				job.AbortedBy = (Guid?)src["aborted_by"];
			if (src["canceled_by"] != DBNull.Value)
				job.CanceledBy = (Guid?)src["canceled_by"];
			if (src["error_message"] != DBNull.Value)
				job.ErrorMessage = (string)src["error_message"];
			job.CreatedOn = (DateTime)src["created_on"];
			if (src["created_by"] != DBNull.Value)
				job.CreatedBy = (Guid?)src["created_by"];

			return job;
		}

		private static SchedulePlan SchedulePlanConvert(DataRow src)
		{
			if (src == null)
				return null;

			SchedulePlan schedulePlan = new SchedulePlan();

			schedulePlan.Id = (Guid)src["id"];
			schedulePlan.Name = (string)src["name"];
			schedulePlan.Type = (SchedulePlanType)src["type"];
			if (src["start_date"] != DBNull.Value)
				schedulePlan.StartDate = (DateTime)src["start_date"];
			if (src["end_date"] != DBNull.Value)
				schedulePlan.EndDate = (DateTime)src["end_date"];
			schedulePlan.ScheduledDays = JsonConvert.DeserializeObject<SchedulePlanDaysOfWeek>((string)src["schedule_days"]);
			if (src["interval_in_minutes"] != DBNull.Value)
				schedulePlan.IntervalInMinutes = (int)src["interval_in_minutes"];
			if (src["start_timespan"] != DBNull.Value)
				schedulePlan.StartTimespan = (int)src["start_timespan"];
			if (src["end_timespan"] != DBNull.Value)
				schedulePlan.EndTimespan = (int)src["end_timespan"];
			if (src["last_trigger_time"] != DBNull.Value)
				schedulePlan.LastTriggerTime = (DateTime)src["last_trigger_time"];
			if (src["next_trigger_time"] != DBNull.Value)
				schedulePlan.NextTriggerTime = (DateTime)src["next_trigger_time"];
			schedulePlan.JobTypeId = (Guid)src["job_type_id"];
			if (JobManager.JobTypes.Any(t => t.Id == schedulePlan.JobTypeId))
				schedulePlan.JobType = JobManager.JobTypes.FirstOrDefault(t => t.Id == schedulePlan.JobTypeId);
			schedulePlan.JobAttributes = JsonConvert.DeserializeObject<ExpandoObject>((string)src["job_attributes"]);
			schedulePlan.Enabled = (bool)src["enabled"];
			if (src["last_started_job_id"] != DBNull.Value)
				schedulePlan.LastStartedJobId = (Guid)src["last_started_job_id"];
			schedulePlan.CreatedOn = (DateTime)src["created_on"];
			if (src["last_modified_by"] != DBNull.Value)
				schedulePlan.LastModifiedBy = (Guid)src["last_modified_by"];
			schedulePlan.LastModifiedOn = (DateTime)src["last_modified_on"];

			return schedulePlan;
		}
	}
}
